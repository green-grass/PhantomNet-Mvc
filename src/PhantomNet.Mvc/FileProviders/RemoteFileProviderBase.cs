using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace PhantomNet.Mvc.FileProviders
{
    public class RemoteFileProviderBase
    {
        private const int DefaultTokenTimeOut = 5000;

        private readonly HttpContext _context;

        public RemoteFileProviderBase(
            IApiTokenProvider tokenProvider,
            IHttpContextAccessor contextAccessor,
            IOptions<RemoteFileProviderOptions> optionsAccessor)
        {
            TokenProvider = tokenProvider;
            SecretKey = optionsAccessor.Value.SecretKey ?? string.Empty;
            TokenTimeOut = optionsAccessor.Value.TokenTimeOut ?? DefaultTokenTimeOut;
            EndPoint = optionsAccessor.Value.EndPoint;

            _context = contextAccessor?.HttpContext;
        }

        protected IApiTokenProvider TokenProvider { get; }

        protected virtual CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        protected string SecretKey { get; }

        protected double TokenTimeOut { get; }

        protected string EndPoint { get; }

        protected virtual async Task<dynamic> InternalFileList(string key, string actionName)
        {
            string timeStamp, token;
            TokenProvider.GenerateToken(SecretKey, actionName, TokenTimeOut, out timeStamp, out token);
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage() {
                    RequestUri = new Uri(Path.Combine(EndPoint, actionName, $"{key}")),
                    Method = HttpMethod.Get
                };

                request.Headers.Add("timeStamp", timeStamp);
                request.Headers.Add("token", token);

                try
                {
                    using (var respond = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken))
                    {
                        var respondData = await respond.Content.ReadAsStringAsync();
                        var model = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<dynamic>(respondData));
                        return model;
                    }
                }
                catch
                {
                    // TOTO:: Log error
                    return null;
                }
            }
        }

        protected virtual async Task<dynamic> InternalUploadFile(string key, string actionName, IFormFileCollection files)
        {
            using (var content = new MultipartFormDataContent())
            {
                foreach (var file in files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    content.Add(fileContent, file.Name, file.FileName);
                }

                return await PostContent(key, actionName, content);
            }
        }

        protected virtual async Task<dynamic> InternalRenameFile(string key, string actionName, string fileName, string newName)
        {
            var values = new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(nameof(fileName), fileName),
                    new KeyValuePair<string, string>(nameof(newName), newName)
                };

            using (var content = new FormUrlEncodedContent(values))
            {
                return await PostContent(key, actionName, content);
            }
        }

        protected virtual async Task<dynamic> InternalDeleteFile(string key, string actionName, string fileName)
        {
            var values = new KeyValuePair<string, string>[] {
                    new KeyValuePair<string, string>(nameof(fileName), fileName),
                };

            using (var content = new FormUrlEncodedContent(values))
            {
                return await PostContent(key, actionName, content);
            }
        }

        protected virtual async Task<dynamic> PostContent(string key, string actionName, HttpContent content)
        {
            string timeStamp, token;
            TokenProvider.GenerateToken(SecretKey, actionName, TokenTimeOut, out timeStamp, out token);
            using (var client = new HttpClient())
            {
                content.Headers.Add("timeStamp", timeStamp);
                content.Headers.Add("token", token);

                var requestUri = Path.Combine(EndPoint, actionName, $"{key}");
                try
                {
                    using (var respond = await client.PostAsync(requestUri, content, CancellationToken))
                    {
                        var respondData = await respond.Content.ReadAsStringAsync();
                        var model = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<dynamic>(respondData));
                        return model;
                    }
                }
                catch
                {
                    // TOTO:: Log error
                    return null;
                }
            }
        }
    }
}
