using System.Collections.Generic;
using UnityEngine;

public class LightworldInventory : MonoBehaviour
{
    public List<InventoryItem> PlayerInventory = new List<InventoryItem>();

    private static LightworldInventory instance;

    public static LightworldInventory Instance => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Object.Destroy(base.gameObject);
            return;
        }
        instance = this;
        Object.DontDestroyOnLoad(instance);
    }

    public void ClearInventory()
    {
        PlayerInventory.Clear();
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

    public void RemoveItem(int itemIndex)
    {
        if (PlayerInventory.Count > 0 && PlayerInventory[itemIndex] != null)
        {
            PlayerInventory[itemIndex] = null;
            SortInventory();
        }
    }

    public void SortInventory()
    {
        PlayerInventory.Sort((InventoryItem a, InventoryItem b) => ((a == null) ? 1 : 0) - ((b == null) ? 1 : 0));
    }
}
