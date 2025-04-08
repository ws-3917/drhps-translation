using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "extraitem", menuName = "Deltaswap/ExtraItem", order = 0)]
public class ExtraMenuItem : ScriptableObject
{
    public string ItemTitle;

    public AudioClip DeveloperCommentary;

    [TextArea(10, 10)]
    public string ItemDescription;

    public GameObject Prefab;
}
