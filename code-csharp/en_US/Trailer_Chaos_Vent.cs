using UnityEngine;

public class Trailer_Chaos_Vent : MonoBehaviour
{
    [SerializeField]
    private Vector3 TargetVentPos;

    [SerializeField]
    private bool IsVerticalVent;

    [SerializeField]
    private ParticleSystem smoke;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerManager.Instance.gameObject)
        {
            StartCoroutine(Trailer_Chaos_VentManager.instance.StartVent(base.transform.position + new Vector3(0f, 0.5f, 0f), TargetVentPos + base.transform.position + new Vector3(0f, 0.5f, 0f), IsVerticalVent, smoke));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(base.transform.position + TargetVentPos, 0.5f);
    }
}
