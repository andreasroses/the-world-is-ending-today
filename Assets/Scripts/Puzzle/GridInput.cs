using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInput : MonoBehaviour
{
    [SerializeField] private RectTransform puzzleBG;
    [SerializeField] private Camera mainCam;
    private Vector3 lastPos;

    public Vector3 GetSelectedMapPosition(){
        Vector3 mousePos = Input.mousePosition;
        if(RectTransformUtility.RectangleContainsScreenPoint(puzzleBG, mousePos, mainCam)){
            RectTransformUtility.ScreenPointToWorldPointInRectangle(puzzleBG, mousePos, mainCam, out lastPos);
        }
        return lastPos;
    }

    public void OnPointerClick(PointerEventData eventData){

    }
}
