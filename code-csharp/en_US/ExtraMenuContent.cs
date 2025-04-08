using System;
using UnityEngine;

[Serializable]
public class ExtraMenuContent
{
    [SerializeField]
    public string ExtraName;

    [SerializeField]
    public ExtraMenuItem Item;

    [SerializeField]
    public NewMainMenu_ExtraSubMenu.ExtraMenuTypes ExtraType;
}
