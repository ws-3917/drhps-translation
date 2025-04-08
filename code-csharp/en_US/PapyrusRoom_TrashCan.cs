using UnityEngine;

public class PapyrusRoom_TrashCan : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] TrashCanSounds;

    [SerializeField]
    private AudioSource TrashCanSource;

    [SerializeField]
    private SpriteRenderer TrashCanRenderer;

    [SerializeField]
    private Sprite TrashCanOpenSprite;

    [SerializeField]
    private Sprite TrashCanClosedSprite;

    public void TrashCan_Open()
    {
        MusicManager.Instance.source.Pause();
        TrashCanRenderer.sprite = TrashCanOpenSprite;
        TrashCanSource.PlayOneShot(TrashCanSounds[0]);
        TrashCanSource.clip = TrashCanSounds[1];
        TrashCanSource.Play();
        TrashCanSource.loop = true;
    }

    public void TrashCan_Close()
    {
        MusicManager.Instance.source.UnPause();
        TrashCanRenderer.sprite = TrashCanClosedSprite;
        TrashCanSource.loop = false;
        TrashCanSource.Stop();
        TrashCanSource.PlayOneShot(TrashCanSounds[0]);
    }
}
