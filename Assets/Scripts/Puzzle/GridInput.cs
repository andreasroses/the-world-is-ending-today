using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class GridInput : MonoBehaviour
{
    [SerializeField] private RectTransform puzzleBG;
    [SerializeField] private Camera mainCam;
    private Vector3 lastPos;
    [SerializeField] private LayerMask gridMask;

    public Vector3 GetSelectedMapPosition(){
        Vector3 mousePos = Input.mousePosition;
        Vector2 calcPos = Vector3.zero;
        if(RectTransformUtility.RectangleContainsScreenPoint(puzzleBG, mousePos, mainCam)){
            //lastPos = mainCam.ScreenToWorldPoint(mousePos);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(puzzleBG, mousePos, mainCam, out lastPos);
        }
        return lastPos;
    }
}
