using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class UserLoginDTO : CommonParamDTO
    {
        public long IVRMUL_Id { get; set; }
        public long IVRMRT_Id { get; set; }
        public string IVRMUL_UserName { get; set; }
        public string IVRMUL_Password { get; set; }
        public string IVRMUL_SecurityQns { get; set; }
        public string IVRMUL_Answer { get; set; }
        public int IVRMUL_ActiveFlag { get; set; }
        public long MI_Id { get; set; }
        public string IVRMUL_SuperAdminFlag { get; set; }
    }
}
