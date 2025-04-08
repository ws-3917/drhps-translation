using System.Collections;
using UnityEngine;

public class EOTDRouxlsRoom_FirstEntryDialogue : MonoBehaviour
{
    [SerializeField]
    private int HasRanPreviously;

    public INT_Chat chat;

    [SerializeField]
    private string PlayerPref = "EOTD_RouxlsRoomEntry";

    private void Start()
    {
        HasRanPreviously = PlayerPrefs.GetInt(PlayerPref, 0);
        StartCoroutine(IntroDelay());
    }

    private void Update()
    {
        if (HasRanPreviously == 1)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
            DarkworldMenu.Instance.CanOpenMenu = true;
            base.enabled = false;
        }
        else
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
            DarkworldMenu.Instance.CanOpenMenu = false;
        }
    }

    private IEnumerator IntroDelay()
    {
        yield return new WaitForSeconds(0.5f);
        if (HasRanPreviously == 0)
        {
            chat.RUN();
        }
    }

    public void EndCutscene()
    {
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        DarkworldMenu.Instance.CanOpenMenu = true;
        base.enabled = false;
        PlayerPrefs.SetInt(PlayerPref, 1);
    }
}
