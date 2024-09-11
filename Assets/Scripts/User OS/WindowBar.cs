using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBar : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private Canvas canvas;
    private Vector2 newPosition;
    private Transform selectedObj;
    
    void Start(){
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        selectedObj = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform,eventData.position,canvas.worldCamera, out newPosition);
        selectedObj.position = canvas.transform.TransformPoint(newPosition);
    }

    
}
