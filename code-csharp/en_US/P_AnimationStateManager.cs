using UnityEngine;

public class P_AnimationStateManager : MonoBehaviour
{
    public string _AnimationState = "Base";

    public bool CanUseBasePlayerAnimations;

    public bool FootstepsEnabled;

    private bool FootstepCooldown;

    [SerializeField]
    private AudioSource PlayerAudioSource;

    [SerializeField]
    private AudioClip[] PlayerAnimationAudioClips;

    [SerializeField]
    private AudioClip[] FootstepSounds;

    private void Update()
    {
        CheckAnimationState();
    }

    private void CheckAnimationState()
    {
        if (_AnimationState == "Base")
        {
            CanUseBasePlayerAnimations = true;
        }
        else
        {
            CanUseBasePlayerAnimations = false;
        }
    }

    public void PlayPlayerAnimationSound(int Index)
    {
        if (Index >= 0 && Index <= PlayerAnimationAudioClips.Length)
        {
            PlayerAudioSource.PlayOneShot(PlayerAnimationAudioClips[Index]);
        }
        else
        {
            Debug.LogWarning("Attempted to play player animation sound outside of index");
        }
    }

    public void PlayFootstepSound(int UseFootstepEnabledBool)
    {
        if (UseFootstepEnabledBool == 1)
        {
            if (FootstepsEnabled && !FootstepCooldown)
            {
                PlayerAudioSource.Stop();
                PlayerAudioSource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Length)]);
            }
        }
        else if (!FootstepCooldown)
        {
            PlayerAudioSource.Stop();
            PlayerAudioSource.PlayOneShot(FootstepSounds[Random.Range(0, FootstepSounds.Length)]);
        }
    }
}
