using UnityEngine;

public class NPCANIMATOR_PlaySound : MonoBehaviour
{
    public AudioSource audiosource;

    public AudioClip[] clips;

    private void PlaySound(int index)
    {
        audiosource.PlayOneShot(clips[index]);
    }
}
