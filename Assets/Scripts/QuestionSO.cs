using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Scriptable Objects/Quiz Question")]
public class QuestionSO : ScriptableObject {
    [TextArea(2, 6)]
    [SerializeField] public string question { get; private set; } = "Enter new question text here";
    [SerializeField] public string[] answers { get; private set; } = new string[4];

    [SerializeField]
    [Range(0, 3)]
    public int correctAnswerIndex { get; private set; } = 0;

    public void Initialize(string question, string[] answers, int correctAnswerIndex) {
        this.question = question;
        this.answers = answers;
        this.correctAnswerIndex = correctAnswerIndex;
    }
}
