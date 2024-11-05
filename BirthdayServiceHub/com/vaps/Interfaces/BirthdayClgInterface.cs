﻿using PreadmissionDTOs.com.vaps.College.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayServiceHub.com.vaps.Interfaces
{
    public interface BirthdayClgInterface
    {
        ClgBirthDayDTO getloaddata(ClgBirthDayDTO data);
        Task<ClgBirthDayDTO> radiochange(ClgBirthDayDTO data);
        Task<ClgBirthDayDTO> sendmsg(ClgBirthDayDTO data);
        ClgBirthDayDTO staflist(ClgBirthDayDTO data);
        void clg_getBirthday(int stu1);
    }
}
