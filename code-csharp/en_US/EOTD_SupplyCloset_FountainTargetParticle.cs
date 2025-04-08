using System.Collections;
using UnityEngine;

public class EOTD_SupplyCloset_FountainTargetParticle : MonoBehaviour
{
    private SpriteRenderer thisRenderer;

    [SerializeField]
    private Sprite GlowSprite;

    private void Awake()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LifeSpan());
    }

    private IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(0.08f);
        thisRenderer.sprite = GlowSprite;
        Object.Destroy(base.gameObject, 0.08f);
    }
}
