using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    [SerializeField]
    public AudioSource source;

    public static MusicManager Instance => instance;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(base.gameObject);
    }

    private void Update()
    {
    }

    public static void PlaySong(AudioClip SongClip, bool FadePreviousSong, float FadeTime, AudioSource CustomSource = null)
    {
        if (CustomSource == null)
        {
            CustomSource = instance.source;
        }
        if (!(SongClip == CustomSource.clip) || !CustomSource.isPlaying)
        {
            if (FadePreviousSong)
            {
                instance.StartCoroutine(FadeToNewSong(FadeTime, SongClip, CustomSource));
                return;
            }
            CustomSource.clip = SongClip;
            CustomSource.Play();
        }
    }

    public static void PauseMusic()
    {
        if (instance.source.clip != null)
        {
            instance.source.Pause();
        }
    }

    public static void ResumeMusic()
    {
        if (instance.source.clip != null)
        {
            instance.source.UnPause();
        }
    }

    public static void StopSong(bool Fade, float FadeTime, AudioSource CustomSource = null)
    {
        if (Fade)
        {
            instance.StartCoroutine(FadeOut(FadeTime, CustomSource));
        }
        else if (CustomSource != null)
        {
            CustomSource.Stop();
        }
        else
        {
            instance.source.Stop();
        }
    }

    public static IEnumerator FadeOut(float FadeTime, AudioSource CustomSource = null)
    {
        if (CustomSource == null)
        {
            CustomSource = instance.source;
        }
        float startVolume = CustomSource.volume;
        while (CustomSource.volume > 0f)
        {
            CustomSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        CustomSource.Stop();
        CustomSource.volume = startVolume;
    }

    public static IEnumerator FadeToNewSong(float FadeTime, AudioClip Song, AudioSource CustomSource = null)
    {
        if (CustomSource == null)
        {
            CustomSource = instance.source;
        }
        float startVolume = CustomSource.volume;
        while (CustomSource.volume > 0f)
        {
            CustomSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        CustomSource.Stop();
        CustomSource.clip = Song;
        CustomSource.Play();
        while (CustomSource.volume < startVolume)
        {
            CustomSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
    }
}
