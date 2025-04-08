using System;
using UnityEngine;

[Serializable]
public class RoomStartPosition
{
    [Header("If the previous scene index doesnt match any Spawn positions, will go to default")]
    public Vector2 DefaultStartPosition;

    public Vector2 DefaultKrisRotation;

    public Vector2 DefaultCameraStartPosition;

    public PreviousStartPosition[] SpawnPositions;
}
