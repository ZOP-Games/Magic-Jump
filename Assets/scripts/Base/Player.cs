using Cinemachine;
using System;
using GameExtensions.Debug;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

// ReSharper disable MergeConditionalExpression

namespace GameExtensions
{
    /// <summary>
    ///     Class representing the player.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    [Serializable]
    public sealed class Player : Entity, ISaveable, ISerializationCallbackReceiver, IInputHandler
    {
        //some constants to make code readable + adjustable
        /// <summary>
        ///     The force the player's <see cref="Rigidbody" /> pushes it up to jump. You can change its value to adjust jump
        ///     height.
        /// </summary>
        private const int JumpForce = 12;

        /// <summary>
        ///     The force the player's <see cref="Rigidbody" /> pushes it to dodge. You can change its value to adjust dodge
        ///     distance.
        /// </summary>
        private const int DodgePower = 230;
        private const int SuperDodgePower = 2300;

        /// <summary>
        ///     The amount of seconds the game waits to look at the objective.
        /// </summary>
        private const int LookTimeout = 3;

        /// <summary>
        ///     The amount of damping on the camera while walking. You can change its value to adjust how far the camera zooms out
        ///     while walking.
        /// </summary>
        private const int WalkDamping = 0;

        /// <summary>
        ///     The amount of damping on the camera while running. You can change its value to adjust how far the camera zooms out
        ///     while running.
        /// </summary>
        /// <remarks>It generally should be smaller than <see cref="WalkDamping" />. </remarks>
        private const int RunDamping = 2;

        /// <summary>
        ///     Drag of the <see cref="Rigidbody" /> component.
        /// </summary>
        /// <remarks>Don't change this unless things don't move as they should.</remarks>
        private const float Drag = .1f;

        /// <summary>
        ///     Angular drag of the <see cref="Rigidbody" /> comonent.
        /// </summary>
        /// <remarks><inheritdoc cref="Drag"/></remarks>
        private const float AngularDrag = 1;

        /// <summary>
        ///     Constraint sum of the <see cref="Rigidbody" /> component. See
        ///     <see href="https://docs.unity3d.com/ScriptReference/Rigidbody-constraints.html" /> for details.
        /// </summary>
        /// <remarks><inheritdoc cref="Drag"/></remarks>
        private const int Constraints = 80;

        /// <summary>
        ///     The start XP threshold which then gets scaled to the player's <see cref="Lvl" />.
        /// </summary>
        private const int DefaultThreshold = 10;

        /// <summary>
        ///     The multiplier the XP threshold increases with each level. You can change its value to adjust how much more
        ///     difficult each level gets.
        /// </summary>
        private const float ThresholdMultiplier = 1.2f;

        private const float ShortRumble = 0.1f;
        private const float MediumRumble = 0.2f;
        private const float LongRumble = 0.3f;
        private const float RightMoveMultiplier = 0.05f;
        private const float ForwardMoveMultiplier = 0.1f;
        private const int TurnMultiplier = 5;
        private const float GravityForce = 0.98f;
        [SerializeField][HideInInspector] private Vector3 playerPos;
        [SerializeField][HideInInspector] private Vector3 playerAngles;

        /// <summary>
        ///     The player's XP.
        /// </summary>
        [field: SerializeField]
        public int Xp { get; private set; }

        /// <summary>
        ///     The player level.
        /// </summary>
        [field: SerializeField]
        public byte Lvl { get; private set; }

        private bool jumping;

        private float jumpHeight;
        /// <summary>
        ///     Used for checking if the player is moving or not
        /// </summary>
        private bool mozog;

        /// <summary>
        ///     Contains joystick input data.
        /// </summary>
        private Vector2 mPos;

        /// <summary>
        ///     Used for checking if the player is running or not
        /// </summary>
        private bool running;

        private LineRenderer line;
        private Transform vCamTf;
        /// <summary>
        ///     the player's <see cref="Transform" />.
        /// </summary>
        [NonSerialized] private Transform tf;

        // this is for things unique to the player (controls, spells, etc.)
        /// <summary>
        ///     The main <see cref="Player"></see> Instance.
        /// </summary>
        public static Player Instance { get; private set; }

        /// <summary>
        ///     id of the moveSpeed parameter, for controlling animation speed from script
        /// </summary>
        private static int MoveSpeedId => Animator.StringToHash("moveSpeed");

        //public references to some objects in the scene
        /// <summary>
        ///     The player's <see cref="PlayerInput" /> component.
        /// </summary>
        [field: NonSerialized]
        public PlayerInput PInput { get; private set; }

        //setting Entity properties, for more info -> see Entity
        protected override int AttackingPmHash => Animator.StringToHash("attacking");
        protected override int MovingPmHash => Animator.StringToHash("moving");
        protected override int RunningPmHash => Animator.StringToHash("running");
        protected override int DamagePmHash => Animator.StringToHash("damage");
        protected override Vector3 AtkSpherePos => new(0, 1.5f, 0.5f);
        protected override int AtkRange => 15;

