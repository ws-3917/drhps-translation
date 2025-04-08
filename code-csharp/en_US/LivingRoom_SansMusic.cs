using UnityEngine;

public class LivingRoom_SansMusic : MonoBehaviour
{
    [SerializeField]
    private AudioClip sansSong;

    private void Start()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }

    public void StartSansMusic()
    {
        MusicManager.PlaySong(sansSong, FadePreviousSong: false, 0f);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        LightworldMenu.Instance.CanOpenMenu = true;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }
}
