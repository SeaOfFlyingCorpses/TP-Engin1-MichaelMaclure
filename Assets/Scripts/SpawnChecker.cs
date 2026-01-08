using System;
using System.Collections.Generic;
using UnityEngine;

namespace Checker
{
    public class SpawnChecker : MonoBehaviour
    {
        [SerializeField] private int gridSizeX = 8;
        [SerializeField] private int gridSizeY = 8;
        [SerializeField] private Transform gridPos;
        
        [SerializeField] private GameObject whiteTilePrefab;
        [SerializeField] private GameObject blackTilePrefab;
        
        public List<GameObject> spawnedTiles = new List<GameObject>();

        private void Start()
        {
            SpawnBoard();
        }

        private void SpawnBoard()
        {
            float tileSize = 10f; 
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    bool isWhite = (i + j) % 2 == 0;
                    GameObject prefabToSpawn = isWhite ? whiteTilePrefab : blackTilePrefab;

                    Vector3 spawnPos = gridPos.position + new Vector3(i * tileSize, 0, j * tileSize);

                    GameObject tile = Instantiate(
                        prefabToSpawn,
                        spawnPos,
                        Quaternion.identity,
                        gridPos
                    );
                    spawnedTiles.Add(tile);
                }
            }
        }
    }
}
