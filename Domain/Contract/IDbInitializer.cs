﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public interface IDbInitializer
    {
        public Task InitiliazeAsync();
    }
}
