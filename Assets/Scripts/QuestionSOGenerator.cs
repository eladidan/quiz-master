using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public static class QuestionSOGenerator {
    public static async Task<List<QuestionSO>> GenerateQuestions(int numberOfQuestions, Category category, Difficulty difficulty) {
        var api = new OpenTriviaAPI();
        await api.StartSession();

        // Fetch multiple choice questions only
        var questions = await api.FetchQuestions(numberOfQuestions, category, difficulty, QuestionType.Multiple);

        return questions.Select(q => {
            // Create new ScriptableObject instance and initialize with constructor
            var allAnswers = new string[4];
            q.incorrect_answers.CopyTo(allAnswers, 0);
            allAnswers[3] = q.correct_answer;

            // Shuffle answers before creating the SO
            var shuffledAnswers = allAnswers.OrderBy(x => Random.value).ToArray();
            var correctIndex = System.Array.IndexOf(shuffledAnswers, q.correct_answer);

            var questionSO = ScriptableObject.CreateInstance<QuestionSO>();
            questionSO.Initialize(q.question, shuffledAnswers, correctIndex);

            return questionSO;
        }).ToList();
    }
}
