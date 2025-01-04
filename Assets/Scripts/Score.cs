using UnityEngine;
using TMPro;
public class Score : MonoBehaviour {
    public int score = 0;
    public int maxScore = 0;

    [SerializeField]
    TextMeshProUGUI scoreText;

    void Start() {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        scoreText.text = $"Score: {Mathf.Round(GetScorePercentage())}%";
    }

    public void IncrementScore(bool isCorrect) {
        if (isCorrect) {
            score++;
        }
        maxScore++;
    }

    public float GetScorePercentage() {
        if (maxScore == 0) {
            return 0;
        }
        return (float)score / maxScore * 100;
    }
}
