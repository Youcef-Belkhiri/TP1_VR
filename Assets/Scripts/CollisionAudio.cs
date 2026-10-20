using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    [SerializeField] private AudioSource _collisionAudio;
    [SerializeField] private ChangeColor _changeColor;


    private void OnCollisionEnter(Collision collision)
    {
        if (_changeColor.IsSelected) return;

        _collisionAudio.Play();
    }
}
