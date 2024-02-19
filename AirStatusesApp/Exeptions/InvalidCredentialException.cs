namespace StatusAirControl.Exeptions
{
    public class InvalidCredentialException : BaseException
    {
        public InvalidCredentialException(Exception ex = null) :
            base("invalid-credential", $"You provided a invalid credential: {ex?.Message}")
        {
        }
    }
}
