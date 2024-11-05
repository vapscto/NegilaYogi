using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.COE
{
    [Table("COE_Events_Videos", Schema ="COE")]
    public class COE_Events_VideosDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int COEEV_Id { get; set; }
        [ForeignKey("COEE_Id")]
        public int COEE_Id { get; set; }
        public string COEEV_Videos { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }


    }
}
