using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace assessment2526;

public partial class Budgets : ContentPage
{
    string regex = "^[a-zA-Z\\s]*$"; // a regex
    string title_info = "";
    string title_finder_info = "";
    bool budgetcreated = false;
    bool budgetupdated = false;
    List<Budget_structure> budget = new List<Budget_structure>(); // creating a budget list with the budget class
    public Budgets()
    {
        InitializeComponent();
        load_username(); // the users name on screen and if text to speech is on says the user name outload 


    }


    protected override void OnAppearing()  // built in maui function that runs every time the users goes to this page 
    {
        base.OnAppearing(); // runs the original on appearing before runs the if statement 
        if (Loginpage.isSpeachEnabled == true)
        {
            TextToSpeech.Default.SpeakAsync("this the budgeting page");
        }
    }
    
    public async void load_username() // displays the users username that is used for login 
    {
        string name = Loginpage.user_info_display_name;
        dsiplying_name.Text = " Welcome " + name;
        if (Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("welcome " + name);
        }

    }
    private async void logout_button(object sender, EventArgs e) // when user presss logout button this function sends them back to the loginpage.
    {
        await Shell.Current.GoToAsync("//loginpage");
    }


    private async void budget_section_display(object sender, EventArgs e) 
    {
        title_info = budget_Title.Text;
        if (!Regex.IsMatch(title_info, regex) && Loginpage.isSpeachEnabled == true) //checks if title of the budget does not match the regex and text to speech is enabled 
        {
            await TextToSpeech.Default.SpeakAsync("this DID NOT match: " + title_info + "please make a budget that is just letters and no number");
            return;
        }
        if (Regex.IsMatch(title_info, regex))
        {
            creating_the_budget();
            return;
        }
        else
        {
            await DisplayAlert("Info", "this DID NOT match: " + title_info + "please make a budget that is just letters and no number", "OK");

        }
    }
    private async void OnBudgetPanelSwiped(object sender, SwipedEventArgs e)
    {
        double screenWidth = this.Width;
        BudgetPanel.IsVisible = false;
        BlankPanel.IsVisible = true;
        await BudgetPanel.TranslateTo(-screenWidth, 0, 300); // moves left, with a 300ms duration

        if (Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("you are on the add amount to an existing budget, swipe lef to  creating a new budget");
        }
    }
    private async void UpdatingBudgetPanelSwiped(object sender, SwipedEventArgs e)
    {
        double screenWidth = this.Width;
        BlankPanel.IsVisible = false;
        BudgetPanel.TranslationX = screenWidth; // moves right depending on how many pixels the screen has moved 
        BudgetPanel.IsVisible = true;

        await BudgetPanel.TranslateTo(0, 0, 300); // moves right with a  duration of 300ms  


        if (Loginpage.isSpeachEnabled == true)
        {
            await TextToSpeech.Default.SpeakAsync("you are on the creating budget section, swipe right to add amount to an existing budget");
        }
    }
    private async void adding_things_to_budget_button(object sender, EventArgs e)
    {
        adding_things_to_budget(); // goes to the adding things budget
    }
    public void creating_the_budget() // creates a new budget object with title , budget amount and added amount of 0
    {
        double amount = double.Parse(budget_amount.Text);
        Budget_structure b = new Budget_structure(title_info.ToLower(), amount, 0);
        budget.Add(b);
        budgetcreated = true;
        display_on_screen();
    }
    public async void adding_things_to_budget() // adds an amount for an existing 
    {
        double amount_add = double.Parse(adding_to_budget.Text);

        foreach (Budget_structure b in budget) // loops existing budgets and checks if it matches users input
        {

            if (b.Title == budget_Title_finder.Text.ToLower()) // updates the budgets progress
            {
                budgetupdated = true;
                b.Added += amount_add;

            }

        }
        if (budgetupdated == true)
        {
            display_on_screen(); // displays the new updated budget 
        }
        else
        {
            await DisplayAlert("INFO", "Please input an existing budget", "OK"); 
        }
    }

    public void display_on_screen() // displays the new created budget and the existing budget. 
    {
        Iteam.Children.Clear(); // clears the  VerticalStackLayout 

        foreach (Budget_structure b in budget) // loops through all the budget created
        {
            double progress = b.Added / b.Total; // calculates the progress 

            double progress_display = Math.Round(progress * 100, 2); // makes the progress into a percentage 

            ProgressBar progressBar = new ProgressBar() // creates a new progress bar with the progress indication in red 
            {

                Progress = progress,
                ProgressColor = Colors.Red,

            };

            Label label = new Label() // creates a new label with the title of the budget and the progress precentage 
            {

                Text = $"{b.Title} {b.Total.ToString()} {progress_display.ToString() + "%"}",
                TextColor = Colors.Black,
            };
            HorizontalStackLayout rowlayout = new HorizontalStackLayout() // provides space between each new created budget. 
            {
                Spacing = 10,
            };
            // adds the label and progressBar first to the horizontal stack  before adding it to the main page(Iteam).
            rowlayout.Children.Add(label); 
            rowlayout.Children.Add(progressBar);
            Iteam.Children.Add(rowlayout);


        }
        foreach (Budget_structure b2 in budget) // loops through the existing budgets and and checks if the budget created or budget updated is true with speech enabled 
        {

            if (Loginpage.isSpeachEnabled == true && budgetcreated == true)
            {
                double precent_for_speach = Math.Round((b2.Added / b2.Total) * 100, 2);
                TextToSpeech.Default.SpeakAsync("created " + title_info + "budget" + precent_for_speach + "%");
                budgetcreated = false;

            }
            if (budgetupdated == true && Loginpage.isSpeachEnabled == true)
            {

                title_finder_info = budget_Title_finder.Text;

                if (b2.Title == title_finder_info.ToLower()) // checks if the title found match the original one. 
                {
                    double precent_for_speach = Math.Round((b2.Added / b2.Total) * 100, 2);
                    TextToSpeech.Default.SpeakAsync("budget " + title_finder_info + " updated to " + precent_for_speach + "%");
                    precent_for_speach = 0;
                    budgetupdated = false;
                }




            }
        }
    }
}

     