using System.Collections;
using UnityEngine;

public class OverworldEnemy_Interaction : MonoBehaviour
{
    [SerializeField]
    private Animator EnemyAnimator;

    [SerializeField]
    private string SpotPlayerAnimation;

    [SerializeField]
    private AudioClip SpotPlayerSound;

    [SerializeField]
    private GameObject CheckmarkObject;

    [SerializeField]
    private bool LWM_CouldBeOpen;

    [SerializeField]
    private bool DWM_CouldBeOpen;

    public Battle Battle;

    [HideInInspector]
    public bool HasBeganBattle;

    private void Awake()
    {
        if ((bool)Battle.BattleSong)
        {
            Battle.BattleSong.LoadAudioData();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !HasBeganBattle)
        {
            HasBeganBattle = true;
            StartCoroutine(SpotPlayerTimed());
        }
    }

    public void ForceTriggerBattle()
    {
        if (!HasBeganBattle)
        {
            HasBeganBattle = true;
            StartCoroutine(SpotPlayerTimed());
        }
    }

    private IEnumerator SpotPlayerTimed()
    {
        BattleSystem.PlayBattleSoundEffect(SpotPlayerSound);
        MusicManager.PauseMusic();
        CheckmarkObject.SetActive(value: true);
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Cutscene;
        LWM_CouldBeOpen = LightworldMenu.Instance.CanOpenMenu;
        DWM_CouldBeOpen = DarkworldMenu.Instance.CanOpenMenu;
        DarkworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.CanOpenMenu = false;
        EnemyAnimator.Play(SpotPlayerAnimation);
        yield return new WaitForSeconds(1f);
        DarkworldMenu.Instance.CanOpenMenu = DWM_CouldBeOpen;
        LightworldMenu.Instance.CanOpenMenu = LWM_CouldBeOpen;
        BattleSystem.StartBattle(Battle, base.transform.position);
        Object.Destroy(base.gameObject);
    }
}
