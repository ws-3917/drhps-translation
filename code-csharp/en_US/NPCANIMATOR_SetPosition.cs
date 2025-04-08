using UnityEngine;

public class NPCANIMATOR_SetPosition : MonoBehaviour
{
    public GameObject GameObjecttoPosition;

    public Vector3 Pos;

    private void SetPosition()
    {
        GameObjecttoPosition.transform.position = Pos;
    }
}
