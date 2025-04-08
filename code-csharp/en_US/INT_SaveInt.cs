using UnityEngine;

public class INT_SaveInt : MonoBehaviour
{
    public SaveManager SaveManager;

    public string IntName;

    public int IntValue;

    public bool RunOnce;

    private bool hasran;

    public void RUN()
    {
        if (RunOnce && !hasran)
        {
            hasran = true;
            SaveManager.SaveIntValue(IntName, IntValue);
        }
        if (!RunOnce)
        {
            SaveManager.SaveIntValue(IntName, IntValue);
        }
    }
}
