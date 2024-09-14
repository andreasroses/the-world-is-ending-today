using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private areaGridBuilder gridBuilder;
    [SerializeField] private List<PuzzleLevel> levels = new List<PuzzleLevel>();
    
    private List<Cell> builtGrid = new List<Cell>();
    private int filledCellsReq = 0;
    void Awake()
    {
        filledCellsReq = levels[0].NumberFilledCellsRequired();
    }
    public void GeneratePuzzleFromGrid(){
        int numFilled = 0;
        builtGrid = gridBuilder.areaGrid;
        List<Cell> activeCells = new List<Cell>();

        int startX = Random.Range(gridBuilder.minWidth, gridBuilder.maxWidth+1);
        int startY = Random.Range(gridBuilder.minHeight, gridBuilder.maxHeight+1);
        
        Cell startCell = builtGrid.Find(puzzleCell => puzzleCell.gridPos == new Vector2Int(startX,startY));
        startCell.filled = true;
        activeCells.Add(startCell);

        while(activeCells.Count > 0 && numFilled < filledCellsReq - 1){
            Cell currCell = activeCells[activeCells.Count - 1];

            List<Cell> neighbors = GetUnvisitedNeighbors(currCell);

            if(neighbors.Count > 0){
                Cell randomCell = neighbors[Random.Range(0, neighbors.Count)];
                randomCell.filled = true;
                activeCells.Add(randomCell);
                numFilled++;
            }else{
                activeCells.RemoveAt(activeCells.Count - 1);
            }
        }
        gridBuilder.MakePuzzleGrid(builtGrid);
    }

    private List<Cell> GetUnvisitedNeighbors(Cell home){
        List<Cell> neighbors = new List<Cell>();
        Vector2Int homePos = home.gridPos;
        Cell neighborCell;
        if (homePos.x > gridBuilder.minWidth && !IsNeighborCellFilled(new Vector2Int(homePos.x - 1, homePos.y), out neighborCell))
            neighbors.Add(neighborCell);
        if (homePos.x < gridBuilder.maxWidth && !IsNeighborCellFilled(new Vector2Int(homePos.x + 1, homePos.y), out neighborCell))
            neighbors.Add(neighborCell);
        if (homePos.y > gridBuilder.minHeight && !IsNeighborCellFilled(new Vector2Int(homePos.x, homePos.y - 1), out neighborCell))
            neighbors.Add(neighborCell);
        if (homePos.y < gridBuilder.maxHeight && !IsNeighborCellFilled(new Vector2Int(homePos.x, homePos.y + 1), out neighborCell))
            neighbors.Add(neighborCell);

        return neighbors;
    }

    private bool IsNeighborCellFilled(Vector2Int neighborPos, out Cell checkedCell){
        Cell neighbor = builtGrid.Find(puzzleCell => puzzleCell.gridPos == neighborPos);
        checkedCell = neighbor;
        return neighbor.filled;
    }
}

[System.Serializable]
public struct PuzzleLevel{
    public List<PieceData> requiredPieces;

    public int NumberFilledCellsRequired(){
        Vector2Int tmp;
        int sum = 0;
        foreach(PieceData piece in requiredPieces){
            tmp = piece.GetPieceSize();
            sum += tmp.x * tmp.y;
        }
        return sum;
    }
}

