using UnityEngine;

public class Enemy_TargetSelectionGlow : MonoBehaviour
{
    public bool CurrentlyTargetted;

    [SerializeField]
    private bool PreviousTarget;

    [SerializeField]
    private Animator GlowAnimator;

    private void Update()
    {
        if (PreviousTarget != CurrentlyTargetted)
        {
            PreviousTarget = CurrentlyTargetted;
            GlowAnimator.SetBool("Targetted", CurrentlyTargetted);
        }
    }

    public void PlayGlowSpecializedAnimation(string anim)
    {
        GlowAnimator.Play(anim);
    }
}
