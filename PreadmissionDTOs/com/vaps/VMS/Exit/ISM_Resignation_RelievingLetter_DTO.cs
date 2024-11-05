using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PreadmissionDTOs.com.vaps.VMS.Exit
{
    public class ISM_Resignation_RelievingLetter_DTO
    {
        public long ISMRESGRL_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMRESG_Id { get; set; }
        public string ISMRESGRL_RelievingLetterLNo { get; set; }
        public DateTime ISMRESGRL_RLDate { get; set; }
        public bool ISMRESGRL_ActiveFlag { get; set; }
        public DateTime ISMRESGRL_CreatedDate { get; set; }
        public DateTime ISMRESGRL_UpdatedDate { get; set; }
        public long ISMRESGRL_CreatedBy { get; set; }
        public long ISMRESGRL_UpdatedBy { get; set; }
    }
}
