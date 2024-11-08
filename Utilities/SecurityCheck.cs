using System.Text.RegularExpressions;
using System.Web;

namespace Node_ApiService_Test.Utilities
{
    public static class SecurityChecks
    {
        private static readonly Regex emailRegex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,}$");

        public static string SanitizeAndValidateEmail(string input)
        {
            //Sanitize the input string
            string sanitizedInput = SanitizeString(input);

            //Validate if the input is a valid email format
            if (!emailRegex.IsMatch(sanitizedInput))
            {
                throw new FormatException("Invalid email format.");
            }

            return sanitizedInput;
        }

        // Basic HTML Encoding (for preventing HTML injection)
        public static string HtmlEncodeInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return HttpUtility.HtmlEncode(input);
        }


        // Comprehensive sanitization function
        public static string SanitizeString(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            //Encode HTML special characters (prevents HTML injection)
            input = HttpUtility.HtmlEncode(input);

            //Remove script tags and any other potentially harmful tags
            input = Regex.Replace(input, "<script.*?</script>", "", RegexOptions.IgnoreCase); // Remove script tags
            input = Regex.Replace(input, "<iframe.*?</iframe>", "", RegexOptions.IgnoreCase); // Remove iframe tags
            input = Regex.Replace(input, "<object.*?</object>", "", RegexOptions.IgnoreCase); // Remove object tags
            input = Regex.Replace(input, "<embed.*?</embed>", "", RegexOptions.IgnoreCase);   // Remove embed tags
            input = Regex.Replace(input, "<applet.*?</applet>", "", RegexOptions.IgnoreCase); // Remove applet tags

            //Remove dangerous event handler attributes like onmouseover, onclick, etc.
            input = Regex.Replace(input, @"\bon\w+\s*=", "", RegexOptions.IgnoreCase);

            //Remove any unwanted HTML tags or restrict allowed tags
            input = Regex.Replace(input, @"<(?!a|b|i|u|em|strong|img|br|p|ul|ol|li|span|div)[^>]+>", "", RegexOptions.IgnoreCase);

            //Remove URLs containing javascript: or data: schemes
            input = Regex.Replace(input, @"(href|src|action|background)\s*=\s*['""][\s]*javascript:[\s\S]*?['""]", "", RegexOptions.IgnoreCase);    // Remove JS URLs
            input = Regex.Replace(input, @"(href|src|action|background)\s*=\s*['""][\s]*data:[\s\S]*?['""]", "", RegexOptions.IgnoreCase);          // Remove data URLs

            //Replace multiple spaces with a single space to clean up the input
            input = Regex.Replace(input, @"\s+", " ").Trim();

            return input;
        }
    }
}
