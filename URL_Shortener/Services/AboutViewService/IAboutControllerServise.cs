namespace URL_Shortener.Services.AboutViewService
{
    public interface IAboutControllerServise
    {
        public Task ChangeTextAsync(string text);
        public Task<string> GetTextAsync();
    }
}
