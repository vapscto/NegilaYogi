using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Visitor_Visitors", Schema = "VM")]
    public class Multiple_VisitorDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long VMMVVI_Id { get; set; }
        public long VMMV_Id { get; set; }
        public string VMMVVI_VisitorName { get; set; }
        public string VMMVVI_VisitorAddress { get; set; }
        public string VMMVVI_VisitorEmailId { get; set; }
        public string VMMVVI_VisitorContactNo { get; set; }
        public string VMMVVI_Remarks { get; set; }
        public long VMMVVI_CreatedBy { get; set; }
        public DateTime VMMVVI_CreatedDate { get; set; }
        public long VMMVVI_UpdatedBy { get; set; }
        public DateTime VMMVVI_Updateddate { get; set; }
        public string VMMVVI_VisitorCardNo { get; set; }
        public string VMMVVI_DocumentUpload { get; set; }
        public string VMMVVI_VisitorPhoto { get; set; }
        public string VMMVVI_IDCardNo { get; set; }
        public bool? VMMVVI_IDCardReturnedFlg { get; set; }
        public bool? VMMVVI_BlocekFlg { get; set; }      
        public DateTime? VMMVVI_IDCardReturnedDateTime { get; set; }
    }
}
