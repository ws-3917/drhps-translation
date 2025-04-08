using System.Collections.Generic;
using UnityEngine;

public class P_InteractCollision : MonoBehaviour
{
    private bool SuccessfulInteraction;

    private List<Component> ListOfInteractables = new List<Component>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((bool)other.GetComponent<INT_Chat>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_Chat>());
        }
        if ((bool)other.GetComponent<INT_Generic>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_Generic>());
        }
        if ((bool)other.GetComponent<INT_ChangeBool>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_ChangeBool>());
        }
        if ((bool)other.GetComponent<INT_ChangePlayerVariable>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_ChangePlayerVariable>());
        }
        if ((bool)other.GetComponent<INT_DestroyComponent>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_DestroyComponent>());
        }
        if ((bool)other.GetComponent<INT_PlaySound>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_PlaySound>());
        }
        if ((bool)other.GetComponent<INT_EnableGameObject>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_EnableGameObject>());
        }
        if ((bool)other.GetComponent<INT_SaveInt>())
        {
            ListOfInteractables.Add(other.GetComponent<INT_SaveInt>());
        }
        Component component = null;
        float num = float.PositiveInfinity;
        foreach (Component listOfInteractable in ListOfInteractables)
        {
            if (Vector2.Distance(PlayerManager.Instance.transform.position, listOfInteractable.transform.position) < num)
            {
                component = listOfInteractable;
            }
        }
        if (component != null)
        {
            if (component.GetType() == typeof(INT_Chat) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_Generic) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("Interact").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_ChangeBool) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_ChangePlayerVariable) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_DestroyComponent) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_PlaySound) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_EnableGameObject) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
            if (component.GetType() == typeof(INT_SaveInt) && !SuccessfulInteraction)
            {
                component.GetType().GetMethod("RUN").Invoke(component, null);
                SuccessfulInteraction = true;
            }
        }
        ListOfInteractables.Clear();
        SuccessfulInteraction = false;
    }
}
