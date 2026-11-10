using System.Collections;
using UnityEngine;
using UnityEngine.XR;


public class HapticFeedbackManager : MonoBehaviour
{
    public static HapticFeedbackManager Instance {get; private set;}       // singleton
    private InputDevice _rightController;
    
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }
    
    
    public void PlayHapticFeedback(float vibrationAmplitude, float vibrationDuration)
    {
        if (!_rightController.isValid)
        {
            _rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            if(!_rightController.isValid) return;
        }
        
        _rightController.SendHapticImpulse(0, vibrationAmplitude, vibrationDuration);
    }
}
