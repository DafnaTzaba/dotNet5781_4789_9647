﻿namespace BO
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
        public bool UserExist { get; set; }
        public string MailAddress { get; set; }
    }
}
