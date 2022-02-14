using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    // this is for things unique to the player (controls, spells, etc.)
    protected override int AttackStateHash => Animator.StringToHash("Attack");
    
    private bool mozog = false;
    private Vector2 mPos;
    private Rigidbody rb;
    private bool grounded = true;
    private bool running = false;
    private bool paused = false;
    private Vector3 speed;
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mozog = true;
            mPos = context.ReadValue<Vector2>();
            Vector3 angles = transform.eulerAngles;
            angles = new Vector3(angles.x, mPos.x * 9, angles.z);
            transform.eulerAngles = angles;
        }

        if (context.canceled)
        {
            mozog = false;
            
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(0,400,0);

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
            rb.AddForce(transform.position.x+23,0,0);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            running = true;
        }

        if (context.canceled && GameHelper.CompareVectors(rb.velocity,5,GameHelper.Operation.Equal))
        {
            running = false;
            speed = rb.velocity;
            speed.z = 5;
            rb.velocity = speed;
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
        if (context.performed && !paused)
        {
            //show pause menu
            paused = true;
            //change to UI ActionMap
            Debug.Log("Paused");
        }
        else if(context.performed)
        {
            //close pause menu
            //change back to Player ActionMap
            paused = false;
            Debug.Log("Unpaused");
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
        if (collision.gameObject.TryGetComponent<TerrainCollider>(out _) && !grounded)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<TerrainCollider>(out _) && grounded)
        {
            grounded = false;
        }
    }

    void Update()
    {
        if (mozog && (Mathf.Abs(rb.velocity.z) < 5 || (Mathf.Abs(rb.velocity.z) < 15 && running)))
        {
            
            rb.AddForce(mPos.x,0,mPos.y);
            
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
}
