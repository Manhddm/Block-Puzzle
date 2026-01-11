using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite normal;
    [SerializeField] private Sprite highlight;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Normal()
    {
        gameObject.SetActive(true);
        _spriteRenderer.color = Color.white;
        _spriteRenderer.sprite = normal;
    }

    public void Highlight()
    {
        gameObject.SetActive(true);
        _spriteRenderer.color = Color.white;
        _spriteRenderer.sprite = highlight;
    }

    public void Hover()
    {
        gameObject.SetActive(true);
        _spriteRenderer.color = Color.white;
        _spriteRenderer.color = new (1.0f, 1.0f, 1.0f, 0.5f);
        _spriteRenderer.sprite = normal;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        _spriteRenderer.color = Color.white;
    }
}
