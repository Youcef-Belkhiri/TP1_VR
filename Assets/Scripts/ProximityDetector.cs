using System;
using TMPro;
using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform[] _cubes;
    [SerializeField] private Color _proximityColor = Color.green;
    [SerializeField] private float _scaleChangeCoef = 1.2f;
    [SerializeField] private float _triggerThresholdDistance = 2f;

    [SerializeField] private float _angleThreshold = 20f;

    private Color[] _originalColors;
    private Vector3[] _originalScales;

    private LineRenderer[] _lineRenderers;
    private TextMeshPro _textMesh;
    
    private void Start()
    {
        var cubesNum = _cubes.Length;
        
        _originalColors = new Color[cubesNum];
        _originalScales = new Vector3[cubesNum];
        _lineRenderers = new LineRenderer[cubesNum];
        
        for (int i = 0; i < cubesNum; i++)
        {
            _originalColors[i] = _cubes[i].GetComponent<Renderer>().material.color;
            _originalScales[i] = _cubes[i].localScale;

            var lineRenderer = _cubes[i].gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            _lineRenderers[i] = lineRenderer;
        }

        _textMesh = new GameObject("3D Text").AddComponent<TextMeshPro>();
        _textMesh.transform.SetParent(_origin);
        _textMesh.fontSize = 2;
        _textMesh.alignment = TextAlignmentOptions.Center;
        _textMesh.transform.localPosition = new Vector3(0, 0, 0.5f);
    }
    
    private void Update()
    {
        float closestDistance = Mathf.Infinity;
        int closestCubeIndex = -1;

        for (var i = 0; i < _cubes.Length; i++)
        {
            var distance = Vector3.Distance(_origin.position, _cubes[i].position);
            
            var cubeMaterial = _cubes[i].GetComponent<Renderer>().material;
            
            cubeMaterial.color = distance < _triggerThresholdDistance ? _proximityColor : _originalColors[i];

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCubeIndex = i;
            }
            
            _cubes[i].localScale = _originalScales[i];
            _lineRenderers[i].enabled = false;
        }
        
        var direction = _cubes[closestCubeIndex].position - _origin.position;
        var angle = Vector3.Angle(direction, _origin.forward);
        
        if (angle < _angleThreshold)
        {
            _cubes[closestCubeIndex].localScale *= _scaleChangeCoef;
            
            _lineRenderers[closestCubeIndex].enabled = true;
            _lineRenderers[closestCubeIndex].SetPosition(0, _origin.position);
            _lineRenderers[closestCubeIndex].SetPosition(1, _cubes[closestCubeIndex].position);
            
            _textMesh.gameObject.SetActive(true);
            _textMesh.SetText($"{Math.Round(closestDistance, 2)}m - {Math.Round(angle)}º");
        }
        else
        {
            _textMesh.gameObject.SetActive(false);
        }
    }
}
