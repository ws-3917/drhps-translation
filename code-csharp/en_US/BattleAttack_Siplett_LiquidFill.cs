using System.Collections;
using UnityEngine;

public class BattleAttack_Siplett_LiquidFill : MonoBehaviour
{
    [SerializeField]
    private Transform Liquid;

    [SerializeField]
    private Vector3 StartingPosition;

    [SerializeField]
    private Vector3 EndingPosition;

    [SerializeField]
    private Vector3 StartingSize;

    [SerializeField]
    private Vector3 EndingSize;

    [SerializeField]
    private Transform BackgroundLiquid;

    [SerializeField]
    private Vector3 BackgroundStartingPosition;

    [SerializeField]
    private Vector3 BackgroundEndingPosition;

    [SerializeField]
    private Vector3 BackgroundStartingSize;

    [SerializeField]
    private Vector3 BackgroundEndingSize;

    [Space(10f)]
    [SerializeField]
    private Transform LiquidSurface;

    [Space(10f)]
    [SerializeField]
    private float LiquidRiseSpeed;

    [SerializeField]
    private float BackgroundLiquidRiseSpeed;

    private void Awake()
    {
        BackgroundLiquid.GetComponent<SpriteRenderer>().size = new Vector2(2.1f, 0f);
        StartingPosition += BattleSystem.Instance.transform.position;
        EndingPosition += BattleSystem.Instance.transform.position;
        BackgroundStartingPosition += BattleSystem.Instance.transform.position;
        BackgroundEndingPosition += BattleSystem.Instance.transform.position;
        LiquidSurface.position = Vector3.one * 500f;
        LiquidSurface.localScale = new Vector2(LiquidSurface.localScale.x, 0f);
        Liquid.position = StartingPosition;
        Liquid.localScale = StartingSize;
        BackgroundLiquid.position = BackgroundStartingPosition;
        BackgroundLiquid.localScale = BackgroundStartingSize;
        StartCoroutine(LiquidFill());
        StartCoroutine(BackgroundLiquidFill());
    }

    private IEnumerator LiquidFill()
    {
        LiquidSurface.position = Vector3.one * 500f;
        yield return new WaitForSeconds(0.25f);
        float progress = 0f;
        float duration = LiquidRiseSpeed;
        Vector3 startPosition = Liquid.position;
        Vector3 startScale = Liquid.localScale;
        while (progress < 1f)
        {
            progress += Time.deltaTime / duration;
            Liquid.position = Vector3.Lerp(startPosition, EndingPosition, progress);
            Liquid.localScale = Vector3.Lerp(startScale, EndingSize, progress);
            LiquidSurface.position = new Vector3(Liquid.position.x, Liquid.position.y + Liquid.localScale.y / 2f, Liquid.position.z);
            yield return null;
        }
        Liquid.position = EndingPosition;
        Liquid.localScale = EndingSize;
    }

    private IEnumerator BackgroundLiquidFill()
    {
        yield return new WaitForSeconds(0.25f);
        float progress = 0f;
        float duration = BackgroundLiquidRiseSpeed;
        Vector3 startPosition = BackgroundLiquid.position;
        _ = BackgroundLiquid.localScale;
        SpriteRenderer BackgroundSpriteRenderer = BackgroundLiquid.GetComponent<SpriteRenderer>();
        while (progress < 1f)
        {
            progress += Time.deltaTime / duration;
            BackgroundLiquid.position = Vector3.Lerp(startPosition, BackgroundEndingPosition, progress);
            BackgroundSpriteRenderer.size = new Vector2(2.1f, Mathf.Lerp(StartingSize.y, BackgroundEndingSize.y, progress));
            LiquidSurface.localScale = new Vector2(LiquidSurface.localScale.x, Mathf.Lerp(0f, 1f, progress));
            yield return null;
        }
        BackgroundLiquid.position = BackgroundEndingPosition;
    }
}
