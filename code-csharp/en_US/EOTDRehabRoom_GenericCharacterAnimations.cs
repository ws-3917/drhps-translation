using UnityEngine;

public class EOTDRehabRoom_GenericCharacterAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator Spamtong;

    [SerializeField]
    private Animator Jevil;

    [SerializeField]
    private Animator Carousel;

    [SerializeField]
    private INT_Chat CarouselChatter;

    private void Start()
    {
        if (PlayerPrefs.GetInt("EOTD_Carousel", 0) != 0)
        {
            CarouselChatter.CurrentIndex = 1;
            Carousel.Play("rehabroom_carousel_spin");
        }
    }

    public void SpamtonAnim_Idle_Left()
    {
        Spamtong.Play("Spamton_Idle_Left");
    }

    public void SpamtonAnim_Sad()
    {
        Spamtong.Play("Spamton_Sad");
    }

    public void SpamtonAnim_Cheer()
    {
        Spamtong.Play("Spamton_Cheer");
    }

    public void SpamtonAnim_Cheer2()
    {
        Spamtong.Play("Spamton_Cheer2");
    }

    public void SpamtonAnim_Beg()
    {
        Spamtong.Play("Spamton_Beg");
    }

    public void SpamtonAnim_Laugh()
    {
        Spamtong.Play("Spamton_Laugh");
    }

    public void SpamtonAnim_FoldedArms()
    {
        Spamtong.Play("Spamton_FoldedArms");
    }

    public void SpamtonJevilAnim_Idle_Left()
    {
        SpamtonAnim_Idle_Left();
        JevilAnim_Idle_Right();
    }

    public void CarouselStartSpin()
    {
        Carousel.Play("rehabroom_carousel_spin");
        PlayerPrefs.SetInt("EOTD_Carousel", 1);
        int secureInt = SecurePlayerPrefs.GetSecureInt("TotalCash");
        if (secureInt > 0)
        {
            SecurePlayerPrefs.SetSecureInt("TotalCash", secureInt - 1);
        }
    }

    public void JevilAnim_Idle_Right()
    {
        Jevil.Play("Jevil_Idle_Right");
    }

    public void JevilAnim_Squished()
    {
        Jevil.Play("Jevil_Squished");
    }

    public void JevilAnim_Sad()
    {
        Jevil.Play("Jevil_Sad");
    }

    public void JevilAnim_Laugh()
    {
        Jevil.Play("Jevil_Laugh");
    }

    public void JevilAnim_Dance()
    {
        Jevil.Play("Jevil_Dance");
    }
}
