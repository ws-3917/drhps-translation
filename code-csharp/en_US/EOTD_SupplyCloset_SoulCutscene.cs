using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EOTD_SupplyCloset_SoulCutscene : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex = 1;

    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    private Transform Kris;

    [SerializeField]
    private RuntimeAnimatorController NormalKrisAnimations;

    [SerializeField]
    private RuntimeAnimatorController KrisHornsAnimation;

    [SerializeField]
    private Vector3[] KrisWalkPositions;

    [SerializeField]
    private List<Vector3> KrisQueuedPositions = new List<Vector3>();

    [SerializeField]
    private AudioClip[] KrisFootstepSounds;

    [SerializeField]
    private string KrisCurrentZombieWalkAnimation = "Kris_Soulless_Walk";

    [SerializeField]
    private Animator SoulRipOutParticle;

    [SerializeField]
    private SpriteRenderer[] ObjectsAffectedByLights;

    [SerializeField]
    private SpriteRenderer Horns;

    [SerializeField]
    private SpriteRenderer backgroundSprite;

    [SerializeField]
    private Sprite background_lightOn;

    [SerializeField]
    private Sprite background_lightOff;

    [SerializeField]
    private SpriteRenderer backgroundBoxes;

    [SerializeField]
    private Sprite backgroundBoxes_Open;

    [SerializeField]
    private Sprite backgroundBoxes_Close;

    [SerializeField]
    private Transform ExclamationMarkBubble;

    [SerializeField]
    private Color LightOffColor;

    private bool KrisFinishedWalking;

    [SerializeField]
    private GameObject[] FountainTargetParticles;

    [SerializeField]
    private GameObject BeforeSoulChats;

    [SerializeField]
    private GameObject AfterSoulChats;

    private void Start()
    {
        Kris = PlayerManager.Instance.transform;
        StartCoroutine(KrisWalkLoop());
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            CutsceneUpdate();
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
            PlayerManager.Instance._PMove._anim.transform.localPosition = new Vector2(0f, -0.9f);
        }
    }

    private void CutsceneUpdate()
    {
        if (CutsceneIndex == 1)
        {
            StartCoroutine(DelayUntilKrisBeginsWalk());
            IncrementCutsceneIndex();
        }
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
    }

    public void IncrementCutsceneIndex_EndChat()
    {
        CutsceneIndex++;
        ChatboxManager.Instance.EndText();
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

    private IEnumerator DelayUntilKrisBeginsWalk()
    {
        PlayerManager.Instance._PMove._anim.runtimeAnimatorController = NormalKrisAnimations;
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_RipOutSoul");
        yield return new WaitForSeconds(6f);
        SoulRipOutParticle.transform.position = Kris.transform.position;
        SoulRipOutParticle.Play("particle_soulripout", -1, 0f);
        yield return new WaitForSeconds(2.183f);
        Kris.GetComponent<Collider2D>().enabled = false;
        KrisQueuedPositions.Add(KrisWalkPositions[0]);
        KrisQueuedPositions.Add(KrisWalkPositions[1]);
        KrisQueuedPositions.Add(KrisWalkPositions[2]);
        KrisQueuedPositions.Add(KrisWalkPositions[3]);
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(0.5f);
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_turnOffLight");
        yield return new WaitForSeconds(1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        PlayerManager.Instance._PMove._anim.GetComponent<SpriteRenderer>().color = LightOffColor;
        backgroundSprite.sprite = background_lightOff;
        SpriteRenderer[] objectsAffectedByLights = ObjectsAffectedByLights;
        for (int i = 0; i < objectsAffectedByLights.Length; i++)
        {
            objectsAffectedByLights[i].color = LightOffColor;
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(1.5f);
        KrisQueuedPositions.Add(KrisWalkPositions[4]);
        KrisQueuedPositions.Add(KrisWalkPositions[5]);
        KrisQueuedPositions.Add(KrisWalkPositions[6]);
        KrisQueuedPositions.Add(KrisWalkPositions[7]);
        KrisQueuedPositions.Add(KrisWalkPositions[8]);
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KrisFinishedWalking = false;
        Horns.sortingLayerName = "AbovePlayer";
        yield return new WaitForSeconds(1f);
        backgroundBoxes.sprite = backgroundBoxes_Close;
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_SoulInBox");
        yield return new WaitForSeconds(3f);
        KrisCurrentZombieWalkAnimation = "Kris_Soulless_WalkLeftDown";
        KrisQueuedPositions.Add(KrisWalkPositions[9]);
        KrisQueuedPositions.Add(KrisWalkPositions[10]);
        KrisQueuedPositions.Add(KrisWalkPositions[11]);
        yield return new WaitForSeconds(3f);
        Horns.sortingLayerName = "Background";
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        KrisFinishedWalking = false;
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_CreateFountain");
        yield return new WaitForSeconds(3.35f);
        GameObject[] fountainTargetParticles = FountainTargetParticles;
        foreach (GameObject obj in fountainTargetParticles)
        {
            yield return new WaitForSeconds(0.0053333333f);
            CutsceneSource.PlayOneShot(CutsceneSounds[1]);
            obj.SetActive(value: true);
        }
        yield return new WaitForSeconds(4.85f);
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_GetUpFromFountain");
        yield return new WaitForSeconds(1.75f);
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_GroundCreateFountain");
        yield return new WaitForSeconds(3f);
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_HitGroundAnger");
        yield return new WaitForSeconds(5f);
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_GetUpAngry");
        KrisCurrentZombieWalkAnimation = "Kris_Soulless_WalkRightDown";
        yield return new WaitForSeconds(1.45f);
        KrisQueuedPositions.Add(KrisWalkPositions[12]);
        KrisQueuedPositions.Add(KrisWalkPositions[13]);
        KrisQueuedPositions.Add(KrisWalkPositions[14]);
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(0.25f);
        ExclamationMarkBubble.gameObject.SetActive(value: true);
        ExclamationMarkBubble.transform.position = Kris.transform.position + new Vector3(0f, 1.2f);
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        yield return new WaitForSeconds(0.65f);
        ExclamationMarkBubble.gameObject.SetActive(value: false);
        KrisQueuedPositions.Add(KrisWalkPositions[15]);
        KrisQueuedPositions.Add(KrisWalkPositions[16]);
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(1f);
        Horns.enabled = false;
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_PutHornsOn");
        KrisCurrentZombieWalkAnimation = "Kris_Soulless_Horns_WalkRightUp";
        yield return new WaitForSeconds(10.85f);
        KrisQueuedPositions.Add(KrisWalkPositions[8]);
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(1f);
        backgroundBoxes.sprite = backgroundBoxes_Open;
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_RetrieveSoul");
        yield return new WaitForSeconds(1.2f);
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_Horns_SwapSoulHand");
        KrisCurrentZombieWalkAnimation = "Kris_Soulless_Horns_WalkSoul";
        yield return new WaitForSeconds(1f);
        KrisQueuedPositions.Add(KrisWalkPositions[7]);
        KrisQueuedPositions.Add(KrisWalkPositions[6]);
        KrisQueuedPositions.Add(KrisWalkPositions[5]);
        KrisQueuedPositions.Add(KrisWalkPositions[4]);
        KrisQueuedPositions.Add(KrisWalkPositions[3]);
        while (!KrisFinishedWalking)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(0.5f);
        PlayerManager.Instance._PMove._anim.Play("Kris_Soulless_Horns_turnOnLight");
        yield return new WaitForSeconds(1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        PlayerManager.Instance._PMove._anim.GetComponent<SpriteRenderer>().color = Color.white;
        backgroundSprite.sprite = background_lightOn;
        objectsAffectedByLights = ObjectsAffectedByLights;
        for (int i = 0; i < objectsAffectedByLights.Length; i++)
        {
            objectsAffectedByLights[i].color = Color.white;
        }
        KrisFinishedWalking = false;
        yield return new WaitForSeconds(1.5f);
        PlayerManager.Instance._PMove._anim.Play("Kris_EOTD_Horns_GainSoul");
        yield return new WaitForSeconds(1.75f);
        SoulRipOutParticle.transform.position = Kris.transform.position;
        SoulRipOutParticle.Play("particle_soulripout", -1, 0f);
        yield return new WaitForSeconds(2.15f);
        CutsceneSource.PlayOneShot(CutsceneSounds[5]);
        PlayerManager.Instance._PMove._anim.runtimeAnimatorController = KrisHornsAnimation;
        CutsceneIndex = 0;
        PlayerManager.Instance._PMove._anim.Play("HORNS_KRIS_IDLE");
        Kris.GetComponent<Collider2D>().enabled = true;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
        LightworldMenu.Instance.CanOpenMenu = true;
        BeforeSoulChats.SetActive(value: false);
        AfterSoulChats.SetActive(value: true);
        base.enabled = false;
    }

    private void Kris_WalkToPosition(Vector3 Position, bool WalkLeft)
    {
        Kris.transform.position = Position;
        PlayerManager.Instance._PMove._anim.Play(KrisCurrentZombieWalkAnimation, -1, 0f);
        CutsceneSource.PlayOneShot(KrisFootstepSounds[Random.Range(0, KrisFootstepSounds.Length)]);
    }

    private IEnumerator KrisWalkLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 2f));
            if (KrisQueuedPositions.Count > 0)
            {
                Kris_WalkToPosition(KrisQueuedPositions[0], WalkLeft: false);
                KrisQueuedPositions.RemoveAt(0);
                if (KrisQueuedPositions.Count == 0)
                {
                    KrisFinishedWalking = true;
                }
                else
                {
                    KrisFinishedWalking = false;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3[] krisWalkPositions = KrisWalkPositions;
        foreach (Vector3 center in krisWalkPositions)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center, 0.25f);
        }
    }
}
