using System.Collections;
using UnityEngine;

public class PapyrusRoom_TutorialCutscene : MonoBehaviour
{
    [SerializeField]
    private int CutsceneIndex;

    [Header("-- References --")]
    [SerializeField]
    private Transform PapyrusTransform;

    [SerializeField]
    private Animator PapyrusAnimator;

    [SerializeField]
    private Animator PapyrusAngryEyebrows;

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
    private Transform Berdly;

    [SerializeField]
    private Animator BerdlyAnimator;

    [SerializeField]
    private Transform Sans;

    [SerializeField]
    private Animator SansAnimator;

    [SerializeField]
    private GameObject OAOShadow;

    [SerializeField]
    private Transform OAOTable;

    [SerializeField]
    private CameraManager PlayerCamera;

    [SerializeField]
    private AudioReverbPreset DungeonReverbPreset;

    [SerializeField]
    private PapyrusRoom_OverworldsAndOgres OAOCutscene;

    [Header("-- Walking Stuff --")]
    [SerializeField]
    private Vector2[] SansWalkPositions;

    [SerializeField]
    private Vector2[] SansWalkDirections;

    [SerializeField]
    private Vector2[] BerdlyWalkPositions;

    [SerializeField]
    private Vector2[] BerdlyWalkDirections;

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

    [SerializeField]
    private AudioClip Music_AdventureStart;

