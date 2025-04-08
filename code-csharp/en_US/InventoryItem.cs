using UnityEngine;

[CreateAssetMenu(fileName = "INVENTORYITEM", menuName = "Deltaswap/InventoryItem", order = 1)]
public class InventoryItem : ScriptableObject
{
    [Header("-- Base Variables --")]
    public string ItemName = "Placeholder";

    public CHATBOXTEXT InfoText;

    public CHATBOXTEXT DropText;

    public CHATBOXTEXT UseText;

    [Header("-- Darkworld Specific Variables --")]
    public bool Useable = true;

    public bool IsSharedItem;

    public bool RemoveOnUse = true;

    public string TopbarInfo = "This is an Item!";

    public string BattleInfo = "Heals 50HP";

    public AudioClip ItemUseSound;

    public DarkworldItemDialogue CharacterDialogue;

    [Header("-- Item Attributes --")]
    public int HealthAddition;
}
