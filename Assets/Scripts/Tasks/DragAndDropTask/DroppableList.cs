using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableList : MonoBehaviour, IDropHandler
{
    

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        draggableItem.parentBeforeDrag = transform;
        //transform.GetChild(0).gameObject.SetActive(false);
    }
}
