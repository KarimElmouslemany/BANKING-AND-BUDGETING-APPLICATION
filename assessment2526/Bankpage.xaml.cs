

using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;
using static Microsoft.Maui.ApplicationModel.Permissions;


namespace assessment2526;

public partial class BankPage : ContentPage
{
    public int user_input;
    public static int results = 0;
    string results_out_put;
    bool takeaway = false;
    bool adding = false;
    string json;

    public BankPage()
    {


        InitializeComponent();


    }

    protected override void OnAppearing()  // built in maui function that runs everytime the users goes to this page 
    {
        base.OnAppearing(); // runs the original on appearing before run the if statment 

        if (Loginpage.isSpeachEnabled == true)
        {
            TextToSpeech.Default.SpeakAsync("this the bankpage");
            load_username(); // displays users username
        }
        else
        {
            load_username(); // displays username on the page 

        }

    }
    public async void load_username() 
    {

        string name = Loginpage.user_info_display_name;
        displaying_username.Text = " Welcome " + name;

        if (Loginpage.isSpeachEnabled == true) // says users name if text to speech is enabled
        {
            await TextToSpeech.Default.SpeakAsync(" Welcome" + name);
        }

    }

    private void Adding_button(object sender, EventArgs e)
    {
        displying_adding_ammount(); 
        adding = true;
        users_adding_things();
    }
    private void takeaway_button(object sender, EventArgs e)
    {
        display_takeway_amount();
        takeaway = true;
        users_adding_things();

    }
    private async void location_getter(object sender, EventArgs e)
    {
        await Location_permsion(); // waits for the location 
    }
    public async void displying_adding_ammount() // displays the added amount to the screen and if text to speech is enabled it speaks 
    {
        user_input = Int32.Parse(amount_inputed.Text);
        results += user_input;
        if (results < 0)
        {
            set_balance_colours(false); // sets the text  colure to red 

            amount_displayed.Text = "£ " + results.ToString();
            if (Loginpage.isSpeachEnabled == true)
            {
                await TextToSpeech.Default.SpeakAsync("You are still in debt please increase the amount of money to more than £ " + user_input);


            }
            else
            {
                await DisplayAlert("Info", "You are still in debt please increase the amount of money to more than £ " + user_input, "OK");
            }

        }
        else
        {
            set_balance_colours(true); // sets the text  cloure to white or black
        
            amount_displayed.Text = "£ " + results.ToString();

            if (Loginpage.isSpeachEnabled == true)
            {
                await TextToSpeech.Default.SpeakAsync("Balance is " + amount_displayed.Text);
            }

        }
    }


