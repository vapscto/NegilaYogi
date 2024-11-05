using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class MasterClassCategoryDTO:CommonParamDTO
    {
        public long ASMCC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool Is_Active { get; set; }
       
        public int count { get; set; }
        public bool returnVal { get; set; }
        public string Input { get; set; }
        public string SearchColumn { get; set; }
        public string Year { get; set; }
        public string categoryName { get; set; }
        public string className { get; set; }
        public string message { get; set; }

        public string settingMsg { get; set; }
        public Array acdYearList { get; set; }
        public Array currentYear { get; set; }
        public Array categoryDrpDwn { get; set; }
        public Array classDrpDwn { get; set; }
        public Array classcategoryList { get; set; }
        public Array sectionList { get; set; }
        public School_M_ClassDTO[] selectedClass { get; set; }
        public MasterClassCategoryDTO[] selectedSection { get; set; }

        public MasterClassCategoryDTO[] selectedClassList { get; set; }
        public string msgdeactive { get; set; }
        public string messagesaveupdate { get; set; }

        //Class Section Mapping On 16-08-2017.
        public long ASMCCS_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string sectionName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array classsectionList { get; set; }
        public Array getsavedsectionlist { get; set; }
        public Array viewsectionlist { get; set; }
        public bool ASMCCS_ActiveFlg { get; set; }
        public int ASMC_order { get; set; }
    }
}
