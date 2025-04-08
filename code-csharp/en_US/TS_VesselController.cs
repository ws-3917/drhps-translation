using System.Collections.Generic;
using UnityEngine;

public class TS_VesselController : MonoBehaviour
{
    [Header("-- Vessel Randomizer --")]
    [SerializeField]
    private Transform vesselTransform;

    public bool CopyAnimationsEnabled = true;

    public bool CopyTransformsEnabled = true;

    [Space(5f)]
    [SerializeField]
    private SpriteRenderer HeadRenderer;

    [SerializeField]
    private int headIndex;

    [SerializeField]
    private SpriteRenderer BodyRenderer;

    [SerializeField]
    private int bodyIndex;

    [SerializeField]
    private SpriteRenderer LegsRenderer;

    [SerializeField]
    private int legsIndex;

    [Space(5f)]
    [SerializeField]
    private Transform HeadMask;

    [SerializeField]
    private Transform BodyMask;

    [SerializeField]
    private Transform LegsMask;

    [Space(5f)]
    [SerializeField]
    private List<TSVesselHead> HeadVarients = new List<TSVesselHead>();

    [SerializeField]
    private List<TSVesselBody> BodyVarients = new List<TSVesselBody>();

    [SerializeField]
    private List<TSVesselLegs> LegVarients = new List<TSVesselLegs>();

    [Space(5f)]
    [SerializeField]
    private SpriteRenderer krisAnimator;

