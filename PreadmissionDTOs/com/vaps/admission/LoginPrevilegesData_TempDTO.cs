using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class LoginPrevilegesData_TempDTO
    {

        public string username { get; set; }
        public long asmcL_Id { get; set; }
        public long asmC_Id { get; set; }
        public string cls_sec { get; set; }
        public string cls_sec_id { get; set; }
        
        // public Att_Temp_Class_Previlages[] cls_sec { get; set; }     
        public Att_Temp_Subject_Previlages[] subs { get; set; }
    }

    //public class Att_Temp_Class_Previlages
    //{       
    //    public long ASMCL_Id { get; set; }
    //    public string ASMCL_ClassName { get; set; }           
    //    public long ASMS_Id { get; set; }
    //    public string ASMC_SectionName { get; set; }

    //}
    
    public class Att_Temp_Subject_Previlages
    {       
        public long ISMS_Id { get; set; }       
        public string ISMS_SubjectName { get; set; }
      

    }
}
