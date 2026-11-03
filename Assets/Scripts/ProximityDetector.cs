using UnityEngine;

public class ProximityDetector : MonoBehaviour
{
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform[] _cubes;
    [SerializeField] private Color _proximityColor = Color.green;
    [SerializeField] private float _scaleChangeCoef = 1.2f;
    [SerializeField] private float _triggerThresholdDistance = 2f;

    private Color[] _originalColors;
    private Vector3[] _originalScales;
    

    private void Start()
    {
        var cubesNum = _cubes.Length;
        
        _originalColors = new Color[cubesNum];
        _originalScales = new Vector3[cubesNum];
        
        for (int i = 0; i < cubesNum; i++)
        {
            _originalColors[i] = _cubes[i].GetComponent<Renderer>().material.color;
            _originalScales[i] = _cubes[i].localScale;
        }
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
        }
        
        _cubes[closestCubeIndex].localScale *= _scaleChangeCoef;
    }
}
