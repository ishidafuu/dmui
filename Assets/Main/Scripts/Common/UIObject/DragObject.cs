using DM;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DragObject : MonoBehaviour
{
    [SerializeField] private GameObject m_DraggingObject;
    [HideInInspector] public Transform m_DraggingParentTransform;

    public void SetDraggingParent(Transform draggingParentTransform)
    {
        if (m_DraggingParentTransform == null)
        {
            m_DraggingParentTransform = draggingParentTransform;
        }
    }
    
    public void BeginDrag(PointerEventData pointerEventData)
    {
        if (m_DraggingParentTransform != null)
        {
            m_DraggingObject.transform.SetParent(m_DraggingParentTransform);
        }
        m_DraggingObject.SetActive(true);
        MoveDraggingObject(pointerEventData);
    }

    public void Drag(PointerEventData pointerEventData)
    {
        MoveDraggingObject(pointerEventData);
    }

    public void EndDrag(PointerEventData pointerEventData)
    {
        Debug.Log($"+++ OnEndDrag");
        m_DraggingObject.SetActive(false);
        MoveDraggingObject(pointerEventData);
    }

    private void MoveDraggingObject(PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_DraggingObject.transform.parent as RectTransform,
            pointerEventData.position,
            UIController.Instance.m_Camera,
            out Vector2 effectLocalPosition);
        m_DraggingObject.transform.localPosition = effectLocalPosition;
    }
}