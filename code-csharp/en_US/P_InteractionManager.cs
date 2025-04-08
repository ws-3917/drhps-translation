using System.Collections;
using UnityEngine;

public class P_InteractionManager : MonoBehaviour
{
    public Vector2 InteractionRadiusDirection;

    public P_MovementBase PlayerMovement;

    public PlayerManager PlayerManager;

    public Animator InteractDIRController;

    public BoxCollider2D InteractCollider;

    public bool CanInteract = true;

    private void Update()
    {
        if (Input.GetKeyDown(PlayerInput.Instance.Key_Confirm) && PlayerManager._PlayerState == PlayerManager.PlayerState.Game && CanInteract && !ChatboxManager.Instance.ChatIsCurrentlyRunning && !ChatboxManager.Instance.ChatboxInteractDebounce)
        {
            AttemptInteract();
        }
    }

    private void AttemptInteract()
    {
        InteractionRadiusDirection = new Vector2(PlayerMovement._anim.GetFloat("MOVEMENTX"), PlayerMovement._anim.GetFloat("MOVEMENTY"));
        InteractDIRController.SetFloat("DIRX", InteractionRadiusDirection.x);
        InteractDIRController.SetFloat("DIRY", InteractionRadiusDirection.y);
        StartCoroutine(InteractCheckTimer());
    }

    private IEnumerator InteractCheckTimer()
    {
        InteractCollider.enabled = true;
        yield return new WaitForSeconds(0.0333f);
        InteractCollider.enabled = false;
    }
}
