using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEBUG_EnableMarkiplier : MonoBehaviour
{
    [Serializable]
    public class MarkiplierState
    {
        public MarkiplierEmotions Emotion;

        public Sprite MarkiplierIcon;

        public AudioClip EmotionSound;
    }

    public enum MarkiplierEmotions
    {
        Default = 0,
        Annoyed = 1,
        Shock = 2,
        Shock_NoSound = 3,
        Crying = 4,
        Horror = 5,
        CheekyGrin = 6
    }

    public enum Corner
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3
    }

    private KeyCode[] konamiCode = new KeyCode[10]
    {
        KeyCode.M,
        KeyCode.A,
        KeyCode.R,
        KeyCode.K,
        KeyCode.I,
        KeyCode.P,
        KeyCode.L,
        KeyCode.I,
        KeyCode.E,
        KeyCode.R
    };

    [SerializeField]
    private AudioClip UnlockSFX;

    private int currentIndex;

    public static bool MarkiplierEnabled;

    public static DEBUG_EnableMarkiplier instance;

    public RawImage Markiplier;

    [SerializeField]
    public List<MarkiplierState> MarkiplierStates = new List<MarkiplierState>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(konamiCode[currentIndex]))
            {
                currentIndex++;
                if (currentIndex >= konamiCode.Length)
                {
                    ToggleMarkiplier();
                    ChangeMarkiplierState(MarkiplierEmotions.Default);
                    currentIndex = 0;
                }
            }
            else
            {
                currentIndex = 0;
            }
        }
        if (!MarkiplierEnabled)
        {
            return;
        }
        if (GonerMenu.Instance.GonerMenuOpen)
        {
            AnchorRawImage(Corner.BottomRight);
        }
        else if (!BattleSystem.CurrentlyInBattle)
        {
            if (ChatboxManager.Instance.ChatIsCurrentlyRunning)
            {
                if (ChatboxManager.Instance.CurrentChatOnTopPos)
                {
                    AnchorRawImage(Corner.BottomLeft);
                }
                else
                {
                    AnchorRawImage(Corner.TopLeft);
                }
            }
        }
        else
        {
            AnchorRawImage(Corner.TopRight);
        }
    }

    private void ToggleMarkiplier()
    {
        MarkiplierEnabled = !MarkiplierEnabled;
        Markiplier.gameObject.SetActive(MarkiplierEnabled);
        if (MarkiplierEnabled)
        {
            CutsceneUtils.PlaySound(UnlockSFX, CutsceneUtils.DRH_MixerChannels.Dialogue);
        }
    }

    public static void ChangeMarkiplierState(MarkiplierEmotions emotion)
    {
        if (!MarkiplierEnabled)
        {
            return;
        }
        foreach (MarkiplierState markiplierState in instance.MarkiplierStates)
        {
            if (markiplierState.Emotion == emotion)
            {
                instance.Markiplier.texture = markiplierState.MarkiplierIcon.texture;
                if (markiplierState.EmotionSound != null)
                {
                    CutsceneUtils.PlaySound(markiplierState.EmotionSound, CutsceneUtils.DRH_MixerChannels.Dialogue);
                }
            }
        }
    }

    public void AnchorRawImage(Corner corner)
    {
        if (Markiplier == null)
        {
            Debug.LogError("Markiplier is missing???");
            return;
        }
        RectTransform rectTransform = Markiplier.rectTransform;
        switch (corner)
        {
            case Corner.TopLeft:
                rectTransform.anchorMin = new Vector2(0f, 1f);
                rectTransform.anchorMax = new Vector2(0f, 1f);
                rectTransform.pivot = new Vector2(0f, 1f);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case Corner.TopRight:
                rectTransform.anchorMin = new Vector2(1f, 1f);
                rectTransform.anchorMax = new Vector2(1f, 1f);
                rectTransform.pivot = new Vector2(1f, 1f);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case Corner.BottomLeft:
                rectTransform.anchorMin = new Vector2(0f, 0f);
                rectTransform.anchorMax = new Vector2(0f, 0f);
                rectTransform.pivot = new Vector2(0f, 0f);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
            case Corner.BottomRight:
                rectTransform.anchorMin = new Vector2(1f, 0f);
                rectTransform.anchorMax = new Vector2(1f, 0f);
                rectTransform.pivot = new Vector2(1f, 0f);
                rectTransform.anchoredPosition = Vector2.zero;
                break;
        }
    }
}
