using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class EmployeeDataImportDTO
    {
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public int dupcnt { get; set; }
        public int failcnt { get; set; }
        public int suscnt { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array employeedetails { get; set; }
      //  public Array EMPDataEMPORT { get; set; }
        public EMPDataEMPORT[] empDataimport { get; set; }
    }


    public class EMPDataEMPORT
    {
        public string EmployeeType { get; set; }
        public string EmployeeGroupType { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeMiddleName { get; set; }
        public string EmployeeLastName { get; set; }
        public string EmployeeCode { get; set; }
        public string Marital_Status { get; set; }
        public string Gender_Name { get; set; }
        public string CasteCategory_Name { get; set; }
        public string Caste_Name { get; set; }
        public string Religion_Name { get; set; }
        public long MobileNo { get; set; }
        public string EmailID { get; set; }        
        public DateTime? employeeDOJ { get; set; }
        public DateTime? employeeDOB { get; set; }

        public long pincode { get; set; }
        public string employeeaddress1 { get; set; }


    }
}
