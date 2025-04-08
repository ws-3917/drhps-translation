using UnityEngine;

public class MusicPreloader : MonoBehaviour
{
    [Header("-= Settings =-")]
    [Header("Whether to preload (True) or unload (False)")]
    [SerializeField]
    private bool PreloadClips = true;

    [SerializeField]
    private AudioClip[] TargetClips;

    private void Awake()
    {
        if (PreloadClips)
        {
            AudioClip[] targetClips = TargetClips;
            for (int i = 0; i < targetClips.Length; i++)
            {
                targetClips[i].LoadAudioData();
            }
        }
        else
        {
            AudioClip[] targetClips = TargetClips;
            for (int i = 0; i < targetClips.Length; i++)
            {
                targetClips[i].UnloadAudioData();
            }
        }
        Object.Destroy(this);
    }
}
