using System;

namespace Anade.Khadamat.Web
{
    public static class AgeHelper
    {
        public static int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        public static bool IsAgeValid(DateTime birthDate, int minAge = 18, int maxAge = 55)
        {
            var age = CalculateAge(birthDate);
            return age >= minAge && age <= maxAge;
        }
    }
}
