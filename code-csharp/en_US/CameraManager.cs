using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public PlayerManager Player;

    public bool FollowPlayerX;

    public bool FollowPlayerY;

    public float CameraSpeed;

    private bool previousvfx;

    public AudioReverbFilter ReverbFilter;

    public static CameraManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        AudioListener.volume = 0.4f;
    }

    private void Update()
    {
        if (!BattleSystem.CurrentlyInBattle)
        {
            MoveCamera();
        }
        if (Player == null)
        {
            Player = PlayerManager.Instance;
        }
        if (previousvfx != SettingsManager.Instance.GetBoolSettingValue("SimpleSFX"))
        {
            previousvfx = SettingsManager.Instance.GetBoolSettingValue("SimpleSFX");
            ReverbFilter.enabled = !SettingsManager.Instance.GetBoolSettingValue("SimpleSFX");
        }
    }

    private void MoveCamera()
    {
        if (FollowPlayerX)
        {
            base.transform.position = new Vector3(PlayerManager.Instance.transform.position.x, base.transform.position.y, -10f);
        }
        if (FollowPlayerY)
        {
            base.transform.position = new Vector3(base.transform.position.x, PlayerManager.Instance.transform.transform.position.y - 0.001f, -10f);
        }
    }
}
