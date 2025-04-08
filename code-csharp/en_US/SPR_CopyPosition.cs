using UnityEngine;

public class SPR_CopyPosition : MonoBehaviour
{
    [Header("-- Variables --")]
    [SerializeField]
    private Transform Target;

    [SerializeField]
    private Vector3 Offset;

    [SerializeField]
    private bool Enabled = true;

    [Header("-- Player Specific --")]
    [SerializeField]
    private bool TargetPlayer;

    private void LateUpdate()
    {
        if (Enabled)
        {
            if (!TargetPlayer && Target != null)
            {
                base.transform.position = Target.position + Offset;
            }
            else if (PlayerManager.Instance != null)
            {
                base.transform.position = PlayerManager.Instance.transform.position + Offset;
            }
        }
    }
}
