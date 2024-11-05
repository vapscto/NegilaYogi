using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreadmissionDTOs.com.vaps.VMS.Training
{
    public class HR_Master_Room_DTO : CommonParamDTO
    {
        public bool select1 { get; set; }

        public long HRMR_Id { get; set; }
        public long HRMRCO_Id { get; set; }
        public long HRMAM_Id { get; set; }
        public long HRMRFI_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMR_RoomName { get; set; }
        public long HRMB_Id { get; set; }
        public long HRMF_Id { get; set; }
        public bool HRMR_ActiveFlag { get; set; }
        public long HRMR_CreatedBy { get; set; }
        public long HRMR_UpdatedBy { get; set; }
        public long userId { get; set; }
        public long  mobileno { get; set; }
        public string emailid { get; set; }
        public string HRMB_BuildingName { get; set; }
        public string HRMF_FloorName { get; set; }
        public bool returnval { get; set; }
        public string returnvalue { get; set; }
        public string HRMR_Desc { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string HRMRFI_FileName { get; set; }
        public string HRMRFI_FilePath { get; set; }
        public Array employeelistedit { get; set; }
        public Array filearray { get; set; }
        public Array editamenitiesfree { get; set; }
        public Array editamenitiespaid { get; set; }
        public Array editfiles { get; set; }
        public Array room_list { get; set; }
        public Array floor_list { get; set; }
        public Array building_list { get; set; }
        public Array freeamnlist { get; set; }
        public Array paidamnlist { get; set; }
        public Array employeelist { get; set; }
        public Array editcontacts { get; set; }
        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public paidemn[] paidemn { get; set; }
        public paidemn[] freeemn { get; set; }
        public HR_Master_Room_DTO[] emp { get; set; }
        public paidemn1[] amlistdeatils { get; set; }
        public bool HRMR_OutSideBookingFlg { get; set; }
        public bool HRMRFI_DefFlg { get; set; }
   
        public long HRMR_Capacity { get; set; }
        public decimal HRMR_RentPerDay { get; set; }
        public long HRMR_NoOfHrs { get; set; }
        public decimal HRMR_RentPerHour { get; set; }
        public string HRMR_TypeFlag { get; set; }
        public string HRMAM_AmenitiesName { get; set; }
        public string HRMAM_AmenitiesDes { get; set; }
        public long HRMRAM_Id { get; set; }
        public bool HRMAM_PriceApplFlg  { get; set; }

}
    
    public class NAACCriteriaFivefileDTO
    {
        public long gridid { get; set; }
        public long cfileid { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public bool defflg { get; set; }
    }

    public class paidemn
    {
        public long hrmraM_Id { get; set; }
        public long hrmaM_Id { get; set; }
        public long noofhrs { get; set; }
        public decimal perday { get; set; }
        public decimal perhours { get; set; }
        public bool flag { get; set; }
        public string amn_name { get; set; }
    }

    public class paidemn1
    {
        public long HRMR_Id { get; set; }
        public long hrmraM_Id { get; set; }
        public long hrmaM_Id { get; set; }
        public long noofhrs { get; set; }
        public decimal perday { get; set; }
        public decimal perhours { get; set; }
        public bool flag { get; set; }
        public string HRMAM_AmenitiesName { get; set; }
        public string HRMAM_AmenitiesDes { get; set; }
        public bool HRMAM_PriceApplFlg { get; set; }
        public decimal? HRMAM_Price { get; set; }
    }

}
