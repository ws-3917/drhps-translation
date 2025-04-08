using UnityEngine;

public class TRIG_REMOVEGAMEOBJECT : MonoBehaviour
{
    public GameObject[] Objects;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            GameObject[] objects = Objects;
            for (int i = 0; i < objects.Length; i++)
            {
                Object.Destroy(objects[i]);
            }
        }
    }
}
