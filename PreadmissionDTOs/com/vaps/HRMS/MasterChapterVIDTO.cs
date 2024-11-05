using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class MasterChapterVIDTO
    {
        public long HRMCVIA_Id

        { get; set; }
        public long MI_Id
        { get; set; }
        public string HRMCVIA_SectionName

        { get; set; }
        public bool HRMCVIA_SubSectionAplFlg

        { get; set; }
        public bool HRMCVIA_MaxLimitAplFlg

        { get; set; }
        public string HRMCVIA_SectionCode

        { get; set; }
        public string HRMCVIA_PartFlg

        { get; set; }
        public decimal? HRMCVIA_MaxLimit { get; set; }

        public bool HRMCVIA_ActiveFlg { get; set; }


        public long HRMCVIA_CreatedBy { get; set; }
        public long HRMCVIA_UpdatedBy { get; set; }
        public long roleId { get; set; }
        public Array emploanList { get; set; }
        public string retrunMsg { get; set; }
        public long HRME_Id { get; set; }

        public Array employeedropdown { get; set; }

        public Array masterloandropdown { get; set; }


        public string hrmE_EmployeeFirstName { get; set; }

        public string HRML_LoanType { get; set; }

        public MasterChapterVIDTO[] MasterChapterVIDTOO { get; set; }
        public Array leaveyeardropdown { get; set; }
        public Array modeOfPaymentdropdown { get; set; }
        public HR_ConfigurationDTO configurationDetails { get; set; }
        public Master_NumberingDTO transnumconfigsettings { get; set; }
        public long LogInUserId { get; set; }
        //Academic Year
        public long ASMAY_Id { get; set; }
        public decimal? empGrossSal { get; set; }
        public Array allowance { get; set; }
        public long HRMCVIA_ORDER { get; set; }
        public Array ordrlist { get; set; }
        public string HRMCIA_ROMANORDER { get; set; }
        public string roman { get; set; }
        public tempdtoromman[] tempdtoromman { get; set; }
    }
    public class tempdtoromman
    {
        public string roman { get; set; }
    }
}
