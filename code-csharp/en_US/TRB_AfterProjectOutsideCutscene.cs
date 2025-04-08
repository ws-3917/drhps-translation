using System.Collections;
using UnityEngine;

public class TRB_AfterProjectOutsideCutscene : MonoBehaviour
{
    [Header("-= Cutscene References =-")]
    [SerializeField]
    private CameraManager playerCamera;

    [SerializeField]
    private Animator Toriel;

    [SerializeField]
    private Animator Alphys;

    [SerializeField]
    private Animator Berdly;

    [SerializeField]
    private GameObject BerdlySweatOverlay;

    [SerializeField]
    private Animator Susie;

    [SerializeField]
    private PlayerManager Kris;

    [Header("- Dialogue -")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [Header("- Sounds -")]
    [SerializeField]
    private AudioClip[] CutsceneSounds;

    private void Start()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        DarkworldMenu.Instance.CanOpenMenu = false;
        Kris = PlayerManager.Instance;
        Toriel.SetBool("InCutscene", value: true);
        Alphys.SetBool("InCutscene", value: true);
        Susie.SetBool("InCutscene", value: true);
        Berdly.SetBool("InCutscene", value: true);
        Kris._PMove.AnimationOverriden = true;
        Kris._PMove._anim.SetBool("MOVING", value: false);
        RotateAlphysToDirection(Vector2.down);
        RotateSusieToDirection(Vector2.down);
        RotateBerdlyToDirection(Vector2.down);
        RotateTorielToDirection(Vector2.right);
        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.down);
        Kris._PMove._anim.SetBool("MOVING", value: true);
        Vector2 krisWalkPos = new Vector2(11f, -0.45f);
        yield return null;
        CutsceneUtils.MoveTransformLinear(Kris.transform, krisWalkPos, 2f);
        yield return new WaitForSeconds(1.5f);
        Susie.gameObject.SetActive(value: true);
        RotateSusieToDirection(Vector2.down);
        Susie.SetBool("InCutscene", value: true);
        Susie.Play("IdleSad");
        CutsceneUtils.PlaySound(CutsceneSounds[0]);
        yield return new WaitForSeconds(0.5f);
        Kris._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.up);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        CutsceneUtils.MoveTransformLinear(endPoint: new Vector2(11f, -1.3f), target: Susie.transform, duration: 1f);
        Susie.speed = 2f;
        Susie.Play("WalkSad");
        yield return new WaitForSeconds(0.66f);
        krisWalkPos = new Vector2(9.25f, -0.45f);
        Kris._PMove._anim.SetBool("MOVING", value: true);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.right);
        CutsceneUtils.MoveTransformLinear(Kris.transform, krisWalkPos, 0.33f);
        yield return new WaitForSeconds(0.33f);
        Kris._PMove._anim.SetBool("MOVING", value: false);
        PlayerManager.Instance._PMove.RotatePlayerAnim(Vector2.right);
        RotateSusieToDirection(Vector2.left);
        Susie.Play("IdleSad");
        Susie.speed = 1f;
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: false);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: false);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.35f);
        Berdly.gameObject.SetActive(value: true);
        RotateBerdlyToDirection(Vector2.down);
        Berdly.SetBool("InCutscene", value: true);
        Berdly.speed = 2f;
        Berdly.Play("Walk");
        CutsceneUtils.PlaySound(CutsceneSounds[0]);
        yield return null;
        CutsceneUtils.MoveTransformLinear(Berdly.transform, new Vector2(11f, -1.95f), 0.85f);
        yield return new WaitForSeconds(0.57f);
        CutsceneUtils.MoveTransformLinear(endPoint: new Vector2(9.25f, -1.75f), target: Kris.transform, duration: 0.28f);
        Vector2 vector = new Vector2(8.85f, -1.3f);
        Susie.Play("Susie_Shock_Right");
        CutsceneUtils.MoveTransformLinear(Susie.transform, vector, 0.28f);
        yield return new WaitForSeconds(0.28f);
        RotateBerdlyToDirection(Vector2.left);
        yield return new WaitForSeconds(0.33f);
        RotateBerdlyToDirection(Vector2.up);
        yield return new WaitForSeconds(0.33f);
        RotateBerdlyToDirection(Vector2.right);
        yield return new WaitForSeconds(0.66f);
        Berdly.Play("Idle");
        Berdly.speed = 1f;
        yield return new WaitForSeconds(1f);
        RotateBerdlyToDirection(Vector2.left);
        RotateSusieToDirection(Vector2.right);
        BerdlySweatOverlay.SetActive(value: false);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: false);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
    }

    private void RotateTorielToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Toriel, "VelocityX", "VelocityY", direction);
    }

    private void RotateSusieToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Susie, "VelocityX", "VelocityY", direction);
    }

    private void RotateBerdlyToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Berdly, "VelocityX", "VelocityY", direction);
    }

    private void RotateAlphysToDirection(Vector2 direction)
    {
        CutsceneUtils.RotateCharacterToDirection(Alphys, "VelocityX", "VelocityY", direction);
    }

    public void SusieAngry()
    {
        Susie.Play("Susie_Angry_Left");
    }

    public void SusieIdle()
    {
        Susie.Play("Idle");
    }

    public void SusieAwkward()
    {
        Susie.Play("Susie_Awkward");
    }

    public void SusieLeft()
    {
        RotateSusieToDirection(Vector2.left);
    }

    public void SusieUp()
    {
        RotateSusieToDirection(Vector2.up);
    }

    public void SusieRight()
    {
        RotateSusieToDirection(Vector2.right);
    }

    public void SusieDown()
    {
        RotateSusieToDirection(Vector2.down);
    }

    public void SusieIdleSad()
    {
        Susie.Play("IdleSad");
    }

    public void BerdlyLeft()
    {
        RotateBerdlyToDirection(Vector2.left);
    }

    public void BerdlyUp()
    {
        RotateBerdlyToDirection(Vector2.up);
    }

    public void BerdlyRight()
    {
        RotateBerdlyToDirection(Vector2.right);
    }

    public void BerdlyDown()
    {
        RotateBerdlyToDirection(Vector2.down);
    }
}
