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

    private bool mozog = false;
    private Vector2 mPos;
    private bool grounded = false;
    private bool running = false;
    private bool paused = false;
    private Vector3 speed;
    private int moveSpeedId;
    private Transform mainCam;
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mozog = true;
            mPos = context.ReadValue<Vector2>();
            transform.Rotate(0, Mathf.Rad2Deg * Mathf.Atan2(mPos.x, mPos.y) * 0.12f, 0);
            anim.SetBool(MovingPmHash, true);
        }

        if (context.canceled)
        {
            mozog = false;
            anim.SetBool(MovingPmHash,false);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && grounded)
        {
            rb.AddForce(0, 300, 0);
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
            rb.AddForce(rb.velocity.x + 23, 0, 0);
        }
    }

    public void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            running = true;
            anim.SetFloat(moveSpeedId, 2);
        }

        if (context.canceled && GameHelper.CompareVectors(rb.velocity, 5, GameHelper.Operation.Equal))
        {
            running = false;
            speed = rb.velocity;
            speed.z = 5;
            rb.velocity = speed;
            anim.SetFloat(moveSpeedId, 1);
        }
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
            GameHelper.Wait(3, DoneLooking, this);

        }
    }
    private static void DoneLooking()
    {
        Debug.Log("Camera looks back at player");
    }


    protected override void Die()
    {
        Debug.Log("player died :(");
    }

    private new void OnCollisionEnter(Collision collision)
    {
        var cCollider = collision.collider;
        if (cCollider is TerrainCollider && !grounded)
        {

            grounded = true;
        }
        else if (cCollider.CompareTag("Enemy"))
        {
            var enemy =cCollider.GetComponent<EnemyBase>();
            if (anim.GetBool(AttackingPmHash))
            {
                enemy.TakeDamage(AtkPower);
            }
            else
            {
                TakeDamage(enemy.AtkPower);
            }
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
        if (mozog && Mathf.Abs(rb.velocity.x) < 5 && Mathf.Abs(rb.velocity.z) < 5 || (running && rb.velocity.x < 15 && rb.velocity.z < 15))
        {
            var pos = transform.position;
            rb.AddRelativeForce(mPos.x * 25, 0, mPos.y * 25);
            mapImgPos.anchoredPosition = new Vector2(-pos.x, -pos.z) * 0.5f;
        }
    }

    private void LateUpdate()
    {
        var tf = transform;
        mainCam.localPosition = tf.position + new Vector3(0, 3, -5);
        mainCam.eulerAngles = tf.eulerAngles + new Vector3(0, -30, 0) * Mathf.Sign(tf.eulerAngles.y);
        fpsText.text = "FPS: " + Time.captureFramerate;
    }

    private void Start()
    {
        moveSpeedId = Animator.StringToHash("moveSpeed");
        mainCam = Camera.main.transform;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }


}
