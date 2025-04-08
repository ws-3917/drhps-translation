using UnityEngine;

public class PushBlock_PreasurePlate : MonoBehaviour
{
    [SerializeReference]
    private SpriteRenderer Renderer;

    [SerializeReference]
    private Animator anim;

    [SerializeReference]
    private Sprite HeldDownSprite;

    [SerializeReference]
    private Sprite BlockCompleteSprite;

    public bool Complete;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PushBlock_Pushable>() && other.GetComponent<PushBlock_Pushable>().CanPressPreasurePlates)
        {
            other.GetComponent<PushBlock_Pushable>().CanPush = false;
            anim.enabled = false;
            Renderer.sprite = HeldDownSprite;
            other.GetComponent<SpriteRenderer>().sprite = BlockCompleteSprite;
            Complete = true;
        }
    }
}
