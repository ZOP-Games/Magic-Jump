using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    // this is for things unique to the player (controls, spells, etc.)
    public RectTransform mapPos;
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
    
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mozog = true;
            mPos = context.ReadValue<Vector2>();
            Vector3 angles = transform.eulerAngles;
            angles = new Vector3(angles.x, Mathf.Atan2(mPos.x,mPos.y)*Mathf.Rad2Deg, angles.z);
            transform.eulerAngles = angles;
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
        switch (context.performed)
        {
            case true when !paused:
                //show pause menu
                paused = true;
                //change to UI ActionMap
                Debug.Log("Paused");
                break;
            case true:
                //close pause menu
                //change back to Player ActionMap
                paused = false;
                Debug.Log("Unpaused");
                break;
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
    
    void FixedUpdate()
    {
        if (mozog && Mathf.Abs(rb.velocity.x) < 5 && Mathf.Abs(rb.velocity.z) < 5 || (running && rb.velocity.x <15 && rb.velocity.z < 15))
        {
            Vector3 pos = transform.position;
            rb.AddForce(mPos.x*25, 0, mPos.y*25);
            
            mapPos.anchoredPosition = new Vector2(pos.x,pos.z);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInParent<Animator>();
        moveStateId = Animator.StringToHash("moving");
        moveSpeedId = Animator.StringToHash("moveSpeed");
        

    }

    
}
