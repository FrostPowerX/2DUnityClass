using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerAnimationController animController;

    Controlls controlls;
    Controlls.OnFootActions footActions;


    [Header("Properties")]
    [SerializeField] float velocity;
    [SerializeField] float sprintVelocity;
    [SerializeField] float jumpForce;
    [SerializeField][Range(-1,0)] float startRay;
    [SerializeField][Range(0.1f,1)] float groundDistance;

    [Header("Debug")]
    [SerializeField] float MoveX;

    [SerializeField] bool running;
    [SerializeField] bool jumped;
    [SerializeField] bool onGround;
    [SerializeField] bool changeLook;

    private void Awake()
    {
        controlls = new Controlls();
        footActions = controlls.OnFoot;
        footActions.Enable();
    }

    void Update()
    {
        ReadInputs();
        ChangeAnimation();
        ChangeLookSide();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
        GroundDetector();
    }

    void ChangeAnimation()
    {
        if (onGround)
        {      
            if (MoveX != 0)
            {
                if (running)
                {
                    animController.ChangeState(new AnimationState { MovementState = State.RUN });
                }
                else
                {
                    animController.ChangeState(new AnimationState { MovementState = State.WALK });
                }
            }
            else animController.ChangeState(new AnimationState { MovementState = State.IDLE });
        }
        if (!onGround || footActions.Jump.triggered)
        {
            animController.ChangeState(new AnimationState { MovementState = State.JUMP });
        }
    }

    void ChangeLookSide()
    {
        changeLook = (MoveX == 0) ? changeLook : changeLook = (MoveX < 0) ? true : false;
        animController.InvertImage(changeLook);
    }

    void Movement()
    {
        Vector2 directionForce = transform.right * MoveX;
        if (running)
        {
            rb.AddForce(directionForce * sprintVelocity);
        }
        else
        {
            rb.AddForce(directionForce * velocity);
        }
    }

    void Jump()
    {
        if (!jumped || !onGround) return;

        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void ReadInputs()
    {
        MoveX = footActions.Movement.ReadValue<float>();

        running = footActions.Sprint.IsPressed();

        jumped = footActions.Jump.IsPressed();
    }

    void GroundDetector()
    {
        Vector2 position = transform.position - (Vector3.down * startRay);

        Debug.DrawRay(position, Vector2.down * groundDistance);

        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundDistance);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Ground") onGround = true;
        }
        else onGround = false;
    }
}
