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
    public static Player Instance { get; private set; }

    //public references to some objects in the scene
    public PlayerInput PInput { get; private set; } //playerInput component
    private MenuController menus;
    private readonly SpellManager spells = SpellManager.Instance;
    private SpellScreen spellScreen;
    [FormerlySerializedAs("LevelUpText")] [SerializeField] private TextMeshProUGUI levelUpText;

    //setting Entity properties, for more info -> see Entity
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int MovingPmHash => Animator.StringToHash("moving");
    protected override Vector3 AtkSpherePos => new(0, 1, 0.5f);
    protected override int AtkSphereRadius => 1;

    private bool mozog; //bool for checking if the player is moving or not
    private Vector2 mPos; //Vector2 containing joystck input data
    private bool grounded; //bool for checking if the player is on ground or not
    private bool running; //bool for checking if the player is running or not
    private int moveSpeedId; //id of the moveSpeed parameter, for controlling animation speed from script
    private Transform tf; //the player's Transform
    private CinemachineVirtualCamera vCam; //the main virtual camera in the scene

    public int Xp { get; private set; }
    public byte Lvl { get; private set; }
    public int XpThreshold { get; private set; } = 10;

    //some constants to make code readable + adjustable
    private const int JumpForce = 400;
    private const int DodgePower = 23;
    private const int LookTimeout = 3;
    private const int WalkDamping = 5;
    private const int RunDamping = 2;
    private const int Constraints = 80;
    private const int DefaultThreshold = 10;
    private const float ThresholdMultiplier = 1.2f;

    //input event handlers
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mPos = context.ReadValue<Vector2>(); //storing input data
            anim.SetBool(MovingPmHash, true); //playing the move animation
            anim.SetFloat(moveSpeedId, 1); //setting aniation playback speed to 1
            mozog = true; //telling the code in FixedUpdate() that the player is moving
        }

        //if the player lets go of the stick, stop moving + checks in case of stick drift
        if (!context.canceled) return;
        mozog = false;
        anim.SetBool(MovingPmHash, false);
        mPos = Vector2.zero;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || !grounded) return;
        rb.AddForce(0, JumpForce, 0); //jump
        //Debug.Log("Jumping, velocity: " + rb.velocity.y); 
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack(); //see Entity.Attack()
        }
    }

    public void HeavyAttack(InputAction.CallbackContext context)
    {
        //we don't know what to do with this yet :/
        if (context.performed)
        {
            Debug.Log("Heavy Attack");
        }
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(rb.velocity.x + DodgePower, 0, 0); //pushing player to the side (idk if we still need this tbh)
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //tell the code in FixedUpdate() we're running
            running = true;
            anim.SetFloat(moveSpeedId, 2); //set "running" animation (speeding up walk animation)
            vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = RunDamping;
        }

        if (!context.canceled) return;
        running = false;
        anim.SetFloat(moveSpeedId, 1);
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = WalkDamping;
    }

    public void UseSpell(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        //play animation
        var spell = spells.SelectedSpell;
        spell.Use(GetEntities(AtkSpherePos,AtkSphereRadius));
        Debug.Log("Use Spell: " + spell);
    }

    public void ChangeSpell(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        spellScreen.Open();
        Debug.Log("Change Spell to: " + spells.SelectedSpell);
    }

    public void Pause(InputAction.CallbackContext context)
    {
        //Debug.Log("menu is " + context.phase);
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        if (!context.canceled) return;
        menus.OpenPause();
        Debug.Log("Paused");
    }

    public void CloseMenu()
    {
        menus.CloseActive();
        Debug.Log("Closed active menu");
    }

    public void CloseMenu(InputAction.CallbackContext context)
    {
        Debug.Log("unpause is " + context.phase);
        if (!context.canceled || PInput.currentActionMap.name == "Player") return;
        menus.CloseActive();
        Debug.Log("Closed active menu");
    }

    public void ShowObjective(InputAction.CallbackContext context)
    {
        //again, not sure if we need this but it's here anyway
        if (!context.performed) return;
        //vCam.LookAt = Objective.transform; it's commented out because there is no objective yet
        Debug.Log("Show Objective");
        Invoke(nameof(DoneLooking), LookTimeout); //after 3 seconds, return to normal camera view
    }

    private void DoneLooking()
    {
        Debug.Log("Camera looks back at player");
        //Camera.current.GetComponent<CinemachineVirtualCamera>().LookAt = tf;
    }


    public override void Die()
    {
        Debug.Log("player died :(");
        gameObject.SetActive(false);
    }

    public override void Stun()
    {
        base.Stun();
        PInput.DeactivateInput();
    }

    protected override void UnStun()
    {
        PInput.ActivateInput();
        base.UnStun();
    }

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

    

    //jumping thing: checks for being on the ground before jumping
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
        moveSpeedId = Animator.StringToHash("moveSpeed"); //setting moveSpeedId
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
        rb = GetComponent<Rigidbody>(); //getting Rigidbody and Animator and Trasnform
        //Rigibody setup inside
        #region rbSetup

        rb.drag = 0.1f;
        rb.angularDrag = 0.05f;
        rb.constraints = (RigidbodyConstraints)Constraints;
        

            #endregion
        Instance = GameObject.Find("player").GetComponent<Player>();
        anim = GetComponent<Animator>();
        tf = transform;
        vCam = CinemachineCore.Instance.GetVirtualCamera(0) as CinemachineVirtualCamera;   //getting virtual camera and setting damping to default value
        if (vCam != null) vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = WalkDamping;
        hpText = GetComponentInChildren<TextMeshPro>(); //getting hp text and setting to default value
        hpText.SetText("HP: 100");
        PlayerPrefs.SetInt("PlayerXp",12);
        PlayerPrefs.SetInt("PlayerLvl",1);
        Xp = PlayerPrefs.GetInt("PlayerXp");
        Lvl = (byte)PlayerPrefs.GetInt("PlayerLvl");
        XpThreshold = (int)(DefaultThreshold*ThresholdMultiplier * Lvl);
        menus = MenuController.Controller;
        spellScreen = FindObjectOfType<SpellScreen>(true);
    }
}

    