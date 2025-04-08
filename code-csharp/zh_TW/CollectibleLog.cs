using UnityEngine;

[CreateAssetMenu(fileName = "COLLECTIBLELOG", menuName = "Deltaswap/Collectible Log", order = 0)]
public class CollectibleLog : ScriptableObject
{
    public enum CollectibleLogType
    {
        文檔 = 0,
        視頻 = 1,
        音頻 = 2
    }

    public bool StartUnlocked;

    public string LogName = "PLACEHOLDER";

    public string LogPlayerPrefName = "log_";

    public CollectibleLogType LogType;

    public GameObject LogPrefab;

    public Sprite LogPreview;

    public Vector2 LogPreviewScale = Vector2.one * 200f;

    public AudioClip LogOpenSound;
}
