using System.Collections;
using GameExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : Entity
{
    // this is for things unique to the player (controls, spells, etc.)
    public RectTransform mapImgPos;
    public PauseScreen pause;
    public TextMeshProUGUI fpsText;
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
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mozog = true;
            mPos = context.ReadValue<Vector2>();
            /*while (Mathf.Approximately(transform.eulerAngles.y,Mathf.Rad2Deg * Mathf.Atan2(mPos.y,mPos.x)))
            {
                transform.Rotate(0,1,0);
            }*/
            anim.SetBool(MovingPmHash, true);
            anim.SetFloat(moveSpeedId,1*Mathf.Sign(mPos.y));
        }

        if (!context.canceled) return;
        mozog = false;
        anim.SetBool(MovingPmHash,false);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed || !grounded) return;
        rb.AddForce(0, 400, 0);
        Debug.Log("Jumping, velocity: " + rb.velocity.y);
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Attack();
        }
    }

    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Heavy Attack");
            //play animation
        }
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(rb.velocity.x + 23, 0, 0);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            running = true;
            anim.SetFloat(moveSpeedId, 2 * Mathf.Sign(mPos.y));
        }

        if (!context.canceled) return;
        running = false;
        anim.SetFloat(moveSpeedId,1 * Mathf.Sign(mPos.y));
    }

    public void UseSpell(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //play animation
            //apply effects
            Debug.Log("Use Spell");
        }
    }

    public void ChangeSpell(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //show spell menu
            //change to UI InputActionMap
            //change to selected spell
            //close spell menu
            //change back to player InputActionMap
            Debug.Log("Change Spell");
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        Debug.Log("pause is " + context.phase);
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                pause.Pause();
                break;
            case InputActionPhase.Canceled:
                Debug.Log("Paused");
                pInput.SwitchCurrentActionMap("UI");
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
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        paused = false;
    }

    public void ShowObjective(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        //Camera.current.GetComponent<CinemachineVirtualCamera>().LookAt = Objective.transform;
        Debug.Log("Show Objective");
        Invoke(nameof(DoneLooking),3);
    }
    private static void DoneLooking()
    {
        Debug.Log("Camera looks back at player");
    }


    protected override void Die()
    {
        Debug.Log("player died :(");
        gameObject.SetActive(false);
    }

    private void ShowFPS()
    {
        fpsText.SetText("FPS: " + 1 / Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var cCollider = collision.collider;
        if (cCollider is TerrainCollider && !grounded)
        {

            grounded = true;
        }
        
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider is TerrainCollider && grounded)
        {
            grounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (running)
        { 
            transform.Rotate(0,mPos.x, 0);
            Move(mPos,15);
        }
        else if(mozog)
        {
		    transform.Rotate(0,mPos.x, 0);
            Move(mPos,5);
        }

        var pos = transform.position;
        mapImgPos.anchoredPosition = new Vector2(-pos.x, -pos.z) * 0.5f;
    }

    private void LateUpdate()
    {
        var tf = transform;
        var tfAngles  = tf.eulerAngles;
        mainCam.localPosition = tf.position + new Vector3(0, 3, -5);
        mainCam.eulerAngles = tfAngles + new Vector3(0, -30, 0) * Mathf.Sign(tfAngles.y);
        
    }
    
    private void Start()
    {
        moveSpeedId = Animator.StringToHash("moveSpeed");
        mainCam = Camera.main.transform;
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
    }


}