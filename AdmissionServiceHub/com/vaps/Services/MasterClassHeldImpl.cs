using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class MasterClassHeldImpl : Interfaces.MasterClassHeldInterface
    {
        private static ConcurrentDictionary<string, MasterClassHeldDTO> _login =
           new ConcurrentDictionary<string, MasterClassHeldDTO>();

        public AcademicContext _academicContext;
        public DomainModelMsSqlServerContext _context;
        public MasterSectionContext _sectionContext;

        public MasterClassHeldImpl(DomainModelMsSqlServerContext masterclassContext, AcademicContext academic, MasterSectionContext sectionContext)
        {
            _context = masterclassContext;
            _academicContext = academic;
            _sectionContext = sectionContext;
        }
        public MasterClassHeldDTO getAllDetails(MasterClassHeldDTO mch)
        {

            try
            {
                List<MasterAcademic> LoadallYear = new List<MasterAcademic>();
                LoadallYear = _academicContext.Academic.Where(a => a.Is_Active == true && a.MI_Id == mch.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToList();
                mch.allyear = LoadallYear.ToArray();

                List<MasterAcademic> CurrentYear = new List<MasterAcademic>();
                CurrentYear = _academicContext.Academic.Where(a => a.Is_Active == true && a.MI_Id == mch.MI_Id && a.ASMAY_Id == mch.AMAY_Id).ToList();
                mch.currentYear = CurrentYear.ToArray();

                List<School_M_Class> allClass = new List<School_M_Class>();
                allClass = _context.School_M_Class.Where(c => c.ASMCL_ActiveFlag == true && c.MI_Id == mch.MI_Id).OrderBy(d => d.ASMCL_Order).ToList();
                mch.classDrpDwn = allClass.ToArray();

                List<Month> allmonth = new List<Month>();
                allmonth = _context.month.Where(m => m.Is_Active == true).ToList();
                mch.monthList = allmonth.ToArray();

                List<School_M_Section> allSection = new List<School_M_Section>();
                allSection = _sectionContext.masterSection.Where(s => s.ASMC_ActiveFlag == 1 && s.MI_Id == mch.MI_Id).OrderBy(d => d.ASMC_Order).ToList();
                mch.sectionDrpDwn = allSection.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return mch;
        }

        public MasterClassHeldDTO getNoOfClassHeld(MasterClassHeldDTO dto)
        {
            if (dto.AMAY_Id > 0 && dto.ASMCL_Id > 0 && dto.ASMS_Id > 0 && dto.selectedmonthList.Length > 0)
            {
                List<decimal> classheld = new List<decimal>();
                for (int i = 0; i < dto.selectedmonthList.Length; i++)
                {
                    var count = _context.masterclassHeld.Where(d => d.ASMAY_Id == dto.AMAY_Id && d.ASMCL_Id == dto.ASMCL_Id && d.ASMS_Id == dto.ASMS_Id && d.IVRM_Month_Id == dto.selectedmonthList[i].IVRM_Month_Id && d.MI_Id == dto.MI_Id).ToList();
                    if (count.Count > 0)
                    {
                        var No = count.FirstOrDefault().ASCH_ClassHeld;
                        classheld.Add(No);
                        dto.NoOfClassHeldCount = classheld.ToArray();
                    }
                    else
                    {
                        var No = 0;
                        classheld.Add(No);
                        dto.NoOfClassHeldCount = classheld.ToArray();
                    }
                }
            }
            return dto;
        }

        public MasterClassHeldDTO saveData(MasterClassHeldDTO mst)
        {
            try
            {

                MonthDTO[] month = mst.selectedmonthList;
                List<string> msg = new List<string>();
                //   MasterSectionDTO[] section = mst.selectedSectionList;
                for (int i = 0; i < month.Length; i++)
                {
                    //for (int j = 0; j < section.Length; j++)
                    //{
                    var checkduplicates = _context.masterclassHeld.Where(d => d.ASMAY_Id == mst.AMAY_Id && d.ASMCL_Id == mst.ASMCL_Id && d.ASMS_Id == mst.ASMS_Id && d.IVRM_Month_Id == month[i].IVRM_Month_Id && d.MI_Id == mst.MI_Id).ToList();
                    var year = _context.AcademicYear.Where(y => y.ASMAY_Id == mst.AMAY_Id).ToList();
                    var classname = _context.School_M_Class.Where(c => c.ASMCL_Id == mst.ASMCL_Id).ToList();
                    var sectionname = _context.Section.Where(s => s.ASMS_Id == mst.ASMS_Id).ToList();
                    var monthname = _context.month.Where(m => m.IVRM_Month_Id == month[i].IVRM_Month_Id).ToList();

                    // Check for attendance.
                    var attendanceDone = _context.Adm_studentAttendance.Where(d => d.MI_Id == mst.MI_Id && d.ASMAY_Id == mst.AMAY_Id && d.ASA_Att_Type.Equals("monthly") && d.ASMCL_Id == mst.ASMCL_Id && d.ASMS_Id == mst.ASMS_Id && d.ASA_FromDate.Value.Month == month[i].IVRM_Month_Id).ToList();

                    if (checkduplicates.Count > 0)
                    {
                        //   if(month[i].ASCH_ClassHeld   < checkduplicates.FirstOrDefault().ASCH_ClassHeld && attendanceDone.Count > 0)
                        //{
                        //    var mssg = "Attendance already done & Class held value for"+" "+monthname.FirstOrDefault().IVRM_Month_Name+ " should not be less than previous Class held value";
                        //    msg.Add(mssg);
                        //}
                        //else
                        //{
                        var result = _context.masterclassHeld.Single(t => t.ASCH_Id == checkduplicates.FirstOrDefault().ASCH_Id);
                        result.ASMAY_Id = mst.AMAY_Id;
                        result.ASCH_Active_Flag = true;
                        result.MI_Id = mst.MI_Id;


                        result.ASCH_ClassHeld = month[i].ASCH_ClassHeld;
                        result.ASMCL_Id = mst.ASMCL_Id;
                        result.ASMS_Id = mst.ASMS_Id;
                        result.IVRM_Month_Id = Convert.ToInt64(month[i].IVRM_Month_Id);
                        result.CreatedDate = result.CreatedDate;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            mst.returnVal = true;
                        }
                        else
                        {
                            mst.returnVal = false;
                        }
                        //}


                        //   mst.message = "Class Held For the Year"+" "+year.FirstOrDefault().ASMAY_Year+" "+"For class"+" "+classname.FirstOrDefault().ASMCL_ClassName+" "+"For Section"+" "+sectionname.FirstOrDefault().ASMC_SectionName+" "+"and For Month"+" "+monthname.FirstOrDefault().IVRM_Month_Name+" "+" already Entered";
                    }
                    else
                    {
                        MasterClassHeld mstclshld = new MasterClassHeld();
                        mstclshld.ASMAY_Id = mst.AMAY_Id;
                        mstclshld.ASCH_Active_Flag = true;
                        mstclshld.MI_Id = mst.MI_Id;
                        mstclshld.ASCH_ClassHeld = month[i].ASCH_ClassHeld;
                        mstclshld.ASMCL_Id = mst.ASMCL_Id;
                        mstclshld.ASMS_Id = mst.ASMS_Id;
                        mstclshld.IVRM_Month_Id = Convert.ToInt64(month[i].IVRM_Month_Id);
                        mstclshld.CreatedDate = DateTime.Now;
                        mstclshld.UpdatedDate = DateTime.Now;
                        _context.Add(mstclshld);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            mst.returnVal = true;
                        }
                        else
                        {
                            mst.returnVal = false;
                        }
                    }
                }
                if (msg.Count > 0)
                {
                    mst.msgcount = msg.Count;
                    mst.message = msg.ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return mst;
        }
    }
}
