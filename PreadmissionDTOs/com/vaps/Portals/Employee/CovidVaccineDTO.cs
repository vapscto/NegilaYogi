using System;
using System.Collections.Generic;
using System.Text;


namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class CovidVaccineDTO
    {
        public long roleid { get; set; }
        public long Userid { get; set; }
        public long MI_Id { get; set; }
        public long ISTCOVVAC_Id { get; set; }
        public long HRME_Id { get; set; }
        public long IMCOVVAC_Id { get; set; }
        public bool IMCOVVAC_ActiveFlag { get; set; }
        public DateTime? ISTCOVVAC_VaccinationDate { get; set; }
        public string ISTCOVVAC_Dose { get; set; }
        public string ISTCOVVAC_FileName { get; set; }
        public string ISTCOVVAC_FilePath { get; set; }
        public bool ISTCOVVAC_ActiveFlag { get; set; }
        public DateTime ISTCOVVAC_CreatedDate { get; set; }
        public DateTime ISTCOVVAC_UpdatedDate { get; set; }
        public long ISTCOVVAC_CreatedBy { get; set; }
        public long ISTCOVVAC_UpdatedBy { get; set; }

        public long ISTUCOVVAC_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime? ISTUCOVVAC_VaccinationDate { get; set; }
        public string ISTUCOVVAC_Dose { get; set; }
        public string ISTUCOVVAC_FileName { get; set; }
        public string ISTUCOVVAC_FilePath { get; set; }
        public bool ISTUCOVVAC_ActiveFlag { get; set; }
        public DateTime ISTUCOVVAC_CreatedDate { get; set; }
        public DateTime ISTUCOVVAC_UpdatedDate { get; set; }
        public long ISTUCOVVAC_CreatedBy { get; set; }
        public long ISTUCOVVAC_UpdatedBy { get; set; }


        public bool returnval { get; set; }
        public string message { get; set; }
        public string IMCOVVAC_VaccinationName { get; set; }
        public Array getloaddetails { get; set; }
        public Array vaccinationtype { get; set; }
        public long hrmeid {get;set;}
        public string RoleType { get; set; }
        public long stdid { get; set; }
        
    }
}
