using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class Adm_M_Student_TempDTO
    {
        public long AMSTFMNO_Id { get; set; }
        public long MI_Id { get; set; }
        public long? AMST_FatherMobleNo { get; set; }
        public long AMST_Id { get; set; }
        public string ACSTSMS_CoutryCode { get; set; }
    }
    public class Adm_M_Student_Eamil
    {
        public long AMSTFEMAIL_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMST_FatheremailId { get; set; }
        public long AMST_Id { get; set; }

    }
    public class Adm_M_Mother_MobileNo1
    {
        public long AMSTMMNO_Id { get; set; }
        public long MI_Id { get; set; }
        public long? AMST_MotherMobileNo { get; set; }
        public long AMST_Id { get; set; }
        public string AMSTMMNO_CoutryCode { get; set; }
    }
    public class Adm_M_Mother_Emailid1
    {
        public long AMSTMEMAIL_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMST_MotherEmailId { get; set; }
        public long AMST_Id { get; set; }

    }
    public class Adm_M_Student_MobileNoDTO
    {
        public long AMSTSMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_MobileNo { get; set; }
        public string AMSTSMS_CountryCode { get; set; }
    }

    public class Adm_M_Student_EmailIdDTO
    {
        public long AMSTE_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_emailId { get; set; }
    }
    public class Adm_M_Student_TempMobileNo
    {
        public long UserName { get; set; }
        public string Role { get; set; }

        //public string concatinatedRole { get; set; }
    }
    public class Adm_M_Student_TempEmailId
    {
        public long UserNameemail { get; set; }
        public string Roleemail { get; set; }

        //public string concatinatedRole { get; set; }
    }
    public class Adm_M_Student_ECS
    {
        public long ASECS_Id { get; set; }
        //    public long AMST_Id { get; set; }
        public string ASECS_AccountHolderName { get; set; }
        public string ASECS_AccountNo { get; set; }
        public string ASECS_AccountType { get; set; }
        public string ASECS_BankName { get; set; }
        public string ASECS_Branch { get; set; }
        public string ASECS_MICRNo { get; set; }
        public bool ASECS_ActiveFlg { get; set; }
    }
    public class Adm_M_Student_ECSDTo
    {
        public long ASECS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ASECS_AccountHolderName { get; set; }
        public string ASECS_AccountNo { get; set; }
        public string ASECS_AccountType { get; set; }
        public string ASECS_BankName { get; set; }
        public string ASECS_Branch { get; set; }
        public string ASECS_MICRNo { get; set; }
        public bool ASECS_ActiveFlg { get; set; }
    }
}
