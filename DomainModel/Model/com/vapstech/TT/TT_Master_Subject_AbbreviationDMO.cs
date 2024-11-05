using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Subject_Abbreviation")]
    public class TT_Master_Subject_AbbreviationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMSUAB_Id { get; set; }

        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string TTMSUAB_Abbreviation { get; set; }
        public bool TTMSUAB_ActiveFlag { get; set; }
      //  public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }



    }
}
