using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum PieceType{
    Radiation, Tornado, Earthquake, Strength
}

public enum PieceSize{
    Low, High, ExtraHigh
}
public class PuzzlePiece : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private PlacementSystem placeSys;
    [SerializeField] private PieceData data;
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
        
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(!isPlaced){
            placeSys.SetHeldPiece(this);
        }
        
    }

    public void OnDrag(PointerEventData eventData){
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)placeSys.canvas.transform,eventData.position,placeSys.canvas.worldCamera, out newPosition);
        transform.position = placeSys.canvas.transform.TransformPoint(newPosition);
    }

    public void OnEndDrag(PointerEventData eventData){
        placeSys.PlacePiece(pieceSize, transform.position);
        if(!isPlaced){
            transform.position = startPos;
        }
        placeSys.SetHeldPiece(null);
    }
}

[System.Serializable]
public struct PieceData{
    public PieceType type;
    public PieceSize size;

    public Vector2Int GetPieceSize(){
        if(size == PieceSize.Low){
            return Vector2Int.one;
        }
        if(size == PieceSize.High){
            return new Vector2Int(1,2);
        }
        return new Vector2Int(2,2);
    }
}
