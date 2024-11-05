using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace PreadmissionDTOs.com.vaps.Transport
{
    public class StdRouteLocationMapDTO
    {
     
        //public StdRouteLocationMapDTO[] TempararyArrayhEADListnew { get; set; }
       public StdRouteLocationMapDTO[] savetmpdata { get; set; }
       public STDDATA[] stddatalist { get; set; }
        public long MI_Id { get; set; }
        public Array admsudentslist { get; set; }
        public Array acayear { get; set; }
        public Array busroutelist { get; set; }
        public Array classlist { get; set; }
        public Array picloclist { get; set; }
        public Array sectionlist { get; set; }
        public Array grouplist { get; set; }
        public Array headlist { get; set; }
       // public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string TRML_LocationName { get; set; }
        public string AMST_LastName { get; set; }
        public long ASMAY_Id { get; set; }
      
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string filterinitialdata { get; set; }
        public Array reportdatelist { get; set; }
        public Array reportdatelist1 { get; set; }
        public long FMG_Id { get; set; }
        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public bool checkedgrplst { get; set; }
        public bool checkedheadlst { get; set; }
        public Array check_fee { get; set; }



        //public StdRouteLocationMapDTO[] savegrplst { get; set; }
        //public StdRouteLocationMapDTO[] saveheadlst { get; set; }

        public int TRMRL_Order { get; set; }
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
        //   public long asmay_id { get; set; }
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

        //MB
        public Array locationlist { get; set; }
        public Array schedulelist { get; set; }
        public long TRSR_Id { get; set; }
       // public long MI_Id { get; set; }
       // public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime TRSR_Date { get; set; }
      //  public long FMG_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long TRSR_PickupSchedule { get; set; }
        public long TRSR_PickUpLocation { get; set; }
        public long TRSR_PickUpMobileNo { get; set; }
        public long TRSR_DropSchedule { get; set; }
        public long TRSR_DropLocation { get; set; }
        public long TRSR_DropMobileNo { get; set; }
        public long TRSR_ApplicationNo { get; set; }
        public bool TRSR_ActiveFlg { get; set; }
        public long? ASTA_Id { get; set; }
        public long? TRMR_Drop_Route { get; set; }
       public FMTIDS[] Inst_data { get; set; }
        public string TRMR_RouteName { get; set; }
        public string ASMAY_Year { get; set; }
        public string FMG_GroupName { get; set; }
        public string PickUp_ScheduleName { get; set; }
        public string Drop_ScheduleName { get; set; }
        public string PickUp_LocationName { get; set; }
        public string Drop_LocationName { get; set; }
        public long AMST_MobileNo { get; set; }
        public Array alrdy_stu_list { get; set; }
        public Array installmentlist { get; set; }
        public Array installmentlist_Paid { get; set; }
        public Array installmentlist_saved{ get; set; }
        public string EnteredData { get; set; }
        public string SearchColumn { get; set; }
        public string duplicateno { get; set; }
        public string AMST_AdmNo { get; set; }
        public long TRML_Id { get; set; }
        public long TRSLM_Id { get; set; }

        public bool TRSLM_ActiveFlag { get; set; }
        public bool grp_flag { get; set; }

        //MB

    }
    //public class temp_id_name_DTO
    //{
    //    public long TRML_Id { get; set; }
    //    public string TRMA_AreaName { get; set; }
    //}
    public class FMTIDS
    {
        public long amsT_Id { get; set; }
        public long FTI_Id { get; set; }
        
    }
    public class STDDATA
    {
        public long AMST_Id { get; set; }
        public long TRSR_PickUpLocation { get; set; }
        public long TRMR_Id { get; set; }

    }
}
