using System.Collections.Generic;
using UnityEngine;

public class NewMainMenu_Trophys : MonoBehaviour
{
    public List<NewMainMenuTrophy> Trophies = new List<NewMainMenuTrophy>();

    public static NewMainMenu_Trophys instance;

    private void Awake()
    {
        instance = this;
        UpdateTrophys();
    }

    public void UpdateTrophys()
    {
        foreach (NewMainMenuTrophy trophy in Trophies)
        {
            if (PlayerPrefs.GetInt(trophy.PlayerPrefName, 0) != 0)
            {
                trophy.TrophyObject.SetActive(value: true);
            }
        }
    }
}
