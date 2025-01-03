using UnityEngine;

public class OpenTriviaAPITest : MonoBehaviour {
    [SerializeField]

    private async void Start() {
        Debug.Log("Starting to fetch and generate questions...");

        QuestionSO[] questions = await QuestionSOGenerator.GenerateQuestions(
            numberOfQuestions: 10,
            category: Category.VideoGames,
            difficulty: Difficulty.Medium
        );

        if (questions.Length == 0) {
            Debug.LogError("Failed to generate questions!");
            return;
        }

        Debug.Log($"Successfully generated {questions.Length} QuestionSOs!");

        // Print out the first question as an example
        if (questions.Length > 0) {
            QuestionSO q = questions[0];
            Debug.Log($"\nExample Question:");
            Debug.Log($"Question: {q.question}");
            Debug.Log($"Correct Answer index: {q.correctAnswerIndex}");
            Debug.Log("All Answers (shuffled):");
            foreach (string answer in q.answers) {
                Debug.Log($"- {answer}");
            }
        }
    }
}