        /// <summary>
        ///     The amount of XP the player needs to level up.
        /// </summary>
        public int XpThreshold { get; private set; } = 10;

        byte ISaveable.Id { get; set; }


        public void OnBeforeSerialize()
        {
            playerAngles = tf != null ? tf.eulerAngles : Vector3.zero;
            playerPos = tf != null ? tf.position : Vector3.zero;
        }

        public void OnAfterDeserialize()
        {
            if (tf is null) return;
            tf.position = playerPos;
            tf.eulerAngles = playerAngles;
        }

        /// <summary>
        ///     Fires when the <see cref="Player" /> is done setting up.
        /// </summary>
        /// <remarks>Use it if you need the Player at the start of the game.</remarks>
        public static event UnityAction PlayerReady;

        public event UnityAction PlayerDied;

        //input event handlers
        /// <summary>
        ///     Gets the player's movement input and saves that data to <see cref="mPos" />.
        /// </summary>
        /// <param name="context">The input data.</param>
        public void Move(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                mPos = context.ReadValue<Vector2>(); //storing input data
                anim.SetFloat(MoveSpeedId, 1); //setting aniation playback speed to 1
                mozog = true; //telling the code in FixedUpdate() that the player is moving
            }

            //if the player lets go of the stick, stop moving
            if (!context.canceled) return;
            mozog = false;
            anim.SetBool(MovingPmHash, false);
            mPos = Vector2.zero;
        }

