using System.Text.RegularExpressions;

namespace timviec.Utils
{
    public class Slug
    {
        public static string GenerateSlug(string phrase)
        {
            string str = phrase.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // Remove non-alphanumeric characters
            str = Regex.Replace(str, @"\s+", " ").Trim(); // Remove multiple spaces
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // Limit the length to 45 characters
            str = Regex.Replace(str, @"\s", "-"); // Convert spaces to hyphens

            // Generate a 4-character ID using a random number generator
            Random rand = new Random();
            string id = new string(Enumerable.Range(0, 4).Select(i => (char)('a' + rand.Next(26))).ToArray());

            str += $"-{id}"; // Append the ID to the end of the slug with a hyphen
            return str;
        }
    }
}
