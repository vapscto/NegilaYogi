using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.NAAC.LessonPlanner;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Services
{
    public class MasterSchoolTopicImpl : Interface.MasterSchoolTopicInterface
    {
        public LessonplannerContext _context;

        public MasterSchoolTopicImpl(LessonplannerContext context)
        {
            _context = context;
        }
        public MasterSchoolTopicDTO Getdetails(MasterSchoolTopicDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getunitlist = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMU_ActiveFlag == true).OrderBy(a => a.LPMU_Order).ToArray();

                data.getdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                   from b in _context.MasterSchoolTopicDMO
                                   from c in _context.SchoolMasterUnitDMO
                                   from d in _context.AcademicYear
                                   from e in _context.AdmissionClass
                                   where (a.ISMS_Id == b.ISMS_Id && c.LPMU_Id == b.LPMU_Id && b.ASMAY_Id == d.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id
                                   && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                   select new MasterSchoolTopicDTO
                                   {
                                       ISMS_SubjectName = a.ISMS_SubjectName + " : " + a.ISMS_SubjectCode,
                                       ISMS_Id = a.ISMS_Id,
                                       LPMMT_Id = b.LPMMT_Id,
                                       LPMMT_TopicName = b.LPMMT_TopicName,
                                       LPMMT_TotalPeriods = b.LPMMT_TotalPeriods,
                                       LPMMT_TotalHrs = b.LPMMT_TotalHrs,
                                       LPMMT_TopicDescription = b.LPMMT_TopicDescription,
                                       LPMMT_ActiveFlag = b.LPMMT_ActiveFlag,
                                       LPMMT_Order = b.LPMMT_Order,
                                       LPMU_UnitName = c.LPMU_UnitName,
                                       ASMAY_Year = d.ASMAY_Year,
                                       ASMCL_ClassName = e.ASMCL_ClassName,
                                       ASMCL_Id = b.ASMCL_Id,
                                       ASMAY_Id = b.ASMAY_Id,
                                       ASMAY_Order = d.ASMAY_Order
                                   }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterSchoolTopicDTO savedetails(MasterSchoolTopicDTO data)
        {
            try
            {
                if (data.LPMMT_Id > 0)
                {
                    var check_duplicate = _context.MasterSchoolTopicDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_TopicName == data.LPMMT_TopicName
                    && a.LPMMT_Id != data.LPMMT_Id && a.ISMS_Id == data.ISMS_Id && a.LPMU_Id == data.LPMU_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id).ToList();
                    if (check_duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _context.MasterSchoolTopicDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id);
                        result.LPMMT_TopicName = data.LPMMT_TopicName;
                        result.LPMMT_TopicDescription = data.LPMMT_TopicDescription;
                        result.LPMMT_TotalHrs = data.LPMMT_TotalHrs;
                        result.LPMMT_TotalPeriods = data.LPMMT_TotalPeriods;
                        result.LPMMT_UpdatedBy = data.Userid;
                        result.LPMU_Id = data.LPMU_Id;
                        result.ASMCL_Id = data.ASMCL_Id;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.UpdatedDate = DateTime.Now;

                        _context.Update(result);
                        int i = _context.SaveChanges();

                        if (i > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {

                    var check_duplicate = _context.MasterSchoolTopicDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_TopicName == data.LPMMT_TopicName && a.ISMS_Id == data.ISMS_Id && a.LPMU_Id == data.LPMU_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id).ToList();
                    if (check_duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        int k = 0;
                        var checkrowcount = _context.MasterSchoolTopicDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ASMCL_Id == data.ASMCL_Id && a.LPMU_Id == data.LPMU_Id).Count();
                        k = checkrowcount + 1;

                        MasterSchoolTopicDMO dmo = new MasterSchoolTopicDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMCL_Id = data.ASMCL_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.LPMMT_TopicName = data.LPMMT_TopicName;
                        dmo.LPMMT_TopicDescription = data.LPMMT_TopicDescription;
                        dmo.LPMMT_TotalPeriods = data.LPMMT_TotalPeriods;
                        dmo.LPMMT_TotalHrs = data.LPMMT_TotalHrs;
                        dmo.LPMMT_Order = k;
                        dmo.ISMS_Id = data.ISMS_Id;
                        dmo.LPMU_Id = data.LPMU_Id;
                        dmo.LPMMT_ActiveFlag = true;
                        dmo.LPMMT_CreatedBy = data.Userid;
                        dmo.LPMMT_UpdatedBy = data.Userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);

                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterSchoolTopicDTO editdeatils(MasterSchoolTopicDTO data)
        {
            try
            {
                var editdetails = _context.MasterSchoolTopicDMO.Where(a => a.LPMMT_Id == data.LPMMT_Id && a.MI_Id == data.MI_Id).ToList();

                data.editdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                    from b in _context.MasterSchoolTopicDMO
                                    where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.LPMMT_Id == data.LPMMT_Id)
                                    select new MasterSchoolTopicDTO
                                    {
                                        ISMS_SubjectName = a.ISMS_SubjectName + " : " + a.ISMS_SubjectCode,
                                        ISMS_Id = a.ISMS_Id,
                                        LPMMT_Id = b.LPMMT_Id,
                                        LPMMT_TopicName = b.LPMMT_TopicName,
                                        LPMMT_TotalPeriods = b.LPMMT_TotalPeriods,
                                        LPMMT_TotalHrs = b.LPMMT_TotalHrs,
                                        LPMMT_TopicDescription = b.LPMMT_TopicDescription,
                                        LPMMT_ActiveFlag = b.LPMMT_ActiveFlag,
                                        LPMMT_Order = b.LPMMT_Order,
                                        LPMU_Id = b.LPMU_Id,
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMCL_Id = b.ASMCL_Id
                                    }).Distinct().ToArray();

                //data.editdetails = _context.MasterSchoolTopicDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id).ToArray();


                data.getclass = (from a in _context.Masterclasscategory
                                 from b in _context.AcademicYear
                                 from c in _context.AdmissionClass
                                 where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true && a.ASMAY_Id == editdetails.FirstOrDefault().ASMAY_Id)
                                 select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterSchoolTopicDTO deactivate(MasterSchoolTopicDTO data)
        {
            try
            {
                var result = _context.MasterSchoolTopicDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id);
                if (result.LPMMT_ActiveFlag == true)
                {
                    result.LPMMT_ActiveFlag = false;
                }
                else
                {
                    result.LPMMT_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.LPMMT_UpdatedBy = data.Userid;
                _context.Update(result);
                int i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public MasterSchoolTopicDTO gettopicdetails(MasterSchoolTopicDTO data)
        {
            try
            {
                data.getsubjecttopicdetails = _context.MasterSchoolTopicDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id).OrderBy(a => a.LPMMT_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterSchoolTopicDTO validateordernumber(MasterSchoolTopicDTO data)
        {
            try
            {
                int k = 0;
                if (data.MasterSchoolTopicOrderDTO.Count() > 0)
                {
                    for (int i = 0; i < data.MasterSchoolTopicOrderDTO.Count(); i++)
                    {
                        k = k + 1;
                        data.LPMMT_Id = data.MasterSchoolTopicOrderDTO[i].LPMMT_Id;
                        var result = _context.MasterSchoolTopicDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id);
                        result.LPMMT_Order = k;
                        result.LPMMT_UpdatedBy = data.Userid;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                    }
                    int j = _context.SaveChanges();
                    if (j > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterSchoolTopicDTO onchangeyear(MasterSchoolTopicDTO data)
        {
            try
            {
                data.getclass = (from a in _context.Masterclasscategory
                                 from b in _context.AcademicYear
                                 from c in _context.AdmissionClass
                                 where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                 select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterSchoolTopicDTO onchangeclass(MasterSchoolTopicDTO data)
        {
            try
            {
                data.getsubject = (from a in _context.Exm_Category_ClassDMO
                                   from b in _context.Exm_Yearly_CategoryDMO
                                   from c in _context.Exm_Yearly_Category_GroupDMO
                                   from d in _context.Exm_Yearly_Category_Group_SubjectsDMO
                                   from e in _context.IVRM_School_Master_SubjectsDMO
                                   where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCG_Id == d.EYCG_Id && d.ISMS_Id == e.ISMS_Id
                                   && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true && d.EYCGS_ActiveFlg == true
                                   && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                   select e).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // College
        public LP_Master_MainTopic_CollegeDTO getcollegedetails(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getunitlist = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMU_ActiveFlag == true).OrderBy(a => a.LPMU_Order).ToArray();

                data.getdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                   from b in _context.LP_Master_MainTopic_CollegeDMO
                                   from c in _context.SchoolMasterUnitDMO
                                   from d in _context.AcademicYear
                                   from e in _context.MasterCourseDMO
                                   from f in _context.ClgMasterBranchDMO
                                   from g in _context.CLG_Adm_Master_SemesterDMO
                                   where (a.ISMS_Id == b.ISMS_Id && c.LPMU_Id == b.LPMU_Id && b.ASMAY_Id == d.ASMAY_Id && b.AMCO_Id == e.AMCO_Id && b.AMB_Id == f.AMB_Id
                                   && b.AMSE_Id == g.AMSE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                   select new LP_Master_MainTopic_CollegeDTO
                                   {
                                       ISMS_SubjectName = a.ISMS_SubjectName + " : " + a.ISMS_SubjectCode,
                                       ISMS_Id = a.ISMS_Id,
                                       LPMMTC_Id = b.LPMMTC_Id,
                                       LPMMTC_TopicName = b.LPMMTC_TopicName,
                                       LPMMTC_TotalPeriods = b.LPMMTC_TotalPeriods,
                                       LPMMTC_TotalHrs = b.LPMMTC_TotalHrs,
                                       LPMMTC_TopicDescription = b.LPMMTC_TopicDescription,
                                       LPMMTC_ActiveFlg = b.LPMMTC_ActiveFlg,
                                       LPMMTC_Order = b.LPMMTC_Order,
                                       LPMU_UnitName = c.LPMU_UnitName,
                                       ASMAY_Year = d.ASMAY_Year,
                                       AMCO_CourseName = e.AMCO_CourseName,
                                       AMB_BranchName = f.AMB_BranchName,
                                       AMSE_SEMName = g.AMSE_SEMName,
                                       AMCO_Id = b.AMCO_Id,
                                       ASMAY_Id = b.ASMAY_Id,
                                       AMB_Id = b.AMB_Id,
                                       AMSE_Id = b.AMSE_Id,
                                       ASMAY_Order = d.ASMAY_Order
                                   }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO onchangecollegeyear(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                  && a.MI_Id == data.MI_Id)
                                  select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO onchangecourse(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                data.getbranch = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  from e in _context.ClgMasterBranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                  && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                  && a.AMCO_Id == data.AMCO_Id)
                                  select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO onchangebranch(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                data.getsemester = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                    from b in _context.MasterCourseDMO
                                    from c in _context.AcademicYear
                                    from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from e in _context.ClgMasterBranchDMO
                                    from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from g in _context.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                    && d.ACAYCB_Id == f.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id && f.ACAYCBS_ActiveFlag == true && a.MI_Id == data.MI_Id
                                    && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                    select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO onchangesemester(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                data.getsubject = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO savecollegedetails(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.LPMMTC_Id > 0)
                {
                    var check_duplicate = _context.LP_Master_MainTopic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_TopicName == data.LPMMTC_TopicName
                    && a.LPMMTC_Id != data.LPMMTC_Id && a.ISMS_Id == data.ISMS_Id && a.LPMU_Id == data.LPMU_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id).ToList();
                    if (check_duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var result = _context.LP_Master_MainTopic_CollegeDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id);
                        result.LPMMTC_TopicName = data.LPMMTC_TopicName;
                        result.LPMMTC_TopicDescription = data.LPMMTC_TopicDescription;
                        result.LPMMTC_TotalHrs = data.LPMMTC_TotalHrs;
                        result.LPMMTC_TotalPeriods = data.LPMMTC_TotalPeriods;
                        result.LPMMTC_UpdatedBy = data.LPMMTC_CreatedBy;
                        result.LPMU_Id = data.LPMU_Id;
                        result.AMCO_Id = data.AMCO_Id;
                        result.AMB_Id = data.AMB_Id;
                        result.AMSE_Id = data.AMSE_Id;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.UpdatedDate = indianTime;
                        _context.Update(result);
                        int i = _context.SaveChanges();

                        if (i > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var check_duplicate = _context.LP_Master_MainTopic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_TopicName == data.LPMMTC_TopicName
                    && a.ISMS_Id == data.ISMS_Id && a.LPMU_Id == data.LPMU_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                    && a.AMSE_Id == data.AMSE_Id).ToList();
                    if (check_duplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        int k = 0;
                        var checkrowcount = _context.LP_Master_MainTopic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_TopicName == data.LPMMTC_TopicName
                        && a.ISMS_Id == data.ISMS_Id && a.LPMU_Id == data.LPMU_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                        && a.AMSE_Id == data.AMSE_Id).Count();
                        k = checkrowcount + 1;
                        LP_Master_MainTopic_CollegeDMO dmo = new LP_Master_MainTopic_CollegeDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.AMCO_Id = data.AMCO_Id;
                        dmo.AMB_Id = data.AMB_Id;
                        dmo.AMSE_Id = data.AMSE_Id;
                        dmo.LPMMTC_TopicName = data.LPMMTC_TopicName;
                        dmo.LPMMTC_TopicDescription = data.LPMMTC_TopicDescription;
                        dmo.LPMMTC_TotalPeriods = data.LPMMTC_TotalPeriods;
                        dmo.LPMMTC_TotalHrs = data.LPMMTC_TotalHrs;
                        dmo.LPMMTC_Order = k;
                        dmo.ISMS_Id = data.ISMS_Id;
                        dmo.LPMU_Id = data.LPMU_Id;
                        dmo.LPMMTC_ActiveFlg = true;
                        dmo.LPMMTC_CreatedBy = data.LPMMTC_CreatedBy;
                        dmo.LPMMTC_UpdatedBy = data.LPMMTC_CreatedBy;
                        dmo.CreatedDate = indianTime;
                        dmo.UpdatedDate = indianTime;
                        _context.Add(dmo);

                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO editcollegedeatils(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                var editdetails = _context.LP_Master_MainTopic_CollegeDMO.Where(a => a.LPMMTC_Id == data.LPMMTC_Id && a.MI_Id == data.MI_Id).ToList();

                data.editdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                    from b in _context.LP_Master_MainTopic_CollegeDMO
                                    where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.LPMMTC_Id == data.LPMMTC_Id)
                                    select new LP_Master_MainTopic_CollegeDTO
                                    {
                                        ISMS_SubjectName = a.ISMS_SubjectName + " : " + a.ISMS_SubjectCode,
                                        ISMS_Id = a.ISMS_Id,
                                        LPMMTC_Id = b.LPMMTC_Id,
                                        LPMMTC_TopicName = b.LPMMTC_TopicName,
                                        LPMMTC_TotalPeriods = b.LPMMTC_TotalPeriods,
                                        LPMMTC_TotalHrs = b.LPMMTC_TotalHrs,
                                        LPMMTC_TopicDescription = b.LPMMTC_TopicDescription,
                                        LPMMTC_ActiveFlg = b.LPMMTC_ActiveFlg,
                                        LPMMTC_Order = b.LPMMTC_Order,
                                        LPMU_Id = b.LPMU_Id,
                                        ASMAY_Id = b.ASMAY_Id,
                                        AMCO_Id = b.AMCO_Id,
                                        AMB_Id = b.AMB_Id,
                                        AMSE_Id = b.AMSE_Id
                                    }).Distinct().ToArray();


                data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                  && a.MI_Id == data.MI_Id)
                                  select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                data.getbranch = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                  from b in _context.MasterCourseDMO
                                  from c in _context.AcademicYear
                                  from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  from e in _context.ClgMasterBranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                  && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                  && a.AMCO_Id == data.AMCO_Id)
                                  select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();

                data.getsemester = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                    from b in _context.MasterCourseDMO
                                    from c in _context.AcademicYear
                                    from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from e in _context.ClgMasterBranchDMO
                                    from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    from g in _context.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                    && d.ACAYCB_Id == f.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id && f.ACAYCBS_ActiveFlag == true && a.MI_Id == data.MI_Id
                                    && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                    select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO collegedeactivate(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var result = _context.LP_Master_MainTopic_CollegeDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id);
                if (result.LPMMTC_ActiveFlg == true)
                {
                    result.LPMMTC_ActiveFlg = false;
                }
                else
                {
                    result.LPMMTC_ActiveFlg = true;
                }
                result.UpdatedDate = indianTime;
                result.LPMMTC_UpdatedBy = data.LPMMTC_CreatedBy;
                _context.Update(result);
                int i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO getcollegetopicdetails(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                data.getsubjecttopicdetails = _context.LP_Master_MainTopic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id 
                && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id==data.AMB_Id && a.AMSE_Id==data.AMSE_Id).OrderBy(a => a.LPMMTC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_Master_MainTopic_CollegeDTO validatecollegeordernumber(LP_Master_MainTopic_CollegeDTO data)
        {
            try
            {
                int k = 0;
                if (data.MastercollegeTopicOrderDTO.Count() > 0)
                {
                    for (int i = 0; i < data.MastercollegeTopicOrderDTO.Count(); i++)
                    {
                        k = k + 1;
                        data.LPMMTC_Id = data.MastercollegeTopicOrderDTO[i].LPMMTC_Id;
                        var result = _context.LP_Master_MainTopic_CollegeDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id);
                        result.LPMMTC_Order = k;
                        result.LPMMTC_UpdatedBy = data.LPMMTC_CreatedBy;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                    }
                    int j = _context.SaveChanges();
                    if (j > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
