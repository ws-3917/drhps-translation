using UnityEngine;

public class SPR_CopyRenderer : MonoBehaviour
{
    [Header("-- Variables --")]
    [SerializeField]
    private SpriteRenderer Renderer;

    [SerializeField]
    private SpriteRenderer Target;

    [Space(10f)]
    [SerializeField]
    private bool Enabled = true;

    [Space(10f)]
    [Header("-- Optional --")]
    [SerializeField]
    private SpriteMask RenderMask;

    [Header("-- Player Specific --")]
    [SerializeField]
    private bool TargetPlayer;

    private SpriteRenderer StoredSpriteRenderer;

    private void LateUpdate()
    {
        if (!Enabled)
        {
            return;
        }
        if (!TargetPlayer)
        {
            if (Target != null)
            {
                if (Renderer != null)
                {
                    Renderer.sprite = Target.sprite;
                }
                if (RenderMask != null)
                {
                    RenderMask.sprite = Target.sprite;
                }
            }
        }
        else
        {
            if (!(PlayerManager.Instance != null))
            {
                return;
            }
            if (StoredSpriteRenderer == null)
            {
                StoredSpriteRenderer = PlayerManager.Instance._PAnimation.GetComponent<SpriteRenderer>();
                return;
            }
            Renderer.sprite = StoredSpriteRenderer.sprite;
            if (RenderMask != null)
            {
                RenderMask.sprite = StoredSpriteRenderer.sprite;
            }
        }
    }
}
