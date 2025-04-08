using UnityEngine;

public class EOTDRehabRoom_Radio : MonoBehaviour
{
    [SerializeField]
    private AudioClip SpamtonSong;

    [SerializeField]
    private AudioClip JevilSong;

    [SerializeField]
    private SpriteRenderer RadioSprite;

    [SerializeField]
    private INT_Generic Interaction;

    private int RehabRoomSong;

    private bool PreviousInteraction;

    private void Start()
    {
        RehabRoomSong = PlayerPrefs.GetInt("EOTD_RehabRoomSong", 0);
        if (RehabRoomSong == 0)
        {
            PreviousInteraction = false;
            Interaction.Interacted = false;
        }
        else
        {
            PreviousInteraction = true;
            Interaction.Interacted = true;
        }
        PlayMusic();
    }

    private void PlayMusic()
    {
        if (RehabRoomSong == 0)
        {
            MusicManager.PlaySong(JevilSong, FadePreviousSong: false, 0f);
            RadioSprite.flipX = false;
        }
        else
        {
            MusicManager.PlaySong(SpamtonSong, FadePreviousSong: false, 1f);
            RadioSprite.flipX = true;
        }
    }

    private void Update()
    {
        if (PreviousInteraction != Interaction.Interacted)
        {
            PreviousInteraction = Interaction.Interacted;
            if (PreviousInteraction)
            {
                RehabRoomSong = 1;
                PlayMusic();
                PlayerPrefs.SetInt("EOTD_RehabRoomSong", 1);
            }
            else
            {
                RehabRoomSong = 0;
                PlayMusic();
                PlayerPrefs.SetInt("EOTD_RehabRoomSong", 0);
            }
        }
    }
}
