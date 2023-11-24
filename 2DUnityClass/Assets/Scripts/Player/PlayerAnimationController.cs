using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator AnimatorRef;

    public AnimationState currentState;

    [SerializeField] SpriteRenderer spRender;

    void Start()
    {
        AnimatorRef = GetComponent<Animator>();
        currentState = new AnimationState() { MovementState = State.IDLE };
    }

    void Update()
    {
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        ValidadeState(currentState);
    }
    void ValidadeState(AnimationState newState)
    {
        bool isWalking = currentState.MovementState == State.WALK;
        bool isRunning = currentState.MovementState == State.RUN;
        bool jump = currentState.MovementState == State.JUMP;
        //               if         true        false
        isWalking = (isRunning) ? isRunning : isWalking;

        AnimatorRef.SetBool("IsWalking", isWalking);
        AnimatorRef.SetBool("IsRunning", isRunning);
        AnimatorRef.SetBool("Jumping", jump);
    }

    public void SetLayerWeight(float weight)
    {
        AnimatorRef.SetLayerWeight(1, weight);
    }
    public void ChangeState(AnimationState state)
    {
        if (currentState.Compare(state))
        {
            currentState = state;
        }
    }
    public void InvertImage(bool value)
    {
        spRender.flipX = value;
    }
}

public enum State
{
    WALK,
    RUN,
    IDLE,
    JUMP
}

public struct AnimationState
{
    public State MovementState;

    public bool Compare(AnimationState stateToCompare)
    {
        if (this.MovementState != stateToCompare.MovementState)
        {
            return true;
        }
        return false;
    }
}
