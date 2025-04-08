using UnityEngine;

public class DEBUG_RemoveOnStart : MonoBehaviour
{
    private void Awake()
    {
        Object.Destroy(base.gameObject);
    }
}
