using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PuzzleGridBuilder : MonoBehaviour
{
    public UnityEvent OnPuzzleBuilt;
    [SerializeField] private RectTransform puzzleArea;
    [SerializeField] private Grid grid;
    private Vector3[] puzzleBounds =  new Vector3[4];
    [System.NonSerialized]
    public List<Cell> puzzleGrid = new List<Cell>();
    [System.NonSerialized]
    public int minWidth = 0;
    [System.NonSerialized]
    public int maxWidth = 0;
    [System.NonSerialized]
    public int minHeight = 0;
    [System.NonSerialized]
    public int maxHeight = 0;
    void Awake(){
        puzzleArea.GetWorldCorners(puzzleBounds);
        BuildGridArray();
    }

    private void BuildGridArray(){
        Vector3Int btmLeftCell = grid.WorldToCell(puzzleBounds[0]);
        Vector3Int topRightCell = grid.WorldToCell(puzzleBounds[2]);
        minWidth = btmLeftCell.x ;
        maxWidth = topRightCell.x - 1;
        minHeight = btmLeftCell.y + 1;
        maxHeight = topRightCell.y ;
        for(int x = btmLeftCell.x; x < topRightCell.x; x++){
            for(int y = topRightCell.y; y > btmLeftCell.y; y--){
                puzzleGrid.Add(new Cell(new Vector2Int(x,y)));
            }
        }
        OnPuzzleBuilt.Invoke();
    }

    void OnDrawGizmos(){
        Color tmp = Color.white;
        Vector3Int btmLeftCell = grid.WorldToCell(puzzleBounds[0]);
        Vector3Int topRightCell = grid.WorldToCell(puzzleBounds[2]);
        for(int x = btmLeftCell.x; x < topRightCell.x; x++){
            for(int y = topRightCell.y; y > btmLeftCell.y; y--){
                if(x == 0 && y == -5){
                    tmp = Color.magenta;
                }
                Vector3 cellPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
                Debug.DrawLine(cellPosition, cellPosition + Vector3.right * grid.cellSize.x, tmp, 100f); // Right edge
                Debug.DrawLine(cellPosition, cellPosition + Vector3.up * grid.cellSize.y, tmp, 100f); // Top edge
            }
        }
    }
}

    public class Cell{
        public bool filled;
        public Vector2Int gridPos;
        public int filledObjID;

        public Cell(Vector2Int pos){
            filled = false;
            gridPos = pos;
            filledObjID = -1;
        }
    }
