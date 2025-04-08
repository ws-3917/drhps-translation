using UnityEngine;

public class TRIG_KITCHENTOGGLE : MonoBehaviour
{
    public LivingRoom_KitchenController Kitchen;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            Kitchen.KitchenVisible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((bool)other.GetComponent<PlayerManager>())
        {
            Kitchen.KitchenVisible = false;
        }
    }
}
