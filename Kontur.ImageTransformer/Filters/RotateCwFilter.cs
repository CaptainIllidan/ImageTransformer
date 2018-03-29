using System.Drawing;

namespace ImageTransformer.Filters
{
    class RotateCwFilter : PixelFilter
    {
        public override void Transform(Bitmap original)
        {
            original.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
    }
}
