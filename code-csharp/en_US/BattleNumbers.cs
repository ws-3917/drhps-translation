using System.Collections;
using UnityEngine;

public class BattleNumbers : MonoBehaviour
{
    private void Awake()
    {
        Object.Destroy(base.gameObject, 2.5f);
    }

    public void BeginFadingNumbers()
    {
        Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            SpriteRenderer component = componentsInChildren[i].GetComponent<SpriteRenderer>();
            if (component != null)
            {
                StartCoroutine(FadeOut(component));
            }
        }
    }

    private IEnumerator FadeOut(SpriteRenderer spriteRenderer)
    {
        float duration = 1f;
        float elapsed = 0f;
        Color originalColor = spriteRenderer.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, elapsed / duration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, a);
            yield return null;
        }
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
