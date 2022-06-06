namespace BlazorFluxorHosted.Client.Features.Weather
{
    public partial class Forecasts
    {
        [Parameter]
        public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
    }
}
