using System.Drawing;

namespace ImageTransformer.Filters
{
    abstract class PixelFilter : IFilter
    {
        public Bitmap Crop(Bitmap original, Rectangle cropArea)
        {
            return original.Clone(cropArea, original.PixelFormat);
        }

        public Bitmap Process(Bitmap original, Rectangle cropArea)
        {
            Transform(original);
            return Crop(original, cropArea);
        }

        public abstract void Transform(Bitmap original);
    }
}
