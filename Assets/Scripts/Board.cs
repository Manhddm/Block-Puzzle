using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int Size = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform gridContainer;

    private Cell[,] _gridCells;
    private int[,] _gridData;
    private List<Vector2Int> _currentPreviewPoints = new List<Vector2Int>();
    private List<int> rowToClear  = new  List<int>();
    private List<int> columnToClear = new  List<int>();



    public void Init()
    {
        _gridCells = new Cell[Size, Size];
        _gridData = new int[Size, Size];
        _currentPreviewPoints.Clear();
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                var cell = Instantiate(cellPrefab, gridContainer);
                cell.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                cell.SetEmpty();
                _gridCells[x, y] = cell;
            }
        }
    }
    //Doi sang toa do luoi
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);
        return new Vector2Int(x, y);
    }
    //Kiem tra xem co dat duoc khong
    public bool CanPlace(Vector2Int anchor, int[,] shape)
    {
        int width = shape.GetLength(0);
        int height = shape.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (shape[x, y] == 1)
                {
                    int gridX = anchor.x + x;
                    int gridY = anchor.y + y;
                    if (gridX < 0 || gridX >= Size || gridY < 0 || gridY >= Size) return false;
                    if (_gridData[gridX, gridY ] == 2) return false;
                }
            }
        }
        return  true;
    }

    //Hien thi bong mo (Hover / Preview)
    public void Preview(Vector2Int anchor, int[,] shape, Color color)
    {
        ClearPreview();
        UnHiglight();
        rowToClear.Clear();
        columnToClear.Clear();
        if (!CanPlace(anchor, shape)) return;
        
        int w =  shape.GetLength(0);
        int h = shape.GetLength(1);
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (shape[x, y] == 1)
                {
                    var  p= new Vector2Int(anchor.x + x, anchor.y + y);
                    _gridData[p.x, p.y] = 1;
                    _gridCells[p.x, p.y].SetColor(color);
                    _gridCells[p.x, p.y].SetHover();
                    _currentPreviewPoints.Add(p);
                }
            }
        }

        if (CheckLines())
        {
            LinesHightlight(color);
        }
    }

    public void ClearPreview()
    {
        foreach (var p in _currentPreviewPoints)
        {
            if (_gridData[p.x, p.y] == 1) _gridData [p.x, p.y] = 0;
            if (_gridData[p.x, p.y] == 0) _gridCells[p.x, p.y].SetEmpty();
        }
        _currentPreviewPoints.Clear();
    }

    public bool TryPlace(Vector2Int anchor, int[,] shape, Color  color)
    {
        if (!CanPlace(anchor, shape)) return false;
        int w = shape.GetLength(0);
        int h = shape.GetLength(1);
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (shape[x, y] == 1)
                {
                    int gridX = anchor.x + x;
                    int gridY = anchor.y + y;
                    _gridData[gridX, gridY] = 2;
                    _gridCells[gridX, gridY].SetFilled();
                    _gridCells[gridX, gridY].SetColor(color);
                }
            }
        }

        if (CheckLines())
        {
            ClearLines();
        }
        return true;
    }

    private void UnHiglight()
    {
        foreach (var x in rowToClear)
        {
            for (int y = 0; y < Size; y++)
            {
                if (_gridData[x, y] == 2)
                {
                    _gridCells[x, y].SetFilled();
                }
            }
        }
        foreach (var y in columnToClear)
        {
            for (int x = 0; x < Size; x++)
            {
                if (_gridData[x, y] == 2)
                {
                    _gridCells[x, y].SetFilled();
                }
            }
        }
        
    }
    private void LinesHightlight(Color color)
    {
        foreach (var x in rowToClear)
        {
            for (int y = 0; y < Size; y++)
            {
                if (_gridData[x, y] == 2)
                {
                    _gridCells[x, y].SetHighlight(color);
                }
            }
        }
        foreach (var y in columnToClear)
        {
            for (int x = 0; x< Size; x++)
            {
                if (_gridData[x, y] == 2)
                {
                    _gridCells[x, y].SetHighlight(color);
                }
            }
        }
    }

    private bool CheckLines()
    {
        for (int x = 0; x < Size; x++)
        {
            bool full = true;
            for (int y = 0; y < Size; y++)
            {
                if (_gridData[x, y] == 0)
                {
                    full = false;
                    break;
                }
            }
            if (full)  rowToClear.Add(x);
        }
        for (int y = 0; y < Size; y++)
        {
            bool full = true;
            for (int x = 0; x < Size; x++)
            {
                if (_gridData[x, y] == 0)
                {
                    full = false;
                    break;
                }
            }
            if (full)  columnToClear.Add(y);
        }
        if (rowToClear.Count > 0 || columnToClear.Count > 0)
        {
            return true;
        }
        rowToClear.Clear();
        columnToClear.Clear();
        return false;

    }
    

    private void ClearLines()
    {
        foreach (var x in rowToClear)
        {
            for (int y = 0; y < Size; y++)
            {
                _gridData[x, y] = 0;
                _gridCells[x, y].SetEmpty();
            }
        }
        foreach (var y in columnToClear)
        {
            for (int x = 0; x < Size; x++)
            {
                _gridData[x, y] = 0;
                _gridCells[x, y].SetEmpty();
            }
        }
        rowToClear.Clear();
        columnToClear.Clear();
    }

}