using System.Collections;
using UnityEngine;

public class Log_Tape : MonoBehaviour
{
    public AudioSource audioSource;

    public Animator animator;

    private void Start()
    {
        StartCoroutine(WaitForAudioToFinish());
        if (audioSource.clip != null)
        {
            audioSource.clip.LoadAudioData();
        }
    }

    private IEnumerator WaitForAudioToFinish()
    {
        animator.speed = 0f;
        yield return new WaitForSeconds(1f);
        animator.speed = 1f;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        animator.speed = 0f;
    }
}
