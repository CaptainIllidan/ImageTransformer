using System.Drawing;

namespace ImageTransformer.Filters
{
    class FlipHFilter : PixelFilter
    {
        public override void Transform(Bitmap original)
        {
            original.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }
    }
}
