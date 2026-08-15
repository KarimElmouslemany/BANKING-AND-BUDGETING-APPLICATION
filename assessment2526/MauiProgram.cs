using CommunityToolkit.Maui; // import for thee maui tool kit 
using Firebase.Auth; // import for firebase authentication
using Firebase.Auth.Providers; // import for accessing diffrent methodes for user to sing in 

namespace assessment2526
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder(); // creats  a app builder 
            builder
                .UseMauiApp<App>()
                .UseMauiMaps() // enables map contorls
                .UseMauiCommunityToolkit() // enables the Community Toolkit.
                .UseMauiCommunityToolkitCamera() // enables the camera control tool kit 
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold"); 
                });


    		

            builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig() // making a new  firebase client
            {
                ApiKey = "AIzaSyATJRrUTUKwBndr71achbZU81tFMzCVKHg", // api key
                AuthDomain = "maui-mobile-app-ef588.firebaseapp.com", // which project this firebase belongs to 
                Providers = new Firebase.Auth.Providers.FirebaseAuthProvider[] // a list of ways the user can login and sing up
                {
                    new  EmailProvider() // allowing user to login through email and password
                }
            }));

            builder.Services.AddTransient<Loginpage>(); // creates a new login page everytime 
            builder.Services.AddTransient<Sign_up>(); // creates a new singup page every time 
            return builder.Build(); //Finalises the configuration
        }
    }
}
