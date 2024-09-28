using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameBoardMoreDeadCells : MonoBehaviour
{
    // all SerializeFields variables can be edited through Unity's interface
    [SerializeField] private Tilemap currentState;
    [SerializeField] private Tilemap nextState;

    [SerializeField] private Tile aliveTile;
    [SerializeField] private Tile deadTile;

    [SerializeField] private Pattern pattern;

    [SerializeField] private float updateInterval = 0.05f;

    // variables used only in the code
    private HashSet<Vector3Int> aliveCells;
    private HashSet<Vector3Int> cellsToCheck;

    // variables displayed in real time on Unity's interface
    public int population { get; private set; }
    public int iterations { get; private set; }
    public float time { get; private set; }

    // Unity method used to initialize variables before the application starts
    private void Awake()
    {
        aliveCells = new HashSet<Vector3Int>();
        cellsToCheck = new HashSet<Vector3Int>();
    }

    // clears all states, cells, and other variables
    private void Clear()
    {
        currentState.ClearAllTiles();
        nextState.ClearAllTiles();

        aliveCells.Clear();
        cellsToCheck.Clear();

        population = 0;
        iterations = 0;
        time = 0f;
    }

    // Unity method used to start the simulation
    private void Start()
    {
        SetPattern(pattern);
    }

    // sets initial pattern
    private void SetPattern(Pattern pattern)
    {
        Clear();

        // gets pattern center
        Vector2Int center = pattern.GetCenter();

        // makes all pattern's cells alive
        for (int i = 0; i < pattern.cells.Length; i++)
        {
            // gets cell
            Vector3Int cell = (Vector3Int)(pattern.cells[i] - center);
            // makes cell alive
            currentState.SetTile(cell, aliveTile);
            // adds cell to alive cell list (HashSet)
            aliveCells.Add(cell);
        }

        // defines total population (alive cells in initial pattern)
        population = aliveCells.Count;
    }

    // Unity method called once after the pattern is loaded
    private void OnEnable()
    {
        StartCoroutine(Simulate());
    }

    // starts the simulation
    private IEnumerator Simulate()
    {
        // sets time interval
        var interval = new WaitForSeconds(updateInterval);
        yield return interval;

        while (enabled)
        {
            // updates state
            UpdateState();
            // updates displayed variables
            population = aliveCells.Count;
            iterations++;
            time += updateInterval;
            // continues
            yield return interval;
        }
    }

    // the Game of Life logic
    private void UpdateState()
    {
        cellsToCheck.Clear();

        // gets cells to check (all neighbors of alive cells)
        // adds them to the cellsToCheck list (HashSet)
        foreach (Vector3Int cell in aliveCells)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        // skips current cell
                        // more cells stay dead -> interesting effect for project?
                        continue;
                    }

                    cellsToCheck.Add(cell + new Vector3Int(x, y, 0));
                }
            }
        }

        // sets next state for all the cells that must be checked
        // these are the rules of the Game of Life
        foreach (Vector3Int cell in cellsToCheck)
        {
            // counts all neighbors
            int neighbors = CountNeighbors(cell);
            // checks if the cell is alive
            bool isAlive = IsAlive(cell);

            if (!isAlive && neighbors == 3)
            {
                // cell lives
                nextState.SetTile(cell, aliveTile);
                aliveCells.Add(cell);
            }
            else if (isAlive && (neighbors < 2 || neighbors > 3))
            {
                // cell dies
                nextState.SetTile(cell, deadTile);
                aliveCells.Remove(cell);
            }
            else
            {
                // no change
                nextState.SetTile(cell, currentState.GetTile(cell));
            }
        }

        // switches tilemaps for the next round
        Tilemap temp = currentState;
        currentState = nextState;
        nextState = temp;
        nextState.ClearAllTiles();
    }

    // gets all neighbors for one cell
    private List<Vector3Int> GetNeighbors(Vector3Int cell)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        // adds all neighbors to the neighbors list
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbor = cell + new Vector3Int(x, y, 0);

                if (x == 0 && y == 0)
                {
                    // skips current cell
                    continue;
                }
                else
                {
                    neighbors.Add(neighbor);
                }
            }
        }

        return neighbors;
    }

    // gets the number of neighbors
    private int CountNeighbors(Vector3Int cell)
    {
        int count = 0;
        // gets list of all neighbors
        List<Vector3Int> neighbors = GetNeighbors(cell);

        // for each neighbor, if it is alive, increments count
        foreach (var neighbor in neighbors)
        {
            if (IsAlive(neighbor))
            {
                count++;
            }
        }

        return count;
    }

    // sets if cell is alive or dead
    private bool IsAlive(Vector3Int cell)
    {
        return currentState.GetTile(cell) == aliveTile;
    }
}
