using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_ChapterVIA")]
    public class HR_Master_ChapterVI
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMCVIA_Id

        { get; set; }
        public long MI_Id  
        { get; set; }
        public string HRMCVIA_SectionName

        { get; set; }
        public bool HRMCVIA_SubSectionAplFlg

        { get; set; }
        public bool HRMCVIA_MaxLimitAplFlg

        { get; set; }
        public string HRMCVIA_SectionCode

        { get; set; }
        public string HRMCVIA_PartFlg

        { get; set; }
       public decimal? HRMCVIA_MaxLimit{ get; set; }

        public bool HRMCVIA_ActiveFlg { get; set; }

        public string HRMCIA_ROMANORDER { get; set; }
        public long HRMCVIA_CreatedBy
        { get; set; }
        public long HRMCVIA_UpdatedBy { get; set; }

        public long HRMCVIA_ORDER { get; set; }

    }
}
