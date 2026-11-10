using System.Collections;
using UnityEngine;

public class TargetExercice3 : MonoBehaviour
{
    private Color _originalColor;
    private AudioSource _audioSource;

    private void Start()
    {
        _originalColor = GetComponent<Renderer>().material.color;
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnShot()
    {
        StartCoroutine(OnShotCoroutine());
    }
    
    private IEnumerator OnShotCoroutine()
    {
        // visual feedback
        GetComponent<Renderer>().material.color = Color.green;
        yield return new WaitForSeconds(1f);
        GetComponent<Renderer>().material.color = _originalColor;
        
        // audio feedback
        _audioSource.Play();
    }
}
