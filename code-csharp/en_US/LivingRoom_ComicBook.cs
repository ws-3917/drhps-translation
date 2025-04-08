using UnityEngine;

public class LivingRoom_ComicBook : MonoBehaviour
{
    public INT_Chat ComicbookChat;

    public CHATBOXTEXT NewChat;

    public SpriteRenderer ComicBookRenderer;

    public Sprite FoldedSprite;

    public AudioSource Source;

    public AudioClip FoldSound;

    public void FoldComicbook()
    {
        Source.PlayOneShot(FoldSound);
        ComicBookRenderer.sprite = FoldedSprite;
        ComicbookChat.Text = NewChat;
        PlayerPrefs.SetInt("PapyrusMeet_ComicSwan", 1);
    }
}
