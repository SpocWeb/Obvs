using System;
using System.Diagnostics;

namespace Obvs.Monitoring.PerformanceCounters
{
    internal static class Data
    {
        public const string CategoryName = "Obvs Monitoring";
        public const string CategoryDescription = "Monitoring for Obvs endpoints.";

        public const string MessagesSent = "Messages Sent Count";
        public const string MessagesSentDescription = "Count of messages sent";
        public const string MessagesReceived = "Messages Received Count";
        public const string MessagesReceivedDescription = "Count of messages received";

        public const string MessagesSentRate = "Messages Sent Rate";
        public const string MessagesSentRateDescription = "Rate of messages sent";
        public const string MessagesReceivedRate = "Messages Received Rate";
        public const string MessagesReceivedRateDescription = "Rate of messages received";

        public const string MessagesSentAverage = "Messages Sent Average";
        public const string MessagesSentAverageDescription = "Average Average time of messages sent";
        public const string MessagesSentAverageBase = "Messages Sent Average Base";
        public const string MessagesSentAverageBaseDescription = "Average time of messages sent base";

        public const string MessagesReceivedAverage = "Messages Received Average";
        public const string MessagesReceivedAverageDescription = "Average Average time of messages received";
        public const string MessagesReceivedAverageBase = "Messages Received Average Base";
        public const string MessagesReceivedAverageBaseDescription = "Average time of messages received base";
    }

    public class PerformanceCounterMonitorFactory<TMessage> : IMonitorFactory<TMessage>
    {
        public PerformanceCounterMonitorFactory()
        {
            try
            {
                if (!PerformanceCounterCategory.Exists(Data.CategoryName))
                {
                    var counters = new CounterCreationDataCollection
                    {
                        new CounterCreationData(Data.MessagesSent, Data.MessagesSentDescription, PerformanceCounterType.NumberOfItems32),
                        new CounterCreationData(Data.MessagesReceived, Data.MessagesReceivedDescription, PerformanceCounterType.NumberOfItems32),
                        new CounterCreationData(Data.MessagesSentRate, Data.MessagesSentRateDescription, PerformanceCounterType.RateOfCountsPerSecond32),
                        new CounterCreationData(Data.MessagesReceivedRate, Data.MessagesReceivedRateDescription, PerformanceCounterType.RateOfCountsPerSecond32),
                        new CounterCreationData(Data.MessagesSentAverage, Data.MessagesSentAverageDescription, PerformanceCounterType.AverageTimer32),
                        new CounterCreationData(Data.MessagesSentAverageBase, Data.MessagesSentAverageBaseDescription, PerformanceCounterType.AverageBase),
                        new CounterCreationData(Data.MessagesReceivedAverage, Data.MessagesReceivedAverageDescription, PerformanceCounterType.AverageTimer32),
                        new CounterCreationData(Data.MessagesReceivedAverageBase, Data.MessagesReceivedAverageBaseDescription, PerformanceCounterType.AverageBase)
                    };
                    PerformanceCounterCategory.Create(Data.CategoryName, Data.CategoryDescription, PerformanceCounterCategoryType.MultiInstance, counters);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        public IMonitor<TMessage> Create(string name)
        {
            return new PerformanceCounterMonitor<TMessage>(name);
        }
    }
}