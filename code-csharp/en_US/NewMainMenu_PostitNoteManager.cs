using System.Collections.Generic;
using UnityEngine;

public class NewMainMenu_PostitNoteManager : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> PossiblePostitNotes = new List<Sprite>();

    [SerializeField]
    private MeshRenderer[] PostitNotes;

    private void Awake()
    {
        MeshRenderer[] postitNotes = PostitNotes;
        foreach (MeshRenderer obj in postitNotes)
        {
            int index = Random.Range(0, PossiblePostitNotes.Count);
            obj.material.mainTexture = PossiblePostitNotes[index].texture;
            obj.material.SetTexture("_EmissionMap", PossiblePostitNotes[index].texture);
            PossiblePostitNotes.Remove(PossiblePostitNotes[index]);
        }
    }
}
