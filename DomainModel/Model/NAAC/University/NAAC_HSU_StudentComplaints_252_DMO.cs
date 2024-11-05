using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_StudentComplaints_252")]
    public class NAAC_HSU_StudentComplaints_252_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSU252SC_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSU252SC_Year { get; set; }
        public long NCHSU252SC_NoOfStudentsComplaints { get; set; }
        public long NCHSU252SC_TotalNoOfStudentsAppereadExam { get; set; }
        public bool NCHSU252SC_ActiveFlag { get; set; }
        public DateTime NCHSU252SC_CreatedDate { get; set; }
        public DateTime NCHSU252SC_UpdatedDate { get; set; }
        public long NCHSU252SC_CreatedBy { get; set; }
        public long NCHSU252SC_UpdatedBy { get; set; }

        public List<NAAC_HSU_StudentComplaints_252_Files_DMO> NAAC_HSU_StudentComplaints_252_Files_DMO { get; set; }
    }
}
