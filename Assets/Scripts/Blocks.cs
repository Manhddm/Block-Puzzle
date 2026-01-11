using System;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;

    private void Start()
    {
        var blockWidth = (float)Board.Size/blocks.Length;
        var cellSize = (float)Board.Size / (Block.Size * blocks.Length + blocks.Length + 1);
        for (var i = 0; i < blocks.Length; i++)
        {
            blocks[i].transform.localPosition = new Vector3(blockWidth * (i + 0.5f) + 0.25f, -0.25f - cellSize * 4.0f, 0f);
            blocks[i].transform.localScale = new Vector3(cellSize, cellSize, cellSize);
            blocks[i].Initialized();
            
        }

        Generate(); 
    }

    private void Generate()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].Show(0);
        }
    }
}
