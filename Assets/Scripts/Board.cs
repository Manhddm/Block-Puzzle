using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public const int Size = 8;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Transform cellsTransform;
    private Cell[,] _cells;
    private int[,] _data = new int[Size, Size]; // [x, y] -> [Column, Row]
    private List<Vector2Int> _hoverPoints = new();

    private List<int> fullLineColumns = new();
    private List<int> fullLineRows = new();

    private void Awake()
    {
        _cells = new Cell[Size, Size];
    }

    private void Start()
    {
        for (int x = 0; x < Size; x++) 
        {
            for (int y = 0; y < Size; y++)
            {
                _cells[x, y] = Instantiate(cellPrefab, cellsTransform);
                _cells[x, y].transform.position = new Vector3(x + 0.5f, y + 0.5f, 0f);
                _cells[x, y].Hide();
            }
        }
    }

    private void HoverPoint(Vector2Int point, int polyominoRow, int polyominoColumn, int[,] polyomino)
    {
        for (int i = 0; i < polyominoRow; i++)
        {
            for (int j = 0; j < polyominoColumn; j++)
            {
                if (polyomino[i, j] > 0)
                {
                    var hoverPoint = point + new Vector2Int(j, i);
                    if (IsValidPoint(hoverPoint))
                        _hoverPoints.Add(hoverPoint);
                    else
                    {
                        _hoverPoints.Clear();
                        return;
                    }
                }
            }
        }
    }

    public bool IsValidPoint(Vector2Int hoverPoint)
    {
        if (hoverPoint.x < 0 || hoverPoint.x >= Size) return false;
        if (hoverPoint.y < 0 || hoverPoint.y >= Size) return false;
        if (_data[hoverPoint.x, hoverPoint.y] > 0) return false; // >0 là có gạch (1 hoặc 2)
        return true;
    }

    public void Hover(Vector2Int position, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRow = polyomino.GetLength(0);
        var polyominoColumn = polyomino.GetLength(1);

        UnHover();    
        UnHighlight(); 

        HoverPoint(position, polyominoRow, polyominoColumn, polyomino); 

        if (_hoverPoints.Count > 0)
        {
            foreach (var hoverPoint in _hoverPoints)
            {
                _data[hoverPoint.x, hoverPoint.y] = 1;
                _cells[hoverPoint.x, hoverPoint.y].Hover();
            }
            Highlight(position, polyominoColumn, polyominoRow);
        }
    }

    public void UnHover()
    {
        foreach (var hoverPoint in _hoverPoints)
        {
            _data[hoverPoint.x, hoverPoint.y] = 0;
            _cells[hoverPoint.x, hoverPoint.y].Hide();
        }
        _hoverPoints.Clear();
    }

    public bool Place(Vector2Int position, int polyominoIndex)
    {
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRow = polyomino.GetLength(0);
        var polyominoColumn = polyomino.GetLength(1);
        UnHover(); 
        HoverPoint(position, polyominoRow, polyominoColumn, polyomino); 
        
        if (_hoverPoints.Count > 0)
        {
       
            foreach (var hoverPoint in _hoverPoints)
            {
                _data[hoverPoint.x, hoverPoint.y] = 2; 
                _cells[hoverPoint.x, hoverPoint.y].Normal();
            }
   
            ClearFullLine(position, polyominoColumn, polyominoRow);
            
            _hoverPoints.Clear();
            return true;
        }
        return false;
    }


    private void ClearFullLine(Vector2Int point, int polyominoColumn, int polyominoRow)
    {
        FullLineColumns(point.x, point.x + polyominoColumn);
        FullLineRows(point.y, point.y + polyominoRow);
        ClearFullLineColumns();
        ClearFullLineRows();
    }

    private void FullLineRows(int fromRow, int toRowExclusive)
    {
        fullLineRows.Clear();
        for (int r = fromRow; r < toRowExclusive; r++) 
        {
            var isFullLine = true;
            for (int c = 0; c < Size; c++) 
            {
                if (_data[c, r] != 2) 
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine) fullLineRows.Add(r);
        }
    }

    private void FullLineColumns(int fromColumn, int toColumnExclusive)
    {
        fullLineColumns.Clear();
        for (int c = fromColumn; c < toColumnExclusive; c++)
        {
            var isFullLine = true;
            for (int r = 0; r < Size; r++)
            {
                if (_data[c, r] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine) fullLineColumns.Add(c);
        }
    }

    private void ClearFullLineRows()
    {
        foreach (var r in fullLineRows)
        {
            for (var c = 0; c < Size; c++)
            {
                _data[c, r] = 0; 
                _cells[c, r].Hide();
            }
        }
    }

    private void ClearFullLineColumns()
    {
        foreach (var c in fullLineColumns)
        {
            for (var r = 0; r < Size; r++)
            {
                _data[c, r] = 0; 
                _cells[c, r].Hide();
            }
        }
    }

    private void UnHighlight()
    {
        foreach (var r in fullLineRows)
            for (var c = 0; c < Size; c++)
                if (_data[c, r] == 2) _cells[c, r].Normal();

        foreach (var c in fullLineColumns)
            for (var r = 0; r < Size; r++)
                if (_data[c, r] == 2) _cells[c, r].Normal();
    }

    private void Highlight(Vector2Int point, int polyominoColumn, int polyominoRow)
    {
        PredictFullLineColumns(point.x, point.x + polyominoColumn);
        PredictFullLineRows(point.y, point.y + polyominoRow);
        
        foreach (var r in fullLineRows)
            for (var c = 0; c < Size; c++)
                if (_data[c, r] == 2) _cells[c, r].Highlight(); 

        foreach (var c in fullLineColumns)
            for (var r = 0; r < Size; r++)
                if (_data[c, r] == 2) _cells[c, r].Highlight(); 
    }

    private void PredictFullLineRows(int fromRow, int toRowExclusive)
    {
        fullLineRows.Clear();
        for (int r = fromRow; r < toRowExclusive; r++)
        {
            var isFullLine = true;
            for (int c = 0; c < Size; c++)
            {
                if (_data[c, r] != 1 && _data[c, r] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine) fullLineRows.Add(r);
        }
    }

    private void PredictFullLineColumns(int fromColumn, int toColumnExclusive)
    {
        fullLineColumns.Clear();
        for (int c = fromColumn; c < toColumnExclusive; c++)
        {
            var isFullLine = true;
            for (int r = 0; r < Size; r++)
            {
                if (_data[c, r] != 1 && _data[c, r] != 2)
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine) fullLineColumns.Add(c);
        }
    }
}