using System.Drawing;

namespace ImageTransformer.Filters
{
    interface IFilter
    {
        Bitmap Process(Bitmap original, Rectangle cropArea);
        void Transform(Bitmap original);
        Bitmap Crop(Bitmap original, Rectangle cropArea);
    }
}
