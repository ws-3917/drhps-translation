using UnityEngine;

public class MP_LivingRoom_InteractableCutscenes : MonoBehaviour
{
    [Header("- References -")]
    public ActivePartyMember Susie;

    private void Start()
    {
        Susie = PartyMemberSystem.Instance.HasMemberInParty(PartyMemberSystem.Instance.Default_Susie);
        MonoBehaviour.print(Susie);
        if (Susie.PartyMemberDescription != null)
        {
            Susie.PartyMemberTransform.position = new Vector2(4.4f, -5.3f);
            Susie.PartyMemberFollowSettings.RotateSusieToDirection(Vector2.up);
        }
    }

    public void MP_LivingRoom_SusieAngry()
    {
        if (Susie != null)
        {
            Susie.PartyMemberFollowSettings.AnimationOverriden = true;
            Susie.PartyMemberFollowSettings.SusieAnimator.Play("Susie_Angry_Left");
        }
    }

    public void MP_LivingRoom_SusieIdleReturn()
    {
        if (Susie != null)
        {
            Susie.PartyMemberFollowSettings.AnimationOverriden = false;
            Susie.PartyMemberFollowSettings.SusieAnimator.Play("Idle");
        }
    }

    public void MP_LivingRoom_SansDoorShakePlayer()
    {
        PlayerManager.Instance.ShakePlayer();
    }
}
