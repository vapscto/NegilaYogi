using AutoMapper;
using CollegeExamServiceHub.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgSubjectMasterImpl : ClgSubjectMasterInterface
    {
        private static ConcurrentDictionary<string, ClgSubjectMasterInterface> _login =
         new ConcurrentDictionary<string, ClgSubjectMasterInterface>();

        private readonly ClgExamContext _MasterSubjectAllMContext;
        public ClgSubjectMasterImpl(ClgExamContext subjectmasterContext)
        {
            _MasterSubjectAllMContext = subjectmasterContext;
        }
        public MasterSubjectAllMDTO DeleteMasterSubDetails(int id)
        {
            MasterSubjectAllMDTO data = new MasterSubjectAllMDTO();
            data.ISMS_Id = id;
            data.already_cnt = false;

            //IVRM_Master_SubjectsDMO pge = Mapper.Map<IVRM_Master_SubjectsDMO>(data);
            if (data.ISMS_Id > 0)
            {
                var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Single(t => t.ISMS_Id == data.ISMS_Id);
                if (result.ISMS_ActiveFlag == 1)
                {
                    var TT_Restricting_Period_SubjectDMO_cnt = _MasterSubjectAllMContext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Restricting_Period_Staff_ClassSectionDMO_cnt = _MasterSubjectAllMContext.TT_Restricting_Period_Staff_ClassSectionDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Restricting_Day_SubjectDMO_cnt = _MasterSubjectAllMContext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Restricting_Day_Staff_ClassSectionDMO_cnt = _MasterSubjectAllMContext.TT_Restricting_Day_Staff_ClassSectionDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Restricting_Day_PeriodDMO_cnt = _MasterSubjectAllMContext.TT_Restricting_Day_PeriodDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Master_Subject_AbbreviationDMO_cnt = _MasterSubjectAllMContext.TT_Master_Subject_AbbreviationDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_LABLIB_DetailsDMO_cnt = _MasterSubjectAllMContext.TT_LABLIB_DetailsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Fixing_Period_SubjectDMO_cnt = _MasterSubjectAllMContext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Fixing_Period_Staff_ClassSectionDMO_cnt = _MasterSubjectAllMContext.TT_Fixing_Period_Staff_ClassSectionDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Fixing_Day_SubjectDMO_cnt = _MasterSubjectAllMContext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Fixing_Day_Staff_ClassSectionDMO_cnt = _MasterSubjectAllMContext.TT_Fixing_Day_Staff_ClassSectionDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Fixing_Day_PeriodDMO_cnt = _MasterSubjectAllMContext.TT_Fixing_Day_PeriodDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Final_Period_Distribution_DetailedDMO_cnt = _MasterSubjectAllMContext.TT_Final_Period_Distribution_DetailedDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Final_Generation_DetailedDMO_cnt = _MasterSubjectAllMContext.TT_Final_Generation_DetailedDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_ConsecutiveDMO_cnt = _MasterSubjectAllMContext.TT_ConsecutiveDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var TT_Bifurcation_DetailsDMO_cnt = _MasterSubjectAllMContext.TT_Bifurcation_Details_DMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Preadmission_Subjectwise_Written_MarksDMO_cnt = _MasterSubjectAllMContext.WIrttenTestSubjectWiseMarksDMO.Where(t => t.ISMS_ID == data.ISMS_Id).ToList();
                    var Exm_Yrly_Cat_Exams_SubwiseDMO_cnt = _MasterSubjectAllMContext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Yearly_Category_Group_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_Col_Master_Group_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Studentwise_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_Col_Studentwise_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Student_Marks_Process_SubjectwiseDMO_cnt = _MasterSubjectAllMContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Student_MarksDMO_cnt = _MasterSubjectAllMContext.ExamMarksDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Master_Group_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_Col_Master_Group_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_M_Promotion_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_M_Promotion_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();

                    if (TT_Restricting_Period_SubjectDMO_cnt.Count == 0 && TT_Restricting_Period_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Restricting_Day_SubjectDMO_cnt.Count == 0 &&
                                            TT_Restricting_Day_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Restricting_Day_PeriodDMO_cnt.Count == 0 && TT_Master_Subject_AbbreviationDMO_cnt.Count == 0 &&
                                            TT_LABLIB_DetailsDMO_cnt.Count == 0 && TT_Fixing_Period_SubjectDMO_cnt.Count == 0 && TT_Fixing_Period_Staff_ClassSectionDMO_cnt.Count == 0 &&
                                            TT_Fixing_Day_SubjectDMO_cnt.Count == 0 && TT_Fixing_Day_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Fixing_Day_PeriodDMO_cnt.Count == 0 &&
                                            TT_Final_Period_Distribution_DetailedDMO_cnt.Count == 0 && TT_Final_Generation_DetailedDMO_cnt.Count == 0 && TT_ConsecutiveDMO_cnt.Count == 0 &&
                                            TT_Bifurcation_DetailsDMO_cnt.Count == 0 && Preadmission_Subjectwise_Written_MarksDMO_cnt.Count == 0 && Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 &&
                                            Exm_Yearly_Category_Group_SubjectsDMO_cnt.Count == 0 && Exm_Studentwise_SubjectsDMO_cnt.Count == 0 && Exm_Student_Marks_Process_SubjectwiseDMO_cnt.Count == 0
                                            && Exm_Student_MarksDMO_cnt.Count == 0 && Exm_Master_Group_SubjectsDMO_cnt.Count == 0 && Exm_M_Promotion_SubjectsDMO_cnt.Count == 0)
                    {
                        result.ISMS_ActiveFlag = 0;
                        result.UpdatedDate = DateTime.Now;
                        _MasterSubjectAllMContext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }

                    //result.ISMS_ActiveFlag = 0;
                }
                else
                {
                    result.ISMS_ActiveFlag = 1;
                    result.UpdatedDate = DateTime.Now;
                    _MasterSubjectAllMContext.Update(result);
                }
                // result.UpdatedDate = DateTime.Now;
                //_MasterSubjectAllMContext.Update(result);
                var flag = _MasterSubjectAllMContext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }

            return data;
        }
        public MasterSubjectAllMDTO EditMasterSubDetails(int id)
        {
            MasterSubjectAllMDTO mast = new MasterSubjectAllMDTO();
            try
            {
                List<IVRM_School_Master_SubjectsDMO> mastersub = new List<IVRM_School_Master_SubjectsDMO>();
                mastersub = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.AsNoTracking().Where(t => t.ISMS_Id.Equals(id)).ToList();

                mast.edit_m_subject = mastersub.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return mast;
        }
        public MasterSubjectAllMDTO getalldetails(int id)
        {
            MasterSubjectAllMDTO mast = new MasterSubjectAllMDTO();
            try
            {
                List<MasterSubjectDMO> sub_list = new List<MasterSubjectDMO>();
                sub_list = _MasterSubjectAllMContext.masterSubject.Where(t => t.MI_Id == id).OrderByDescending(a => a.CreatedDate).ToList();
                mast.MasterSubjectData = sub_list.ToArray();

                List<IVRM_School_Master_SubjectsDMO> mastersub = new List<IVRM_School_Master_SubjectsDMO>();
                mastersub = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(c => c.MI_Id == id).ToList();

                mast.subject_m_list = mastersub.Distinct().OrderBy(t => t.CreatedDate).ToArray();

                mast.subject_m_list_new = mastersub.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                mast.sub_list = (from a in _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO
                                 where (a.MI_Id == id && a.ISMS_ActiveFlag == 1)
                                 select new MasterSubjectAllMDTO { ISMS_SubjectName = a.ISMS_SubjectName, ISMS_Id = a.ISMS_Id }).Distinct().ToArray();

                mast.courst_list = (from a in _MasterSubjectAllMContext.MasterCourseDMO
                                    where (a.MI_Id == id && a.AMCO_ActiveFlag == true)
                                    select new MasterSubjectAllMDTO { AMCO_CourseName = a.AMCO_CourseName, AMCO_Id = a.AMCO_Id }).Distinct().ToArray();

                mast.branch_list = (from a in _MasterSubjectAllMContext.ClgMasterBranchDMO
                                    where (a.MI_Id == id && a.AMB_ActiveFlag == true)
                                    select new MasterSubjectAllMDTO { AMB_BranchName = a.AMB_BranchName, AMB_Id = a.AMB_Id }).Distinct().ToArray();

                mast.year_list = (from a in _MasterSubjectAllMContext.AcademicYear
                                  where (a.MI_Id == id && a.Is_Active == true)
                                  select new MasterSubjectAllMDTO { ASMAY_Year = a.ASMAY_Year, ASMAY_Id = a.ASMAY_Id }).Distinct().ToArray();

                mast.yearOfintro = (from a in _MasterSubjectAllMContext.AcademicYear
                                    where (a.MI_Id == id && a.Is_Active == true)
                                    select new MasterSubjectAllMDTO { yearname = a.ASMAY_Year, yearid = a.ASMAY_Id }).Distinct().ToArray();

                mast.mappinglistdata = (from a in _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO
                                        from b in _MasterSubjectAllMContext.IVRM_Master_Subjects_Branch_DMO
                                        from c in _MasterSubjectAllMContext.MasterCourseDMO
                                        from d in _MasterSubjectAllMContext.ClgMasterBranchDMO
                                        where (a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ISMS_Id == b.ISMS_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id
                                        && a.MI_Id == id)
                                        select new MasterSubjectAllMDTO
                                        {
                                            AMCO_CourseName = c.AMCO_CourseName,
                                            AMB_BranchName = d.AMB_BranchName,
                                            ISMS_SubjectName = a.ISMS_SubjectName,
                                            ISMS_SubjectCode = a.ISMS_SubjectCode,
                                            ISMS_IntroYear = a.ISMS_IntroYear,
                                            ISMS_DiscontinuedYear = a.ISMS_DiscontinuedYear,
                                        }).Distinct().OrderBy(t => t.ISMS_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return mast;
        }
        public MasterSubjectAllMDTO GetMasterSubDetails(MasterSubjectAllMDTO master)
        {
            //MasterSectionDTO mast = new MasterSectionDTO();
            //try
            //{
            //    List<MasterSubjectAllMDMO> mastersec = new List<MasterSubjectAllMDMO>();
            //    mastersec = _MasterSubjectAllMContext.MasterSubjectAllM.Where(t=>t.MI_Id==master.MI_Id).ToList();
            //    master.MasterSubjectData = mastersec.ToArray();
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return master;
        }
        public MasterSubjectAllMDTO SaveMasterSubDetails(MasterSubjectAllMDTO objpge)
        {
           
            try
            {
                if (objpge.ISMS_Id > 0)
                {
                    var result321 = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => (t.ISMS_SubjectName.Equals(objpge.ISMS_SubjectName) || t.ISMS_SubjectCode.Equals(objpge.ISMS_SubjectCode) || t.ISMS_IVRSSubjectName.Equals(objpge.ISMS_IVRSSubjectName)) && t.MI_Id.Equals(objpge.MI_Id) && t.ISMS_Id != objpge.ISMS_Id);
                    if (result321.Count() > 0)
                    {
                        var result3211 = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => (t.ISMS_SubjectName.Equals(objpge.ISMS_SubjectName)) && t.MI_Id.Equals(objpge.MI_Id) && t.ISMS_Id != objpge.ISMS_Id);
                        if (result3211.Count() > 0)
                        {
                            objpge.returnvaluetype = "subjectname";
                        }
                        else
                        {
                            objpge.returnvaluetype = "subjectcode";
                        }
                        objpge.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Single(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                        result.ISMS_SubjectName = objpge.ISMS_SubjectName;
                        result.ISMS_SubjectCode = objpge.ISMS_SubjectCode;
                        result.ISMS_IVRSSubjectName = objpge.ISMS_IVRSSubjectName;
                        result.ISMS_SubjectNameNew = objpge.ISMS_SubjectNameNew;
                        result.ISMS_Max_Marks = objpge.ISMS_Max_Marks;
                        result.ISMS_Min_Marks = objpge.ISMS_Min_Marks;
                        result.ISMS_ExamFlag = objpge.ISMS_ExamFlag.Value;
                        result.ISMS_PreadmFlag = objpge.ISMS_PreadmFlag.Value;
                        result.ISMS_SubjectFlag = objpge.ISMS_SubjectFlag.Value;
                        result.ISMS_TTFlag = objpge.ISMS_TTFlag.Value;
                        result.ISMS_AttendanceFlag = objpge.ISMS_AttendanceFlag.Value;
                        result.ISMS_LanguageFlg = objpge.ISMS_LanguageFlg.Value;
                        result.ISMS_AtExtraFeeFlg = objpge.ISMS_AtExtraFeeFlg.Value;
                        result.ISMS_BatchAppl = objpge.ISMS_BatchAppl.Value;
                        result.ISMS_ActiveFlag = 1;
                        result.UpdatedDate = DateTime.Now;
                        _MasterSubjectAllMContext.Update(result);
                        var contactExists = _MasterSubjectAllMContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            objpge.returnval = true;
                        }
                        else
                        {
                            objpge.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => (t.ISMS_SubjectName.Equals(objpge.ISMS_SubjectName) || t.ISMS_SubjectCode.Equals(objpge.ISMS_SubjectCode) || t.ISMS_IVRSSubjectName.Equals(objpge.ISMS_IVRSSubjectName)) && t.MI_Id.Equals(objpge.MI_Id));
                    if (result.Count() > 0)
                    {
                        var result3211 = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => (t.ISMS_SubjectName.Equals(objpge.ISMS_SubjectName)) && t.MI_Id.Equals(objpge.MI_Id) && t.ISMS_Id != objpge.ISMS_Id);
                        if (result3211.Count() > 0)
                        {
                            objpge.returnvaluetype = "subjectname";
                        }
                        else
                        {
                            objpge.returnvaluetype = "subjectcode";
                        }
                        objpge.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        //--removed mapper function and added object for saving subjects----//-adarsh
                        var row_cnt = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => t.MI_Id == objpge.MI_Id).ToList().Count;

                        IVRM_School_Master_SubjectsDMO objpge1 = new IVRM_School_Master_SubjectsDMO();
                        objpge1.ISMS_OrderFlag =  row_cnt + 1;
                        objpge1.ISMS_ActiveFlag = 1;
                        objpge1.ISMS_SubjectName = objpge.ISMS_SubjectName;
                        objpge1.ISMS_SubjectCode = objpge.ISMS_SubjectCode;
                        objpge1.ISMS_IVRSSubjectName = objpge.ISMS_IVRSSubjectName;
                        objpge1.ISMS_SubjectNameNew = objpge.ISMS_SubjectNameNew;
                        objpge1.MI_Id = objpge.MI_Id;
                        objpge1.ISMS_ExamFlag = objpge.ISMS_ExamFlag.Value;
                        objpge1.ISMS_PreadmFlag = objpge.ISMS_PreadmFlag.Value;
                        objpge1.ISMS_Max_Marks = objpge.ISMS_Max_Marks;
                        objpge1.ISMS_Min_Marks = objpge.ISMS_Min_Marks;

                        objpge1.ISMS_TTFlag = objpge.ISMS_TTFlag.Value;
                        objpge1.ISMS_AttendanceFlag = objpge.ISMS_AttendanceFlag.Value;
                        objpge1.ISMS_LanguageFlg = objpge.ISMS_LanguageFlg.Value;
                        objpge1.ISMS_AtExtraFeeFlg = objpge.ISMS_AtExtraFeeFlg.Value;
                        objpge1.ISMS_BatchAppl = objpge.ISMS_BatchAppl.Value;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        _MasterSubjectAllMContext.Add(objpge1);
                        var contactExists = _MasterSubjectAllMContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            objpge.returnval = true;
                        }
                        else
                        {
                            objpge.returnval = false;
                        }
                    }
                }
                List<IVRM_School_Master_SubjectsDMO> master_subjs = new List<IVRM_School_Master_SubjectsDMO>();
                master_subjs = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(c => c.MI_Id == objpge.MI_Id).ToList();
                objpge.subject_m_list = master_subjs.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();


            }
            catch (Exception ee)
            {
                objpge.returnval = false;
                Console.WriteLine(ee.Message);
            }

            return objpge;
        }
        public MasterSubjectAllMDTO validateordernumber(MasterSubjectAllMDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.subjectDTO.Count() > 0)
                {
                    foreach (subject_orderDTO mob in dto.subjectDTO)
                    {
                        if (mob.ISMS_Id > 0)
                        {
                            var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Single(t => t.ISMS_Id.Equals(mob.ISMS_Id));
                            result.ISMS_OrderFlag = mob.ISMS_OrderFlag;

                            result.UpdatedDate = DateTime.Now;
                            _MasterSubjectAllMContext.Update(result);
                            //_MasterSubjectAllMContext.SaveChanges();
                        }
                    }
                    // dto.retrunMsg = "Order Updated sucessfully";
                    var flag = _MasterSubjectAllMContext.SaveChanges();
                    if (flag >= 1)
                    {
                        dto.returnval = true;
                        dto.retrunMsg = "Order Updated Successfully";
                    }
                    else
                    {
                        dto.returnval = false;
                        dto.retrunMsg = "Error occured";
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public MasterSubjectAllMDTO savedata2(MasterSubjectAllMDTO data)
        {
            try
            {
                if (data.ISMS_Id > 0)
                {
                    var dulpicate = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_Id != data.ISMS_Id && t.ISMS_SubjectName == data.ISMS_SubjectName).ToList();

                    if (dulpicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_Id == data.ISMS_Id).SingleOrDefault();

                        result.ISMS_IntroYear = data.ISMS_IntroYear;
                        result.ISMS_FilePath = data.ISMS_FilePath;
                        result.ISMS_FileName = data.ISMS_FileName;
                        result.ISMS_DiscontinuedFlg = data.ISMS_DiscontinuedFlg;
                        result.ISMS_DiscontinuedYear = data.ISMS_DiscontinuedYear;
                        result.ISMS_DiscontinuedReason = data.ISMS_DiscontinuedReason;

                        _MasterSubjectAllMContext.Update(result);

                        var check = _MasterSubjectAllMContext.IVRM_Master_Subjects_Branch_DMO.Where(t => t.ISMS_Id == result.ISMS_Id).ToList();
                        if (check.Count > 0)
                        {
                            foreach (var item in check)
                            {
                                _MasterSubjectAllMContext.Remove(item);
                            }

                            for (int i = 0; i < data.branch_list_data.Length; i++)
                            {
                                IVRM_Master_Subjects_Branch_DMO obj = new IVRM_Master_Subjects_Branch_DMO();

                                obj.ISMS_Id = result.ISMS_Id;
                                obj.AMCO_Id = data.AMCO_Id;
                                obj.AMB_Id = data.branch_list_data[i].AMB_Id;
                                obj.IMSBR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _MasterSubjectAllMContext.Add(obj);
                            }
                        }
                        else
                        {
                            for (int b = 0; b < data.branch_list_data.Length; b++)
                            {
                                IVRM_Master_Subjects_Branch_DMO obj = new IVRM_Master_Subjects_Branch_DMO();

                                obj.ISMS_Id = result.ISMS_Id;
                                obj.AMCO_Id = data.AMCO_Id;
                                obj.AMB_Id = data.branch_list_data[b].AMB_Id;
                                obj.IMSBR_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;

                                _MasterSubjectAllMContext.Add(obj);
                            }
                        }

                        int s = _MasterSubjectAllMContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }
                else
                {

                }

                data.mappinglistdata = (from a in _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO
                                        from b in _MasterSubjectAllMContext.IVRM_Master_Subjects_Branch_DMO
                                        from c in _MasterSubjectAllMContext.MasterCourseDMO
                                        from d in _MasterSubjectAllMContext.ClgMasterBranchDMO
                                        where (a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ISMS_Id == b.ISMS_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id)
                                        select new MasterSubjectAllMDTO
                                        {
                                            AMCO_CourseName = c.AMCO_CourseName,
                                            AMB_BranchName = d.AMB_BranchName,
                                            ISMS_SubjectName = a.ISMS_SubjectName,
                                            ISMS_SubjectCode = a.ISMS_SubjectCode,
                                            ISMS_IntroYear = a.ISMS_IntroYear,
                                            ISMS_DiscontinuedYear = a.ISMS_DiscontinuedYear,
                                        }).Distinct().OrderBy(t => t.ISMS_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

    }
}
