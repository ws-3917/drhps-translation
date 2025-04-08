using UnityEngine;

public class SPR3D_Scrolling : MonoBehaviour
{
    private Renderer rend;

    private Vector2 savedOffset;

    public float scrollSpeed;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float x = Mathf.Repeat(Time.time * scrollSpeed, 1f);
        Vector2 value = new Vector2(x, 0f);
        rend.sharedMaterial.SetTextureOffset("_MainTex", value);
    }
}
