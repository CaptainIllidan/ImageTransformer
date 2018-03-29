using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace ImageTransformer.Data
{
    public class UrlParser
    {
        public Rectangle ParseParams(string path)
        {
            var parameters = CorrectParameters(path.Substring(path.IndexOf('/') + 1).Split(',').Select(x => Convert.ToInt32(x)).ToArray());
            Rectangle cropArea = new Rectangle(Convert.ToInt32(parameters[0]), Convert.ToInt32(parameters[1]), Convert.ToInt32(parameters[2]), Convert.ToInt32(parameters[3]));
            return cropArea;
        }

        public bool IsCorrectRequest(HttpListenerContext listenerContext)
        {
            {
                return (correctFormatRequest.Match(listenerContext.Request.Url.AbsolutePath).Success &&
                    listenerContext.Request.ContentLength64 <= MaxContentSize);
            }
        }

        public string GetRequestPath(HttpListenerContext listenerContext)
        {
            return listenerContext.Request.Url.AbsolutePath.Substring(9);
        }

        private int[] CorrectParameters(int[] parameters)
        {
            for (int i = 2; i < 4; i++)
            {
                if (parameters[i] < 0)
                {
                    parameters[i - 2] += parameters[i];
                    parameters[i] *= -1;
                }
            }
            return parameters;
        }

        private Regex correctFormatRequest = new Regex(@"\A/process/(rotate-cw|rotate-ccw|flip-h|flip-v)/-?\d{1,10},-?\d{1,10},-?\d{1,10},-?\d{1,10}\z", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private const int MaxContentSize = 100 * 1024 * 1024;
    }
}
