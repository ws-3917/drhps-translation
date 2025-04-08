using System.Collections;
using UnityEngine;

public class BattleAttack_Siplett_Splash : MonoBehaviour
{
    public bool EnableRandomFlipVarient = true;

    public bool EnableDelayedTimer;

    [SerializeField]
    private GameObject Droplet;

    [SerializeField]
    private Transform DropletSpawnPosition;

    [SerializeField]
    private float DropletSpawnRadius;

    [SerializeField]
    private Transform DropletEndArcPosition;

    [SerializeField]
    private Animator CupAnimator;

    [SerializeField]
    private AudioClip[] SplashSounds;

    [SerializeField]
    private SpriteRenderer CupRenderer;

    [SerializeField]
    private SpriteRenderer LiquidRenderer;

    [SerializeField]
    private int DropletsPerSplash = 5;

    [SerializeField]
    private int AmountOfSplashes = 5;

    [SerializeField]
    private float AttackTime = 7.5f;

    private bool HasRanGuaranteedDroplet;

    private void Awake()
    {
        StartCoroutine(SplashLoop());
        if (Random.Range(0, 2) == 0 && EnableRandomFlipVarient)
        {
            base.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        CupRenderer.color = new Color(1f, 1f, 1f, 0f);
        LiquidRenderer.color = new Color(1f, 1f, 1f, 0f);
        CutsceneUtils.FadeInSprite(CupRenderer);
        CutsceneUtils.FadeInSprite(LiquidRenderer);
    }

    private IEnumerator SplashLoop()
    {
        yield return new WaitForSeconds(0.25f);
        int remainingSplashes = AmountOfSplashes;
        if (EnableDelayedTimer)
        {
            yield return new WaitForSeconds(AttackTime / (float)AmountOfSplashes / 2f);
        }
        while (remainingSplashes > 1)
        {
            yield return null;
            StartCoroutine(LaunchDroplet());
            yield return new WaitForSeconds(AttackTime / (float)AmountOfSplashes);
            remainingSplashes--;
            HasRanGuaranteedDroplet = false;
        }
    }

    private IEnumerator LaunchDroplet()
    {
        CupAnimator.Play("Siplett_BattleAttack_CupSplash", -1, 0f);
        yield return new WaitForSeconds(0.24150002f);
        BattleSystem.PlayBattleSoundEffect(SplashSounds[Random.Range(0, SplashSounds.Length)], Random.Range(0.55f, 0.7f), Random.Range(0.75f, 1.25f));
        for (int i = 0; i < DropletsPerSplash; i++)
        {
            Vector2 vector = (Vector2)DropletSpawnPosition.position + Random.insideUnitCircle * DropletSpawnRadius;
            GameObject gameObject = Object.Instantiate(Droplet, vector, Quaternion.identity, BattleSystem.Instance.Holder_Bullets.transform);
            gameObject.transform.localScale = Vector3.one * Random.Range(0.85f, 1f);
            if (!HasRanGuaranteedDroplet)
            {
                HasRanGuaranteedDroplet = true;
                gameObject.name = "dadada";
                float num = Mathf.Abs(BattleSystem.Instance.BattleBox.transform.position.x - Battle_PlayerSoul.Instance.transform.position.x) / 2f;
                gameObject.GetComponent<BattleAttack_Siplett_DropletArc>().JumpEndPos = new Vector2(Battle_PlayerSoul.Instance.transform.position.x - num, DropletEndArcPosition.position.y);
                gameObject.GetComponent<BattleAttack_Siplett_DropletArc>().DisableRandomOffsetMarkiplier = true;
            }
            else
            {
                gameObject.GetComponent<BattleAttack_Siplett_DropletArc>().JumpEndPos = DropletEndArcPosition.position;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (DropletSpawnPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(DropletSpawnPosition.position, DropletSpawnRadius);
        }
    }
}
