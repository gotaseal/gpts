using gpts.Services;

namespace gpts
{
    public partial class MainPage : ContentPage
    {
        OpenAIService openAIService;
        public MainPage(OpenAIService svc)
        {
            openAIService = svc;
            InitializeComponent();
        }
        private async void OnRestaurantClicked(object sender, EventArgs e)
        {
            await GetRecommendation("restaurant");
        }

        private async void OnHotelClicked(object sender, EventArgs e)
        {
            await GetRecommendation("hotel");
        }

        private async void OnAttractionClicked(object sender, EventArgs e)
        {
            await GetRecommendation("attraction");
        }

        private async Task GetRecommendation(string recommendationType)
        {
            if (string.IsNullOrWhiteSpace(LocationEntry.Text))
            {
                await DisplayAlert("你忘了輸入一個城市", "請輸入一個城市(也可以是國家)", "OK");
                return;
            }
            SmallLabel.Text = "還在處理中,請稍後";
            var message = await openAIService.CallOpenAI(recommendationType, LocationEntry.Text);
            SmallLabel.Text = message;
        }
    }

}