using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleActItemSelectable : MonoBehaviour
{
    [Header("-- References --")]
    public TextMeshProUGUI Selectable_NameText;

    public Transform Selectable_InfoHolder;

    public GameObject CharacterIconPrefab;

    public Color UnselectedColor = Color.white;

    public void AddCharacterIcon(PartyMember targetPartyMember)
    {
        if (CharacterIconPrefab == null)
        {
            Debug.LogWarning("prefab For CharacterIcon is Null");
            return;
        }
        GameObject obj = Object.Instantiate(CharacterIconPrefab, Selectable_InfoHolder);
        obj.transform.SetAsFirstSibling();
        obj.GetComponent<Image>().sprite = targetPartyMember.PartyMemberBattleIcon;
        obj.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(targetPartyMember.PartyMemberBattleIcon.textureRect.width * 2f, targetPartyMember.PartyMemberBattleIcon.textureRect.height * 2f);
        BattlePartyMember partyMember = BattleSystem.GetPartyMember(targetPartyMember);
        if (partyMember != null && partyMember.PartyMember_Health <= 0f)
        {
            UnselectedColor = Color.gray;
            Selectable_NameText.color = Color.gray;
        }
    }

    public void SetSelectableName(string name)
    {
        Selectable_NameText.text = name;
    }
}
