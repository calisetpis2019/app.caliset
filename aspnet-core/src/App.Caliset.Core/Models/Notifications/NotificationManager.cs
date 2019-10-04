using Abp.Domain.Repositories;
using Abp.Domain.Services;
using App.Caliset.Models.UserDeviceTokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Caliset.Models.Notifications
{
    public class NotificationManager : DomainService, INotificationManager

    {
        private readonly IRepository<UserDeviceToken> _repositoryUserDeviceTokens;
        public NotificationManager(IRepository<UserDeviceToken> repositoryUserDeviceTokens) {
             _repositoryUserDeviceTokens = repositoryUserDeviceTokens;
        }
        public void sendNotification(string Title,string Text, long UserId)
        {


            string deviceToken = _repositoryUserDeviceTokens.FirstOrDefault(x => x.UserId == UserId).DeviceToken;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add("Authorization: Key=AAAAEENtp_Q:APA91bEgvANU3DOnaMuFjJsr04LgEDPU2kE8cwyOS82oBfyJY9ewB8dA5fpGXOhtIpr0ZdoBR0IcylHnJQByo3j46oogpkfJjcfbde44GLQTkHOW1UOcA32o0zRM1DxMtOGMzTQ41s5f");
            httpWebRequest.Method = "POST";

     

            byte[] postBytes = Encoding.UTF8.GetBytes("{\"notification\": { " +
                                                                        "\"title\": " + Title + "," +
                                                                        "\"text\": " + Text + "," +
                                                                        "\"sound\": \"default\"" +
                                                                        "}," +
                                                                        "\"priority\": \"High\"," +
                                                                        "\"to\":" + deviceToken +
                                                                        "}");

            Stream dataStream = httpWebRequest.GetRequestStream();
            dataStream.Write(postBytes, 0, postBytes.Length);
            dataStream.Close();
            
            //using (var stream = httpWebRequest.GetRequestStream())
            //{
            //    stream.Write(byte2, 0, byte2.Length);
            //}

            var response = (HttpWebResponse)httpWebRequest.GetResponse();
            Console.WriteLine();
        }
    }
}
