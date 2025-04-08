using UnityEngine;

public class TRB_MonsterKid_AnimEvents : MonoBehaviour
{
    [SerializeField]
    private TRB_Project_MonsterKidSnowdrake MKSDProject;

    public void ShakeMonsterKid()
    {
        CutsceneUtils.ShakeTransform(base.transform, 0.25f, 0.5f);
    }

    public void PlaySplatSFX()
    {
        MKSDProject.PlaySplatSFX();
    }
}
