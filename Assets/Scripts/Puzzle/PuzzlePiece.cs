using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum PieceType{
    Radiation, Tornado, Earthquake, Strength, None
}

public enum PieceSize{
    Low, High, ExtraHigh, None
}
public class PuzzlePiece : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform rectTransform;
    private PlacementSystem placeSys;
    public PieceData data;
    private Vector2Int pieceSize;
    [System.NonSerialized]
    public bool isPlaced = false;
    private Vector3 startPos;
    [System.NonSerialized]
    public List<Cell> filledCells = new();
    private Vector2 newPosition;
    void OnEnable()
    {
        placeSys = GameObject.FindGameObjectWithTag("PlacementSystem").GetComponent<PlacementSystem>();
        pieceSize = data.GetPieceSize();
    }

    void Start(){
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        UpdatePivot(GetPivotForRotation(transform.eulerAngles.z));

        
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(!isPlaced){
            placeSys.SetHeldPiece(this);
        }
        
    }

    public void OnDrag(PointerEventData eventData){
        if(!isPlaced){
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)placeSys.canvas.transform,eventData.position,placeSys.canvas.worldCamera, out newPosition);
            transform.position = placeSys.canvas.transform.TransformPoint(newPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if(!isPlaced){
            placeSys.PlacePiece(data.pzlPiece, pieceSize);
        }
        if(!isPlaced){
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
        placeSys.SetHeldPiece(null);
    }

    private Vector2 GetPivotForRotation(float rotationAngle){
    // Determine the pivot based on rotation
    // Example logic for 90-degree rotations
        if (Mathf.Abs(rotationAngle % 180) == 90)
        {
            return new Vector2(1, 0);  // For 90 or 270 degrees, pivot at bottom-right
        }
        return new Vector2(0, 0);    // For 0 or 180 degrees, pivot at bottom-left
    }
    private void UpdatePivot(Vector2 newPivot){
        Vector2 size = rectTransform.rect.size;
        
        Vector2 oldPivot = rectTransform.pivot;
        Vector2 offset = (oldPivot - newPivot) * size;
        
        rectTransform.pivot = newPivot;
        
        //rectTransform.anchoredPosition += offset;
    }
}

[System.Serializable]
public struct PieceData{
    public PieceItem pzlPiece;

    public Vector2Int GetPieceSize(){
        if(pzlPiece.size == PieceSize.Low){
            return Vector2Int.one;
        }
        if(pzlPiece.size == PieceSize.High){
            return new Vector2Int(1,2);
        }
        return new Vector2Int(2,2);
    }
}

[System.Serializable]
public struct PieceItemQuantifier{
    public PieceItem piece;
    public int quantity;
}

[System.Serializable]
public struct PieceItem{
    public PieceSize size;
    public PieceType type;
}
