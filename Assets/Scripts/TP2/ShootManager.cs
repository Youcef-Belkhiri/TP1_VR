using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private Transform _controllerTransform;
    [SerializeField] private float _toleranceAimAngle = 20f;
    [SerializeField] private ParticleSystem _shootParticlesPrefab;
    [SerializeField] private AudioClip _shotAudio;

    [SerializeField] private float _vibrationAmplitude = 1;
    [SerializeField] private float _vibrationDuration = 1;
    [SerializeField] private GameObject _redSpherePrefab;
    private GameObject _redSphere;
    
    private InputDevice _rightController;
    private GameObject[] _targets;
    private Color[] _originalColors;

    private bool _triggerWasPressed = false;
    

    private void Start()
    {
        _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        _targets = GameObject.FindGameObjectsWithTag("Target");
        _originalColors = new Color[_targets.Length];
        
        for (int i = 0; i < _targets.Length; i++)
        {
            _originalColors[i] = _targets[i].GetComponent<Renderer>().material.color;
        }

        _redSphere = Instantiate(_redSpherePrefab);
        _redSphere.SetActive(false);
    }

    private void Update()
    {
        if (!_rightController.isValid)
        {
            _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if(!_rightController.isValid) return;
        }
        
        // Raycast
        if (Physics.Raycast(_controllerTransform.position, _controllerTransform.forward, out var hit, 100f))
        {
            _redSphere.transform.position = hit.point;
            _redSphere.gameObject.SetActive(true);
        }
        else
        {
            _redSphere.gameObject.SetActive(false);
        }
        
        // Détecter l’appui sur la gâchette
        if (_rightController.TryGetFeatureValue(CommonUsages.triggerButton, out var triggerPressed))
        {
            // comparer avec l’état antérieur de triggerPressed pour savoir si l’appui est nouveau
            if (triggerPressed && !_triggerWasPressed)
            {
                _triggerWasPressed = true;

                for (var i = 0; i < _targets.Length; i++)
                {
                    var target = _targets[i];
                    var direction = target.transform.position - _controllerTransform.position;
                    var angle = Vector3.Angle(direction, _controllerTransform.forward);
                    if (angle < _toleranceAimAngle)
                    {
                        var particles = Instantiate(_shootParticlesPrefab, target.transform.position, Quaternion.identity);
                        Destroy(particles, 2f);
                        AudioSource audioSource = target.GetComponent<AudioSource>();
                        audioSource.PlayOneShot(_shotAudio);

                        StartCoroutine(ClignotementCoroutine(target, i));
                    }
                }

                // Haptic feedback
                StartCoroutine(HapticFeedbackCoroutine());
            }
            else if (!triggerPressed)
            {
                _triggerWasPressed = false;
            }
        }
    }

    private IEnumerator HapticFeedbackCoroutine()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            _rightController.SendHapticImpulse(0, _vibrationAmplitude, _vibrationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ClignotementCoroutine(GameObject target, int targetIndex)
    {
        var targetRenderer = target.GetComponent<Renderer>();

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            var color = Color.Lerp(_originalColors[targetIndex], Color.red, Mathf.PingPong(elapsedTime * 3f, 1f));
            targetRenderer.material.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
