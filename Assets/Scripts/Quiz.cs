using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Quiz : MonoBehaviour {
    List<QuestionSO> questions;

    [Header("Question")]
    [SerializeField]
    TextMeshProUGUI questionText;

    [Header("Answers")]
    [SerializeField]
    Button[] answerButtons;

    [SerializeField]
    int correctAnswerIndex = -1;

    [Header("Button Sprites")]

    [SerializeField]
    Sprite defaultAnswerSprite;

    [SerializeField]
    Sprite correctAnswerSprite;

    [Header("Progress Bar")]
    [SerializeField]
    Slider progressBar;


    Timer timer;

    [Header("Score")]
    [SerializeField]
    Score score;

    int currentQuestionIndex = 0;
    async void Start() {
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        if (timer == null) {
            Debug.LogError("Timer not found");
        }
        score = GameObject.Find("Score").GetComponent<Score>();
        if (score == null) {
            Debug.LogError("Score not found");
        }
        questions = await QuestionSOGenerator.GenerateQuestions(10, Category.VideoGames, Difficulty.Easy);
        correctAnswerIndex = questions[currentQuestionIndex].correctAnswerIndex;
        questionText = GameObject.Find("QuestionText").GetComponent<TextMeshProUGUI>();
        answerButtons = GameObject.Find("AnswerButtonGroup").GetComponentsInChildren<Button>();
        defaultAnswerSprite = Resources.Load<Sprite>("Sprites/neon_square_blue");
        correctAnswerSprite = Resources.Load<Sprite>("Sprites/neon_square_orange");
        progressBar = GameObject.Find("ProgressSlider").GetComponent<Slider>();
        progressBar.maxValue = questions.Count;
        incrementProgressBar();
        DisplayQuestion();
    }

    void Update() {
        if (timer.state == TimerState.AnsweredAndWaiting && timer.timerValue <= 0) {
            NextQuestion();
        }
        else if (timer.state == TimerState.Answered && timer.timerValue <= 0) {
            Answer(-1);
        }
    }

    void ToggleAnswerButtons(bool interactable) {
        foreach (Button button in answerButtons) {
            button.interactable = interactable;
        }
    }

    public void AnswerButtonOnClick(int index) {
        Answer(index);
    }

    private void Answer(int chosenAnswerIndex) {
        ToggleAnswerButtons(false);
        QuestionSO question = questions[currentQuestionIndex];

        if (chosenAnswerIndex == question.correctAnswerIndex) {
            answerButtons[chosenAnswerIndex].GetComponent<Image>().sprite = correctAnswerSprite;
            questionText.text = "Correct!";
            score.IncrementScore(true);
        }
        else {
            answerButtons[question.correctAnswerIndex].GetComponent<Image>().sprite = correctAnswerSprite;
            questionText.text = $"Sorry, the correct answer is {question.answers[question.correctAnswerIndex]}";
            score.IncrementScore(false);
        }
        timer.startTimerAnsweredAndWaiting();
    }

    void NextQuestion() {
        currentQuestionIndex++;
        correctAnswerIndex = questions[currentQuestionIndex].correctAnswerIndex;
        if (currentQuestionIndex >= questions.Count) {
            // for now, just reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        incrementProgressBar();
        ToggleAnswerButtons(true);
        DisplayQuestion();
        timer.startTimerAnswering();
    }

    void incrementProgressBar() {
        progressBar.value = currentQuestionIndex + 1;
    }


    void DisplayQuestion() {
        QuestionSO question = questions[currentQuestionIndex];

        questionText.text = question.question;
        for (int i = 0; i < question.answers.Length; i++) {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.answers[i];
            // reset the sprite
            answerButtons[i].GetComponent<Image>().sprite = defaultAnswerSprite;
        }

    }
}
