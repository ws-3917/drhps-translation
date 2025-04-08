using UnityEngine;

public class NPCANIMATOR_UnparentObject : MonoBehaviour
{
    public GameObject GameobjectToUnparent;

    private void Unparent()
    {
        GameobjectToUnparent.transform.parent = null;
    }
}
