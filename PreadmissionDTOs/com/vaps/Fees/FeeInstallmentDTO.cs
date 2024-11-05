using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeInstallmentDTO:CommonParamDTO
    {
       
        public long FMI_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMI_Name { get; set; }
        public long? FMI_No_Of_Installments { get; set; }
        public string FMI_Installment_Type { get; set; }
        public bool FMI_ActiceFlag { get; set; }
        public bool returnval { get; set; }
        public string msg { get; set; }
        public string returnvalexist { get; set; }
        public Array InstallmentData { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array InTData { get; set; }
        public Array gettingtabledata { get; set; }
        public Array temparr { get; set; }
        public Array sendingarr { get; set; }
        public List<FeeInstallmentyeralyDTO> fydto { get; set; }
        public Array academicdrp { get; set; }
        public Array instypesdrp { get; set; }
        public List<FeeInstalmentDueDateDTO> fidddto { get; set; }
        public long temyrid { get; set; }
        public Array datasendhtml { get; set; }
        public string returnvalue { set; get; }
        public long ASMAY_Id { get; set; }
    }
}
