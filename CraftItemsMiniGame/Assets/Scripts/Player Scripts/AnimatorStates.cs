using UnityEngine;

public static class AnimatorStates
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Interact = Animator.StringToHash("Interacting");
    public static readonly int PickUp = Animator.StringToHash("Lifting");
    public static readonly int ShakeNo = Animator.StringToHash("ShakingNo");
    public static readonly int Surfing = Animator.StringToHash("Surfing");
}