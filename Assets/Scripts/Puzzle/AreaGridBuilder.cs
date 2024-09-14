using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class AreaGridBuilder : MonoBehaviour
{
    public UnityEvent<int> OnGridBuilt;
    [SerializeField] private RectTransform puzzleArea;
    [SerializeField] private Grid grid;
    [SerializeField] private ObjectPoolModified cellSprites;
    [SerializeField] private List<PuzzleLevel> levels = new List<PuzzleLevel>();
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
    private int filledCellsReq;
    private int currLevel = 0;
    void Awake(){
        filledCellsReq = levels[currLevel].NumberFilledCellsRequired();
        levels[currLevel].CollectRequiredPieces();
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
        OnGridBuilt.Invoke(filledCellsReq);
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
    public bool PlacedCellHere(PieceItem pzlPiece,Vector3Int cellPos, Vector2Int size, float rotationAngle, out List<Cell> occupiedCells){
        Vector2Int checkPos = new Vector2Int(cellPos.x, cellPos.y);
        occupiedCells = new List<Cell>();

        if(!isValidCellPosition(checkPos)){
            return false;
        }

        Cell foundCell = GetCellFromPuzzleGrid(checkPos);

        if(foundCell.filled || neighborsSameType(foundCell, pzlPiece)){
            return false;
        }

        occupiedCells.Add(foundCell);
        
        Vector2Int adjustedSize = AdjustSizeForRotation(size, rotationAngle);
        
        if(adjustedSize == Vector2Int.one){
            foundCell.filled = true;
            foundCell.piece = pzlPiece;
            return true;
        }

        if(isSpaceAvailable(foundCell, adjustedSize, out List<Cell> neighbors)){
            foundCell.filled = true;
            foundCell.piece = pzlPiece;
            foreach (Cell c in neighbors){
                c.filled = true;
                c.piece = pzlPiece;
                occupiedCells.Add(c);
            }
            return true;
        }
        return false;
    }
    private Vector2Int AdjustSizeForRotation(Vector2Int size, float rotationAngle){
        // Normalize the rotation to a 0, 90, 180, 270 range
        rotationAngle = rotationAngle % 360;

        // Swap width and height for 90° or 270° rotations
        if(rotationAngle == 90 || rotationAngle == 270){
            return new Vector2Int(size.y, size.x);
        }

        // No adjustment for 0° or 180° rotations
        return size;
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
    private bool neighborsSameType(Cell home, PieceItem p){
        List<Cell> neighbors = new List<Cell>();
        Vector2Int homePos = home.gridPos;
        Cell neighbor;
        if(neighborCellInPuzzleGrid(new Vector2Int(homePos.x - 1, homePos.y), out neighbor)){
            neighbors.Add(neighbor);
        }
        if(neighborCellInPuzzleGrid(new Vector2Int(homePos.x + 1, homePos.y), out neighbor)){
            neighbors.Add(neighbor);
        }
        if (neighborCellInPuzzleGrid(new Vector2Int(homePos.x, homePos.y - 1), out neighbor)){
            neighbors.Add(neighbor);
        }
            
        if (neighborCellInPuzzleGrid(new Vector2Int(homePos.x, homePos.y + 1), out neighbor)){
            neighbors.Add(neighbor);
        }
        foreach(Cell c in neighbors){
            if(c.piece.Equals(p)){
                return true;
            }
        }
        return false;
    }

    private bool neighborCellInPuzzleGrid(Vector2Int gridPos, out Cell neighbor){
        neighbor = new(Vector2Int.zero);
        if(isValidCellPosition(gridPos)){
            neighbor = puzzleGrid.Find(puzzleCell => puzzleCell.gridPos == gridPos);
            return true;
        }
        return false;
    }
    private List<Vector2Int> CalculateSizePositions(Vector2Int homePos, Vector2Int size){
        List<Vector2Int> positions = new();
        for(int x = 0; x < size.x; x++){
            for(int y = 0; y < size.y; y++){
                if(x == 0 && y == 0) continue;
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

    public bool PieceListsMatch(List<PieceItem> piecesPlaced){
        var check1 = levels[currLevel].pieceTypesReq.Except(piecesPlaced);
        var check2 = piecesPlaced.Except(levels[currLevel].pieceTypesReq);
        return !check1.Any() && !check2.Any();
    }
}

    public class Cell{
        public bool filled;
        public Vector2Int gridPos;
        public PieceItem piece;
        public Cell(Vector2Int pos){
            filled = false;
            gridPos = pos;
            piece.size = PieceSize.None;
            piece.type = PieceType.None;
        }
    }
