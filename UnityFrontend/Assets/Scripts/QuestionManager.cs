using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private VerticalLayoutGroup answerButtonsGroup;
    [SerializeField] private GameObject answerButtonPrefab;

    private void OnValidate()
    {
        if (questionText == null)
            Debug.LogError("questionText cannot be null");
        if (answerButtonsGroup == null)
            Debug.LogError("answerButtonsGroup cannot be null");
        if (answerButtonPrefab == null)
            Debug.LogError("answerButtonPrefab cannot be null");
    }

    private void setQuestion(string question)
    {
        questionText.text = question;
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

    public void TestSetAnswers()
    {
        List<string> testAnswers = new List<string>();
        testAnswers.Add("testAnswer1");
        testAnswers.Add("testAnswer2");
        testAnswers.Add("testAnswer3");
        setAnswers(testAnswers);
    }

    public void TestSetQuestion()
    {
        setQuestion("This is a test question.");
    }
}
