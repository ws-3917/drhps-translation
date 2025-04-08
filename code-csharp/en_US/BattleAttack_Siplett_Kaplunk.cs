using System.Collections;
using UnityEngine;

public class BattleAttack_Siplett_Kaplunk : MonoBehaviour
{
    public SpriteRenderer LiquidFront_Renderer;

    public SpriteRenderer LiquidBack_Renderer;

    [SerializeField]
    private float AttackTime;

    [Space(10f)]
    [SerializeField]
    private float IceCubeSpawn_Y;

    [SerializeField]
    private float IceCubeSpawnRange_X;

    [SerializeField]
    private int IceCubesToSpawn = 15;

    [SerializeField]
    private GameObject IceCubePrefab;

    public static BattleAttack_Siplett_Kaplunk instance;

    private int spawnedIcecubes;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (LiquidFront_Renderer != null)
        {
            CutsceneUtils.FadeInSprite(LiquidFront_Renderer);
        }
        if (LiquidBack_Renderer != null)
        {
            CutsceneUtils.FadeInSprite(LiquidBack_Renderer);
        }
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        if (spawnedIcecubes < IceCubesToSpawn)
        {
            spawnedIcecubes++;
            yield return new WaitForSeconds(AttackTime / (float)(IceCubesToSpawn / 2));
            GameObject obj = Object.Instantiate(IceCubePrefab, BattleSystem.Instance.Holder_Bullets.transform);
            float x = Mathf.Clamp(Battle_PlayerSoul.Instance.transform.position.x + Random.Range(0f - IceCubeSpawnRange_X, IceCubeSpawnRange_X), base.transform.position.x + -1.35f, base.transform.position.x + 1.35f);
            obj.transform.parent = BattleSystem.Instance.Holder_Bullets.transform;
            obj.transform.position = new Vector2(x, base.transform.position.y + IceCubeSpawn_Y);
            StartCoroutine(AttackLoop());
        }
    }
}
