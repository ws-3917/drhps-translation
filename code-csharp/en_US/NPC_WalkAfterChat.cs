using UnityEngine;

public class NPC_WalkAfterChat : MonoBehaviour
{
    public INT_Chat Chat;

    public NPC NPC;

    public Vector3[] WalkToPositions;

    public float[] WalkSpeed;

    private int WalkToIndex = -1;

    public int ChatIndexToWalkTo;

    private bool HasRan;

    private void Update()
    {
        if (Chat.FinishedText && Chat.CurrentIndex == ChatIndexToWalkTo && !HasRan)
        {
            HasRan = true;
            WalkToIndex++;
            if (WalkToIndex < WalkToPositions.Length)
            {
                NPC.WalkToPosition(WalkToPositions[WalkToIndex], WalkSpeed[WalkToIndex]);
            }
        }
        if (NPC.FinishedMoveTo && WalkToIndex < WalkToPositions.Length)
        {
            NPC.FinishedMoveTo = false;
            WalkToIndex++;
            if (WalkToIndex < WalkToPositions.Length)
            {
                NPC.WalkToPosition(WalkToPositions[WalkToIndex], WalkSpeed[WalkToIndex]);
            }
        }
    }
}
