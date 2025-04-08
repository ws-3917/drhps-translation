using System.Collections;
using UnityEngine;

public class UprankVideoScene_Cutscene : MonoBehaviour
{
    public Animator UprankAnimator;

    public INT_Chat CutsceneChatter;

    private void Start()
    {
        StartCoroutine(ChatRunDelay());
    }

    private void Update()
    {
        if (CutsceneChatter.FinishedText)
        {
            Object.Destroy(base.gameObject);
        }
    }

    private IEnumerator ChatRunDelay()
    {
        yield return new WaitForSeconds(9.5f);
        CutsceneChatter.RUN();
    }

    public void UprankAnim_Idle()
    {
        UprankAnimator.Play("Uprank_Idle");
    }

    public void UprankAnim_Shrug()
    {
        UprankAnimator.Play("Uprank_Shrug");
    }

    public void UprankAnim_HandsTogether()
    {
        UprankAnimator.Play("Uprank_HandsTogether");
    }
}
