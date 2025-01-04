using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    [SerializeField] public float timeToCompleteQuestion { get; } = 30f;
    [SerializeField] public float timeToShowCorrectAnswer { get; } = 5f;

    [SerializeField] Image timerImage;

    [SerializeField] public TimerState state { get; private set; }

    [SerializeField] public float timerValue { get; private set; }

    void Start() {
        state = TimerState.Answering;
        timerValue = timeToCompleteQuestion;
        timerImage = GameObject.Find("TimerImage").GetComponent<Image>();
        timerImage.fillAmount = 1;
    }

    void Update() {
        UpdateTimer();
        float timerDenominator = state == TimerState.Answering ? timeToCompleteQuestion : timeToShowCorrectAnswer;
        timerImage.fillAmount = timerValue / timerDenominator;
    }

    void UpdateTimer() {
        timerValue -= Time.deltaTime;

        if (timerValue <= 0 && state != TimerState.AnsweredAndWaiting) {
            state = state == TimerState.Answering ? TimerState.Answered : TimerState.AnsweredAndWaiting;
        }
    }

    public void startTimerAnswering() {
        state = TimerState.Answering;
        timerValue = timeToCompleteQuestion;
    }

    public void startTimerAnsweredAndWaiting() {
        state = TimerState.AnsweredAndWaiting;
        timerValue = timeToShowCorrectAnswer;
    }
}

public enum TimerState {
    Answering,
    Answered,
    AnsweredAndWaiting
}
