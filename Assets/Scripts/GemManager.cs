using UnityEngine;
using System.Collections.Generic;

public class GemManager : MonoBehaviour
{
    public GameObject[] gemPrefabs;
    public float gemSize = 0.4f;
    public Vector2 spawnAreaSize = new(4, 1);
    public Vector2 spawnAreaPosition = new(0, -4);
    private List<Vector3> usedPositions = new List<Vector3>();
    private float minDistanceBetweenGems;

    void Start()
    {
        minDistanceBetweenGems = gemSize * 1.1f;
        CreateGemPairs();
    }

    void CreateGemPairs()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject gemType = gemPrefabs[i];

            for (int j = 0; j < 2; j++)
            {
                Vector3 randomPosition;
                int attempts = 0;
                do
                {
                    randomPosition = new Vector3(
                        Random.Range(spawnAreaPosition.x - spawnAreaSize.x / 2, spawnAreaPosition.x + spawnAreaSize.x / 2),
                        Random.Range(spawnAreaPosition.y - spawnAreaSize.y / 2, spawnAreaPosition.y + spawnAreaSize.y / 2),
                        0);
                    attempts++;
                }
                while (!IsPositionValid(randomPosition) && attempts < 100);

                if (attempts < 100)
                {
                    Instantiate(gemType, randomPosition, Quaternion.identity, transform);
                    usedPositions.Add(randomPosition);
                }
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 usedPosition in usedPositions)
        {
            if (Vector3.Distance(position, usedPosition) < minDistanceBetweenGems)
            {
                return false;
            }
        }
        return true;
    }
}