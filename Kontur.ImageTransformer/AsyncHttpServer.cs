using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections.Concurrent;
using ImageTransformer.Data;
using ImageTransformer.Filters;

namespace ImageTransformer
{
    public class AsyncHttpServer : IDisposable
    {
        /// <param name="accepts">
        /// Higher values mean more connections can be maintained yet at a much slower average response time; fewer connections will be rejected.
        /// Lower values mean less connections can be maintained yet at a much faster average response time; more connections will be rejected.
        /// </param>
        public AsyncHttpServer(int accepts = 2)
        {
            listener = new HttpListener();
            this.accepts = accepts * Environment.ProcessorCount;
        }

        public void Start(string prefix)
        {
            lock (listener)
            {
                if (!isRunning)
                {
                    listener.Prefixes.Clear();
                    listener.Prefixes.Add(prefix);
                    listener.TimeoutManager.RequestQueue = TimeSpan.FromMilliseconds(1000);
                    listener.Start();
                    semaphoreRead = new Semaphore(accepts, accepts);
                    semaphoreExecute= new Semaphore(accepts/2, accepts/2);
                    scheduler = new ConcurrentExclusiveSchedulerPair(TaskScheduler.Default, accepts*2, 1).ExclusiveScheduler;
                    blockingCollection = new BlockingCollection<HttpListenerContext>(accepts);

                    listenerThread = new Thread(Listen)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.Highest
                    };

                    consumerThread = new Thread(Consume)
                    {
                        IsBackground = true,
                        Priority = ThreadPriority.AboveNormal
                    };
                    listenerThread.Start();
                    consumerThread.Start();

                    urlParser = new UrlParser();
                    isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (listener)
            {
                if (!isRunning)
                    return;

                listener.Stop();

                listenerThread.Abort();
                listenerThread.Join();

                isRunning = false;
            }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            disposed = true;

            Stop();

            listener.Close();
        }

        private void Listen()
        {
            while (true)
            {
                try
                {
                    semaphoreRead.WaitOne();
                    listener.GetContextAsync().ContinueWith((t) =>
                    {
                        var context = t.Result;
                        if (!blockingCollection.TryAdd(context))
                            SendResponse((HttpStatusCode)429, context);
                        semaphoreRead.Release();
                    });
                }
                catch (Exception error)
                {
                    Console.WriteLine(error.Message);
                    return;
                }
            }
        }

        private void Consume()
        {
            while (true)
            {
                try
                {
                    HttpListenerContext context = null;
                    blockingCollection.TryTake(out context);
                    if (context != null)
                        if (semaphoreExecute.WaitOne(0))
                            Task.Factory.StartNew(()=>HandleContext(context), CancellationToken.None, TaskCreationOptions.None, scheduler);
                        else
                            SendResponse((HttpStatusCode)429, context);
                }
                catch (Exception error)
                {
                    return;
                }
            }
        }

        private void SendResponse(HttpStatusCode statusCode, HttpListenerContext listenerContext)
        {
            listenerContext.Response.StatusCode = (int)statusCode;
            listenerContext.Response.OutputStream.Close();
        }

        private bool SendResponseIfEmpty(int height, int width, HttpListenerContext listenerContext)
        {
            if (height == 0 || width == 0)
            {
                SendResponse(HttpStatusCode.NoContent, listenerContext);
                semaphoreExecute.Release();
                return true;
            }
            return false;
        }

        private async Task HandleContext(HttpListenerContext listenerContext)
        {
            if (!urlParser.IsCorrectRequest(listenerContext))
                SendResponse(HttpStatusCode.BadRequest, listenerContext);
            else
            {
                using (var image = new Bitmap(listenerContext.Request.InputStream))
                {
                    var path = urlParser.GetRequestPath(listenerContext);
                    var filterName = path.Substring(0, path.IndexOf('/'));
                    var cropArea = urlParser.ParseParams(path);
                    cropArea.Intersect(new Rectangle(0, 0, image.Width, image.Height));
                    if (!SendResponseIfEmpty(cropArea.Height, cropArea.Width, listenerContext))
                    {
                        Bitmap resultImage = FilterFactory.GetFilter(filterName).Process(image, cropArea);
                        if (SendResponseIfEmpty(resultImage.Height, resultImage.Width, listenerContext))
                            return;
                        listenerContext.Response.ContentType = "image/png";
                        using (var ms = new MemoryStream())
                        {
                            resultImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            listenerContext.Response.ContentLength64 = ms.Length;
                            ms.WriteTo(listenerContext.Response.OutputStream);
                        }
                        listenerContext.Response.OutputStream.Close();
                        semaphoreExecute.Release();
                    }
                }
            }
        }

        private readonly HttpListener listener;
        private readonly int accepts;
        private Semaphore semaphoreRead;
        private Semaphore semaphoreExecute;
        private TaskScheduler scheduler;
        private BlockingCollection<HttpListenerContext> blockingCollection;
        private Thread listenerThread;
        private Thread consumerThread;
        private bool disposed;
        private volatile bool isRunning;
        private UrlParser urlParser;
    }
}
