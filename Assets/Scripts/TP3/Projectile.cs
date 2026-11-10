using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _initialPosition;
    private bool _disabled = false;
    private Rigidbody _rb;
    
    private WallObstacle _wallObstacle;
    
    private void Start()
    {
        _initialPosition = transform.position;
        _rb = GetComponent<Rigidbody>();
        
        _wallObstacle = FindAnyObjectByType<WallObstacle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_disabled) return;
        
        if (other.CompareTag("Obstacle"))
        {
            OnFail();
        }
        else if (other.TryGetComponent(out TargetExercice3 target))
        {
            OnSuccess(target);
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (_disabled) return;
        
        if (other.gameObject.CompareTag("Obstacle"))
        {
            OnFail();
        }
    }

    private void OnFail()
    {
        _disabled = true;
        Invoke(nameof(ResetProjectile), 1f);
        
        // negative haptic feedback
        HapticFeedbackManager.Instance.PlayHapticFeedback(1, 1);
        _wallObstacle?.OnBallFailed();
    }
    
    private void OnSuccess(TargetExercice3 target)
    {
        _disabled = true;
        Invoke(nameof(ResetProjectile), 0.7f);
            
        // positive feedbacks
        target.OnShot();
        ScoreManager.Instance.IncrementScore();
    }
    

    private void ResetProjectile()
    {
        transform.position = _initialPosition;
        _disabled = false;
        
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
