using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleAttackBar : MonoBehaviour
{
    public BattlePartyMember TargetPartyMember;

    public BattleActiveEnemy TargetActiveEnemy;

    [SerializeField]
    private Image BarMemberIcon;

    [SerializeField]
    private RawImage AttackBarBackground;

    public RectTransform AttackBar;

    [SerializeField]
    private RectTransform CriticalPosition;

    public bool HasSetupPosition;

    public bool IsCurrentlyCritical;

    private void Update()
    {
        if (base.gameObject.activeSelf && HasSetupPosition)
        {
            UpdateBarPosition();
            if (CalculateAccuracy() == 150f)
            {
                IsCurrentlyCritical = true;
            }
            else
            {
                IsCurrentlyCritical = false;
            }
        }
    }

    public void PlayPartyMemberAttackAnimation()
    {
        if (TargetPartyMember != null)
        {
            BattleSystem.Instance.PartyMembers_MemberPlayAnimation(TargetPartyMember, "Attack");
        }
    }

    public float CalculateDamage(int Attack, float Accuracy, int EnemyDefense)
    {
        EnemyDefense = Mathf.Clamp(EnemyDefense, 1, int.MaxValue);
        return Mathf.Clamp((float)Attack * Accuracy / 20f - (float)(3 * EnemyDefense), 0f, float.PositiveInfinity);
    }

    public float CalculateAccuracy()
    {
        float num = Vector2.Distance(CriticalPosition.anchoredPosition, AttackBar.anchoredPosition);
        if (num <= 8f)
        {
            AttackBar.anchoredPosition = new Vector2(-465.7925f, AttackBar.anchoredPosition.y);
            return 150f;
        }
        if (num <= 10f)
        {
            return 120f;
        }
        if (num <= 20f)
        {
            return 110f;
        }
        return 100f;
    }

    private void UpdateBarPosition()
    {
        AttackBar.anchoredPosition = new Vector2(AttackBar.anchoredPosition.x - 468f * Time.deltaTime, AttackBar.anchoredPosition.y);
    }

    public void UpdateGraphics()
    {
        if (TargetPartyMember != null)
        {
            BarMemberIcon.sprite = TargetPartyMember.PartyMemberInBattle.PartyMemberBattleIcon;
            AttackBarBackground.color = TargetPartyMember.PartyMemberInBattle.PartyMemberColor;
        }
    }

    public float GetDistanceToCritical()
    {
        return Vector2.Distance(CriticalPosition.anchoredPosition, AttackBar.anchoredPosition);
    }

    public void SetNewBarPosition(float NewX)
    {
        MonoBehaviour.print("setup new bar positon");
        AttackBar.anchoredPosition = new Vector2(NewX, AttackBar.anchoredPosition.y);
        HasSetupPosition = true;
    }

    public void FadeAttackBar(float fadeSpeed = 1f, Color targetColor = default(Color))
    {
        StartCoroutine(FadeAttackBarTimed(fadeSpeed, targetColor));
    }

    private IEnumerator FadeAttackBarTimed(float fadeSpeed, Color targetColor = default(Color))
    {
        Color barcolor = AttackBar.GetComponent<RawImage>().color;
        while (barcolor != targetColor)
        {
            barcolor = Color.Lerp(barcolor, targetColor, fadeSpeed * Time.deltaTime);
            AttackBar.GetComponent<RawImage>().color = barcolor;
            if (Vector4.Distance(barcolor, targetColor) < 0.01f)
            {
                AttackBar.GetComponent<RawImage>().color = targetColor;
                break;
            }
            yield return null;
        }
    }
}
