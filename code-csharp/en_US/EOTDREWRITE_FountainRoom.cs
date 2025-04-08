using System.Collections;
using UnityEngine;

public class EOTDREWRITE_FountainRoom : MonoBehaviour
{
    public int CutsceneIndex;

    [Header("-- Cutscene References --")]
    [SerializeField]
    private CameraManager PlayerCamera;

    [Header("Characters")]
    [SerializeField]
    private PlayerManager Kris;

    [SerializeField]
    private Susie_Follower Susie;

    [SerializeField]
    private Susie_Follower Ralsei;

    [Header("Character Positions")]
    [SerializeField]
    private Transform SusieStartPos;

    [SerializeField]
    private Transform RalseiStartPos;

    [Header("UI")]
    [SerializeField]
    private GameObject CutsceneFade;

    [Header("-- Cutscene Settings --")]
    [SerializeField]
    private AudioClip Music_Fountain;

    [SerializeField]
    private AudioClip Music_Friendship;

    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    private void Start()
    {
        Kris = PlayerManager.Instance;
        IncrementCutsceneIndex();
        StartCoroutine(Cutscene());
    }

    private void Update()
    {
        if (CutsceneIndex > 0)
        {
            Kris._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
    }

    public void IncrementCutsceneIndex()
    {
        CutsceneIndex++;
    }

    private IEnumerator Cutscene()
    {
        yield return null;
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_SusieDarkworld).PartyMemberFollowSettings;
        Ralsei = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Ralsei).PartyMemberFollowSettings;
        Susie.transform.position = new Vector2(-8f, 0.092f);
        Ralsei.transform.position = new Vector2(-8f, -2.5f);
        if (Susie == null)
        {
            MonoBehaviour.print("Susie Missing?");
        }
        if (Ralsei == null)
        {
            MonoBehaviour.print("Ralsei Missing?");
        }
        Susie.transform.Find("Shadow").gameObject.SetActive(value: true);
        Ralsei.transform.Find("Shadow").gameObject.SetActive(value: true);
        if (PlayerPrefs.GetInt("EOTD_FinishedArgueCutscene", 0) == 0)
        {
            Susie.FollowingEnabled = false;
            Ralsei.FollowingEnabled = false;
            Susie.transform.position = SusieStartPos.position;
            Ralsei.transform.position = RalseiStartPos.position;
            Susie.RotateSusieToDirection(Vector2.right);
            Ralsei.RotateSusieToDirection(Vector2.down);
            PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.left);
            CameraManager.instance.transform.position = new Vector3(Ralsei.transform.position.x, CameraManager.instance.transform.position.y, CameraManager.instance.transform.position.z);
            CutsceneIndex = 1;
            PlayerPrefs.SetInt("EOTD_FinishedArgueCutscene", 1);
            MusicManager.PlaySong(Music_Fountain, FadePreviousSong: false, 0f);
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.5f);
            CutsceneUtils.RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Shock);
            while (CutsceneIndex != 2)
            {
                yield return null;
            }
            SusieIdle_Up();
            KrisIdle_Up();
            yield return new WaitForSeconds(1f);
            UI_FADE.Instance.StartFadeOut(0.35f);
            CutsceneFade.SetActive(value: false);
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Horror);
            yield return new WaitForSeconds(2f);
            CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: false);
            while (CutsceneIndex != 3)
            {
                yield return null;
            }
            DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Crying);
            while (PlayerCamera.transform.position.x != Kris.transform.position.x)
            {
                yield return null;
                Vector3 target = new Vector3(Kris.transform.position.x, PlayerCamera.transform.position.y, PlayerCamera.transform.position.z);
                PlayerCamera.transform.position = Vector3.MoveTowards(PlayerCamera.transform.position, target, 3f * Time.deltaTime);
            }
            PlayerCamera.FollowPlayerX = true;
            CutsceneIndex = 0;
            Kris.ResetToGameState();
            Susie.FollowingEnabled = true;
            Ralsei.FollowingEnabled = true;
            DarkworldMenu.Instance.CanOpenMenu = true;
            PlayerPrefs.SetInt("EOTD_FinishedArgueCutscene", 1);
        }
        else
        {
            CutsceneFade.SetActive(value: false);
            MusicManager.PlaySong(Music_Friendship, FadePreviousSong: false, 0f);
            CutsceneIndex = 0;
            PlayerCamera.FollowPlayerX = true;
            CutsceneIndex = 0;
            Kris.ResetToGameState();
            Susie.FollowingEnabled = true;
            Ralsei.FollowingEnabled = true;
            DarkworldMenu.Instance.CanOpenMenu = true;
            Kris.transform.position = new Vector2(-7.95f, -1.35f);
            Susie.transform.position = Kris.transform.position;
            Ralsei.transform.position = Kris.transform.position;
        }
    }

    public void SusieIdle_Right()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.right);
    }

    public void SusieIdle_Up()
    {
        Susie.SusieAnimator.Play("Idle");
        Susie.RotateSusieToDirection(Vector2.up);
    }

    public void SusieConfident_Right()
    {
        Susie.SusieAnimator.Play("SusieDarkworld_Proud_Right");
    }

    public void RalseiIdle_Down()
    {
        Ralsei.SusieAnimator.Play("Idle");
        Ralsei.RotateSusieToDirection(Vector2.down);
    }

    public void RalseiIdle_Up()
    {
        Ralsei.SusieAnimator.Play("Idle");
        Ralsei.RotateSusieToDirection(Vector2.up);
    }

    public void RalseiIdleBlush_Down()
    {
        Ralsei.SusieAnimator.Play("Ralsei_Blush_Down");
    }

    public void KrisIdle_Up()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.up);
    }

    public void KrisIdle_Left()
    {
        Kris._PMove.RotatePlayerAnim(Vector2.left);
    }
}
