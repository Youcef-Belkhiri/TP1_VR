using UnityEngine;
using UnityEngine.XR;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private Transform _controllerTransform;
    [SerializeField] private float _toleranceAimAngle = 20f;
    [SerializeField] private ParticleSystem _shootParticlesPrefab;
    [SerializeField] private AudioClip _shotAudio;

    private InputDevice _rightController;
    private GameObject[] _targets;

    private bool _triggerWasPressed = false;
    

    private void Start()
    {
        _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        _targets = GameObject.FindGameObjectsWithTag("Target");
    }

    private void Update()
    {
        if (!_rightController.isValid)
        {
            _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if(!_rightController.isValid) return;
        }
        
        // Détecter l’appui sur la gâchette
        if (_rightController.TryGetFeatureValue(CommonUsages.triggerButton, out var triggerPressed))
        {
            // comparer avec l’état antérieur de triggerPressed pour savoir si l’appui est nouveau
            if (triggerPressed && !_triggerWasPressed)
            {
                _triggerWasPressed = true;
                
                foreach (var target in _targets)
                {
                    var direction = target.transform.position - _controllerTransform.position;
                    var angle = Vector3.Angle(direction, _controllerTransform.forward);
                    if (angle < _toleranceAimAngle)
                    {
                        var particles = Instantiate(_shootParticlesPrefab, target.transform.position, Quaternion.identity);
                        Destroy(particles, 2f);
                        AudioSource audioSource = target.GetComponent<AudioSource>();
                        audioSource.PlayOneShot(_shotAudio);
                    }
                }
            }
            else if (!triggerPressed)
            {
                _triggerWasPressed = false;
            }
        }
    }
}
