using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_SectionManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] OptionTransforms;

    [SerializeField]
    private Vector3[] OriginalOptionTransform;

    [SerializeField]
    private RectTransform[] UnselectableTransforms;

    [SerializeField]
    private Vector3[] OriginalUnselectableTransform;

    [SerializeField]
    private TextMeshProUGUI ControlsText;

    [SerializeField]
    private string ThisControlText;

    [SerializeField]
    private float lerpSpeed = 5f;

    [Header("Spacing for if SelectionMode is 1")]
    [SerializeField]
    private float OptionSpacing = 700f;

    public int cursorPos;

    private int secondarycursorPos;

    [Space(10f)]
    [SerializeField]
    private int SelectionMode;

    public int SelectionState = 1;

    [SerializeField]
    private int SelectionActionIndex;

    [Space(10f)]
    [SerializeField]
    private string HoverAnimationStateName;

    [SerializeField]
    private string UnHoverAnimationStateName;

    [SerializeField]
    private string SelectAnimationStateName;

    [Space(10f)]
    [SerializeField]
    private AudioSource SectionSource;

    [SerializeField]
    private AudioClip SwitchSectionClip;

    [SerializeField]
    private AudioClip ConfirmSectionClip;

    [SerializeField]
    private AudioClip DenySectionClip;

    [SerializeField]
    private AudioClip ReturnSectionClip;

    [SerializeField]
    private AudioClip EnterSectionClip;

    [Space(10f)]
    [SerializeField]
    private MainMenu_SectionManager[] OtherSections;

    [SerializeField]
    private MainMenu_SectionManager PreviousSection;

    [SerializeField]
    private ChatboxSimpleText GonerText;

    [SerializeField]
    private CHATBOXTEXT ChatboxText;

    [SerializeField]
    private bool RanText;

    [Header("Selection Index 1, Scene Transition")]
    [SerializeField]
    private int[] TargetSceneName;

    [SerializeField]
    private TRIG_LEVELTRANSITION transition;

    [Space(10f)]
    [Header("Selection Mode 1 Specific")]
    [SerializeField]
    private RectTransform CreditOptionTransform;

    [SerializeField]
    private Vector3 OriginalCreditOptionTransform;

    [SerializeField]
    private MainMenu_SectionManager CreditSection;

    [Space(10f)]
    [Header("Selection Mode 2 Specific")]
    [SerializeField]
    private Hypothesis[] Hypotheticals;

    [SerializeField]
    private MainMenu_ContextMenu ContextMenu;

    private void Start()
    {
        for (int i = 0; i < OptionTransforms.Length; i++)
        {
            OriginalOptionTransform[i] = OptionTransforms[i].localPosition;
            OptionTransforms[i].localPosition = OriginalOptionTransform[i] - Vector3.up * 700f;
            if (SelectionActionIndex != 1 || !(Hypotheticals[i] != null))
            {
                continue;
            }
            Hypothesis hypothesis = Hypotheticals[i];
            GameObject gameObject = null;
            GameObject gameObject2 = null;
            GameObject gameObject3 = null;
            for (int j = 0; j < OptionTransforms[i].childCount; j++)
            {
                if (OptionTransforms[i].GetChild(j).name == "SectionIcon")
                {
                    gameObject = OptionTransforms[i].GetChild(j).gameObject;
                }
                if (OptionTransforms[i].GetChild(j).name == "SectionName")
                {
                    gameObject2 = OptionTransforms[i].GetChild(j).gameObject;
                }
                if (OptionTransforms[i].GetChild(j).name == "SectionDescription")
                {
                    gameObject3 = OptionTransforms[i].GetChild(j).gameObject;
                }
            }
            RawImage component = gameObject.GetComponent<RawImage>();
            TextMeshProUGUI component2 = gameObject2.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI component3 = gameObject3.GetComponent<TextMeshProUGUI>();
            component.texture = hypothesis.HypothesisMenuSprite.texture;
            component2.text = hypothesis.HypothesisName;
            component3.text = hypothesis.HypothesisDescription;
        }
        for (int k = 0; k < UnselectableTransforms.Length; k++)
        {
            OriginalUnselectableTransform[k] = UnselectableTransforms[k].localPosition;
            UnselectableTransforms[k].localPosition = OriginalUnselectableTransform[k] - Vector3.up * 700f;
        }
        if (SelectionMode == 1)
        {
            OriginalCreditOptionTransform = CreditOptionTransform.localPosition;
            CreditOptionTransform.localPosition = OriginalCreditOptionTransform - Vector3.up * 700f;
        }
    }

    private void Update()
    {
        if (SelectionState == 1)
        {
            if (ChatboxText != null && GonerText != null && !RanText)
            {
                RanText = true;
                GonerText.RunText(ChatboxText, 0);
                SectionSource.PlayOneShot(EnterSectionClip);
            }
            if (ThisControlText != "" && (bool)ControlsText && ControlsText.text != ThisControlText)
            {
                ControlsText.text = ThisControlText;
            }
            if (SelectionMode == 1)
            {
                ContextMenu.CloseContextMenu_PreventPreviousStateUpdate();
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Right))
            {
                if (secondarycursorPos == 0 && SelectionMode != 2)
                {
                    cursorPos++;
                    if (cursorPos >= OptionTransforms.Length)
                    {
                        cursorPos = 0;
                    }
                    SectionSource.PlayOneShot(SwitchSectionClip);
                }
            }
            else if (Input.GetKeyDown(PlayerInput.Instance.Key_Left) && secondarycursorPos == 0 && SelectionMode != 2)
            {
                cursorPos--;
                if (cursorPos < 0)
                {
                    cursorPos = OptionTransforms.Length - 1;
                }
                SectionSource.PlayOneShot(SwitchSectionClip);
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Down))
            {
                if (SelectionMode == 1)
                {
                    secondarycursorPos++;
                    if (secondarycursorPos >= 2)
                    {
                        secondarycursorPos = 0;
                    }
                    if (secondarycursorPos < 0)
                    {
                        secondarycursorPos = 1;
                    }
                    SectionSource.PlayOneShot(SwitchSectionClip);
                }
                if (SelectionMode == 2)
                {
                    cursorPos++;
                    if (cursorPos >= OptionTransforms.Length)
                    {
                        cursorPos = 0;
                    }
                    SectionSource.PlayOneShot(SwitchSectionClip);
                }
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Up))
            {
                if (SelectionMode == 1)
                {
                    secondarycursorPos--;
                    if (secondarycursorPos >= 2)
                    {
                        secondarycursorPos = 0;
                    }
                    if (secondarycursorPos < 0)
                    {
                        secondarycursorPos = 1;
                    }
                    SectionSource.PlayOneShot(SwitchSectionClip);
                }
                if (SelectionMode == 2)
                {
                    cursorPos--;
                    if (cursorPos < 0)
                    {
                        cursorPos = OptionTransforms.Length - 1;
                    }
                    SectionSource.PlayOneShot(SwitchSectionClip);
                }
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm))
            {
                if (SelectionMode != 2)
                {
                    SectionSource.PlayOneShot(ConfirmSectionClip);
                    SelectionState = 0;
                    CompareSelectionAction();
                }
                if (secondarycursorPos == 0)
                {
                    if (SelectionMode != 2 && (bool)OptionTransforms[cursorPos].GetComponent<Animator>())
                    {
                        OptionTransforms[cursorPos].GetComponent<Animator>().Play(SelectAnimationStateName);
                    }
                }
                else if ((bool)CreditOptionTransform.GetComponent<Animator>())
                {
                    CreditOptionTransform.GetComponent<Animator>().Play(SelectAnimationStateName);
                }
            }
            if (Input.GetKeyDown(PlayerInput.Instance.Key_Cancel) && SelectionState == 1)
            {
                ReturnToPrevious();
            }
            if (secondarycursorPos == 1 && cursorPos != 1)
            {
                cursorPos = 1;
            }
        }
        else
        {
            RanText = false;
        }
        LerpMenuOptions();
    }

    private void LerpMenuOptions()
    {
        if (SelectionState == 1)
        {
            if (SelectionMode == 0)
            {
                float num = 0.65f;
                float num2 = 1f;
                for (int i = 0; i < OptionTransforms.Length; i++)
                {
                    float x = (float)(i - cursorPos) * OptionSpacing;
                    Vector3 b = new Vector3(x, 0f, 0f);
                    Vector3 b2 = ((cursorPos == i) ? (Vector3.one * num2) : (Vector3.one * num));
                    OptionTransforms[i].localScale = Vector3.Lerp(OptionTransforms[i].localScale, b2, lerpSpeed * Time.deltaTime);
                    OptionTransforms[i].localPosition = Vector3.Lerp(OptionTransforms[i].localPosition, b, lerpSpeed * Time.deltaTime);
                    if (cursorPos == i)
                    {
                        OptionTransforms[i].SetAsLastSibling();
                        if ((bool)OptionTransforms[i].GetComponent<Animator>())
                        {
                            OptionTransforms[i].GetComponent<Animator>().Play(HoverAnimationStateName);
                        }
                    }
                    else
                    {
                        OptionTransforms[i].SetAsFirstSibling();
                        if ((bool)OptionTransforms[i].GetComponent<Animator>())
                        {
                            OptionTransforms[i].GetComponent<Animator>().Play(UnHoverAnimationStateName);
                        }
                    }
                }
            }
            if (SelectionMode == 1)
            {
                float num3 = 0.65f;
                float num4 = 0.7f;
                if (SelectionState == 1)
                {
                    for (int j = 0; j < OptionTransforms.Length; j++)
                    {
                        Vector3 one = Vector3.one;
                        _ = Vector3.one;
                        if (secondarycursorPos == 0)
                        {
                            one = ((cursorPos == j) ? (Vector3.one * num4) : (Vector3.one * num3));
                            _ = Vector3.one * num4;
                        }
                        else
                        {
                            one = Vector3.one * num3;
                            _ = Vector3.one * num4;
                        }
                        OptionTransforms[j].localScale = Vector3.Lerp(OptionTransforms[j].localScale, one, lerpSpeed * Time.deltaTime);
                        CreditOptionTransform.localScale = Vector3.Lerp(CreditOptionTransform.localScale, one, lerpSpeed * Time.deltaTime);
                        Vector3 b3 = OriginalOptionTransform[j];
                        Vector3 originalCreditOptionTransform = OriginalCreditOptionTransform;
                        if (cursorPos == j && secondarycursorPos == 0)
                        {
                            OptionTransforms[j].SetAsLastSibling();
                            if ((bool)OptionTransforms[j].GetComponent<Animator>())
                            {
                                OptionTransforms[j].GetComponent<Animator>().Play(HoverAnimationStateName);
                            }
                        }
                        else
                        {
                            if ((bool)OptionTransforms[j].GetComponent<Animator>())
                            {
                                OptionTransforms[j].GetComponent<Animator>().Play(UnHoverAnimationStateName);
                            }
                            OptionTransforms[j].SetAsFirstSibling();
                        }
                        if (secondarycursorPos == 0)
                        {
                            if ((bool)OptionTransforms[j].GetComponent<Animator>())
                            {
                                CreditOptionTransform.GetComponent<Animator>().Play(UnHoverAnimationStateName);
                            }
                        }
                        else if ((bool)OptionTransforms[j].GetComponent<Animator>())
                        {
                            CreditOptionTransform.GetComponent<Animator>().Play(HoverAnimationStateName);
                        }
                        OptionTransforms[j].localPosition = Vector3.Lerp(OptionTransforms[j].localPosition, b3, lerpSpeed * Time.deltaTime);
                        CreditOptionTransform.localPosition = Vector3.Lerp(CreditOptionTransform.localPosition, originalCreditOptionTransform, lerpSpeed * Time.deltaTime);
                    }
                }
            }
            if (SelectionMode == 2)
            {
                float num5 = 0.65f;
                float num6 = 1f;
                for (int k = 0; k < UnselectableTransforms.Length; k++)
                {
                    UnselectableTransforms[k].localPosition = Vector3.Lerp(UnselectableTransforms[k].localPosition, OriginalUnselectableTransform[k], lerpSpeed * Time.deltaTime);
                }
                for (int l = 0; l < OptionTransforms.Length; l++)
                {
                    float y = (float)(l - cursorPos) * OptionSpacing;
                    Vector3 b4 = new Vector3(-323.05f, y, 0f);
                    Vector3 b5 = ((cursorPos == l) ? (Vector3.one * num6) : (Vector3.one * num5));
                    OptionTransforms[l].localScale = Vector3.Lerp(OptionTransforms[l].localScale, b5, lerpSpeed * Time.deltaTime);
                    OptionTransforms[l].localPosition = Vector3.Lerp(OptionTransforms[l].localPosition, b4, lerpSpeed * Time.deltaTime);
                    if (cursorPos == l)
                    {
                        OptionTransforms[l].SetAsLastSibling();
                        if ((bool)OptionTransforms[l].GetComponent<Animator>())
                        {
                            OptionTransforms[l].GetComponent<Animator>().Play(HoverAnimationStateName);
                        }
                    }
                    else
                    {
                        OptionTransforms[l].SetAsFirstSibling();
                        if ((bool)OptionTransforms[l].GetComponent<Animator>())
                        {
                            OptionTransforms[l].GetComponent<Animator>().Play(UnHoverAnimationStateName);
                        }
                    }
                }
            }
        }
        if (SelectionState == 2)
        {
            for (int m = 0; m < OptionTransforms.Length; m++)
            {
                Vector3 b6 = OptionTransforms[m].localPosition - Vector3.up * 960f;
                OptionTransforms[m].localPosition = Vector3.Lerp(OptionTransforms[m].localPosition, b6, lerpSpeed * Time.deltaTime / 4f);
            }
            for (int n = 0; n < UnselectableTransforms.Length; n++)
            {
                Vector3 b7 = UnselectableTransforms[n].localPosition - Vector3.up * 960f;
                UnselectableTransforms[n].localPosition = Vector3.Lerp(UnselectableTransforms[n].localPosition, b7, lerpSpeed * Time.deltaTime);
            }
            if (SelectionMode == 1)
            {
                Vector3 b8 = CreditOptionTransform.localPosition - Vector3.up * 960f;
                CreditOptionTransform.localPosition = Vector3.Lerp(CreditOptionTransform.localPosition, b8, lerpSpeed * Time.deltaTime / 4f);
            }
        }
    }

    private void CompareSelectionAction()
    {
        switch (SelectionActionIndex)
        {
            case 0:
                if (secondarycursorPos == 0)
                {
                    StartCoroutine(DelayUntilTransition());
                }
                else
                {
                    StartCoroutine(ShowCredits());
                }
                break;
            case 1:
                SelectionState = 2;
                ContextMenu.OpenContextMenu(Hypotheticals[cursorPos], this);
                break;
        }
    }

    private void ReturnToPrevious()
    {
        if (PreviousSection != null)
        {
            SelectionState = 2;
            SectionSource.PlayOneShot(ReturnSectionClip);
            PreviousSection.SelectionState = 1;
        }
    }

    private IEnumerator DelayUntilTransition()
    {
        yield return new WaitForSeconds(1f);
        if (OtherSections[cursorPos] != null)
        {
            SelectionState = 2;
            OtherSections[cursorPos].SelectionState = 1;
            OtherSections[cursorPos].PreviousSection = this;
        }
        else
        {
            SelectionState = 1;
        }
    }

    private IEnumerator ShowCredits()
    {
        yield return new WaitForSeconds(1f);
        SelectionState = 2;
        CreditSection.SelectionState = 1;
        CreditSection.PreviousSection = this;
    }
}
