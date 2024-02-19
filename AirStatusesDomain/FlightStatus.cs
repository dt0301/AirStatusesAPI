namespace AirStatusesDomain
{
    /// <summary>
    /// Перечисление FlightStatus представляет статус полета.
    /// </summary>
    public enum FlightStatus
    {
        /// <summary>
        /// Полет прибывает вовремя.
        /// </summary>
        InTime,

        /// <summary>
        /// Полет задерживается.
        /// </summary>
        Delayed,

        /// <summary>
        /// Полет отменен.
        /// </summary>
        Cancelled
    }

}
