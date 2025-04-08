using System.Collections;
using UnityEngine;

public class TRB_GasterSpeechIntro : MonoBehaviour
{
    [Header("-= References =-")]
    [SerializeField]
    private ChatboxSusieFountain gasterChatbox;

    [SerializeField]
    private CHATBOXTEXT gasterDialogue;

    [SerializeField]
    private TRB_TorielWakeKris WakeKrisCutscene;

    [SerializeField]
    private AudioClip snd_risingocean;

    private void Start()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1f);
        gasterChatbox.RunText(gasterDialogue, 0, null, ResetCurrentTextIndex: false);
        while (gasterChatbox.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        gasterChatbox.RunText(gasterDialogue, 1, null, ResetCurrentTextIndex: false);
        while (gasterChatbox.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        MusicManager.StopSong(Fade: false, 0f);
        yield return new WaitForSeconds(2f);
        gasterChatbox.FountainTextSpeedMarkiplier = 0.0125f;
        gasterChatbox.RunText(gasterDialogue, 2, null, ResetCurrentTextIndex: false);
        CutsceneUtils.PlaySound(snd_risingocean, CutsceneUtils.DRH_MixerChannels.Music);
        yield return new WaitForSeconds(2f);
        gasterChatbox.EndText();
        WakeKrisCutscene.StartCutscene();
    }
}
