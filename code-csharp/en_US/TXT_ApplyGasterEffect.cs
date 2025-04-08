using TMPro;
using UnityEngine;

public class TXT_ApplyGasterEffect : MonoBehaviour
{
    public float offsetDistance = 0.5f;

    public float transparency = 0.5f;

    private TextMeshPro[] clones = new TextMeshPro[4];

    private Vector3[] directions = new Vector3[4]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right
    };

    private void Start()
    {
        if (GetComponent<TextMeshPro>() == null)
        {
            Debug.LogError("No TextMeshPro component found on this GameObject!");
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            GameObject gameObject = Object.Instantiate(base.gameObject, base.transform.position + directions[i] * offsetDistance, Quaternion.identity, base.transform);
            Object.Destroy(gameObject.GetComponent<TXT_ApplyGasterEffect>());
            clones[i] = gameObject.GetComponent<TextMeshPro>();
            if (clones[i] == null)
            {
                clones[i] = gameObject.GetComponentInChildren<TextMeshPro>();
            }
            if (clones[i] != null)
            {
                Color color = clones[i].color;
                color.a = transparency;
                clones[i].color = color;
            }
            else
            {
                Debug.LogError("Clone does not have a TextMeshPro component!");
            }
        }
    }

    private void Update()
    {
        TextMeshPro component = GetComponent<TextMeshPro>();
        if (component == null)
        {
            return;
        }
        TextMeshPro[] array = clones;
        foreach (TextMeshPro textMeshPro in array)
        {
            if (textMeshPro != null)
            {
                textMeshPro.text = component.text;
            }
        }
    }
}
