using System;

[Serializable]
public class Textbox
{
    public CHATBOXCHAR[] Character;

    public CHATBOXCHOICE[] Choice;

    public string[] Text;

    public CHATBOXACTION[] Action;

    public CHATBOXREACTION[] Reaction;

    public CHATBOXSUBACTION[] SubActions;

    public Textbox(CHATBOXCHAR[] thischar, CHATBOXCHOICE[] thischoice, string[] thistext, CHATBOXACTION[] thisaction, CHATBOXREACTION[] thisreaction, CHATBOXSUBACTION[] thissubaction)
    {
        Character = thischar;
        Choice = thischoice;
        Text = thistext;
        Action = thisaction;
        Reaction = thisreaction;
        SubActions = thissubaction;
    }
}
