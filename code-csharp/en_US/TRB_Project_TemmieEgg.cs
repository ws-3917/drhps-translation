using System.Collections;
using UnityEngine;

public class TRB_Project_TemmieEgg : MonoBehaviour
{
    [Header("-= Cutscene Chats =-")]
    [SerializeField]
    private CHATBOXTEXT[] CutsceneChats;

    [SerializeField]
    private AudioClip temmiesong_noNoise;

    [SerializeField]
    private AudioClip temmiesong;

    private Animator Temmie;

    private Transform Egg;

    private float[] ChirptriggerTimes = new float[10]
    {
        65f / 96f,
        0.6925f,
        73f / 96f,
        0.7754948f,
        0.8170052f,
        0.82242185f,
        0.8434375f,
        0.8589323f,
        0.9267188f,
        0.941276f
    };

    private bool[] hasChirpTriggered;

    private float lastSongTime;

    private float currentTime;

    private void Start()
    {
        hasChirpTriggered = new bool[ChirptriggerTimes.Length];
        lastSongTime = 0f;
        Temmie = TRB_Projects_Shared.instance.Temmie;
        Egg = TRB_Projects_Shared.instance.Egg;
        MusicManager.PlaySong(temmiesong_noNoise, FadePreviousSong: false, 0f);
        GonerMenu.Instance.ShowMusicCredit("NULL", "Sooski");
        Temmie.transform.position = new Vector2(-0.2f, 0.8f);
        Egg.transform.position = new Vector2(1.1f, 1.675f);
        Temmie.Play("Idle");
        CutsceneUtils.RotateCharacterToDirection(TRB_Projects_Shared.instance.Alphys, "VelocityX", "VelocityY", Vector2.left);
        TRB_Projects_Shared.instance.CreateNewLightShadow(new Vector2(0.95f, 1.825f), new Vector2(1f, 0.7125f));
        StartCoroutine(ProjectCutscene());
    }

    private void Update()
    {
        CheckForChirp();
    }

    private void CheckForChirp()
    {
        if (!MusicManager.Instance.source.isPlaying)
        {
            return;
        }
        float num = MusicManager.Instance.source.time / 38.4f;
        if (MusicManager.Instance.source.time < lastSongTime)
        {
            for (int i = 0; i < hasChirpTriggered.Length; i++)
            {
                hasChirpTriggered[i] = false;
            }
        }
        for (int j = 0; j < ChirptriggerTimes.Length; j++)
        {
            if (!hasChirpTriggered[j] && num >= ChirptriggerTimes[j])
            {
                Debug.Log($"Triggered event at {ChirptriggerTimes[j] * 38.4f} seconds.");
                CutsceneUtils.MoveTransformOnArc(Egg, Egg.position, 0.1f, 0.1f);
                hasChirpTriggered[j] = true;
            }
        }
        lastSongTime = MusicManager.Instance.source.time;
    }

    public IEnumerator ProjectCutscene()
    {
        Temmie.Play("Idle");
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 0, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 1, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 2, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 3, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 4, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 5, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        TRB_Projects_Shared.instance.Berdly.Play("Idle");
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 6, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(2.5f);
        MusicManager.PauseMusic();
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 7, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.25f);
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 8, ForcePosition: true, OnBottom: true);
        MusicManager.ResumeMusic();
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        TRB_Projects_Shared.instance.RemoveLightShadow();
        CutsceneUtils.RunFreshChat(CutsceneChats[0], 9, ForcePosition: true, OnBottom: true);
        while (ChatboxManager.Instance.ChatIsCurrentlyRunning)
        {
            yield return null;
        }
        TRB_Projects_Shared.instance.NextProject();
    }

    public void BerdlyShock()
    {
        TRB_Projects_Shared.instance.Berdly.Play("ShockUp");
    }

    public void Tem_Shake()
    {
        CutsceneUtils.ShakeTransform(Temmie.transform, 0.25f, 3f);
    }

    public void Tem_LookLeft()
    {
        Temmie.SetFloat("VelocityX", -1f);
    }

    public void Tem_LookRight()
    {
        Temmie.SetFloat("VelocityX", 1f);
    }

    public void TemmieSwitchSong()
    {
        MusicManager.Instance.source.Play();
        MusicManager.Instance.source.time = currentTime;
    }

    public void CutTemmieSong()
    {
        currentTime = MusicManager.Instance.source.time;
        MusicManager.Instance.source.clip = temmiesong;
        MusicManager.Instance.source.Stop();
    }
}
