using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageTransformer;
using System.Net;
using System.Drawing;

namespace UnitTestProject1
{
    [TestClass()]
    public class TransformerTests
    {
        private AsyncHttpServer server;

        [TestInitialize]
        public void SetupContext()
        {
            server = new AsyncHttpServer();
            server.Start("http://localhost:8080/");
        }
        [TestCleanup]
        public void Stop()
        {
            server.Stop();
            server.Dispose();
        }

        [TestMethod]
        public void RotateCwCorrect()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/process/rotate-cw/10,50,135,95");
            var img = new Bitmap("C:\\Users\\Дмитрий\\Source\\Repos\\ImageTransformer\\UnitTestProject1\\bin\\Debug\\Lenna.png");
            request.Method = "POST";
            img.Save(request.GetRequestStream(), System.Drawing.Imaging.ImageFormat.Png);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var image = new Bitmap(response.GetResponseStream());
            image.Save("Lenna-rorate-cw-debug.png", System.Drawing.Imaging.ImageFormat.Png);
            bool imageEquality = true;
            var modelImage = new Bitmap(img);
            modelImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            modelImage = modelImage.Clone(new Rectangle(10, 50, 135, 95), modelImage.PixelFormat);
            modelImage.Save("Lenna-rorate-cw-model.png", System.Drawing.Imaging.ImageFormat.Png);
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    if (modelImage.GetPixel(x, y) != image.GetPixel(x, y))
                    {
                        var pixIm = image.GetPixel(x, y);
                        var pixModel = modelImage.GetPixel(x, y);
                        imageEquality = false;
                    }
            response.Close();
            Assert.IsTrue(imageEquality);
        }

        [TestMethod]
        public void RotateCcwCorrect()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/process/rotate-ccw/10,10,135,135");
            var img = new Bitmap("C:\\Users\\Дмитрий\\Source\\Repos\\ImageTransformer\\UnitTestProject1\\bin\\Debug\\Lenna.png");
            request.Method = "POST";
            img.Save(request.GetRequestStream(), System.Drawing.Imaging.ImageFormat.Png);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var image = new Bitmap(response.GetResponseStream());
            image.Save("Lenna-rorate-ccw-debug.png", System.Drawing.Imaging.ImageFormat.Png);
            bool imageEquality = true;
            var modelImage = new Bitmap(img);
            modelImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            modelImage = modelImage.Clone(new Rectangle(10, 10, 135, 135), modelImage.PixelFormat);
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    if (modelImage.GetPixel(x, y) != image.GetPixel(x, y))
                        imageEquality = false;
            response.Close();
            Assert.IsTrue(imageEquality);
        }

        [TestMethod]
        public void FlipVCorrect()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/process/flip-v/10,10,135,135");
            var img = new Bitmap("C:\\Users\\Дмитрий\\Source\\Repos\\ImageTransformer\\UnitTestProject1\\bin\\Debug\\Lenna.png");
            request.Method = "POST";
            img.Save(request.GetRequestStream(), System.Drawing.Imaging.ImageFormat.Png);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var image = new Bitmap(response.GetResponseStream());
            image.Save("Lenna-flip-v-debug.png", System.Drawing.Imaging.ImageFormat.Png);
            bool imageEquality = true;
            var modelImage = new Bitmap(img);
            modelImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
            modelImage = modelImage.Clone(new Rectangle(10, 10, 135, 135), modelImage.PixelFormat);
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    if (modelImage.GetPixel(x, y) != image.GetPixel(x, y))
                        imageEquality = false;
            response.Close();
            Assert.IsTrue(imageEquality);
        }

        [TestMethod]
        public void FlipHCorrect()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:8080/process/flip-h/10,10,135,135");
            var img = new Bitmap("C:\\Users\\Дмитрий\\Source\\Repos\\ImageTransformer\\UnitTestProject1\\bin\\Debug\\Lenna.png");
            request.Method = "POST";
            img.Save(request.GetRequestStream(), System.Drawing.Imaging.ImageFormat.Png);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            var image = new Bitmap(response.GetResponseStream());
            image.Save("Lenna-flip-h-debug.png", System.Drawing.Imaging.ImageFormat.Png);
            bool imageEquality = true;
            var modelImage = new Bitmap(img);
            modelImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            modelImage = modelImage.Clone(new Rectangle(10, 10, 135, 135), modelImage.PixelFormat);
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    if (modelImage.GetPixel(x, y) != image.GetPixel(x, y))
                        imageEquality = false;
            response.Close();
            Assert.IsTrue(imageEquality);
        }
    }
}
