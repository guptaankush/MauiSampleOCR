using Plugin.Maui.OCR;

namespace MAUISampleOCR
{
    public partial class MainPage : ContentPage
    {
        private readonly IOcrService _ocrService;

        public MainPage(IOcrService ocrService)
        {
            _ocrService = ocrService ?? OcrPlugin.Default;
            InitializeComponent();
        }

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    await OcrPlugin.Default.InitAsync();
        //}

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                var pickResult = await MediaPicker.Default.PickPhotoAsync();
                await CommonMethod(pickResult);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "Okay");
            }
        }

        private async void CounterBtn2_Clicked(object sender, EventArgs e)
        {
            try
            {
                var pickResult = await MediaPicker.Default.CapturePhotoAsync();
                await CommonMethod(pickResult);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Exception", ex.Message, "Okay");
            }
        }

        private async Task CommonMethod(FileResult? pickResult)
        {
            if (pickResult != null)
            {
                using var imageAsStream = await pickResult.OpenReadAsync();
                var imageAsByteArr = new byte[imageAsStream.Length];
                await imageAsStream.ReadAsync(imageAsByteArr);

                var result = await _ocrService.RecognizeTextAsync(imageAsByteArr, true);

                if (result.Success)
                    await DisplayAlert("OCR Result", result.AllText, "Okay");
                else
                    await DisplayAlert("No success", "No OCR possible", "OK");
            }
        }
    }

}
