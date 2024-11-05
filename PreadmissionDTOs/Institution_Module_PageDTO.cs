using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class Institution_Module_PageDTO : CommonParamDTO
    {
        public long IVRMIMP_Id { get; set; }
        public long IVRMIM_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public int IVRMIMP_Flag { get; set; }
        public int IVRMIMP_PageOrder { get; set; }

        public string IVRMRT_Role { get; set; }
        public string MI_Name { get; set; }
        public string IVRMM_ModuleName { get; set; }
        public string IVRMMP_PageName { get; set; }
        
        

       

        public long IVRMM_Id { get; set; }
        public long MI_Id { get; set; }
        public Institution_Module_PageDTO[] savetmpdata { get; set; }
        public Institution_Module_PageDTO[] privilagedata { get; set; }
        public Array PageDropDown { get; set; }
        public string url { get; set; }
        public long IVRM_Module_Category_Id { get; set; }
        // Changed on 11-11-2016 as per new table & requirement
        public long moduleId { get; set; }
        public long pageId { get; set; }
        public long menuId { get; set; }
        public string IVRMP_PageURL { get; set; }
        public bool IVRMP_TemplateFlag { get; set; }
        public int IVRMP_TemplateTypeFlag { get; set; }
        public int order { get; set; }
        public long parentId { get; set; }
        public bool pagenonpageflag { get; set; }
        // Changed on 11-11-2016 as per new table & requirement


        public string IVRMMMI_Icon { get; set; }
        public string IVRMMMI_Color { get; set; }

        public long empid { get; set; }
    }
}
