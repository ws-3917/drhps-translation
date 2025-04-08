using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DarkworldMenu_BarPartyMember : MonoBehaviour
{
    public ActivePartyMember CurrentMember;

    [SerializeField]
    private Image Icon;

    [SerializeField]
    private Image Name;

    [SerializeField]
    private TextMeshProUGUI currentHealth;

    [SerializeField]
    private TextMeshProUGUI maxHealth;

    [SerializeField]
    private Image healthBar;

    public Image selectedIcon;

    public TextMeshProUGUI addhealthText;

    public Animator addhealthAnimator;

    public bool IsPlayers;

    public void SetupPartyMember(ActivePartyMember Member)
    {
        if (Member != null)
        {
            CurrentMember = Member;
            Icon.sprite = Member.PartyMemberDescription.PartyMemberBattleIcon;
            if (Member.PartyMemberDescription.PartyMemberBattleIcon != null)
            {
                Icon.rectTransform.sizeDelta = new Vector2(Member.PartyMemberDescription.PartyMemberBattleIcon.textureRect.width * 2f, Member.PartyMemberDescription.PartyMemberBattleIcon.textureRect.height * 2f);
            }
            Icon.rectTransform.anchoredPosition += CurrentMember.PartyMemberDescription.PartyMemberBattleIcon_CMenuOffset;
            Name.sprite = Member.PartyMemberDescription.PartyMemberBattle_NameIcon;
            if (Member.PartyMemberDescription.PartyMemberBattle_NameIcon != null)
            {
                Name.rectTransform.sizeDelta = new Vector2(Member.PartyMemberDescription.PartyMemberBattle_NameIcon.texture.width * 2, Member.PartyMemberDescription.PartyMemberBattle_NameIcon.texture.height * 2);
            }
            if (IsPlayers)
            {
                currentHealth.text = PlayerManager.Instance._PlayerHealth.ToString();
                maxHealth.text = PlayerManager.Instance._PlayerMaxHealth.ToString();
            }
            else
            {
                currentHealth.text = Member.CurrentHealth.ToString();
                maxHealth.text = Member.PartyMemberDescription.MaximumHealth.ToString();
            }
            healthBar.color = Member.PartyMemberDescription.PartyMemberColor;
            healthBar.fillAmount = (float)Member.CurrentHealth / (float)Member.PartyMemberDescription.MaximumHealth;
        }
        else
        {
            Debug.LogError("Party member null?");
        }
    }

    public void UpdateHealth(ActivePartyMember Member)
    {
        if (Member != null)
        {
            if (IsPlayers)
            {
                currentHealth.text = PlayerManager.Instance._PlayerHealth.ToString();
                maxHealth.text = PlayerManager.Instance._PlayerMaxHealth.ToString();
                Member.CurrentHealth = (int)PlayerManager.Instance._PlayerHealth;
            }
            else
            {
                currentHealth.text = Member.CurrentHealth.ToString();
                maxHealth.text = Member.PartyMemberDescription.MaximumHealth.ToString();
            }
            healthBar.fillAmount = (float)Member.CurrentHealth / (float)Member.PartyMemberDescription.MaximumHealth;
        }
        else
        {
            Debug.LogError("Party member null?");
        }
    }
}
