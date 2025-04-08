using System.Collections.Generic;
using UnityEngine;

public class EOTDREWRITE_FountainColor : MonoBehaviour
{
    [Header("Settings")]
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    public Gradient gradient;

    public float speed = 1f;

    private float gradientTime;

    public List<float> originalDarknesses = new List<float>();

    private void Start()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                float grayscale = spriteRenderer.color.grayscale;
                originalDarknesses.Add(grayscale);
            }
            else
            {
                originalDarknesses.Add(1f);
            }
        }
    }

    private void LateUpdate()
    {
        gradientTime += speed * Time.deltaTime;
        if (gradientTime > 1f)
        {
            gradientTime -= 1f;
        }
        Color color = gradient.Evaluate(gradientTime);
        for (int i = 0; i < spriteRenderers.Count; i++)
        {
            SpriteRenderer spriteRenderer = spriteRenderers[i];
            if (spriteRenderer != null)
            {
                float num = originalDarknesses[i];
                Color color2 = new Color(color.r * num, color.g * num, color.b * num, spriteRenderer.color.a);
                spriteRenderer.color = color2;
            }
        }
    }
}
