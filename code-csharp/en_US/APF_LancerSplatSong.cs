using System.Collections;
using UnityEngine;

public class APF_LancerSplatSong : MonoBehaviour
{
    [SerializeField]
    private AudioClip snd_splatcer;

    public void PlayLancerSequence()
    {
        ChatboxManager.Instance.AllowInput = false;
        StartCoroutine(LancerSequence());
    }

    private IEnumerator LancerSequence()
    {
        while (ChatboxManager.Instance.TextIsCurrentlyTyping)
        {
            yield return null;
        }
        MusicManager.PauseMusic();
        CutsceneUtils.PlaySound(snd_splatcer, CutsceneUtils.DRH_MixerChannels.Music);
        yield return new WaitForSeconds(snd_splatcer.length + 0.1f);
        ChatboxManager.Instance.MimicInput_Confirm();
        ChatboxManager.Instance.AllowInput = true;
        MusicManager.ResumeMusic();
    }
}
