using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_LoadingIcon : MonoBehaviour
{
    [SerializeField]
    private RawImage loadingIcon;

    [SerializeField]
    private float fadeSpeed = 2f;

    private float targetOpacity;

    private Coroutine fadeCoroutine;

    public static UI_LoadingIcon Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        Instance = this;
        Object.DontDestroyOnLoad(base.gameObject);
    }

    public static void ToggleLoadingIcon(bool showIcon)
    {
        if (Instance == null)
        {
            Debug.LogError("UI_LoadingIcon instance is not set. Make sure it's added to the scene.");
        }
        else if (Instance.loadingIcon != null)
        {
            Instance.SetTargetOpacity(showIcon ? 1f : 0f);
        }
        else
        {
            Debug.LogError("LoadingIcon RawImage is not assigned in the UI_LoadingIcon.");
        }
    }

    private void SetTargetOpacity(float opacity)
    {
        targetOpacity = opacity;
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeToTargetOpacity());
    }

    private IEnumerator FadeToTargetOpacity()
    {
        Color currentColor = loadingIcon.color;
        while (!Mathf.Approximately(currentColor.a, targetOpacity))
        {
            currentColor.a = Mathf.MoveTowards(currentColor.a, targetOpacity, fadeSpeed * Time.deltaTime);
            loadingIcon.color = currentColor;
            yield return null;
        }
        fadeCoroutine = null;
    }
}
