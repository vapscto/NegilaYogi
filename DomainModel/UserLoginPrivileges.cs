using DomainModel.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel
{
    [Table("IVRM_User_Login_Privileges")]
     public class UserLoginPrivileges 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMULP_Id { get; set; }
        public long MI_Id { get; set; }
        public int Id { get; set; }
        public long IVRMIMP_Id { get; set; }

        public bool IVRMSTUUP_AddFlag { get; set; }

        public bool IVRMSTUUP_UpdateFlag { get; set; }

        public bool IVRMSTUUP_DeleteFlag { get; set; }

        public bool IVRMSTUUP_ReportFlag { get; set; }

        public bool IVRMSTUUP_ActiveFlag { get; set; }
        public bool IVRMSTUUP_SearchFlag { get; set; }

        public bool IVRMSTUUP_ProcessFlag { get; set; } 
    }
}
