using BlazorFluxorHosted.Client.Services;

namespace BlazorFluxorHosted.Client.Features.Weather
{
    public class Store
    {
        [FeatureState]
        public record WeatherState
        {
            public bool IsLoading { get; init; } = default;
            public IEnumerable<WeatherForecast> Forecasts { get; init; }
            private WeatherState() { } // Initialize the state properties
            public WeatherState(IEnumerable<WeatherForecast> forecasts)
            {
                Forecasts = forecasts ?? Array.Empty<WeatherForecast>();
            }
        }

        public class FetchDataAction { }
        public class FetchDataResultAction
        {
            public IEnumerable<WeatherForecast> Forecasts { get; }

            public FetchDataResultAction(IEnumerable<WeatherForecast> forecasts)
            {
                Forecasts = forecasts;
            }
        }

        public class SetForecastsAction
        {
            public IEnumerable<WeatherForecast> Forecasts { get; }

            public SetForecastsAction(IEnumerable<WeatherForecast> forecasts)
            {
                Forecasts = forecasts;
            }
        }

        public class SetIsLoadingAction
        {
            public bool IsLoading { get; }

            public SetIsLoadingAction(bool isLoading)
            {
                IsLoading = isLoading;
            }
        }
        public static class Reducers
        {
            // Tip: Do not inject dependencies into reducers!
            // Reducers should ideally be pure functions; if you find a need
            // to inject dependencies into a reducer then you might be taking
            // the wrong approach, and should instead be using an Effect.

            [ReducerMethod]
            public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataAction action)
            {
                return state with
                {
                    IsLoading = true,
                    Forecasts = null
                };
            }

            [ReducerMethod]
            public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action)
            {
                return state with
                {
                    IsLoading = false,
                    Forecasts = action.Forecasts
                };
            }





            [ReducerMethod]
            public static WeatherState ReduceSetForecastsAction(WeatherState state, SetForecastsAction action)
            {
                return state with
                {
                    Forecasts = action.Forecasts
                };
            }

            [ReducerMethod]
            public static WeatherState ReduceSetIsLoadingAction(WeatherState state, SetIsLoadingAction action)
            {
                return state with
                {
                    IsLoading = action.IsLoading
                };
            }
        }

        public class Effects
        {
            // Since a Reducer cannot call out to an API, we need Effect methods to
            // do that.

            // Effect handlers cannot (and should not) affect state directly. They
            // are triggered when the action they are interested in is dispatched
            // through the store, and as a response they can dispatch new actions.

            // Similar to the Reducers class but with one big and obvious difference:
            // Effects class has a constructor into which we can inject dependencies,
            // and then use in each EffectMethod:

            private readonly IWeatherForecastService WeatherForecastService;
            public Effects(IWeatherForecastService weatherForecastService)
            {
                WeatherForecastService = weatherForecastService;
            }

            [EffectMethod(typeof(FetchDataAction))]
            public async Task HandleFetchDataAction(IDispatcher dispatcher)
            {
                await Task.Delay(1000);
                var forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
                dispatcher.Dispatch(new FetchDataResultAction(forecasts));
            }

            // NO LONGER USED
            [EffectMethod(typeof(FetchDataAction))]
            public async Task LoadForecasts(IDispatcher dispatcher)
            {
                dispatcher.Dispatch(new SetIsLoadingAction(true));

                // Simulate-ish slow connection (to view loading spinner in development.
                await Task.Delay(3000);

                IEnumerable<WeatherForecast>? forecasts =
                    await WeatherForecastService.GetForecastAsync(DateTime.Now);

                if (forecasts is not null)
                {
                    dispatcher.Dispatch(new SetForecastsAction(forecasts));
                    dispatcher.Dispatch(new SetIsLoadingAction(false));
                }
            }
        }
    }
}
