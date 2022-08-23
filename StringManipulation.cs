using saper.DataStructures;

namespace saper
{
    public class StringManipulation
    {
        public static Vector2D getButtonCoordsFromString(string buttonCoords)
        {
            //String format is "buttonXpos:buttonYpos"
            Vector2D buttonPosition = new Vector2D();

            int colonPosition = buttonCoords.IndexOf(':');
            string buttonXpos = buttonCoords.Substring(0, colonPosition);
            string buttonYpos = buttonCoords.Substring(colonPosition + 1, buttonCoords.Length - colonPosition - 1);

            buttonPosition.x = SafeConversion.StringToInt(buttonXpos);
            buttonPosition.y = SafeConversion.StringToInt(buttonYpos);

            return buttonPosition;
        }

        private static void getButtonCoordsFromString_TEST(string buttonCoords)
        {

            Vector2D buttonPosition = new Vector2D(StringManipulation.getButtonCoordsFromString(buttonCoords));

            Console.WriteLine("Vector2D: " + buttonPosition.x + ":" + buttonPosition.y);
            Console.WriteLine("String: " + buttonCoords);
            Console.WriteLine();

            if (buttonCoords == (buttonPosition.x + ":" + buttonPosition.y))
            {
                Console.WriteLine("Test Passed");
            }
            else
            {
                Console.WriteLine("Test Failed");
            }
        }
    }
}
