using System;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] blocks;
    private int _blockCount = 0;

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
            _blockCount++;
            blocks[i].gameObject.SetActive(true);
            blocks[i].Show(0);
        }
    }

    public void Remove()
    {
        _blockCount--;
        if (_blockCount <= 0)
        {
            Generate();
        }
    }
}
