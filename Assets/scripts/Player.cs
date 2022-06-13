using System.Collections;
using Cinemachine;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
// ReSharper disable MemberCanBeMadeStatic.Global

public class Player : Entity
{
    // this is for things unique to the player (controls, spells, etc.)

    //references to some objects in the scene
    public PauseScreen pause; //the pause screen
    public TextMeshProUGUI fpsText; //the TMP text for displaying FPS

    //setting Entity properties, for more info -> see Entity
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int MovingPmHash => Animator.StringToHash("moving");
    protected override Vector3 AtkSpherePos => new(0, 1, 1);
    protected override int AtkSphereRadius => 2;
    protected override string OwnName { get; set; }

    private bool mozog; //bool for checking if the player is moving or not
    private Vector2 mPos; //Vector2 containing joystck input data
    private bool grounded; //bool for checking if the player is on ground or not
    private bool running; //bool for checking if the player is running or not
    private bool paused; //bool for checking if the game is paused or not
    private int moveSpeedId; //id of the moveSpeed parameter, for controlling animation speed from script
    private PlayerInput pInput; //playerInput component
    private Transform tf; //the player's Transform
    private CinemachineVirtualCamera vCam; //the main virtual camera in the scene

    //some constants to make code readable + adjustable
    private const int JumpForce = 400;
    private const int DodgePower = 23;
    private const int LookTimeout = 3;
    private const int WalkDamping = 5;
    private const int RunDamping = 1;

    //input event handlers
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mPos = context.ReadValue<Vector2>().normalized; //storing input data
            anim.SetBool(MovingPmHash, true); //playing the move animation
            anim.SetFloat(moveSpeedId, 1); //setting aniation playback speed to 1
            mozog = true; //telling the code in FixedUpdate() that the player is moving
        }

        //if the player lets go of the stick, stop moving + checks in case of stick drift
        if (!context.canceled && !mPos.CompareWithValue(0, GameHelper.Operation.Equal)) return;
        mozog = false;
        anim.SetBool(MovingPmHash, false);

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
            Attack(AtkSpherePos,AtkSphereRadius); //see Entity.Attack()
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
        if (context.performed)
        {
            //not much here
            //play animation
            //apply effects
            Debug.Log("Use Spell");
        }
    }

    public void ChangeSpell(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //not much here
            Debug.Log("Change Spell");
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("pause is " + context.phase);
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                pause.Pause();
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Paused");
                pInput.SwitchCurrentActionMap("UI"); //changes input action map to UI
                paused = true;
                break;
        }

    }

    public void UnPause(InputAction.CallbackContext context)
    {
        Debug.Log("unpause is " + context.phase);
        if (!context.performed || !paused) return;
        pause.UnPause();
        Debug.Log("Resumed");
        pInput.SwitchCurrentActionMap("Player"); //changes input action map back to player
        paused = false;
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


    protected override void Die()
    {
        Debug.Log("player died :(");
        gameObject.SetActive(false);
    }

    //show FPS so we can see it in builds
    private void ShowFPS()
    {
        fpsText.SetText("FPS: " + 1 / Time.deltaTime); //FPS = 1 / frametime
    }

    //jumping thing: checks for being on the ground before jumping
    protected void OnCollisionStay(Collision collision)
    {
        if (collision.collider is TerrainCollider && !grounded)
        {

            grounded = true; //if the player is touching the ground, they can jump
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider is TerrainCollider && grounded)
        {
            grounded = false; //as soon as the player leaves the ground, they can't jump
        }
    }

    //FixedUpdate() updates a fixed amount per second (50-ish), useful for physics or control
    private void FixedUpdate()
    {
        //movement logic
        var angle = Mathf.Atan2(mPos.x, mPos.y) * Mathf.Rad2Deg;    //getting the angle from stick input
        //Debug.Log("mPos: " + mPos + "angle: " + angle);
        tf.localEulerAngles += new Vector3(0,angle/50,0);   //rotating the playing
        if (running) Move(tf.forward, RunSpeed); //moving the running player forward
        else if (mozog) Move(tf.InverseTransformDirection(tf.forward));  //moving the player forward
    }


    //Start() runs once when the object is enabled, lots of early game setup goes here
    private void Start()
    { 
        moveSpeedId = Animator.StringToHash("moveSpeed"); //setting moveSpeedId
        pInput = GetComponent<PlayerInput>(); //setting PlayerInput
        //PlayerInput setup inside

        #region PiSetup

//setting up PlayerInput so I don't have to do it all the time
            pInput.actions["Move"].performed += Move;
            pInput.actions["Move"].canceled += Move;
            pInput.actions["Jump"].performed += Jump;
            pInput.actions["Attack"].performed += LightAttack;
            pInput.actions["Heavy Attack"].performed += HeavyAttack;
            pInput.actions["Dodge"].performed += Dodge;
            pInput.actions["Run"].performed += Run;
            pInput.actions["Run"].canceled += Run;
            pInput.actions["Spell"].performed += UseSpell;
            pInput.actions["Change"].performed += ChangeSpell;
            pInput.actions["Pause"].performed += Pause;
            pInput.actions["Pause"].canceled += Pause;
            pInput.actions["Exit"].canceled += UnPause;
            pInput.actions["Show Objective"].performed += ShowObjective;


            #endregion

        InvokeRepeating(nameof(ShowFPS), 0, 0.5f); //starting to display FPS
        tag = "Player"; //setting a player, helps w/ identification
        OwnName = name;
        rb = GetComponent<Rigidbody>(); //getting Rigidbody and Animator and Trasnform
        anim = GetComponent<Animator>();
        tf = transform;
        vCam = Camera.main!.GetComponent<CinemachineVirtualCamera>();   //getting virtual camera and setting damping to default value
        vCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_ZDamping = WalkDamping;
        hpText = GetComponentInChildren<TextMeshPro>(); //getting hp text and setting to default value
        hpText.SetText("HP: 100");
    }


    }
    