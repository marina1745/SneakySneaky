using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControls : MonoBehaviour
{

    //speed variables
    public float baseSpeed;
    public float sneakSpeedFactor;
    public float sprintSpeedFactor;
    public float currentSpeed;
    
    //input variables
    public Vector3 movement;
    public Vector3 moveDir;
    public Vector3 movementInput;
    public bool sprint, sneak, isMoving;
    public bool isGrounded;
    
    
    //turning speed
    private float rotationFactor = 15f;
    private float turnSmoothVelocity;
    
    //gravity and jumping variables 
    private float gravity = -10f;
    private float groundedGravity = -0.5f;
    private bool isJumpPressed = false;
    private float initialJumoVelocity;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 2f;
    private bool isJumping = false;
    
    //ground variable (use this to change footstep sounds, if we want to do that still)
    public string currentGround;
    
    
    
    
    //references
    private PlayerInputActions _controls;
    public CharacterController controller ;
    [SerializeField] private Transform cam;
    



    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //setting up inputs
        controller = GetComponent<CharacterController>();
        _controls = new PlayerInputActions();
        _controls.Enable();
        _controls.Mouse.Move.started += OnMove;
        _controls.Mouse.Move.performed += OnMove;
        _controls.Mouse.Move.canceled += OnMove;

        _controls.Mouse.Jump.performed += OnJump;
        _controls.Mouse.Jump.canceled += OnJump;

        _controls.Mouse.Sprint.performed += OnSprint;
        _controls.Mouse.Sprint.canceled += OnSprint;

        _controls.Mouse.Sneak.performed += OnSneak;
        _controls.Mouse.Sneak.canceled += OnSneak;

        currentSpeed = baseSpeed;
        SetupJumpVariables();

    }

    void SetupJumpVariables()
    {
        float timeToApex = maxJumpHeight / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumoVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movementInput.x = context.ReadValue<Vector2>().x;
        movementInput.z = context.ReadValue<Vector2>().y;
        isMoving = movementInput.x != 0 || movementInput.z != 0;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void OnSneak(InputAction.CallbackContext context)
    {
        sneak = context.ReadValueAsButton();
        if (sneak && !sprint)
        {
            currentSpeed = baseSpeed * sneakSpeedFactor;
        }
        else currentSpeed = baseSpeed;
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.ReadValueAsButton();
        if (sprint && !sneak)
        {
            currentSpeed = baseSpeed * sprintSpeedFactor;
        }
        else currentSpeed = baseSpeed;
    }
    

    private void OnDisable()
    {
        _controls.Disable();
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        CheckGround();
        HandleMovement();
        HandleRotation();
        HandleGravity();
        HandleJump();

    }


    void HandleMovement()
    {
        if (isMoving)
        {
            movement.x = moveDir.x * currentSpeed;
            movement.z = moveDir.z * currentSpeed;
        }
        else
        {
            movement.x = 0f;
            movement.z = 0f;
        }
        controller.Move(Time.deltaTime* movement);
    }
    

    void HandleRotation()
    {
        float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
            rotationFactor * Time.deltaTime);
        if (isMoving)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.Normalize();
        }
        
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
        {
            movement.y = groundedGravity;
        }
        else
        {
            movement.y += gravity * Time.deltaTime;
        }
    }

    void HandleJump()
    {
        if (!isJumping && controller.isGrounded && isJumpPressed)
        {
            isJumping = true;
            movement.y = initialJumoVelocity;
        }
        else if (!isJumpPressed && isJumping && controller.isGrounded)
        {
            isJumping = false;
        }
    }
    
    void CheckGround()
    {
        isGrounded = controller.isGrounded;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
        {
            if (!hit.transform.CompareTag("Untagged"))
            {
                currentGround = hit.transform.tag;
            }
            
        }
    }


    public float pushPower = 1.0f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.1f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }








}
