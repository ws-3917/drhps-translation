using System.Collections;
using UnityEngine;

public class Trailer_WorldCreation_TileFlicker : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Sprite[] _GlitchSprites;

    [SerializeField]
    private Sprite TargetSprite;

    [SerializeField]
    private int GlitchAmount;

    [SerializeField]
    private float GlitchInterval;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
        TargetSprite = _renderer.sprite;
        GlitchAmount = Random.Range(1, 10);
        GlitchInterval = Random.Range(0.005f, 0.025f);
    }

    public void StartGlitching()
    {
        _renderer.enabled = true;
        StartCoroutine(GlitchLoop());
    }

    private IEnumerator GlitchLoop()
    {
        for (int i = 0; i < GlitchAmount; i++)
        {
            _renderer.color = Random.ColorHSV();
            _renderer.sprite = _GlitchSprites[Random.Range(0, _GlitchSprites.Length)];
            yield return new WaitForSeconds(GlitchInterval);
        }
        _renderer.color = Color.white;
        _renderer.sprite = TargetSprite;
    }
}
