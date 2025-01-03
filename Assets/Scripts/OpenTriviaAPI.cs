using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Text;
using System.Web;

public class OpenTriviaAPI {
    private const string BASE_URL = "https://opentdb.com/api.php";
    private const string SESSION_URL = "https://opentdb.com/api_token.php";
    private string sessionToken;

    public async Task<bool> StartSession() {
        string url = $"{SESSION_URL}?command=request";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) {
                SessionResponse response = JsonUtility.FromJson<SessionResponse>(request.downloadHandler.text);
                sessionToken = response.token;
                return true;
            }
            return false;
        }
    }

    public async Task<Question[]> FetchQuestions(int numberOfQuestions, Category category,
        Difficulty difficulty, QuestionType type) {
        StringBuilder urlBuilder = new StringBuilder($"{BASE_URL}?amount={numberOfQuestions}");

        if (category != Category.Any)
            urlBuilder.Append($"&category={((int)category)}");
        if (difficulty != Difficulty.Any)
            urlBuilder.Append($"&difficulty={difficulty.ToString().ToLower()}");
        if (type != QuestionType.Any)
            urlBuilder.Append($"&type={type.ToString().ToLower()}");
        if (!string.IsNullOrEmpty(sessionToken))
            urlBuilder.Append($"&token={sessionToken}");

        using (UnityWebRequest request = UnityWebRequest.Get(urlBuilder.ToString())) {
            await request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) {
                QuestionsResponse response = JsonUtility.FromJson<QuestionsResponse>(request.downloadHandler.text);
                // URL decode all questions and answers
                foreach (Question q in response.results) {
                    q.question = HttpUtility.HtmlDecode(q.question);
                    q.correct_answer = HttpUtility.HtmlDecode(q.correct_answer);
                    for (int i = 0; i < q.incorrect_answers.Length; i++) {
                        q.incorrect_answers[i] = HttpUtility.HtmlDecode(q.incorrect_answers[i]);
                    }
                }
                return response.results;
            }
            return new Question[0];
        }
    }
}

[Serializable]
public class Question {
    public string category;
    public string type;
    public string difficulty;
    public string question;
    public string correct_answer;
    public string[] incorrect_answers;
}

[Serializable]
public class SessionResponse {
    public int response_code;
    public string token;
}

[Serializable]
public class QuestionsResponse {
    public int response_code;
    public Question[] results;
}

public enum Category {
    Any = 0,
    GeneralKnowledge = 9,
    Books = 10,
    Film = 11,
    Music = 12,
    Television = 14,
    VideoGames = 15,
    Science = 17,
    Computers = 18,
    Mathematics = 19,
    Sports = 21,
    Geography = 22,
    History = 23,
    Politics = 24,
    Art = 25,
    Animals = 27,
    Vehicles = 28,
    Comics = 29,
    Gadgets = 30,
    Anime = 31,
    Cartoons = 32
}

public enum Difficulty {
    Any,
    Easy,
    Medium,
    Hard
}

public enum QuestionType {
    Any,
    Multiple,
    Boolean
}
