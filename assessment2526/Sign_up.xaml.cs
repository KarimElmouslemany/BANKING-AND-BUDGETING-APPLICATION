
using Firebase.Auth;
using Microsoft.Maui.ApplicationModel.Communication;
namespace assessment2526;
public partial class Sign_up : ContentPage
{
    private readonly FirebaseAuthClient _authClient; // Declares a private field that is read only that will hold the Firebase sign up  clint
    public Sign_up(FirebaseAuthClient authClient)
	{
        _authClient = authClient; // Stores the Firebase client that was passed

        InitializeComponent();
	}
    protected override void OnAppearing()  // built in maui function that runs every time the users goes to this page 
    {
        base.OnAppearing(); // runs the original on appearing before run the if statement 

        if (Loginpage.isSpeachEnabled == true)
        {
            TextToSpeech.Default.SpeakAsync("welcome to the Sign up page");
        }
    }
    private async void SignUpButton(object sender, EventArgs e) // gets the users login and sign up info 
    {
        string user_name = SignUp_username_Input.Text; // assigns user email details 
        string password = SignUp_password_Input.Text; // assigns users password 
        Loginpage.user_info_display_name = SignUp_DisplayName_Input.Text;

        if (string.IsNullOrEmpty(user_name) || string.IsNullOrEmpty(password) && Loginpage.isSpeachEnabled == true) // check if the filed is empty for username and password and says it in text to speach 
        {
            await TextToSpeech.Default.SpeakAsync("one of the fields is empty");
            return;
        }
        if(password.Length < 6 && Loginpage.isSpeachEnabled == true) // check if password is the right length and text to speach is enabled 
        {
            await TextToSpeech.Default.SpeakAsync("password is too short enter a password that is longer than 6 characters");
            return; 
        }
        if (string.IsNullOrEmpty(user_name)|| string.IsNullOrEmpty(password)) // check if username and password is empty 
        {
            await DisplayAlert("Error", "one of the fields is empty", "OK");
            return;
        }
        if (password.Length  < 6) { // check if passowrd is less then six 

            await DisplayAlert("Error", "password is to short enter a password that is longer than 6 characters","OK");
            return;
        }
        if (string.IsNullOrEmpty(Loginpage.user_info_display_name) && Loginpage.isSpeachEnabled == true) // check users name is empty 
        {
            await TextToSpeech.Default.SpeakAsync("Please enter a username");
            return;
        }
        if (string.IsNullOrEmpty(Loginpage.user_info_display_name)) // check iiif username is empty for users who doen thave text to speach on 
        {
            await DisplayAlert("Error","username is empty", "OK");
            return;
        }
        try
        {  
          var user_states = await _authClient.CreateUserWithEmailAndPasswordAsync(user_name, password); // creates an acount on firebase so the user can login 
            if(Loginpage.isSpeachEnabled == true)
            {
                await TextToSpeech.Default.SpeakAsync("Account created");
                await Shell.Current.GoToAsync("//Bank"); // sends user to the main page
                return;
            }
            await DisplayAlert("Success", "Account created", "OK");
            await Shell.Current.GoToAsync("//Bank"); // sends user to the main page
        }
        catch(Exception ex) {
            Firebase_signup_errors(ex); // check firebase errors 
        }
    }
    private async void OnHelpClicked(object sender, EventArgs e) // diisplayes the info on screen when the "?" button is clicked 
    {
        await DisplayAlert("Sign Up Help",
        "Choose a username, enter your email, and create a password to register a new account.\n\n" +
        "If you already have an account, tap 'Back to login page'.",
        "OK");
    }
    private async void Back_to_login_Button(object sender, EventArgs e) // takers the user back to the login page if they pressed the back to login button 
    {
        try
        {
            await Shell.Current.GoToAsync("//loginpage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Crash", ex.ToString(), "OK");
        }

    }

    public async void Firebase_signup_errors(Exception error) // displays firebase errors in a user friendly way 
    {
        string erros_message = error.Message.ToLower();

        if (erros_message.Contains("email_exists") || erros_message.Contains("email-already-in-use"))
        {
            await DisplayAlert("Signup Failed", "This email is already registered. Try logging in instead.", "OK");
        }
        else if (erros_message.Contains("invalid_email") || erros_message.Contains("invalid-email"))
        {
            await DisplayAlert("Signup Failed", "Please enter a valid email address.", "OK");
        }
        else if (erros_message.Contains("weak_password") || erros_message.Contains("weak-password"))
        {
            await DisplayAlert("Signup Failed", "Password is too weak. Use at least 6 characters.", "OK");
        }
        else if (erros_message.Contains("missing_password"))
        {
            await DisplayAlert("Signup Failed", "Please enter a password.", "OK");
        }
        else if (erros_message.Contains("missing_email"))
        {
            await DisplayAlert("Signup Failed", "Please enter an email address.", "OK");
        }
        else if (erros_message.Contains("operation_not_allowed") || erros_message.Contains("operation-not-allowed"))
        {
            await DisplayAlert("Signup Failed", "Signup is currently disabled. Please try again later.", "OK");
        }
        else if (erros_message.Contains("too_many_attempts") || erros_message.Contains("too-many-requests"))
        {
            await DisplayAlert("Signup Failed", "Too many attempts. Please try again later.", "OK");
        }
        else if (erros_message.Contains("network") || erros_message.Contains("timeout"))
        {
            await DisplayAlert("Signup Failed", "Network error. Check your internet connection.", "OK");
        }
        else
        {
            await DisplayAlert("Signup Failed", "Something went wrong. Please try again.", "OK");
        }
    }
}