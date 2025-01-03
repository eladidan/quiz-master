using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Scriptable Objects/Quiz Question")]
public class QuestionSO : ScriptableObject {
    [TextArea(2, 6)]
    [SerializeField] public string question { get; } = "Enter new question text here";
    [SerializeField] public string[] answers { get; } = new string[4];

    [SerializeField]
    [Range(0, 3)]
    public int correctAnswerIndex { get; } = 0;
}
