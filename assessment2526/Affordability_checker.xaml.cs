namespace assessment2526;

using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.IO;
using System.Diagnostics;

public partial class Affordability_checker : ContentPage
{
    int inputted_amount = 0;
   int balance = 0;
	public Affordability_checker()
	{
		InitializeComponent();
	}

    protected override void OnAppearing() // built in maui function that runs every time the users goes to this page 
    {
        base.OnAppearing(); // runs the original on appearing before run the if statement 

        if (Loginpage.isSpeachEnabled)
        {
            TextToSpeech.Default.SpeakAsync("this the affordability checker page");
        }
    }
    private async void Photo_taken(object sender, EventArgs e)
	{

        try
        {
            var captureImageCTS = new CancellationTokenSource(TimeSpan.FromSeconds(3)); // creating a cancelation token. 
            Stream stream = await Camera.CaptureImage(captureImageCTS.Token); // captures image and if capturing image takes longer than 3 seconds stop
            if (stream != null)
            {
                image_preview.Source = null;
                string targetFile = Path.Combine(FileSystem.Current.AppDataDirectory, $"currentphoto_{DateTime.Now.Ticks}.jpg"); // the path with the time stamp 
                stream.Position = 0;
                using FileStream outputStream = File.Create(targetFile); // creates file 
                await stream.CopyToAsync(outputStream); // writes image data to file 
                outputStream.Close(); // stop's writing data to the devices disk 
                image_preview.Source = targetFile; // shows image on the image label called image_preview 

                affordability_check();
            }
            else {


                await DisplayAlert("Error", "stream was null", "OK");


            }

        }
        catch (Exception ex)
        {
           
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    public async void affordability_check() // check if users can afforded this iteam or not 
    {
        balance = BankPage.results; 

        if(iteam_input_amount.Text == null && Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("please enter a price for this item ");
            return;
        }
        if (iteam_input_amount.Text == null)
        {
            await DisplayAlert("INFO", "please enter a price for this item ", "OK");
            return;
        }
        inputted_amount = Int32.Parse(iteam_input_amount.Text);
        if(inputted_amount == 0 && Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("Please enter the price of the item ");
            return;
        }
        if(inputted_amount == 0)
        {
            await DisplayAlert("INFO", "please enter an amount for this item ", "OK");
            return;
        }
        if(inputted_amount < balance && Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("You can afford this");
            return; 
        }
        if(inputted_amount > balance && Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("You can not afford this ");
            return;
        }
        if (inputted_amount < balance) {
            await DisplayAlert("INFO", "you can afford this ", "OK");
            return;
        }
        else
        {
            await DisplayAlert("INFO", "you can afford it ", "OK");
        }
    }

}

