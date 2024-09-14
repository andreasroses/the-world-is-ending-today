using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private AreaGridBuilder gridBuilder;
    private List<Cell> builtGrid = new List<Cell>();
    
    public void GeneratePuzzleFromGrid(int numReq){
        int numFilled = 0;
        builtGrid = gridBuilder.areaGrid;
        List<Cell> activeCells = new List<Cell>();

        int startX = UnityEngine.Random.Range(gridBuilder.minWidth, gridBuilder.maxWidth+1);
        int startY = UnityEngine.Random.Range(gridBuilder.minHeight, gridBuilder.maxHeight+1);
        
        Cell startCell = builtGrid.Find(puzzleCell => puzzleCell.gridPos == new Vector2Int(startX,startY));
        startCell.filled = true;
        activeCells.Add(startCell);

        while(activeCells.Count > 0 && numFilled < numReq - 1){
            Cell currCell = activeCells[activeCells.Count - 1];

            List<Cell> neighbors = GetUnvisitedNeighbors(currCell);

            if(neighbors.Count > 0){
                Cell randomCell = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];
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
    public List<PieceItemQuantifier> requiredPieces;
    [System.NonSerialized]
    public List<PieceItem> pieceTypesReq;
    public int NumberFilledCellsRequired(){
        Vector2Int tmp;
        int sum = 0;
        foreach(PieceItemQuantifier item in requiredPieces){
            tmp = getPieceSize(item.piece.size);
            sum += tmp.x * tmp.y * item.quantity;
        }
        return sum;
    }

    private Vector2Int getPieceSize(PieceSize size){
        if(size == PieceSize.Low){
            return Vector2Int.one;
        }
        if(size == PieceSize.High){
            return new Vector2Int(1,2);
        }
        return new Vector2Int(2,2);
    }

    public void CollectRequiredPieces(){
        if(pieceTypesReq == null){
            pieceTypesReq = new List<PieceItem>();
        }
        foreach(PieceItemQuantifier p in requiredPieces){
            pieceTypesReq.AddRange(Enumerable.Repeat(p.piece, p.quantity));
        }
    }
}

