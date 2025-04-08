using UnityEngine;

public class AprilFools_DOOMSprite : MonoBehaviour
{
    public int angle;

    public float angleF;

    public SpriteRenderer sprite;

    public Sprite[] sprites = new Sprite[16];

    private void Update()
    {
        angleF = Mathf.Atan2(Camera.main.transform.position.z - base.transform.position.z, Camera.main.transform.position.x - base.transform.position.x) * 57.29578f;
        if (angleF < 0f)
        {
            angleF += 360f;
        }
        angleF += base.transform.eulerAngles.y;
        angle = Mathf.RoundToInt(angleF / 22.5f);
        while (angle < 0 || angle >= 16)
        {
            angle += (int)(-16f * Mathf.Sign(angle));
        }
        sprite.sprite = sprites[angle];
    }
}
