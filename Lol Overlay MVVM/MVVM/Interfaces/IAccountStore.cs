using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{
    public interface IAccountStore
    {
        Task AddAccountAsync(string username, string password);
        Task RemoveAccountAsync(string username);
        Task<Dictionary<string, string>> GetAccountsAsync();
    }
}