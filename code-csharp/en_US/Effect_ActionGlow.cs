using UnityEngine;

public class Effect_ActionGlow : MonoBehaviour
{
    [Header("- References -")]
    [SerializeField]
    private SpriteRenderer[] glowSprites;

    public void SetNewGlowSprite(Sprite sprite)
    {
        SpriteRenderer[] array = glowSprites;
        for (int i = 0; i < array.Length; i++)
        {
            array[i].sprite = sprite;
        }
    }
}
