using UnityEngine;

public class TRIG_PLAYSOUND : MonoBehaviour
{
    public AudioSource Source;

    public AudioClip clip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            Source.clip = clip;
            Source.Play();
        }
    }
}
