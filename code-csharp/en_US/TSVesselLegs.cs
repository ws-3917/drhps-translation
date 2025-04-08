using System;
using UnityEngine;

[Serializable]
public class TSVesselLegs
{
    public bool isFlippedVarient;

    [Header("Idle Sprites")]
    public Sprite LegSprite_Idle_Down;

    public Sprite LegSprite_Idle_Right;

    public Sprite LegSprite_Idle_Up;

    [Header("Walk Sprites")]
    public Sprite LegSprite_Walk_Down;

    public Sprite LegSprite_Walk_Right;

    public Sprite LegSprite_Walk_Up;

    [Header("Walk2 Sprites")]
    public Sprite LegSprite_Walk2_Down;

    public Sprite LegSprite_Walk2_Up;
}
