using static BlazorFluxorHosted.Client.Features.Counter.Store;

namespace BlazorFluxorHosted.Client.Features.Counter
{
    public partial class Counter
    {
        [Inject]
        private IState<CounterState> CounterState { get; set; }

        [Inject]
        public IDispatcher Dispatcher { get; set; }

        public int Count => CounterState.Value.ClickCount;
        private void IncrementCount()
        {
            var action = new IncrementCounterAction();
            Dispatcher.Dispatch(action);
        }
    }
}
