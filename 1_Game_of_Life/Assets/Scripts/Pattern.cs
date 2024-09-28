using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game of Life / Pattern")]
public class Pattern : ScriptableObject
{
    public Vector2Int[] cells;

    public Vector2Int GetCenter()
    {
        // if the pattern is empty, return 0
        if (cells == null || cells.Length == 0)
        {
            return Vector2Int.zero;
        }

        // initializes min and max with 0
        Vector2Int min = Vector2Int.zero;
        Vector2Int max = Vector2Int.zero;

        // sets min and max value depending on each cell's coordinates
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int cell = cells[i];

            // new min and max values for x (between the current cell and the current min and max)
            min.x = Mathf.Min(cell.x, min.x);
            max.x = Mathf.Max(cell.x, max.x);
            // new min and max values for y (between the current cell and the current min and max)
            min.y = Mathf.Min(cell.y, min.y);
            max.y = Mathf.Max(cell.y, max.y);
        }

        // return center (value between min and max)
        return (min + max) / 2;
    }
}
