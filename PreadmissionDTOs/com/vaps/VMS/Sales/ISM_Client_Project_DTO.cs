using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
   public  class ISM_Client_Project_DTO
    {
        public long ISMCLTPRBOM_Id { get; set; }
        public long ISMMCLTPR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMCLTPRMDOC_Id { get; set; }
        public long ISMCLTPRDOC_Id { get; set; }
        public long ISMCLTPRMP_Id { get; set; }
        public long User_Id { get; set; }
        public long ISMCLTC_Id { get; set; }
        public decimal ISMCLTPRBOM_Qty { get; set; }
        public decimal ISMCLTPRMP_Qty { get; set; }
        public string ISMCLTPRBOM_Remarks { get; set; }
        public string ISMCLTPRMDOC_Name { get; set; }
        public string ISMCLTPRMDOC_Description { get; set; }
        public string ISMCLTPRMP_ResourceName { get; set; }
        public string ISMCLTPRMP_Remarks { get; set; }
        public string returndata { get; set; }
        public string ISMCLTC_Name { get; set; }
        public string ISMCLTC_Description { get; set; }
        public string ISMCLTPRDOC_FilePath { get; set; }
        public string ISMCLTPRDOC_FileName { get; set; }
        public bool ISMCLTPRBOM_ActiveFlag { get; set; }
        public bool ISMCLTPRMDOC_ActiveFlag { get; set; }
        public bool ISMCLTC_ActiveFlag { get; set; }
        public bool ISMCLTPRDOC_ActiveFlag { get; set; }
        public bool ISMCLTPRMP_ActiveFlag { get; set; }
        public long ISMCLTPRBOM_CreatedBy { get; set; }
        public long ISMCLTPRBOM_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime ISMCLTPRDOC_Date { get; set; }



        public Array components_list { get; set; }
        public Array components_details { get; set; }
        public Array bom_details { get; set; }
        public Array mp_details { get; set; }
        public Array doc_details { get; set; }
        public Array clientproject_dd { get; set; }
        public Array bom_list { get; set; }
        public Array mp_list { get; set; }
        public Array doc_list { get; set; }
        public Array docmaster_list { get; set; }
        public Array docmaster_details { get; set; }
        public Array components_dd { get; set; }
        public Array document_dd { get; set; }
    }
}
