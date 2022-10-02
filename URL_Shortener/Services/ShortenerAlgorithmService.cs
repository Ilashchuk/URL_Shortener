namespace URL_Shortener.Services
{
    public class ShortenerAlgorithmService : IShortenerAlgorithmService
    {
        public string IdToShortURL(int n)
        {
            // Map to store 62 possible characters
            char[] map = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

            String shorturl = "";

            // Convert given integer id to a base 62 number
            while (n > 0)
            {
                // use above map to store actual character
                // in short url
                shorturl += (map[n % 62]);
                n = n / 62;
            }

            // Reverse shortURL to complete base conversion
            return Reverse(shorturl);

        }

        // Function to get integer ID back from a short url
        public int shortURLtoID(string shortURL)
        {
            int id = 0; // initialize result

            // A simple base conversion logic
            for (int i = 0; i < shortURL.Length; i++)
            {
                if ('a' <= shortURL[i] &&
                           shortURL[i] <= 'z')
                    id = id * 62 + shortURL[i] - 'a';
                if ('A' <= shortURL[i] &&
                           shortURL[i] <= 'Z')
                    id = id * 62 + shortURL[i] - 'A' + 26;
                if ('0' <= shortURL[i] &&
                           shortURL[i] <= '9')
                    id = id * 62 + shortURL[i] - '0' + 52;
            }
            return id;
        }
        public string Reverse(string input)
        {
            char[] a = input.ToCharArray();
            int l, r = a.Length - 1;
            for (l = 0; l < r; l++, r--)
            {
                char temp = a[l];
                a[l] = a[r];
                a[r] = temp;
            }
            return string.Join("", a);
        }
    }
}
