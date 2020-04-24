using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Tambola
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            if(App.opened.Count > 0)
            {
                foreach (int number in App.opened)
                {
                    var ButtonName = "Lbl" + number;
                    Label lblname = (Label)FindByName(ButtonName);
                    lblname.SetDynamicResource(VisualElement.StyleProperty, "orangeButton");
                }
                if (App.opened.Count >= 2)
                    lblOldNumber.Text = App.opened[App.opened.Count - 2].ToString();
                else
                    lblOldNumber.Text = "100";
                lblCurrentNumber.Text = App.opened.Last().ToString();

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            if (App.opened.Count() < 90)
            {
                lblOldNumber.Text = lblCurrentNumber.Text;
                int nextnumber = generaterandomuniquenumber();
                lblCurrentNumber.Text = nextnumber.ToString();

                SpeechOptions options = new SpeechOptions(){
                    Pitch = 0.7f
                };
                string speektext = "Number is" + nextnumber;

                await TextToSpeech.SpeakAsync(speektext, options);
               
                var ButtonName = "Lbl" + nextnumber;
                Label lblname = (Label)FindByName(ButtonName);
                lblname.SetDynamicResource(VisualElement.StyleProperty, "orangeButton");
            }
            else
            {
                await DisplayAlert("Message", "Game Done, All numbers Called", "Ok");
                reset();
            }
            ((Button)sender).IsEnabled = true;
        }

        private int generaterandomuniquenumber()
        {
            int newrandom = 0;
            do
            {
                newrandom = App.random.Next(1, 91);
                if (App.opened.Contains(newrandom) && newrandom == 0)
                    continue;

            } while (App.opened.Contains(newrandom));
            App.opened.Add(newrandom);
            return newrandom;
        }

        private async void ResetGame_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("Reset ?", "Game will start from begining.", "Ok", "Cancel");

            if (action)
            {
                reset();
            }
        }

        private void reset()
        {
            App.opened = new List<int>();
            for (int i = 1; i <= 90; i++)
            {
                var ButtonName = "Lbl" + i;
                Label lbl = (Label)FindByName(ButtonName);
                lbl.SetDynamicResource(VisualElement.StyleProperty, "plainButton");
            }
            lblCurrentNumber.Text = "100";
            lblOldNumber.Text = "00";
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            int label = Convert.ToInt32(((Label)sender).Text);

            if(App.opened.Contains(label))
            {
                int index = App.opened.IndexOf(label);
                await DisplayAlert("Detail","Number was called at " + (index + 1).ToString() + " Position" , "Ok");
            }
            else
            {
                await DisplayAlert("Detail", "Number didn't called", "Ok");
            }
        }
    }
}
