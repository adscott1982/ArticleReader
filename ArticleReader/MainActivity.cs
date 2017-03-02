using Android.App;
using Android.Widget;
using Android.OS;
using System;
using ArticleReader.TextToSpeech;
using System.Threading.Tasks;

namespace ArticleReader
{
    [Activity(Label = "Article Reader", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button LoadUrlButton;
        Button SpeakButton;
        Button StopButton;
        EditText UrlBox;
        TextView TextView;
        ITextToSpeech Tts;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            this.Tts = Xamarin.Forms.DependencyService.Get<ITextToSpeech>();
            this.LoadUrlButton = FindViewById<Button>(Resource.Id.LoadUrlButton);
            this.SpeakButton = FindViewById<Button>(Resource.Id.SpeakButton);
            this.StopButton = FindViewById<Button>(Resource.Id.StopButton);
            this.TextView = FindViewById<TextView>(Resource.Id.TextView);
            this.UrlBox = FindViewById<EditText>(Resource.Id.urlText);

            LoadUrlButton.Click += LoadUrlClick;
            SpeakButton.Click += SpeakText;
            StopButton.Click += StopSpeech;

        }

        private async void LoadUrlClick(object sender, EventArgs e)
        {
            LoadUrlButton.Enabled = false;

            var text = UrlBox.Text;
            var url = new Uri(text);

            if (!url.IsWellFormedOriginalString())
            {
                TextView.Text = "Invalid url";
                LoadUrlButton.Enabled = false;
                return;
            }

            ClearText();

            await Task.Run(() =>
            {
                var webPage = new WebArticleParser.WebPageText(url.AbsoluteUri);

                TextView.Text = webPage.Text;
            });

            LoadUrlButton.Enabled = false;
        }

        private void SpeakText (object sender, EventArgs e)
        {
            this.Tts.Speak(TextView.Text);
        }

        private void ClearText()
        {
            this.TextView.Text = "";
        }

        private void StopSpeech(object sender, EventArgs e)
        {
            this.Tts.Stop();
        }
    }
}

