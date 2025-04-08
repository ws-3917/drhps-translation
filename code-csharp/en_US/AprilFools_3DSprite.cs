using UnityEngine;

public class AprilFools_3DSprite : MonoBehaviour
{
    private void Update()
    {
        if (!(Camera.main == null))
        {
            Vector3 forward = Camera.main.transform.position - base.transform.position;
            forward.y = 0f;
            if (forward.sqrMagnitude > 0.01f)
            {
                base.transform.rotation = Quaternion.LookRotation(forward);
            }
        }
    }
}
