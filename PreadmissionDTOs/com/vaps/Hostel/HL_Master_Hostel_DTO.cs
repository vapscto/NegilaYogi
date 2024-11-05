using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
   public class HL_Master_Hostel_DTO
    {
        public long HLMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string HLMH_Name { get; set; }
        public string HLMH_BoysGirlsFlg { get; set; }
        public string HLMH_Address { get; set; }
        public long IVRMMC_Id { get; set; }
        public long IVRMMS_Id { get; set; }
        public string HLMH_City { get; set; }
        public long HLMH_Pincode { get; set; }
        public long HLMH_ContactNo { get; set; }
        public long HLMH_TotalFloor { get; set; }
        public long HLMH_TotalRoom { get; set; }
        public long HLMH_TotalCapacity { get; set; }
        public string HLMH_Desc { get; set; }
        public bool HLMH_ActiveFlag { get; set; }
        public string HLMHW_WardenType { get; set; }
        public long HLMHP_Id { get; set; }
        public string HLMHP_FileName { get; set; }
        public string HLMHP_FilePath { get; set; }
        public bool HLMHP_ActiveFlg { get; set; }       
        public string IVRMMS_Name { get; set; }
        public string IVRMMS_Code { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMC_CountryCode { get; set; }        
        public string IVRMMG_GenderName { get; set; }
        public long HLMM_Id { get; set; }
        public long HLMHMS_Id { get; set; }
        public string HLMM_Name { get; set; }
        public long HLMFTY_Id { get; set; }
        public string HLMFTY_FacilityName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long HRME_Id { get; set; }
        public bool HLMHW_ActiveFlg { get; set; }
        public long HLMHF_Id { get; set; }
        public bool HLMHF_ActiveFlg { get; set; }
        
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array countrylist { get; set; }
        public Array statelistdata { get; set; }
        public Array get_gridlistdata { get; set; }        
        public Array edit_hostel_row { get; set; }        
        public Array edit_countrylist { get; set; }        
        public Array editphotolist { get; set; }        
        public Array mess_list { get; set; }        
        public Array facilities_list { get; set; }        
        public Array employee_list { get; set; }        
        public Array edithostelmess_id { get; set; }        
        public Array edit_facilitylist { get; set; }        
        public Array edit_emplist { get; set; }        
        public Array edit_mess { get; set; }        
        public Array edit_photolist { get; set; }        
        public Array mappedfacilitylist { get; set; }        
        public Array mappedEmpllist { get; set; }        
        public Array viewuploadflies { get; set; }        

        public hostelPictureUploadDTO[] hostelPictureUploadDTO { get; set; }
        public selectedFacilityDTO[] selectedFacilityDTO { get; set; }
        public selectedWardenDTO[] selectedWardenDTO { get; set; }
        public selectedMessDTO[] selectedMessDTO { get; set; }       
    }

    public class hostelPictureUploadDTO
    {
        public long HLMHP_Id { get; set; }
        public string HLMHP_FileName { get; set; }
        public string HLMHP_FilePath { get; set; }
    }
    public class selectedFacilityDTO
    {
        public long HLMFTY_Id { get; set; }
    }
    public class selectedWardenDTO
    {
        public long HRME_Id { get; set; }
    }
    public class selectedMessDTO
    {
        public long HLMM_Id { get; set; }
    }
}
