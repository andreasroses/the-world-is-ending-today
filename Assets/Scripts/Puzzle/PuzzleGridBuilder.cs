using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class areaGridBuilder : MonoBehaviour
{
    public UnityEvent OnPuzzleBuilt;
    [SerializeField] private RectTransform puzzleArea;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectPoolModified cellSprites;
    private List<GameObject> puzzleCells = new List<GameObject>();
    private Vector3[] puzzleBounds =  new Vector3[4];
    [System.NonSerialized]
    public List<Cell> areaGrid = new List<Cell>();
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
                areaGrid.Add(new Cell(new Vector2Int(x,y)));
            }
        }
        OnPuzzleBuilt.Invoke();
    }

    public void MakePuzzleGrid(List<Cell> markedGrid){
        puzzleGrid.Clear();
        foreach(Cell c in markedGrid){
            if(c.filled){
                c.filled = false;
                puzzleGrid.Add(c);
            }
        }
        VisualizeAreaGrid();
    }
    public bool PlacedCellHere(Vector3Int cellPos, Vector2Int size, out List<Cell> occupiedCells){
        Vector2Int checkPos = new Vector2Int(cellPos.x, cellPos.y);
        occupiedCells = new List<Cell>();

        if(!isValidCellPosition(checkPos)){
            return false;
        }

        Cell foundCell = GetCellFromPuzzleGrid(checkPos);

        if(foundCell.filled){
            return false;
        }

        occupiedCells.Add(foundCell);

        if(size == Vector2Int.one){
            foundCell.filled = true;
            return true;
        }

        if(isSpaceAvailable(foundCell, size, out List<Cell> neighbors)){
            foundCell.filled = true;
            foreach (Cell c in neighbors){
                c.filled = true;
                occupiedCells.Add(c);
            }
            return true;
        }
        return false;
    }

    private bool isValidCellPosition(Vector2Int cellPos){
        return puzzleGrid.Exists(puzzleCell => puzzleCell.gridPos == cellPos);
    }

    private Cell GetCellFromPuzzleGrid(Vector2Int cellPos){
        return puzzleGrid.Find(puzzleCell => puzzleCell.gridPos == cellPos);
    }
    private bool isSpaceAvailable(Cell home, Vector2Int size , out List<Cell> neighbors){
        List<Vector2Int> sizePositions = CalculateSizePositions(home.gridPos, size);
        List<Cell> openSpots = new();
        int numUnfilled = 0;
        foreach(Vector2Int pos in sizePositions){
            Cell neighbor;
            if(!IsNeighborCellFilled(pos, out neighbor)){
                numUnfilled++;
                openSpots.Add(neighbor);
            }
        }
        neighbors = openSpots;
        return sizePositions.Count == numUnfilled;
    }
    private List<Vector2Int> CalculateSizePositions(Vector2Int homePos, Vector2Int size){
        List<Vector2Int> positions = new();
        for(int x = 0; x < size.x; x++){
            for(int y = 0; y < size.y; y++){
                positions.Add(homePos + new Vector2Int(x, y));
            }
        }
        return positions;
    }
    private bool IsNeighborCellFilled(Vector2Int neighborPos, out Cell checkedCell){
            if(isValidCellPosition(neighborPos)){
                Cell neighbor = puzzleGrid.Find(puzzleCell => puzzleCell.gridPos == neighborPos);
                checkedCell = neighbor;
                return neighbor.filled;
            }
            checkedCell = null;
            return true;
    }
    private void VisualizeAreaGrid(){
        foreach(Cell c in puzzleGrid){
            GameObject cellSpr = cellSprites.GetPooledObject("FilledCell");
            Vector3Int cellPos = new Vector3Int(c.gridPos.x, c.gridPos.y, 0);
            cellSpr.transform.SetParent(gameObject.transform.parent);
            Vector3 newPos = grid.CellToWorld(cellPos);
            newPos.z = 200;
            cellSpr.transform.position = newPos;
            cellSpr.transform.localScale = new Vector3(1, 1, 1);
            RectTransform rect = (RectTransform)cellSpr.transform;
            rect.sizeDelta = new Vector2(1, 1);
            cellSpr.SetActive(true);
            puzzleCells.Add(cellSpr);
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
