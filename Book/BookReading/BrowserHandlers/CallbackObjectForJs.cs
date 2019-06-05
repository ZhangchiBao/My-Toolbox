namespace BookReading.BrowserHandlers
{
    public class CallbackObjectForJs
    {
        public CallbackObjectForJs()
        {
        }

        public object DataContext { get; set; }

        public void execute(string methodName, params object[] parameter)
        {
            var contextType = DataContext.GetType();
            var method = contextType.GetMethod(methodName);
            method?.Invoke(DataContext, parameter);
        }
    }
}
