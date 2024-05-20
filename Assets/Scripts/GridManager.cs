using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellBackground5Prefab;
    public GameObject cellBackground6Prefab;
    public int gridSize = 6;
    public float cellSize = 0.635f;
    private Vector3[,] gridPositions;
    private GameObject[,] occupiedGems;
    private int score = 0;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        gridPositions = new Vector3[gridSize, gridSize];
        occupiedGems = new GameObject[gridSize, gridSize];

        float gridWidth = gridSize * cellSize;
        float gridHeight = gridSize * cellSize;
        Vector3 startPosition = new Vector3(-gridWidth / 2 + cellSize / 2, -gridHeight / 2 + cellSize / 2, 0);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = startPosition + new Vector3(x * cellSize, y * cellSize, 0);
                gridPositions[x, y] = position;

                GameObject cellPrefab = (x + y) % 2 == 0 ? cellBackground5Prefab : cellBackground6Prefab;
                Instantiate(cellPrefab, position, Quaternion.identity, transform);
            }
        }
    }

    public bool TrySnapToGrid(Vector3 position, out Vector3 snappedPosition)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (Vector3.Distance(position, gridPositions[x, y]) < cellSize / 2)
                {
                    if (occupiedGems[x, y] == null)
                    {
                        snappedPosition = gridPositions[x, y];
                        return true;
                    }
                    else
                    {
                        snappedPosition = Vector3.zero;
                        return false;
                    }
                }
            }
        }
        snappedPosition = Vector3.zero;
        return false;
    }

    public void OccupyCell(Vector3 position, GameObject gem)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (Vector3.Distance(position, gridPositions[x, y]) < cellSize / 2)
                {
                    occupiedGems[x, y] = gem;
                    CheckAndDestroyMatchingGems(x, y, gem.GetComponent<SpriteRenderer>().color);
                    return;
                }
            }
        }
    }

    void CheckAndDestroyMatchingGems(int startX, int startY, Color gemColor)
    {
        int horizontalCount = 1;
        for (int x = startX + 1; x < gridSize; x++)
        {
            if (occupiedGems[x, startY] != null && occupiedGems[x, startY].GetComponent<SpriteRenderer>().color == gemColor)
            {
                horizontalCount++;
            }
            else
            {
                break;
            }
        }

        int verticalCount = 1;
        for (int y = startY + 1; y < gridSize; y++)
        {
            if (occupiedGems[startX, y] != null && occupiedGems[startX, y].GetComponent<SpriteRenderer>().color == gemColor)
            {
                verticalCount++;
            }
            else
            {
                break;
            }
        }

        if (horizontalCount >= 3)
        {
            for (int x = startX; x < startX + horizontalCount; x++)
            {
                Destroy(occupiedGems[x, startY]);
                occupiedGems[x, startY] = null;
            }
            score += horizontalCount;
        }

        if (verticalCount >= 3)
        {
            for (int y = startY; y < startY + verticalCount; y++)
            {
                Destroy(occupiedGems[startX, y]);
                occupiedGems[startX, y] = null;
            }
            score += verticalCount;
        }
    }

    public bool IsOccupied(Vector3 position)
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (Vector3.Distance(position, gridPositions[x, y]) < cellSize / 2 && occupiedGems[x, y] != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}