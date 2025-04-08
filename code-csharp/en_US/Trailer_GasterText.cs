using System.Collections;
using TMPro;
using UnityEngine;

public class Trailer_GasterText : MonoBehaviour
{
    public int columns = 5;

    public int rows = 5;

    public float spacing = 2f;

    public GameObject textPrefab;

    public float WobbleOffset;

    public float WobbleDelay;

    private Transform[,] textGrid;

    private void Start()
    {
        if (textPrefab == null)
        {
            Debug.LogError("Text Prefab is not assigned.");
            return;
        }
        textGrid = new Transform[columns, rows];
        GenerateTextGrid();
    }

    private void GenerateTextGrid()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject gameObject = Object.Instantiate(textPrefab, new Vector3((float)i * spacing, (float)j * spacing, 0f), Quaternion.identity, base.transform);
                TextMeshPro component = gameObject.GetComponent<TextMeshPro>();
                if (component != null)
                {
                    component.text = GetRandomLetter();
                    textGrid[i, j] = gameObject.transform;
                    StartCoroutine(Wobble(textGrid[i, j]));
                }
                else
                {
                    Debug.LogError("Text Prefab does not have a TextMeshPro component.");
                }
            }
        }
    }

    private string GetRandomLetter()
    {
        return ((char)Random.Range(65, 91)).ToString();
    }

    private IEnumerator Wobble(Transform textTransform)
    {
        Vector3 originalPosition = textTransform.localPosition;
        while (true)
        {
            Vector3 vector = new Vector3(Random.Range(0f - WobbleOffset, WobbleOffset), Random.Range(0f - WobbleOffset, WobbleOffset), 0f);
            textTransform.localPosition = originalPosition + vector;
            if (Random.Range(0, 15) == 5)
            {
                textTransform.GetComponent<TextMeshPro>().text = GetRandomLetter();
            }
            yield return new WaitForSeconds(WobbleDelay);
        }
    }
}
