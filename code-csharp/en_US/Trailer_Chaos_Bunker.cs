using System.Collections;
using UnityEngine;

public class Trailer_Chaos_Bunker : MonoBehaviour
{
    [SerializeField]
    private Animator BunkerAnimator;

    [SerializeField]
    private INT_Generic Interaction;

    private bool PreviousInteraction;

    [SerializeField]
    private ParticleSystem VineParticle;

    [SerializeField]
    private AudioSource cutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneClips;

    private void Update()
    {
        if (PreviousInteraction != Interaction.Interacted)
        {
            PreviousInteraction = Interaction.Interacted;
            StartCoroutine(BunkerOpenAnimation());
        }
    }

    private IEnumerator BunkerOpenAnimation()
    {
        BunkerAnimator.Play("BunkerOpen");
        cutsceneSource.PlayOneShot(CutsceneClips[0]);
        yield return new WaitForSeconds(1f);
        VineParticle.Play();
    }
}
