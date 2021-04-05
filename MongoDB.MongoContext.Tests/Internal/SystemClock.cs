using System;

namespace MongoDB.MongoContext.Tests
{
    public sealed class SystemClock : IClock
    {
        private const long TicksPerMillisecond = 10_000;

        public static readonly SystemClock Instance = new();

        /// <summary>
        /// Returns the current system time in UTC with milliseconds precision, flooring the additional precision
        /// provided by ticks.
        /// </summary>
        public DateTime UtcNow
        {
            get
            {
                var ticks = DateTime.UtcNow.Ticks;
                var totalMilliseconds = ticks / TicksPerMillisecond;
                var ticksWithMillisPrecision = totalMilliseconds * TicksPerMillisecond;
                return new DateTime(ticksWithMillisPrecision, DateTimeKind.Utc);
            }
        }
    }
}