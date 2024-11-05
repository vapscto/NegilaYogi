using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Master_OE_ANS_Options")]
    public class PA_Master_OE_ANS_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMOEAO_Id { get; set; }       
        public long MI_Id { get; set; }
        public string PAMOEAO_Options { get; set; }
        public string PAMOEAO_OptionsFlag { get; set; }           
    }
}
