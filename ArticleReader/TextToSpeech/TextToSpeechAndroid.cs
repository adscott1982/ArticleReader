using Android.Speech.Tts;
using Xamarin.Forms;
using System.Collections.Generic;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(ArticleReader.TextToSpeech.TextToSpeechAndroid))]
namespace ArticleReader.TextToSpeech
{
    class TextToSpeechAndroid : Java.Lang.Object, ITextToSpeech, Android.Speech.Tts.TextToSpeech.IOnInitListener
    {
        Android.Speech.Tts.TextToSpeech speaker;
        string toSpeak = "";
        int charLimit = 3900;

        public TextToSpeechAndroid()
        {
            var ctx = Forms.Context; // useful for many Android SDK features
            speaker = new Android.Speech.Tts.TextToSpeech(ctx, this);
        }

        public void Speak(string text)
        {
            speaker.Stop();
            toSpeak = text;

            if (toSpeak.Length > charLimit)
            {
                var splitStrings = SplitString(toSpeak);

                foreach(var splitString in splitStrings)
                {
                    speaker.Speak(splitString, QueueMode.Add, null, null);
                }
            }
            else
            {
                speaker.Speak(toSpeak, QueueMode.Flush, null, null);
            }
        }

        private List<string> SplitString(string s)
        {
            var splitStrings = new List<string>();

            while(s.Length > charLimit)
            {
                var splitPosition = s.Substring(0, charLimit).LastIndexOf('.');
                var splitString = s.Substring(0, splitPosition);
                s = s.Substring(splitPosition + 1);

                splitStrings.Add(splitString);
            }

            return splitStrings;
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Add, null, null);
            }
        }

        public void Stop()
        {
            speaker.Stop();
        }
        #endregion
    }
}