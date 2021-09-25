using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Symptom
{
    public List<Symptom> children = new List<Symptom>();
    public string question;

    public Symptom(string question)
    {
        this.question = question;
    }


    public Symptom Add(string question)
    {
        Symptom child = new Symptom(question);
        children.Add(child);

        return child;
    }
}

public class QuestionGraph : MonoBehaviour
{
    private Symptom rootSymptom = new Symptom("Have you felt ill or unwell?");

    private Symptom currentSymptom;
    private Stack<Symptom> questionStack = new Stack<Symptom>();

    private void Awake()
    {
        var pain = rootSymptom.Add("Were you in pain?");
        pain.Add("Have you had headaches?");
        pain.Add("Did your joints hurt?");

        var fatigue = rootSymptom.Add("Did you feel fatigued?");
        fatigue.Add("Were you short of breath?");
        fatigue.Add("Have you had trouble sleeping?");

        questionStack.Push(rootSymptom);

        Debug.Log("QuestionGraph initialized");
    }

    public string GetNextQuestion()
    {
        if (questionStack.Count <= 0)
            return null;

        currentSymptom = questionStack.Pop();
        return currentSymptom.question;
    }

    public void AnswerQuestion(bool positive)
    {
        if (positive)
            foreach (var symptom in currentSymptom.children)
                questionStack.Push(symptom);
    }
}
