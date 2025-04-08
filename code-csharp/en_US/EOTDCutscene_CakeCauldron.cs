using System.Collections;
using UnityEngine;

public class EOTDCutscene_CakeCauldron : MonoBehaviour
{
    public int CutsceneIndex;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private GameObject Explosion;

    [SerializeField]
    private GameObject Cake;

    [SerializeField]
    private SpriteRenderer CakeRenderer;

    [SerializeField]
    private GameObject Cake_Left;

    [SerializeField]
    private GameObject Cake_Right;

    [SerializeField]
    private GameObject CakeSplat;

    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private INT_Chat CauldronChatter;

    [SerializeField]
    private CHATBOXTEXT CutsceneChats;

    [SerializeField]
    private Animator SusieGlowAnimator;

    [SerializeField]
    private GameObject HealParticlePrefab;

    private GameObject storedparticle;

    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    private void Start()
    {
        if (PlayerPrefs.GetInt("EOTD_CakeCutscene", 0) == 0)
        {
            PlayerPrefs.SetInt("EOTD_CakeCutscene", 1);
        }
        else
        {
            CauldronChatter.CurrentIndex = 1;
        }
    }

    private void LateUpdate()
    {
        CakeSplat.transform.position = PlayerManager.Instance._PAnimation.transform.position;
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
        switch (CutsceneIndex)
        {
            case 2:
                StartCoroutine(DelayBeforeExplosion());
                IncrementCutscene();
                break;
            case 4:
                Explosion.SetActive(value: true);
                Explosion.GetComponent<Animator>().Play("spr_badexplosion_explode");
                CutsceneSource.PlayOneShot(CutsceneSounds[0]);
                IncrementCutscene();
                break;
            case 5:
                Cake.SetActive(value: true);
                Cake.transform.position = Vector3.MoveTowards(Cake.transform.position, Explosion.transform.position + Vector3.up * 20f, 20f * Time.deltaTime);
                if (Cake.transform.position == Explosion.transform.position + Vector3.up * 20f)
                {
                    Cake.transform.position = Susie.transform.position + Vector3.up * 15f;
                    IncrementCutscene();
                }
                break;
            case 6:
                Cake.transform.position = Vector3.MoveTowards(Cake.transform.position, Susie.transform.position + Vector3.up * 2.5f, 15f * Time.deltaTime);
                if (Cake.transform.position == Susie.transform.position + Vector3.up * 2.5f)
                {
                    IncrementCutscene();
                    StartCoroutine(DelayBeforeCakeSlice());
                }
                break;
            case 8:
                if (PlayerManager.Instance.transform.position.x < 0f)
                {
                    Cake_Left.transform.position = Vector3.MoveTowards(Cake_Left.transform.position, Susie.transform.position + Vector3.up * 2.5f, 15f * Time.deltaTime);
                    Cake_Right.transform.position = Vector3.MoveTowards(Cake_Right.transform.position, PlayerManager.Instance.transform.position + Vector3.up * 2.5f, 15f * Time.deltaTime);
                    if (Cake_Left.transform.position == Susie.transform.position + Vector3.up * 2.5f && Cake_Right.transform.position == PlayerManager.Instance.transform.position + Vector3.up * 2.5f)
                    {
                        IncrementCutscene();
                        StartCoroutine(DelayBeforeCakeSlicesDrop());
                    }
                }
                else
                {
                    Cake_Right.transform.position = Vector3.MoveTowards(Cake_Right.transform.position, Susie.transform.position + Vector3.up * 2.5f, 15f * Time.deltaTime);
                    Cake_Left.transform.position = Vector3.MoveTowards(Cake_Left.transform.position, PlayerManager.Instance.transform.position + Vector3.up * 2.5f, 15f * Time.deltaTime);
                    if (Cake_Right.transform.position == Susie.transform.position + Vector3.up * 2.5f && Cake_Left.transform.position == PlayerManager.Instance.transform.position + Vector3.up * 2.5f)
                    {
                        IncrementCutscene();
                        StartCoroutine(DelayBeforeCakeSlicesDrop());
                    }
                }
                break;
            case 9:
                if (PlayerManager.Instance.transform.position.x < 0f)
                {
                    Cake_Left.transform.position = Vector3.MoveTowards(Cake_Left.transform.position, Susie.transform.position + Vector3.up / 2f, 15f * Time.deltaTime);
                    Cake_Right.transform.position = Vector3.MoveTowards(Cake_Right.transform.position, PlayerManager.Instance.transform.position + Vector3.up / 2f, 15f * Time.deltaTime);
                    CakeSplat.GetComponent<SpriteRenderer>().flipX = true;
                    if (Cake_Left.transform.position == Susie.transform.position + Vector3.up / 2f && Cake_Right.transform.position == PlayerManager.Instance.transform.position + Vector3.up / 2f)
                    {
                        IncrementCutscene();
                    }
                }
                else
                {
                    Cake_Right.transform.position = Vector3.MoveTowards(Cake_Right.transform.position, Susie.transform.position + Vector3.up / 2f, 15f * Time.deltaTime);
                    Cake_Left.transform.position = Vector3.MoveTowards(Cake_Left.transform.position, PlayerManager.Instance.transform.position + Vector3.up / 2f, 15f * Time.deltaTime);
                    CakeSplat.GetComponent<SpriteRenderer>().flipX = false;
                    if (Cake_Right.transform.position == Susie.transform.position + Vector3.up / 2f && Cake_Left.transform.position == PlayerManager.Instance.transform.position + Vector3.up / 2f)
                    {
                        IncrementCutscene();
                    }
                }
                break;
            case 10:
                Cake_Left.SetActive(value: false);
                Cake_Right.SetActive(value: false);
                SusieGlowAnimator.Play("SusieGlow_Heal");
                Susie.SusieAnimator.Play("SusieDarkworld_Eat");
                storedparticle = Object.Instantiate(HealParticlePrefab);
                storedparticle.transform.position = Susie.transform.position;
                CutsceneSource.PlayOneShot(CutsceneSounds[2]);
                CutsceneSource.PlayOneShot(CutsceneSounds[1]);
                CakeSplat.SetActive(value: true);
                StartCoroutine(DelayBeforeSusieAsk());
                IncrementCutscene();
                break;
            case 13:
                CutsceneIndex = 0;
                PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
                DarkworldMenu.Instance.CanOpenMenu = true;
                break;
            case 3:
            case 7:
            case 11:
            case 12:
                break;
        }
    }

