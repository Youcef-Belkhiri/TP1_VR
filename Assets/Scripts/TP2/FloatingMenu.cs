using TMPro;
using UnityEngine;

public class FloatingMenu : MonoBehaviour
{
    private TextMeshPro _text;
    private Camera _camera;

    private void Start()
    {
        _text = GetComponentInChildren<TextMeshPro>();
        _camera = Camera.main;
    }

    private void Update()
    {
        _text.SetText($"Local: {transform.localPosition} \nGlobal: {transform.position}");
        
        var directionToCam = transform.position - _camera.transform.position;
        transform.rotation = Quaternion.LookRotation(directionToCam);
    }
}
