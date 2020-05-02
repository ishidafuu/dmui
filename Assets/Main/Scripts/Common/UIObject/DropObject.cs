using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropObject : MonoBehaviour//, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void Drop(PointerEventData pointerEventData)
    {
        Debug.Log($"---DropObject OnDrop");
    }    
}