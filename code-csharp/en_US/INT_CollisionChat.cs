using UnityEngine;

public class INT_CollisionChat : MonoBehaviour
{
    public INT_Chat ChatToCollide;

    public bool HasRan;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !HasRan)
        {
            HasRan = true;
            ChatToCollide.RUN();
        }
    }
}
