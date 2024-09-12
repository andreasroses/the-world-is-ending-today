using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GeneratePuzzle : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private PuzzleGridBuilder gridBuilder;
    [SerializeField] private List<PuzzleLevel> levels = new List<PuzzleLevel>();
    [SerializeField] private ObjectPoolModified cellSprites;
    private List<GameObject> puzzleCells = new List<GameObject>();
    private List<Cell> builtGrid = new List<Cell>();
    private int filledCellsReq = 0;
    void Awake()
    {
        filledCellsReq = levels[0].NumberFilledCellsRequired();
    }
    public void GeneratePuzzleFromGrid(){
        int numFilled = 0;
        builtGrid = gridBuilder.puzzleGrid;
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
        gridBuilder.puzzleGrid = builtGrid;
        VisualizePuzzleGrid();
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

    private void VisualizePuzzleGrid(){
        foreach(Cell c in builtGrid){
            if(c.filled){
                GameObject cellSpr = cellSprites.GetPooledObject("FilledCell");
                Vector3Int cellPos = new Vector3Int(c.gridPos.x, c.gridPos.y,0);
                cellSpr.transform.SetParent(gameObject.transform);
                Vector3 newPos = grid.CellToWorld(cellPos);
                newPos.z = 200;
                cellSpr.transform.position = newPos;
                cellSpr.transform.localScale = new Vector3(1,1,1);
                RectTransform rect = (RectTransform)cellSpr.transform;
                rect.sizeDelta = new Vector2(1,1);
                cellSpr.SetActive(true);
                puzzleCells.Add(cellSpr);
            }
        }
    }
}

[System.Serializable]
public struct PuzzleLevel{
    public int numBig;
    public int numSmallBlue;
    public int numSmallRed;

    public int NumberFilledCellsRequired(){
        return (numBig*2) + numSmallBlue + numSmallRed;
    }
}

