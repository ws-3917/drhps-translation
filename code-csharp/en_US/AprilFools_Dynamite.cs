using UnityEngine;

public class AprilFools_Dynamite : MonoBehaviour
{
    [SerializeField]
    private GameObject ExplosionPrefab;

    [SerializeField]
    private Animator DynamiteAnimator;

    [SerializeField]
    private float TimeTillBoom = 4f;

    private float elapsedtime;

    private void Awake()
    {
        TimeTillBoom = (TimeTillBoom += Random.Range(-0.2f, 0.2f));
    }

    private void Update()
    {
        elapsedtime += Time.deltaTime;
        DynamiteAnimator.speed = 4f * (elapsedtime / TimeTillBoom);
        if (elapsedtime > TimeTillBoom)
        {
            Object.Instantiate(ExplosionPrefab, base.transform.position, Quaternion.identity);
            Object.Destroy(base.gameObject);
        }
    }
}
