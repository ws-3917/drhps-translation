using System;
using UnityEngine;

[Serializable]
public class TSVesselBody
{
    [Header("Idle Sprites")]
    public Sprite BodySprite_Idle_Down;

    public Sprite BodySprite_Idle_Right;

    public Sprite BodySprite_Idle_Up;

    [Header("Walk Sprites")]
    public Sprite BodySprite_Walk_Down;

    public Sprite BodySprite_Walk_Right;

    public Sprite BodySprite_Walk_Up;

    [Header("Walk2 Sprites")]
    public Sprite BodySprite_Walk2_Down;

    public Sprite BodySprite_Walk2_Right;

    public Sprite BodySprite_Walk2_Up;

    [Space(5f)]
    public Sprite BodySprite_Hug;
}
