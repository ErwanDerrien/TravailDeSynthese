using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Numitor.SDK.Model;

using UnityEngine;
using UnityEngine.Networking;


namespace Numitor.SDK.DAO
{
    public class HttpDao<T> where T : BaseModel
    {
        protected string baseUrl;
        private string entityName;
        private UTF8Encoding encoder = new UTF8Encoding();

        public HttpDao(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.entityName = typeof(T).Name;
        }

        public string composeUrl(string baseUrl, string httpVerb)
        {
            return composeUrl(baseUrl, null, httpVerb);
        }

        protected string composeUrl(string baseUrl, string id, string httpVerb)
        {
            string candidateUrl = baseUrl;
            if (id != null)
            {
                candidateUrl += "/" + id;
            }
            string extension = "." + httpVerb.ToLower();
            return candidateUrl + extension;
        }

        protected string appendParameters(string url, Hashtable parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return url;
            }

            List<string> combinations = new List<string>();
            foreach (string key in parameters.Keys)
            {
                var value = parameters[key];
                if (value.GetType() == typeof(string[]))
                {
                    foreach (string individual in ((string[])value))
                    {
                        combinations.Add(key + "=" + HttpUtility.UrlEncode((string)individual));
                    }
                }
                else
                {
                    combinations.Add(key + "=" + HttpUtility.UrlEncode((string)value));
                }
            }

            return url + '?' + string.Join("&", combinations);
        }


        public virtual async Task<T> Get(string id)
        {
            string url = composeUrl(baseUrl, "get");

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

                    return JsonUtility.FromJson<T>(jsonPayload);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{nameof(Get)} for {entityName} failed with message: {ex.Message}");
            }

            return default;
        }

        public virtual async Task<string> Create(T entity)
        {
            string url = composeUrl(baseUrl, "post");

            try
            {
                string payload = JsonUtility.ToJson(entity);

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
                    string location = request.GetResponseHeader("Location");

                    return location.Substring(location.LastIndexOf('/') + 1);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{nameof(Get)} for {entityName} failed with message: {ex.Message}");
            }

            return default;
        }

        public virtual Task<string> Update(string id, T partialEntity) {
            throw new NotSupportedException();
        }

        public virtual Task Delete(string id) {
            throw new NotSupportedException();
        }
    }
}