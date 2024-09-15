using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlacementSystem : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private Grid grid;
    [SerializeField] private AreaGridBuilder gridBuilder;
    [SerializeField] private GridInput gridInput;
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private TextMeshProUGUI displayTxt;
    private List<PieceItem> placedPieces = new();
    private PuzzlePiece currPiece;
    private RectTransform cellImg;
    void Start(){
        cellImg = cellIndicator.GetComponent<RectTransform>();
    }

    public void PlacePiece(PieceItem item, Vector2Int size){
        Vector3Int checkPos = grid.WorldToCell(gridInput.GetSelectedMapPosition());
        List<Cell> occupiedCells;
        if(gridBuilder.PlacedCellHere(item, checkPos, size, currPiece.transform.eulerAngles.z, out occupiedCells)){
            currPiece.filledCells = occupiedCells;
            currPiece.isPlaced = true;
            Vector3 placePos = grid.CellToWorld(checkPos);
            currPiece.transform.position = placePos;
            currPiece.transform.SetAsLastSibling();
            placedPieces.Add(currPiece.data.pzlPiece);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currPiece != null){
            UpdateCellIndicatorArea();
            cellIndicator.SetActive(true);
            cellIndicator.transform.rotation = currPiece.transform.rotation;
            Vector3 mousePos = gridInput.GetSelectedMapPosition();
            Vector3Int gridPos = grid.WorldToCell(mousePos);
            
            Vector2 newPivot = GetPivotForRotation(currPiece.transform.eulerAngles.z);
            UpdatePivot(cellImg, newPivot);
            
            Vector3 adjustedPosition = grid.CellToWorld(gridPos);
            cellIndicator.transform.position = adjustedPosition;
            
            cellIndicator.transform.SetAsLastSibling();
            mouseIndicator.transform.position = mousePos;
            if(Input.GetMouseButtonDown(1)){
                currPiece.transform.Rotate(new Vector3(0,0,-90));
            }
        }
        else{
            cellIndicator.SetActive(false);
        }

    }
    public void SetHeldPiece(PuzzlePiece p){
        currPiece = p;
    }

    public void CheckPuzzleInput(){
        if(gridBuilder.PieceListsMatch(placedPieces)){
            displayTxt.gameObject.SetActive(true);
            displayTxt.text = "Correct";
        }
    }

    private Vector2 GetPivotForRotation(float rotationAngle){
        if (Mathf.Abs(rotationAngle % 180) == 90)
        {
            return new Vector2(1, 0);  // For 90 or 270 degrees, pivot at bottom-right
        }
        return new Vector2(0, 0);    // For 0 or 180 degrees, pivot at bottom-left
    }
    private void UpdatePivot(RectTransform rectTransform, Vector2 newPivot){
        Vector2 size = rectTransform.rect.size;
        
        Vector2 oldPivot = rectTransform.pivot;
        Vector2 offset = (oldPivot - newPivot) * size;
        
        rectTransform.pivot = newPivot;
        
        rectTransform.anchoredPosition += offset;
    }

    private void UpdateCellIndicatorArea(){
        Vector2Int currSize = currPiece.data.GetPieceSize();
        //cellImg.sizeDelta = currSize;
        cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currSize.x);
        cellImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currSize.y);
    }
}