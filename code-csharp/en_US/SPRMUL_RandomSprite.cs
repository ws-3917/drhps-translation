using UnityEngine;

public class SPRMUL_RandomSprite : MonoBehaviour
{
    [Header("-= Sprite Settings =-")]
    public SpriteRenderer[] spriteRenderers;

    public Sprite[] sprites;

    [Header("-= Randomization Settings =-")]
    public bool setSeed;

    public int seed;

    private void Start()
    {
        AssignRandomSprites();
    }

    public void AssignRandomSprites()
    {
        if (sprites.Length == 0 || spriteRenderers.Length == 0)
        {
            Debug.LogWarning("Missing sprites or sprite renderers!");
            return;
        }
        if (setSeed)
        {
            Random.InitState(seed);
        }
        SpriteRenderer[] array = spriteRenderers;
        foreach (SpriteRenderer spriteRenderer in array)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }
    }
}
