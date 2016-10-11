﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QCloud.WeApp.SDK
{
    public partial class TunnelService
    {
        private async Task HandlePost(ITunnelHandler handler, TunnelHandleOptions options)
        {
            string requestBody = null;
            bool checkSignature = false;

            using (Stream stream = Request.InputStream)
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    requestBody = await reader.ReadToEndAsync();
                }
            }
            
            try
            {
                var bodyDefination = new {
                    data = new {
                        tunnelIds = new string[0],
                        type = string.Empty,
                        content = string.Empty
                    },
                    signature = string.Empty
                };

                var body = JsonConvert.DeserializeAnonymousType(requestBody, bodyDefination);
                var data = body.data;
                var signature = body.signature;

                if (checkSignature && !JsonConvert.SerializeObject(data).CheckSignature(signature))
                {
                    LogRequest(requestBody, "Error: Signature Failed");
                    return;
                }


            }
            catch (JsonException)
            {
                LogRequest(requestBody, "Error: Invalid Json");
                return;
            }
            catch (Exception)
            {
                LogRequest(requestBody, "Error: Unknown Data");
            }

        }

        private void LogRequest(string requestBody, string handleResult)
        {
            bool log = true;
            if (log)
            {
                try
                {
                    File.WriteAllText($"C:\\requests\\{DateTime.Today.ToString("yyyyMMdd_HH_mm_ss")}", requestBody + "\n\n" + handleResult);
                }
                catch { }
            }
        }
    }
}
