using System.Collections;
using UnityEngine;

public class EOTD_SupplyCloset_SusieCutscene : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex = 1;

    [SerializeField]
    private Animator Susie;

    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    private Animator Kris;

    [SerializeField]
    private RuntimeAnimatorController TenseKrisAnimation;

    [SerializeField]
    private Vector3[] SusieWalkPositions;

    [SerializeField]
    private InventoryItem BallOfJunk;

    private void Start()
    {
        Kris = PlayerManager.Instance._PMove._anim;
        CutsceneIndex = 1;
        StartCoroutine(SusieAwakeDelay());
        PlayerManager.Instance._PMove._anim.Play("Kris_KnockedOut");
        Susie.Play("Susie_KnockedOut");
        LightworldInventory.Instance.PlayerInventory.Add(BallOfJunk);
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            CutsceneUpdate();
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
        }
        else
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
            LightworldMenu.Instance.CanOpenMenu = true;
            base.enabled = false;
        }
    }

    private void CutsceneUpdate()
    {
        switch (CutsceneIndex)
        {
            case 2:
                IncrementCutsceneIndex();
                StartCoroutine(SusieAwake());
                break;
            case 4:
                IncrementCutsceneIndex();
                StartCoroutine(SusieGetUp());
                break;
            case 6:
                IncrementCutsceneIndex();
                StartCoroutine(KrisAwake());
                break;
            case 8:
                IncrementCutsceneIndex();
                SusieAnim_IdleSadRight();
                StartCoroutine(SusieConcern());
                break;
            case 10:
                if (Susie.transform.position != SusieWalkPositions[0])
                {
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[0], 3f * Time.deltaTime);
                    Susie.Play("WalkSad");
                    Susie.SetFloat("VelocityX", 1f);
                    Susie.SetFloat("VelocityY", 0f);
                }
                else
                {
                    SusieAnim_IdleSadRight();
                    RunFreshChat(CutsceneChats[3], 1);
                    IncrementCutsceneIndex();
                }
                break;
            case 12:
                StartCoroutine(KrisHugSusie());
                IncrementCutsceneIndex();
                break;
            case 14:
                IncrementCutsceneIndex();
                IncrementCutsceneIndex();
                break;
            case 16:
                StartCoroutine(DelayUntilSusieWalkOut());
                IncrementCutsceneIndex();
                break;
            case 18:
                if (Susie.transform.position != SusieWalkPositions[1])
                {
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[1], 3f * Time.deltaTime);
                    break;
                }
                CutsceneSource.PlayOneShot(CutsceneSounds[3]);
                Susie.gameObject.SetActive(value: false);
                CutsceneIndex = 0;
                Kris.runtimeAnimatorController = TenseKrisAnimation;
                Kris.Play("TENSE_KRIS_IDLE");
                PlayerManager.Instance._PMove._anim.transform.localPosition = new Vector2(0f, -0.9f);
                break;
            case 1:
            case 3:
            case 5:
            case 7:
            case 9:
            case 11:
            case 13:
            case 15:
            case 17:
                break;
        }
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
    }

    private void RunFreshChat(CHATBOXTEXT text, int index = 0, bool ForcePosition = false, bool OnBottom = false)
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

    public void SusieAnim_IdleUp()
    {
        Susie.Play("IdleSad");
        Susie.SetFloat("VelocityX", 0f);
        Susie.SetFloat("VelocityY", 1f);
    }

    public void SusieAnim_IdleRight()
    {
        Susie.Play("Idle");
        Susie.SetFloat("VelocityX", 1f);
        Susie.SetFloat("VelocityY", 0f);
    }

    public void SusieAnim_IdleSadRight()
    {
        Susie.Play("IdleSad");
        Susie.SetFloat("VelocityX", 1f);
        Susie.SetFloat("VelocityY", 0f);
    }

    public void SusieAnim_TenseHug3()
    {
        Susie.Play("Susie_TenseHug3");
    }

    public void SusieAnim_TenseHug4()
    {
        Susie.Play("Susie_TenseHug4");
    }

    private IEnumerator SusieAwake()
    {
        Susie.Play("Susie_KnockedOut_Shake");
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        yield return new WaitForSeconds(1.5f);
        Susie.Play("Susie_Defeated");
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        yield return new WaitForSeconds(2f);
        RunFreshChat(CutsceneChats[0]);
    }

    private IEnumerator SusieAwakeDelay()
    {
        yield return new WaitForSeconds(2f);
        CutsceneIndex = 2;
    }

    private IEnumerator SusieGetUp()
    {
        yield return new WaitForSeconds(0.5f);
        Susie.Play("IdleSad");
        Susie.SetFloat("VelocityX", 0f);
        Susie.SetFloat("VelocityY", -1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        yield return new WaitForSeconds(1f);
        Susie.SetFloat("VelocityX", -1f);
        Susie.SetFloat("VelocityY", 0f);
        yield return new WaitForSeconds(1f);
        Susie.SetFloat("VelocityX", 0f);
        Susie.SetFloat("VelocityY", 1f);
        yield return new WaitForSeconds(1f);
        Susie.SetFloat("VelocityX", 1f);
        Susie.SetFloat("VelocityY", 0f);
        yield return new WaitForSeconds(1.5f);
        RunFreshChat(CutsceneChats[1]);
    }

    private IEnumerator KrisAwake()
    {
        yield return new WaitForSeconds(1f);
        Kris.Play("Kris_Defeated");
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        yield return new WaitForSeconds(1f);
        Kris.Play("Kris_Tense_Left");
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[2]);
    }

    private IEnumerator SusieConcern()
    {
        yield return new WaitForSeconds(1f);
        Kris.Play("Kris_Tense2_Left");
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[3]);
        SusieAnim_IdleSadRight();
    }

    private IEnumerator KrisHugSusie()
    {
        yield return new WaitForSeconds(1.5f);
        Kris.runtimeAnimatorController = TenseKrisAnimation;
        Kris.Play("TENSE_KRIS_IDLE");
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        yield return new WaitForSeconds(2f);
        Susie.SetFloat("VelocityX", -1f);
        Susie.SetFloat("VelocityY", 0f);
        yield return new WaitForSeconds(1.5f);
        Susie.SetFloat("VelocityX", 1f);
        Susie.SetFloat("VelocityY", 0f);
        RunFreshChat(CutsceneChats[4]);
    }

    private IEnumerator DelayUntilSusieConsole()
    {
        yield return new WaitForSeconds(2f);
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        Susie.Play("Susie_TenseHug3");
        Kris.Play("Kris_TenseHug3");
        yield return new WaitForSeconds(2f);
        Susie.SetFloat("VelocityX", 0f);
        Susie.SetFloat("VelocityY", -1f);
        yield return new WaitForSeconds(1f);
        Susie.SetFloat("VelocityX", 1f);
        Susie.SetFloat("VelocityY", 0f);
        RunFreshChat(CutsceneChats[4], 1);
    }

    private IEnumerator DelayUntilSusieWalkOut()
    {
        yield return new WaitForSeconds(1f);
        IncrementCutsceneIndex();
        Susie.Play("WalkSad");
        Susie.SetFloat("VelocityX", -1f);
        Susie.SetFloat("VelocityY", 0f);
    }
}
