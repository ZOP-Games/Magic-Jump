using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity//, ICancelHandler
{
    // this is for things unique to the player (controls, spells, etc.)
    public RectTransform mapPos;
    public PauseScreen pause;
    protected override int AttackStateHash => Animator.StringToHash("Attack");
    
    private bool mozog = false;
    private Vector2 mPos;
    private Rigidbody rb;
    private bool grounded = false;
    private bool running = false;
    private bool paused = false;
    private Vector3 speed;
    private Animator anim;
    private int moveStateId;
    private int moveSpeedId;
    private Transform mainCam;
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mozog = true;
            mPos = context.ReadValue<Vector2>();
            transform.Rotate(0, Mathf.Rad2Deg * Mathf.Atan2(mPos.x, mPos.y)*0.12f, 0);
            anim.SetBool(moveStateId,true);
        }

        if (context.canceled)
        {
            mozog = false;
            anim.SetBool(moveStateId,false);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(0,300,0);
            Debug.Log("Jumping, velocity: " + rb.velocity.y);
            
        }
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
            rb.AddForce(rb.velocity.x+23,0,0);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            running = true;
            anim.SetFloat(moveSpeedId,2);
        }

        if (context.canceled && GameHelper.CompareVectors(rb.velocity,5,GameHelper.Operation.Equal))
        {
            running = false;
            speed = rb.velocity;
            speed.z = 5;
            rb.velocity = speed;
            anim.SetFloat(moveSpeedId,1);
        }
    }

    public void UseSpell(InputAction.CallbackContext context)
    {
        if(context.performed)
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
                GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                paused = true;
                break;
        }
        
    }

    public void UnPause(InputAction.CallbackContext context)
    {
        Debug.Log("unpause is " + context.phase);
        if (context.performed && paused)
        {
            pause.UnPause();
            Debug.Log("Resumed");
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            paused = false;
        } 
    }

    public void ShowObjective(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Camera.current.GetComponent<CinemachineVirtualCamera>().LookAt = Objective.transform;
            Debug.Log("Show Objective");
            GameHelper.Wait(3,DoneLooking,this);
            
        }
    }
    private void DoneLooking()
    {
        Debug.Log("Camera looks back at player");
    }


    public override void Attack()
    {
        //play animation
    }

    protected override void Die()
    {
        Debug.Log("player died :(");
    }

    private new void OnCollisionEnter(Collision collision)
    {
        if (collision.collider is TerrainCollider && !grounded)
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
        if (mozog && Mathf.Abs(rb.velocity.x) < 5 && Mathf.Abs(rb.velocity.z) < 5 || (running && rb.velocity.x <15 && rb.velocity.z < 15))
        {
            var pos = transform.position;
            rb.AddRelativeForce(mPos.x * 25, 0, mPos.y * 25);
            mapPos.anchoredPosition = new Vector2(-pos.x,-pos.z)*0.5f;
        }
    }

    private void LateUpdate()
    {
        var tf = transform;
        mainCam.localPosition = tf.position + new Vector3(0, 3, -5);
        mainCam.eulerAngles = tf.eulerAngles + new Vector3(0,-30,0)*Mathf.Sign(tf.eulerAngles.y);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInParent<Animator>();
        moveStateId = Animator.StringToHash("moving");
        moveSpeedId = Animator.StringToHash("moveSpeed");
        mainCam = Camera.main.transform;
    }

    
}
