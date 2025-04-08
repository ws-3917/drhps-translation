using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "TEXT", menuName = "Deltaswap/ChatboxText", order = 1)]
public class CHATBOXTEXT : ScriptableObject
{
    [Header("# = 0.5 second pause\n@ = 0.125 second pause\n; = new line in dialogue\n~ = new line in dialogue but bulletpoint is one below than normal\n£/* = Forcefully end chat, useful for when rich text is used")]
    [Space(5f)]
    [Header("WARNING FOR RICH TEXT! Only use ONE per dialogue, and try avoid pausing characters.")]
    [Space(5f)]
    [Header("Battle Specific - use Empty text to continue dialogue but show no bubble.")]
    [Space(5f)]
    public Textbox[] Textboxes;
}
