using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Block : MonoBehaviour
{
    private const  int Size = 5;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private float dragScale = 1.0f;
    [SerializeField] private Vector3 inputOffset = new Vector3(0.0f, 2.0f, 0.0f);
    private Cell[,] _cells = new Cell[Size,Size];
    private int[,] _shapeData = new int[Size,Size];
    public int id;
    [SerializeField]private Vector3 _startPos;
    private Vector3 _startScale;
    private Camera _camera;
    private int _shapeW, _shapeH;
    private Vector2 center;
    private Vector3 inputPos;
    private Color[] _colors = new []{Color.blue, Color.green,  Color.red, Color.cyan, Color.magenta, Color.yellow};
    private Color color;
    private Vector3 _previousMousePosition = Vector3.positiveInfinity;
    private Vector2Int currentDragPoint;
    private void Awake()
    {
        _camera = Camera.main;
    }
    
    public void Init()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                var cell =  Instantiate(cellPrefab, transform);
                cell.SetEmpty();
                _cells[x, y] = cell;
            }
        }
        _startPos = transform.localPosition;
        _startScale = transform.localScale;
    }
    public void Show(int shapeIndex)
    {
        Hide();
        id = shapeIndex;
        _shapeData = Polyominos.GetShape(shapeIndex);
        _shapeW = _shapeData.GetLength(0);
        _shapeH = _shapeData.GetLength(1);
        center = new Vector2(_shapeW * 0.5f, _shapeH * 0.5f);
        color = _colors[Random.Range(0, _colors.Length)];
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                bool isInsideShape = (x < _shapeW && y < _shapeH && _shapeData[x,y] ==1);
                if (isInsideShape)
                {
                    var cell = _cells[x,y];
                    cell.transform.localPosition = new Vector3(x - center.x + 0.5f, y - center.y + 0.5f, 0f);
                    cell.SetColor(color);
                    cell.SetFilled();
                }
                else
                {
                    _cells[x,y].SetEmpty();
                }
            }
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        transform.localPosition = _startPos;
        transform.localScale = _startScale;
        for (int x = 0; x < Size; x++)
        {
            for (int y = 0; y < Size; y++)
            {
                _cells[x,y].SetEmpty();
            }
        }
    }
    private void OnMouseDown()
    {
        transform.localPosition = _startPos + inputOffset;
        transform.localScale = new Vector3(dragScale, dragScale, dragScale);
        inputPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        var currentPos = GameplayManager.Instance.board.WorldToGrid((Vector2)transform.position - center);
        _previousMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        var currentPos= Input.mousePosition;
        if (currentPos == _previousMousePosition) return;
        var inputDelta = (Vector2)(_camera.ScreenToWorldPoint(Input.mousePosition) - inputPos);
        transform.localPosition = _startPos + inputOffset + (Vector3)inputDelta;
        currentDragPoint =  Vector2Int.RoundToInt((Vector2)transform.position - center);
        GameplayManager.Instance.board.Preview(currentDragPoint, Polyominos.GetShape(id), color);
        _previousMousePosition = currentPos;
        
        
    }

    private void OnMouseUp()
    {
        if (!GameplayManager.Instance.board.TryPlace(currentDragPoint, Polyominos.GetShape(id), color))
        {
            transform.localPosition = _startPos;
            var scale = GameplayManager.Instance.blocks.cellSize;
            transform.localScale = new Vector3(scale, scale, scale);
            GameplayManager.Instance.board.ClearPreview();
        }
        else
        {
            gameObject.SetActive(false);
            GameplayManager.Instance.blocks.CheckRefill();
        }
    }
    
    
}
