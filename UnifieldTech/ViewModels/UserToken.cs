using System;

namespace UnifieldTech.ViewModels
{
    public class UserToken
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
