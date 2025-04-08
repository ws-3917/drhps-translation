using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapyrusRoom_OverworldsAndOgres : MonoBehaviour
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
    private Animator SansArmAnimator;

    [SerializeField]
    private ParticleSystem SansDice1;

    [SerializeField]
    private ParticleSystem SansDice2;

    [SerializeField]
    private Sprite[] DiceSprites;

    [SerializeField]
    private GameObject OAOShadow;

    [SerializeField]
    private Animator OAODungeonShadowAnimator;

    [SerializeField]
    private Animator OAODungeonGoldShadowAnimator;

    [SerializeField]
    private Animator OAODungeonShadowGoblinAnimator;

    [SerializeField]
    private Animator OAODungeonShadowBlackAnimator;

    [SerializeField]
    private Animator OAODungeonShadowBridgeAnimator;

    [SerializeField]
    private Animator OAODungeonShadowDragonAnimator;

    [SerializeField]
    private Animator OAODungeonShadowFirewallShadow;

    [SerializeField]
    private Animator OAODungeonShadowFirewall;

    [SerializeField]
    private Animator OAODungeonShadowWhite;

    [SerializeField]
    private Animator OAODungeonShadowFinale;

    [SerializeField]
    private Transform OAOTable;

    [SerializeField]
    private Transform OAOMysticalArtifact;

    [SerializeField]
    private AudioClip mus_campaignover;

    [SerializeField]
    private HypothesisGoal goal_win;

    [SerializeField]
    private SPR_YSorting[] sortedObjects;

    [SerializeField]
    private bool KrisPreviousTurn_Fight;

    [SerializeField]
    private CameraManager PlayerCamera;

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

    [SerializeField]
    private AudioClip Music_Rumbling;

    [Header("-- Debug --")]
    [SerializeField]
    private GameObject[] Debug_StuffToEnable;

    [SerializeField]
    private GameObject[] Debug_StuffToDisable;

    [SerializeField]
    private Vector2[] Debug_StartCharacterPositions;

    [Header("-- OAO Shared --")]
    [SerializeField]
    private float papyrusDamage;

    [SerializeField]
    private float susieDamage;

    [SerializeField]
    private float krisDamage;

    [SerializeField]
    private float berdlyDamage;

    [Header("-- Goblin Fight --")]
    [SerializeField]
    private float goblinDamage;

    [Header("-- 3 Door Choice --")]
    private string ThreeDoorChoice = "";

    [SerializeField]
    private Sprite OgreOutline;

    [Header("-- After OAO --")]
    [SerializeField]
    private Vector2[] BerdlyExitWalkPositions;

    [SerializeField]
    private Vector2[] BerdlyExitWalKDirections;

    [SerializeField]
    private Vector2[] SusieKrisWalkPositions;

    [SerializeField]
    private Vector2[] SusieKrisWalKDirections;

    [SerializeField]
    private Vector2[] SansWalkPositions;

    [SerializeField]
    private Vector2[] SansWalkDirections;

    private void Start()
    {
        mus_campaignover.LoadAudioData();
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
        if (Input.GetKeyDown(KeyCode.H) && DRHDebugManager.instance.DebugModeEnabled)
        {
            Debug_StartCutscene();
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
        Kris = PlayerManager.Instance;
        PlayerCamera = CameraManager.instance;
        yield return new WaitForSeconds(0.2f);
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
        while (PlayerCamera.transform.position.x != OAOTable.position.x || PlayerCamera.transform.position.y != -0.45f)
        {
            yield return null;
            Vector3 target = new Vector3(OAOTable.position.x, -0.45f, PlayerCamera.transform.position.z);
            PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, target, 3f * Time.deltaTime);
        }
        yield return new WaitForSeconds(0.5f);
        OAOShadow.SetActive(value: true);
        CutsceneSource.PlayOneShot(CutsceneSounds[1]);
        yield return new WaitForSeconds(1f);
        MusicManager.PlaySong(Music_TGAOATLEWMB, FadePreviousSong: false, 0f);
        IncrementCutsceneIndex();
        RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: false);
        SPR_YSorting[] array = sortedObjects;
        foreach (SPR_YSorting obj in array)
        {
            obj.enabled = false;
            obj.SPR.sortingOrder = 0;
        }
        OAODungeonShadowAnimator.Play("OAO_ShadowDungeon_FadeIn");
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.25f);
        RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.25f);
        RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.25f);
        RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        MusicManager.StopSong(Fade: true, 0.25f);
        yield return new WaitForSeconds(0.25f);
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        OAODungeonShadowGoblinAnimator.Play("OAO_ShadowDungeon_FadeIn");
        yield return new WaitForSeconds(1.5f);
        MusicManager.PlaySong(Music_TGAOATLEOTT, FadePreviousSong: false, 0f);
        RunFreshChat(CutsceneChats[1], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        Sans_ThrowDice(1, 4);
        goblinDamage += 5f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[2], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 7)
        {
            yield return null;
        }
        Sans_ThrowDice(6, 2);
        goblinDamage += 8f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[2], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 8)
        {
            yield return null;
        }
        if (KrisPreviousTurn_Fight)
        {
            Sans_ThrowDice(4, 6);
            goblinDamage += 10f;
            yield return new WaitForSeconds(1.65f);
            RunFreshChat(CutsceneChats[2], 2, ForcePosition: true, OnBottom: false);
            while (CutsceneIndex != 9)
            {
                yield return null;
            }
        }
        else
        {
            CutsceneIndex = 9;
        }
        CutsceneIndex = 9;
        Sans_ThrowDice(5, 1);
        yield return new WaitForSeconds(1.65f);
        if (KrisPreviousTurn_Fight)
        {
            RunFreshChat(CutsceneChats[3], 0, ForcePosition: true, OnBottom: false);
            krisDamage += 6f;
            susieDamage += 6f;
            berdlyDamage += 6f;
            papyrusDamage += 3f;
        }
        else
        {
            RunFreshChat(CutsceneChats[3], 1, ForcePosition: true, OnBottom: false);
            krisDamage += 3f;
            susieDamage += 3f;
            berdlyDamage += 3f;
            papyrusDamage += 2f;
        }
        while (CutsceneIndex != 10)
        {
            yield return null;
        }
        ChatboxManager.Instance.EndText();
        yield return new WaitForSeconds(0.5f);
        if (KrisPreviousTurn_Fight)
        {
            RunFreshChat(CutsceneChats[3], 3, ForcePosition: true, OnBottom: false);
        }
        else
        {
            RunFreshChat(CutsceneChats[3], 4, ForcePosition: true, OnBottom: false);
        }
        while (CutsceneIndex != 11)
        {
            yield return null;
        }
        Sans_ThrowDice(5, 2);
        goblinDamage += 7f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[4], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 12)
        {
            yield return null;
        }
        Sans_ThrowDice(4, 5);
        goblinDamage += 9f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[4], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 13)
        {
            yield return null;
        }
        Sans_ThrowDice(2, 1);
        susieDamage -= 3f;
        berdlyDamage -= 3f;
        krisDamage -= 3f;
        papyrusDamage -= 3f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[4], 2, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 14)
        {
            yield return null;
        }
        if (KrisPreviousTurn_Fight)
        {
            Sans_ThrowDice(4, 6);
            goblinDamage += 10f;
            yield return new WaitForSeconds(1.65f);
            RunFreshChat(CutsceneChats[4], 3, ForcePosition: true, OnBottom: false);
            while (CutsceneIndex != 15)
            {
                yield return null;
            }
        }
        else
        {
            CutsceneIndex = 15;
        }
        Sans_ThrowDice(4, 4);
        yield return new WaitForSeconds(1.65f);
        if (KrisPreviousTurn_Fight)
        {
            RunFreshChat(CutsceneChats[5], 0, ForcePosition: true, OnBottom: false);
            krisDamage += 8f;
            susieDamage += 8f;
            berdlyDamage += 8f;
            papyrusDamage += 8f;
        }
        else
        {
            RunFreshChat(CutsceneChats[5], 1, ForcePosition: true, OnBottom: false);
            krisDamage += 4f;
            susieDamage += 4f;
            berdlyDamage += 4f;
            papyrusDamage += 4f;
        }
        while (CutsceneIndex != 16)
        {
            yield return null;
        }
        ChatboxManager.Instance.EndText();
        yield return new WaitForSeconds(0.5f);
        RunFreshChat(CutsceneChats[5], 2, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 17)
        {
            yield return null;
        }
        Sans_ThrowDice(1, 3);
        goblinDamage += 4f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[6], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 18)
        {
            yield return null;
        }
        Sans_ThrowDice(2, 2);
        goblinDamage += 4f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[6], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 19)
        {
            yield return null;
        }
        Sans_ThrowDice(6, 6);
        goblinDamage += 12f;
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[6], 2, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 20)
        {
            yield return null;
        }
        if (KrisPreviousTurn_Fight)
        {
            Sans_ThrowDice(5, 3);
            goblinDamage += 8f;
            yield return new WaitForSeconds(1.65f);
            RunFreshChat(CutsceneChats[6], 3, ForcePosition: true, OnBottom: false);
            while (CutsceneIndex != 21)
            {
                yield return null;
            }
        }
        else
        {
            CutsceneIndex = 21;
        }
        MusicManager.StopSong(Fade: true, 0.25f);
        yield return new WaitForSeconds(0.25f);
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        CutsceneSource.PlayOneShot(CutsceneSounds[3]);
        OAODungeonShadowGoblinAnimator.Play("OAO_ShadowDungeon_FadeOut");
        yield return new WaitForSeconds(1.5f);
        ChatboxManager.Instance.EndText();
        yield return new WaitForSeconds(2f);
        RunFreshChat(CutsceneChats[7], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 22)
        {
            yield return null;
        }
        CutsceneIndex = 1;
        OAODungeonGoldShadowAnimator.Play("OAO_ShadowDungeon_FadeOut");
        CutsceneSource.PlayOneShot(CutsceneSounds[9]);
        MusicManager.PlaySong(Music_TGAOATLEWMB, FadePreviousSong: false, 0f);
        yield return new WaitForSeconds(0.5f);
        RunFreshChat(CutsceneChats[8], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return null;
        CutsceneSource.PlayOneShot(CutsceneSounds[9]);
        switch (ThreeDoorChoice)
        {
            case "skull":
                IncrementCutsceneIndex();
                IncrementCutsceneIndex();
                break;
            case "questionmark":
                MusicManager.StopSong(Fade: true, 0.25f);
                OAODungeonShadowBlackAnimator.Play("OAO_ShadowDungeon_FadeIn");
                while (CutsceneIndex != 3)
                {
                    yield return null;
                }
                IncrementCutsceneIndex();
                break;
            case "sword":
                yield return new WaitForSeconds(1.65f);
                RunFreshChat(CutsceneChats[9], 1, ForcePosition: true, OnBottom: false);
                while (CutsceneIndex != 3)
                {
                    yield return null;
                }
                MusicManager.StopSong(Fade: true, 0.25f);
                yield return new WaitForSeconds(0.25f);
                PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
                CutsceneSource.PlayOneShot(CutsceneSounds[2]);
                OAODungeonShadowGoblinAnimator.GetComponent<SpriteRenderer>().sprite = OgreOutline;
                OAODungeonShadowGoblinAnimator.Play("OAO_ShadowDungeon_FadeIn");
                yield return new WaitForSeconds(1.5f);
                RunFreshChat(CutsceneChats[9], 2, ForcePosition: true, OnBottom: false);
                while (CutsceneIndex != 4)
                {
                    yield return null;
                }
                OAODungeonShadowGoblinAnimator.Play("OAO_ShadowDungeon_FadeOut");
                yield return new WaitForSeconds(1f);
                break;
        }
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        if (ThreeDoorChoice == "questionmark")
        {
            StartCoroutine(EndOAO());
            yield break;
        }
        CutsceneIndex = 1;
        OAODungeonShadowBridgeAnimator.Play("OAO_ShadowDungeon_FadeIn");
        CutsceneSource.PlayOneShot(CutsceneSounds[9]);
        yield return null;
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[10], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        MusicManager.PlaySong(Music_Rumbling, FadePreviousSong: false, 0f);
        OAODungeonShadowBridgeAnimator.Play("OAO_ShadowDungeon_Shake");
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        MusicManager.StopSong(Fade: true, 0.25f);
        yield return new WaitForSeconds(0.25f);
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        CutsceneSource.PlayOneShot(CutsceneSounds[2]);
        OAODungeonShadowDragonAnimator.Play("OAO_ShadowDungeon_FadeIn");
        OAODungeonShadowBridgeAnimator.Play("OAO_ShadowDungeon_FadeIn", -1, 1f);
        yield return new WaitForSeconds(1.5f);
        CutsceneIndex = 1;
        RunFreshChat(CutsceneChats[10], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[11], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[11], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        CutsceneSource.PlayOneShot(CutsceneSounds[4]);
        if (ThreeDoorChoice != "sword")
        {
            yield return new WaitForSeconds(1f);
            MusicManager.PlaySong(CutsceneSounds[5], FadePreviousSong: false, 0f);
            OAODungeonShadowFirewallShadow.Play("OAO_ShadowDungeon_FadeIn", -1, 1f);
            OAODungeonShadowFirewall.Play("OAO_ShadowDungeon_FireWall", -1, 0f);
            yield return new WaitForSeconds(3f);
            StartCoroutine(EndOAO());
            yield break;
        }
        yield return new WaitForSeconds(1.25f);
        EveryoneShock();
        for (int j = 0; j < 4; j++)
        {
            CutsceneSource.PlayOneShot(CutsceneSounds[10]);
            if (!SettingsManager.Instance.GetBoolSettingValue("SimpleVFX"))
            {
                CutsceneUtils.ShakeTransform(PlayerCamera.transform, 0.125f, 0.7f);
            }
            yield return new WaitForSeconds(0.8f);
        }
        RunFreshChat(CutsceneChats[13], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        OAODungeonShadowFirewallShadow.speed = 1f;
        OAODungeonShadowAnimator.Play("OAO_ShadowDungeon_FadeOut", -1, 0f);
        OAODungeonShadowFirewallShadow.Play("OAO_ShadowDungeon_FadeOut", -1, 0f);
        OAODungeonShadowFirewall.speed = 1f;
        OAODungeonShadowDragonAnimator.Play("OAO_ShadowDungeon_FadeOut", -1, 0f);
        CutsceneSource.PlayOneShot(CutsceneSounds[9]);
        OAODungeonShadowFinale.Play("OAO_ShadowDungeon_FadeIn", -1, 0f);
        yield return new WaitForSeconds(1.5f);
        RunFreshChat(CutsceneChats[14], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        Sans_ThrowDice(3, 2);
        yield return new WaitForSeconds(1.65f);
        RunFreshChat(CutsceneChats[14], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 7)
        {
            yield return null;
        }
        EveryoneShock();
        Sans_Idle_Down();
        MusicManager.PlaySong(mus_campaignover, FadePreviousSong: false, 0f);
        OAOMysticalArtifact.gameObject.SetActive(value: true);
        CutsceneUtils.MoveTransformLinear(OAOMysticalArtifact, new Vector3(-1.265f, -2.01f), 6f);
        yield return new WaitForSeconds(6f);
        OAODungeonShadowWhite.Play("OAO_ShadowDungeon_FadeWhite", -1, 0f);
        yield return new WaitForSeconds(4f);
        HypotheticalGoalManager.Instance.CompleteGoal(goal_win);
        StartCoroutine(EndOAO());
    }

    private IEnumerator EndOAO()
    {
        MusicManager.StopSong(Fade: true, 2f);
        UI_FADE.Instance.StartFadeIn(-1, 0.25f);
        CutsceneIndex = 1;
        yield return new WaitForSeconds(3f);
        EveryoneLook_AtTable();
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        Object.Destroy(OAOMysticalArtifact.GetComponentInChildren<Animator>().gameObject);
        OAODungeonShadowBridgeAnimator.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowDragonAnimator.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowFirewallShadow.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowFirewall.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowAnimator.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowBlackAnimator.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowFinale.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        OAODungeonShadowWhite.Play("OAO_ShadowDungeon_FadeOut", -1, 1f);
        PapyrusArmorAnimator.transform.position = Vector2.up * 500f;
        SansArmorAnimator.transform.position = Vector2.up * 500f;
        OAOShadow.SetActive(value: false);
        SPR_YSorting[] array = sortedObjects;
        for (int i = 0; i < array.Length; i++)
        {
            array[i].enabled = true;
        }
        Vector3 position = new Vector3(0f, -0.45f, PlayerCamera.transform.position.z);
        PlayerCamera.transform.position = position;
        yield return new WaitForSeconds(0.1f);
        UI_FADE.Instance.StartFadeOut(0.25f);
        yield return new WaitForSeconds(2.5f);
        RunFreshChat(CutsceneChats[12], 0, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 2)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.75f);
        CutsceneUtils.ShakeTransform(Berdly.transform);
        CutsceneUtils.ShakeTransform(Susie.transform);
        CutsceneSource.PlayOneShot(CutsceneSounds[6]);
        CutsceneSource.PlayOneShot(CutsceneSounds[7]);
        Berdly_Shock_Left();
        Susie.SusieAnimator.Play("Susie_Shock_NoShake");
        yield return new WaitForSeconds(1.5f);
        RunFreshChat(CutsceneChats[12], 1, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 3)
        {
            yield return null;
        }
        int berdlyWalkIndex = 0;
        while ((Vector2)Berdly.position != BerdlyExitWalkPositions[1])
        {
            yield return null;
            if ((Vector2)Berdly.position != BerdlyExitWalkPositions[berdlyWalkIndex])
            {
                BerdlyAnimator.speed = 2.5f;
                BerdlyAnimator.Play("Walk");
                BerdlyAnimator.SetFloat("MOVEMENTX", BerdlyExitWalKDirections[berdlyWalkIndex].x);
                BerdlyAnimator.SetFloat("MOVEMENTY", BerdlyExitWalKDirections[berdlyWalkIndex].y);
                Susie.RotateSusieTowardsPosition(PapyrusTransform.position);
                Berdly.position = Vector2.MoveTowards(Berdly.position, BerdlyExitWalkPositions[berdlyWalkIndex], 8f * Time.deltaTime);
            }
            else
            {
                berdlyWalkIndex++;
            }
        }
        BerdlyAnimator.GetComponent<SpriteRenderer>().enabled = false;
        CutsceneSource.PlayOneShot(CutsceneSounds[8]);
        yield return new WaitForSeconds(1f);
        RunFreshChat(CutsceneChats[12], 2, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 4)
        {
            yield return null;
        }
        PapyrusAnimator.Play("Papyrus_Phone_Left");
        int susieWalkIndex = 0;
        int krisWalkIndex = 0;
        while ((Vector2)Susie.transform.position != SusieKrisWalkPositions[2] || Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled || (Vector2)Kris.transform.position != SusieKrisWalkPositions[0])
        {
            yield return null;
            if (susieWalkIndex <= 2)
            {
                if ((Vector2)Susie.transform.position != SusieKrisWalkPositions[susieWalkIndex])
                {
                    Susie.SusieAnimator.Play("Walk");
                    Susie.transform.position = Vector2.MoveTowards(Susie.transform.position, SusieKrisWalkPositions[susieWalkIndex], 4f * Time.deltaTime);
                    Susie.RotateSusieToDirection(SusieKrisWalKDirections[susieWalkIndex]);
                }
                else
                {
                    susieWalkIndex++;
                }
            }
            else if (Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled)
            {
                Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled = false;
                CutsceneSource.PlayOneShot(CutsceneSounds[8]);
            }
            if (krisWalkIndex < 1)
            {
                if ((Vector2)Kris.transform.position != SusieKrisWalkPositions[krisWalkIndex])
                {
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.SetBool("MOVING", value: true);
                    Kris._PMove.RotatePlayerAnim(SusieKrisWalKDirections[krisWalkIndex]);
                    Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, SusieKrisWalkPositions[krisWalkIndex], 2.25f * Time.deltaTime);
                }
                else
                {
                    krisWalkIndex++;
                }
            }
        }
        if (Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled)
        {
            Susie.SusieAnimator.GetComponent<SpriteRenderer>().enabled = false;
            CutsceneSource.PlayOneShot(CutsceneSounds[8]);
        }
        RunFreshChat(CutsceneChats[12], 3, ForcePosition: true, OnBottom: false);
        Kris._PMove._anim.SetBool("MOVING", value: false);
        Kris._PMove.RotatePlayerAnim(Vector2.left);
        while (CutsceneIndex != 5)
        {
            yield return null;
        }
        int sansWalkIndex = 0;
        while ((Vector2)Sans.transform.position != SansWalkPositions[1])
        {
            yield return null;
            if (sansWalkIndex < 2)
            {
                if ((Vector2)Sans.transform.position != SansWalkPositions[sansWalkIndex])
                {
                    SansAnimator.Play("Walk");
                    SansArmorAnimator.Play("SansArmor_Walk");
                    SansAnimator.SetBool("MOVING", value: true);
                    CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", SansWalkDirections[sansWalkIndex]);
                    Kris._PMove.RotatePlayerAnimTowardsPosition(SansAnimator.transform.position);
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
        SansAnimator.SetBool("MOVING", value: false);
        RunFreshChat(CutsceneChats[12], 4, ForcePosition: true, OnBottom: false);
        while (CutsceneIndex != 6)
        {
            yield return null;
        }
        while ((Vector2)Kris.transform.position != SusieKrisWalkPositions[2])
        {
            yield return null;
            if (krisWalkIndex < 3)
            {
                if ((Vector2)Kris.transform.position != SusieKrisWalkPositions[krisWalkIndex])
                {
                    Kris._PMove.AnimationOverriden = true;
                    Kris._PMove._anim.SetBool("MOVING", value: true);
                    Kris._PMove.RotatePlayerAnim(SusieKrisWalKDirections[krisWalkIndex]);
                    Kris.transform.position = Vector2.MoveTowards(Kris.transform.position, SusieKrisWalkPositions[krisWalkIndex], 2f * Time.deltaTime);
                }
                else
                {
                    krisWalkIndex++;
                }
            }
        }
        Kris.transform.position = Vector2.left * 70f;
        CutsceneSource.PlayOneShot(CutsceneSounds[8]);
        yield return new WaitForSeconds(1.5f);
        UI_FADE.Instance.StartFadeIn(37, 1f, UnpauseOnEnd: true, NewMainMenuManager.MainMenuStates.Hypothetical);
    }

    public void Sans_ThrowDice(int Number1, int Number2)
    {
        StartCoroutine(Sans_ThrowDice_Timer(Number1, Number2));
    }

    private IEnumerator Sans_ThrowDice_Timer(int Number1, int Number2)
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        SansArmAnimator.Play("Sans_OAOHand_ThrowDice");
        SansAnimator.Play("Sans_Has_His_Right_Arm_Chopped_Off_For_One_Second_AHHHHHHHHH_GORE");
        yield return new WaitForSeconds(0.183f);
        ParticleSystem.TextureSheetAnimationModule dice1module = SansDice1.textureSheetAnimation;
        dice1module.SetSprite(0, DiceSprites[Number1]);
        SansDice1.Play();
        ParticleSystem.TextureSheetAnimationModule dice2module = SansDice2.textureSheetAnimation;
        dice2module.SetSprite(0, DiceSprites[Number2]);
        SansDice2.Play();
        CutsceneSource.PlayOneShot(CutsceneSounds[0]);
        for (int i = 0; i < Random.Range(4, 10); i++)
        {
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            dice1module.SetSprite(0, DiceSprites[Random.Range(1, 6)]);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            dice2module.SetSprite(0, DiceSprites[Random.Range(1, 6)]);
        }
        yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        dice1module.SetSprite(0, DiceSprites[Number1]);
        dice2module.SetSprite(0, DiceSprites[Number2]);
    }

    public void Sans_Idle_Down()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
    }

    public void Sans_Idle_Left()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
    }

    public void Berdly_Idle_Down()
    {
        BerdlyAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
    }

    public void Berdly_Idle_Up()
    {
        BerdlyAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
    }

    public void Berdly_Idle_Left()
    {
        BerdlyAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
    }

    public void Berdly_Idle_Right()
    {
        BerdlyAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
    }

    public void Berdly_Praise()
    {
        BerdlyAnimator.Play("berdly_praise");
    }

    public void Berdly_Shock_Left()
    {
        BerdlyAnimator.Play("berdly_shock_left");
    }

    public void EveryoneLook_AtSans()
    {
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        Susie.SusieAnimator.Play("Idle");
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.up);
        Berdly_Idle_Up();
    }

    public void EveryoneLook_AtPapyrus()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        Susie.SusieAnimator.Play("Idle");
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.up);
        Berdly_Idle_Up();
    }

    public void EveryoneLook_AtSusie()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        Kris._PMove.RotatePlayerAnim(Vector2.right);
        Berdly_Idle_Right();
    }

    public void EveryoneLook_AtBerdly()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.left);
        Kris._PMove.RotatePlayerAnim(Vector2.right);
    }

    public void EveryoneLook_AtKris()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.left);
        Berdly_Idle_Left();
    }

    public void EveryoneLook_AtTable()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
        Berdly_Idle_Up();
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void SansPapyrus_LookEachother()
    {
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
        Berdly_Idle_Up();
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void EveryoneShock()
    {
        PapyrusAnimator.Play("Papyrus_Shock_Right");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_ShockRight");
        Susie.SusieAnimator.Play("Susie_Shock_Up");
        BerdlyAnimator.Play("berdly_shock_up");
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void PapyrusShock()
    {
        PapyrusAnimator.Play("Papyrus_Shock_Right");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_ShockRight");
    }

    public void PapyrusPhoneLeft()
    {
        PapyrusAnimator.Play("Papyrus_Phone_Left");
    }

    public void PapyrusPhoneLeft_Delayed()
    {
        StartCoroutine(PapyrusPhoneLeft_Delayed_Timer());
    }

    private IEnumerator PapyrusPhoneLeft_Delayed_Timer()
    {
        yield return new WaitForSeconds(3f);
        PapyrusAnimator.Play("Papyrus_Phone_Left");
    }

    public void PapyrusPhoneRight()
    {
        PapyrusAnimator.Play("Papyrus_Phone_Right");
    }

    public void KrisIdle_Right()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.right);
    }

    public void KrisIdle_Up()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void KrisIdle_Left()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.left);
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

    public void SusieAnim_AngryLeft()
    {
        Susie.SusieAnimator.Play("Susie_Angry_Left");
    }

    public void SusieAnim_HeadScratch()
    {
        Susie.SusieAnimator.Play("Susie_Awkward");
    }

    public void PapyrusAnim_IdleDown()
    {
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
    }

    public void PapyrusAnim_IdleRight()
    {
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.right);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.right);
    }

    public void PapyrusAnim_IdleLeft()
    {
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.left);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.left);
    }

    public void PapyrusAnim_Praise()
    {
        PapyrusAnimator.Play("Papyrus_Praise");
        PapyrusArmorAnimator.Play("MP_PapyrusRoom_PapyrusArmor_Praise");
        PapyrusAngryEyebrows.gameObject.SetActive(value: false);
    }

    public void PapyrusEyebrows_Toggle()
    {
        PapyrusAngryEyebrows.gameObject.SetActive(!PapyrusAngryEyebrows.gameObject.activeSelf);
    }

    public void OAODoorChoice_Skull()
    {
        ThreeDoorChoice = "skull";
    }

    public void OAODoorChoice_QuestionMark()
    {
        ThreeDoorChoice = "questionmark";
    }

    public void OAODoorChoice_Sword()
    {
        ThreeDoorChoice = "sword";
    }

    public void OAODiceRoll_2()
    {
        Sans_ThrowDice(1, 1);
    }

    public void OAODiceRoll_7()
    {
        Sans_ThrowDice(4, 3);
    }

    public void OAODiceRoll_12()
    {
        Sans_ThrowDice(6, 6);
    }

    public void SetKrisPreviousTurn_Fight()
    {
        KrisPreviousTurn_Fight = true;
    }

    public void SetKrisPreviousTurn_Act()
    {
        KrisPreviousTurn_Fight = false;
    }

    public void ShowGoldShadow()
    {
        OAODungeonGoldShadowAnimator.Play("OAO_ShadowDungeon_FadeIn");
    }

    private void Debug_StartCutscene()
    {
        ChatboxManager.Instance.EndText();
        GameObject[] debug_StuffToDisable = Debug_StuffToDisable;
        for (int i = 0; i < debug_StuffToDisable.Length; i++)
        {
            debug_StuffToDisable[i].SetActive(value: false);
        }
        debug_StuffToDisable = Debug_StuffToEnable;
        for (int i = 0; i < debug_StuffToDisable.Length; i++)
        {
            debug_StuffToDisable[i].SetActive(value: true);
        }
        PlayerCamera.ReverbFilter.reverbPreset = AudioReverbPreset.Off;
        Kris = PlayerManager.Instance;
        PlayerCamera = CameraManager.instance;
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
        SansAnimator.Play("Idle");
        SansAnimator.SetBool("MOVING", value: false);
        SansArmorAnimator.Play("SansArmor");
        CutsceneUtils.RotateCharacterToDirection(SansAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(SansArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        PapyrusAnimator.Play("IdleFace");
        PapyrusArmorAnimator.Play("PapyrusArmor");
        CutsceneUtils.RotateCharacterToDirection(PapyrusAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusArmorAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        CutsceneUtils.RotateCharacterToDirection(PapyrusAngryEyebrows, "MOVEMENTX", "MOVEMENTY", Vector2.down);
        Kris._PMove.RotatePlayerAnim(Vector2.up);
        Susie.RotateSusieToDirection(Vector2.up);
        BerdlyAnimator.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(BerdlyAnimator, "MOVEMENTX", "MOVEMENTY", Vector2.up);
        Kris.transform.position = Debug_StartCharacterPositions[0];
        Susie.transform.position = Debug_StartCharacterPositions[1];
        PapyrusTransform.transform.position = Debug_StartCharacterPositions[2];
        Sans.transform.position = Debug_StartCharacterPositions[3];
        Berdly.transform.position = Debug_StartCharacterPositions[4];
        ChatboxManager.Instance.StoredAdditiveValues = new List<string> { "shortsword" };
        StartCutscene();
    }
}
