using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.College.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClgBirthdayServiceHub.com.vaps.Interfaces
{
    public interface ClgBirthdayInterface
    {
        ClgBirthDayDTO getloaddata(ClgBirthDayDTO data);
        Task<ClgBirthDayDTO> radiochange(ClgBirthDayDTO data);
        Task<ClgBirthDayDTO> sendmsg(ClgBirthDayDTO data);
        ClgBirthDayDTO staflist(ClgBirthDayDTO data);
        void clg_getBirthday(int stu1);
    }
}
