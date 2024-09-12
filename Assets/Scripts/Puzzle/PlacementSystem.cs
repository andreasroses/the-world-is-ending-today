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
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private GridInput gridInput;
    [SerializeField] private ObjectsDatabase db;
    private int selectedObjIndex = -1;
    [SerializeField] private GameObject gridVisual;

    void Start(){
        //StopPlacement();
    }

    public void StartPlacement(int id, ObjectType type){
        selectedObjIndex = db.objectsData.FindIndex(data => data.ID == id);
        if(selectedObjIndex < 0){
            Debug.LogError($"No ID found {id}");
            return;
        }
        gridVisual.SetActive(true);
        cellIndicator.SetActive(true);
    }
    private void StopPlacement()
    {
        throw new NotImplementedException();
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
}