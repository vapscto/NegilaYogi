using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Adm_M_Visitor_Management", Schema = "VM")]
    public class AddVisitorsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMVM_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMVM_Name { get; set; }
        public long AMVM_Contact_No { get; set; }
        public string AMVM_Emailid { get; set; }
        public string AMVM_Card_No { get; set; }
        public string AMVM_Identity_Type { get; set; }
        public string AMVM_Address { get; set; }
        public string AMVM_To_Meet { get; set; }
        public string AMVM_Purpose { get; set; }
        public DateTime? Date_Visit { get; set; }
        public string Time_Visit { get; set; }
        public string AMVM_Type { get; set; }
        public string AMVM_Status { get; set; }
        public string AMVM_Out_Time { get; set; }
        public DateTime? AMVM_Entry_Date { get; set; }
        public string AMVM_Remarks { get; set; }
        public bool AMVM_ActiveFlag { get; set; }

    }
}
