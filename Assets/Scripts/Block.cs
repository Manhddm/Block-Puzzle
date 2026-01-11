using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public const int Size = 5;
    [SerializeField] private Cell cellPrefab;
    private readonly Cell[,] _cells = new Cell[Size, Size];
    private Vector3 _previousMousePosition = Vector3.positiveInfinity;
    private Vector3 _position;
    private Vector3 _scale;
    [SerializeField] private Vector3 inputOffset = new Vector3(0.0f, 2.0f, 0.0f);
    private Camera _camera;
    private Vector3 _inputPoint;
    private int _polyominoIndex;
    private Vector2Int _currenDragPoin;
    private Vector2Int _previousDragPoin;
    private Vector2 _center;
    private void Awake()
    {
        _camera = Camera.main;
    }
    public void Initialized()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                // var position = new Vector3(Size, Size - 1 - i, 0);
                // _cells[i, j] = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                _cells[i, j] = Instantiate(cellPrefab, transform);
            }
        }

        _position = transform.position;
        _scale = transform.localScale;
    }

    public void Show(int polyominoIndex)
    {
        Hide();
        _polyominoIndex = polyominoIndex;
        var polyomino = Polyominos.Get(polyominoIndex);
        var polyominoRows = polyomino.GetLength(0);
        var polyominoColumns = polyomino.GetLength(1);
        _center = new Vector2(polyominoColumns*0.5f, polyominoRows*0.5f);
        for (int i = 0; i < polyominoRows; i++)
        {
            for (int j = 0; j < polyominoColumns; j++) 
            {
                if (polyomino[i, j] > 0)
                {
                    _cells[i, j].transform.localPosition = new(j - _center.x + 0.5f, i- _center.y + 0.5f, 0.0f);
                    _cells[i,j] .Normal();
                }
            }
        }
    }

    public void Hide()
    {
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                _cells[i,j].Hide();
            }
        }
    }

    #region Mouse Events

    private void OnMouseDown()
    {
        
        transform.localPosition = _position + inputOffset;
        transform.localScale = Vector3.one;
        _inputPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        _currenDragPoin = Vector2Int.RoundToInt((Vector2)transform.position - _center);
        _previousDragPoin = _currenDragPoin;

        _previousMousePosition = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        var currentMousePosition = Input.mousePosition;
        if (currentMousePosition == _previousMousePosition) return;
        var inputDelta =(Vector2)(_camera.ScreenToWorldPoint(Input.mousePosition) - _inputPoint);
        transform.localPosition = _position + inputOffset + (Vector3)inputDelta;
        _currenDragPoin = Vector2Int.RoundToInt((Vector2)transform.position - _center);
        if (_currenDragPoin != _previousDragPoin)
        {
            _previousDragPoin = _currenDragPoin;
            GameplayManager.Instance.board.Hover(_currenDragPoin,  _polyominoIndex);
            
        }
        _previousMousePosition = currentMousePosition;
    }

    private void OnMouseUp()
    {
        _currenDragPoin = Vector2Int.RoundToInt((Vector2)transform.position - _center);
        if (GameplayManager.Instance.board.Place(_currenDragPoin, _polyominoIndex) == true)
        {
            gameObject.SetActive(false);
            GameplayManager.Instance.blocks.Remove();
        }
        
        
        transform.localPosition = _position;
        transform.localScale = _scale;
        _previousMousePosition = Vector3.positiveInfinity;
 

    }

    #endregion
}
