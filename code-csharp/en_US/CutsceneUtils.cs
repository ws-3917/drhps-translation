using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CutsceneUtils : MonoBehaviour
{
    public enum DRH_MixerChannels
    {
        Effect = 0,
        Music = 1,
        Dialogue = 2,
        Master = 3
    }

    public static CutsceneUtils instance;

    public INT_Chat UtilChatbox;

    public AudioSource UtilSource;

    [SerializeField]
    private AudioMixerGroup MixerGroup_Effects;

    [SerializeField]
    private AudioMixerGroup MixerGroup_Music;

    [SerializeField]
    private AudioMixerGroup MixerGroup_Dialogue;

    [SerializeField]
    private AudioMixerGroup MixerGroup_Master;

    private void Awake()
    {
        instance = this;
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
    }

    public static void RotateCharacterToDirection(Animator targetAnimator, string X_floatName, string Y_floatname, Vector2 Direction)
    {
        if (targetAnimator.isActiveAndEnabled)
        {
            targetAnimator.SetFloat(X_floatName, Direction.x);
            targetAnimator.SetFloat(Y_floatname, Direction.y);
        }
        else
        {
            Debug.Log("Unable to rotate Character! | targetAnimator isn't active and or enabled");
        }
    }

    public static void RotateCharacterTowardsPosition(Animator targetAnimator, string X_floatName, string Y_floatname, Vector2 targetPosition)
    {
        if (targetAnimator.isActiveAndEnabled)
        {
            Vector2 vector = targetAnimator.transform.position;
            Vector2 vector2 = targetPosition - vector;
            vector2.Normalize();
            targetAnimator.SetFloat(X_floatName, vector2.x);
            targetAnimator.SetFloat(Y_floatname, vector2.y);
        }
        else
        {
            Debug.Log("Unable to rotate towards position! | targetAnimator isn't active and or enabled");
        }
    }

    public static void ShakeTransform(Transform target, float multiplier = 0.25f, float duration = 2f)
    {
        if (target.gameObject.activeSelf)
        {
            instance.StartCoroutine(instance.ShakeTarget(target, multiplier, duration));
        }
        else
        {
            Debug.Log("Unable to shake transform! | transform gameobject not enabled");
        }
    }

    public static void StopAllEffects()
    {
        instance.StopAllCoroutines();
    }

    public static void PlaySound(AudioClip clip, DRH_MixerChannels TargetMixerGroup = DRH_MixerChannels.Effect, float Volume = 1f, float Pitch = 1f)
    {
        if (clip != null)
        {
            switch (TargetMixerGroup)
            {
                case DRH_MixerChannels.Effect:
                    instance.UtilSource.outputAudioMixerGroup = instance.MixerGroup_Effects;
                    break;
                case DRH_MixerChannels.Music:
                    instance.UtilSource.outputAudioMixerGroup = instance.MixerGroup_Music;
                    break;
                case DRH_MixerChannels.Dialogue:
                    instance.UtilSource.outputAudioMixerGroup = instance.MixerGroup_Dialogue;
                    break;
                case DRH_MixerChannels.Master:
                    instance.UtilSource.outputAudioMixerGroup = instance.MixerGroup_Master;
                    break;
            }
            instance.UtilSource.pitch = Pitch;
            instance.UtilSource.PlayOneShot(clip, Volume);
        }
        else
        {
            Debug.Log("Unable to play sound! | clip is missing or null");
        }
    }

    public IEnumerator ShakeTarget(Transform target, float multiplier = 1f, float duration = 1f)
    {
        if (target != null)
        {
            Vector3 originalPosition = target.position;
            float elapsedTime = 0f;
            while (multiplier > 0f && !(target == null))
            {
                float num = UnityEngine.Random.Range(-1f, 1f) * multiplier;
                target.position = new Vector3(originalPosition.x + num, originalPosition.y, originalPosition.z);
                elapsedTime += Time.fixedDeltaTime;
                multiplier -= Time.fixedDeltaTime * (1f / duration);
                yield return null;
            }
            if (target != null)
            {
                target.position = originalPosition;
            }
        }
    }

    public static void RunFreshChat(CHATBOXTEXT text, int index, bool ForcePosition, bool OnBottom, INT_Chat CustomSource = null)
    {
        if (text != null)
        {
            if (CustomSource != null)
            {
                CustomSource.FirstTextPlayed = false;
                CustomSource.CurrentIndex = index;
                CustomSource.FinishedText = false;
                CustomSource.Text = text;
                if (ForcePosition)
                {
                    CustomSource.ManualTextboxPosition = true;
                    CustomSource.OnBottom = OnBottom;
                }
                CustomSource.RUN();
            }
            else
            {
                instance.UtilChatbox.FirstTextPlayed = false;
                instance.UtilChatbox.CurrentIndex = index;
                instance.UtilChatbox.FinishedText = false;
                instance.UtilChatbox.Text = text;
                if (ForcePosition)
                {
                    instance.UtilChatbox.ManualTextboxPosition = true;
                    instance.UtilChatbox.OnBottom = OnBottom;
                }
                instance.UtilChatbox.RUN();
            }
        }
        else
        {
            Debug.Log("Unable to run fresh chat! | CHATBOXTEXT is null");
        }
    }

    public static void FadeInSprite(SpriteRenderer spriteRenderer, float fadeSpeed = 1f, float targetAlpha = 1f)
    {
        instance.StartCoroutine(FadeInSpriteTimed(spriteRenderer, fadeSpeed, targetAlpha));
    }

    public static void FadeOutSprite(SpriteRenderer spriteRenderer, float fadeSpeed = 1f, float targetAlpha = 0f)
    {
        instance.StartCoroutine(FadeOutSpriteTimed(spriteRenderer, fadeSpeed, targetAlpha));
    }

    public static void FadeSpriteToColor(SpriteRenderer spriteRenderer, Color targetColor, float fadeSpeed = 1f)
    {
        instance.StartCoroutine(FadeSpriteToColorTimed(spriteRenderer, targetColor, fadeSpeed));
    }

    private static IEnumerator FadeInSpriteTimed(SpriteRenderer spriteRenderer, float fadeSpeed, float targetAlpha = 1f)
    {
        if (spriteRenderer != null)
        {
            Color spriteColor = spriteRenderer.color;
            while (spriteColor.a < targetAlpha && spriteRenderer != null)
            {
                spriteColor.a += fadeSpeed * Time.deltaTime;
                spriteColor.a = Mathf.Clamp(spriteColor.a, 0f, targetAlpha);
                spriteRenderer.color = spriteColor;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("FadeInSprite failed! spriteRenderer is null");
        }
    }

    private static IEnumerator FadeOutSpriteTimed(SpriteRenderer spriteRenderer, float fadeSpeed, float targetAlpha = 0f)
    {
        if (spriteRenderer != null)
        {
            Color spriteColor = spriteRenderer.color;
            while (spriteColor.a > targetAlpha)
            {
                spriteColor.a -= fadeSpeed * Time.deltaTime;
                spriteColor.a = Mathf.Clamp(spriteColor.a, targetAlpha, 1f);
                spriteRenderer.color = spriteColor;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("FadeOutSprite failed! spriteRenderer is null");
        }
    }

    private static IEnumerator FadeSpriteToColorTimed(SpriteRenderer spriteRenderer, Color targetColor, float fadeSpeed)
    {
        Color currentColor = spriteRenderer.color;
        while (currentColor != targetColor)
        {
            currentColor = (spriteRenderer.color = Color.Lerp(currentColor, targetColor, fadeSpeed * Time.deltaTime));
            if (Vector4.Distance(currentColor, targetColor) < 0.01f)
            {
                spriteRenderer.color = targetColor;
                break;
            }
            yield return null;
        }
    }

    public static void FadeInText(TextMeshProUGUI textMeshUI, float fadeSpeed = 1f, float targetAlpha = 1f)
    {
        instance.StartCoroutine(FadeInTextTimed(textMeshUI, fadeSpeed, targetAlpha));
    }

    public static void FadeOutText(TextMeshProUGUI textMeshUI, float fadeSpeed = 1f, float targetAlpha = 0f)
    {
        instance.StartCoroutine(FadeOutTextTimed(textMeshUI, fadeSpeed, targetAlpha));
    }

    private static IEnumerator FadeInTextTimed(TextMeshProUGUI textMeshUI, float fadeSpeed, float targetAlpha = 1f)
    {
        Color textColor = textMeshUI.color;
        while (textColor.a < targetAlpha)
        {
            textColor.a += fadeSpeed * Time.deltaTime;
            textColor.a = Mathf.Clamp(textColor.a, 0f, targetAlpha);
            textMeshUI.color = textColor;
            yield return null;
        }
    }

    private static IEnumerator FadeOutTextTimed(TextMeshProUGUI textMeshUI, float fadeSpeed, float targetAlpha = 0f)
    {
        Color textColor = textMeshUI.color;
        while (textColor.a > targetAlpha)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textColor.a = Mathf.Clamp(textColor.a, targetAlpha, 1f);
            textMeshUI.color = textColor;
            yield return null;
        }
    }

    public static void FadeInText3D(TextMeshPro textMesh, float fadeSpeed = 1f, float targetAlpha = 1f)
    {
        instance.StartCoroutine(FadeInText3DTimed(textMesh, fadeSpeed, targetAlpha));
    }

    public static void FadeOutText3D(TextMeshPro textMesh, float fadeSpeed = 1f, float targetAlpha = 0f)
    {
        instance.StartCoroutine(FadeOutText3DTimed(textMesh, fadeSpeed, targetAlpha));
    }

    private static IEnumerator FadeInText3DTimed(TextMeshPro textMesh, float fadeSpeed, float targetAlpha = 1f)
    {
        Color textColor = textMesh.color;
        while (textColor.a < targetAlpha)
        {
            textColor.a += fadeSpeed * Time.deltaTime;
            textColor.a = Mathf.Clamp(textColor.a, 0f, targetAlpha);
            textMesh.color = textColor;
            yield return null;
        }
    }

    private static IEnumerator FadeOutText3DTimed(TextMeshPro textMesh, float fadeSpeed, float targetAlpha = 0f)
    {
        Color textColor = textMesh.color;
        while (textColor.a > targetAlpha)
        {
            textColor.a -= fadeSpeed * Time.deltaTime;
            textColor.a = Mathf.Clamp(textColor.a, targetAlpha, 1f);
            textMesh.color = textColor;
            yield return null;
        }
    }

    public static void MoveTransformOnArc(Transform target, Vector3 endPoint, float height, float duration, bool rotateAlongArc = false)
    {
        instance.StartCoroutine(ArcMotionRoutine(target, endPoint, height, duration, rotateAlongArc));
    }

    private static IEnumerator ArcMotionRoutine(Transform target, Vector3 endPoint, float height, float duration, bool rotateAlongArc)
    {
        Vector3 startPoint = target.position;
        Vector3 previousPosition = startPoint;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float num = Mathf.Clamp01(elapsedTime / duration);
            Vector3 vector = Vector3.Lerp(startPoint, endPoint, num);
            float y = Mathf.Sin(num * MathF.PI) * height;
            Vector3 vector3 = (target.position = vector + new Vector3(0f, y, 0f));
            if (rotateAlongArc && num > 0f)
            {
                Vector3 normalized = (vector3 - previousPosition).normalized;
                float z = Mathf.Atan2(normalized.y, normalized.x) * 57.29578f;
                target.rotation = Quaternion.Euler(0f, 0f, z);
            }
            previousPosition = vector3;
            yield return null;
        }
        target.position = endPoint;
        if (rotateAlongArc)
        {
            Vector3 normalized2 = (endPoint - startPoint).normalized;
            float z2 = Mathf.Atan2(normalized2.y, normalized2.x) * 57.29578f;
            target.rotation = Quaternion.Euler(0f, 0f, z2);
        }
    }

    public static void MoveTransformLinear(Transform target, Vector3 endPoint, float duration)
    {
        if (instance != null)
        {
            instance.StartCoroutine(MoveTransformLinearRoutine(target, endPoint, duration));
        }
    }

    private static IEnumerator MoveTransformLinearRoutine(Transform target, Vector3 endPoint, float duration)
    {
        if (target == null)
        {
            Debug.LogError("Target Transform cannot be null.");
            yield break;
        }
        Vector3 startPosition = target.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            target.position = Vector3.Lerp(startPosition, endPoint, t);
            yield return null;
        }
        target.position = endPoint;
    }

    public static void MoveTransformSmooth(Transform target, Vector3 endPoint, float duration)
    {
        if (instance != null)
        {
            instance.StartCoroutine(MoveTransformSmoothRoutine(target, endPoint, duration));
        }
    }

    private static IEnumerator MoveTransformSmoothRoutine(Transform target, Vector3 endPoint, float duration)
    {
        if (target == null)
        {
            Debug.LogError("Target Transform cannot be null.");
            yield break;
        }
        Vector3 startPosition = target.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            t = Mathf.SmoothStep(0f, 1f, t);
            target.position = Vector3.Lerp(startPosition, endPoint, t);
            yield return null;
        }
        target.position = endPoint;
    }

    public static void MoveTransformEaseOut(Transform target, Vector3 endPoint, float duration, float BeginEase = 0.5f)
    {
        if (instance != null)
        {
            instance.StartCoroutine(MoveTransformEaseOutRoutine(target, endPoint, duration, BeginEase));
        }
    }

    private static IEnumerator MoveTransformEaseOutRoutine(Transform target, Vector3 endPoint, float duration, float BeginEase = 0.5f)
    {
        if (target == null)
        {
            Debug.LogError("Target Transform cannot be null.");
            yield break;
        }
        Vector3 startPosition = target.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float num = Mathf.Clamp01(elapsedTime / duration);
            if (num >= BeginEase)
            {
                float t = (num - BeginEase) / (1f - BeginEase);
                t = Mathf.SmoothStep(0f, 1f, t);
                num = Mathf.Lerp(BeginEase, 1f, t);
            }
            target.position = Vector3.Lerp(startPosition, endPoint, num);
            yield return null;
        }
        target.position = endPoint;
    }

    public static void FadeInUIImage(Image image, float fadeSpeed = 1f, float targetAlpha = 1f)
    {
        instance.StartCoroutine(FadeUIImage(image, fadeSpeed, targetAlpha));
    }

    public static void FadeOutUIImage(Image image, float fadeSpeed = 1f, float targetAlpha = 0f)
    {
        instance.StartCoroutine(FadeUIImage(image, fadeSpeed, targetAlpha));
    }

    private static IEnumerator FadeUIImage(Image image, float fadeSpeed, float targetAlpha = 1f)
    {
        Color textColor = image.color;
        float startAlpha = textColor.a;
        float alphaDirection = Mathf.Sign(targetAlpha - startAlpha);
        while (Mathf.Abs(textColor.a - targetAlpha) > 0.01f)
        {
            textColor.a += fadeSpeed * Time.deltaTime * alphaDirection;
            textColor.a = Mathf.Clamp(textColor.a, Mathf.Min(startAlpha, targetAlpha), Mathf.Max(startAlpha, targetAlpha));
            image.color = textColor;
            yield return null;
        }
        textColor.a = targetAlpha;
        image.color = textColor;
    }
}
