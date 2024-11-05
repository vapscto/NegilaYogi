using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VMS.Sales
{
    public class ISM_Sales_Master_Source_DTO
    {
        public long ISMSMSO_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMSMSO_SourceName { get; set; }
        public string ISMSMSO_Remarks { get; set; }
        public bool ISMSMSO_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ISMSMSO_CreatedBy { get; set; }
        public long ISMSMSO_UpdatedBy { get; set; }
        public string returnvales { get; set; }
        public bool retbool { get; set; }
        public long userId { get; set; }
        public Array source_list { get; set; }
        public Array edit_source_list { get; set; }
    }
}
