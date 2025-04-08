using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerState
    {
        Game = 0,
        Cutscene = 1,
        NoPlayerMovement = 2,
        Battle = 3
    }

    public P_MovementBase _PMove;

    public P_InteractionManager _PInteract;

    public P_AnimationStateManager _PAnimation;

    public AudioSource PlayerAudioSource;

    public INT_Chat PlayerINT_Chat;

    public SpriteRenderer PlayerSpriteRenderer;

    public float _PlayerHealth = 160f;

    public float _PlayerMaxHealth = 160f;

    [Header("Game,Cutscene,NoPlayerMovement")]
    public PlayerState _PlayerState;

    private static PlayerManager instance;

    public static PlayerManager Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(instance);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.F11))
        {
            Screen.fullScreen = !Screen.fullScreen;
            SettingsManager.Instance.SaveBoolSetting("Fullscreen", Screen.fullScreen);
        }
    }

    public void PlayerPrintDebugTest_One()
    {
        Debug.LogWarning("One");
    }

    public void PlayerPrintDebugTest_Two()
    {
        Debug.LogWarning("Two");
    }

    public void PlayerPrintDebugTest_Three()
    {
        Debug.LogWarning("Three");
    }

    public void ResetToGameState()
    {
        _PlayerState = PlayerState.Game;
    }

    public void ShakePlayer(float multiplier = 0.25f, float duration = 2f)
    {
        StartCoroutine(ShakeTarget(PlayerSpriteRenderer.transform, multiplier, duration));
    }

    private IEnumerator ShakeTarget(Transform target, float multiplier = 1f, float duration = 1f)
    {
        if (target != null)
        {
            Vector3 originalPosition = target.position;
            float elapsedTime = 0f;
            PartyMemberSystem.Instance.SetAllPartyMembersFollowing(Following: false);
            PartyMemberSystem.Instance.SetAllPartyMemberStates(Susie_Follower.MemberFollowerStates.Disabled);
            while (multiplier > 0f && !(target == null))
            {
                float num = Random.Range(-1f, 1f) * multiplier;
                target.position = new Vector2(originalPosition.x + num, originalPosition.y);
                elapsedTime += Time.fixedDeltaTime;
                multiplier -= Time.fixedDeltaTime * (1f / duration);
                yield return null;
            }
            if (target != null)
            {
                target.position = originalPosition;
            }
            PartyMemberSystem.Instance.SetAllPartyMembersFollowing(Following: true);
            PartyMemberSystem.Instance.SetAllPartyMemberStates(Susie_Follower.MemberFollowerStates.CopyingInputs);
        }
    }
}
