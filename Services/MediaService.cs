using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace QuizApp.Services
{
    public class MediaService : IMediaService
    {
        private readonly MediaElement mediaElement;

        public MediaService()
        {
            mediaElement = new MediaElement();
            MediaElement = mediaElement;
        }

        public object MediaElement { get; }

        public async Task SpeakAsync(string text)
        {
            var voice = SpeechSynthesizer.AllVoices.First(x => x.Gender == VoiceGender.Male && x.Language == "en-GB");
            using (var speechSynthesizer = new SpeechSynthesizer { Voice = voice })
            {
                var stream = await speechSynthesizer.SynthesizeTextToStreamAsync(text);
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();
            }
        }
    }
}
