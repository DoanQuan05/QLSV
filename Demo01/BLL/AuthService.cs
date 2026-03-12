using Demo01.DAL;

namespace Demo01.BLL
{
    public class AuthService
    {
        private readonly AccountDal _dal = new AccountDal();

        public bool Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            return _dal.ValidateLogin(username.Trim(), password);
        }
    }
}





