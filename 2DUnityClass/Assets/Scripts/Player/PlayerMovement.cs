using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] PlayerAnimationController animController;
    [SerializeField] SpriteRenderer spRender;

    Controlls controlls;
    Controlls.OnFootActions footActions;

    [SerializeField] float velocity;
    [SerializeField] float sprintVelocity;
    [SerializeField] float jumpForce;
    [SerializeField] float startRay;
    [SerializeField] float groundDistance;

    [SerializeField] float MoveX;

    bool running;
    bool jumped;
    [SerializeField] bool onGround;

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
        spRender.flipX = (MoveX == 0) ? spRender.flipX : spRender.flipX = (MoveX < 0) ? true : false;
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