using System.Drawing;

namespace ImageTransformer.Filters
{
    class FlipVFilter : PixelFilter
    {
        public override void Transform(Bitmap original)
        {
            original.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }
    }
}
