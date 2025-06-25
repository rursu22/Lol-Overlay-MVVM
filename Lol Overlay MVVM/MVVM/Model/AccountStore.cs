using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lol_Overlay_MVVM.MVVM.Model
{

    public class AccountStore : IAccountStore
    {
        private readonly IEncryptionService _encryptionService;
        private readonly string _filePath; 
        public AccountStore(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accounts.json");
        }

        public async Task AddAccountAsync(string username, string password)
        {
            var accounts = await LoadAsync();
            if (accounts.ContainsKey(username))
                throw new ArgumentException("Account already exists", nameof(username));

            accounts[username] = _encryptionService.Encrypt(password);
            await SaveAsync(accounts);
        }

        public async Task RemoveAccountAsync(string username)
        {
            var accounts = await LoadAsync();
            if (accounts.Remove(username))
                await SaveAsync(accounts);
        }

        public Task<Dictionary<string, string>> GetAccountsAsync()
            => LoadAsync();

        private async Task<Dictionary<string, string>> LoadAsync()
        {
            if (!File.Exists(_filePath))
                return new Dictionary<string, string>();

            try
            {
                await using var stream = File.OpenRead(_filePath);
                var dict = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
                return dict ?? new Dictionary<string, string>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to load accounts: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }

        private async Task SaveAsync(Dictionary<string, string> accounts)
        {
            try
            {
                await using var stream = File.Create(_filePath);
                await JsonSerializer.SerializeAsync(stream, accounts, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to save accounts: {ex.Message}");
            }
        }
    }
}