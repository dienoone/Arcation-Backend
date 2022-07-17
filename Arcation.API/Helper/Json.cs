using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Arcation.API.Helper
{
    public static class Json<T> where T : class
    {
        public static byte[] EncodeSingelJson(T entity)
        {
            string json = JsonConvert.SerializeObject(entity);
            string base64EncodedExternalAccount = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            return Convert.FromBase64String(base64EncodedExternalAccount);
        }

        public static byte[] EncodeListJson(IEnumerable<T> entity)
        {
            string json = JsonConvert.SerializeObject(entity);
            string base64EncodedExternalAccount = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
            return Convert.FromBase64String(base64EncodedExternalAccount);
        }

        public static T DecodeSingelJson(string entity)
        {
            byte[] byteArray = Convert.FromBase64String(entity);
            string jsonBack = Encoding.UTF8.GetString(byteArray);
            return JsonConvert.DeserializeObject<T>(jsonBack);
        }

        public static IEnumerable<T> DecodeListJson(string entity)
        {
            byte[] byteArray = Convert.FromBase64String(entity);
            string jsonBack = Encoding.UTF8.GetString(byteArray);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonBack);
        }

    }
}
