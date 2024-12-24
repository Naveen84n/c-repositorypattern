using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lkq.Threading;

namespace TestRulesRepo.Server
{
    [ExcludeFromCodeCoverage]

    public static class HttpClientExtensions
    {
        /// <summary>
        /// Gets a token
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static async Task<AuthResponse> GetTokenAsync(this HttpClient client, string uri, string clientId, string secret)
        {
            var kvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("scope", "testread"),
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            };
            var header =
                $"basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"))}";
            var headers = new Dictionary<string, string>
            {
                { "Authorization", header },
                { "accept", "application/json" }
            };
            var result = await PostAsync(client, uri, "", headers, kvp);
            if ((int) result.StatusCode == 200)
            {
                return result.ReadResultAs<AuthResponseSuccess>();
            }
            var subResult = result.ReadResultAs<AuthResponseFailed>();

            if (!string.IsNullOrEmpty(subResult.ErrorDescription) ||
                subResult.Error != AuthError.InvalidClient.ToString())
            {
                return subResult;
            }

            // grab the error from the header
            subResult.Error = AuthError.AuthenticationFailed.ToString();
            subResult.ErrorDescription =
                string.Join(" ", result.Headers.First(t => t.Key == "WWW-Authenticate").Value);
            return subResult;
        }

        /// <summary>
        /// Gets a token
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="clientId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static AuthResponse GetToken(this HttpClient client, string uri, string clientId, string secret)
        {
            return AsyncHelpers.RunSync(() => GetTokenAsync(client, uri, clientId, secret));
        }

        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string uri, string token, Dictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            if (headers != null && headers.Count > 0)
            {
                headers.Select(t =>
                    {
                        var (key, value) = t;
                        request.Headers.Add(key, value);
                        return false;
                    }
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed  using as a for-each
                ).ToList();
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer",token);

            }
            var result = await client.SendAsync(request);
            return result;
        }

        public static HttpResponseMessage Get(this HttpClient client, string uri, string token, Dictionary<string, string> headers)
        {
            return AsyncHelpers.RunSync(() => GetAsync(client, uri, token, headers));
        }

        async public static Task<HttpResponseMessage> PostAsync(this HttpClient client, string uri, string token,
            Dictionary<string, string> headers, object body)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            if (headers != null && headers.Count > 0)
            {
                headers.Select(t =>
                    {
                        var (key, value) = t;
                        request.Headers.Add(key, value);
                        return false;
                    }
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed  using as a for-each
                ).ToList();
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

            }

            var isXml = ContentIsXml(headers, body);
            if (body != null && body.GetType() == typeof(List<KeyValuePair<string, string>>))
            {
                // we need to do form post
                request.Content = new FormUrlEncodedContent((List<KeyValuePair<string,string>>)body);
            }
            else
            {
                StringContent content = null;
                await using (var bodyStream = new MemoryStream())
                {
                    if (isXml && body != null)
                    {
                        if (body.GetType() != typeof(string))
                        {
                            XmlSerializer x = new XmlSerializer(body.GetType());
                            x.Serialize(bodyStream, body);
                            bodyStream.Seek(0, SeekOrigin.Begin); // move to the start
                            content = new StringContent(Encoding.UTF8.GetString(bodyStream.GetBuffer()),
                                Encoding.UTF8, "text/xml");
                        }
                        else
                        {
                            content = new StringContent(body as string ?? string.Empty,
                                Encoding.UTF8, "text/xml");
                        }
                    }
                }

                if (!isXml)
                {
                    var jsonResult = System.Text.Json.JsonSerializer.Serialize(body);
                    content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                }

                request.Content = content;
            }

            var result = await client.SendAsync(request);
            return result;
        }

        public static HttpResponseMessage Post(this HttpClient client, string uri, string token, Dictionary<string, string> headers, object body)
        {
            return AsyncHelpers.RunSync(() => PostAsync(client, uri, token, headers, body));
        }

        /// <summary>
        /// Returns true or false if the content type being submitted is in XML format
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static bool ContentIsXml(Dictionary<string, string> headers, object body)
        {
            if (body is string s)
            {
                if (s.StartsWith("<?xml"))
                {
                    return true;
                }
            }

            bool xml;
            if (headers == null || !headers.Any(t =>
                t.Key.Equals("accept", StringComparison.OrdinalIgnoreCase) ||
                t.Key.Equals("content-type", StringComparison.OrdinalIgnoreCase)))
            {
                return false; // default not XML
            }
            var contentType =
                headers.Keys.FirstOrDefault(t => t.Equals("content-type", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(contentType))
            {
                xml = string.Equals(headers[contentType], "text/xml");
            }
            else
            {
                // they only have an accept header
                var accept =
                    headers.Keys.FirstOrDefault(t => t.Equals("accept", StringComparison.OrdinalIgnoreCase));
                // ReSharper disable once AssignNullToNotNullAttribute - we already checked for nulls
                xml = string.Equals(headers[accept], "text/xml");
            }
            return xml;
        }


        /// <summary>
        /// Reads a response into a list of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        async public static Task<IEnumerable<T>> ReadResultAsListAsync<T>(this HttpResponseMessage message)
        {
            if (message.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return new List<T>();
            }

            var msg = await message.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(msg))
            {
                return new List<T>();
            }

            if (message.Content.Headers.ContentType != null && (message.Content.Headers.ContentType.MediaType != null && message.Content.Headers.ContentType != null && (message.Content.Headers.ContentType.MediaType.StartsWith("application/json") || msg.StartsWith("{"))))
            {
                // json
                var results = System.Text.Json.JsonSerializer.Deserialize<List<T>>(msg);
                return results;
            }
            else
            {
                // xml
                var x = new XmlSerializer(typeof(List<T>));
                var results = (IEnumerable<T>)x.Deserialize(new StringReader(msg));
                return results;
            }
        }

        /// <summary>
        /// Reads a response into a list of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReadResultAsList<T>(this HttpResponseMessage message)
        {
            return AsyncHelpers.RunSync(() => ReadResultAsListAsync<T>(message));
        }

        /// <summary>
        /// reads a response into type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        async public static Task<T> ReadResultAsAsync<T>(this HttpResponseMessage message)
        {
            var msg = await message.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(msg))
            {
                return default;
            }

            if (message.Content.Headers.ContentType != null && (message.Content.Headers.ContentType.MediaType != null && (message.Content.Headers.ContentType != null && (message.Content.Headers.ContentType.MediaType.StartsWith("application/json") || msg.StartsWith("{")))))
            {
                // json
                //var results = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(msg);
                var results = Lkq.Json.Serializer.Deserialize<T>(msg, true);
                return results;
            }
            else
            {
                // xml
                var x = new XmlSerializer(typeof(T));
                var results = (T)x.Deserialize(new StringReader(msg));
                return results;
            }
        }

        /// <summary>
        /// reads a response into type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T ReadResultAs<T>(this HttpResponseMessage message)
        {
            return AsyncHelpers.RunSync(() => ReadResultAsAsync<T>(message));
        }
    }
}
