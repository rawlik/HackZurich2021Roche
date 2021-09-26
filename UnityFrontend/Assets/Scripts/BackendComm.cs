using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using System.Net.Http;
//using System.Text.Json;

public class QuestionWithId
{
    public string id;
    public string text;
}


public class BackendComm : MonoBehaviour
{
    const string appurl = "https://hackzurichroche.herokuapp.com/";
    private static readonly HttpClient client = new HttpClient();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task<string> SendAnswer(int userId, QuestionWithId questionWithId, bool positive)
    {
        Debug.Log("Sending answer request");
        // {\"userId\":1,\"questionId\":1,\"answer\":1}"
        //var values = new Dictionary<string, string>
        //    {
        //        { "userId", "hello" },
        //        { "questionId", "world" },
        //        { "answer", positive ? "1" : "0" }
        //    };

        //var content = new FormUrlEncodedContent(values);

        var content = new StringContent($"{{\"userId\":{userId},\"questionId\":{questionWithId.id},\"answer\":{(positive ? 1 : 0)}}}", Encoding.UTF8, "application/json");

        //content.Headers.Add("accept", "*/*");
        //content.Headers.Add("Content-Type", "application/json");

        // curl - X POST "https://hackzurichroche.herokuapp.com/send-answer/" - H  "accept: */*" - H  "Content-Type: application/json" - d "{\"userId\":1,\"questionId\":1,\"answer\":1}"
        var response = await client.PostAsync($"{appurl}send-answer/", content);

        var responseString = await response.Content.ReadAsStringAsync();
        Debug.Log($"   got answer {responseString}");

        return responseString;
    }

    public async Task<string> StartSession(int patientId)
    {
        var uriString = $"{appurl}start/{patientId}";
        Debug.Log($"Sending start Session request: {uriString}");
        var content = new FormUrlEncodedContent(new Dictionary<string, string> { });
        var response = await client.PostAsync(uriString, content);
        var responseString = await response.Content.ReadAsStringAsync();
        //var responseString = await client.GetStringAsync(uriString);
        Debug.Log($"    got response: {responseString}");

        return responseString;
    }

    public async Task<QuestionWithId> GetQuestion(int patientId)
    {
        Debug.Log("Sending start getQuestion request");
        var responseString = await client.GetStringAsync($"{appurl}question/by-patient-id/{patientId}");
        Debug.Log($"    got response: {responseString}");

        var questionWithId = JsonUtility.FromJson<QuestionWithId>(responseString);
        Debug.Log($"    question text is: {questionWithId.text}");
        Debug.Log($"    question id is: {questionWithId.id}");
        //JsonSerializer.Deserialize();

        // {"id":1,"text":"Have you felt ill or unwell?}"


        return questionWithId;
    }
}


// curl - X POST "https://hackzurichroche.herokuapp.com/start/1" - H  "accept: */*" - d ""
// curl - X GET "https://hackzurichroche.herokuapp.com/question/by-patient-id/1" - H  "accept: */*"
