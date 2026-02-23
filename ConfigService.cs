using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace StayAwake
{
    public class VpnConfig
    {
        public string ProfileName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string EncryptedPassword { get; set; } = string.Empty;

        public string GetPassword()
        {
            if (string.IsNullOrEmpty(EncryptedPassword)) return string.Empty;
            try
            {
                byte[] encryptedData = Convert.FromBase64String(EncryptedPassword);
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SetPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                EncryptedPassword = string.Empty;
                return;
            }
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] encryptedData = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            EncryptedPassword = Convert.ToBase64String(encryptedData);
        }
    }

    public static class ConfigService
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vpn_config.json");

        public static void SaveConfig(VpnConfig config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }

        public static VpnConfig LoadConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                return new VpnConfig();
            }

            try
            {
                string json = File.ReadAllText(ConfigPath);
                return JsonSerializer.Deserialize<VpnConfig>(json) ?? new VpnConfig();
            }
            catch
            {
                return new VpnConfig();
            }
        }
    }
}
