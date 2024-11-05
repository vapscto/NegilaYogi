using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{

    [Table("NAAC_HSU_345_TeacherResearchPapers")]
   public class NAAC_HSU_345_TeacherResearchPapers_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

     public long NCHSU345TRP_Id { get; set; }
 public long MI_Id { get; set; }
        public long NCHSU345TRP_Year { get; set; }
        public string NCHSU345TRP_PaperTitle { get; set; }
        public string NCHSU345TRP_AuthorName { get; set; }
        public string NCHSU345TRP_DepartmentName { get; set; }
        public string NCHSU345TRP_JournalName { get; set; }
        public string NCHSU345TRP_ISSNNumber { get; set; }
        public string NCHSU345TRP_link { get; set; }
        public string NCHSU345TRP_NamesOfIndexingDatabases { get; set; }
        public bool NCHSU345TRP_ActiveFlag { get; set; }
        public long NCHSU345TRP_CreatedBy { get; set; }
        public long NCHSU345TRP_UpdatedBy { get; set; }
        public DateTime NCHSU345TRP_CreatedDate { get; set; }
        public DateTime NCHSU345TRP_UpdatedDate { get; set; }
        public List<NAAC_HSU_345_TeacherResearchPapers_Files_DMO> NAAC_HSU_345_TeacherResearchPapers_Files_DMO { get; set; }

    }
}
