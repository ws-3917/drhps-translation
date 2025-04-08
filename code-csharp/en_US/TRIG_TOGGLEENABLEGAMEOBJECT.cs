using UnityEngine;

public class TRIG_TOGGLEENABLEGAMEOBJECT : MonoBehaviour
{
    public GameObject[] TargetGameobject;

    public bool CollisonResult;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            GameObject[] targetGameobject = TargetGameobject;
            for (int i = 0; i < targetGameobject.Length; i++)
            {
                targetGameobject[i].SetActive(CollisonResult);
            }
        }
    }
}
