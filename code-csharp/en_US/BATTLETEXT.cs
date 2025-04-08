using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BATTLETEXT", menuName = "Deltaswap/Battle/BattleText", order = 0)]
public class BATTLETEXT : ScriptableObject
{
    public BattleText[] BattleTextboxes;
}
[Serializable]
public class BattleText
{
    public string[] Text;

    public CHATBOXACTION[] Action;

    public BattleText(string[] thistext, CHATBOXACTION[] thisaction)
    {
        Text = thistext;
        Action = thisaction;
    }
}