    private void Start()
    {
        Music_AdventureStart.LoadAudioData();
        StartCoroutine(setupSusie());
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

    private IEnumerator setupSusie()
    {
        yield return new WaitForSeconds(0.2f);
        ActivePartyMember activePartyMember = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie);
        if (activePartyMember != null)
        {
            if (activePartyMember != null)
            {
                MonoBehaviour.print(activePartyMember.PartyMemberDescription);
            }
            Susie = activePartyMember.PartyMemberFollowSettings;
        }
        else
        {
            Debug.LogError("Susie is not in the party?????");
        }
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
        Kris = PlayerManager.Instance;
        PlayerCamera = CameraManager.instance;
        yield return new WaitForSeconds(0.2f);
        Kris._PAnimation.FootstepsEnabled = false;
        Kris._PMove.GetComponent<Collider2D>().enabled = false;
        while (PlayerCamera.transform.position.x != OAOTable.position.x || PlayerCamera.transform.position.y != OAOTable.position.y)
        {
            yield return null;
            Vector3 target = new Vector3(OAOTable.position.x, OAOTable.position.y, PlayerCamera.transform.position.z);
            PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, target, 3f * Time.deltaTime);
        }
        yield return new WaitForSeconds(0.1f);
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
        MusicManager.PlaySong(Music_AdventureStart, FadePreviousSong: false, 0f);
        while (CutsceneIndex != 1)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.25f);
        RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        OAOShadow.SetActive(value: true);
        MusicManager.PlaySong(Music_NoSound, FadePreviousSong: false, 0f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        OAOShadow.SetActive(value: false);
        MusicManager.PlaySong(Music_NoSound, FadePreviousSong: false, 0f);
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(1.35f);
        RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        int sansWalkIndex = 0;
        while ((Vector2)Sans.transform.position != SansWalkPositions[5])
        {
            yield return null;
            if (sansWalkIndex < 6)
            {
                if ((Vector2)Sans.transform.position != SansWalkPositions[sansWalkIndex])
                {
                    SansAnimator.Play("Walk");
                    SansArmorAnimator.Play("SansArmor_Walk");
                    SansAnimator.SetBool("MOVING", value: true);
                    CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    Sans.transform.position = Vector2.MoveTowards(Sans.transform.position, SansWalkPositions[sansWalkIndex], 3f * Time.deltaTime);
                    Kris._PMove.RotatePlayerAnimTowardsPosition(SansAnimator.transform.position);
                    Susie.RotateSusieTowardsPosition(SansAnimator.transform.position);
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
        SansAnimator.GetComponent<SpriteRenderer>().enabled = false;
        SansArmorAnimator.GetComponent<SpriteRenderer>().enabled = false;
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        yield return new WaitForSeconds(1.75f);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.up);
        RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: true);
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        while (PlayerCamera.transform.position.x != 0f || PlayerCamera.transform.position.y != -0.45f)
        {
            yield return null;
            Vector3 target2 = new Vector3(0f, -0.45f, PlayerCamera.transform.position.z);
            PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, target2, 5f * Time.deltaTime);
        }
        SansAnimator.GetComponent<SpriteRenderer>().enabled = true;
        SansArmorAnimator.GetComponent<SpriteRenderer>().enabled = true;
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        sansWalkIndex = 5;
        while ((Vector2)Sans.transform.position != SansWalkPositions[6])
        {
            yield return null;
            if (sansWalkIndex < 7)
            {
                if ((Vector2)Sans.transform.position != SansWalkPositions[sansWalkIndex])
                {
                    SansAnimator.Play("Walk");
                    SansArmorAnimator.Play("SansArmor_Walk");
                    SansAnimator.SetBool("MOVING", value: true);
                    CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    Sans.transform.position = Vector2.MoveTowards(Sans.transform.position, SansWalkPositions[sansWalkIndex], 3f * Time.deltaTime);
                    Kris._PMove.RotatePlayerAnimTowardsPosition(SansAnimator.transform.position);
                    Susie.RotateSusieTowardsPosition(SansAnimator.transform.position);
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
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        yield return new WaitForSeconds(0.5f);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        Berdly.gameObject.SetActive(value: true);
        BerdlyAnimator.GetComponent<SpriteRenderer>().enabled = true;
        int berdlyWalkIndex = 0;
        while ((Vector2)Berdly.transform.position != BerdlyWalkPositions[0])
        {
            yield return null;
            if (berdlyWalkIndex < 1)
            {
                if ((Vector2)Berdly.transform.position != BerdlyWalkPositions[berdlyWalkIndex])
                {
                    BerdlyAnimator.Play("Walk");
                    CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", BerdlyWalkDirections[berdlyWalkIndex]);
                    Berdly.transform.position = Vector2.MoveTowards(Berdly.transform.position, BerdlyWalkPositions[berdlyWalkIndex], 3f * Time.deltaTime);
                    Kris._PMove.RotatePlayerAnimTowardsPosition(SansAnimator.transform.position);
                    Susie.RotateSusieTowardsPosition(SansAnimator.transform.position);
                }
                else
                {
                    berdlyWalkIndex++;
                }
            }
            else
            {
                BerdlyAnimator.Play("Idle");
            }
        }
        BerdlyAnimator.Play("Idle");
        yield return new WaitForSeconds(0.25f);
        BerdlyAnimator.Play("berdly_praise");
        yield return new WaitForSeconds(0.1f);
        RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 7)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        RunFreshChat(CutsceneChats[1], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 8)
        {
            yield return null;
        }
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        Kris_IdleUp();
        SusieAnim_IdleUp();
        sansWalkIndex = 6;
        berdlyWalkIndex = 1;
        while ((Vector2)Sans.transform.position != SansWalkPositions[9] || (Vector2)Berdly.transform.position != BerdlyWalkPositions[2])
        {
            yield return null;
            if (sansWalkIndex < 10)
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
                CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
                CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
            }
            if (sansWalkIndex >= 7)
            {
                PapyrusTransform.position = Vector3.MoveTowards(PapyrusTransform.position, new Vector2(-2.015f, PapyrusTransform.position.y), 3f * Time.deltaTime);
            }
            if (berdlyWalkIndex >= 2)
            {
                Kris.transform.position = Vector3.MoveTowards(Kris.transform.position, new Vector2(-2.565f, Kris.transform.position.y), 3f * Time.deltaTime);
                Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, new Vector2(0f, Susie.transform.position.y), 3f * Time.deltaTime);
            }
            if (berdlyWalkIndex < 3)
            {
                if ((Vector2)Berdly.transform.position != BerdlyWalkPositions[berdlyWalkIndex])
                {
                    BerdlyAnimator.Play("Walk");
                    CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", BerdlyWalkDirections[berdlyWalkIndex]);
                    Berdly.transform.position = Vector2.MoveTowards(Berdly.transform.position, BerdlyWalkPositions[berdlyWalkIndex], 3f * Time.deltaTime);
                }
                else
                {
                    berdlyWalkIndex++;
                }
            }
            else
            {
                BerdlyAnimator.Play("Idle");
                CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
            }
        }
        BerdlyAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        while (PapyrusTransform.position.x != -2.015f)
        {
            yield return null;
            PapyrusTransform.position = Vector3.MoveTowards(PapyrusTransform.position, new Vector2(-2.015f, PapyrusTransform.position.y), 3f * Time.deltaTime);
        }
        while (Kris.transform.position.x != -2.565f || Susie.transform.position.x != 0f)
        {
            yield return null;
            Kris.transform.position = Vector3.MoveTowards(Kris.transform.position, new Vector2(-2.565f, Kris.transform.position.y), 3f * Time.deltaTime);
            Susie.transform.position = Vector3.MoveTowards(Susie.transform.position, new Vector2(0f, Susie.transform.position.y), 3f * Time.deltaTime);
        }
        EndCutscene();
    }

    public void EndCutscene()
    {
        CutsceneIndex = 0;
        OAOCutscene.StartCutscene();
    }

    public void Sans_Idle_Down()
    {
        SansAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
    }

    public void Sans_Idle_Right()
    {
        SansAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
    }

    public void Sans_Idle_Left()
    {
        SansAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
    }

    public void Susie_HeadScratch()
    {
        Susie.SusieAnimator.Play("Susie_Awkward");
    }

    public void SusieAnim_IdleRight()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.right);
    }

    public void SusieAnim_IdleLeft()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.left);
    }

    public void SusieAnim_IdleUp()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
    }

    public void Kris_IdleDown()
    {
        Kris._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.down);
    }

    public void Kris_IdleRight()
    {
        Kris._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.right);
    }

    public void Kris_IdleLeft()
    {
        Kris._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.left);
    }

    public void Kris_IdleUp()
    {
        Kris._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void Papyrus_Shock()
    {
        PapyrusAnimator.Play("Papyrus_Shock");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Shock");
    }

    public void SusieKris_FacePapyus()
    {
        Kris._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
    }

    public void SusieKris_FaceBerdly()
    {
        Kris._PMove._anim.Play("OVERWORLD_NOELLE_IDLE");
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.right);
    }

    private void OnDrawGizmos()
    {
        if (SansWalkPositions != null && SansWalkPositions.Length >= 2 && BerdlyWalkPositions != null && BerdlyWalkPositions.Length >= 2)
        {
            for (int i = 0; i < SansWalkPositions.Length - 1; i++)
            {
                Vector2 vector = SansWalkPositions[i];
                Vector2 vector2 = SansWalkPositions[i + 1];
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(vector, vector2);
            }
            for (int j = 0; j < BerdlyWalkPositions.Length - 1; j++)
            {
                Vector2 vector3 = BerdlyWalkPositions[j];
                Vector2 vector4 = BerdlyWalkPositions[j + 1];
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(vector3, vector4);
            }
        }
    }
}