    public void IncrementCutscene()
    {
        CutsceneIndex++;
    }

    public void BeginCutscene()
    {
        CutsceneIndex = 1;
    }

    private void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom)
    {
        CutsceneChatter.FirstTextPlayed = false;
        CutsceneChatter.CurrentIndex = index;
        CutsceneChatter.FinishedText = false;
        CutsceneChatter.Text = text;
        if (ForcePosition)
        {
            CutsceneChatter.ManualTextboxPosition = true;
            CutsceneChatter.OnBottom = OnBottom;
        }
        CutsceneChatter.RUN();
    }

    private IEnumerator DelayBeforeExplosion()
    {
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        SusieGlowAnimator = Susie.transform.Find("Glow").transform.Find("GlowSprite").GetComponent<Animator>();
        yield return new WaitForSeconds(1f);
        IncrementCutscene();
    }

    private IEnumerator DelayBeforeCakeSlice()
    {
        Cake_Left.transform.position = Cake.transform.position;
        Cake_Right.transform.position = Cake.transform.position;
        yield return new WaitForSeconds(1f);
        CakeRenderer.enabled = false;
        Cake_Left.SetActive(value: true);
        Cake_Right.SetActive(value: true);
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(1f);
        IncrementCutscene();
    }

    private IEnumerator DelayBeforeCakeSlicesDrop()
    {
        yield return new WaitForSeconds(0.5f);
        IncrementCutscene();
    }

    private IEnumerator DelayBeforeSusieAsk()
    {
        yield return new WaitForSeconds(1.25f);
        Susie.SusieAnimator.Play("Idle");
        RunFreshChat(CutsceneChats, 0, ForcePosition: false, OnBottom: false);
    }
}
