using UnityEngine;

public class INT_Generic : MonoBehaviour
{
    [Header("Generic component used for detecting player interactions.")]
    [Header("When interacted, Interacted will be the opposite of what it currently is")]
    [Header("-= Variables =-")]
    public bool CanUse = true;

    public bool Interacted;

    public bool SingleUse = true;

    public void Interact()
    {
        if (CanUse)
        {
            if (SingleUse)
            {
                Interacted = !Interacted;
                CanUse = false;
            }
            else
            {
                Interacted = !Interacted;
            }
        }
    }
}
