
namespace assessment2526
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing() // built in maui function that runs every time the users goes to this page 
        {
            base.OnAppearing(); // runs the original on appearing before run the if statement 

            if (Loginpage.isSpeachEnabled)
            {
                TextToSpeech.Default.SpeakAsync("this the setting page");
            }
        }
        void text_speach_toggle(object sender, ToggledEventArgs e) // this function turns text to speech on or off depending on what the users chooses 
        {
            if (Loginpage.isSpeachEnabled == false)
            {

                Loginpage.isSpeachEnabled = true;
                TextToSpeech.Default.SpeakAsync("text to speach is now on");
            }
            else
            {
                Loginpage.isSpeachEnabled = false;
                TextToSpeech.Default.SpeakAsync("text to speach is now off");

            }

        }

    }
}