        /// <summary>
        ///     Event handler for jumping.
        /// </summary>
        /// <param name="context">The input data.</param>
        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed || jumping) return;
            jumping = true;
            jumpHeight = JumpForce;
        }

        /// <summary>
        ///     Event handler for standard (light) attacking.
        /// </summary>
        /// <param name="context">
        ///     <inheritdoc cref="Jump" />
        /// </param>
        public void LightAttack(InputAction.CallbackContext context)
        {
            Attack(); //see Entity.Attack()
        }

        protected override void Attack()
        {
            //docs here
            base.Attack();
            Rumble.RumbleFor(Rumble.RumbleStrength.Medium, Rumble.RumbleStrength.Medium, 0.1f);
        }

        public override void TakeDamage(int amount)
        {
            if (DebugManager.IsInvincible) return;
            base.TakeDamage(amount);
            Rumble.RumbleFor(Rumble.RumbleStrength.Strong, Rumble.RumbleStrength.Strong, 0.1f);
        }

        /// <summary>
        ///     Event handler for second (heavy) attacking.
        /// </summary>
        /// <param name="context">
        ///     <inheritdoc cref="Jump" />
        /// </param>
        public void HeavyAttack(InputAction.CallbackContext context)
        {
            //we don't know what to do with this yet :/
            if (context.performed)
                Stun();
            Invoke(nameof(UnStun), 3);
        }

        /// <summary>
        ///     Event handler for dodging.
        /// </summary>
        /// <param name="context">
        ///     <inheritdoc cref="Jump" />
        /// </param>
        public void Dodge(InputAction.CallbackContext context)
        {
            if (!context.performed || !(Mathf.Abs(cc.velocity.x) < 1)) return;
            var power = DebugManager.IsSuperDodging ? SuperDodgePower : DodgePower;
            cc.Move(new Vector3(power, 0, 0)); //pushing player to the side (idk if we still need this tbh) hell yeah brother
        }

        /// <summary>
        ///     Event handler for running.
        /// </summary>
        /// <param name="context">
        ///     <inheritdoc cref="Jump" />
        /// </param>
        public void Run(InputAction.CallbackContext context)
        {

            if (context.performed)
            {
                //tell the code in FixedUpdate() we're running
                Rumble.RumbleFor(Rumble.RumbleStrength.Medium, Rumble.RumbleStrength.Light, 0.1f);
                running = true;
                anim.SetFloat(MoveSpeedId, 2); //set "running" animation (speeding up walk animation)
                anim.SetBool(RunningPmHash, true);
            }

            if (!context.canceled) return;
            running = false;
            anim.SetFloat(MoveSpeedId, 1);
            anim.SetBool(RunningPmHash, false);
        }

        public void SaveGame(InputAction.CallbackContext context)
        {
            if (!context.canceled) return;
            SaveManager.SaveAll();
        }

        public void LoadGame(InputAction.CallbackContext context)
        {
            if (!context.canceled) return;
            SaveManager.LoadAll();
        }

        /// <summary>
        ///     Kills the player.
        /// </summary>
        /// <remarks>[overriden from <see cref="Entity.Die" />]</remarks>
        public override void Die()
        {
            DebugConsole.LogError("player died :(");
            enabled = false;
            PlayerDied?.Invoke();
        }

        /// <summary>
        ///     Stuns the player.
        /// </summary>
        /// <remarks>[overriden from <see cref="Entity.Stun" />]</remarks>
        public override void Stun()
        {
            base.Stun();
            PInput.DeactivateInput();
            Rumble.RumbleFor(Rumble.RumbleStrength.Medium, Rumble.RumbleStrength.Strong, 0.3f);
        }

        /// <summary>
        ///     Cancels the stun effect.
        /// </summary>
        /// <remarks>[overriden from <see cref="Entity.UnStun" />]</remarks>
        protected override void UnStun()
        {
            PInput.ActivateInput();
            base.UnStun();
        }

        /// <summary>
        ///     Adds XP to the player, and levels it up if necesarry.
        /// </summary>
        /// <param name="amount">The amount of XP given to the player.</param>
        public void AddXp(int amount)
        {
            Xp += amount;
            if (Xp < XpThreshold) return;
            Lvl++;
            XpThreshold = Xp + Mathf.RoundToInt(XpThreshold * ThresholdMultiplier);
            DebugConsole.Log("leveled up! Level" + Lvl, DebugConsole.SuccessColor);
        }

        //FixedUpdate() updates a fixed amount per second (50-ish), useful for physics or control
        private void FixedUpdate()
        {
            if (DebugManager.DrawForceLine)
            {
                line.gameObject.SetActive(true);
                line.SetPosition(1, cc.velocity);
            }
            else if (line.gameObject.activeSelf)
            {
                line.gameObject.SetActive(false);
            }
            var direction = Vector3.zero;
            if (mozog)
            {
                var angle = vCamTf.eulerAngles.y + Mathf.Atan2(mPos.x, mPos.y) * Mathf.Rad2Deg;
                tf.rotation = Quaternion.Slerp(tf.rotation, Quaternion.Euler(0, angle, 0),
                 TurnMultiplier * Time.fixedDeltaTime);
                var lookForward = vCamTf.forward.normalized;
                lookForward.y = 0;
                var lookRight = vCamTf.right.normalized * RightMoveMultiplier;
                lookRight.y = 0;
                direction = ForwardMoveMultiplier * mPos.y * lookForward + lookRight * mPos.x;
            }
            direction.y = cc.velocity.y - GravityForce;
            if (jumping)
            {
                direction.y += jumpHeight;
                jumpHeight = 0;
                if (cc.isGrounded)
                {
                    jumping = false;
                }
            }

            var speed = running ? WalkSpeed : RunSpeed;
            Move(direction * Time.fixedDeltaTime, running, speed);
        }

        //Start() runs once when the object is enabled, lots of early game setup goes here
        private new void Start()
        {
            base.Start();
            if (Instance is not null) Destroy(this);
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
            PInput.actions["Save"].canceled += SaveGame;
            PInput.actions["Load"].canceled += LoadGame;
            PInput.SwitchCurrentActionMap("Player");

            #endregion

            tag = "Player"; //setting a player tag, helps w/ identification
            cc = GetComponent<CharacterController>(); //getting Rigidbody and Animator and Transform and Instance and XP and difficulty
            anim = GetComponent<Animator>();
            tf = transform;
            Instance = this;
            vCamTf = FindAnyObjectByType<CinemachineBrain>().transform;
            XpThreshold = (int)(DefaultThreshold * ThresholdMultiplier * Lvl);
            Difficulty.DifficultyLevelChanged += () =>
            {
                if (DifficultyMultiplier > 1.5f) Hp = Mathf.RoundToInt(Hp / DifficultyMultiplier);
            };
            if (DifficultyMultiplier > 1.5f) Hp = Mathf.RoundToInt(Hp / DifficultyMultiplier);
            DebugInputHandler.Instance.AddInputCallback("[Debug] Add XP", () =>
            {
                AddXp(1000);
                DebugConsole.Log("Added 1000 XP!", DebugConsole.TestColor);
            });
            line = GetComponentInChildren<LineRenderer>();
            (this as ISaveable).AddToList();
            PlayerReady?.Invoke();
        }

        #region InputActionAdder

        /// <summary>
        ///     Adds an <see cref="UnityAction" /> to the input action list.
        /// </summary>
        /// <param name="actionName">The name of the <see cref="InputAction" />.</param>
        /// <param name="action">The action to be invoked.</param>
        /// <param name="type">When you want to listen to the event.</param>
        public void AddInputAction(string actionName, UnityAction action,
            IInputHandler.ActionType type = IInputHandler.ActionType.Performed)
        {
            switch (type)
            {
                case IInputHandler.ActionType.Started:
                    PInput.actions[actionName].started += context =>
                    {
                        if (context.started) action.Invoke();
                    };
                    break;
                case IInputHandler.ActionType.Performed:
                    PInput.actions[actionName].performed += context =>
                    {
                        if (context.performed) action.Invoke();
                    };
                    break;
                case IInputHandler.ActionType.Canceled:
                    PInput.actions[actionName].canceled += context =>
                    {
                        if (context.canceled) action.Invoke();
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type,
                        "The specified ActionType does not exist");
            }
        }

        #endregion

        public void Revive()
        {
            enabled = true;
            Instance.Hp = 100;
        }
    }
}