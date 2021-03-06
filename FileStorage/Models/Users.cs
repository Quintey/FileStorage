﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileStorage.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
    }
}