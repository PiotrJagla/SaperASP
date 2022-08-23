using saper.DataStructures;

namespace saper
{
    public class Validation
    {
        public static bool isOutsize2DArray(Vector2D arrayIndex, Vector2D arraySize)
        {
            return (arrayIndex.x < 0 ||
                    arrayIndex.y < 0 ||
                    arrayIndex.x >= arraySize.x ||
                    arrayIndex.y >= arraySize.y);
        }
    }
}
