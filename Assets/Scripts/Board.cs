using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
    public const int Size = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;
    private Cell[,] _cells;
    private int[,] _data = new int[Size, Size]; //0 Empty, 1 hover, 2 normal
    private List<Vector2Int> _hoverPoints = new();
    private void Awake()
    {
        _cells = new Cell[Size, Size];
    }

    private void Start()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                _cells[i, j] = Instantiate(cellPrefab, cellsTransform);
                _cells[i,j].transform.position = new Vector3(i+0.5f, j+0.5f, 0f);
                _cells[i,j].Hide();
            }
        }
    }

    private void HoverPoint(Vector2Int point, int polyominoRow, int polyominoColumn, int[,] polyomino)
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; i++)
            {
                if (polyomino[i, j] > 0)
                {
                    var hoverPo
                }
            }
        }
    }
    public void Hover(Vector2Int position, int polyominoIndex)
    {
        
    }

    private void Hover()
    {
        foreach (var hoverPoint in _hoverPoints)
        {
            _data[hoverPoint.x, hoverPoint.y] = 1;
            _cells[hoverPoint.x, hoverPoint.y].Hover();
        }
    }
    private void UnHover()
    {
        foreach (var hoverPoint in _hoverPoints)
        {
            _data[hoverPoint.x, hoverPoint.y] = 0;
            _cells[hoverPoint.x, hoverPoint.y].Hover();
        }
        _hoverPoints.Clear();
    }
}
