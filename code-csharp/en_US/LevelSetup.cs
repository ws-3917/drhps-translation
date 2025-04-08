using System.Collections.Generic;
using Discord;
using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public bool ContainsKris;

    public bool ResetKrisHealth;

    public PlayerManager.PlayerState KrisStartingState;

    public bool IsDarkworld;

    [Header("For if RoomHasMultipleEntrances is false")]
    public Transform PlayerStartPosition;

    [Header("Range: -1,1")]
    public Vector2 PlayerStartRotation;

    [Space(5f)]
    [Header("-= Party System =-")]
    public bool ClearAllMembers;

    public bool ReplaceMembers;

    public List<PartyMember> NewPartyMembers = new List<PartyMember>();

    [Space(5f)]
    [Header("-= Calls =-")]
    public bool ReplaceCalls;

    public bool ClearCalls;

    [Header("Make sure this matches current queued call amount, keep empty to not change")]
    public CHATBOXTEXT[] NewCalls;

    [Header("Use a # to not have the \"Call\" text added to name")]
    public string[] CallNames;

    [Space(5f)]
    public bool FadeIn = true;

    [Space(5f)]
    public bool DisableGonerMenu;

    [Space(5f)]
    [Header("-= Music =-")]
    public bool PlaySongOnLevelLoad;

    [Header("Keep empty to stop the current song, if previous bool is true")]
    public AudioClip Song;

    [Header("-= Room spawn Positions =-")]
    public bool RoomHasMultipleEntrances;

    public bool RoomContainsPartyMembers;

    public RoomStartPosition RoomPositions;

    [Space(10f)]
    [Header("-= Discord RPC =-")]
    public bool SetNewActivity;

    public string LargeImage;

    public string LargeImageText;

    public string State;

    public string Details;

    public string VesselText;

    private List<PartyMember_StoredReplacedStats> originalMemberStats = new List<PartyMember_StoredReplacedStats>();

    private void Awake()
    {
    }

    private void Start()
    {
        ChatboxManager.Instance.InDarkworld = IsDarkworld;
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Default);
        if (RoomHasMultipleEntrances)
        {
            AttemptNewEntranceSpawn();
        }
        else
        {
            _ = PlayerStartRotation;
            PlayerManager.Instance.transform.position = PlayerStartPosition.position;
            PlayerManager.Instance._PMove.RotatePlayerAnim(PlayerStartRotation);
        }
        if (ClearAllMembers)
        {
            PartyMemberSystem.Instance.RemoveAllPartyMember();
        }
        if (ReplaceMembers)
        {
            foreach (ActivePartyMember activePartyMember in PartyMemberSystem.Instance.ActivePartyMembers)
            {
                PartyMember_StoredReplacedStats partyMember_StoredReplacedStats = new PartyMember_StoredReplacedStats();
                partyMember_StoredReplacedStats.memberDescription = activePartyMember.PartyMemberDescription;
                partyMember_StoredReplacedStats.memberHealth = activePartyMember.CurrentHealth;
                originalMemberStats.Add(partyMember_StoredReplacedStats);
            }
            PartyMemberSystem.Instance.RemoveAllPartyMember();
            for (int i = 0; i < NewPartyMembers.Count; i++)
            {
                bool flag = false;
                foreach (PartyMember_StoredReplacedStats originalMemberStat in originalMemberStats)
                {
                    if (NewPartyMembers[i] == originalMemberStat.memberDescription)
                    {
                        flag = true;
                        PartyMemberSystem.Instance.AddPartyMember(NewPartyMembers[i], originalMemberStat.memberHealth);
                        break;
                    }
                }
                if (!flag)
                {
                    PartyMemberSystem.Instance.AddPartyMember(NewPartyMembers[i]);
                }
            }
            originalMemberStats.Clear();
        }
        else
        {
            for (int j = 0; j < NewPartyMembers.Count; j++)
            {
                PartyMemberSystem.Instance.AddPartyMember(NewPartyMembers[j]);
            }
        }
        if (ContainsKris)
        {
            PlayerManager.Instance.gameObject.SetActive(value: true);
            PlayerManager.Instance._PlayerState = KrisStartingState;
            PlayerManager.Instance._PMove.InDarkworld = IsDarkworld;
        }
        else
        {
            LightworldMenu.Instance.CanOpenMenu = false;
            PlayerManager.Instance.gameObject.SetActive(value: false);
            PlayerManager.Instance.transform.position = Vector3.zero;
        }
        if (ResetKrisHealth)
        {
            PlayerManager.Instance._PlayerHealth = PlayerManager.Instance._PlayerMaxHealth;
        }
        if (FadeIn)
        {
            UI_FADE.Instance.StartFadeOut();
        }
        Object.Destroy(PlayerStartPosition.gameObject);
        if (ReplaceCalls)
        {
            for (int k = 0; k < LightworldMenu.Instance.CallChatIndexes.Length; k++)
            {
                LightworldMenu.Instance.CallChatIndexes[k] = 0;
            }
            for (int l = 0; l < NewCalls.Length; l++)
            {
                if (NewCalls[l] != null)
                {
                    LightworldMenu.Instance.QueuedCharacterCalls[l] = NewCalls[l];
                }
            }
            for (int m = 0; m < LightworldMenu.Instance.CallMenuNames.Length; m++)
            {
                if (m < CallNames.Length && CallNames[m] != null)
                {
                    LightworldMenu.Instance.CallMenuNames[m] = CallNames[m];
                    LightworldMenu.Instance.CallChatIndexes[m] = 0;
                }
                else
                {
                    LightworldMenu.Instance.CallMenuNames[m] = "";
                }
            }
        }
        if (ClearCalls)
        {
            for (int n = 0; n < LightworldMenu.Instance.CallChatIndexes.Length; n++)
            {
                LightworldMenu.Instance.CallChatIndexes[n] = 0;
            }
            for (int num = 0; num < LightworldMenu.Instance.CallMenuNames.Length; num++)
            {
                LightworldMenu.Instance.CallMenuNames[num] = "";
            }
        }
        if (PlaySongOnLevelLoad)
        {
            if (Song != null)
            {
                if (MusicManager.Instance.source.clip != null)
                {
                    if (MusicManager.Instance.source.clip != Song)
                    {
                        MusicManager.PlaySong(Song, FadePreviousSong: false, 0f);
                    }
                }
                else
                {
                    MusicManager.PlaySong(Song, FadePreviousSong: false, 0f);
                }
            }
            else
            {
                MusicManager.StopSong(Fade: true, 1f);
            }
        }
        if (RoomContainsPartyMembers)
        {
            foreach (ActivePartyMember activePartyMember2 in PartyMemberSystem.Instance.ActivePartyMembers)
            {
                activePartyMember2.PartyMemberFollowSettings.RotateSusieToDirection(PlayerStartRotation);
            }
        }
        GonerMenu.Instance.CanOpenGonerMenu = !DisableGonerMenu;
        if (SetNewActivity)
        {
            Discord_Controller.instance.ChangeActivity(ActivityType.Playing, "", State, Details, LargeImage, LargeImageText, VesselText);
        }
    }

    private void AttemptNewEntranceSpawn()
    {
        int @int = PlayerPrefs.GetInt("Game_PreviousVistedRoom", 0);
        bool flag = false;
        PreviousStartPosition[] spawnPositions = RoomPositions.SpawnPositions;
        foreach (PreviousStartPosition previousStartPosition in spawnPositions)
        {
            if (previousStartPosition.PreviousSceneIndex != @int)
            {
                continue;
            }
            flag = true;
            PlayerManager.Instance.transform.position = previousStartPosition.StartPosition;
            PlayerManager.Instance._PMove.RotatePlayerAnim(previousStartPosition.KrisStartRotation);
            CameraManager.instance.transform.position = new Vector3(previousStartPosition.CameraStartPosition.x, previousStartPosition.CameraStartPosition.y, -10f);
            if (!RoomContainsPartyMembers || PartyMemberSystem.Instance.ActivePartyMembers.Count <= 0)
            {
                continue;
            }
            foreach (ActivePartyMember activePartyMember in PartyMemberSystem.Instance.ActivePartyMembers)
            {
                activePartyMember.PartyMemberFollowSettings.transform.position = previousStartPosition.StartPosition + previousStartPosition.KrisStartRotation / 10f;
                activePartyMember.PartyMemberFollowSettings.RotateSusieToDirection(previousStartPosition.KrisStartRotation);
            }
        }
        if (flag)
        {
            return;
        }
        PlayerManager.Instance.transform.position = RoomPositions.DefaultStartPosition;
        PlayerManager.Instance._PMove.RotatePlayerAnim(RoomPositions.DefaultKrisRotation);
        CameraManager.instance.transform.position = new Vector3(RoomPositions.DefaultCameraStartPosition.x, RoomPositions.DefaultCameraStartPosition.y, -10f);
        if (!RoomContainsPartyMembers || PartyMemberSystem.Instance.ActivePartyMembers.Count <= 0)
        {
            return;
        }
        foreach (ActivePartyMember activePartyMember2 in PartyMemberSystem.Instance.ActivePartyMembers)
        {
            activePartyMember2.PartyMemberFollowSettings.transform.position = RoomPositions.DefaultStartPosition + RoomPositions.DefaultKrisRotation;
            activePartyMember2.PartyMemberFollowSettings.RotateSusieToDirection(RoomPositions.DefaultKrisRotation);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (RoomHasMultipleEntrances)
        {
            PreviousStartPosition[] spawnPositions = RoomPositions.SpawnPositions;
            foreach (PreviousStartPosition obj in spawnPositions)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(RoomPositions.DefaultStartPosition, 0.5f);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(obj.StartPosition, 0.5f);
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(obj.CameraStartPosition, new Vector3(16.125f, 12f, 1f));
                Gizmos.DrawWireSphere(obj.CameraStartPosition, 0.5f);
            }
        }
    }
}
