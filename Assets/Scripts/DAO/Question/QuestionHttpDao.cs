using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Numitor.SDK.DAO;
using Numitor.SDK.Model.QuestionModel;

using UnityEngine;
using UnityEngine.Networking;

namespace Numitor.SDK.DAO.QuestionDao
{
    public class QuestionHttpDao : HttpDao<Question>
    {
        public QuestionHttpDao(string baseUrl) : base(baseUrl) { }

        public override async Task<Question> Get()
        {
            string url = baseUrl + "/next";

            try
            {
                using var request = UnityWebRequest.Get(url);

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed: {request.error}");
                }
                else if (request.responseCode == 200)
                {
                    var jsonPayload = request.downloadHandler.text;

                    return JsonUtility.FromJson<Question>(jsonPayload);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{nameof(Get)} failed with message: {ex.Message}");
            }

            return default;
        }

        public override async Task<string> Create(Question entity)
        {
            string url = composeUrl(baseUrl, "post");

            try
            {
                var payload = JsonUtility.ToJson(entity);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] encodedPayload = encoder.GetBytes(payload);

                UploadHandler uploader = new UploadHandlerRaw(encodedPayload);
                uploader.contentType = "application/json";

                UnityWebRequest request = new UnityWebRequest(url, "POST");
                request.uploadHandler = uploader;
                request.downloadHandler = new DownloadHandlerBuffer();

                var operation = request.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed: {request.error}");
                }
                else if (request.responseCode == 201)
                {
                    return request.downloadHandler.text;
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed with message: {ex.Message}");
            }

            return default;
        }
    }
}