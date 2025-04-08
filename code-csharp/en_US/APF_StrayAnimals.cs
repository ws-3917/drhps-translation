using System;
using System.Collections.Generic;
using UnityEngine;

public class APF_StrayAnimals : MonoBehaviour
{
    public List<Transform> sprites;

    public float speed = 2f;

    public float width = 3f;

    public float height = 2f;

    public float followDelay = 0.2f;

    private float timeOffset;

    private List<float> spriteOffsets;

    private void Start()
    {
        timeOffset = UnityEngine.Random.Range(0f, MathF.PI * 2f);
        spriteOffsets = new List<float>();
        for (int i = 0; i < sprites.Count; i++)
        {
            spriteOffsets.Add((float)i * followDelay);
        }
    }

    private void Update()
    {
        MoveSprites();
    }

    private void MoveSprites()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            if (sprites[i] != null)
            {
                float f = Time.time * speed + timeOffset - spriteOffsets[i];
                float x = Mathf.Cos(f) * width;
                float y = Mathf.Sin(f) * height;
                Vector3 vector = new Vector3(x, y, sprites[i].position.z);
                float num = (0f - Mathf.Sin(f)) * width;
                Mathf.Cos(f);
                _ = height;
                sprites[i].position = base.transform.position + vector;
                bool flipX = num < 0f;
                sprites[i].GetComponentInChildren<SpriteRenderer>().flipX = flipX;
            }
        }
    }
}
