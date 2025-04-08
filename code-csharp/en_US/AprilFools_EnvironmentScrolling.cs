using UnityEngine;

public class AprilFools_EnvironmentScrolling : MonoBehaviour
{
    public float ScrolLSpeed = 48f;

    [SerializeField]
    private float WrapDistance = -819f;

    private void Update()
    {
        base.transform.position += Vector3.back * ScrolLSpeed * Time.deltaTime;
        if (base.transform.position.z < WrapDistance)
        {
            base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f - base.transform.position.z);
        }
    }
}
