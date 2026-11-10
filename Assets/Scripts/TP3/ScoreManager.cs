using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance {get; private set;}       // singleton
    
    [SerializeField] private TextMeshPro _scoreText;
    private int _score = 0;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void IncrementScore()
    {
        _score++;
        _scoreText.SetText($"Score: {_score}");
    }
    
}
