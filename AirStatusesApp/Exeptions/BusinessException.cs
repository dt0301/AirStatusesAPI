namespace StatusAirControl.Exeptions
{
    public class BaseException : Exception
    {
        public string Token { get; set; }
        public BaseException(string token, string message) : base(message)
        {
            this.Token = token;
        }

        public BaseException(string token, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Token = token;
        }
    }
}