    private void Start()
    {
        headIndex = Random.Range(0, HeadVarients.Count);
        bodyIndex = Random.Range(0, BodyVarients.Count);
        legsIndex = Random.Range(0, LegVarients.Count);
        krisAnimator = PlayerManager.Instance.PlayerSpriteRenderer;
        PlayerManager.Instance.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    private void LateUpdate()
    {
        if (CopyAnimationsEnabled)
        {
            CopyKrisAnimations();
        }
        if (CopyTransformsEnabled)
        {
            vesselTransform.transform.position = PlayerManager.Instance.transform.position + new Vector3(0f, 0f, 0f);
        }
    }

    private void CopyKrisAnimations()
    {
        if (krisAnimator.sprite.name == "kris_idle_down")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Idle_Down, BodyVarients[bodyIndex].BodySprite_Idle_Down, LegVarients[legsIndex].LegSprite_Idle_Down, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true);
        }
        if (krisAnimator.sprite.name == "kris_idle_right")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Idle_Right, BodyVarients[bodyIndex].BodySprite_Idle_Right, LegVarients[legsIndex].LegSprite_Idle_Right);
        }
        if (krisAnimator.sprite.name == "kris_idle_up")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Idle_Up, BodyVarients[bodyIndex].BodySprite_Idle_Up, LegVarients[legsIndex].LegSprite_Idle_Up, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true, isUpwards: true);
        }
        if (krisAnimator.sprite.name == "kris_idle_left")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Idle_Right, BodyVarients[bodyIndex].BodySprite_Idle_Right, LegVarients[legsIndex].LegSprite_Idle_Right, headFlipped: true, bodyFlipped: true, legsFlipped: true);
        }
        if (krisAnimator.sprite.name == "kris_walk1_down")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Down, BodyVarients[bodyIndex].BodySprite_Walk_Down, LegVarients[legsIndex].LegSprite_Walk_Down, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true);
            if (LegVarients[legsIndex].isFlippedVarient)
            {
                SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Down, BodyVarients[bodyIndex].BodySprite_Walk2_Down, LegVarients[legsIndex].LegSprite_Walk_Down, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true);
            }
        }
        if (krisAnimator.sprite.name == "kris_walk1_right")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Right, BodyVarients[bodyIndex].BodySprite_Walk_Right, LegVarients[legsIndex].LegSprite_Walk_Right);
        }
        if (krisAnimator.sprite.name == "kris_walk1_up")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Up, BodyVarients[bodyIndex].BodySprite_Walk_Up, LegVarients[legsIndex].LegSprite_Walk_Up, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true, isUpwards: true);
            if (LegVarients[legsIndex].isFlippedVarient)
            {
                SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Up, BodyVarients[bodyIndex].BodySprite_Walk2_Up, LegVarients[legsIndex].LegSprite_Walk_Up, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true, isUpwards: true);
            }
        }
        if (krisAnimator.sprite.name == "kris_walk1_left")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Right, BodyVarients[bodyIndex].BodySprite_Walk_Right, LegVarients[legsIndex].LegSprite_Walk_Right, headFlipped: true, bodyFlipped: true, legsFlipped: true);
        }
        if (krisAnimator.sprite.name == "kris_walk2_down")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Down, BodyVarients[bodyIndex].BodySprite_Walk2_Down, LegVarients[legsIndex].LegSprite_Walk2_Down, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true);
            if (LegVarients[legsIndex].isFlippedVarient)
            {
                SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Down, BodyVarients[bodyIndex].BodySprite_Walk_Down, LegVarients[legsIndex].LegSprite_Walk2_Down, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true);
            }
        }
        if (krisAnimator.sprite.name == "kris_walk2_right")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Right, BodyVarients[bodyIndex].BodySprite_Walk2_Right, LegVarients[legsIndex].LegSprite_Walk_Right);
        }
        if (krisAnimator.sprite.name == "kris_walk2_up")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Up, BodyVarients[bodyIndex].BodySprite_Walk2_Up, LegVarients[legsIndex].LegSprite_Walk2_Up, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true, isUpwards: true);
            if (LegVarients[legsIndex].isFlippedVarient)
            {
                SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Up, BodyVarients[bodyIndex].BodySprite_Walk_Up, LegVarients[legsIndex].LegSprite_Walk2_Up, headFlipped: false, bodyFlipped: false, legsFlipped: false, ValidLegFlipping: true, isUpwards: true);
            }
        }
        if (krisAnimator.sprite.name == "kris_walk2_left")
        {
            SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Walk_Right, BodyVarients[bodyIndex].BodySprite_Walk2_Right, LegVarients[legsIndex].LegSprite_Walk_Right, headFlipped: true, bodyFlipped: true, legsFlipped: true);
        }
    }

    public void SetVesselPartAnimations(Sprite headSprite, Sprite bodySprite, Sprite legSprite, bool headFlipped = false, bool bodyFlipped = false, bool legsFlipped = false, bool ValidLegFlipping = false, bool isUpwards = false)
    {
        HeadRenderer.sprite = headSprite;
        BodyRenderer.sprite = bodySprite;
        LegsRenderer.sprite = legSprite;
        HeadRenderer.flipX = headFlipped;
        BodyRenderer.flipX = bodyFlipped;
        if (ValidLegFlipping && LegVarients[legsIndex].isFlippedVarient)
        {
            LegsRenderer.flipX = !legsFlipped;
            LegsMask.transform.localScale = new Vector3((!LegsRenderer.flipX) ? 1 : (-1), 1f, 1f);
            if (isUpwards)
            {
                LegsRenderer.transform.localPosition = new Vector2(-0.05f, -0.9f);
            }
            else
            {
                LegsRenderer.transform.localPosition = new Vector2(0.05f, -0.9f);
            }
        }
        else
        {
            LegsRenderer.flipX = legsFlipped;
            LegsMask.transform.localScale = new Vector3((!LegsRenderer.flipX) ? 1 : (-1), 1f, 1f);
            LegsRenderer.transform.localPosition = new Vector2(0f, -0.9f);
        }
        if (HeadRenderer.flipX)
        {
            HeadMask.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            HeadMask.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (BodyRenderer.flipX)
        {
            BodyMask.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            BodyMask.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void SetVesselToHug()
    {
        SetVesselPartAnimations(HeadVarients[headIndex].HeadSprite_Hug, BodyVarients[bodyIndex].BodySprite_Hug, LegVarients[legsIndex].LegSprite_Idle_Right, headFlipped: true, bodyFlipped: true, legsFlipped: true);
    }
}
