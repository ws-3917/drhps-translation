using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartyMemberSystem : MonoBehaviour
{
    [Header("-= Stats =-")]
    public List<ActivePartyMember> ActivePartyMembers = new List<ActivePartyMember>();

    [Header("-= Default PartyMembers =-")]
    public PartyMember Default_NoelleDarkworld;

    public PartyMember Default_SusieDarkworld;

    public PartyMember Default_Susie;

    public PartyMember Default_Ralsei;

    public PartyMember Default_Kris;

    private static PartyMemberSystem instance;

    public static PartyMemberSystem Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(instance);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    public void AddPartyMember(PartyMember Member, int StartingHealth = -1)
    {
        if (Member != null)
        {
            GameObject gameObject = Object.Instantiate(Member.PartyMemberPrefab, base.transform);
            if (gameObject != null)
            {
                ActivePartyMember activePartyMember = new ActivePartyMember();
                activePartyMember.PartyMemberDescription = Member;
                activePartyMember.PartyMemberTransform = gameObject.transform;
                if (StartingHealth == -1)
                {
                    activePartyMember.CurrentHealth = Member.MaximumHealth;
                }
                else
                {
                    activePartyMember.CurrentHealth = StartingHealth;
                }
                gameObject.name = Member.PartyMemberName + " " + SceneManager.GetActiveScene().name;
                ActivePartyMembers.Add(activePartyMember);
                int num = ActivePartyMembers.IndexOf(activePartyMember);
                Susie_Follower component = gameObject.transform.GetComponent<Susie_Follower>();
                if (component != null)
                {
                    activePartyMember.PartyMemberFollowSettings = component;
                    component.SusieAnimator.keepAnimatorStateOnDisable = true;
                    component.delay = 0.5f + 0.625f * (float)num;
                    activePartyMember.PartyMemberTransform.position = PlayerManager.Instance.transform.position;
                }
                else
                {
                    Debug.LogWarning("Failed to set Party Member Walk Offset! ( No Susie_Follower in Prefab )");
                }
            }
            else
            {
                Debug.LogWarning("Failed to create Party Member! ( No PartyMemberPrefab )");
            }
        }
        else
        {
            Debug.LogWarning("Failed to create Party Member! ( No Member Provided )");
        }
    }

    public void RemovePartyMember(PartyMember SpecificMember = null, bool RemoveSpecificIndex = false, int SpecificMemberIndex = 0)
    {
        if (RemoveSpecificIndex)
        {
            if (SpecificMemberIndex <= ActivePartyMembers.Count)
            {
                Object.Destroy(ActivePartyMembers[SpecificMemberIndex].PartyMemberTransform.gameObject);
                ActivePartyMembers.RemoveAt(SpecificMemberIndex);
                ResetAllMemberFollowerDelays();
            }
            else
            {
                Debug.LogWarning("Failed to remove Party Member! ( SpecificedMemberIndex is greater than ActivePartyMembers list )");
            }
            return;
        }
        ActivePartyMember activePartyMember = HasMemberInParty(SpecificMember);
        if (activePartyMember != null)
        {
            Object.Destroy(activePartyMember.PartyMemberTransform.gameObject);
            ActivePartyMembers.Remove(activePartyMember);
            ResetAllMemberFollowerDelays();
        }
        else
        {
            Debug.LogWarning("Failed to remove Party Member! ( SpecificMember is not in ActivePartyMembers )");
        }
    }

    public void AllPartyMemberPlayAnimation(string animationName)
    {
        foreach (ActivePartyMember activePartyMember in ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.SusieAnimator.Play(animationName);
        }
    }

    public void AllPartyMemberFaceDirection(Vector2 Direction)
    {
        foreach (ActivePartyMember activePartyMember in ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.RotateSusieToDirection(Direction);
        }
    }

    public void AllPartyMemberFacePosition(Vector2 targetPosition)
    {
        foreach (ActivePartyMember activePartyMember in ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.RotateSusieTowardsPosition(targetPosition);
        }
    }

    public void RemoveAllPartyMember()
    {
        for (int i = 0; i < ActivePartyMembers.Count; i++)
        {
            Object.Destroy(ActivePartyMembers[i].PartyMemberTransform.gameObject);
        }
        ActivePartyMembers.Clear();
        ResetAllMemberFollowerDelays();
    }

    public ActivePartyMember HasMemberInParty(PartyMember Member)
    {
        if (Member == null)
        {
            return null;
        }
        ActivePartyMember result = null;
        for (int i = 0; i < ActivePartyMembers.Count; i++)
        {
            if (ActivePartyMembers[i].PartyMemberDescription == Member)
            {
                result = ActivePartyMembers[i];
            }
        }
        return result;
    }

    public void ResetAllMemberFollowerDelays()
    {
        for (int i = 0; i < ActivePartyMembers.Count; i++)
        {
            if (ActivePartyMembers[i].PartyMemberFollowSettings != null)
            {
                ActivePartyMembers[i].PartyMemberFollowSettings.positions.Clear();
                ActivePartyMembers[i].PartyMemberFollowSettings.rotations.Clear();
                ActivePartyMembers[i].PartyMemberFollowSettings.delay = 0.5f + 0.625f * (float)i;
            }
        }
        ResetAllMembersToPlayer();
    }

    public void ResetAllMembersToPlayer()
    {
        if (ActivePartyMembers.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < ActivePartyMembers.Count; i++)
        {
            if (ActivePartyMembers[i] != null && ActivePartyMembers[i].PartyMemberFollowSettings != null)
            {
                ActivePartyMembers[i].PartyMemberFollowSettings.positions.Clear();
                ActivePartyMembers[i].PartyMemberFollowSettings.rotations.Clear();
                ActivePartyMembers[i].PartyMemberTransform.position = PlayerManager.Instance.transform.position;
            }
        }
    }

    public Transform GetSpecificMemberTransform(PartyMember Member)
    {
        ActivePartyMember activePartyMember = HasMemberInParty(Member);
        if (activePartyMember != null)
        {
            return activePartyMember.PartyMemberTransform;
        }
        Debug.LogWarning("Failed to get Party Member Transform! ( Member not found in ActivePartyMembers )");
        return null;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
    }

    public void SetAllPartyMembersActive(bool ActiveSelf)
    {
        foreach (ActivePartyMember activePartyMember in ActivePartyMembers)
        {
            activePartyMember.PartyMemberTransform.gameObject.SetActive(ActiveSelf);
        }
    }

    public void SetAllPartyMembersFollowing(bool Following)
    {
        foreach (ActivePartyMember activePartyMember in ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.FollowingEnabled = Following;
        }
    }

    public void SetAllPartyMemberStates(Susie_Follower.MemberFollowerStates State)
    {
        foreach (ActivePartyMember activePartyMember in ActivePartyMembers)
        {
            activePartyMember.PartyMemberFollowSettings.currentState = State;
        }
    }
}
