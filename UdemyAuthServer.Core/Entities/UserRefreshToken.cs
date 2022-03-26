﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAuthServer.Core.Entities
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string Code { get; set; } //RefreshToken adını code dedik
        public DateTime Expiration { get; set; }
    }
}
