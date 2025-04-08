using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu_CreditSectionManager : MonoBehaviour
{
    [SerializeField]
    private MainMenu_SectionManager CreditSection;

    [SerializeField]
    private List<CreditArea> creditAreas = new List<CreditArea>();

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private TextMeshProUGUI NameText;

    [SerializeField]
    private TextMeshProUGUI DescriptionText;

    [SerializeField]
    private Animator CharacterIconAnimator;

    private int PreviousCursorPos = -1;

    private void Update()
    {
        if (PreviousCursorPos != CreditSection.cursorPos)
        {
            OnCursorPosChange();
            PreviousCursorPos = CreditSection.cursorPos;
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && source.clip != null && CreditSection.SelectionState == 1)
        {
            source.Play();
        }
    }

    private void OnCursorPosChange()
    {
        if (!creditAreas[CreditSection.cursorPos].Title)
        {
            NameText.text = creditAreas[CreditSection.cursorPos].CreditNames;
            DescriptionText.text = creditAreas[CreditSection.cursorPos].CreditDescription;
            CharacterIconAnimator.Play(creditAreas[CreditSection.cursorPos].CreditCharacterAnimationName);
            source.clip = creditAreas[CreditSection.cursorPos].CreditSound;
        }
        else
        {
            NameText.text = "";
            DescriptionText.text = "";
            CharacterIconAnimator.Play("CreditChar_Hidden");
            source.clip = null;
        }
    }
}
