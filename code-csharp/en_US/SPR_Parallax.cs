using UnityEngine;

public class SPR_Parallax : MonoBehaviour
{
    private float length;

    private float height;

    private Vector2 startpos;

    private GameObject Camera;

    [Header("0 - 1 | Higher the number the more distance from camera")]
    public float HorizontalParallaxAmount;

    public float VerticalParallaxAmount;

    public bool backgroundLoop;

    private void Start()
    {
        Camera = CameraManager.instance.gameObject;
        startpos = base.transform.position;
        if ((bool)GetComponent<SpriteRenderer>())
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
            height = GetComponent<SpriteRenderer>().bounds.size.y;
        }
        else
        {
            length = 1f;
            height = 1f;
            Debug.LogWarning("Unable to find SpriteRenderer in SPR_Parallax object, defaulting to length/height = 1");
        }
    }

    private void LateUpdate()
    {
        float num = Camera.transform.position.x * (1f - HorizontalParallaxAmount);
        float num2 = Camera.transform.position.x * HorizontalParallaxAmount;
        float num3 = Camera.transform.position.y * (1f - VerticalParallaxAmount);
        float num4 = Camera.transform.position.y * VerticalParallaxAmount;
        base.transform.position = new Vector3(startpos.x + num2, startpos.y + num4, base.transform.position.z);
        if (backgroundLoop)
        {
            if (num > startpos.x + length)
            {
                startpos.x += length;
            }
            else if (num < startpos.x - length)
            {
                startpos.x -= length;
            }
            if (num3 > startpos.y + height)
            {
                startpos.y += height;
            }
            else if (num3 < startpos.y - height)
            {
                startpos.y -= height;
            }
        }
    }
}
