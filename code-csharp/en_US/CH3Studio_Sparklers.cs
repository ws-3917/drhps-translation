using UnityEngine;

public class CH3Studio_Sparklers : MonoBehaviour
{
    [SerializeField]
    private Animator[] Sparklers;

    private void Start()
    {
        Animator[] sparklers = Sparklers;
        foreach (Animator obj in sparklers)
        {
            obj.Play("CH3Studio_Sparkle", -1, Random.Range(0, 1));
            obj.speed = Random.Range(0.45f, 0.55f);
        }
    }
}
