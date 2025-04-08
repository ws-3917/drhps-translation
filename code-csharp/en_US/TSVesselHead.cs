using System;
using UnityEngine;

[Serializable]
public class TSVesselHead
{
    [Header("Idle Sprites")]
    public Sprite HeadSprite_Idle_Down;

    public Sprite HeadSprite_Idle_Right;

    public Sprite HeadSprite_Idle_Up;

    [Header("Walk Sprites")]
    public Sprite HeadSprite_Walk_Down;

    public Sprite HeadSprite_Walk_Right;

    public Sprite HeadSprite_Walk_Up;

    [Space(5f)]
    public Sprite HeadSprite_Hug;
}
