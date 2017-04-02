using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Gitter.j113203
{
    public class Gitter
    {
        private string token = "";
        private string roomID = "";
        private string botName = "";

        public Gitter(string token, string roomID, string botName)
        {
            this.token = token;
            this.roomID = roomID;
            this.botName = botName;
            lastMessageId = getLastMessage().First().id;
        }

        public void doNext(Action<dynamic> callback)
        {
            List<dynamic> msg = getLastMessage(lastMessageId);
            if (msg.Count == 1)
            {
                lastMessageId = msg.First().id;
                callback(msg[0]);
            }
            Thread.Sleep(5000);
            doNext(callback);
        }

        private string lastMessageId;

        public static string doGet(string url)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Encoding = Encoding.UTF8;
                    return wc.DownloadString(url);
                }
                catch (WebException ex)
                {
                    Debug.WriteLine(ex);
                    return "";
                }
            }
        }

        public static string doPost(string url, string token, string data)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    wc.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                    wc.Headers[HttpRequestHeader.Accept] = "application/json";
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    wc.Encoding = Encoding.UTF8;
                    return wc.UploadString(url, data);
                }
                catch (WebException ex)
                {
                    Debug.WriteLine(ex);
                    return "";
                }
            }
        }

        private List<dynamic> getLastMessage(string afterId = "")
        {
            string url = "https://api.gitter.im/v1/rooms/" + roomID + "/chatMessages?access_token=" + token + "&limit=1";
            if (afterId.Length > 0)
            {
                url += "&afterId=" + afterId;
            }
            string json = doGet(url);
            return JsonConvert.DeserializeObject<List<dynamic>>(json);
        }

        public void sendMessage(string t)
        {
            doPost("https://api.gitter.im/v1/rooms/" + roomID + "/chatMessages", token, JsonConvert.SerializeObject(new { text = t }));
        }
    }
}