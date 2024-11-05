using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class AcademicDTO : CommonParamDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMAY_From_Year { get; set; }
        public string ASMAY_To_Year { get; set; }
        public DateTime? ASMAY_From_Date { get; set; }
        public DateTime? ASMAY_To_Date { get; set; }
        public DateTime? ASMAY_PreAdm_F_Date { get; set; }
        public DateTime? ASMAY_PreAdm_T_Date { get; set; }
        public int ASMAY_Order { get; set; }
        public int ASMAY_ActiveFlag { get; set; }
        public Array AcademicList { get; set; }
        public int ASMAY_Pre_ActiveFlag { get; set; }
        public DateTime? ASMAY_Cut_Of_Date { get; set; }
        public bool Is_Active { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string EnteredData { get; set; }
        public string SearchColumn { get; set; }
        public int count { get; set; }
        public int userId { get; set; }
        public string ASMAY_Year { get; set; }
        public string MI_Name { get; set; }
        public Array institutionList { get; set; }
        public long roleId { get; set; }
        public string messagesaveupdate { get; set; }
        public Array getyear { get; set; }
        public Array getallyear { get; set; }
        public Array getallyearnew { get; set; }
        public List<AcademicDTO> yearorder { get; set; }
        public int fhrors { get; set; }
        public int fminutes { get; set; }
        public int fsec { get; set; }
        public int thrors { get; set; }
        public int tminutes { get; set; }
        public int tsec { get; set; }
        public bool ASMAY_ReggularFlg { get; set; }
        public bool ASMAY_NewFlg { get; set; }
        public bool ASMAY_NewAdmissionFlg { get; set; }
        public DateTime? ASMAY_TransportSDate { get; set; }
        public DateTime? ASMAY_TransportEDate { get; set; }
        public long ASMAY_CreatedBy { get; set; }
        public long ASMAY_UpdatedBy { get; set; }
        public DateTime? ASMAY_ReferenceDate { get; set; }
        public string ASMAY_AcademicYearCode { get; set; }
        public DateTime? ASMAY_AdvanceFeeDate { get; set; }
        public DateTime? ASMAY_RegularFeeFDate { get; set; }
        public DateTime? ASMAY_RegularFeeTDate { get; set; }
        public DateTime? ASMAY_ArrearFeeDate { get; set; }
        public Array geteditdetails { get; set; }
        public long HRMLY_LeaveYearOrder { get; set; }
    }
}
