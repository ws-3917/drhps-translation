using TMPro;
using UnityEngine;

public class deletemelater : MonoBehaviour
{
    public TextMeshPro text;

    public int maxvisiblecharacters;

    private void Update()
    {
        text.maxVisibleCharacters = maxvisiblecharacters;
        MonoBehaviour.print($"max visible characters: {text.maxVisibleCharacters}");
        MonoBehaviour.print($"text length: {text.text.Length}");
    }
}
