using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class CLGTTCommonImpl : Interfaces.CLGTTCommonInterface
    {
        private static ConcurrentDictionary<string, CLGTTCommonDTO> _login =
             new ConcurrentDictionary<string, CLGTTCommonDTO>();

        public TTContext _TTContext;
        ILogger<CLGTTCommonImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGTTCommonImpl(TTContext academiccontext, ILogger<CLGTTCommonImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
      

        #region GET BRANCH
        public CLGTTCommonDTO getBranch(CLGTTCommonDTO data)
        {
            try
            {


                data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _TTContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
            

              

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
       

        #region GET Course based on category mapping
        public CLGTTCommonDTO getcourse_catg(CLGTTCommonDTO data)
        {
            try
            {
                data.courselist = (from a in _TTContext.CLGTT_Category_CourseBranchDMO
                                   from b in _TTContext.MasterCourseDMO
                                   where b.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == data.TTMC_Id &&  a.TTCC_ActiveFlag == true && b.AMCO_ActiveFlag == true
                                   select b
                                 ).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 

        #region GET Branch based on category mapping
        public CLGTTCommonDTO getbranch_catg(CLGTTCommonDTO data)
        {
            try
            {
                data.branchlist = (from a in _TTContext.CLGTT_Category_CourseBranchDMO
                                   from b in _TTContext.ClgMasterBranchDMO
                                   where b.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == data.TTMC_Id &&  a.TTCC_ActiveFlag == true && b.AMB_ActiveFlag == true && a.AMB_Id==b.AMB_Id
                                   select b
                                 ).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion   
        #region GET Branch based on category mapping
        public CLGTTCommonDTO multplegetbranch_catg(CLGTTCommonDTO data)
        {
            try
            {
                List<long> CIDS = new List<long>();
                foreach (var item in data.crids)
                {
                    CIDS.Add(item.AMCO_Id);
                }

                data.branchlist = (from a in _TTContext.CLGTT_Category_CourseBranchDMO
                                   from b in _TTContext.ClgMasterBranchDMO
                                   where b.MI_Id == data.MI_Id && CIDS.Contains(a.AMCO_Id) && a.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == data.TTMC_Id &&  a.TTCC_ActiveFlag == true && b.AMB_ActiveFlag == true && a.AMB_Id==b.AMB_Id
                                   select b
                                 ).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion
        
        #region GET semister 
        public CLGTTCommonDTO get_semister(CLGTTCommonDTO data)
        {
            try
            {
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                     from b in _TTContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _TTContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag==true && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag==true)
                                     select a).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion     
        #region GET semister 
        public CLGTTCommonDTO multget_semister(CLGTTCommonDTO data)
        {
            try
            {
                List<long> CIDS = new List<long>();
                foreach (var item in data.crids)
                {
                    CIDS.Add(item.AMCO_Id);
                }

                List<long> BIDS = new List<long>();
                foreach (var item in data.brnchds)
                {
                    BIDS.Add(item.AMB_Id);
                }
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                     from b in _TTContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _TTContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag==true &&  CIDS.Contains(b.AMCO_Id) && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id &&  BIDS.Contains(c.AMB_Id) && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag==true)
                                     select a).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion  
        #region GET Section 
        public CLGTTCommonDTO get_section(CLGTTCommonDTO data)
        {
            try
            {
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                     from b in _TTContext.CLG_Adm_College_AY_CourseDMO
                                     from c in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _TTContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select a).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion    
        
        #region GET STAFF BASED ON PERIOD DISTRIBUTION
        public CLGTTCommonDTO get_staff(CLGTTCommonDTO data)
        {
            try
            {
                data.stafflist = (from a in _TTContext.HR_Master_Employee_DMO
                                  from b in _TTContext.TT_Master_Staff_AbbreviationDMO
                                  from c in _TTContext.TT_Final_Period_DistributionDMO
                                  from d in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                  where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && c.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id && d.ACMS_Id == data.ACMS_Id && d.TTMC_Id == data.TTMC_Id && c.TTFPD_Id == d.TTFPD_Id && c.TTFPD_ActiveFlag == true && b.TTMSAB_ActiveFlag==true)
                                     select new CLGPRDDistributionDTO
                                     {
                                         empName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                         HRME_Id = b.HRME_Id,
                                         TTMSAB_Abbreviation = b.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(j => j.empName).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion
        #region GET subject BASED ON PERIOD DISTRIBUTION WITH STAFF SELECTION
        public CLGTTCommonDTO get_subject(CLGTTCommonDTO data)
        {
            try
            {
                data.subjectlist = (from a in _TTContext.IVRM_School_Master_SubjectsDMO
                                  from b in _TTContext.TT_Master_Subject_AbbreviationDMO
                                  from c in _TTContext.TT_Final_Period_DistributionDMO
                                  from d in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                  where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && c.HRME_Id == data.HRME_Id && c.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id && d.ACMS_Id == data.ACMS_Id && d.TTMC_Id == data.TTMC_Id && c.TTFPD_Id == d.TTFPD_Id && c.TTFPD_ActiveFlag == true && b.TTMSUAB_ActiveFlag==true && a.ISMS_Id==b.ISMS_Id && a.ISMS_Id==d.ISMS_Id)
                                     select new CLGPRDDistributionDTO
                                     {
                                         ISMS_Id = a.ISMS_Id,
                                         TTMSUAB_Abbreviation = b.TTMSUAB_Abbreviation,
                                         ISMS_SubjectName=a.ISMS_SubjectName
                                     }).Distinct().OrderBy(j => j.ISMS_SubjectName).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion  
        #region GET subject BASED ON PERIOD DISTRIBUTION  ALL STAFF
        public CLGTTCommonDTO get_subject_onsec(CLGTTCommonDTO data)
        {
            try
            {
                data.subjectlist = (from a in _TTContext.IVRM_School_Master_SubjectsDMO
                                  from b in _TTContext.TT_Master_Subject_AbbreviationDMO
                                  from c in _TTContext.TT_Final_Period_DistributionDMO
                                  from d in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                  where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && c.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id && d.ACMS_Id == data.ACMS_Id && d.TTMC_Id == data.TTMC_Id && c.TTFPD_Id == d.TTFPD_Id && c.TTFPD_ActiveFlag == true && b.TTMSUAB_ActiveFlag==true && a.ISMS_Id==b.ISMS_Id && a.ISMS_Id==d.ISMS_Id)
                                     select new CLGPRDDistributionDTO
                                     {
                                         ISMS_Id = a.ISMS_Id,
                                         TTMSUAB_Abbreviation = b.TTMSUAB_Abbreviation,
                                         ISMS_SubjectName=a.ISMS_SubjectName
                                     }).Distinct().OrderBy(j => j.ISMS_SubjectName).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region GET MASTER DAY BASED ON SEMISTER SELECTION
        public CLGTTCommonDTO get_semday(CLGTTCommonDTO data)
        {
            try
            {
                
                data.daydropdown = (from a in _TTContext.CLGTT_Master_Day_CourseBranchDMO
                                   from b in _TTContext.TT_Master_DayDMO
                                   where  b.MI_Id == data.MI_Id && a.AMCO_Id==data.AMCO_Id && a.AMB_Id==data.AMB_Id && a.AMSE_Id==data.AMSE_Id && a.TTMD_Id==b.TTMD_Id && a.TTMDC_ActiveFlag==true && a.ASMAY_Id==data.ASMAY_Id
                                   select b
                                 ).Distinct().ToArray();
                

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
        
        #region GET STAFF BASED ON ACADEMIC YEAR SELECTION
        public CLGTTCommonDTO get_staffaca(CLGTTCommonDTO data)
        {
            try
            {

                data.stafflist = (from a in _TTContext.HR_Master_Employee_DMO
                                  from b in _TTContext.TT_Master_Staff_AbbreviationDMO
                                  from c in _TTContext.TT_Final_Period_DistributionDMO
                                  where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && c.ASMAY_Id == data.ASMAY_Id && c.TTFPD_ActiveFlag == true && b.TTMSAB_ActiveFlag == true)
                                  select new CLGPRDDistributionDTO
                                  {
                                      empName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                      HRME_Id = b.HRME_Id,
                                      TTMSAB_Abbreviation = b.TTMSAB_Abbreviation
                                  }).Distinct().OrderBy(j => j.empName).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region GET COURSE BASED ON  STAFF SELECTION
        public CLGTTCommonDTO get_course_onstaff(CLGTTCommonDTO data)
        {
            try
            {
                data.courselist = (from a in _TTContext.MasterCourseDMO
                                  from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && b.TTFPD_Id==c.TTFPD_Id && b.ASMAY_Id==data.ASMAY_Id && b.TTFPD_ActiveFlag==true && b.HRME_Id==data.HRME_Id && a.AMCO_Id==c.AMCO_Id
                                   select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion  
        
        #region GET BRANCH BASED ON  STAFF/COURSE SELECTION
        public CLGTTCommonDTO get_branch_onstaff(CLGTTCommonDTO data)
        {
            try
            {
                data.branchlist = (from a in _TTContext.ClgMasterBranchDMO
                                  from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && b.TTFPD_Id==c.TTFPD_Id && b.ASMAY_Id==data.ASMAY_Id && b.TTFPD_ActiveFlag==true && b.HRME_Id==data.HRME_Id && c.AMCO_Id==data.AMCO_Id && a.AMB_Id==c.AMB_Id
                                   select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
        #region GET SEM BASED ON  STAFF/COURSE/BRANCH SELECTION
        public CLGTTCommonDTO get_sem_onstaff(CLGTTCommonDTO data)
        {
            try
            {
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                  from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && b.TTFPD_Id==c.TTFPD_Id && b.ASMAY_Id==data.ASMAY_Id && b.TTFPD_ActiveFlag==true && b.HRME_Id==data.HRME_Id && c.AMCO_Id==data.AMCO_Id && a.AMSE_Id==c.AMSE_Id && c.AMB_Id==data.AMB_Id
                                     select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion
        #region GET SECTION BASED ON  STAFF/COURSE/BRANCH/SEM SELECTION
        public CLGTTCommonDTO get_sec_onstaff(CLGTTCommonDTO data)
        {
            try
            {
                data.sectionlist = (from a in _TTContext.Adm_College_Master_SectionDMO
                                  from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && b.TTFPD_Id==c.TTFPD_Id && b.ASMAY_Id==data.ASMAY_Id && b.TTFPD_ActiveFlag==true && b.HRME_Id==data.HRME_Id && c.AMCO_Id==data.AMCO_Id && a.ACMS_Id==c.ACMS_Id && c.AMB_Id==data.AMB_Id && c.AMSE_Id==data.AMSE_Id
                                     select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion 
        
        #region GET SUBJECT BASED ON  STAFF/COURSE/BRANCH/SEM SELECTION
        public CLGTTCommonDTO get_subject_onstaff(CLGTTCommonDTO data)
        {
            try
            {
                data.subjectlist = (from a in _TTContext.IVRM_School_Master_SubjectsDMO
                                  from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && b.TTFPD_Id==c.TTFPD_Id && b.ASMAY_Id==data.ASMAY_Id && b.TTFPD_ActiveFlag==true && b.HRME_Id==data.HRME_Id && c.AMCO_Id==data.AMCO_Id && a.ISMS_Id==c.ISMS_Id && c.AMB_Id==data.AMB_Id && c.AMSE_Id==data.AMSE_Id && c.ACMS_Id==data.ACMS_Id
                                     select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion    
        
        #region GET SUBJECT BASED ON  ACADEMIC YEAR SELECTION
        public CLGTTCommonDTO get_subjecttab3(CLGTTCommonDTO data)
        {
            try
            {
                data.subjectlist = (from a in _TTContext.IVRM_School_Master_SubjectsDMO
                                  from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id==b.MI_Id && a.MI_Id==data.MI_Id && b.TTFPD_Id==c.TTFPD_Id && b.ASMAY_Id==data.ASMAY_Id && b.TTFPD_ActiveFlag==true   && a.ISMS_Id==c.ISMS_Id 
                                     select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region GET COURSE BASED ON  SUBJECT SELECTION
        public CLGTTCommonDTO get_course_onsubject(CLGTTCommonDTO data)
        {
            try
            {
                data.courselist = (from a in _TTContext.MasterCourseDMO
                                   from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.TTFPD_Id == c.TTFPD_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTFPD_ActiveFlag == true && c.ISMS_Id == data.ISMS_Id && a.AMCO_Id == c.AMCO_Id
                                   select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion  
        
        #region GET BRANCH BASED ON  SUBJECT SELECTION
        public CLGTTCommonDTO get_branch_onsubject(CLGTTCommonDTO data)
        {
            try
            {
                data.branchlist = (from a in _TTContext.ClgMasterBranchDMO
                                   from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.TTFPD_Id == c.TTFPD_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTFPD_ActiveFlag == true && c.ISMS_Id == data.ISMS_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id==a.AMB_Id
                                   select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion    
        #region GET SEM BASED ON  SUBJECT SELECTION
        public CLGTTCommonDTO get_sem_onsubject(CLGTTCommonDTO data)
        {
            try
            {
                data.semisterlist = (from a in _TTContext.CLG_Adm_Master_SemesterDMO
                                   from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.TTFPD_Id == c.TTFPD_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTFPD_ActiveFlag == true && c.ISMS_Id == data.ISMS_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id==data.AMB_Id && a.AMSE_Id==c.AMSE_Id
                                   select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion   
        #region GET SECTION BASED ON  SUBJECT SELECTION
        public CLGTTCommonDTO get_sec_onsubject(CLGTTCommonDTO data)
        {
            try
            {
                data.sectionlist = (from a in _TTContext.Adm_College_Master_SectionDMO
                                   from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.TTFPD_Id == c.TTFPD_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTFPD_ActiveFlag == true && c.ISMS_Id == data.ISMS_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id==data.AMB_Id && c.AMSE_Id==data.AMSE_Id && a.ACMS_Id==c.ACMS_Id
                                   select a).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion   
        
        #region GET STAFF BASED ON  SUBJECT SELECTION
        public CLGTTCommonDTO get_staff_onsubject(CLGTTCommonDTO data)
        {
            try
            {
                data.stafflist = (from a in _TTContext.HR_Master_Employee_DMO
                                   from b in _TTContext.TT_Final_Period_DistributionDMO
                                   from c in _TTContext.CLGTT_PRDDistributionDetailsDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.TTFPD_Id == c.TTFPD_Id && b.ASMAY_Id == data.ASMAY_Id && b.TTFPD_ActiveFlag == true && c.ISMS_Id == data.ISMS_Id && c.AMCO_Id == data.AMCO_Id && c.AMB_Id==data.AMB_Id && c.AMSE_Id==data.AMSE_Id && c.ACMS_Id==data.ACMS_Id && a.HRME_Id==b.HRME_Id
                                    select new CLGTTCommonDTO
                                    {
                                        staffName = a.HRME_EmployeeFirstName + "  " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "  " || a.HRME_EmployeeMiddleName == "0" ? "  " : a.HRME_EmployeeMiddleName) + "  " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "  " || a.HRME_EmployeeLastName == "0" ? "  " : a.HRME_EmployeeLastName),
                                        HRME_Id = b.HRME_Id,
                                    }).Distinct().OrderBy(j => j.staffName).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion  
    }
}
