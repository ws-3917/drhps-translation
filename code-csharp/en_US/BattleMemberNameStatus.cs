using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleMemberNameStatus : MonoBehaviour
{
    public TextMeshProUGUI memberNameText;

    public Image HealthBar_PositiveImage;

    public Image HealthBar_NegativeImage;

    public void SetupMemberNameStatus(string memberName, float Health, float maxHealth)
    {
        memberNameText.text = memberName;
        float fillAmount = Mathf.Clamp01(Health / maxHealth);
        float fillAmount2 = Mathf.Clamp01((0f - Health) / maxHealth);
        HealthBar_PositiveImage.fillAmount = fillAmount;
        HealthBar_NegativeImage.fillAmount = fillAmount2;
    }
}
