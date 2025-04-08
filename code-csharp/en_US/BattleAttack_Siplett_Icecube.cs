using UnityEngine;

public class BattleAttack_Siplett_Icecube : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve fallSpeedCurve;

    [SerializeField]
    private float curveDuration = 3f;

    [SerializeField]
    private Sprite[] PossibleIceSprites;

    [SerializeField]
    private Sprite RareIceeIcecube;

    [SerializeField]
    private float iceRotateRange = 3f;

    [SerializeField]
    private GameObject Prefab_Kaplunk;

    [SerializeField]
    private SpriteRenderer thisRenderer;

    [SerializeField]
    private AudioClip KaplunkSFX;

    private float elapsedTime;

    private void Awake()
    {
        int num = Random.Range(0, 100);
        MonoBehaviour.print("Dice Equal = " + num);
        if (num == 66)
        {
            thisRenderer.sprite = RareIceeIcecube;
        }
        else
        {
            thisRenderer.sprite = PossibleIceSprites[Random.Range(0, PossibleIceSprites.Length)];
        }
        base.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
        iceRotateRange = Random.Range(0f - iceRotateRange, iceRotateRange);
        if (Random.Range(0, 2) == 0)
        {
            thisRenderer.flipX = true;
        }
        if (Random.Range(0, 2) == 0)
        {
            thisRenderer.flipY = true;
        }
    }

    private void Start()
    {
        CutsceneUtils.FadeInSprite(thisRenderer);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float num = fallSpeedCurve.Evaluate(elapsedTime / curveDuration);
        base.transform.position = new Vector2(base.transform.position.x, base.transform.position.y - num * Time.deltaTime);
        base.transform.rotation = Quaternion.Euler(0f, 0f, base.transform.rotation.eulerAngles.z + iceRotateRange * Time.deltaTime);
        if (BattleAttack_Siplett_Kaplunk.instance != null && base.transform.position.y < BattleAttack_Siplett_Kaplunk.instance.LiquidBack_Renderer.transform.position.y)
        {
            Object.Instantiate(Prefab_Kaplunk, BattleSystem.Instance.Holder_Bullets.transform).transform.position = base.transform.position;
            BattleSystem.PlayBattleSoundEffect(KaplunkSFX, 1f, Random.Range(0.85f, 1.15f));
            Object.Destroy(base.gameObject);
        }
        if (elapsedTime > curveDuration)
        {
            Object.Destroy(base.gameObject);
        }
    }
}
