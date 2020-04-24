using DM;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvasTran;
    private GameObject draggingObject;

    void Awake()
    {
        canvasTran = transform.parent.parent;
    }

    public void OnBeginDrag(PointerEventData pointerEventData)
    {
        // CreateDragObject();
        // draggingObject.transform.position = pointerEventData.position;
        NewMethod(pointerEventData);
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        // draggingObject.transform.position = pointerEventData.position;
        // 座標変換
        NewMethod(pointerEventData);
    }

    private void NewMethod(PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform,
            pointerEventData.position,
            UIController.Instance.m_Camera,
            out Vector2 effectLocalPosition);
        transform.localPosition = effectLocalPosition;
    }

    public void OnEndDrag(PointerEventData pointerEventData)
    {
        // gameObject.GetComponent<Image>().color = Vector4.one;
        // Destroy(draggingObject);
        NewMethod(pointerEventData);
    }

    // ドラッグオブジェクト作成
    private void CreateDragObject()
    {
        draggingObject = new GameObject("Dragging Object");
        draggingObject.transform.SetParent(canvasTran);
        draggingObject.transform.SetAsLastSibling();
        draggingObject.transform.localScale = Vector3.one;

        // レイキャストがブロックされないように
        CanvasGroup canvasGroup = draggingObject.AddComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;

        Image draggingImage = draggingObject.AddComponent<Image>();
        Image sourceImage = GetComponent<Image>();

        draggingImage.sprite = sourceImage.sprite;
        draggingImage.rectTransform.sizeDelta = sourceImage.rectTransform.sizeDelta;
        draggingImage.color = sourceImage.color;
        draggingImage.material = sourceImage.material;

        gameObject.GetComponent<Image>().color = Vector4.one * 0.6f;
    }
}