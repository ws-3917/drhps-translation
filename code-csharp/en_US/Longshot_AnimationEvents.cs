using UnityEngine;

public class Longshot_AnimationEvents : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] LongshotSoundClips;

    public void Longshot_PlaySound(int index)
    {
        if (index >= 0 && index <= LongshotSoundClips.Length)
        {
            float volume = 0.4f;
            if (ChatboxManager.Instance.ChatIsCurrentlyRunning)
            {
                volume = 0.25f;
            }
            CutsceneUtils.PlaySound(LongshotSoundClips[index], CutsceneUtils.DRH_MixerChannels.Effect, volume);
        }
    }

    public void ShakeLongshot(float Intensity)
    {
        CutsceneUtils.ShakeTransform(base.transform, Intensity, 1f);
    }
}
