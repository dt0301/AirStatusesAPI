using StatusAirControl.Exeptions;

namespace AirStatusesApp.Exeptions
{
    public class EntityNotfoundException : BaseException
    {
        public EntityNotfoundException(Exception ex, long entityId, string entityName) :
            base("entity-notfound", $"Entity notfound: {entityName}, Id: {entityId}, {ex.Message}")
        {
        }
    }
}
