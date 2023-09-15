using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace TestTemplate6.Application.Tests.Helpers
{
    public class FakeLogger<T> : ILogger<T>
    {
        public List<string> LogEntriesInformation { get; } = new List<string>();

        public IDisposable BeginScope<TState>(TState state) => new FakeLoggingScope();
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            var result = formatter.Invoke(state, exception);
            LogEntriesInformation.Add(result);
        }
    }
}
