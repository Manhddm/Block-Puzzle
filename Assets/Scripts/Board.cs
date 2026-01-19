
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Board : MonoBehaviour
{
    [FormerlySerializedAs("Size")] public int size = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform gridContainer;

    private Cell[,] _gridCells;
    private int[,] _gridData;
    private readonly List<Vector2Int> _currentPreviewPoints = new List<Vector2Int>();
    [SerializeField] private List<int> rowToClear  = new  List<int>();
    private readonly List<int> _columnToClear = new  List<int>();
    // private bool isLose = false;


    private void Update()
    {
        CheckLose();
    }

    public void Init()
    {
        _gridCells = new Cell[size, size];
        _gridData = new int[size, size];
        _currentPreviewPoints.Clear();
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                var cell = Instantiate(cellPrefab, gridContainer);
                cell.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                cell.SetEmpty();
                _gridCells[x, y] = cell;
            }
        }
    }
    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x);
        int y = Mathf.RoundToInt(worldPosition.y);
        return new Vector2Int(x, y);
    }

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
                    if (gridX < 0 || gridX >= size || gridY < 0 || gridY >= size) return false;
                    if (_gridData[gridX, gridY ] == 2) return false;
                }
            }
        }
        return  true;
    }
    
    public void Preview(Vector2Int anchor, int[,] shape, Color color)
    {
        ClearPreview();
        UnHiglight();
        rowToClear.Clear();
        _columnToClear.Clear();
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
            for (int y = 0; y < size; y++)
            {
                if (_gridData[x, y] == 2)
                {
                    _gridCells[x, y].SetFilled();
                }
            }
        }
        foreach (var y in _columnToClear)
        {
            for (int x = 0; x < size; x++)
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
            for (int y = 0; y < size; y++)
            {
                if (_gridData[x, y] == 2)
                {
                    _gridCells[x, y].SetHighlight(color);
                }
            }
        }
        foreach (var y in _columnToClear)
        {
            for (int x = 0; x< size; x++)
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
        for (int x = 0; x < size; x++)
        {
            bool full = true;
            for (int y = 0; y < size; y++)
            {
                if (_gridData[x, y] == 0)
                {
                    full = false;
                    break;
                }
            }
            if (full)  rowToClear.Add(x);
        }
        for (int y = 0; y < size; y++)
        {
            bool full = true;
            for (int x = 0; x < size; x++)
            {
                if (_gridData[x, y] == 0)
                {
                    full = false;
                    break;
                }
            }
            if (full)  _columnToClear.Add(y);
        }
        if (rowToClear.Count > 0 || _columnToClear.Count > 0)
        {
            return true;
        }
        rowToClear.Clear();
        _columnToClear.Clear();
        return false;

    }
    

    private void ClearLines()
    {
        foreach (var x in rowToClear)
        {
            for (int y = 0; y < size; y++)
            {
                _gridData[x, y] = 0;
                _gridCells[x, y].SetEmpty();
            }
        }
        foreach (var y in _columnToClear)
        {
            for (int x = 0; x < size; x++)
            {
                _gridData[x, y] = 0;
                _gridCells[x, y].SetEmpty();
            }
        }
        rowToClear.Clear();
        _columnToClear.Clear();
    }

    public bool CheckLose()
    {
        var blocks = GameplayManager.Instance.blocks.GetBlocks();
        foreach (var block in blocks)
        {
            if (!block.gameObject.activeSelf) continue;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    var vint = new Vector2Int(x, y);
                    if (CanPlace(vint, Polyominos.GetShape(block.id))) return false;
                }
            }
        }
        return true;
    }

}