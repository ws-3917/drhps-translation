using UnityEngine;

[CreateAssetMenu(fileName = "REACTION", menuName = "Deltaswap/ChatboxReaction", order = 0)]
public class CHATBOXREACTION : ScriptableObject
{
    [Header("Each reaction is shown in sequential order, starting from the top.")]
    public REACTIONDATA[] Reaction;
}
