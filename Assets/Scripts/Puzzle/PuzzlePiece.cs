using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PuzzlePiece : MonoBehaviour , IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private PlacementSystem placeSys;
    private Vector2Int pieceSize;
    public bool isPlaced = false;
    private Vector3 startPos;
    [System.NonSerialized]
    public List<Cell> filledCells = new();
    private Vector2 newPosition;
    void OnEnable()
    {
        placeSys = GameObject.FindGameObjectWithTag("PlacementSystem").GetComponent<PlacementSystem>();
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
