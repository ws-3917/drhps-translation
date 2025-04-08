using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GonerMenu_Settings_Element : MonoBehaviour
{
    public string SettingsElementName = "Placeholder";

    [TextArea(5, 5)]
    public string SettingsDescription = "Placeholder Description, Lalalala";

    public bool CurrentlySelected;

    public bool CanBeCanceled = true;

    private bool oldCurrentSelected;

    [SerializeField]
    private RawImage[] RawImages_AffectedByColor;

    [SerializeField]
    private TextMeshProUGUI[] TMP_AffectedByColor;

    private void Update()
    {
        if (!base.gameObject.activeSelf || oldCurrentSelected == CurrentlySelected)
        {
            return;
        }
        oldCurrentSelected = CurrentlySelected;
        if (CurrentlySelected)
        {
            RawImage[] rawImages_AffectedByColor = RawImages_AffectedByColor;
            for (int i = 0; i < rawImages_AffectedByColor.Length; i++)
            {
                rawImages_AffectedByColor[i].color = Color.yellow;
            }
            TextMeshProUGUI[] tMP_AffectedByColor = TMP_AffectedByColor;
            for (int i = 0; i < tMP_AffectedByColor.Length; i++)
            {
                tMP_AffectedByColor[i].color = Color.yellow;
            }
        }
        else
        {
            RawImage[] rawImages_AffectedByColor = RawImages_AffectedByColor;
            for (int i = 0; i < rawImages_AffectedByColor.Length; i++)
            {
                rawImages_AffectedByColor[i].color = Color.white;
            }
            TextMeshProUGUI[] tMP_AffectedByColor = TMP_AffectedByColor;
            for (int i = 0; i < tMP_AffectedByColor.Length; i++)
            {
                tMP_AffectedByColor[i].color = Color.white;
            }
        }
    }
}
