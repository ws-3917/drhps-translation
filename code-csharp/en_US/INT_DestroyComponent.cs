using UnityEngine;

public class INT_DestroyComponent : MonoBehaviour
{
    public Component[] ComponentsToDestroy;

    public bool HasGameObjects;

    public GameObject[] GameObjectsToDestroy;

    public void RUN()
    {
        Component[] componentsToDestroy = ComponentsToDestroy;
        for (int i = 0; i < componentsToDestroy.Length; i++)
        {
            Object.Destroy(componentsToDestroy[i]);
        }
        if (HasGameObjects)
        {
            GameObject[] gameObjectsToDestroy = GameObjectsToDestroy;
            for (int i = 0; i < gameObjectsToDestroy.Length; i++)
            {
                Object.Destroy(gameObjectsToDestroy[i]);
            }
        }
    }
}
