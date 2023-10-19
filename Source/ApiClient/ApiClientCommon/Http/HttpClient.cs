using System;
using System.Threading.Tasks;
using ApiClient.Common.Enums;
using ApiClient.Common.Constants;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
//using ApiClient.Logger;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.IO;

namespace ApiClient.Common.Http
{
    /// <summary>
    /// Generic http client that supports JSON and XML.
    /// </summary>
    /// <typeparam name="TRequest">TRequest</typeparam>
    /// <typeparam name="TResponse">TResponse</typeparam>
    public class HttpClient
    {
        public string PostData(string url, string postData)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.76 Safari/537.36 Safari/537.36";
                request.Method = "POST";
                request.Timeout = 300000;
                request.KeepAlive = true;
                request.Referer = url;
                request.Accept = "*/*";
                request.ContentType = "application/json; charset=UTF-8";
                request.Headers["X-Accept"] = MimeTypes.APPLICATION_JSON;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                //Get response
                string str = "";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    str = reader.ReadToEnd();
                    response.Close();
                    reader.Close();
                }
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> PostDataAsync(string url, string postData)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.76 Safari/537.36 Safari/537.36";
                request.Method = "POST";
                request.Timeout = 300000;
                request.KeepAlive = true;
                request.Referer = url;
                request.Accept = "*/*";
                request.ContentType = "application/json; charset=UTF-8";
                request.Headers["X-Accept"] = MimeTypes.APPLICATION_JSON;
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                //Get response
                string str = "";
                using (var response = await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    str = reader.ReadToEnd();
                    response.Close();
                    reader.Close();
                }
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string GetData(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.76 Safari/537.36";
                request.Method = "GET";
                request.KeepAlive = true;
                request.ContentType = MimeTypes.APPLICATION_FORM_ENCODED;
                request.Timeout = 300000;
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //Get response
                string str = "";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    str = reader.ReadToEnd();
                    response.Close();
                    reader.Close();
                }
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
