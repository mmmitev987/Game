using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableSlot : MonoBehaviour, IDropHandler
{
    private GameObject label;

    void Start()
    {
        // Save reference to the label (first child)
        label = transform.GetChild(0).gameObject;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount <= 1) {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.parentBeforeDrag = transform;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }

    void OnTransformChildrenChanged() // The Unity function OnTransformChildrenChanged() is called after a child has been added to or removed from a GameObject’s Transform.
    {
        // If the slot has only the label left, show it again
        if (transform.childCount <= 1)
        {
            label.SetActive(true);
        }
        else {
            label.SetActive(false);
        }
        
    }
}
