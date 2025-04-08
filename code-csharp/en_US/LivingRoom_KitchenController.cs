using UnityEngine;

public class LivingRoom_KitchenController : MonoBehaviour
{
    public bool KitchenVisible;

    public Animator LevelBackgroundAnimator;

    [SerializeField]
    private Cutscene_SansBeforePapyrus sans;

    private void Start()
    {
        LevelBackgroundAnimator.Play("LivingRoom_KitchenShow", 0, 1f);
        Invoke("resetSpeed", 0.1f);
    }

    private void resetSpeed()
    {
        LevelBackgroundAnimator.speed = 1f;
    }

    private void Update()
    {
        LevelBackgroundAnimator.SetBool("KitchenVisible", KitchenVisible);
        sans.WalkToKitchen = KitchenVisible;
    }
}
