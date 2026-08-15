
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace assessment2526;

public partial class Loginpage : ContentPage
{

    public static string user_info_display_name; // declaring a string variable that can be used in any cs file 
    public static bool isSpeachEnabled = true; // declaring a bool variable that can be used in any cs file 
    string user_info_username = ""; // declaring a string variable 
    string user_info_password = ""; // declaring a string variable 
    private readonly FirebaseAuthClient _authClient; // Declares a private field that is read only that will hold the Firebase login clint
    public Loginpage(FirebaseAuthClient authClient)
    {
        InitializeComponent();
        _authClient = authClient; // Stores the Firebase client that was passed

    }

    protected override void OnAppearing()  // built in maui function that runs every time the users goes to this page 
    {
        base.OnAppearing(); // runs the original on appearing before run the if statement 

        if (isSpeachEnabled)
        {
            TextToSpeech.SpeakAsync("Welcome user to the Login page.Text to speech will be on the  whole time. If you want me to not talk please press text to speech OFF ");
        }
    }

    private async void OnHelpClicked(object sender, EventArgs e) // help button that displays info on what to do on this page
    {
        await DisplayAlert("Help",
       "Enter your username, email, and password to sign in.\n\n" +
       "If you do not have an account, tap Sign Up to create one.\n\n" +
       "Tap the 'Text to speech OFF' button to stop hearing the screen read aloud.",
       "OK");

    }
    private async void LoginButton(object sender, EventArgs e) // when users press the login button it executes this section of code to allow the user to sign in through  firebase 
    {
        try
        {
           user_info_username = login_username_input.Text; // stores users login input variable 
           user_info_password = login_password_input.Text; // stores users login password to this variable 
            user_info_display_name = display_name_input.Text;
         bool  checker_returned = await check_credentials(); // waits the check that the cardinalates are correct 
            if (checker_returned == true) // if credential are correct run the code inside the if statement 
            {
                await _authClient.SignInWithEmailAndPasswordAsync(user_info_username, user_info_password); // sends a request to firebase to login this user and to check if the email and password is in the database.   

            }
            
            if(checker_returned == true && isSpeachEnabled == true)
            {
               await TextToSpeech.Default.SpeakAsync("login successful");
               await Shell.Current.GoToAsync("//Bank"); // goes to the main page if the user credentials are correct
                return;
            }
            if (checker_returned == true)
            {
                await DisplayAlert("login", "login successful", "OK");
                await Shell.Current.GoToAsync("//Bank"); // goes to the main page if the user credentials are correct
            }
            else
            {
                await Shell.Current.GoToAsync("//loginpage");
            }
        }

        catch (Exception ex)
        {
          Firebase_errors(ex); // displays firebase errors in a user friendly way
        }
    }

    public async void Firebase_errors(Exception error) // displays firebase errors in a user friendly way 
    {
        string erros_message = error.Message.ToLower();

        if (erros_message.Contains("email_not_found") || erros_message.Contains("user-not-found")) // Check if Firebase returned a user not found 
        {
            await DisplayAlert("Login Failed", "No account found with this email.", "OK");
        }
        else if (erros_message.Contains("invalid_password") || erros_message.Contains("wrong-password")) //Check if Firebase returned if the  wrong password was entered
        {
            await DisplayAlert("Login Failed", "Incorrect password. Please try again.", "OK");
        }
        else if (erros_message.Contains("invalid_email") || erros_message.Contains("invalid-email")) //Check if Firebase returned a invalid email was entered 
        {
            await DisplayAlert("Login Failed", "Please enter a valid email address.", "OK");
        }
        else if (erros_message.Contains("user_disabled") || erros_message.Contains("user-disabled")) //Check if firebase returned if the users account is disabled
        {
            await DisplayAlert("Login Failed", "This account has been disabled.", "OK");
        }
        else if (erros_message.Contains("too_many_attempts") || erros_message.Contains("too-many-requests")) //Check if firebase returned if the users had to many attempts 
        {
            await DisplayAlert("Login Failed", "Too many failed login attempts. Please try again later.", "OK");
        }
        else if (erros_message.Contains("network") || erros_message.Contains("timeout")) //Check if firebase returned if the users had disconnected 
        {
            await DisplayAlert("Login Failed", "Network error. Check your internet connection.", "OK");
        }
        else if (erros_message.Contains("invalid_login_credentials") || erros_message.Contains("invalid-credential")) //Check if firebase returned invalid email 
        {
            await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
        }
        else
        {
            await DisplayAlert("Login Failed", "Something went wrong. Please try again.", "OK"); // just displays  something went wrong if its non of these errors
        }


    }

    public async Task<bool> check_credentials() // checks credentials to see non of the fields are empty or that the password is the correct length
    {
        bool checker = true;

        if (string.IsNullOrEmpty(user_info_display_name))
        {
            await DisplayAlert("info", "please enter a username", "OK");
            return checker = false;
        }
        if (string.IsNullOrEmpty(user_info_password) || string.IsNullOrEmpty(user_info_username))
        {
            await DisplayAlert("info", "please fill the required filled", "OK");
            return checker = false;
        }

        if (user_info_password.Length < 6)
        {
            await DisplayAlert("info", "incorrect password", "OK");
            return checker = false;



        }
        return checker;
    }
    private async void GotoSingup(object sender, EventArgs e)
	{
        await Shell.Current.GoToAsync("//Sign_up"); // when button is pressed goes to the sing up page. 
    }

    public async void text_speech_toggle(object sender, EventArgs e) // turns off text to speech
    {
        isSpeachEnabled = false;
       
    }
}