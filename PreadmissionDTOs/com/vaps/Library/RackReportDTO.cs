using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
    public class RackReportDTO:CommonParamDTO
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMBANO_Id { get; set; }
        public long LMP_Id { get; set; }
        public string LMB_BookType { get; set; }
        public string LML_LanguageName { get; set; }
        public string LMP_PublisherName { get; set; }
        public long LML_Id { get; set; }
        public string Rack_Name { get; set; }
        public string Rack_Location { get;set; }
        public long Floor_Id { get; set; }
        public long LMC_Id { get; set; }
        public string LMS_SubjectName { get; set; }
        public string FloorName { get; set; }
        public long LMS_Id { get; set; }
        public long LMAL_Id { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string LMC_CategoryName { get; set; }
        public Array floorlist { get; set; }
        public Array racklist { get; set; }
        public Array reportlist { get; set; }
        public Array lib_list { get; set; }
        public long IVRMUL_Id { get; set; }

        public string LMRA_FloorName { get; set; }
        public string LMRA_BuildingName { get; set; }
        public string LMRA_RackName { get; set; }
        public long LMRA_Id { get; set; }
        public string LMBANO_AvialableStatus { get; set; }

    }
}
