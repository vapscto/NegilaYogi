using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class CLGStudentRouteMappingDTO
    {
        public long TRSRFGCO_Id { get; set; }
        public long TRSRCO_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string EnteredData { get; set; }
        public string SearchColumn { get; set; }
        public long AMCST_Id { get; set; }
        public DateTime TRSRCO_Date { get; set; }
        public bool ACSRM_Mapping_Flag { get; set; }
        public long ACSRM_Monthid { get; set; }
        public long TRSRCO_PickUpRoute { get; set; }
        public long TRSRCO_PickupSession { get; set; }
        public long TRRSCO_PickUpLocation { get; set; }
        public long? TRRSCO_PickUpMobileNo { get; set; }
        public long TRSRCO_DropRoute { get; set; }
        public long TRSRCO_DropSession { get; set; }
        public long TRRSCO_DropLocation { get; set; }
        public long? TRRSCO_DropMobileNo { get; set; }
        public long? ASTACO_Id { get; set; }
        public string TRRSCO_ApplicationNo { get; set; }
        public string studenttype { get; set; }
        public string duplicateno { get; set; }   
        public bool TRRSCO_ActiveFlg { get; set; }
        public long FMG_Id { get; set; }
        public string pickup_SessionName { get; set; }
        public string TRMR_RouteName { get; set; }
        
        public string drop_SessionName { get; set; }
        public bool TRSRFGCO_ActiveFlg { get; set; }
       

        public long IVRM_Month_Id { get; set; }


        public bool returnval { get; set; }
        public Array alrdy_stu_list { get; set; }
        public Array grpeditlist { get; set; }
        public Array yealist { get; set; }
        public Array monthdropdown { get; set; }
        public Array Buspasslist { get; set; }
        public Array grouplist { get; set; }
        public Array courselist { get; set; }
        public Array sectionlist { get; set; }
        public Array reportdatelist { get; set; }
        public Array routelist { get; set; }
        public Array picsesslist { get; set; }
        public Array locationlist { get; set; }
        public Array reportdatelist1 { get; set; }
        public Array drpsesslist { get; set; }
        public Array admsudentslist { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ACYST_RollNo { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public string AMCST_emailId { get; set; }
        public string AMCST_AdmNo { get; set; }      
        public string AMCST_FirstName { get; set; }
        public long PickUp_Session { get; set; }
        public long Drop_Session { get; set; }
        public clggrpid[] some_data { get; set; }
        public CLGStudentRouteMappingDTO[] savegrplst { get; set; }
        public CLGStudentRouteMappingDTO[] saveheadlst { get; set; }
        public CLGStudentRouteMappingDTO[] TempararyArrayhEADListnew { get; set; }
        public CLGStudentRouteMappingDTO[] savetmpdata { get; set; }
    }

    public class clgtemp_id_name_DTO
    {
        public long TRML_Id { get; set; }
        public string TRMA_AreaName { get; set; }
    }
    public class clggrpid
    {
        public long amcsT_Id { get; set; }
        public clgtemp_id_name_DTO[] grp_list { get; set; }
        public long? TRRSCO_PickUpMobileNo { get; set; }
        public long? TRRSCO_DropMobileNo { get; set; }
        public string TRRSCO_ApplicationNo { get; set; }
    }
}
