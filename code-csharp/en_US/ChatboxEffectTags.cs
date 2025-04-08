using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ChatboxEffectTags : MonoBehaviour
{
    public TMP_Text ChatboxTextUI;

    [Header("Optional")]
    public TMP_Text ChatboxTextShadowUI;

    [Header("-= Effect Settings =-")]
    [Header("Shaking | link=\"shake\"")]
    public float shakeIntensity = 5f;

    public int seed;

    private System.Random random;

    private TMP_TextInfo textInfo;

    private bool isTyping = true;

    private void Awake()
    {
        random = new System.Random(seed);
        ChatboxTextUI.ForceMeshUpdate();
        textInfo = ChatboxTextUI.textInfo;
    }

    private void OnEnable()
    {
        ChatboxTextUI.OnPreRenderText += ApplyShakeEffect;
        StartCoroutine(ContinuousShake());
    }

    private void OnDisable()
    {
        ChatboxTextUI.OnPreRenderText -= ApplyShakeEffect;
        StopCoroutine(ContinuousShake());
    }

    private void ApplyShakeEffect(TMP_TextInfo textInfo)
    {
        if (textInfo.characterCount == ChatboxTextUI.maxVisibleCharacters)
        {
            isTyping = false;
        }
        ShakeText(textInfo);
    }

    private IEnumerator ContinuousShake()
    {
        while (true)
        {
            yield return null;
            if (!isTyping)
            {
                ChatboxTextUI.ForceMeshUpdate();
                ShakeText(ChatboxTextUI.textInfo);
            }
        }
    }

    private void ShakeText(TMP_TextInfo textInfo)
    {
        for (int i = 0; i < textInfo.linkCount; i++)
        {
            TMP_LinkInfo tMP_LinkInfo = textInfo.linkInfo[i];
            if (!(tMP_LinkInfo.GetLinkID() == "shake"))
            {
                continue;
            }
            for (int j = tMP_LinkInfo.linkTextfirstCharacterIndex; j < tMP_LinkInfo.linkTextfirstCharacterIndex + tMP_LinkInfo.linkTextLength; j++)
            {
                if (j >= textInfo.characterCount)
                {
                    continue;
                }
                TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[j];
                if (tMP_CharacterInfo.isVisible)
                {
                    int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
                    int vertexIndex = tMP_CharacterInfo.vertexIndex;
                    Vector3 vector = new Vector3(((float)random.NextDouble() * 2f - 1f) * Time.deltaTime * shakeIntensity, ((float)random.NextDouble() * 2f - 1f) * Time.deltaTime * shakeIntensity, 0f);
                    Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
                    for (int k = 0; k < 4; k++)
                    {
                        vertices[vertexIndex + k] += vector;
                    }
                }
            }
        }
        for (int l = 0; l < textInfo.meshInfo.Length; l++)
        {
            textInfo.meshInfo[l].mesh.vertices = textInfo.meshInfo[l].vertices;
            ChatboxTextUI.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
            if (ChatboxTextShadowUI != null)
            {
                ChatboxTextShadowUI.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
            }
        }
    }
}
