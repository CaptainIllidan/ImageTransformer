using System.Drawing;

namespace ImageTransformer.Filters
{
    class RotateCcwFilter : PixelFilter
    {
        public override void Transform(Bitmap original)
        {
            original.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }
    }
}
