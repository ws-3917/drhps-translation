using UnityEngine;

public class SPR_Scrolling : MonoBehaviour
{
    [Header("-- Variables --")]
    [SerializeField]
    private Material TargetMaterial;

    [SerializeField]
    private Vector2 ScrollSpeed;

    private Vector2 scroll;

    private void Start()
    {
        TargetMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        scroll += ScrollSpeed * Time.deltaTime;
        TargetMaterial.mainTextureOffset = scroll;
    }
}
