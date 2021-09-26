using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private GameObject answerButtonsGameObject;
    [SerializeField] private VerticalLayoutGroup answerButtonsGroup;
    [SerializeField] private GameObject answerButtonPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private QuestionGraph questionGraph;
    [SerializeField] private BackendComm backendComm;

    int patientId = 1;
    QuestionWithId currentQuestion;

    private void OnValidate()
    {
        if (questionText == null)
            Debug.LogError("questionText cannot be null");
        if (answerButtonsGameObject == null)
            Debug.LogError("answerButtonsGameObject cannot be null");
        if (answerButtonsGroup == null)
            Debug.LogError("answerButtonsGroup cannot be null");
        if (answerButtonPrefab == null)
            Debug.LogError("answerButtonPrefab cannot be null");
        if (animator == null)
            Debug.LogError("animator cannot be null");
        if (questionGraph == null)
            Debug.LogError("questionGraph cannot be null");
        if (backendComm == null)
            Debug.LogError("backendComm cannot be null");
    }

    private async void Start()
    {
        await backendComm.StartSession(patientId);
        SetNextQuestion();
    }

    private void setQuestion(string question)
    {
        if (question != null)
        {
            Debug.Log($"Setting question: {question}");
            answerButtonsGameObject.SetActive(true);
            questionText.text = question;
            animator.Play("Fade-in");
        }
        else
        {
            setThankYouPanel();
        }
    }

    private void setThankYouPanel()
    {
        answerButtonsGameObject.SetActive(false);
        questionText.text = "Thank you!";
        animator.Play("Fade-in");
    }

    private void cleanAnswers()
    {
        foreach (Transform answerButton in answerButtonsGroup.transform)
        {
            Destroy(answerButton.gameObject);
        }
    }

    private void setAnswers(IEnumerable<string> answers)
    {
        cleanAnswers();

        foreach (var answer in answers)
        {
            var answerButton = Instantiate(answerButtonPrefab);
            answerButton.transform.parent = answerButtonsGroup.transform;
        }
    }

    public async void AnswerPositive()
    {
        Debug.Log("Positive answer");

        // internal simulator
        //questionGraph.AnswerQuestion(true);
        await backendComm.SendAnswer(patientId, currentQuestion, true);

        animator.Play("Fade-out");
        SetNextQuestion();
    }

    public async void AnswerNagative()
    {
        Debug.Log("Negative answer");

        // internal simulator
        //questionGraph.AnswerQuestion(true);
        await backendComm.SendAnswer(patientId, currentQuestion, false);

        animator.Play("Fade-out");
        SetNextQuestion();
    }

    private int testSetNextQuestionCounter = 0;

    public async Task<string> SetNextQuestion()
    {
        Debug.Log("SetNewQuestion");

        // wait for the animation to finish
        await Task.Delay(300);
        //yield return new WaitForSeconds(0.3f);

        //var question = testSetNextQuestionCounter < 3 ? $"Next Question {testSetNextQuestionCounter}" : null;
        //testSetNextQuestionCounter += 1;

        // internal simulator
        //var question = questionGraph.GetNextQuestion();

        currentQuestion = await backendComm.GetQuestion(patientId);

        setQuestion(currentQuestion.text);

        return currentQuestion.text;
    }

    public void TestSetAnswers()
    {
        List<string> testAnswers = new List<string>();
        testAnswers.Add("testAnswer1");
        testAnswers.Add("testAnswer2");
        testAnswers.Add("testAnswer3");
        setAnswers(testAnswers);
    }

    private int testSetQuestionCounter = 0;

    public void TestSetQuestion()
    {
        setQuestion(testSetQuestionCounter < 3 ? $"This is a test question {testSetQuestionCounter}." : null);
        testSetQuestionCounter += 1;
    }

    public void TestNoMoreQuestions()
    {
        setQuestion(null);
    }
}
