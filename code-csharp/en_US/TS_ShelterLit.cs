using System.Collections;
using UnityEngine;

public class TS_ShelterLit : MonoBehaviour
{
    [Header("-- References --")]
    [SerializeField]
    private GameObject BlackFade;

    [SerializeField]
    private AudioClip snd_noise;

    private void Start()
    {
        LightworldMenu.Instance.CanOpenMenu = false;
        LightworldMenu.Instance.AllowInput = false;
        StartCoroutine(Cutscene());
    }

    private IEnumerator Cutscene()
    {
        yield return new WaitForSeconds(1.5f);
        BlackFade.SetActive(value: false);
        CutsceneUtils.PlaySound(snd_noise);
        LightworldMenu.Instance.CanOpenMenu = true;
        LightworldMenu.Instance.AllowInput = true;
        PlayerManager.Instance._PlayerState = PlayerManager.PlayerState.Game;
    }
}
