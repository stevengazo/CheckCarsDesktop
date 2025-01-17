using CheckCarsDesktop.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckCarsDesktop.Services
{
   class UserCredentialsStorage
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "userCredentials.json");

        public void SaveCredentials(UserCredentials credentials)
        {
            var json = JsonConvert.SerializeObject(credentials);
            File.WriteAllText(_filePath, json);
        }

        public UserCredentials LoadCredentials()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<UserCredentials>(json);
            }
            return null;
        }

        public void ClearCredentials()
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }
    }
}
