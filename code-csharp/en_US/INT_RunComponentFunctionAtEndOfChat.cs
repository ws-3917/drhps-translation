using UnityEngine;

public class INT_RunComponentFunctionAtEndOfChat : MonoBehaviour
{
    [Header("WARNING! This component is removed once ran")]
    [Space(20f)]
    public INT_Chat TargetChat;

    public CHATBOXTEXT TargetTextbox;

    public Component TargetComponent;

    [Space(5f)]
    public string MethodName;

    private void Update()
    {
        if (TargetChat.FinishedText && ChatboxManager.Instance.previouschatboxtext == TargetTextbox)
        {
            if (TargetComponent != null && TargetComponent.GetType().GetMethod(MethodName) != null)
            {
                TargetComponent.GetType().GetMethod(MethodName).Invoke(TargetComponent, null);
            }
            else
            {
                MonoBehaviour.print(TargetComponent.gameObject.name);
                MonoBehaviour.print(MethodName);
                MonoBehaviour.print("did you forget to make the method public?");
            }
            Object.Destroy(this);
        }
    }
}
