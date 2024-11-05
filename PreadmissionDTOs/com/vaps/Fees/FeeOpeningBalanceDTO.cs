using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeOpeningBalanceDTO
    {
        public TempDTO[] TempararyArrayListnew { get; set; }
        public FeeOpeningBalanceDTO[] TempararyArrayhEADListnew { get; set; }
        public FeeOpeningBalanceDTO[] savetmpdata { get; set; }
        public long MI_ID { get; set; }
        public Array admsudentslist { get; set; }
        public Array acayear { get; set; }
        public Array busroutelist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array grouplist { get; set; }
        public Array headlist { get; set; }
        public string filterrefund { get; set; }
        public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public long asmay_id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        
        public string filterinitialdata { get; set; }
        public Array reportdatelist { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        
        public string FMH_FeeName { get; set; }
        //public string FMG_GroupName { get; set; }
        //public string FTI_Name { get; set; }
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }
        public int fsscount { get; set; }
        
        public FeeOpeningBalanceDTO[] savegrplst { get; set; }
        public FeeOpeningBalanceDTO[] saveheadlst { get; set; }

        public long asmayidpss { get; set; }
        public string typeofrpt { get; set; }
        public DateTime? datepass { get; set; }
        public string fillbusroutestudents { get; set; }
        public long fillclasflg { get; set; }
        public long fillseccls { get; set; }
        public string studenttype { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        //April 15th,mahaboob
        public long FMOB_Id { get; set; }
      //  public long MI_Id { get; set; }
        //public long AMST_Id { get; set; }
       // public long ASMAY_Id { get; set; }
        //public long FMH_Id { get; set; }
        public DateTime? FMOB_EntryDate { get; set; }
        public decimal FMOB_Student_Due { get; set; }
        public decimal FMOB_Institution_Due { get; set; }


        public bool checkedvalue { get; set; }

        public bool returnval { get; set; }
        public string returntxt { get; set; }
        public Array Class_Category_List { get; set; }

        public long fmcC_Id { get; set; }
        public string searchType { get; set; }
        public string searchtext { get; set; }
        public string searchnumber { get; set; }

        public long userid { get; set; }

        public Array fillmastergroup { get; set; }
        public Array fillmasterhead { get; set; }
        public Array fillinstallment { get; set; }

    }
}
