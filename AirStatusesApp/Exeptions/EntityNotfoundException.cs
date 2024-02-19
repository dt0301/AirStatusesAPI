using StatusAirControl.Exeptions;

namespace AirStatusesApp.Exeptions
{
    /// <summary>
    /// Исключение, которое возникает, когда сущность не найдена.
    /// </summary>
    public class EntityNotfoundException : BaseException
    {
        /// <summary>
        /// Конструктор исключения, который принимает внутреннее исключение, идентификатор сущности и имя сущности.
        /// </summary>
        /// <param name="ex">Внутреннее исключение.</param>
        /// <param name="entityId">Идентификатор сущности.</param>
        /// <param name="entityName">Имя сущности.</param>
        public EntityNotfoundException(Exception ex, long entityId, string entityName) :
            base("entity-notfound", $"Сущность не найдена: {entityName}, Id: {entityId}, {ex.Message}")
        {
        }
    }
}

