using System;

namespace MedicalAPI.DTOs
{
    public class TokensInfo
    {
        public string AccessToken { get; set; }

        public Guid RefreshToken { get; set; }
    }
}
