using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trailer_WorldCreation_Manager : MonoBehaviour
{
    [SerializeField]
    private List<Trailer_WorldCreation_TileFlicker> FlickerTiles = new List<Trailer_WorldCreation_TileFlicker>();

    [SerializeField]
    private Animator Kris;

    [SerializeField]
    private Animator Camera;

    [SerializeField]
    private Animator Pillar;

    [SerializeField]
    private GameObject PillarDust;

    [SerializeField]
    private Transform BookFall;

    [SerializeField]
    private SpriteRenderer PillarRenderer;

    [SerializeField]
    private Sprite Pillar_WithManual;

    [SerializeField]
    private Sprite Pillar_Transition;

    [SerializeField]
    private Animator StarShine;

    [SerializeField]
    private GameObject Star;

    [SerializeField]
    private Animator LoadUI;

    [SerializeField]
    private TextMeshProUGUI Text;

    private bool Running;

    private float Timer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Running = true;
            StartCoroutine(TileFlicker());
        }
        if (Running)
        {
            Timer += Time.deltaTime;
        }
    }

    private void SortByPosition(List<Trailer_WorldCreation_TileFlicker> inputList)
    {
        inputList.Sort((Trailer_WorldCreation_TileFlicker a, Trailer_WorldCreation_TileFlicker b) => (a.transform.position.y != b.transform.position.y) ? b.transform.position.y.CompareTo(a.transform.position.y) : a.transform.position.x.CompareTo(b.transform.position.x));
        for (int i = 0; i < inputList.Count - 1; i++)
        {
            if (Random.Range(0, 6) >= 3)
            {
                Trailer_WorldCreation_TileFlicker value = inputList[i];
                int index = Random.Range(i, inputList.Count);
                inputList[i] = inputList[index];
                inputList[index] = value;
            }
        }
    }

    private IEnumerator TileFlicker()
    {
        LoadUI.Play("Trailer_Shot2_Load");
        yield return new WaitForSeconds(1f);
        AddText("Loading World");
        Camera.Play("Trailer_Shot2_CameraStart");
        SortByPosition(FlickerTiles);
        foreach (Trailer_WorldCreation_TileFlicker flickerTile in FlickerTiles)
        {
            flickerTile.StartGlitching();
            yield return new WaitForSeconds(0.035f);
        }
        AddText("Finished Loading World");
        MonoBehaviour.print(Timer);
        yield return new WaitForSeconds(0.5f);
        AddText("Adding Objects {0/3}");
        Pillar.Play("Trailer_Shot2_ManualPillar_Fall");
        yield return new WaitForSeconds(0.333f);
        Camera.Play("Trailer_Shot2_CameraShake");
        AddText("Adding Objects {1/3}");
        PillarDust.SetActive(value: true);
        yield return new WaitForSeconds(1.067f);
        while (BookFall.position.y != -1.6f)
        {
            yield return new WaitForSeconds(0f);
            BookFall.position = Vector3.MoveTowards(BookFall.position, new Vector2(BookFall.position.x, -1.6f), 6f * Time.deltaTime);
        }
        AddText("Adding Objects {2/3}");
        BookFall.position = new Vector3(6f, 6f, 6f);
        PillarRenderer.sprite = Pillar_Transition;
        yield return new WaitForSeconds(0.15f);
        PillarRenderer.sprite = Pillar_WithManual;
        yield return new WaitForSeconds(1f);
        AddText("Adding Objects {3/3}");
        Star.SetActive(value: true);
        StarShine.Play("Trailer_Shot2_StarShine");
        yield return new WaitForSeconds(1.5f);
        AddText("Importing Vessel");
        Kris.Play("Trailer_Shot2_KrisLoad");
    }

    private void AddText(string text)
    {
        Text.text = text;
    }
}
