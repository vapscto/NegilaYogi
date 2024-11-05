using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{

    [Table("NAAC_HSU_346_EmpApprovedJournalLists")]
  public  class HSU_346_EMPApprovedJournalList_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCHSU346EAJL_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSU346EAJL_year { get; set; }
        public string NCHSU346EAJL_EmpName { get; set; }
        public string NCHSU346EAJL_BookTitle { get; set; }
        public string NCHSU346EAJL_PaperTitle { get; set; }
        public string NCHSU346EAJL_TitleOfProcConference { get; set; }
        public string NCHSU346EAJL_NameOfConference { get; set; }
        public string NCHSU346EAJL_NationalORInternational { get; set; }
        public string NCHSU346EAJL_ISBNNo { get; set; }
        public string NCHSU346EAJL_AffiliatingInsttimeOfPublication { get; set; }
        public string NCHSU346EAJL_PublisherName { get; set; }
        public bool NCHSU346EAJL_ActiveFlag { get; set; }
        public long NCHSU346EAJL_CreatedBy { get; set; }
        public long NCHSU346EAJL_UpdatedBy { get; set; }
        public DateTime? NCHSU346EAJL_CreatedDate { get; set; }
        public DateTime? NCHSU346EAJL_UpdatedDate { get; set; }
        public decimal NCHSU346EAJL_NoOfCitationsExcludeCitations { get; set; }
        public decimal NCHSU346EAJL_ISTHIndex { get; set; }
        public string NCHSU346EAJL_InstAffMenPub { get; set; }
        public long NCHSU346EAJL_NoOfCitationsScopus { get; set; }
        public long NCHSU346EAJL_NoOfCitationsWebOfScience { get; set; }
        public string NCHSU346EAJL_JournalTitle { get; set; }
public List<HSU_346_EmpApprovedJournalLists_Files_DMO> HSU_346_EmpApprovedJournalLists_Files_DMO { get; set; }
    }
}
