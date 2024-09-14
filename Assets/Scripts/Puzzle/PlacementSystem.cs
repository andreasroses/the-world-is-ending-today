// MIT License

// Copyright (c) 2023 Peter

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private Grid grid;
    [SerializeField] private areaGridBuilder gridBuilder;
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private GridInput gridInput;
    private int selectedObjIndex = -1;
    private List<PuzzlePiece> placedPieces = new();
    private PuzzlePiece currPiece;
    void Start(){
        //StopPlacement();
    }

    public void PlacePiece(Vector2Int size, Vector3 endPos){
        Vector3Int checkPos = grid.WorldToCell(endPos);
        List<Cell> occupiedCells;
        if(gridBuilder.PlacedCellHere(checkPos, size, out occupiedCells)){
            currPiece.filledCells = occupiedCells;
            currPiece.isPlaced = true;
            Vector3 placePos = grid.CellToWorld(checkPos);
            currPiece.transform.position = placePos;
            placedPieces.Add(currPiece);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = gridInput.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
        cellIndicator.transform.SetAsLastSibling();
    }

    public void SetHeldPiece(PuzzlePiece p){
        currPiece = p;
    }


}