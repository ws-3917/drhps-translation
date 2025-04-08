using UnityEngine;

public class BattleAttack_Siplett_Homing : MonoBehaviour
{
    [Header("-= Movement =-")]
    [SerializeField]
    private float acceleration = 5f;

    [SerializeField]
    private float maxSpeed = 20f;

    [SerializeField]
    private float rotationSpeed = 2f;

    [SerializeField]
    private float rotationDuration = 3f;

    private Vector2 velocity = Vector2.zero;

    private float rotationTimer;

    private void Start()
    {
        rotationSpeed = Random.Range(2, 4);
    }

    private void Update()
    {
        if (Battle_PlayerSoul.Instance == null)
        {
            Debug.LogWarning("Battle_PlayerSoul.Instance is not set.");
            return;
        }
        rotationTimer += Time.deltaTime;
        if (rotationTimer >= rotationDuration)
        {
            rotationSpeed = 0f;
        }
        Vector2 normalized = ((Vector2)Battle_PlayerSoul.Instance.transform.position - (Vector2)base.transform.position).normalized;
        float b = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
        if (rotationSpeed > 0f)
        {
            float z = Mathf.LerpAngle(base.transform.eulerAngles.z, b, rotationSpeed * Time.deltaTime);
            base.transform.rotation = Quaternion.Euler(0f, 0f, z);
        }
        Vector2 vector = base.transform.right;
        velocity += vector * acceleration * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            velocity = velocity.normalized * maxSpeed;
        }
        base.transform.position += (Vector3)velocity * Time.deltaTime;
    }
}
