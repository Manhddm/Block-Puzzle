using UnityEngine;

public class Blocks : MonoBehaviour
{
    [SerializeField] private Block[] _blocks;
    public float cellSize;
    private void Start()
    {
        var shapeW = (float)GameplayManager.Instance.board.Size / _blocks.Length;
        cellSize = (float)GameplayManager.Instance.board.Size / (5 * _blocks.Length + _blocks.Length + 1);
        for (int i = 0; i < _blocks.Length; i++)
        {
            Vector3 startPos = new Vector3(shapeW * (i + 0.5f) + 0.25f, -0.25f - cellSize * 4.0f, 0f);
            Vector3 startScale = new Vector3(cellSize, cellSize, cellSize);
            _blocks[i].transform.localPosition = startPos;
            _blocks[i].transform.localScale = startScale;
            _blocks[i].Init(); 
        }
        SpawnBlocks();
    }

    public void SpawnBlocks()
    {
        for (int i = 0; i < _blocks.Length; i++)
        {
            _blocks[i].Show(Random.Range(0, Polyominos.shapes.Length));
        }
    }

    public void CheckRefill()
    {
        bool allUsed = true;
        foreach (var block in _blocks)
        {
            if (block.gameObject.activeSelf)
            {
                allUsed = false;
                break;
            }
        }

        if (allUsed)
        {
            SpawnBlocks();
        }
    } 
}