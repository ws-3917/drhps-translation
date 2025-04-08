using UnityEngine;

public class BattleAttack_RudinnTrio_Bullet_Diamond : MonoBehaviour
{
    public float maxSpeed = 5f;

    public float acceleration = 2f;

    private float currentSpeed;

    private void OnEnable()
    {
        Object.Destroy(base.gameObject, 2f);
    }

    private void Update()
    {
        currentSpeed += acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
        base.transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
    }
}
