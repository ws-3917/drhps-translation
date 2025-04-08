using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScrollingRawImage : MonoBehaviour
{
    [SerializeField]
    private RawImage _img;

    [SerializeField]
    private float _x;

    [SerializeField]
    private float _y;

    [SerializeField]
    private bool AffectedBySimplifyVFX;

    [Header("Random Controls")]
    [SerializeField]
    private bool SlowlyRandom;

    [SerializeField]
    private float _xMin;

    [SerializeField]
    private float _yMin;

    [SerializeField]
    private float _xMax;

    [SerializeField]
    private float _yMax;

    [SerializeField]
    private float RandomSpeed;

    private bool simplifyvfx;

    private void Awake()
    {
        if (SlowlyRandom)
        {
            StartCoroutine(RandomChange());
        }
        if (AffectedBySimplifyVFX)
        {
            StartCoroutine(SettingsCheckTick());
        }
    }

    private void Update()
    {
        if (!simplifyvfx)
        {
            _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.unscaledDeltaTime, _img.uvRect.size);
        }
    }

    private IEnumerator RandomChange()
    {
        if (!simplifyvfx)
        {
            _x = Mathf.Lerp(_x, Random.Range(_xMin, _xMax), RandomSpeed * Time.unscaledDeltaTime);
            _y = Mathf.Lerp(_y, Random.Range(_yMin, _yMax), RandomSpeed * Time.unscaledDeltaTime);
        }
        yield return new WaitForSecondsRealtime(0.25f);
        StartCoroutine(RandomChange());
    }

    private IEnumerator SettingsCheckTick()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        simplifyvfx = SettingsManager.Instance.GetBoolSettingValue("SimpleVFX");
        StartCoroutine(SettingsCheckTick());
    }
}
