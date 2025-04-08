using System.Collections;
using UnityEngine;

public class EOTDRehabRoom_Cungadero : MonoBehaviour
{
    [SerializeField]
    private Animator CarAnimator;

    [SerializeField]
    private Vector3[] CarWalkPositions;

    [SerializeField]
    private string[] CarAnimation;

    [SerializeField]
    private int CarWalkIndex;

    [SerializeField]
    private INT_Chat CarChat;

    [SerializeField]
    private AudioSource CarSource;

    [SerializeField]
    private AudioClip[] CarSounds;

    [SerializeField]
    private BoxCollider2D CarCollision;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [SerializeField]
    private EOTDRehabRoom_GenericCharacterAnimations RoomNPCs;

    [SerializeField]
    private int CutsceneIndex;

    private void Start()
    {
        if (PlayerPrefs.GetInt("EOTD_Car", 0) != 0)
        {
            CarChat.CurrentIndex = 2;
        }
    }

    private void Update()
    {
        if (CutsceneIndex != 0)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
        if (CutsceneIndex != 2)
        {
            return;
        }
        if (CarWalkIndex < CarWalkPositions.Length)
        {
            if (base.transform.position != CarWalkPositions[CarWalkIndex])
            {
                base.transform.position = Vector3.MoveTowards(base.transform.position, CarWalkPositions[CarWalkIndex], 4f * Time.deltaTime);
                CarAnimator.Play(CarAnimation[CarWalkIndex]);
            }
            else
            {
                CarWalkIndex++;
            }
        }
        if (CarWalkIndex >= 2 && base.transform.position == CarWalkPositions[2])
        {
            CutsceneIndex = 3;
            CarChat.CanUse = true;
            CarChat.FinishedText = false;
            CarChat.FirstTextPlayed = false;
            CarChat.CurrentIndex = 1;
            CarChat.RUN();
        }
        PlayerManager.Instance._PMove.RotatePlayerAnimTowardsPosition(CarAnimator.transform.position);
        Ralsei.RotateSusieTowardsPosition(CarAnimator.transform.position);
        Susie.RotateSusieTowardsPosition(CarAnimator.transform.position);
    }

    public void StartCutscene()
    {
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei).PartyMemberFollowSettings;
        PlayerPrefs.SetInt("EOTD_Car", 1);
        CutsceneIndex = 1;
        StartCoroutine(LegReveal());
        CarCollision.enabled = false;
        RoomNPCs.SpamtonAnim_Idle_Left();
    }

    public void EndCutscene()
    {
        CutsceneIndex = 0;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        DarkworldMenu.Instance.CanOpenMenu = true;
        RoomNPCs.SpamtonAnim_Idle_Left();
    }

    private IEnumerator LegReveal()
    {
        yield return new WaitForSeconds(0.5f);
        CarAnimator.Play("rehabroom_car_standidle");
        CarSource.PlayOneShot(CarSounds[0]);
        yield return new WaitForSeconds(1f);
        CutsceneIndex = 2;
        CarSource.PlayOneShot(CarSounds[1]);
    }
}
