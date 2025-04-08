using System.Collections.Generic;
using UnityEngine;

public class HypotheticalSetup : MonoBehaviour
{
    [Header("Below are saved flags that will be reset upon loading this scene")]
    [SerializeField]
    private List<PPFloat> SavedFloatsToReset = new List<PPFloat>();

    [SerializeField]
    private List<PPString> SavedStringsToReset = new List<PPString>();

    [SerializeField]
    private List<PPInt> SavedIntsToReset = new List<PPInt>();

    [Header("Choose to reset the player inventory to a specific set of items")]
    [SerializeField]
    private bool ResetLightworldInventory = true;

    [SerializeField]
    private bool ResetDarkworldInventory = true;

    [SerializeField]
    private InventoryItem[] NewLightworldInventory;

    [SerializeField]
    private InventoryItem[] NewDarkworldInventory;

    [SerializeField]
    private InventoryItem[] NewDarkworldKeyItems;

    [SerializeField]
    private bool AllowShowTipText = true;

    private void Start()
    {
        ResetSavedValues();
        ResetInventoryToNew();
        UI_LoadingIcon.ToggleLoadingIcon(showIcon: false);
        if (PlayerPrefs.GetInt("TipShown_Pause") == 0 && AllowShowTipText)
        {
            PlayerPrefs.SetInt("TipShown_Pause", 1);
            GonerMenu.Instance.ShowTip($"Press <color=yellow>{PlayerInput.Instance.Key_Pause}</color> to pause the game");
        }
    }

    private void ResetSavedValues()
    {
        if (SavedFloatsToReset.Count > 0)
        {
            foreach (PPFloat item in SavedFloatsToReset)
            {
                PlayerPrefs.SetFloat(item.PP_FloatName, item.PP_Value);
            }
        }
        if (SavedStringsToReset.Count > 0)
        {
            foreach (PPString item2 in SavedStringsToReset)
            {
                PlayerPrefs.SetString(item2.PP_StringName, item2.PP_Value);
            }
        }
        if (SavedIntsToReset.Count <= 0)
        {
            return;
        }
        foreach (PPInt item3 in SavedIntsToReset)
        {
            PlayerPrefs.SetInt(item3.PP_IntName, item3.PP_Value);
        }
    }

    private void ResetInventoryToNew()
    {
        if (ResetLightworldInventory && LightworldInventory.Instance != null && NewLightworldInventory.Length != 0)
        {
            LightworldInventory.Instance.ClearInventory();
            LightworldInventory.Instance.SetupNewInventory(NewLightworldInventory);
        }
        if (ResetDarkworldInventory && DarkworldInventory.Instance != null)
        {
            if (NewDarkworldInventory.Length != 0 || NewDarkworldKeyItems.Length != 0)
            {
                DarkworldInventory.Instance.ClearInventory();
            }
            if (NewDarkworldInventory.Length != 0)
            {
                DarkworldInventory.Instance.SetupNewInventory(NewDarkworldInventory);
            }
            if (NewDarkworldKeyItems.Length != 0)
            {
                DarkworldInventory.Instance.SetupKeyItems(NewDarkworldKeyItems);
            }
        }
    }
}
