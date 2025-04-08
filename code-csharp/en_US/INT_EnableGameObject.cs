using System.Collections;
using UnityEngine;

public class INT_EnableGameObject : MonoBehaviour
{
    public GameObject[] GameObjectsToEnable;

    public float EnableDelay;

    public void RUN()
    {
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(EnableDelay);
        GameObject[] gameObjectsToEnable = GameObjectsToEnable;
        for (int i = 0; i < gameObjectsToEnable.Length; i++)
        {
            gameObjectsToEnable[i].SetActive(value: true);
        }
    }
}
