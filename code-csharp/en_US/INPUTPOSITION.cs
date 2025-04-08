using System;
using UnityEngine;

[Serializable]
public class INPUTPOSITION
{
    public int[] SceneNameIndex;

    public Vector2[] StartPosition;

    public Vector2[] StartDirection;

    public Vector3[] StartCameraPosition;

    public INPUTPOSITION(int[] thisScene, int[] thisIndex, Vector2[] thisPos, Vector2[] thisDirection, Vector3[] thisCamPos)
    {
        SceneNameIndex = thisScene;
        StartPosition = thisPos;
        StartDirection = thisDirection;
        StartCameraPosition = thisCamPos;
    }
}
