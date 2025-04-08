using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UI_CopyText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TargetText;

    [SerializeField]
    private TextMeshProUGUI thisText;

    [Range(0f, 1f)]
    [SerializeField]
    private float TextOpacity;

    [Header("-- Custom Colors --")]
    [SerializeField]
    private bool UseSpecificColor;

    [SerializeField]
    private Color SpecificColor;

    [Header("-- Additional Settings --")]
    [Header("True clone copies the target texts position, bounds and maxvisible characters")]
    [Header("Use sparingly, can be quite performance intenstive")]
    [SerializeField]
    private bool TrueClone;

    [SerializeField]
    private Vector3 Offset;

    [Header("Doesn't disable rich text itself, but removes the <> tags")]
    [SerializeField]
    private bool DisableRichTextTags;

    [SerializeField]
    private bool DarkenColorTags;

    [Range(0f, 1f)]
    [SerializeField]
    private float darkenAmount = 0.2f;

    private string[] preserveTags = new string[9] { "b", "color", "i", "u", "br", "scale", "lowercase", "uppercase", "link" };

    private void LateUpdate()
    {
        if (TargetText != null && thisText != null && TargetText.gameObject.activeSelf && TargetText.enabled && thisText.enabled && thisText.text != TargetText.text)
        {
            if (DisableRichTextTags)
            {
                thisText.text = RemoveRichTextTags(TargetText.text, preserveTags);
            }
            else
            {
                thisText.text = TargetText.text;
            }
            if (!UseSpecificColor)
            {
                thisText.color = new Color(TargetText.color.r, TargetText.color.g, TargetText.color.b, TextOpacity);
            }
            else
            {
                thisText.color = new Color(SpecificColor.r, SpecificColor.g, SpecificColor.b, SpecificColor.a);
            }
        }
        if (TrueClone)
        {
            thisText.maxVisibleCharacters = TargetText.maxVisibleCharacters;
            thisText.margin = TargetText.margin;
            thisText.transform.position = TargetText.transform.position + Offset;
            thisText.fontSize = TargetText.fontSize;
            thisText.font = TargetText.font;
            thisText.enabled = TargetText.enabled;
            thisText.styleSheet = TargetText.styleSheet;
            thisText.textStyle = TargetText.textStyle;
        }
    }

    public string RemoveRichTextTags(string input, string[] preserveTags)
    {
        string text = string.Join("|", preserveTags);
        string pattern = "<(?!/?(?:" + text + ")\\b).*?>";
        if (DarkenColorTags)
        {
            input = Regex.Replace(input, "<color=([^>]+)>", delegate (Match match)
            {
                string value = match.Groups[1].Value;
                if (ColorUtility.TryParseHtmlString(value, out var color) || (value.StartsWith("#") && ColorUtility.TryParseHtmlString(value, out color)) || (!value.StartsWith("#") && ColorUtility.TryParseHtmlString("#" + value, out color)))
                {
                    color.r *= 1f - darkenAmount;
                    color.g *= 1f - darkenAmount;
                    color.b *= 1f - darkenAmount;
                    string text2 = ColorUtility.ToHtmlStringRGB(color);
                    return "<color=#" + text2 + ">";
                }
                return match.Value;
            });
        }
        return Regex.Replace(input, pattern, "");
    }
}
