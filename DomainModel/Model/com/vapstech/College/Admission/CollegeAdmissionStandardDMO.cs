using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_School_Configuration")]
    public  class CollegeAdmissionStandardDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASC_Id { get; set; }
        public long MI_Id { get; set; }
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
        public string ASC_Stu_Photo_Path { get; set; }
        public string ASC_Staff_Photo_Path { get; set; }
        public string ASC_Logo_Path { get; set; }
        public string ASC_Doc_Path { get; set; }
        public string ASC_DefaultSMS_Flag { get; set; }
        public int ASC_School_Address { get; set; }
        public int ASC_Category_Address { get; set; }
        public string ASC_DefaultPhotoUpload { get; set; }
        public int ASC_Att_Default_OrderFlag { get; set; }
        public int ASC_Default_Clm__Adm_Flag { get; set; }
        public int ASC_Default_Clm__Rol_Flag { get; set; }
        public int ASC_Default_Clm__Reg_Flag { get; set; }
        public int ADMC_TCAllowBalanceFlg { get; set; }
        public int ASC_ECS_Flag { get; set; }
        public string ASC_Att_Scheduler_Flag { get; set; }
    }
}
