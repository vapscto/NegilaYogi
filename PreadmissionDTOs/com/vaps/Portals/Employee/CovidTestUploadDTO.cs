using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class CovidTestUploadDTO
    {
        public long roleid { get; set; }
        public long Userid { get; set; }
        public long MI_Id { get; set; }
        public long hrmeid { get; set; }
        public string RoleType { get; set; }
        public long stdid { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public Array getloaddetails { get; set; }
        public long ISTCOVTST_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? ISTCOVTST_TestDate { get; set; }
        public string ISTCOVTST_TestResult { get; set; }
        public string ISTCOVTST_FileName { get; set; }
        public string ISTCOVTST_FilePath { get; set; }
        public bool ISTCOVTST_ActiveFlag { get; set; }


        public long ISTUCOVTST_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ISTUCOVTST_TestDate { get; set; }
        public string ISTUCOVTST_TestResult { get; set; }
        public string ISTUCOVTST_FileName { get; set; }
        public string ISTUCOVTST_FilePath { get; set; }
        public bool ISTUCOVTST_ActiveFlag { get; set; }



    }
}
