using UnityEngine;
using UnityEngine.SceneManagement;

public class TRIG_LEVELTRANSITION : MonoBehaviour
{
    public int LevelToGo;

    [Header("Multiplier, 1 = normal | 0.5 = half speed")]
    [SerializeField]
    private float TransitionSpeed = 1f;

    [SerializeField]
    private NewMainMenuManager.MainMenuStates TargetState = NewMainMenuManager.MainMenuStates.Hypothetical;

    [Header("-= Audio =-")]
    [SerializeField]
    private AudioClip PlaySoundOnTransition;

    [SerializeField]
    private float SoundVolume = 1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            PlayerPrefs.SetInt("Game_PreviousVistedRoom", SceneManager.GetActiveScene().buildIndex);
            BeginTransition(TransitionSpeed);
        }
    }

    public void BeginTransition(float Speed = 1f, bool UnpauseOnEnd = false)
    {
        if (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            ChatboxManager.Instance.EndText();
        }
        UI_FADE.Instance.StartFadeIn(LevelToGo, Speed, UnpauseOnEnd, TargetState);
        if (PlaySoundOnTransition != null)
        {
            PlayerManager.Instance.PlayerAudioSource.PlayOneShot(PlaySoundOnTransition, SoundVolume);
        }
    }

    public void SimpleBeginTransition()
    {
        if (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            ChatboxManager.Instance.EndText();
        }
        UI_FADE.Instance.StartFadeIn(LevelToGo, TransitionSpeed, UnpauseOnEnd: false, TargetState);
        if (PlaySoundOnTransition != null)
        {
            PlayerManager.Instance.PlayerAudioSource.PlayOneShot(PlaySoundOnTransition, SoundVolume);
        }
    }
}
