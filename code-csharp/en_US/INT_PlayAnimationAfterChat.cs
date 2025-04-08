using UnityEngine;

public class INT_PlayAnimationAfterChat : MonoBehaviour
{
    public INT_Chat Chat;

    public Animator Animator;

    public AnimationClip ClipToPlay;

    public int ChatIndexToPlay;

    public bool PlayedAnimation;

    private void Update()
    {
        if (Chat.FinishedText && Chat.CurrentIndex == ChatIndexToPlay && !PlayedAnimation)
        {
            PlayedAnimation = true;
            Animator.Play(ClipToPlay.name);
        }
    }
}
