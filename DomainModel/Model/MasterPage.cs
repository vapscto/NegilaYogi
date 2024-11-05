using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Page")]
    public class MasterPage : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMP_Id { get; set; }
        public string IVRMMP_PageName { get; set; }
        public string IVRMP_PageDesc { get; set; }
        // Changed on 11-11-2016 as per new table & requirement
        public string IVRMP_PageURL { get; set; }
        public bool IVRMP_TemplateFlag { get; set; }
      //  public int IVRMP_TemplateTypeFlag { get; set; }
        // Changed on 11-11-2016 as per new table & requirement

        // Added by Sachin to Hold the page state 20-10-2016
       // public string URL { get; set; }
        // Added by sachin to configure page category 20-10-2016
        public long IVRM_Module_Category_Id { get; set; }
        public bool? IVRMP_MandatoryFlag { get; set; }

        // public MasterPageModuleMapping masterPageModuleMapping { get; set; }
        public string IVRMP_PageDisplayName { get; set; }
        public long userid { get; set; }

        public string IVRMP_BrowerUrl { get; set; }
    }
}
