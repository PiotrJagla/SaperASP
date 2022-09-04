using saper.DataStructures;

namespace saper
{
    public class Validation
    {
        public static bool isOutsize2DArray(Vector2D arrayIndex, Vector2D arraySize)
        {
            return (arrayIndex.x < 0 ||
                    arrayIndex.y < 0 ||
                    arrayIndex.y >= arraySize.y ||
                    arrayIndex.x >= arraySize.x);
        }

        public static bool isCustomModeInputCorrect(string gameBoardWidth, string gameBoardHeight, string bombsNumber)
        {
            int width = 0;
            int height = 0;
            int bombs = 0;
            try
            {
                width = Convert.ToInt32(gameBoardWidth);
                height = Convert.ToInt32(gameBoardHeight);
                bombs = Convert.ToInt32(bombsNumber);
            }
            catch
            {
                return false;
            }

            if (areSaperBoardNumbersCorrect(width, height, bombs) == true)
                return true;
            else
                return false;
        }

        private static bool areSaperBoardNumbersCorrect(int gameBoardWidth, int gameBoardHeight, int bombsNumber)
        {
            return (gameBoardHeight <= Constants.maxGameBoardSize.x && gameBoardHeight >= 2) &&
                   (gameBoardWidth <= Constants.maxGameBoardSize.y && gameBoardWidth >= 2) &&
                   bombsNumber <= (gameBoardHeight * gameBoardWidth) / 3 && bombsNumber >= 1;
        }
    }
}
