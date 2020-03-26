using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Tambola
{
    public partial class MainPage : ContentPage
    {
        private List<int> opened = new List<int>();
        Random random = new Random();
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            if (opened.Count() < 90)
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
            }
            ((Button)sender).IsEnabled = true;
        }

        private int generaterandomuniquenumber()
        {
            int newrandom = 0;
            do
            {
                newrandom = random.Next(1, 91);
                if (opened.Contains(newrandom) && newrandom == 0)
                    continue;

            } while (opened.Contains(newrandom));
            opened.Add(newrandom);
            return newrandom;
        }

        private async void ResetGame_Clicked(object sender, EventArgs e)
        {
            var action = await DisplayAlert("Reset ?", "Game will start from begining.", "Ok", "Cancel");

            if (action)
            {
                opened = new List<int>();
                for (int i = 1; i <= 90; i++)
                {
                    var ButtonName = "Lbl" + i;
                    Label lbl = (Label)FindByName(ButtonName);
                    lbl.SetDynamicResource(VisualElement.StyleProperty, "plainButton");
                }
                lblCurrentNumber.Text = "100";
                lblOldNumber.Text = "00";
            }
        }
    }
}
