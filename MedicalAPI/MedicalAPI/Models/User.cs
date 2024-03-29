﻿using System;

namespace MedicalAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string HashedPassword { get; set; }

        public byte[] Salt { get; set; }

        public Guid RefreshToken { get; set; }

        public DateTime RefreshTokenExp { get; set; }
    }
}
