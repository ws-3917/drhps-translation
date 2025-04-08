using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AprilFools_Longshot : MonoBehaviour
{
    [Header("-= Longshot Stats =-")]
    public int HP = 60;

    public float MovementSpeed = 4f;

    public bool CanWalkToPos;

    public bool Damageable = true;

    private bool Attacking;

    [SerializeField]
    private Transform[] RandomWalkPositions;

    [SerializeField]
    private Animator LongshotAnimator;

    [SerializeField]
    private AprilFools_PlayerController Player;

    [SerializeField]
    private AudioClip SFX_LongshotDamaged;

    private Vector3 LastFramePos;

    [Header("-= UI =-")]
    [SerializeField]
    private Image LongshotHPBar;

    [SerializeField]
    private Image LongshotHPTransition;

    [Header("-= Attacks =-")]
    [SerializeField]
    private GameObject LongshotAttackPrefab_Rocket;

    [SerializeField]
    private GameObject LongshotAttackPrefab_Dynamite;

    [SerializeField]
    private GameObject LongshotAttackPrefab_RocketHoming;

    [SerializeField]
    private List<Transform> RandomDynamiteDropPositions = new List<Transform>();

    [Header("-= Sounds =-")]
    [SerializeField]
    private AudioClip SFX_Jump;

    [SerializeField]
    private AudioClip SFX_ThrowDynamite;

    [SerializeField]
    private AudioClip SFX_DoubleMissile;

    [SerializeField]
    private AudioClip SFX_LongshotPissed;

    [SerializeField]
    private AudioClip SFX_LongshotLaugh;

    private void Start()
    {
        StartCoroutine(RandomWalkLoop());
    }

    private void Update()
    {
        LongshotAnimator.SetFloat("VelocityMagnitude", Vector3.Distance(base.transform.position, LastFramePos));
        if (base.transform.position.x > Player.transform.position.x)
        {
            LongshotAnimator.SetFloat("VelocityX", 1f);
        }
        else
        {
            LongshotAnimator.SetFloat("VelocityX", -1f);
        }
        LastFramePos = base.transform.position;
        LongshotHPBar.fillAmount = Mathf.Lerp(LongshotHPBar.fillAmount, (float)HP / 60f, 6f * Time.deltaTime);
        LongshotHPTransition.fillAmount = Mathf.Lerp(LongshotHPTransition.fillAmount, (float)HP / 60f, 2.5f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(LongshotAttack_JumpRocket());
        }
    }

    public void DamageLongshot()
    {
        float num = HP;
        if (Damageable)
        {
            CutsceneUtils.PlaySound(SFX_LongshotDamaged, CutsceneUtils.DRH_MixerChannels.Dialogue, 0.75f);
            HP--;
            MovementSpeed += 0.25f;
            if (num == 30f && HP < 30)
            {
                StartCoroutine(LongshotPhase2());
            }
            if (num == 1f && HP < 1)
            {
                StartCoroutine(DelayEndingFight());
            }
            StartCoroutine(DamageAnim());
        }
    }

    private IEnumerator DelayEndingFight()
    {
        yield return new WaitForSeconds(0.1f);
        AprilFools_Manager.instance.EndFight();
    }

    public void BeginLongshotAI()
    {
        StartCoroutine(AttackLoop());
        CanWalkToPos = true;
    }

    private IEnumerator DamageAnim()
    {
        CanWalkToPos = false;
        LongshotAnimator.Play("Shock");
        yield return new WaitForSeconds(0.25f);
        LongshotAnimator.Play("Idle");
        CanWalkToPos = true;
    }

    private IEnumerator LongshotPhase2()
    {
        Damageable = false;
        CanWalkToPos = false;
        Attacking = true;
        LongshotAnimator.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4575472f, 0.4575472f);
        MusicManager.PauseMusic();
        LongshotAnimator.speed = 4f;
        LongshotAnimator.SetBool("InCutscene", value: true);
        LongshotAnimator.Play("Walk");
        CutsceneUtils.instance.UtilSource.pitch = 1f;
        CutsceneUtils.PlaySound(SFX_LongshotPissed, CutsceneUtils.DRH_MixerChannels.Effect, 0.45f);
        CutsceneUtils.PlaySound(SFX_LongshotLaugh, CutsceneUtils.DRH_MixerChannels.Dialogue, 0.75f);
        AprilFools_Manager.instance.LockIn();
        yield return new WaitForSeconds(2f);
        LongshotAnimator.speed = 1f;
        LongshotAnimator.SetBool("InCutscene", value: false);
        MusicManager.Instance.source.pitch = 1.15f;
        MusicManager.Instance.source.Play();
        Damageable = true;
        CanWalkToPos = true;
        Attacking = false;
    }

    private IEnumerator RandomWalkLoop()
    {
        yield return new WaitForSeconds(Random.Range(0.35f, 1f));
        Vector3 randomPos = RandomWalkPositions[Random.Range(0, RandomWalkPositions.Length)].position;
        while (Vector3.Distance(base.transform.position, randomPos) > 0.1f)
        {
            yield return null;
            if (CanWalkToPos)
            {
                base.transform.position = Vector3.MoveTowards(base.transform.position, randomPos, MovementSpeed * Time.deltaTime);
            }
        }
        StartCoroutine(RandomWalkLoop());
    }

    private IEnumerator AttackLoop()
    {
        float t = (float)HP / 60f;
        float minInclusive = Mathf.Lerp(0.25f, 1f, t);
        float maxInclusive = Mathf.Lerp(0.5f, 2f, t);
        yield return new WaitForSeconds(Random.Range(minInclusive, maxInclusive));
        Attacking = true;
        if (Damageable)
        {
            PlayRandomAttack();
        }
        else
        {
            Attacking = false;
        }
        while (Attacking)
        {
            yield return null;
        }
        StartCoroutine(AttackLoop());
    }

    private void PlayRandomAttack()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                Attacking = false;
                break;
            case 1:
                StartCoroutine(LongshotAttack_JumpRocket());
                break;
            case 2:
                StartCoroutine(LongshotAttack_DynamiteSpread());
                break;
            case 3:
                StartCoroutine(LongshotAttack_JumpRocketHoming());
                break;
            default:
                Attacking = false;
                break;
        }
    }

    private IEnumerator LongshotAttack_JumpRocket()
    {
        CanWalkToPos = false;
        Damageable = false;
        CutsceneUtils.MoveTransformOnArc(base.transform, base.transform.position, 4f, 0.5f);
        CutsceneUtils.PlaySound(SFX_Jump, CutsceneUtils.DRH_MixerChannels.Effect, 0.35f, Random.Range(0.9f, 1.1f));
        LongshotAnimator.Play("AprilFools25_Jump");
        yield return new WaitForSeconds(0.25f);
        Object.Instantiate(LongshotAttackPrefab_Rocket, base.transform.position, Quaternion.identity).transform.LookAt(Player.transform.position + Vector3.down * 0.75f);
        Damageable = true;
        yield return new WaitForSeconds(0.25f);
        LongshotAnimator.Play("Idle");
        yield return new WaitForSeconds(0.5f);
        CanWalkToPos = true;
        Attacking = false;
    }

    private IEnumerator LongshotAttack_DynamiteSpread()
    {
        CanWalkToPos = false;
        Damageable = true;
        List<Transform> PossibleDropLocations = new List<Transform>(RandomDynamiteDropPositions);
        for (int i = 0; i < 5 - HP * 5 / 60; i++)
        {
            LongshotAnimator.Play("AprilFools25_FireDynamite", -1, 0f);
            CutsceneUtils.PlaySound(SFX_ThrowDynamite, CutsceneUtils.DRH_MixerChannels.Effect, 0.25f, Random.Range(0.9f, 1.1f));
            Damageable = true;
            GameObject obj = Object.Instantiate(LongshotAttackPrefab_Dynamite, base.transform.position, Quaternion.identity);
            Transform transform = PossibleDropLocations[Random.Range(0, PossibleDropLocations.Count)];
            PossibleDropLocations.Remove(transform);
            CutsceneUtils.MoveTransformOnArc(obj.transform, transform.position, Random.Range(5, 8), Random.Range(2, 3));
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        Damageable = true;
        CanWalkToPos = true;
        Attacking = false;
    }

    private IEnumerator LongshotAttack_JumpRocketHoming()
    {
        CanWalkToPos = false;
        Damageable = false;
        CutsceneUtils.MoveTransformOnArc(base.transform, base.transform.position, 4f, 0.5f);
        LongshotAnimator.Play("AprilFools25_FireTwoMissiles");
        yield return new WaitForSeconds(0.25f);
        GameObject obj = Object.Instantiate(LongshotAttackPrefab_RocketHoming, base.transform.position + Vector3.up, Quaternion.identity);
        CutsceneUtils.PlaySound(SFX_DoubleMissile, CutsceneUtils.DRH_MixerChannels.Effect, 0.35f, Random.Range(0.9f, 1.1f));
        obj.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        yield return new WaitForSeconds(0.25f);
        GameObject obj2 = Object.Instantiate(LongshotAttackPrefab_RocketHoming, base.transform.position + Vector3.up, Quaternion.identity);
        CutsceneUtils.PlaySound(SFX_DoubleMissile, CutsceneUtils.DRH_MixerChannels.Effect, 0.35f, Random.Range(0.9f, 1.1f));
        obj2.transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        Damageable = true;
        yield return new WaitForSeconds(0.5f);
        LongshotAnimator.Play("Idle");
        yield return new WaitForSeconds(0.25f);
        CanWalkToPos = true;
        Attacking = false;
    }
}
