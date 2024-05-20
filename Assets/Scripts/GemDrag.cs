using UnityEngine;

public class GemDrag : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 lastValidPosition;
    private bool isDragging = false;
    private GridManager gridManager;

    void Start()
    {
        startPosition = transform.position;
        lastValidPosition = startPosition;
        gridManager = FindObjectOfType<GridManager>();
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
        if (gridManager)
        {
            Vector3 snappedPosition;
            if (gridManager.TrySnapToGrid(transform.position, out snappedPosition))
            {
                if (!gridManager.IsOccupied(snappedPosition))
                {
                    transform.position = snappedPosition;
                    lastValidPosition = snappedPosition;
                }
                else
                {
                    transform.position = lastValidPosition;
                }
            }
            else
            {
                transform.position = startPosition;
            }
        }
        else
        {
            transform.position = startPosition;
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }
}