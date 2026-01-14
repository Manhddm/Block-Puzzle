using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite normal;
    private SpriteRenderer _spriteRenderer;
    
    private Color _baseColor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (normal !=  null) _spriteRenderer.sprite = normal;
    }

    private void ValidateComponents()
    {
        if (_spriteRenderer == null) 
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (_spriteRenderer.sprite == null && normal !=   null) _spriteRenderer.sprite = normal;
    }

    public void SetEmpty()
    {
        gameObject.SetActive(false);
    }

    public void SetColor(Color color)
    {
        ValidateComponents();
        _baseColor = color;
        _baseColor.a = 1f;
        if (_spriteRenderer != null)
            _spriteRenderer.color = _baseColor;
    }

    public void SetFilled()
    {
        ValidateComponents();
        gameObject.SetActive(true);
        _spriteRenderer.sprite = normal;
        _spriteRenderer.color = _baseColor;
    }
    public void SetHighlight(Color color)
    {
        ValidateComponents();
        gameObject.SetActive(true);
        _spriteRenderer.sprite = normal;
        _spriteRenderer.color = Color.Lerp(color, Color.white, 0.5f);
        
    }

    public void SetHover()
    {
        ValidateComponents();
        gameObject.SetActive(true);
        _spriteRenderer.sprite = normal;
        var hoverColor = _baseColor;
        
        hoverColor.a = 0.5f;
        _spriteRenderer.color = hoverColor;
    }
    
}
