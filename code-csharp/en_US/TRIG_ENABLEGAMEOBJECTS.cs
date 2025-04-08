using UnityEngine;

public class TRIG_ENABLEGAMEOBJECTS : MonoBehaviour
{
    public GameObject[] Objects;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            GameObject[] objects = Objects;
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(value: true);
            }
        }
    }
}
