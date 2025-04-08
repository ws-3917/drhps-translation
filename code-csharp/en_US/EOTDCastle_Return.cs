using UnityEngine;

public class EOTDCastle_Return : MonoBehaviour
{
    [SerializeField]
    private INT_Chat ReturnChat;

    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        ReturnChat.CurrentIndex = 0;
        ReturnChat.CanUse = true;
        ReturnChat.FinishedText = false;
        ReturnChat.FirstTextPlayed = false;
        ReturnChat.RUN();
        other.transform.position = other.transform.position - Vector3.up / 5f;
        Debug.LogWarning("TODO - Require player to interact with lancer before being able to return");
    }

    public void ReturnToFountain()
    {
        ChatboxManager.Instance.EndText();
        LevelTransition.BeginTransition();
    }

    public void ChoiceNo()
    {
        ChatboxManager.Instance.EndText();
        ReturnChat.CanUse = false;
    }
}
