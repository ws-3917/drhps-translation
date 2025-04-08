using TMPro;
using UnityEngine;

public class TEXTUI_SubmeshChecker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextUI;

    private void Update()
    {
        if (TextUI.transform.childCount > 0)
        {
            Debug.LogWarning("Submesh TextMeshPro Detected");
            Debug.LogWarning(TextUI.text);
            base.enabled = false;
        }
        if (TextUI.text.Contains('…'))
        {
            Debug.LogError("Incorrect elipse detected");
            Debug.LogWarning(TextUI.text);
        }
        if (TextUI.text.Contains('’'))
        {
            Debug.LogError("Incorrect apostrophe detected");
            Debug.LogWarning(TextUI.text);
        }
    }
}
