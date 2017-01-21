namespace ArticleReader.TextToSpeech
{
    interface ITextToSpeech
    {
        void Speak(string text);
        void Stop();
    }
}