using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject mouseIndicator, cellIndicator;
    [SerializeField] private GridInput gridInput;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = gridInput.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePos);
        mouseIndicator.transform.position = mousePos;
        cellIndicator.transform.position = grid.CellToWorld(gridPos);
    }
}
