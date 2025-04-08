using UnityEngine;

public class Outlawed_BackstageElevatorCutscenes : MonoBehaviour
{
    [Header("-= Cutscene References =-")]
    [Header("- Sounds -")]
    [SerializeField]
    private AudioClip mus_greenroom_backstage;

    [SerializeField]
    private AudioClip mus_greenroom;

    private void Start()
    {
        if (MusicManager.Instance.source.clip == mus_greenroom)
        {
            float time = MusicManager.Instance.source.time;
            MusicManager.PlaySong(mus_greenroom_backstage, FadePreviousSong: false, 0f);
            MusicManager.Instance.source.time = time;
        }
    }

    private void Update()
    {
    }
}
