using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace GameExtensions 
{
    /// <summary>
    /// Class representing the player.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public sealed class Player : Entity, ISaveable
    {
        // this is for things unique to the player (controls, spells, etc.)
        /// <summary>
        /// The main <see cref="Player"></see> Instance.
        /// </summary>
        public static Player Instance { get; private set; }

        //public references to some objects in the scene
        /// <summary>
        /// The player's <see cref="PlayerInput"/> component.
        /// </summary>
        [field:NonSerialized]public PlayerInput PInput { get; private set; }

        /// <summary>
        /// The "level up!" text object.
        /// </summary>
        [FormerlySerializedAs("LevelUpText")] [SerializeField]
        private TextMeshProUGUI levelUpText;

        //setting Entity properties, for more info -> see Entity
        protected override int AttackingPmHash => Animator.StringToHash("attacking");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override Vector3 AtkSpherePos => new(0, 1, 0.5f);
        protected override int AtkSphereRadius => 1;


        /// <summary>
        /// Used for checking if the player is moving or not
        /// </summary>
        private bool mozog;

        /// <summary>
        /// Contains joystck input data.
        /// </summary>
        private Vector2 mPos;

        /// <summary>
        /// Used for checking if the player is on ground or not
        /// </summary>
        private bool grounded;

        /// <summary>
        /// Used for checking if the player is running or not
        /// </summary>
        private bool running;

        /// <summary>
        /// id of the moveSpeed parameter, for controlling animation speed from script
        /// </summary>
        private static int MoveSpeedId => Animator.StringToHash("moveSpeed");

        /// <summary>
        /// the player's <see cref="Transform"/>.
        /// </summary>
        [SerializeField][HideInInspector]private Transform tf;

        /// <summary>
        /// the main <see cref="CinemachineVirtualCamera"/> in the scene
        /// </summary>
        private CinemachineVirtualCamera vCam;

        /// <summary>
        /// The player's XP.
        /// </summary>
        [field:SerializeField]public int Xp { get; private set; }

        /// <summary>
        /// The player level.
        /// </summary>
        [field:SerializeField]public byte Lvl { get; private set; }

        /// <summary>
        /// The amount of XP the player needs to level up.
        /// </summary>
        public int XpThreshold { get; private set; } = 10;

        //some constants to make code readable + adjustable
        /// <summary>
        /// The force the player's <see cref="Rigidbody"/> pushes it up to jump. You can change its value to adjust jump height.
        /// </summary>
        private const int JumpForce = 400;

        /// <summary>
        /// The force the player's <see cref="Rigidbody"/> pushes it to dodge. You can change its value to adjust dodge distance.
        /// </summary>
        private const int DodgePower = 23;

        /// <summary>
        /// The amount of seconds the game waits to look at the objective.
        /// </summary>
        private const int LookTimeout = 3;

        /// <summary>
        /// The amount of damping on the camera while walking. You can change its value to adjust how far the camera zooms out while walking.
        /// </summary>
        private const int WalkDamping = 5;

        /// <summary>
        /// The amount of damping on the camera while running. You can change its value to adjust how far the camera zooms out while running.
        /// </summary>
        /// <remarks>It generally should be smaller than <see cref="WalkDamping"/>. </remarks>
        private const int RunDamping = 2;
        
        /// <summary>
        /// Drag of the <see cref="Rigidbody"/> component.
        /// </summary>
        /// <remarks>Don't change this unless things don't move as they should.</remarks>
        private const float Drag = .1f;

        /// <summary>
        /// Angular drag of the <see cref="Rigidbody"/> comonent.
        /// </summary>
        /// <remarks>Don't change this unless things don't move as they should.</remarks>
        private const float AngularDrag = 1;
        /// <summary>
        /// Constraint sum of the <see cref="Rigidbody"/> component. See <see href="https://docs.unity3d.com/ScriptReference/Rigidbody-constraints.html"/> for details.
        /// </summary>
        /// <remarks>Don't change this unless things don't move as they should.</remarks>
        private const int Constraints = 80;

        /// <summary>
        /// The start XP threshold which then gets scaled to the player's <see cref="Lvl"/>.
        /// </summary>
        private const int DefaultThreshold = 10;

        /// <summary>
        /// The multiplier the XP threshold increases with each level. You can change its value to adjust how much more difficult each level gets.
        /// </summary>
        private const float ThresholdMultiplier = 1.2f;

        //input event handlers
        /// <summary>
        /// Gets the player's movement input and saves that data to <see cref="mPos"/>.
        /// </summary>
        /// <param name="context">The input data.</param>
        public void Move(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                mPos = context.ReadValue<Vector2>(); //storing input data
                anim.SetBool(MovingPmHash, true); //playing the move animation
                anim.SetFloat(MoveSpeedId, 1); //setting aniation playback speed to 1
                mozog = true; //telling the code in FixedUpdate() that the player is moving
            }

            //if the player lets go of the stick, stop moving + checks in case of stick drift
            if (!context.canceled) return;
            mozog = false;
            anim.SetBool(MovingPmHash, false);
            mPos = Vector2.zero;
        }

        /// <summary>
        /// Event handler for jumping.
        /// </summary>
        /// <param name="context">The input data.</param>
        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed || !grounded) return;
            rb.AddForce(0, JumpForce, 0); //jump
            //Debug.Log("Jumping, velocity: " + rb.velocity.y); 
        }

        /// <summary>
        /// Event handler for standard (light) attacking.
        /// </summary>
        /// <param name="context"><inheritdoc cref="Jump"/></param>
        public void LightAttack(InputAction.CallbackContext context)
        {
            Attack(); //see Entity.Attack()
            
        }

        /// <summary>
        /// Event handler for second (heavy) attacking.
        /// </summary>
        /// <param name="context"><inheritdoc cref="Jump"/></param>
        public void HeavyAttack(InputAction.CallbackContext context)
        {
            //we don't know what to do with this yet :/
            if (context.performed)
            {
                Debug.Log("Heavy Attack");
            }
        }

        /// <summary>
        /// Event handler for dodging.
        /// </summary>
        /// <param name="context"><inheritdoc cref="Jump"/></param>
        public void Dodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                rb.AddForce(rb.velocity.x + DodgePower, 0,
                    0); //pushing player to the side (idk if we still need this tbh)
            }
        }

        /// <summary>
        /// Event handler for running.
        /// </summary>
        /// <param name="context"><inheritdoc cref="Jump"/></param>
        public void Run(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //tell the code in FixedUpdate() we're running
                running = true;
                anim.SetFloat(MoveSpeedId, 2); //set "running" animation (speeding up walk animation)
                vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = RunDamping;
            }

            if (!context.canceled) return;
            running = false;
            anim.SetFloat(MoveSpeedId, 1);
            vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = WalkDamping;
        }


        /// <summary>
        /// Event handler for looking at the objective.
        /// </summary>
        /// <param name="context"><inheritdoc cref="Jump"/></param>
        public void ShowObjective(InputAction.CallbackContext context)
        {
            //again, not sure if we need this but it's here anyway
            if (!context.performed) return;
            //vCam.LookAt = Objective.transform; it's commented out because there is no objective yet
            Debug.Log("Show Objective");
            Invoke(nameof(DoneLooking), LookTimeout); //after 3 seconds, return to normal camera view
        }

        /// <summary>
        /// Looks back at the player.
        /// </summary>
        /// <remarks>It's used for calling it in Invoke().</remarks>
        private void DoneLooking()
        {
            Debug.Log("Camera looks back at player");
            //vCam.LookAt = tf;
        }

        /// <summary>
        /// Kills the player.
        /// </summary>
        /// <remarks>[overriden from <see cref="Entity.Die"/>]</remarks>
        public override void Die()
        {
            Debug.Log("player died :(");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Stuns the player.
        /// </summary>
        /// <remarks>[overriden from <see cref="Entity.Stun"/>]</remarks>
        public override void Stun()
        {
            base.Stun();
            PInput.DeactivateInput();
        }

        /// <summary>
        /// Cancels the stun effect.
        /// </summary>
        /// <remarks>[overriden from <see cref="Entity.UnStun"/>]</remarks>
        protected override void UnStun()
        {
            PInput.ActivateInput();
            base.UnStun();
        }

        /// <summary>
        /// Adds XP to the player, and levels it up if necesarry.
        /// </summary>
        /// <param name="amount">The amount of XP given to the player.</param>
        public void AddXp(int amount)
        {
            Xp += amount;
            if (Xp < XpThreshold) return;
            Lvl++;
            XpThreshold = Xp + Mathf.RoundToInt(XpThreshold * ThresholdMultiplier);
            levelUpText.SetText(levelUpText.text.Remove(14, levelUpText.text.Length - 15));
            levelUpText.text += Lvl;
            StartCoroutine(levelUpText.gameObject.ActivateFor(3));
        }

        #region InputActionAdder

        /// <summary>
        /// Adds an <see cref="UnityAction"/> to the input action list.
        /// </summary>
        /// <param name="actionName">The name of the <see cref="InputAction"/>.</param>
        /// <param name="action">The action to be invoked.</param>
        /// <param name="type">When you want to listen to the event.</param>
        public void AddInputAction(string actionName, UnityAction action, ActionType type = ActionType.Performed)
        {
            switch (type)
            {
                case ActionType.Started : 
                    void StartedCallback(InputAction.CallbackContext context)
                    {
                        if (context.started) action.Invoke();
                    }
                    PInput.actions[actionName].started += StartedCallback;
                    break;
                case ActionType.Performed:
                    void PerformedCallback(InputAction.CallbackContext context)
                    {
                        if (context.performed) action.Invoke();
                    }
                    PInput.actions[actionName].performed += PerformedCallback;
                    break;
                case ActionType.Canceled: 
                    void CancelledCallback(InputAction.CallbackContext context)
                    {
                        if (context.canceled) action.Invoke();
                    }
                    PInput.actions[actionName].canceled += CancelledCallback;
                    break;
                default: Debug.LogError("bad ActionType found");
                    break;
            }
        }
        /// <summary>
        /// Defines which event the added action will add to.
        /// </summary>
        public enum ActionType
        {
            Started,
            Performed,
            Canceled
        }

        #endregion



        public static event UnityAction PlayerReady;

        private void OnCollisionStay(Collision collision)
        {
            if (collision.collider is TerrainCollider && !grounded)
                grounded = true; //if the player is touching the ground, they can jump
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.collider is TerrainCollider && grounded)
                grounded = false; //as soon as the player leaves the ground, they can't jump
        }

        //FixedUpdate() updates a fixed amount per second (50-ish), useful for physics or control
        private void FixedUpdate()
        {
            //movement logic
            var angle = Mathf.Atan2(mPos.x, mPos.y) * Mathf.Rad2Deg; //getting the angle from stick input
            tf.localEulerAngles += new Vector3(0, angle * Time.fixedDeltaTime, 0); //rotating the player
            if (running) Move(tf.InverseTransformDirection(tf.forward), RunSpeed); //moving the running player forward
            else if (mozog) Move(tf.InverseTransformDirection(tf.forward));
        }

        //Start() runs once when the object is enabled, lots of early game setup goes here
        private void Start()
        {
            if(Instance is not null) Destroy(this);
            PInput = GetComponent<PlayerInput>(); //setting PlayerInput
            //PlayerInput setup inside

            #region PiSetup

            //setting up PlayerInput so I don't have to do it all the time
            PInput.actions["Move"].performed += Move;
            PInput.actions["Move"].canceled += Move;
            PInput.actions["Jump"].performed += Jump;
            PInput.actions["Attack"].performed += LightAttack;
            PInput.actions["Heavy Attack"].performed += HeavyAttack;
            PInput.actions["Dodge"].performed += Dodge;
            PInput.actions["Run"].performed += Run;
            PInput.actions["Run"].canceled += Run;
            PInput.actions["Show Objective"].performed += ShowObjective;
            PInput.SwitchCurrentActionMap("Player");

            #endregion

            tag = "Player"; //setting a player, helps w/ identification
            rb = GetComponent<Rigidbody>(); //getting Rigidbody and Animator and Trasnform and MenuController and SpellScreen
            //Rigibody setup inside

            #region rbSetup

            rb.drag = Drag;
            rb.angularDrag = AngularDrag;
            rb.constraints = (RigidbodyConstraints) Constraints;

            #endregion
            
            anim = GetComponent<Animator>();
            tf = transform;
            Instance = GameObject.Find("player").GetComponent<Player>(); //setting Instance
            vCam = CinemachineCore.Instance
                    .GetVirtualCamera(0) as
                CinemachineVirtualCamera; //getting virtual camera and setting damping to default value
            if (vCam != null) vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = WalkDamping;
            PlayerPrefs.SetInt("PlayerXp", 12); //setting XP (for dev purposes)
            PlayerPrefs.SetInt("PlayerLvl", 1);
            Xp = PlayerPrefs.GetInt("PlayerXp"); //getting XP and Level, then calculating the current XP threshold //todo: move PlayerPrefs saves to real saves
            Lvl = (byte) PlayerPrefs.GetInt("PlayerLvl");
            XpThreshold = (int) (DefaultThreshold * ThresholdMultiplier * Lvl);
            PlayerReady?.Invoke();
        }

       
    }
}