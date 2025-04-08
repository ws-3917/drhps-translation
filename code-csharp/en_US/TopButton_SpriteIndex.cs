using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TopButton_SpriteIndex
{
    public string Name;

    [Space(5f)]
    public Sprite SectionTextSprite;

    public Sprite Unhovered;

    public Sprite Hovered;

    public Sprite Selected;

    [Space(5f)]
    public RawImage ButtonRenderer;

    public DarkworldMenu.DarkworldMenuStates TargetState;
}
