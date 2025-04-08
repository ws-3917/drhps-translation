using UnityEngine;

public class SPR_FaceTarget : MonoBehaviour
{
    public bool Active;

    public bool TargetPlayer;

    [SerializeField]
    private Transform Target;

    [SerializeField]
    private Animator TargetAnimator;

    [SerializeField]
    private string DirectionParameter_X;

    [SerializeField]
    private string DirectionParameter_Y;

    private void Update()
    {
        if (Active)
        {
            Vector2 vector = Vector3.zero;
            vector = (TargetPlayer ? ((Vector2)(PlayerManager.Instance.transform.position - base.transform.position)) : ((Vector2)(Target.position - base.transform.position)));
            TargetAnimator.SetFloat(DirectionParameter_X, vector.x);
            TargetAnimator.SetFloat(DirectionParameter_Y, vector.y);
        }
    }
}
