using System.Collections;
using UnityEngine;

public class IntroDisclaimerManager : MonoBehaviour
{
    [SerializeField]
    private ChatboxGoner WingGasterChatbox;

    [SerializeField]
    private CHATBOXTEXT WingGasterText;

    [SerializeField]
    private TRIG_LEVELTRANSITION levelTransition;

    [SerializeField]
    private AudioClip Song;

    private void Start()
    {
        StartCoroutine(DisclaimerStart());
        if (PlayerPrefs.GetInt("DisclaimerViewed", 0) == 1)
        {
            levelTransition.BeginTransition(0f);
        }
        else
        {
            PlayerPrefs.SetInt("DisclaimerViewed", 1);
        }
        MonoBehaviour.print("Disabled Disclaimer skip because currently in editor");
    }

    private IEnumerator DisclaimerStart()
    {
        MusicManager.PlaySong(Song, FadePreviousSong: true, 1f);
        yield return new WaitForSeconds(2f);
        WingGasterChatbox.RunText(WingGasterText, 0, null, ResetCurrentTextIndex: false);
    }

    public void TransitionToMM()
    {
        levelTransition.BeginTransition();
    }
}
