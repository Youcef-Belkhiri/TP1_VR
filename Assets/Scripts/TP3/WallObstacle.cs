using System.Collections;
using UnityEngine;

public class WallObstacle : MonoBehaviour
{
    private Renderer _renderer;
    private Color _color;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _color = _renderer.material.color;
    }

    public void OnBallFailed()
    {
        StartCoroutine(ChangeWallColor());
    }
    
    private IEnumerator ChangeWallColor()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1)
        {
            _renderer.material.color = Color.Lerp(_color, Color.red, Mathf.PingPong(elapsedTime * 4, 1f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _renderer.material.color = _color;
    }
}
