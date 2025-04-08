using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CHOICE", menuName = "Deltaswap/ChatboxChoice", order = 1)]
public class CHATBOXCHOICE : ScriptableObject
{
    [Header("0 - ChoiceCount | Default selected (OOB values = No default selection, soul in center)")]
    public int DefaultSelectedChoice;

    [Space(5f)]
    [Header("Use < br> for new lines, slash N doesn't work for some reason")]
    public List<string> Choices = new List<string>();

    [Space(5f)]
    [Header("Player can press X to prematurely exit dialogue")]
    public bool CanBackOut;

    [Header("Only show and allow input from options at end of Text running")]
    public bool ShowOnTextScrollFinish;

    [Header("For customizing text size, font and color")]
    public CHATBOXCHAR ChoiceCharacterReference;

    public List<CHATBOXTEXT> ChoiceTextResults = new List<CHATBOXTEXT>();

    [Header("- Some Default Choice Position Values -")]
    [Header("2 choice positions\r\n\r\noption1 -253.4, -82.5\r\noption2 46.6, -82.5\r\n\r\n3 choice positions\r\n\r\noption1 -230, -82.5\r\noption2 -30, -82.5\r\noption3 170, -82.5\r\n\r\n4 choice positions\r\n\r\noption1 -400, -82.5\r\noption2 -150, -82.5\r\noption3 150, -82.5\r\noption4 400, -82.5")]
    public List<Vector2> ChoicePositions = new List<Vector2>
    {
        new Vector2(-253.4f, -82.5f),
        new Vector2(46.6f, -82.5f)
    };
}