    public async void display_takeway_amount()
    {
        user_input = Int32.Parse(amount_inputed.Text);
        results -= user_input;
        if (results < 0)
        {
            set_balance_colours(false); // tells the function to change the color to red

            amount_displayed.Text = "£ " + results.ToString(); // displays the bank balance on screen 
            if (Loginpage.isSpeachEnabled == true)
            {
                await TextToSpeech.Default.SpeakAsync("You are in  debt");
            }
            else
            {
                await DisplayAlert("Info", "You are in  debt", "OK");
            }
  
        }
        else
        {
            if (Loginpage.isSpeachEnabled == true)
            {
                set_balance_colours(true); // tells the function to change ethe color to normal(white or black)
                amount_displayed.Text = "£ " + results.ToString();
                await TextToSpeech.Default.SpeakAsync("the balance now is " + amount_displayed.Text);
            }
            else
            {
                set_balance_colours(true); // tells the function to change ethe color to normal(white or black)
                
                amount_displayed.Text = "£ " + results.ToString();
            }
        }

    }
    public void users_adding_things()
    {
        ObservableCollection<string> transaction_list = new ObservableCollection<string>(); //  creates a list that can be updated dynamically
        results_out_put = user_input.ToString();
        transaction_list.Add(results_out_put); // add the users input to the list 
        for (int i = 0; i < transaction_list.Count; i++) // loops through the list 
        {
            Label label = new Label()
            {
                Text = transaction_list[i].ToString() // creates a label with the transaction ammount 


            };
            transfers_stack.Children.Add(label); // adds  it to the stack layout called "transfers_stack" in the xml file
            if (adding == true)
            {
                label.BackgroundColor = Colors.Green; // if users added an amount to an transaction it becomes green 
                adding = false;
            }
            if (takeaway == true) // if user takes away an amount form the bank it becomes a red transaction
            {
                label.BackgroundColor = Colors.Red;
                takeaway = false;
            }


        }




    }
    public async Task Location_permsion()
    { // task allows the app to keep running while the code inside the function is still executing.  

        var permission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>(); // ask permission for using location and stores which permission it is. 

        if (permission == PermissionStatus.Granted) // checkes if permission for the location is granted 
        {

            var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High)); // gets the devices live location. 
            if (location == null) // checkes if the location is null 
            {

                await DisplayAlert("info", "can not get phones location", "ok");
            }
            else
            {
                ATM_map.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(location.Latitude, location.Longitude), Distance.FromKilometers(10))); // moves the map to the location that has been set in the (location settings on the phone)
                await retreving_ATM_info();

            }

        }
        else
        {
            await DisplayAlert("info","something went wrong","ok");

        }
    }

    public async Task retreving_ATM_info()
    {
        var client = new HttpClient();
        ATM_map.Pins.Clear();
        string query = $"[out:json][timeout:25];node[\"amenity\"=\"atm\"](51.28,-0.51,51.70,0.33);out 50;"; // the quarry that returns ATMS in a JSON format with a timeout of 25  and that will find a maximum of  50 ATMs that are in the London area 
        string url = $"https://overpass-api.de/api/interpreter?data={Uri.EscapeDataString(query)}"; // this the endpoint that is used get the ATMs(from open streetmaap)
        for (int i = 0; i < 5; i++)
        {

            try
            {

                json = await client.GetStringAsync(url); // if waits for the API to  return the 50 ATMS 
                if (!string.IsNullOrEmpty(json)) // if the API returned empty it breaks the loop and try again
                {
                    break;
                }
            }
            catch (Exception)
            {
                if (i == 4) // if its on its 4 attempt then it just throws this message to the users
                {
                    await DisplayAlert("Debug", "There is no near by ATMs. please try again next time", "OK");
                    throw;
                }
            }
        }

        var doc = JsonDocument.Parse(json); // gets json response from the API
        var elements = doc.RootElement.GetProperty("elements"); // finds the property that has the the ATM results 

        foreach (var element in elements.EnumerateArray()) // loops through the ATM results one at a time and gets the lat and lon property. 
        {

            double lat = element.GetProperty("lat").GetDouble();
            double lon = element.GetProperty("lon").GetDouble();
            ATM_Structure ATM = new ATM_Structure(lat, lon, "ATM"); // creates a object of atm with the lat and lon 


            var pin = new Pin // creating the pin for with the location being the lat and lon and give it a name of ATM
            {
                Label = ATM.Name,
                Location = new Location(ATM.Lat, ATM.Lon)

            };
            ATM_map.Pins.Add(pin); // adding the pin to the map , so that it can be shown to the user  

        }
        if (Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("number of ATMs near you" + ATM_map.Pins.Count); // tells the user the number of pins near them
        }
        else
        {
            await DisplayAlert("Debug", $"number of ATMs near you {ATM_map.Pins.Count}", "OK"); // tells the user the number of pins near them
        }
       
    }



    public void set_balance_colours(bool theme)
    {

        if (theme == false) {

            if (Application.Current.RequestedTheme == AppTheme.Dark) // when the balance goes negative change the colour(red) corsponding to dark or light theme 
            {
                amount_displayed.TextColor = (Color)Application.Current.Resources["ExpenseDark"]; // on dark theme its this red colour 
         
            }
            else
            {
                amount_displayed.TextColor = (Color)Application.Current.Resources["ExpenseLight"]; // on light mode its this red colour 
             
            }

        }
        else
        { // go back to the original colors of black and white if its positive numbers 
            if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                amount_displayed.TextColor = (Color)Application.Current.Resources["TextDark"]; // text becomes black 
            }
            else
            {
                amount_displayed.TextColor = (Color)Application.Current.Resources["TextLight"]; // text becomes white 
            }


        }
    }

}
