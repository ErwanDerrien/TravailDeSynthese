using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Numitor.SDK.DAO;
using Numitor.SDK.Model.AuthorModel;

using UnityEngine;
using UnityEngine.Networking;

namespace Numitor.SDK.DAO.LoginDao
{
    public class LoginHttpDao : HttpDao<Author>
    {
        public LoginHttpDao(string baseUrl) : base(baseUrl) { }

        public override async Task<string> Get(Author author)
        {
            string url = baseUrl;

            try
            {
                string payload = JsonUtility.ToJson(author);

                UTF8Encoding encoder = new UTF8Encoding();
                byte[] encodedPayload = encoder.GetBytes(payload);

                UploadHandler uploader = new UploadHandlerRaw(encodedPayload);
                uploader.contentType = "application/json";

                UnityWebRequest request = new UnityWebRequest(url, "GET");
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
                else if (request.responseCode == 200) // OK
                {
                    var jsonPayload = request.downloadHandler.text;

                    return jsonPayload;
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"A failed with message: {ex.Message}");
            }

            return default;
        }
    }
}