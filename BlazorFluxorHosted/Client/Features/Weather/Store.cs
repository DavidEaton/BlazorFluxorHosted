using BlazorFluxorHosted.Client.Services;

namespace BlazorFluxorHosted.Client.Features.Weather
{
    // Comments taken from Peter Morris, Fluxor author:
    // https://github.com/mrpmorris/Fluxor/blob/master/Docs/README.md
    public class Store
    {
        // THE STATE
        [FeatureState]
        public record WeatherState
        {
            public bool IsLoading { get; init; } = default;
            public bool IsInitialized { get; init; } = default;
            public IEnumerable<WeatherForecast> Forecasts { get; init; }
            private WeatherState() { } // Initialize the state properties 
            public WeatherState(IEnumerable<WeatherForecast> forecasts)
            {
                Forecasts = forecasts ?? Array.Empty<WeatherForecast>();
            }
        }

        // ACTIONS
        public class FetchDataAction
        {
            public FetchDataAction()
            {
                Console.WriteLine("Creating FetchDataAction! (Action)");
            }
        }
        public class FetchDataResultAction
        {
            public IEnumerable<WeatherForecast> Forecasts { get; }

            public FetchDataResultAction(IEnumerable<WeatherForecast> forecasts)
            {
                Console.WriteLine("Creating FetchDataResultAction! (Action)");
                Forecasts = forecasts;
            }
        }
        public class SetIsInitializedAction { }
        public class SetIsLoadingAction
        {
            public bool IsLoading { get; }

            public SetIsLoadingAction(bool isLoading)
            {
                Console.WriteLine($"Creating SetIsLoadingAction! (Action) isLoading: {isLoading})");
                IsLoading = isLoading;
            }
        }

        // REDUCERS
        public static class Reducers
        {
            // Reducers methods accept previous state + action to create and return new state

            // Reducers methods return state. Nothing else (like an Effect method) returns state.

            // Tip: Do not inject dependencies into reducers!
            // Reducers should ideally be pure functions; if you find a need
            // to inject dependencies into a reducer then you might be taking
            // the wrong approach, and should instead be using an Effect.

            [ReducerMethod]
            public static WeatherState ReduceFetchDataAction(WeatherState state, FetchDataAction action)
            {
                Console.WriteLine("ReduceFetchDataAction! (ReducerMethod)");
                return state with
                {
                    IsLoading = true,
                    Forecasts = null
                };
            }

            [ReducerMethod]
            public static WeatherState ReduceFetchDataResultAction(WeatherState state, FetchDataResultAction action)
            {
                Console.WriteLine("ReduceFetchDataResultAction! (ReducerMethod)");
                return state with
                {
                    IsLoading = false,
                    Forecasts = action.Forecasts
                };
            }

            [ReducerMethod]
            public static WeatherState ReduceSetIsLoadingAction(WeatherState state, SetIsLoadingAction action)
            {
                Console.WriteLine("ReduceSetIsLoadingAction! (ReducerMethod)");
                return state with
                {
                    IsLoading = action.IsLoading
                };
            }

            [ReducerMethod]
            public static WeatherState ReduceSetIsInitializedAction(WeatherState state, SetIsInitializedAction action)
            {
                Console.WriteLine("ReduceSetIsInitializedAction! (ReducerMethod)");
                return state with
                {
                    IsInitialized = true
                };
            }
        }

        // EFFECTS
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
            private readonly IDispatcher Dispatcher;

            public Effects(IWeatherForecastService weatherForecastService, IDispatcher dispatcher)
            {
                WeatherForecastService = weatherForecastService;
                Dispatcher = dispatcher;
            }

            [EffectMethod(typeof(FetchDataAction))]
            public async Task HandleFetchDataAction(IDispatcher dispatcher)
            {
                Console.WriteLine("HandleFetchDataAction! (EffectMethod)");
                Dispatcher.Dispatch(new SetIsLoadingAction(true));
                await Task.Delay(1000);
                var forecasts = await WeatherForecastService.GetForecastAsync(DateTime.Now);
                dispatcher.Dispatch(new FetchDataResultAction(forecasts));
            }

            // NO LONGER USED
            //[EffectMethod(typeof(FetchDataAction))]
            //public async Task LoadForecasts(IDispatcher dispatcher)
            //{
            //    dispatcher.Dispatch(new SetIsLoadingAction(true));

            //    // Simulate-ish slow connection (to view loading spinner in development.
            //    await Task.Delay(3000);

            //    IEnumerable<WeatherForecast>? forecasts =
            //        await WeatherForecastService.GetForecastAsync(DateTime.Now);

            //    if (forecasts is not null)
            //    {
            //        dispatcher.Dispatch(new SetForecastsAction(forecasts));
            //        dispatcher.Dispatch(new SetIsLoadingAction(false));
            //    }
            //}
        }
    }
}
