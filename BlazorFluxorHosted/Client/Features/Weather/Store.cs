namespace BlazorFluxorHosted.Client.Features.Weather
{
    public class Store
    {
        [FeatureState]
        public record WeatherState
        {
            public bool Initialized { get; init; }
            public bool IsLoading { get; init; }
            public IEnumerable<WeatherForecast> Forecasts { get; init; }
        }

        public class SetInitializedAction { }
        public class LoadForecastsAction { }
        public class SetForecastsAction
        {
            public IEnumerable<WeatherForecast> Forecasts { get; }

            public SetForecastsAction(IEnumerable<WeatherForecast> forecasts)
            {
                Forecasts = forecasts;
            }
        }

        public class SetLoadingAction
        {
            public bool Loading { get; }

            public SetLoadingAction(bool loading)
            {
                Loading = loading;
            }
        }
        public static class Reducers
        {
            // Tip: Do not inject dependencies into reducers!
            // Reducers should ideally be pure functions; if you find a need
            // to inject dependencies into a reducer then you might be taking
            // the wrong approach, and should instead be using an Effect.

            [ReducerMethod]
            public static WeatherState ReduceSetForecastsAction(WeatherState state, SetForecastsAction action)
            {
                return state with
                {
                    Forecasts = action.Forecasts
                };
            }

            [ReducerMethod]
            public static WeatherState ReduceSetLoadingAction(WeatherState state, SetLoadingAction action)
            {
                return state with
                {
                    IsLoading = action.Loading
                };
            }

            [ReducerMethod(typeof(SetInitializedAction))]
            public static WeatherState ReduceSetInitializedAction(WeatherState state)
            {
                return state with
                {
                    Initialized = true
                };
            }
        }

        public class Effects
        {
            // Since a Reducer cannot call out to an API,
            // we need Effect methods to do it.

            // Effect handlers cannot (and should not) affect state directly. They
            // are triggered when the action they are interested in is dispatched
            // through the store, and as a response they can dispatch new actions.

            private readonly HttpClient Http;

            // Similar to the Reducers class but with one big and obvious difference:
            // Effects class has a constructor into which we can inject dependencies,
            // and then use in each EffectMethod:
            public Effects(HttpClient http)
            {
                Http = http;
            }

            [EffectMethod(typeof(LoadForecastsAction))]
            public async Task LoadForecasts(IDispatcher dispatcher)
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                IEnumerable<WeatherForecast>? forecasts = await CallForecasts();
                dispatcher.Dispatch(new SetForecastsAction(forecasts));
                dispatcher.Dispatch(new SetLoadingAction(false));
            }

            private async Task<IEnumerable<WeatherForecast>> CallForecasts()
            {
                Console.WriteLine("CallForecasts!");

                try
                {
                    var casts = await Http.GetFromJsonAsync<IEnumerable<WeatherForecast>>("WeatherForecast");

                    return casts;

                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
    }
}
