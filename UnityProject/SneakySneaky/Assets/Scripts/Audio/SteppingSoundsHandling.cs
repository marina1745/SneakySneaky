using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class SteppingSoundsHandling : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> stepClips;
    public float pauseTimeSneak = 0.4f, pauseTimeWalking=0.3f, pauseTimeRunning=0.18f;
    private AudioSource source;
    private int nextClipToCall=0;
    private PlayerInputActions _controls;
    public CharacterController controller;
    public AudioClip jumpClip, landClip;

    bool sprint,moving;

    bool isJumpPressed = false, isJumping = false;
    private void OnEnable()
    {
        _controls = new PlayerInputActions();
        controller = GetComponent<CharacterController>();
        _controls.Enable();
        _controls.Mouse.Move.started += OnMove;
        _controls.Mouse.Move.performed += OnMove;
        _controls.Mouse.Move.canceled += OnMoveStopped;

        _controls.Mouse.Jump.performed+= OnJump;
        _controls.Mouse.Jump.canceled += OnJump;

        _controls.Mouse.Sprint.performed += OnSprint;
        _controls.Mouse.Sprint.canceled += OnSprint;

      

        
    }

   
    private void Start()
    {
        //Debug.Log(stepClips.Count);
        if(stepClips.Count==0)
        {
            Object[] files = Resources.LoadAll("Audio/Foley/Mouse/Steps",typeof(AudioClip));
            source = GetComponent<AudioSource>();
            foreach (Object f in files)
                stepClips.Add((AudioClip)f);
            stepClips.Sort((k,j) => k.name.CompareTo(j.name)); 
        }
    }

    private void Update()
    {
        HandleJump();
    }


    private void PlayStepClip()
    {

        source.PlayOneShot(stepClips[nextClipToCall]);
        if (nextClipToCall % 2 == 0)
            nextClipToCall++;
        else
            nextClipToCall = Random.Range(0, stepClips.Count / 2)*2;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        
        if (context.ReadValue<Vector2>().x != 0 || context.ReadValue<Vector2>().y != 0
            && !sprint && !moving)
        {
            CancelInvoke();
            InvokeRepeating("PlayStepClip", 0, pauseTimeWalking);
            moving = true;
            //Debug.Log("Moving activated");
            
        }
        moving = sprint ? false : context.ReadValue<Vector2>().x != 0 || context.ReadValue<Vector2>().y != 0;
    }
    private void OnMoveStopped(InputAction.CallbackContext context)
    {
        CancelInvoke();
        moving = false;
        //Debug.Log("Move stopped");
    }


    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        
    }

    private void HandleJump()
    {
        if (!isJumping && controller.isGrounded && isJumpPressed)
        {
            isJumping = true;
            source.PlayOneShot(jumpClip);
        }
        else if (!isJumpPressed && isJumping && controller.isGrounded)
        {
            isJumping = false;
            source.PlayOneShot(landClip);
        }
    }



    private void OnSprint(InputAction.CallbackContext context)
    {
        
        if (!sprint && context.ReadValueAsButton())
        {
            CancelInvoke();
            InvokeRepeating("PlayStepClip", 0, pauseTimeRunning);
            Debug.Log("sprint activated");
           
        }
        sprint=context.ReadValueAsButton();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
