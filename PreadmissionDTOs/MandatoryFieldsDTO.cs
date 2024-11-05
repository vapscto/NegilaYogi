using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MandatoryFieldsDTO : CommonParamDTO
    {
        public long IVRM_MAND_Id { get; set; }

        public long IVRMP_Id { get; set; }

        public string IVRM_MAND_FIELDS { get; set; }

        public long MI_Id { get; set; }
        public long MO_Id { get; set; }
        public Array mandatoryFieldList { get; set; }
    }
}
