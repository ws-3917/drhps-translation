using UnityEngine;

public class SPR_YSorting : MonoBehaviour
{
    public SpriteRenderer SPR;

    public string AbovePlayer = "High";

    public string BelowPlayer = "Default";

    [Space(10f)]
    [Header("Sort by Y position (basically its like negative y pos * 6 rounded)")]
    public bool AutomaticRealtimeSorting;

    public int YOffset;

    private void Start()
    {
    }

    private void Update()
    {
        if (!AutomaticRealtimeSorting)
        {
            if (PlayerManager.Instance.transform.position.y > base.transform.position.y)
            {
                SPR.sortingLayerName = AbovePlayer;
            }
            else
            {
                SPR.sortingLayerName = BelowPlayer;
            }
        }
        else
        {
            SPR.sortingOrder = Mathf.RoundToInt((0f - SPR.transform.position.y) * 6f) + YOffset;
        }
    }
}
