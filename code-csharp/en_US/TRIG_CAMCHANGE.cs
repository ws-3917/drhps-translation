using UnityEngine;

public class TRIG_CAMCHANGE : MonoBehaviour
{
    public CameraManager Camera;

    public bool CameraXResult;

    public bool CameraYResult;

    public bool OnTriggerExit;

    public bool XOnTriggerExit;

    public bool YOnTriggerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            Camera.FollowPlayerX = CameraXResult;
            Camera.FollowPlayerY = CameraYResult;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>() && OnTriggerExit)
        {
            Camera.FollowPlayerX = XOnTriggerExit;
            Camera.FollowPlayerY = YOnTriggerExit;
        }
    }
}
