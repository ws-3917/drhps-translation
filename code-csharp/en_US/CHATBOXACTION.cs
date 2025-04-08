using UnityEngine;

[CreateAssetMenu(fileName = "ACTION", menuName = "Deltaswap/ChatboxAction", order = 0)]
public class CHATBOXACTION : ScriptableObject
{
    [Header("Play a sound during this action, add multiple to the list for random")]
    public bool PlaySound;

    public AudioClip[] PossibleSounds;

    [Space(10f)]
    [Header("Run a function in a certain component.")]
    public bool RunComponentFunction;

    [Header("WARNING! Will fail if two objects have the same name")]
    public string TargetComponentGameObjectName;

    public string TargetComponentClassname;

    public string FunctionName;

    [Space(5f)]
    public CHATBOXACTION_SubAction[] SubActions;

    [Space(5f)]
    [Header("Runs everything this action will do when the players ends the chat that this action is on")]
    public bool RunActionOnChatEnd;
}
