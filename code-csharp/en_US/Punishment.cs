using System.Collections;
using UnityEngine;

public class Punishment : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer Uprank;

    [SerializeField]
    private Sprite Uprank_Down;

    [SerializeField]
    private Sprite Uprank_Left;

    [SerializeField]
    private Sprite Uprank_Right;

    [SerializeField]
    private INT_Chat chatter;

    private void Start()
    {
        StartCoroutine(text());
        UI_LoadingIcon.ToggleLoadingIcon(showIcon: false);
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }

    private IEnumerator text()
    {
        yield return new WaitForSeconds(3f);
        CutsceneUtils.FadeInSprite(Uprank);
        yield return new WaitForSeconds(1.5f);
        chatter.RUN();
        yield return new WaitForSeconds(1.5f);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        CutsceneUtils.FadeOutSprite(Uprank);
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }

    public void UprankPunishment_Down()
    {
        Uprank.sprite = Uprank_Down;
    }

    public void UprankPunishment_Left()
    {
        Uprank.sprite = Uprank_Left;
    }

    public void UprankPunishment_Right()
    {
        Uprank.sprite = Uprank_Right;
    }
}
