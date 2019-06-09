using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using RSG;

public class RequestEngine : MonoBehaviour
{
    public Text URLText;
    public Text IndicatorText;
    public Text SuccessText;
    public Text MessageText;
    public InputField NameInput;
    public bool IsRunning = false;
    public static RequestEngine Current;
    public void Awake()
    {
        Current = this;
    }

    [Serializable]
    public class ParameterizedRequest
    {
        public string name;
    }

    [Serializable]
    public class WebServiceResponse
    {
        public bool success;
        public string message;
    }

    void StartRequest(string url)
    {
        URLText.text = url;
        IndicatorText.text = "Running...";
        IsRunning = true;
    }

    void ProcessRequest(IPromise<WebServiceResponse> req)
    {
        req
            .Then(resp => {
                IndicatorText.text = "Completed (Success)";
                IsRunning = false;
                SuccessText.text = resp.success.ToString();
                MessageText.text = resp.message;
            })
            .Catch(err => {
                IndicatorText.text = "Completed (Error)";
                IsRunning = false;
                SuccessText.text = "false";
                MessageText.text = err.Message;
            });
    }

    public void DoBasicRequest()
    {
        var url = "http://localhost:3000/api/basic";
        StartRequest(url);
        ProcessRequest(RestClient.Get<WebServiceResponse>(url));
    }

    public void DoParameterizedRequest()
    {
        var url = "http://localhost:3000/api/with-params";
        var parameters = new ParameterizedRequest() { name = NameInput.text };
        StartRequest(url);
        ProcessRequest(RestClient.Post<WebServiceResponse>(url, parameters));
    }

    public void DoFailedRequest()
    {
        var url = "http://localhost:3000/api/failed";
        StartRequest(url);
        ProcessRequest(RestClient.Get<WebServiceResponse>(url));
    }

    public void DoNotFoundRequest()
    {
        var url = "http://localhost:3000/api/notfound";
        StartRequest(url);
        ProcessRequest(RestClient.Get<WebServiceResponse>(url));
    }
}
