﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface registration
    {
        Task<string> regdata(regis id);
        Task<bool> getregdata(string username);
    
    }
}