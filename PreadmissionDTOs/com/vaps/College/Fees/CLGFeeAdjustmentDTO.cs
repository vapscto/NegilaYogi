using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CLGFeeAdjustmentDTO
    {
        public long FSA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime FSA_Date { get; set; }
        public long FSA_From_FMH_Id { get; set; }
        public long FSA_From_FMG_Id { get; set; }
        public long FSA_AdjustedAmount { get; set; }
        public long FSA_To_FMH_Id { get; set; }
        public long FSA_To_FMG_Id { get; set; }
        public bool FSA_ActiveFlag { get; set; }
        public long FSA_From_FTI_Id { get; set; }
        public long FSA_TO_FTI_Id { get; set; }
        public long FSA_From_FMA_Id { get; set; }
        public long FSA_To_FMA_Id { get; set; }

        public Array fillyear { get; set; }
        public Array fillstudent { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array fillfromgroup { get; set; }
        public Array filltogroup { get; set; }
        public Array fillfromhead { get; set; }
        public Array filltohead { get; set; }
        public Array filldata { get; set; }

        public long AMCO_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long AMB_Id { get; set; }
        public string ASMC_SectionName { get; set; }

       

        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string FMG_GroupNameF { get; set; }
        public string FMG_GroupNameT { get; set; }
        public string FMH_FeeNameF { get; set; }
        public string FMH_FeeNameT { get; set; }

        public string FTI_NameF { get; set; }
        public string FTI_NameT { get; set; }
        public string multiplegroupF { get; set; }
        public string multiplegroupT { get; set; }
        public long tobepaid { get; set; }
        public long FSS_RunningExcessAmount { get; set; }

        public long FSS_PaidAmount { get; set; }

        public Array currentYear { get; set; }
        public List<CLGFeeAdjustmentDTO> fromlist { get; set; }
        public List<CLGFeeAdjustmentDTO> tolist { get; set; }
        public string returnduplicatestatus { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
        public string searchnumber { get; set; }
        public DateTime searchdate { get; set; }
        public long userid { get; set; }
        public string filterrefund { get; set; }
        public long AMSE_Id { get; set; }

    }
}
