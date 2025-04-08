using UnityEngine;

public class PushBlock_CollisionPrediction : MonoBehaviour
{
    public bool Colliding;

    public bool printresult;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Colliding && other.tag != "Player" && other.tag != "PuzzlePushBlock")
        {
            Colliding = true;
            if (printresult)
            {
                MonoBehaviour.print(other.name);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!Colliding && other.tag != "Player" && other.tag != "PuzzlePushBlock")
        {
            Colliding = true;
        }
        if (printresult)
        {
            MonoBehaviour.print(other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Colliding && other.tag != "Player" && other.tag != "PuzzlePushBlock")
        {
            Colliding = false;
        }
        if (printresult)
        {
            MonoBehaviour.print(other.name);
        }
    }

    private void Update()
    {
        Vector2 vector = new Vector2(PlayerManager.Instance.transform.position.x, PlayerManager.Instance.transform.position.y);
        Vector2 vector2 = new Vector2(base.transform.parent.position.x, base.transform.parent.position.y);
        Vector2 vector3 = vector2 - vector;
        if (Mathf.Abs(vector3.x) > Mathf.Abs(vector3.y))
        {
            base.transform.position = vector2 + new Vector2(Mathf.Sign(vector3.x), 0f);
        }
        else
        {
            base.transform.position = vector2 + new Vector2(0f, Mathf.Sign(vector3.y));
        }
    }
}
