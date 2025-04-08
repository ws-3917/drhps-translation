using System.Collections;
using UnityEngine;

public class CH3Opening_CrowdMember : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer Renderer;

    [SerializeField]
    private Sprite[] PossibleSprites;

    private Vector2 StartPos;

    private Vector2 TargetPos;

    private void Start()
    {
        StartPos = base.transform.position;
        Renderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
        StartCoroutine(CrowdLoop());
    }

    private void Update()
    {
        base.transform.position = Vector2.Lerp(base.transform.position, TargetPos, 0.5f * Time.fixedDeltaTime);
    }

    private IEnumerator CrowdLoop()
    {
        TargetPos = Random.insideUnitCircle + StartPos;
        Renderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
        if (Random.Range(0, 2) == 0)
        {
            Renderer.flipX = true;
        }
        else
        {
            Renderer.flipX = false;
        }
        base.transform.position = StartPos;
        StartCoroutine(FadeTo(1f, Random.Range(0.3f, 0.5f)));
        while (Renderer.color.a != 1f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(Random.Range(2, 4));
        StartCoroutine(FadeTo(0f, Random.Range(0.3f, 0.5f)));
        while (Renderer.color.a != 0f)
        {
            yield return null;
        }
        StartCoroutine(CrowdLoop());
    }

    public IEnumerator FadeTo(float targetOpacity, float duration)
    {
        Color currentColor = Renderer.color;
        float startOpacity = currentColor.a;
        float opacityDifference = targetOpacity - startOpacity;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float a = Mathf.Clamp01(startOpacity + opacityDifference * (elapsedTime / duration));
            Renderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, a);
            yield return null;
        }
        Renderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);
    }
}
