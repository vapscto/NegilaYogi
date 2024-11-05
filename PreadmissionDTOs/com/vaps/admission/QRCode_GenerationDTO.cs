using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public  class QRCode_GenerationDTO
    {


        public long User_Id { get; set; }
        public string Message { get; set; }
        public Array profiledata { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }


        public DateTime AMST_Date { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_Sex { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string AMST_FatherName { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        
        public int ASMCL_ClassOrder { get; set; }
        public string ASMC_SectionName { get; set; }

        public int ASMC_SectionOrder { get; set; }

        //new 

        public long IQRGD_Id { get; set; }
        public long MI_Id { get; set; }
        public long Amst_Id { get; set; }
        public string IQRGD_URL { get; set; }
        public string IQRGD_QRCode { get; set; }

        public DateTime IQRGD_CreatedDate { get; set; }
        public DateTime IQRGD_UpdatedDate { get; set; }
        public long IQRGD_CreatedBy { get; set; }
        public long IQRGD_UpdatedBy { get; set; }

        public long SQRGD_Id { get; set; }
        public long hrmE_id { get; set; }
        public string SQRGD_URL { get; set; }
        public string SQRGD_QRCode { get; set; }

        public DateTime SQRGD_CreatedDate { get; set; }
        public DateTime SQRGD_UpdatedDate { get; set; }
        public long SQRGD_CreatedBy { get; set; }
        public long SQRGD_UpdatedBy { get; set; }

        public qrlistarray[] qrlistarray{ get; set; }
        public Staffqrlistarray1[] Staffqrlistarray1 { get; set; }
        public Staffqrlistarray12[] Staffqrlistarray12 { get; set; }
        public Qrcodegeneration[] Qrcodegeneration { get; set; }
        public qrlistarray12[] qrlistarray12 { get; set; }
        
        //public studentsQRcode[] studentsQRcode { get; set; }

        public Staffreportqrlist[] Staffreportqrlist { get; set; }

        //public Staffqrlistarray[] Staffqrlistarray { get; set; }
        //StafflistList
        public Array StaffReportList { get; set; }
        public Array YearList { get; set; }
        public Array ClassList { get; set; }
        public Array SectionList { get; set; }
        public Array StudentList { get; set; }
        public Array getreport { get; set; }
        public Array StaffList { get; set; }
        public Array StudentQRlist { get; set; }

        
        public string flagtype { get; set; }
        public string Flag { get; set; }






        public long roleId { get; set; }

        //institution
        public InstitutionDTO institutionDetails { get; set; }
        //designation name
        public string DesignationName { get; set; }
        //list for employee
        public MasterEmployeeDTO currentemployeeDetails { get; set; }
        //employee name dropdown
        public Array employeedropdown { get; set; }
        //employee Qualification
        public Array employequalification { get; set; }
        public Array employeedocument { get; set; }
        public string HRMC_QulaificationName { get; set; }

        public long?[] stafflist { get; set; }
        public long?[] hrmgT_IdList { get; set; }
        public long?[] employeeTypeIdList { get; set; }
        public long?[] hrmD_IdList { get; set; }
        public long?[] hrmdeS_IdList { get; set; }

        public Array departmentdropdown { get; set; }
        public Array designationdropdown { get; set; }
        public Array employeeclasssubject { get; set; }

        public Array employeeTypedropdown { get; set; }

        public Array groupTypedropdown { get; set; }

        public long LogInUserId { get; set; }
        public EmployeeProfileReportDTO[] ArrayempsList { get; set; }
        public Array institutionDetails_Array { get; set; }
        public List<EmployeeProfileReportDTO> AllInOne { get; set; }

        public string HRMEDS_DocumentName { get; set; }
        public string HRMEDS_DocumentImageName { get; set; }
        public string HRMEDS_DucumentDescription { get; set; }

        //Type
        public string Type { get; set; }
        public long HRMGT_Id { get; set; }
        public Array employeedetailList { get; set; }
        public Array grouplist { get; set; }
        public Array departmentlist { get; set; }
        public Array designationlist { get; set; }
        public DateTime? HRME_DOC { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string HRMGT_EmployeeGroupType { get; set; }
        public Array filltypes { get; set; }
        public string multipletype { get; set; }
        public string multipledep { get; set; }
        public long[] hrmE_multiId { get; set; }
  
        public long ISMS_Id { get; set; }

        public string ISMS_SubjectName { get; set; }
        public string Institutionname { get; set; }










    }


    public class qrlistarray12
    {
        public long Amst_Id { get; set; }
        public string IQRGD_QRCode { get; set; }
    }


    public class qrlistarray
    {
        public long Amst_Id { get; set; }
        public string IQRGD_QRCode { get; set; }
    }

    public class Staffqrlistarray12
    {
        public long hrmE_id { get; set; }
        public string SQRGD_QRCode { get; set; }
    }

    public class Staffqrlistarray1
    {
        public long hrmE_id { get; set; }
        public string SQRGD_QRCode { get; set; }
    }

    public class Staffreportqrlist
    {
        public long hrmE_id { get; set; }
        public string SQRGD_QRCode { get; set; }
    }

    public class Qrcodegeneration
    {
        public long hrmE_id { get; set; }
        public string SQRGD_QRCode { get; set; }
    }


    //public class studentsQRcode
    //{
    //    public long hrmE_id { get; set; }
    //    public string SQRGD_QRCode { get; set; }
    //}


    //Staffreportqrlist
}
