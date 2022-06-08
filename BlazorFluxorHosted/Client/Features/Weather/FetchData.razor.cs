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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            LoadForecasts();
        }

        private void LoadForecasts()
        {
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
