namespace WebArt.Services
{
    internal interface IUserSessionService
    {
        void SetToken(string token);
        string GetToken();
    }
    public class UserSessionService : IUserSessionService
    {
        private string Token;
        public void SetToken(string token)
        {
            Token = token;
        }
        public string GetToken()
        {
            return Token;
        }
    }
}
