using UnityEngine;

public class NewMainMenu_CheatCode : MonoBehaviour
{
    private KeyCode[] konamiCode = new KeyCode[10]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A
    };

    [SerializeField]
    private AudioClip UnlockSFX;

    private int currentIndex;

    private void Update()
    {
        if (!Input.anyKeyDown)
        {
            return;
        }
        if (Input.GetKeyDown(konamiCode[currentIndex]))
        {
            currentIndex++;
            if (currentIndex >= konamiCode.Length)
            {
                UnlockGambling();
                currentIndex = 0;
            }
        }
        else
        {
            currentIndex = 0;
        }
    }

    private void UnlockGambling()
    {
        PlayerPrefs.SetInt("UnlockedGambling", 1);
        PlayerPrefs.Save();
        CutsceneUtils.PlaySound(UnlockSFX);
        Debug.Log("Gambling unlocked!");
    }
}
