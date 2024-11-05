using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdmissionStandardDTO:CommonParamDTO
    {
        public long ASC_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public int ASC_Adm_AddFieldsFlag { get; set; }
        public int ASC_TC_AddFieldsFlag { get; set; }
        public string ASC_Att_DefaultEntry_Type { get; set; }
        public int ASC_MaxAgeApl_Flag { get; set; }
        public int ASC_MinAgeApl_Flag { get; set; }
        public string ASC_AdmNo_RegNo_RollNo_DefaultFlag { get; set; }
        public string ASC_DefaultDisplay_Flag { get; set; }
        public string ASC_Default_Gender { get; set; }
        public string ASC_ParentsMonthlyIncome_Flag { get; set; }
        public string ASC_ParentsAnnualIncome_Flag { get; set; }
        public string ASC_DefaultSMS_Flag { get; set; }
        public int ASC_School_Address { get; set; }
        public int ASC_Category_Address { get; set; }
        public string ASC_DefaultPhotoUpload { get; set; }
        public string ASC_Stu_Photo_Path { get; set; }
        public string ASC_Staff_Photo_Path { get; set; }
        public string ASC_Logo_Path { get; set; }
        public string ASC_Doc_Path { get; set; }
        public int ASC_Att_Default_OrderFlag { get; set; }
        public int ASC_Default_Clm__Adm_Flag { get; set; }
        public int ASC_Default_Clm__Rol_Flag { get; set; }
        public int ASC_Default_Clm__Reg_Flag { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }     
        public Array fillconfig { get; set; }
        //public string message { get; set; }
        public int ASC_TC_Payment { get; set; }
        public int ASC_ECS_Flag { get; set; }
        public int ASC_TC_Library { get; set; }
        public string ASC_Att_Scheduler_Flag { get; set; }

        // Admission Cancel Config
        public long AACC_Id { get; set; }
        public bool? AACC_DOAFlg { get; set; }
        public long AACC_FromDays { get; set; }
        public long AACC_ToDays { get; set; }
        public decimal? AACC_CancellationPer { get; set; }
        public Array getdetails { get; set; }
        public Array editdetails { get; set; }
    }
}
