using System;
using System.Text;

namespace Utility
{
    public class UtilityFunctions
    {
        public static string GetOnlineStatusCssClass(string status)
        {
            if (status == "Online")
                return "status online";

            else if (status == "Away")
                return "status away";

            else
                return "status offline";
        }

        public static string GetUserImagePath(string photoUrl)
        {
            if (String.IsNullOrEmpty(photoUrl))
                return "/assets/img/user.jpg";
            return photoUrl;
        }

        public static string GenerateRandomPassword(int length)
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();
            var specialCharacters = "!#$%&()*+-/:;<=?@[\\{}]^_";

            for (int i = 0; i < length; i++)
            {
                // Decide whether to add an uppercase letter, a lowercase letter, or a number
                int whichType = random.Next(0, 4);
                switch (whichType)
                {
                    // Lower case letter
                    case 0:
                        str_build.Append((char)(97 + random.Next(0, 26)));
                        break;
                    // Upper case letter
                    case 1:
                        str_build.Append((char)(65 + random.Next(0, 26)));
                        break;
                    // Number
                    case 2:
                        str_build.Append((char)(48 + random.Next(0, 10)));
                        break;
                    // Special Character
                    case 3:
                        str_build.Append(specialCharacters[random.Next(specialCharacters.Length)]);
                        break;
                }
            }

            return str_build.ToString();
        }
    }
}