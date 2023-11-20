using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator AnimatorRef;

    public AnimationState currentState;

    // Start is called before the first frame update
    void Start()
    {
        AnimatorRef = GetComponent<Animator>();
        currentState = new AnimationState() { MovementState = State.IDLE };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimations();
    }


    public void ChangeState(AnimationState state)
    {
        if (currentState.Compare(state))
        {
            currentState = state;
        }
    }


    public void UpdateState()
    {

    }

    public void UpdateAnimations()
    {
        ValidadeState(currentState);
    }
    public void ValidadeState(AnimationState newState)
    {
        bool isWalking = currentState.MovementState == State.WALK;
        bool isRunning = currentState.MovementState == State.RUN;
        bool jump = currentState.MovementState == State.JUMP;

        isWalking = (isRunning) ? isRunning : isWalking;

        AnimatorRef.SetBool("IsWalking", isWalking);
        AnimatorRef.SetBool("IsRunning", isRunning);
        AnimatorRef.SetBool("Jumping", jump);
    }

    public void SetLayerWeight(float weight)
    {
        AnimatorRef.SetLayerWeight(1, weight);
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
