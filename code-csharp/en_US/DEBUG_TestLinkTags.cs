using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DEBUG_TestLinkTags : MonoBehaviour
{
    public TMP_Text textMeshPro;

    public float shakeIntensity = 5f;

    public int seed;

    private TMP_TextInfo textInfo;

    private bool isShaking;

    private System.Random random;

    private void Start()
    {
        random = new System.Random(seed);
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;
        StartCoroutine(ShakeCharacters());
    }

    private IEnumerator ShakeCharacters()
    {
        isShaking = true;
        while (isShaking)
        {
            textMeshPro.ForceMeshUpdate();
            textInfo = textMeshPro.textInfo;
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
                textMeshPro.UpdateGeometry(textInfo.meshInfo[l].mesh, l);
            }
            yield return null;
        }
    }
}
