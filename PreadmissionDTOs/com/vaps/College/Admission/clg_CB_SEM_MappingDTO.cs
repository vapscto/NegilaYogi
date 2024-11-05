using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class clg_CB_SEM_MappingDTO : CommonParamDTO
    {
        public Array semlist { get; set; }
        public Array coursebranchlist { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public long AMCO_Id { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMCOBM_Id { get; set; }
        public Array courselist { get; set; }
        public ClgYearWiseStudentDTO[] resultData1 { get; set; }

        public ClgAdmTempDTO[] selectedsem { get; set; }
        public long ACMAY_Id { get; set; }
        public long MI_Id { get; set; }

        public long AMSE_Id { get; set; }
        public long? AMCOC_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACYST_RollNo { get; set; }

        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }

        public string AMCST_AdmNo { get; set; }
        public long ACYST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string ACMAY_AcademicYear { get; set; }
        public bool AYST_PassFailFlag { get; set; }
        public bool ACYST_ActiveFlag { get; set; }
        public long LoginId { get; set; }

        public DateTime? ACYST_DateTime { get; set; }

        public long ACAYC_Id { get; set; }
        public long ACAYCB_Id { get; set; }

        public DateTime? ACAYCBS_SemEndDate { get; set; }
        public string AMSE_EvenOdd { get; set; }

        public Array prosemlist { get; set; }
        public Array promoyear { get; set; }


        public bool AMCOBMS_ActiveFlg { get; set; }
        public long AMCOBMS_Id { get; set; }

        public string returnduplicatestatus { get; set; }
        public Array griddata { get; set; }
        public Array editgriddata { get; set; }
        public Array semdetails { get; set; }

          public bool returnval { get; set; }
    }
}
