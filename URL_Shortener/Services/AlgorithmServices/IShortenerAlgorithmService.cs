﻿namespace URL_Shortener.Services.AlgorithmServices
{
    public interface IShortenerAlgorithmService
    {
        public string IdToShortURL(int n);
        public int shortURLtoID(string shortURL);
    }
}
