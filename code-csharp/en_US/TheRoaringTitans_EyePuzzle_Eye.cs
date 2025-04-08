using UnityEngine;

public class TheRoaringTitans_EyePuzzle_Eye : MonoBehaviour
{
    public bool EyeOn;

    private INT_Generic Interact;

    [SerializeField]
    private TheRoaringTitans_EyePuzzle_Eye[] OherInteractedEyes;

    private bool PreviousEyeValue;

    private bool PreviousInteractBool;

    [SerializeField]
    private AudioClip ClickSound;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private SpriteRenderer Renderer;

    [SerializeField]
    private Sprite EyeSprite_On;

    [SerializeField]
    private Sprite EyeSprite_Off;

    private void Start()
    {
        Interact = base.gameObject.GetComponent<INT_Generic>();
        if (EyeOn)
        {
            Renderer.sprite = EyeSprite_On;
        }
        else
        {
            Renderer.sprite = EyeSprite_Off;
        }
    }

    private void LateUpdate()
    {
        if (EyeOn != PreviousEyeValue)
        {
            PreviousEyeValue = EyeOn;
            if (EyeOn)
            {
                Renderer.sprite = EyeSprite_On;
            }
            else
            {
                Renderer.sprite = EyeSprite_Off;
            }
        }
        if (Interact.Interacted != PreviousInteractBool)
        {
            source.PlayOneShot(ClickSound);
            PreviousInteractBool = Interact.Interacted;
            TheRoaringTitans_EyePuzzle_Eye[] oherInteractedEyes = OherInteractedEyes;
            foreach (TheRoaringTitans_EyePuzzle_Eye obj in oherInteractedEyes)
            {
                obj.EyeOn = !obj.EyeOn;
            }
        }
    }
}
