namespace saper
{
    public class SafeConversion
    {
        public static int StringToInt(string stringToConvert)
        {
            try
            {
                return Convert.ToInt32(stringToConvert);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }
    }
}
