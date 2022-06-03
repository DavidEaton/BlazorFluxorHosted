namespace BlazorFluxorHosted.Client.Features.Counter
{
    public class Store
    {
        [FeatureState]
        public record CounterState
        {
            public int ClickCount { get; init; }
        }

        public class IncrementCounterAction { }

        public static class Reducers
        {
            [ReducerMethod(typeof(IncrementCounterAction))]
            public static CounterState ReduceIncrementCounterAction(CounterState state)
            {
                // Counter.razor.cs IncrementCount method Dispatches new IncrementCounterAction
                // Dispatcher calls this Reducer method to create a new state from existing state
                // New state.ClickCount value will be the sum of existing state + 1:
                Console.WriteLine($"ReduceIncrementCounterAction! state.ClickCount: {state.ClickCount}");
                return state with
                {
                    ClickCount = state.ClickCount + 1
                };
            }
        }
    }
}
