using UnityEngine;

public class INT_PlayAudiosourceAfterChat : MonoBehaviour
{
    public INT_Chat Chat;

    public AudioSource Audiosource;

    public AudioClip clip;

    public int ChatIndexToPlay;

    public bool PlayedAudiosource;

    private void Update()
    {
        if (Chat.FinishedText && Chat.CurrentIndex == ChatIndexToPlay && !PlayedAudiosource)
        {
            PlayedAudiosource = true;
            Audiosource.clip = clip;
            Audiosource.Play();
        }
    }
}
