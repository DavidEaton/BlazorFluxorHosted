using static BlazorFluxorHosted.Client.Features.Weather.Store;

namespace BlazorFluxorHosted.Client.Features.Weather
{
    public partial class FetchData : ComponentBase, IDisposable
    {
        [Inject]
        private IState<WeatherState> WeatherState { get; set; }
        
        [Inject]
        public IDispatcher Dispatcher { get; set; }

        private IEnumerable<WeatherForecast> Forecasts => WeatherState.Value.Forecasts;

        private bool IsInitialized => WeatherState.Value.IsInitialized;
        private bool IsLoading => WeatherState.Value.IsLoading;

        protected override void OnInitialized()
        {
            Console.WriteLine($"OnInitialized! IsInitialized: {IsInitialized}, IsLoading: {IsLoading}");
            if (!IsInitialized)
            {
                LoadForecasts();
                Dispatcher.Dispatch(new SetIsInitializedAction());
            }
        }

        private void LoadForecasts()
        {
            Console.WriteLine($"LoadForecasts! IsInitialized: {IsInitialized}");
            Console.WriteLine($"LoadForecasts! IsLoading: {IsLoading}");

            Dispatcher.Dispatch(new FetchDataAction());
        }

        // Wire-up state
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
                WeatherState.StateChanged += StateChanged;
        }

        public void StateChanged(object sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            WeatherState.StateChanged -= StateChanged;
        }
    }
}
