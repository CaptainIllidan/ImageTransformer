using System;

namespace ImageTransformer.Filters
{
    static class FilterFactory
    {
        public static IFilter GetFilter(string filter)
        {
            switch (filter)
            {
                case "rotate-cw":
                    return new RotateCwFilter();
                case "rotate-ccw":
                    return new RotateCcwFilter();
                case "flip-v":
                    return new FlipVFilter();
                case "flip-h":
                    return new FlipHFilter();
                default:
                    throw new ApplicationException($"Filter {filter} is not found");
            }
        }
    }
}
