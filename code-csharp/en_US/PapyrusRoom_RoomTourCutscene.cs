using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapyrusRoom_RoomTourCutscene : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex;

    [Header("-- References --")]
    [SerializeField]
    private Transform PapyrusTransform;

    [SerializeField]
    private Animator PapyrusAnimator;

    [SerializeField]
    private INT_TalkingAnimation PapyrusTalkController;

    [SerializeField]
    private Animator PapyrusArmorAnimator;

    [SerializeField]
    private Animator SansArmorAnimator;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Transform Sans;

    [SerializeField]
    private Animator SansAnimator;

    [SerializeField]
    private PapyrusRoom_TutorialCutscene TutorialCutscene;

    [Header("-- Settings --")]
    [SerializeField]
    private Vector2[] KrisWalkPositions;

    [SerializeField]
    private Vector2[] SusieWalkPositions;

    [Space(10f)]
    [SerializeField]
    private Vector2[] KrisWalkDirections;

    [SerializeField]
    private Vector2[] SusieWalkDirections;

    [Space(10f)]
    [SerializeField]
    private Vector2[] PapyrusWalkDirections;

    [SerializeField]
    private Vector2[] PapyrusWalkPositions;

    [Space(10f)]
    [SerializeField]
    private Vector2[] SansWalkDirections;

    [SerializeField]
    private Vector2[] SansWalkPositions;

    [Header("-- Cutscene Chats --")]
    [SerializeField]
    private INT_Chat CutsceneChatter;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Header("-- Cutscene Sounds --")]
    [SerializeField]
    private AudioSource CutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneSounds;

    [SerializeField]
    private AudioClip Music_TGAOATLEWMB;

    [SerializeField]
    private AudioClip Music_NoSound;

    [SerializeField]
    private AudioClip Music_TGAOATLEOTT;

    private void Start()
    {
    }

    private void Update()
    {
        if (CutsceneIndex > 0)
        {
            Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
            LightworldMenu.Instance.CanOpenMenu = false;
            Susie.FollowingEnabled = false;
            Susie.AnimationOverriden = false;
        }
    }

    public void StartCutscene()
    {
        PapyrusTalkController.TargetChat = CutsceneChatter;
        StartCoroutine(Cutscene());
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
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

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(0.2f);
        Kris = PlayerManager.Instance;
        ActivePartyMember activePartyMember = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie);
        if (activePartyMember != null)
        {
            MonoBehaviour.print(activePartyMember.PartyMemberDescription);
            Susie = activePartyMember.PartyMemberFollowSettings;
        }
        else
        {
            Debug.LogError("Susie is not in the party?????");
        }
        Kris._PAnimation.FootstepsEnabled = false;
        Kris._PMove.GetComponent<Collider2D>().enabled = false;
        IncrementCutsceneIndex();
        yield return new WaitForSeconds(0.5f);
        int papyrusWalkIndex = 0;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[0])
        {
            yield return null;
            if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
            {
                PapyrusAnimator.Play("Walk");
                PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
            }
            else
            {
                papyrusWalkIndex++;
            }
        }
        PapyrusAnimator.Play("IdleFace");
        Susie.SusieAnimator.SetBool("InCutscene", value: true);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        Susie.RotateSusieToDirection(Vector2.left);
        Kris._PMove.RotatePlayerAnim(Vector2.left);
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        MusicManager.PlaySong(Music_TGAOATLEWMB, FadePreviousSong: false, 0f);
        papyrusWalkIndex = 1;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[2])
        {
            yield return null;
            Susie.RotateSusieTowardsPosition(PapyrusTransform.position);
            Kris._PMove.RotatePlayerAnimTowardsPosition(PapyrusTransform.position);
            if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
            {
                PapyrusAnimator.Play("Walk");
                PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
            }
            else
            {
                papyrusWalkIndex++;
            }
        }
        RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: false);
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        papyrusWalkIndex = 2;
        int susieWalkIndex = 0;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[5] || (Vector2)Susie.transform.position != SusieWalkPositions[0])
        {
            yield return null;
            Susie.RotateSusieTowardsPosition(PapyrusTransform.position);
            Kris._PMove.RotatePlayerAnimTowardsPosition(PapyrusTransform.position);
            if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
            {
                PapyrusAnimator.Play("Walk");
                PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
            }
            else
            {
                papyrusWalkIndex++;
            }
            if (susieWalkIndex < 1)
            {
                if ((Vector2)Susie.transform.position != SusieWalkPositions[susieWalkIndex])
                {
                    if (papyrusWalkIndex >= 4)
                    {
                        Susie.SusieAnimator.Play("Walk");
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[susieWalkIndex], 4f * Time.deltaTime);
                    }
                }
                else
                {
                    susieWalkIndex++;
                    SansAnimator.Play("Idle");
                    CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
                    SansArmorAnimator.Play("MP_PapyrusRoom_SansArmor_Right");
                }
            }
            else
            {
                Susie.SusieAnimator.Play("Idle");
            }
        }
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        papyrusWalkIndex = 5;
        susieWalkIndex = 1;
        int krisWalkIndex = 0;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[9] || (Vector2)Susie.transform.position != SusieWalkPositions[3] || (Vector2)Kris.transform.position != KrisWalkPositions[2])
        {
            yield return null;
            CutsceneUtils.RotateCharacterTowardsPosition(SansAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            SansArmorAnimator.Play("SansArmor");
            CutsceneUtils.RotateCharacterTowardsPosition(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
            {
                PapyrusAnimator.Play("Walk");
                PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
            }
            else
            {
                papyrusWalkIndex++;
            }
            if (susieWalkIndex < 4)
            {
                if ((Vector2)Susie.transform.position != SusieWalkPositions[susieWalkIndex])
                {
                    if (papyrusWalkIndex >= 7)
                    {
                        Susie.SusieAnimator.Play("Walk");
                        Susie.RotateSusieToDirection(SusieWalkDirections[susieWalkIndex]);
                        Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[susieWalkIndex], 4f * Time.deltaTime);
                    }
                }
                else
                {
                    susieWalkIndex++;
                }
            }
            if (krisWalkIndex >= 3)
            {
                continue;
            }
            if ((Vector2)Kris.transform.position != KrisWalkPositions[krisWalkIndex])
            {
                if (papyrusWalkIndex >= 7)
                {
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.SetBool("MOVING", value: true);
                    Kris._PMove.RotatePlayerAnim(KrisWalkDirections[krisWalkIndex]);
                    Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[krisWalkIndex], 4f * Time.deltaTime);
                }
            }
            else
            {
                krisWalkIndex++;
            }
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        papyrusWalkIndex = 10;
        susieWalkIndex = 4;
        krisWalkIndex = 3;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[10] || (Vector2)Susie.transform.position != SusieWalkPositions[5] || (Vector2)Kris.transform.position != KrisWalkPositions[4])
        {
            yield return null;
            MonoBehaviour.print("running");
            CutsceneUtils.RotateCharacterTowardsPosition(SansAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            SansArmorAnimator.Play("SansArmor");
            CutsceneUtils.RotateCharacterTowardsPosition(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            if (papyrusWalkIndex < 11)
            {
                if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
                {
                    PapyrusAnimator.Play("Walk");
                    PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                    PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                    PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
                }
                else
                {
                    papyrusWalkIndex++;
                }
            }
            else
            {
                PapyrusAnimator.Play("IdleFace");
                CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
            }
            if (susieWalkIndex < 6)
            {
                if ((Vector2)Susie.transform.position != SusieWalkPositions[susieWalkIndex])
                {
                    Susie.SusieAnimator.Play("Walk");
                    Susie.RotateSusieToDirection(SusieWalkDirections[susieWalkIndex]);
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[susieWalkIndex], 4f * Time.deltaTime);
                }
                else
                {
                    susieWalkIndex++;
                }
            }
            else
            {
                Susie.SusieAnimator.Play("Idle");
                Susie.RotateSusieToDirection(Vector2.left);
            }
            if (krisWalkIndex < 5)
            {
                if ((Vector2)Kris.transform.position != KrisWalkPositions[krisWalkIndex])
                {
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.SetBool("MOVING", value: true);
                    Kris._PMove.RotatePlayerAnim(KrisWalkDirections[krisWalkIndex]);
                    Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[krisWalkIndex], 4f * Time.deltaTime);
                }
                else
                {
                    krisWalkIndex++;
                }
            }
            else
            {
                Kris._PMove._anim.SetBool("MOVING", value: false);
            }
        }
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.left);
        Kris._PMove.RotatePlayerAnim(Vector2.down);
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        papyrusWalkIndex = 11;
        krisWalkIndex = 4;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[12] || (Vector2)Kris.transform.position != KrisWalkPositions[5])
        {
            yield return null;
            CutsceneUtils.RotateCharacterTowardsPosition(SansAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            SansArmorAnimator.Play("SansArmor");
            CutsceneUtils.RotateCharacterTowardsPosition(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            Susie.RotateSusieTowardsPosition(PapyrusTransform.position);
            if (papyrusWalkIndex < 13)
            {
                if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
                {
                    PapyrusAnimator.Play("Walk");
                    PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                    PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                    PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 4f * Time.deltaTime);
                }
                else
                {
                    papyrusWalkIndex++;
                }
            }
            else
            {
                PapyrusAnimator.Play("IdleFace");
                CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
            }
            if (krisWalkIndex < 6)
            {
                if ((Vector2)Kris.transform.position != KrisWalkPositions[krisWalkIndex])
                {
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.SetBool("MOVING", value: true);
                    Kris._PMove.RotatePlayerAnim(KrisWalkDirections[krisWalkIndex]);
                    Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[krisWalkIndex], 4f * Time.deltaTime);
                }
                else
                {
                    krisWalkIndex++;
                }
            }
            else
            {
                Kris._PMove._anim.SetBool("MOVING", value: false);
            }
        }
        Susie.RotateSusieToDirection(Vector2.right);
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        Kris._PMove._anim.SetBool("MOVING", value: false);
        RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 7)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        RunFreshChat(CutsceneChats[0], 6, ForcePosition: true, OnBottom: false);
        MusicManager.PlaySong(Music_NoSound, FadePreviousSong: true, 1.5f);
        while (CutsceneIndex != 8)
        {
            yield return null;
        }
        MusicManager.PlaySong(Music_TGAOATLEOTT, FadePreviousSong: false, 0f);
        papyrusWalkIndex = 12;
        PapyrusAnimator.speed = 2.5f;
        while ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[15])
        {
            yield return null;
            CutsceneUtils.RotateCharacterTowardsPosition(SansAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            SansArmorAnimator.Play("SansArmor");
            CutsceneUtils.RotateCharacterTowardsPosition(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", PapyrusTransform.position);
            if (papyrusWalkIndex < 16)
            {
                if ((Vector2)PapyrusTransform.position != PapyrusWalkPositions[papyrusWalkIndex])
                {
                    PapyrusAnimator.Play("Walk");
                    PapyrusAnimator.SetFloat("MOVEMENTX", PapyrusWalkDirections[papyrusWalkIndex].x);
                    PapyrusAnimator.SetFloat("MOVEMENTY", PapyrusWalkDirections[papyrusWalkIndex].y);
                    PapyrusTransform.position = Vector2.MoveTowards(PapyrusTransform.position, PapyrusWalkPositions[papyrusWalkIndex], 8f * Time.deltaTime);
                }
                else
                {
                    papyrusWalkIndex++;
                }
            }
        }
        PapyrusAnimator.speed = 1f;
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        yield return new WaitForSeconds(0.15f);
        RunFreshChat(CutsceneChats[0], 7, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 9)
        {
            yield return null;
        }
        PapyrusArmorAnimator.GetComponent<SpriteRenderer>().enabled = true;
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        PapyrusAnimator.Play("Papyrus_HandsUp_Down");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Down");
        while (PapyrusArmorAnimator.transform.localPosition.y > 0f)
        {
            yield return null;
            PapyrusArmorAnimator.transform.position = Vector2.MoveTowards(PapyrusArmorAnimator.transform.position, (Vector2)PapyrusArmorAnimator.transform.position - Vector2.up * 4f, 7.5f * Time.fixedDeltaTime * Time.timeScale);
        }
        PapyrusArmorAnimator.transform.position = PapyrusTransform.position;
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(0.25f);
        RunFreshChat(CutsceneChats[0], 8, ForcePosition: true, OnBottom: true);
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        while (CutsceneIndex != 10)
        {
            yield return null;
        }
        krisWalkIndex = 5;
        susieWalkIndex = 5;
        int sansWalkIndex = 0;
        while ((Vector2)Kris.transform.position != KrisWalkPositions[7] || (Vector2)Susie.transform.position != SusieWalkPositions[7] || (Vector2)Sans.transform.position != SansWalkPositions[1])
        {
            yield return null;
            if (krisWalkIndex < 8)
            {
                if ((Vector2)Kris.transform.position != KrisWalkPositions[krisWalkIndex])
                {
                    if (susieWalkIndex >= 7)
                    {
                        Kris._PMove.AnimationOverriden = true;
                        Kris._PMove._anim.SetBool("MOVING", value: true);
                        Kris._PMove.RotatePlayerAnim(KrisWalkDirections[krisWalkIndex]);
                        Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, KrisWalkPositions[krisWalkIndex], 4f * Time.deltaTime);
                    }
                }
                else
                {
                    krisWalkIndex++;
                }
            }
            else
            {
                Kris._PMove._anim.SetBool("MOVING", value: false);
            }
            if (susieWalkIndex < 8)
            {
                if ((Vector2)Susie.transform.position != SusieWalkPositions[susieWalkIndex])
                {
                    Susie.SusieAnimator.Play("Walk");
                    Susie.RotateSusieToDirection(SusieWalkDirections[susieWalkIndex]);
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieWalkPositions[susieWalkIndex], 4f * Time.deltaTime);
                }
                else
                {
                    susieWalkIndex++;
                }
            }
            else
            {
                Susie.SusieAnimator.Play("Idle");
            }
            if (sansWalkIndex < 2)
            {
                if ((Vector2)Sans.transform.position != SansWalkPositions[sansWalkIndex])
                {
                    SansAnimator.Play("Walk");
                    SansArmorAnimator.Play("SansArmor_Walk");
                    SansAnimator.SetBool("MOVING", value: true);
                    CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    Sans.transform.position = Vector2.MoveTowards(Sans.transform.position, SansWalkPositions[sansWalkIndex], 3f * Time.deltaTime);
                }
                else
                {
                    sansWalkIndex++;
                }
            }
            else
            {
                SansAnimator.SetBool("MOVING", value: false);
                SansArmorAnimator.Play("SansArmor");
            }
        }
        Susie.RotateSusieToDirection(Vector2.up);
        Susie.SusieAnimator.Play("Idle");
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        yield return new WaitForSeconds(0.25f);
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_RightPhone");
        PapyrusAnimator.Play("Papyrus_NoHands_Right");
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        MusicManager.StopSong(Fade: false, 0f);
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[0], 9, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 11)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        PapyrusArmorAnimator.Play("PapyrusArmor");
        PapyrusAnimator.Play("IdleFace");
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        RunFreshChat(CutsceneChats[0], 10, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 12)
        {
            yield return null;
        }
        CutsceneIndex = 0;
        TutorialCutscene.StartCutscene();
    }

    public void SetupAdditive_Knight()
    {
        ChatboxManager.Instance.StoredAdditiveValues = new List<string> { "shortsword" };
    }

    public void SetupAdditive_Archer()
    {
        ChatboxManager.Instance.StoredAdditiveValues = new List<string> { "crossbow" };
    }

    public void SetupAdditive_Mage()
    {
        ChatboxManager.Instance.StoredAdditiveValues = new List<string> { "spell book" };
    }

    public void SetupAdditive_Myself()
    {
        ChatboxManager.Instance.StoredAdditiveValues = new List<string> { "dagger" };
    }

    private void OnDrawGizmos()
    {
        if (PapyrusWalkPositions != null && PapyrusWalkPositions.Length >= 2 && SusieWalkPositions != null && SusieWalkPositions.Length >= 2 && KrisWalkPositions != null && KrisWalkPositions.Length >= 2 && SansWalkPositions != null && SansWalkPositions.Length >= 2)
        {
            for (int i = 0; i < PapyrusWalkPositions.Length - 1; i++)
            {
                Vector2 vector = PapyrusWalkPositions[i];
                Vector2 vector2 = PapyrusWalkPositions[i + 1];
                Gizmos.color = Color.red;
                Gizmos.DrawLine(vector, vector2);
            }
            for (int j = 0; j < SusieWalkPositions.Length - 1; j++)
            {
                Vector2 vector3 = SusieWalkPositions[j];
                Vector2 vector4 = SusieWalkPositions[j + 1];
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(vector3, vector4);
            }
            for (int k = 0; k < KrisWalkPositions.Length - 1; k++)
            {
                Vector2 vector5 = KrisWalkPositions[k];
                Vector2 vector6 = KrisWalkPositions[k + 1];
                Gizmos.color = Color.green;
                Gizmos.DrawLine(vector5, vector6);
            }
            for (int l = 0; l < SansWalkPositions.Length - 1; l++)
            {
                Vector2 vector7 = SansWalkPositions[l];
                Vector2 vector8 = SansWalkPositions[l + 1];
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(vector7, vector8);
            }
        }
    }
}
