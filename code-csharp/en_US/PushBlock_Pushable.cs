using UnityEngine;

public class PushBlock_Pushable : MonoBehaviour
{
    public bool CanPressPreasurePlates;

    public bool CanPush = true;

    [SerializeField]
    private float MoveSpeed = 5f;

    [SerializeField]
    private PushBlock_CollisionPrediction Collision;

    [SerializeField]
    private AudioClip PushSound;

    [SerializeField]
    private BoxCollider2D collision;

    [SerializeField]
    private INT_Generic Interaction;

    private bool previousInteraction;

    private bool Moving;

    private Vector2 TargetPosition;

    private EOTDRouxlsRoom_Puzzle puzzle;

    private void Start()
    {
        puzzle = Object.FindFirstObjectByType<EOTDRouxlsRoom_Puzzle>();
    }

    private void Update()
    {
        Vector2 vector = new Vector2(PlayerManager.Instance.transform.position.x, PlayerManager.Instance.transform.position.y);
        Vector2 vector2 = new Vector2(base.transform.position.x, base.transform.position.y);
        Vector2 vector3 = vector - vector2;
        collision.offset = vector3.normalized / 4f;
        if (Interaction.Interacted != previousInteraction && CanPush)
        {
            if (puzzle != null && !puzzle.hasMovedPartyMembers)
            {
                puzzle.FUCKINGMOVE();
                puzzle = null;
            }
            previousInteraction = Interaction.Interacted;
            if (!Moving && !Collision.Colliding)
            {
                Moving = true;
                PlayerManager.Instance.PlayerAudioSource.PlayOneShot(PushSound);
                if (Mathf.Abs(vector3.x) > Mathf.Abs(vector3.y))
                {
                    if (vector3.x > 0f)
                    {
                        TargetPosition = vector2 + Vector2.left;
                    }
                    else
                    {
                        TargetPosition = vector2 + Vector2.right;
                    }
                }
                else if (vector3.y > 0f)
                {
                    TargetPosition = vector2 + Vector2.down;
                }
                else
                {
                    TargetPosition = vector2 + Vector2.up;
                }
                MonoBehaviour.print(TargetPosition);
            }
        }
        if (Moving)
        {
            if ((Vector2)base.transform.position != TargetPosition)
            {
                base.transform.position = Vector2.MoveTowards(base.transform.position, TargetPosition, MoveSpeed * Time.deltaTime);
            }
            else
            {
                Moving = false;
            }
        }
    }
}
