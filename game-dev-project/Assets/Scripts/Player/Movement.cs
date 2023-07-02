using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Movement : MonoBehaviour
{
    public Action onJump;
    public Action onStartClimb;
    public Action onStopClimb;
    public Action onCatch;
    public Action onSlide;
    public Action onEscPressed;
    public PlayerInput PlayerInput => playerInput;
    
    [Header("Movement")] 
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float slideForce = 1f;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private bool canCarry = false;
    
    [SerializeField] private Collider2D collider;

    [Header("Rotate ability")] 
    [SerializeField] private float rotateSpeed;
    
    private Rigidbody2D rb = null;
    private Animator animator = null;

    private float horizontal;
    private float vertical;
    private float beginGravity;

    private bool isNearbyLadder = false;
    private bool isClimbing = false;
    private bool isSliding = false;

    private bool isNextToBox = false;
    private bool isCatchingBox = false;
    private bool isCatchingMirror = false;
    private Box caughtBox = null;
    private Mirror caughtMirror = null;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction catchAction;
    private InputAction escAction;
    private InputAction scrollAction;
    private Box boxInRange;
    private bool isOnPlatform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        beginGravity = rb.gravityScale;
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        catchAction = playerInput.actions["Catch"];
        escAction = playerInput.actions["Esc"];
        scrollAction = playerInput.actions["Rotate"];

        jumpAction.performed += HandleJump;
        catchAction.performed += HandleCatch;
        escAction.performed += HandleEsc;
        scrollAction.performed += RotateMirror;
    }

    private void RotateMirror(InputAction.CallbackContext obj)
    {
        if (isCatchingMirror)
        {
            float scrollValue = scrollAction.ReadValue<float>();

            if (scrollValue > 0)
            {
                caughtMirror.Rotate(rotateSpeed);
            }
            else
            {
                caughtMirror.Rotate(-rotateSpeed);
            }
        }
    }


    private void OnDestroy()
    {
        jumpAction.performed -= HandleJump;
        catchAction.performed -= HandleCatch;
        escAction.performed -= HandleEsc;
    }

    private void HandleEsc(InputAction.CallbackContext context)
    {
        onEscPressed?.Invoke();
    }

    private void Update()
    {
        GetInput();
        MoveOnLadder();
        RunAnimation();
        HandleRun();
        CarryBox();
    }

    public void ForceStopPlayer()
    {
        rb.velocity = Vector2.zero;
        enabled = false;
        animator.SetBool("isMoving", false);
    }

    public void ForceStartPlayer()
    {
        enabled = true;
        animator.SetBool("isMoving", true);
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        if (CheckGrounded())
        {
            onJump?.Invoke();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void HandleCatch(InputAction.CallbackContext context)
    {
        if (canCarry)
        {
            if (isNextToBox && !isCatchingBox && boxInRange)
            {
                caughtBox = boxInRange;
                
                Mirror mirror = caughtBox.GetComponent<Mirror>();
              
                if (mirror)
                {
                    caughtMirror = mirror;
                    isCatchingMirror = true;
                }
                
                onCatch?.Invoke();
                isCatchingBox = true;
            }
            else if (isCatchingBox)
            {
                onCatch?.Invoke();
                isCatchingMirror = false;
                isCatchingBox = false;
                caughtBox = null;
                caughtMirror = null;
            }
        }
    }

    private void CarryBox()
    {
        if (isCatchingBox && caughtBox != null)
        {
            if (transform.eulerAngles.y == 180f)
            {
                if (isCatchingMirror)
                {
                    caughtBox.transform.position = transform.position + new Vector3(0.7f, 0f, 0f);
                }
                else
                {
                    caughtBox.transform.position = transform.position + new Vector3(1f, 0f, 0f);
                }
                
            }
            else
            {
                if (isCatchingMirror)
                {
                    caughtBox.transform.position = transform.position + new Vector3(-0.7f, 0f, 0f);
                }
                else
                {
                    caughtBox.transform.position = transform.position + new Vector3(-1f, 0f, 0f);
                }
            }
        }

    }

    private bool CheckGrounded()
    {

        Bounds bounds = collider.bounds;
        Vector2 bottomCenter = new Vector2(bounds.center.x, bounds.min.y);
        Vector2 bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        Vector2 bottomRight = new Vector2(bounds.max.x, bounds.min.y);

        RaycastHit2D hitCenter = Physics2D.Raycast(bottomCenter, -Vector2.up, groundDistance);
        RaycastHit2D hitLeft = Physics2D.Raycast(bottomLeft, -Vector2.up, groundDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(bottomRight, -Vector2.up, groundDistance);

        return hitLeft.collider != null || hitCenter.collider != null || hitRight.collider != null;

    }
    
    private void MoveOnLadder()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = beginGravity;
        }
    }


    private void GetInput() 
    {
        if (isNearbyLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void HandleRun()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        horizontal = input.x;
        vertical = input.y;
       // TO SMOOTH VELOCITY: currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
       if (isSliding)
        {
            rb.AddForce(new Vector2(horizontal * slideForce, 0f));
        }
        else
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (horizontal > 0 && transform.rotation.y == 0)
            transform.rotation = Quaternion.Euler(0, 180f, 0);

        else if (horizontal < 0 && transform.rotation.y != 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    private void RunAnimation()
    {
        if (horizontal != 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        boxInRange = col.GetComponent<Box>();
        
        if (boxInRange) {
            isNextToBox = true;
        }
        
        Ice ice = col.GetComponent<Ice>();
        if (ice)
        {
            onSlide?.Invoke();
            isSliding = true;
        }

        Ladder ladder = col.GetComponent<Ladder>();
        if (ladder)
        {
            onStartClimb?.Invoke();
            isNearbyLadder = true;
        }

        if (!isOnPlatform) 
        {
            Platform platform = col.GetComponent<Platform>();

            if (platform)
            {
                isOnPlatform = true;
                gameObject.transform.SetParent(platform.transform);
            }
        }
        else
        {
            gameObject.transform.SetParent(null);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        boxInRange = other.GetComponent<Box>();
        
        if (boxInRange)
        {
            isNextToBox = false;
        }
        
        Ice ice = other.GetComponent<Ice>();
        if (ice)
        {
            isSliding = false;
        }
        Ladder ladder = other.GetComponent<Ladder>();
        
        if (ladder)
        {
            onStopClimb?.Invoke();
            isNearbyLadder = false;
            isClimbing = false;
        }
        
        Platform platform = other.GetComponent<Platform>();

        if (platform)
        {
            isOnPlatform = false;
            gameObject.transform.SetParent(null);
        }
    }
}
