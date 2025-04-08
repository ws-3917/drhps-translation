using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkworldInventory : MonoBehaviour
{
    public List<InventoryItem> PlayerInventory = new List<InventoryItem>();

    public List<InventoryItem> PlayerKeyItems = new List<InventoryItem>();

    private static DarkworldInventory instance;

    [Header("- Shared References -")]
    [SerializeField]
    private AudioClip snd_garbagenoise;

    public static DarkworldInventory Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void ClearInventory()
    {
        PlayerInventory.Clear();
        PlayerKeyItems.Clear();
    }

    public void SetupNewInventory(InventoryItem[] items)
    {
        if (items.Length == 0)
        {
            return;
        }
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem != null)
            {
                PlayerInventory.Add(inventoryItem);
            }
        }
    }

    public void SetupKeyItems(InventoryItem[] items)
    {
        if (items.Length == 0)
        {
            return;
        }
        foreach (InventoryItem inventoryItem in items)
        {
            if (inventoryItem != null)
            {
                PlayerKeyItems.Add(inventoryItem);
            }
        }
    }

    public void PlayGarbagePhoneSequence()
    {
        ChatboxManager.Instance.AllowInput = false;
        StartCoroutine(GarbagePhoneSequence());
    }

    private IEnumerator GarbagePhoneSequence()
    {
        while (ChatboxManager.Instance.TextIsCurrentlyTyping)
        {
            yield return null;
        }
        MusicManager.PauseMusic();
        CutsceneUtils.PlaySound(snd_garbagenoise);
        yield return new WaitForSeconds(snd_garbagenoise.length + 0.5f);
        ChatboxManager.Instance.MimicInput_Confirm();
        ChatboxManager.Instance.AllowInput = true;
        MusicManager.ResumeMusic();
    }
}
