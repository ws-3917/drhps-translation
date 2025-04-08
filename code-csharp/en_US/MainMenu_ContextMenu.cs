using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu_ContextMenu : MonoBehaviour
{
    [SerializeField]
    private bool MenuOpen;

    [SerializeField]
    private bool AllowInput;

    [SerializeField]
    private bool WaitingUntilZUnpressed;

    [SerializeField]
    private Animator ContextMenuAnimator;

    [SerializeField]
    private MainMenu_SectionManager PreviousSection;

    [SerializeField]
    private TRIG_LEVELTRANSITION LevelTransition;

    [SerializeField]
    private TextMeshProUGUI ContextText;

    [SerializeField]
    private TextMeshProUGUI ContextTitleText;

    [SerializeField]
    private TextMeshProUGUI ControlsText;

    [SerializeField]
    private List<GameObject> ContextGoals = new List<GameObject>();

    [SerializeField]
    private GameObject InputControlText;

    [SerializeField]
    private Image ContextIcon;

    [SerializeField]
    private AudioSource ContextSource;

    [SerializeField]
    private AudioClip Sound_Confirm;

    [SerializeField]
    private AudioClip Sound_Return;

    [SerializeField]
    private ChatboxSimpleText GonerText;

    [SerializeField]
    private CHATBOXTEXT ChatboxText;

    [SerializeField]
    private bool RanText;

    private Hypothesis CurrentHypothesis;

    public void OpenContextMenu(Hypothesis Hypothesis, MainMenu_SectionManager thisSection)
    {
        if (Hypothesis != null && thisSection != null)
        {
            CurrentHypothesis = Hypothesis;
            PreviousSection = thisSection;
            MenuOpen = true;
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Return))
            {
                WaitingUntilZUnpressed = true;
            }
            else
            {
                StartCoroutine(OpenMenuInputDebounce());
            }
            if (ChatboxText != null && GonerText != null && !RanText)
            {
                RanText = true;
                GonerText.RunText(ChatboxText, 0);
            }
            InputControlText.SetActive(value: true);
            ContextText.text = CurrentHypothesis.HypothesisDescription;
            ContextTitleText.text = CurrentHypothesis.HypothesisName;
            ContextIcon.sprite = CurrentHypothesis.HypothesisMenuSprite;
            SetupCurrentHypothesisGoals();
            ControlsText.text = "";
            ContextMenuAnimator.Play("ContextMenu_FadeIn");
        }
        else
        {
            Debug.LogWarning("Hypothesis or thisSection is null?");
            Debug.LogWarning(Hypothesis);
            Debug.LogWarning(thisSection);
            MenuOpen = false;
        }
    }

    public void CloseContextMenu()
    {
        MenuOpen = false;
        RanText = false;
        ContextMenuAnimator.Play("ContextMenu_FadeOut");
        PreviousSection.SelectionState = 1;
    }

    public void CloseContextMenu_PreventPreviousStateUpdate()
    {
        MenuOpen = false;
        RanText = false;
        ContextMenuAnimator.Play("ContextMenu_Hidden");
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.RightShift)) && MenuOpen && AllowInput)
        {
            StartCoroutine(CloseContextDebounce());
            ContextSource.PlayOneShot(Sound_Return);
        }
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return)) && MenuOpen && AllowInput && !WaitingUntilZUnpressed)
        {
            AllowInput = false;
            InputControlText.SetActive(value: false);
            LevelTransition.LevelToGo = CurrentHypothesis.HypothesisStartingScene;
            LevelTransition.BeginTransition(2f);
            MusicManager.StopSong(Fade: true, 1f);
            ContextSource.PlayOneShot(Sound_Confirm);
        }
        if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Return))
        {
            WaitingUntilZUnpressed = false;
        }
        if (Input.GetKeyDown(KeyCode.Delete) && MenuOpen && AllowInput)
        {
            PlayerPrefs.SetInt("HypothesisGoal_" + CurrentHypothesis.HypothesisGoals[1].GoalPlayprefName, 0);
        }
    }

    private IEnumerator CloseContextDebounce()
    {
        yield return new WaitForSeconds(0.05f);
        CloseContextMenu();
    }

    private IEnumerator OpenMenuInputDebounce()
    {
        yield return new WaitForSeconds(0.05f);
        AllowInput = true;
    }

    private void SetupCurrentHypothesisGoals()
    {
        for (int i = 0; i < ContextGoals.Count; i++)
        {
            if (i < CurrentHypothesis.HypothesisGoals.Length)
            {
                EditGoalValues(i, CurrentHypothesis.HypothesisGoals[i]);
            }
            else
            {
                ClearGoalValue(i);
            }
        }
    }

    private void EditGoalValues(int index, HypothesisGoal Goal)
    {
        GameObject gameObject = ContextGoals[index];
        GameObject gameObject2 = null;
        GameObject gameObject3 = null;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "GoalHint")
            {
                gameObject2 = gameObject.transform.GetChild(i).gameObject;
            }
            if (gameObject.transform.GetChild(i).name == "GoalProgress")
            {
                gameObject3 = gameObject.transform.GetChild(i).gameObject;
            }
        }
        TextMeshProUGUI component = gameObject2.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI component2 = gameObject3.GetComponent<TextMeshProUGUI>();
        component.text = Goal.GoalHint;
        int @int = PlayerPrefs.GetInt("HypothesisGoal_" + Goal.GoalPlayprefName, 0);
        if (@int >= Goal.RequiredTasks)
        {
            component.color = Color.green;
            component2.color = Color.green;
        }
        else
        {
            component.color = Color.white;
            component2.color = Color.white;
        }
        if (Goal.RequiredTasks > 1)
        {
            component2.text = @int + "/" + Goal.RequiredTasks;
        }
        else if (@int == 0)
        {
            component2.text = "( )";
        }
        else
        {
            component2.text = "(X)";
        }
    }

    private void ClearGoalValue(int index)
    {
        GameObject gameObject = ContextGoals[index];
        GameObject gameObject2 = null;
        GameObject gameObject3 = null;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).name == "GoalHint")
            {
                gameObject2 = gameObject.transform.GetChild(i).gameObject;
            }
            if (gameObject.transform.GetChild(i).name == "GoalProgress")
            {
                gameObject3 = gameObject.transform.GetChild(i).gameObject;
            }
        }
        TextMeshProUGUI component = gameObject2.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI component2 = gameObject3.GetComponent<TextMeshProUGUI>();
        component.text = "";
        component2.text = "";
    }
}
