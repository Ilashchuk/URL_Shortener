using System.Text;

namespace URL_Shortener.Services.AboutViewService
{
    public class AboutControllerServise : IAboutControllerServise
    {
        public string path = @"wwwroot\about.txt";
        public async Task ChangeTextAsync(string text)
        {
            using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(text);
                await fstream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        public async Task<string> GetTextAsync()
        {
            string text;
            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                string textFromFile = Encoding.Default.GetString(buffer);
                text = textFromFile.Trim();
            }
            return text;
        }
    }
}
