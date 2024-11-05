using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StaffLoginDTO : CommonParamDTO
    {
        public long IVRMSTAUP_Id { get; set; }
        public long MI_Id { get; set; }

        public long MO_Id { get; set; }
        public int IVRMSTAUL_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long IVRMIMP_Id { get; set; }
        public bool IVRMSTAUP_AddFlag { get; set; }
        public bool IVRMSTAUP_UpdateFlag { get; set; }
        public bool IVRMSTAUP_DeleteFlag { get; set; }
        public bool IVRMUMALP_AddFlg { get; set; }
        public bool IVRMUMALP_UpdateFlg { get; set; }
        public bool IVRMUMALP_DeleteFlg { get; set; }
        public bool IVRMSTAUP_ReportFlag { get; set; }
        public bool IVRMSTAUP_ActiveFlag { get; set; }
        public bool IVRMSTAUP_ProcessFlag { get; set; }
        public long? AMC_Id { get; set; }

        public long IVRMUMALP_Id { get; set; }
        public string User_Name { get; set; }

        public bool NoUserName { get; set; }

        public string User_Name_exact { get; set; }

        public string usrerolenameexact { get; set; }
        public long IVRMRT_Id { get; set; }

        public string flag { get; set; }
        public Array fillorganisation { get; set; }
        public Array fillinstitution { get; set; }
        public Array fillstaff { get; set; }

        public Array fillstaffusers { get; set; }
        public Array fillroletype { get; set; }
        public Array fillmodule { get; set; }
        public Array fillcategory { get; set; }

        public TempDTO[] TempararyArrayList { get; set; }

        public TempDTODELETE[] TempararyArrayListdelete { get; set; }
        public Array fillpages { get; set; }
        public Array showgrid1 { get; set; }

        public Array pagegrid { get; set; }
        public Array showgrid2 { get; set; }

        public Array savedgrid { get; set; }

        public Array thirdgriddatamobile { get; set; }

        public Array thirdgriddatamobileMulti { get; set; }

        public Array thirdgriddata { get; set; }

        public Array previousgrid { get; set; }

        public Array checkusername { get; set; }
        public string IVRMMP_PageName { get; set; }

        public string returnMsg { get; set; }

        public string ivrmM_ModuleName { get; set; }

        public string curuser { get; set; }

        public long IVRMP_Id { get; set; }

        public string newuser { get; set; }

        


        public string IVRMRT_Role { get; set; }

        public string MI_Name { get; set; }

        public bool IVRMRP_AddFlag { get; set; }

        public long IVRMMAP_Id { get; set; }
        public bool IVRMMAP_AddFlg { get; set; }
        public bool IVRMMAP_UpdateFlg { get; set; }
        public bool IVRMMAP_DeleteFlg { get; set; }
        public bool IVRMRP_DeleteFlag { get; set; }
        public bool IVRMRP_UpdateFlag { get; set; }
        public bool IVRMRP_ReportFlag { get; set; }

        public bool IVRMRP_ProcessFlag { get; set; }

        public Array savedgridd { get; set; }

        public string returnval { get; set; }

        public multipleinsti[] multipleinsti { get; set; }

        public savetmpdata[] savetmpdata { get; set; }


        public savetmpdatamobile[] savetmpdatamobile { get; set; }

        public multiplestaff[] multiplestaff { get; set; }

        public deletmobile[] deletmobile { get; set; }
        public updatemobilepagespre[] updatemobilepagespre { get; set; }
        public datasaved[] datasaved { get; set; }

        public deletesaved[] deletesaved { get; set; }

        public long IVRMM_Id { get; set; }

        public string singlemulti { get; set; }

        public long Id { get; set; }

        public long ASMAY_Id { get; set; }

        public long roleId { get; set; }

        public string staffusername { get; set; }

        public int NewUserId { get; set; }
        public long HRME_Id { get; set; }

        public long HRME_MobileNo { get; set; }

        public string HRME_Photo { get; set; }

        public string HRME_EmailId { get; set; }
        public string rolenamess { get; set; }

        public string intname { get; set; }

        public string listofinst { get; set; }
        public string stafname { get; set; }

        public string Machine_Ip_Address { get; set; }

        public string searchfilter { get; set; }

        public Array studentDetails { get; set; }

        public Array singleemployee { get; set; }

        public Array categaryids { get; set; }
        public Array moduleexistid { get; set; }

        public string usrerolename { get; set; }

        public string department { get; set; }

        public string designation { get; set; }

    }

    public class multipleinsti
    {
        public long mi_id { get; set; }
    }

    public class updatemobilepagespre
    {
        public long IVRMUMALP_Id { get; set; }
        public bool? IVRMUMALP_AddFlg { get; set; }
        public bool? IVRMUMALP_UpdateFlg { get; set; }
        public bool? IVRMUMALP_DeleteFlg { get; set; }
    }
    public class savetmpdatamobile
    {
        public long? IVRMMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }

    }

    public class deletesaved
    {
        public long IVRMSTAUP_Id { get; set; }
    }

    public class deletmobile
    {
        public long IVRMUMALP_Id { get; set; }
    }

    public class savetmpdata
    {
        public bool? IVRMRP_AddFlag { get; set; }
        public bool? IVRMRP_DeleteFlag { get; set; }
        public bool? IVRMRP_UpdateFlag { get; set; }
        public bool? IVRMRP_ProcessFlag { get; set; }
        public bool? IVRMRP_ReportFlag { get; set; }
        public long IVRMIMP_Id { get; set; }
        public long IVRMP_Id { get; set; }
    }

    public class multiplestaff
    {
        public int IVRMSTAUL_Id { get; set; }
    }
    public class datasaved
    {
        public bool? IVRMRP_AddFlag { get; set; }
        public bool? IVRMRP_DeleteFlag { get; set; }
        public bool IVRMRP_UpdateFlag { get; set; }
        public bool? IVRMRP_ProcessFlag { get; set; }
        public bool? IVRMRP_ReportFlag { get; set; }       
        public long IVRMSTAUP_Id { get; set; }
    }

}
