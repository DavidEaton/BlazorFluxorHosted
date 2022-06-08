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

        private bool isLoading => WeatherState.Value.IsLoading;

        protected override async Task OnInitializedAsync()
        {
            if (WeatherState.Value.IsInitialized == false)
            {
                LoadForecasts();
                Dispatcher.Dispatch(new SetIsInitializedAction(true));
            }
        }

        private void LoadForecasts()
        {
            Dispatcher.Dispatch(new LoadForecastsAction());
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
