using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class UserLoginEmployeeDTO : CommonParamDTO
    {
        public long IVRMULF_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public long MI_Id { get; set; }
        public long Emp_Code { get; set; }
    }
}
