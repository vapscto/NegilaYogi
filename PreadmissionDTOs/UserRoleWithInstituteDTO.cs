using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class UserRoleWithInstituteDTO : CommonParamDTO
    {
        public long IVRMULI_Id { get; set; }
        public long MI_Id { get; set; }
        public long Id { get; set; }

        public int Activeflag { get; set; } 
    }
}
