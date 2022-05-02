using System.Collections;
using Cinemachine;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : Entity
{
    // this is for things unique to the player (controls, spells, etc.)

    //references to some objects in the scene
    public RectTransform mapImgPos;
    public PauseScreen pause;
    public TextMeshProUGUI fpsText;

    //setting attack and move state hashes
    protected override int AttackingPmHash => Animator.StringToHash("attacking");
    protected override int MovingPmHash => Animator.StringToHash("moving");

    private bool mozog;
    private Vector2 mPos;
    private bool grounded;
    private bool running;
    private bool paused;
    private int moveSpeedId;
    private Transform mainCam;
    private PlayerInput pInput;
    private Transform tf;



    private const int JumpForce = 400;

    //input event handlers
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mozog = true; //telling the code in FixedUpdate() that the player is moving
            mPos = context.ReadValue<Vector2>(); //storing input data
            /*while (Mathf.Approximately(transform.eulerAngles.y,Mathf.Rad2Deg * Mathf.Atan2(mPos.y,mPos.x)))   //rotating somehow (we don't know how yet)
            {
                transform.Rotate(0,1,0);
            }*/
            anim.SetBool(MovingPmHash, true); //playing the move animation
            anim.SetFloat(moveSpeedId,1*Mathf.Sign(mPos.y)); //setting aniation playback speed to 1
        }
        //if the player lets go of the stick, stop moving
        if (!context.canceled) return;
        mozog = false;
        anim.SetBool(MovingPmHash,false);
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
            rb.AddForce(rb.velocity.x + 23, 0, 0); //pushing player to the side (idk if we still need this tbh)
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //tell the code in FixedUpdate() we're running
            running = true;
            anim.SetFloat(moveSpeedId, 2 * Mathf.Sign(mPos.y)); //set "running" animation (speeding up walk animation)
        }

        if (!context.canceled) return;
        running = false;
        anim.SetFloat(moveSpeedId,1 * Mathf.Sign(mPos.y));
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
        //Camera.current.GetComponent<CinemachineVirtualCamera>().LookAt = Objective.transform; it's commented out because there is no objective yet
        Debug.Log("Show Objective");
        Invoke(nameof(DoneLooking),3); //after 3 seconds, return to normal camera view
    }
    private void DoneLooking()
    {
        Debug.Log("Camera looks back at player");
        Camera.current.GetComponent<CinemachineVirtualCamera>().LookAt = tf;
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
    private void OnCollisionEnter(Collision collision)
    {
        var cCollider = collision.collider;
        if (cCollider is TerrainCollider && !grounded)
        {

            grounded = true;    //if the player is touching the ground, they can jump
        }
        
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider is TerrainCollider && grounded)
        {
            grounded = false; //as soon as the player leaves the ground, they can't jump
        }
    }

    private void FixedUpdate()
    {
        var pos = 0f;
        if (running)
        { 
            pos = tf.position.z;
            tf.Rotate(0,mPos.x, 0);
            mapImgPos.anchoredPosition = new Vector2(250,-pos+250);    //make the map track the player's movement
            mapImgPos.eulerAngles = new Vector3(0,0,tf.eulerAngles.y);  //rotating map
            Move(mPos.ToVector3(),15);
            
        }
        else if(mozog)
        {
            pos = tf.position.z;
            tf.Rotate(0,mPos.x, 0);
            mapImgPos.anchoredPosition = new Vector2(250,-pos+250);     //make the map track the player's movement
            mapImgPos.Rotate(0, 0, -mPos.x);    //rotating map
            Move(mPos.ToVector3(),5);
            
        }

    }

    private void LateUpdate()
    {
        var tfAngles  = tf.eulerAngles;
        mainCam.localPosition = tf.position + new Vector3(0, 3, -5);
        mainCam.eulerAngles = tfAngles + new Vector3(0, -30, 0) * Mathf.Sign(tfAngles.y);
        
    }
    
    private void Start()
    {
        moveSpeedId = Animator.StringToHash("moveSpeed");
        mainCam = Camera.main!.transform;
        pInput = GetComponent<PlayerInput>();
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
        InvokeRepeating(nameof(ShowFPS),0,0.5f);
        tag = "Player";
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        tf = transform;
    }


}