using System.Collections;
using UnityEngine;

public class AprilFools_Rocket : MonoBehaviour
{
    [SerializeField]
    private float RocketSpeed;

    [SerializeField]
    private GameObject ExplosionPrefab;

    [SerializeField]
    private bool IsHoming;

    [SerializeField]
    private float rotationSpeed = 6f;

    private void Start()
    {
        if (IsHoming)
        {
            StartCoroutine(DelayExplosion());
        }
    }

    private void Update()
    {
        base.transform.position += base.transform.forward * RocketSpeed * Time.deltaTime;
        if (IsHoming)
        {
            Vector3 target = AprilFools_Manager.instance.Player.transform.position - base.transform.position;
            float maxRadiansDelta = rotationSpeed * Time.deltaTime;
            Vector3 forward = Vector3.RotateTowards(base.transform.forward, target, maxRadiansDelta, 0f);
            base.transform.rotation = Quaternion.LookRotation(forward);
        }
    }

    private IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(1f);
        IsHoming = false;
        yield return new WaitForSeconds(5f);
        Object.Instantiate(ExplosionPrefab, base.transform.position, Quaternion.identity);
        Object.Destroy(base.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<AprilFools_Longshot>() && !other.GetComponent<AprilFools_Rocket>() && !other.GetComponent<AprilFools_Dynamite>())
        {
            Object.Instantiate(ExplosionPrefab, base.transform.position, Quaternion.identity);
            Object.Destroy(base.gameObject);
        }
    }
}
