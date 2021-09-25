using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;

public class BackendComm : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private async void sendPost()
    {
        var values = new Dictionary<string, string>
            {
                { "thing1", "hello" },
                { "thing2", "world" }
            };

        var content = new FormUrlEncodedContent(values);

        var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

        var responseString = await response.Content.ReadAsStringAsync();
    }

    private async void getGet()
    {
        var responseString = await client.GetStringAsync("http://www.example.com/recepticle.aspx");
    }
}
