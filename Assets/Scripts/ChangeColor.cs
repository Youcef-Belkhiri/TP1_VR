using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private Color _selectionColor = Color.green;
    [SerializeField] private Color _hoverColor = Color.blue;

    private Color _initialColor;
    private Renderer _renderer;
    private bool _isSelected = false;


    public bool IsSelected => _isSelected;

	
	private void Start() 
	{  
		_renderer = GetComponent<Renderer>();
        _initialColor = _renderer.materials[0].color;
    }


    public void OnSelect(bool selected)
	{
        _isSelected = selected;

        if (selected)
		{
            _renderer.materials[0].color = _selectionColor;
        }
		else
		{
            _renderer.materials[0].color = _initialColor;
        }
    }


    public void OnHover(bool hoveredOver)
    {
        if (hoveredOver)
        {
            _renderer.materials[0].color = _hoverColor;
        }
        else if (!_isSelected)
        {
            _renderer.materials[0].color = _initialColor;
        }
    }
}
