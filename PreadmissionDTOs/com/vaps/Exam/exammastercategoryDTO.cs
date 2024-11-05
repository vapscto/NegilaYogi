using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class exammastercategoryDTO : CommonParamDTO
    {

        public bool already_cnt { get; set; }
        //master category
        public int EMCA_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public string examorpromotionflag { get; set; }
        public bool? EMCA_CCECheckingFlg { get; set; }
        public bool EMCA_ActiveFlag { get; set; }

        //master category_class
        public int ECAC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool ECAC_ActiveFlag { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array avail_sections { get; set; }
        public Array selected_sections { get; set; }
        public Array binddetails { get; set; }
        public Array mastetr_category_list { get; set; }
        public Array edit_m_category { get; set; }
        public Array edit_category_class { get; set; }
        public Array categorylist { get; set; }
        public Array category_class_list { get; set; }
        public Array get_format_mappeddetails { get; set; }
        public Array detailslist { get; set; }
        public School_M_ClassDTO[] clssids { get; set; }
        public long[] secids { get; set; }
        public Array view_cls_sections { get; set; }
        public string returnMsg { get; set; }
        public string EMCA_Name { get; set; }
        public string EMCA_Detail { get; set; }
        public Array exammastercategoryname { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string msg { get; set; }
        public saved_temp[] selected_temp { get; set; }
        public int ASMAY_Order { get; set; }
        public string message { get; set; }
        public long EPCFT_Id { get; set; }
        public long userId { get; set; }
        public bool? EPCFT_ActiveFlg { get; set; }

    }
    public class saved_temp
    {
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public int EMCA_Id { get; set; }
        public string EMCA_CategoryName { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Section_DTO[] sections { get; set; }
    }
    public class Section_DTO:CommonParamDTO
    {
        public long ASMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMC_SectionCode { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMC_ActiveFlag { get; set; }
        public int ASMC_MaxCapacity { get; set; }
    }

}
