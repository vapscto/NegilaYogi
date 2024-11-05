using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class PreadmissionSchoolRegistrationDocumentsDTO : CommonParamDTO
    {
        public long PASRD_Id { get; set; }
        public long PASR_Id { get; set; }
        public long AMSMD_Id { get; set; }
        public string Document_Path { get; set; }

        public string AMSMD_DocumentName { get; set; }

        public bool AMSMD_FLAG { get; set; }

    }
}
