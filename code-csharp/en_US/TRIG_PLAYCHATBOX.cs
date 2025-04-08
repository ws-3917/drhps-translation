using System.Collections;
using UnityEngine;

public class TRIG_PLAYCHATBOX : MonoBehaviour
{
    [Header("WARNING! This can only run once!")]
    public INT_Chat Chat;

    public float TriggerDelay;

    public bool ChatHasRan;

    public bool DestroyOnCollide;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>() && !ChatHasRan)
        {
            ChatHasRan = true;
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(TriggerDelay);
        Chat.RUN();
        if (DestroyOnCollide)
        {
            Object.Destroy(base.gameObject);
        }
    }
}
