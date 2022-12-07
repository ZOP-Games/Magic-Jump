using System.Collections;
using Cinemachine;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Serialization;

[RequireComponent(typeof(PlayerInput))]
public class Player : Entity
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
    public PlayerInput PInput { get; private set; }
    /// <summary>
    /// The active <see cref="MenuController"></see> instance.
    /// </summary>
    private MenuController menus;
    /// <summary>
    /// The active <see cref="SpellManager"/> instance.
    /// </summary>
    private readonly SpellManager spells = SpellManager.Instance;
    /// <summary>
    /// The <see cref="SpellScreen"/> in use.
    /// </summary>
    private SpellScreen spellScreen;
    /// <summary>
    /// The "level up!" text object.
    /// </summary>
    [FormerlySerializedAs("LevelUpText")] [SerializeField] private TextMeshProUGUI levelUpText;

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
    private Transform tf; 
    /// <summary>
    /// the main <see cref="CinemachineVirtualCamera"/> in the scene
    /// </summary>
    private CinemachineVirtualCamera vCam;

    /// <summary>
    /// The player's XP.
    /// </summary>
    public int Xp { get; private set; }
    /// <summary>
    /// The player level.
    /// </summary>
    public byte Lvl { get; private set; }
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
    /// Constraint sum of the <see cref="Rigidbody"/> component. See <see href="https://docs.unity3d.com/ScriptReference/Rigidbody-constraints.html"/> for details.
    /// </summary>
    /// <remarks>Don't change this unless physics don't move as they should.</remarks>
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
        if (context.performed)
        {
            Attack(); //see Entity.Attack()
        }
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
            rb.AddForce(rb.velocity.x + DodgePower, 0, 0); //pushing player to the side (idk if we still need this tbh)
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
    /// Event handler for using spells.
    /// </summary>
    /// <param name="context"><inheritdoc cref="Jump"/></param>
    public void UseSpell(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        //play animation
        var spell = spells.SelectedSpell;
        spell.Use(GetEntities(AtkSpherePos,AtkSphereRadius));
        Debug.Log("Use Spell: " + spell);
    }
    /// <summary>
    /// Event handler for changing spells.
    /// </summary>
    /// <param name="context"><inheritdoc cref="Jump"/></param>
    public void ChangeSpell(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        spellScreen.Open();
        Debug.Log("Change Spell to: " + spells.SelectedSpell);
    }
    /// <summary>
    /// Event handler for pausing.
    /// </summary>
    /// <param name="context"><inheritdoc cref="Jump"/></param>
    public void Pause(InputAction.CallbackContext context)
    {
        //Debug.Log("menu is " + context.phase);
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        if (!context.canceled) return;
        menus.OpenPause();
        Debug.Log("Paused");
    }
    /// <summary>
    /// Closes the active menu. Used for when a <see cref="UnityEngine.UI.Button"/> is pressed.
    /// </summary>
    public void CloseMenu()
    {
        menus.CloseActive();
        Debug.Log("Closed active menu");
    }
    /// <summary>
    /// Closes the active menu. Used for pressing the 'cancel' <see cref="UnityEngine.InputSystem.InputAction"/>.
    /// </summary>
    /// <param name="context"><inheritdoc cref="Jump"/></param>
    public void CloseMenu(InputAction.CallbackContext context)
    {
        Debug.Log("unpause is " + context.phase);
        if (!context.canceled || PInput.currentActionMap.name == "Player") return;
        menus.CloseActive();
        Debug.Log("Closed active menu");
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
        XpThreshold = Xp+Mathf.RoundToInt(XpThreshold * ThresholdMultiplier);
        levelUpText.SetText(levelUpText.text.Remove(14, levelUpText.text.Length - 15));
        levelUpText.text += Lvl;
        StartCoroutine(levelUpText.gameObject.ActivateFor(3));
    }

    
    protected void OnCollisionStay(Collision collision)
    {
        if (collision.collider is TerrainCollider && !grounded) grounded = true;    //if the player is touching the ground, they can jump

    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider is TerrainCollider && grounded) grounded = false;    //as soon as the player leaves the ground, they can't jump
    }

    //FixedUpdate() updates a fixed amount per second (50-ish), useful for physics or control
    private void FixedUpdate()
    {
        //movement logic
        var angle = Mathf.Atan2(mPos.x, mPos.y) * Mathf.Rad2Deg;    //getting the angle from stick input
        tf.localEulerAngles += new Vector3(0,angle/50,0);   //rotating the player
        if (running) Move(tf.InverseTransformDirection(tf.forward), RunSpeed); //moving the running player forward
        else if (mozog) Move(tf.InverseTransformDirection(tf.forward));
    }


    //Start() runs once when the object is enabled, lots of early game setup goes here
    private void Start()
    {
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
            PInput.actions["Spell"].performed += UseSpell;
            PInput.actions["Change"].performed += ChangeSpell;
            PInput.actions["Pause"].canceled += Pause;
            PInput.actions["Exit"].canceled +=  CloseMenu;
            PInput.actions["Show Objective"].performed += ShowObjective;
            PInput.SwitchCurrentActionMap("Player");

            #endregion
        tag = "Player"; //setting a player, helps w/ identification
        rb = GetComponent<Rigidbody>(); //getting Rigidbody and Animator and Trasnform and MenuController and SpellScreen
        //Rigibody setup inside
        #region rbSetup

        rb.drag = 0.1f;
        rb.angularDrag = 0.05f;
        rb.constraints = (RigidbodyConstraints)Constraints;
        

            #endregion
            anim = GetComponent<Animator>();
        tf = transform;
        menus = MenuController.Controller;
        spellScreen = FindObjectOfType<SpellScreen>(true);
        Instance = GameObject.Find("player").GetComponent<Player>();    //setting Instance
        vCam = CinemachineCore.Instance.GetVirtualCamera(0) as CinemachineVirtualCamera;   //getting virtual camera and setting damping to default value
        if (vCam != null) vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = WalkDamping;
        hpText = GetComponentInChildren<TextMeshPro>(); //getting hp text and setting to default value
        hpText.SetText("HP: 100");
        PlayerPrefs.SetInt("PlayerXp",12);  //setting XP (for dev purposes)
        PlayerPrefs.SetInt("PlayerLvl",1);
        Xp = PlayerPrefs.GetInt("PlayerXp");    //getting XP and Level, then calculating the current XP threshold
        Lvl = (byte)PlayerPrefs.GetInt("PlayerLvl");
        XpThreshold = (int)(DefaultThreshold*ThresholdMultiplier * Lvl);
    }
}

    