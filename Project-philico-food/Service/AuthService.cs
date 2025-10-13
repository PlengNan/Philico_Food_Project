using Project_philico_food.Db;
using Project_philico_food.Models;
using Project_philico_food.functions;

namespace Project_philico_food.Service
{
    internal static class AuthService
    {
        public static UsersModel CurrentUser { get; private set; }

        public static bool Login(string username, string password, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                error = "Please enter username and password.";
                return false;
            }

            var repo = new UsersDb();
            var aes = new AESEncryption();

            string encUser = aes.Encrypt(username.Trim());
            string encPass = aes.Encrypt(password.Trim());

            if (!repo.VerifyLogin(encUser, encPass, out var user))
            {
                error = string.IsNullOrWhiteSpace(repo.Err)
                    ? "Invalid username or password."
                    : repo.Err;
                return false;
            }

            CurrentUser = user;
            return true;
        }

        public static void Logout() => CurrentUser = null;
    }
}
