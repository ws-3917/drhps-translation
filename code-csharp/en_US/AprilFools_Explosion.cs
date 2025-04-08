using UnityEngine;

public class AprilFools_Explosion : MonoBehaviour
{
    [SerializeField]
    private Animator ExplosionAnim;

    [SerializeField]
    private AudioSource ExplosionSound;

    [SerializeField]
    private float ExplosionRadius = 8f;

    private void Start()
    {
        ExplosionAnim.Play("spr_badexplosion_explode");
        ExplosionSound.Play();
        AprilFools_PlayerController player = AprilFools_Manager.instance.Player;
        if (Vector3.Distance(player.transform.position, base.transform.position) <= ExplosionRadius)
        {
            player.DamagePlayer(Random.Range(8, 16));
        }
        Object.Destroy(base.gameObject, 0.55f);
    }
}
