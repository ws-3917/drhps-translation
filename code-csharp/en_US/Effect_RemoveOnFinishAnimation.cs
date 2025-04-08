using UnityEngine;

public class Effect_RemoveOnFinishAnimation : MonoBehaviour
{
    [Header("- References -")]
    [SerializeField]
    private Animator targetAnimator;

    private void Update()
    {
        if (targetAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            Object.Destroy(base.gameObject);
        }
    }
}
