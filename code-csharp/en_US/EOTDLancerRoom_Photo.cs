using UnityEngine;

public class EOTDLancerRoom_Photo : MonoBehaviour
{
    [SerializeField]
    private INT_Generic Interactable;

    [SerializeField]
    private AudioClip InteractSound;

    [SerializeField]
    private GameObject NewSprite;

    private void Update()
    {
        if (Interactable.Interacted)
        {
            PlayerManager.Instance.PlayerAudioSource.PlayOneShot(InteractSound);
            NewSprite.SetActive(value: true);
            Interactable.Interacted = false;
        }
    }
}
