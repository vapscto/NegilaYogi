using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Book_Transaction", Schema = "LIB")]
    public class BookTransactionDMO:CommonParamDMO
    {
        [Key]
        public long LBTR_Id { get; set; }
       public long MI_Id { get; set; }
        public long LMBANO_Id { get; set; }
        public DateTime? LBTR_IssuedDate { get; set; }
        public string LBTR_GuestName { get; set; }
        public long LBTR_GuestContactNo { get; set; }
        public string LBTR_GuestEmailId { get; set; }
        public DateTime? LBTR_DueDate { get; set; }
        public string LBTR_Status { get; set; }
        public DateTime LBTR_ReturnedDate { get; set; }
        public DateTime? LBTR_RenewedDate { get; set; }
        public decimal LBTR_TotalFine { get; set; }
        public decimal LBTR_FineCollected { get; set; }
        public decimal LBTR_FineWaived { get; set; }
        public bool LBTR_ActiveFlg { get; set; }
        public int LBTR_Renewalcounter { get; set; }


        public List<LIB_Book_Transaction_StudentDMO> LIB_Book_Transaction_StudentDMO { get; set; }
        public List<LIB_Book_Transaction_DepartmentDMO> LIB_Book_Transaction_DepartmentDMO { get; set; }
        public List<LIB_Book_Transaction_StaffDMO> LIB_Book_Transaction_StaffDMO { get; set; }
        public List<LIB_Book_Transaction_Student_CollegeDMO> LIB_Book_Transaction_Student_CollegeDMO { get; set; }




    }
}
