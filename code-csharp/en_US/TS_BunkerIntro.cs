using System.Collections;
using UnityEngine;

public class TS_BunkerIntro : MonoBehaviour
{
    [Header("-- References --")]
    [SerializeField]
    private CameraManager playerCamera;

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private InventoryItem shelterKeys;

    [Space(5f)]
    [SerializeField]
    private bool isNightTime = true;

    [Header("- Camera Controls -")]
    [SerializeField]
    private float CameraFollowBB_Top;

    [SerializeField]
    private float CameraFollowBB_Bottom;

    [Header("- Shelter -")]
    [SerializeField]
    private Animator BunkerAnimator;

    [SerializeField]
    private INT_Chat Interaction;

    [SerializeField]
    private ParticleSystem VineParticle;

    [SerializeField]
    private AudioSource cutsceneSource;

    [SerializeField]
    private AudioClip[] CutsceneClips;

    [Space(5f)]
    [SerializeField]
    private CHATBOXTEXT EarlyTimeText;

    private void Start()
    {
        playerTransform = PlayerManager.Instance.transform;
        StartCoroutine(DelayEnablingMenu());
        if (!isNightTime)
        {
            StartCoroutine(DaytimeText());
        }
    }

    private void Update()
    {
        if (PlayerManager.Instance._PMove._anim.GetComponent<SpriteRenderer>().color != new Color(0.1648718f, 0.2496044f, 0.6132076f) && isNightTime)
        {
            PlayerManager.Instance._PMove._anim.GetComponent<SpriteRenderer>().color = new Color(0.1648718f, 0.2496044f, 0.6132076f);
        }
        if (playerTransform.position.y < CameraFollowBB_Top && playerTransform.position.y > CameraFollowBB_Bottom)
        {
            playerCamera.FollowPlayerY = true;
            return;
        }
        if (playerCamera.transform.position.y != CameraFollowBB_Top && playerCamera.transform.position.y != CameraFollowBB_Bottom)
        {
            if (Vector2.Distance(playerCamera.transform.position, Vector2.up * CameraFollowBB_Top) < Vector2.Distance(playerCamera.transform.position, Vector2.up * CameraFollowBB_Bottom))
            {
                playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, CameraFollowBB_Top, playerCamera.transform.position.z);
            }
            else
            {
                playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, CameraFollowBB_Bottom, playerCamera.transform.position.z);
            }
        }
        playerCamera.FollowPlayerY = false;
    }

    private IEnumerator DelayEnablingMenu()
    {
        while (UI_FADE.Instance.isFading)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        LightworldMenu.Instance.CanOpenMenu = true;
        DarkworldMenu.Instance.CanOpenMenu = false;
    }

    public void OpenBunker()
    {
        Interaction.gameObject.SetActive(value: false);
        StartCoroutine(BunkerOpenAnimation());
    }

    private IEnumerator BunkerOpenAnimation()
    {
        yield return null;
        ChatboxManager.Instance.EndText();
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        LightworldMenu.Instance.CanOpenMenu = false;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        yield return new WaitForSeconds(0.5f);
        cutsceneSource.PlayOneShot(CutsceneClips[1]);
        for (int i = 0; i < LightworldInventory.Instance.PlayerInventory.Count; i++)
        {
            if (LightworldInventory.Instance.PlayerInventory[i] == shelterKeys)
            {
                LightworldInventory.Instance.PlayerInventory.RemoveAt(i);
                LightworldInventory.Instance.PlayerInventory.Add(null);
                break;
            }
        }
        yield return new WaitForSeconds(1f);
        BunkerAnimator.Play("BunkerOpen");
        cutsceneSource.PlayOneShot(CutsceneClips[0]);
        yield return new WaitForSeconds(1f);
        VineParticle.Play();
        yield return new WaitForSeconds(1.5f);
        LightworldMenu.Instance.CanOpenMenu = true;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
    }

    private IEnumerator DaytimeText()
    {
        yield return new WaitForSeconds(0.5f);
        DEBUG_EnableMarkiplier.ChangeMarkiplierState(DEBUG_EnableMarkiplier.MarkiplierEmotions.Crying);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
        CutsceneUtils.RunFreshChat(EarlyTimeText, 0, ForcePosition: true, OnBottom: false);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        LightworldMenu.Instance.CanOpenMenu = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Vector2.up * CameraFollowBB_Bottom, Vector2.up * CameraFollowBB_Bottom);
    }
}
