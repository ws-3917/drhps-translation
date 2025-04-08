using UnityEngine;

public class Outlawed_MusicTransitioner : MonoBehaviour
{
    [SerializeField]
    private AudioClip mus_greenroom_backstage;

    [SerializeField]
    private AudioClip mus_greenroom;

    private void Start()
    {
        if (MusicManager.Instance.source.clip == mus_greenroom_backstage)
        {
            float time = MusicManager.Instance.source.time;
            MusicManager.PlaySong(mus_greenroom, FadePreviousSong: false, 0f);
            MusicManager.Instance.source.time = time;
        }
    }
}
