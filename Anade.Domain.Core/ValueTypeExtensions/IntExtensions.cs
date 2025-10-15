namespace Anade.Domain.Core.ValueTypeExtensions
{
    public static class IntExtensions
    {
        public static int CountDigit(this int number)
        {          
            return number.ToString().Length;
        }

        #region Boolean methods : is 1, 2, 3, 4 or 5 digits long
        public static bool isOneDigit(this int number)
        {
            return number.CountDigit() == 1;
        }
        public static bool isTwoDigits(this int number)
        {
            return number.CountDigit() == 2;
        }
        public static bool isThreeDigits(this int number)
        {
            return number.CountDigit() == 3;
        }
        public static bool isFourDigits(this int number)
        {
            return number.CountDigit() == 4;
        }
        public static bool isFiveDigits(this int number)
        {
            return number.CountDigit() == 5;
        } 
        #endregion
    }
}
