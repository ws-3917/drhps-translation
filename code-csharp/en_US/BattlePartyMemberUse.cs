using System;

[Serializable]
public class BattlePartyMemberUse
{
    public BattlePlayerMove BattleMove;

    public BattlePartyMember targetPartyMember;

    public BattleActiveEnemy targetEnemy;

    public BattlePartyMember targetMember;

    public InventoryItem Item_ToUse;

    public int StoredItemOriginalIndex;

    public BattleAction Action_ToRun;
}
