using UnityEngine;

public class Trailer_TextboxPlayer : MonoBehaviour
{
    public INT_Chat myChat;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            myChat.RUN();
        }
    }
}
