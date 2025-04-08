using System.Collections;
using UnityEngine;

public class UI_EOTD_LancerPostcard : MonoBehaviour
{
    public GameObject PostcardUI;

    public bool CurrentlyActive;

    private bool CooldownEnabled;

    private int PageIndex;

    [SerializeField]
    private AudioClip PageTurnSFX;

    [SerializeField]
    private Animator PostcardAnimator;

    [SerializeField]
    private string[] PageAnimations;

    [SerializeField]
    private CHATBOXTEXT SusieText;

    [SerializeField]
    private INT_Chat PostcardChat;

    public void StartPostcard()
    {
        DarkworldMenu.Instance.CanOpenMenu = false;
        PostcardUI.SetActive(value: true);
        PageIndex = 0;
        CurrentlyActive = true;
        CooldownEnabled = false;
        PostcardAnimator.Play("LancerPostcard_Idle");
    }

    public void EndPostcard()
    {
        DarkworldMenu.Instance.CanOpenMenu = true;
        PostcardUI.SetActive(value: false);
        PageIndex = 0;
        CurrentlyActive = false;
        CooldownEnabled = false;
        PostcardAnimator.Play("LancerPostcard_Idle");
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        PostcardChat.FirstTextPlayed = false;
        PostcardChat.CurrentIndex = 0;
        PostcardChat.FinishedText = false;
        PostcardChat.Text = SusieText;
        PostcardChat.RUN();
    }

    private void Update()
    {
        if (CurrentlyActive && !CooldownEnabled && Input.GetKeyDown(PlayerInput.Instance.Key_Right))
        {
            StartCoroutine(InputCooldown());
            PlayerManager.Instance.PlayerAudioSource.PlayOneShot(PageTurnSFX);
            if (PageIndex <= 1)
            {
                if (PostcardUI.activeSelf)
                {
                    PostcardAnimator.Play(PageAnimations[PageIndex]);
                }
            }
            else
            {
                EndPostcard();
            }
            PageIndex++;
        }
        if (CurrentlyActive)
        {
            PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        }
    }

    private IEnumerator InputCooldown()
    {
        CooldownEnabled = true;
        yield return new WaitForSeconds(1f);
        CooldownEnabled = false;
    }
}
