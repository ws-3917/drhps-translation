using System;
using UnityEngine;

public class BattleAttack_Siplett_DropletArc : MonoBehaviour
{
    [Header("-= Arc Utility =-")]
    public float JumpArcHeight = 3f;

    public float JumpDuration = 0.5f;

    public bool DisableRandomOffsetMarkiplier;

    [SerializeField]
    private SpriteRenderer BulletSprite;

    [SerializeField]
    private Sprite[] RandomSprites;

    private Vector2 lastPosition;

    [SerializeField]
    private Vector3 JumpStartPos;

    public Vector3 JumpEndPos;

    [SerializeField]
    private float RandomOffsetMarkiplier;

    [SerializeField]
    private AudioClip dropletsound;

    private float timeElapsed;

    private bool isMoving;

    private void Awake()
    {
        BulletSprite.sprite = RandomSprites[UnityEngine.Random.Range(0, RandomSprites.Length)];
        BulletSprite.color = new Color(1f, 1f, 1f, 0f);
        CutsceneUtils.FadeInSprite(BulletSprite, 2f);
    }

    private void Start()
    {
        JumpStartPos = base.transform.position;
        if (!DisableRandomOffsetMarkiplier)
        {
            JumpEndPos += (Vector3)UnityEngine.Random.insideUnitCircle * RandomOffsetMarkiplier;
        }
        lastPosition = base.transform.position;
        StartArcMovement();
    }

    private void Update()
    {
        ArcUpdate();
        SetRotationByVelocity();
    }

    private void SetRotationByVelocity()
    {
        Vector2 vector = base.transform.position;
        Vector2 vector2 = vector - lastPosition;
        if (vector2.sqrMagnitude > 0.001f)
        {
            float num = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
            base.transform.rotation = Quaternion.Euler(0f, 0f, num + 90f);
        }
        lastPosition = vector;
    }

    private void ArcUpdate()
    {
        if (isMoving)
        {
            timeElapsed += Time.deltaTime;
            float num = timeElapsed / JumpDuration;
            Vector2 vector = CalculateArcPosition(num);
            base.transform.position = vector;
            if (num >= 1f)
            {
                isMoving = false;
                timeElapsed = 0f;
                base.transform.position = JumpEndPos;
                UnityEngine.Object.Destroy(base.gameObject);
            }
        }
    }

    public void StartArcMovement()
    {
        base.transform.position = JumpStartPos;
        timeElapsed = 0f;
        isMoving = true;
    }

    private Vector2 CalculateArcPosition(float t)
    {
        Vector2 vector = Vector2.Lerp(JumpStartPos, JumpEndPos, t);
        float num = JumpArcHeight * Mathf.Sin(MathF.PI * t);
        return new Vector2(vector.x, vector.y + num);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Liquid")
        {
            BulletSprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            base.gameObject.GetComponentInChildren<ParticleSystem>().Stop();
            BattleSystem.PlayBattleSoundEffect(dropletsound, 0.25f);
            UnityEngine.Object.Destroy(base.gameObject, 0.25f);
        }
    }
}
