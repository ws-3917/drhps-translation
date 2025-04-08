using System;
using UnityEngine;

[Serializable]
public class CreditArea
{
    public string CreditNames;

    public string CreditCharacterAnimationName;

    public AudioClip CreditSound;

    public bool Title;

    [TextArea]
    public string CreditDescription;
}
