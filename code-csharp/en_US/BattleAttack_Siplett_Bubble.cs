using System.Collections;
using UnityEngine;

public class BattleAttack_Siplett_Bubble : MonoBehaviour
{
    [Header("-= Movement =-")]
    [SerializeField]
    private float upwardSpeed = 1f;

    [SerializeField]
    private float swayDistance = 0.5f;

    [SerializeField]
    private Vector2 swayFrequency = new Vector2(1f, 3f);

    [Header("-= Lifetime =-")]
    [SerializeField]
    private Vector2 lifetimeRange = new Vector2(2f, 5f);

    [SerializeField]
    private AudioClip[] popSFXs;

    private float originalX;

    private float swayDirection = 1f;

    private float targetSwayDirection = 1f;

    private float swayFrequenceFixed;

    private Animator animator;

    private bool isPopping;

    private void Start()
    {
        originalX = base.transform.position.x;
        animator = GetComponent<Animator>();
        float time = Random.Range(lifetimeRange.x, lifetimeRange.y);
        Invoke("PopBubble", time);
        swayFrequenceFixed = Random.Range(swayFrequency.x, swayFrequency.y);
        base.transform.localScale = Vector3.one * Random.Range(0.35f, 0.45f);
        animator.speed = Random.Range(0.8f, 1.2f);
    }

    private void Update()
    {
        if (!isPopping)
        {
            base.transform.Translate(Vector3.up * upwardSpeed * Time.deltaTime);
            swayDirection = Mathf.Lerp(swayDirection, targetSwayDirection, Time.deltaTime * 5f);
            float num = Mathf.Sin(Time.time * swayFrequenceFixed) * swayDistance;
            num *= swayDirection;
            float x = Mathf.Clamp(originalX + num, originalX - swayDistance, originalX + swayDistance);
            base.transform.position = new Vector3(x, base.transform.position.y, base.transform.position.z);
            if (Mathf.Abs(base.transform.position.x - originalX) >= swayDistance)
            {
                targetSwayDirection = 0f - targetSwayDirection;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Battle_Soul")
        {
            PopBubble();
        }
    }

    private void PopBubble()
    {
        if (!isPopping)
        {
            isPopping = true;
            if (animator != null)
            {
                animator.speed = 1f;
                animator.Play("Pop");
            }
            BattleSystem.PlayBattleSoundEffect(popSFXs[Random.Range(0, popSFXs.Length)]);
            StartCoroutine(DestroyAfterPop());
        }
    }

    private IEnumerator DestroyAfterPop()
    {
        yield return new WaitForSeconds(0.111f);
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.333f);
        Object.Destroy(base.gameObject);
    }
}
