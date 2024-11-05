using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using WebApplication1.Interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

namespace WebApplication1.Services
{
    public class MasterSubjectAllMImpl : MasterSubjectAllMInterface
    {
        private static ConcurrentDictionary<string, MasterSubjectAllMInterface> _login =
         new ConcurrentDictionary<string, MasterSubjectAllMInterface>();

        public MasterSubjectContext _MasterSubjectAllMContext;
        // public TTContext _ttcontext;
        // public enquiryreportContext _enquiryreportContext;
        // public DomainModelMsSqlServerContext _allcontext;
        // public ExamContext _examcontext;
        // public SubjectwisePeriodSettingsContext _swpsc;
        public MasterSubjectAllMImpl(MasterSubjectContext masterSubjectContext)//, TTContext _TTContext, enquiryreportContext _EnquiryreportContext, DomainModelMsSqlServerContext _Allcontext, ExamContext _ExamContext , SubjectwisePeriodSettingsContext _Swpsc
        {
            _MasterSubjectAllMContext = masterSubjectContext;
            //_allcontext = _Allcontext;
            //_ttcontext = _TTContext;
            //_enquiryreportContext = _EnquiryreportContext;
            //_examcontext = _ExamContext;
            //_swpsc = _Swpsc;
        }

        public MasterSubjectAllMDTO DeleteMasterSubDetails(int id)
        {
            MasterSubjectAllMDTO data = new MasterSubjectAllMDTO();
            data.ISMS_Id = id;
            data.already_cnt = false;
            IVRM_Master_SubjectsDMO pge = Mapper.Map<IVRM_Master_SubjectsDMO>(data);
            if (pge.ISMS_Id > 0)
            {
                var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Single(t => t.ISMS_Id == pge.ISMS_Id);
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
                    var Exm_Yrly_Cat_Exams_SubwiseDMO_cnt = _MasterSubjectAllMContext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Yearly_Category_Group_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_Yearly_Category_Group_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Studentwise_SubjectsDMO_cnt = _MasterSubjectAllMContext.StudentMappingDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();

                    var Exm_Student_Marks_Process_SubjectwiseDMO_cnt = _MasterSubjectAllMContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Student_MarksDMO_cnt = _MasterSubjectAllMContext.ExamMarksDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Master_Group_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_Master_Group_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_M_Promotion_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_M_Promotion_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Exm_Login_Privilege_SubjectsDMO_cnt = _MasterSubjectAllMContext.Exm_Login_Privilege_SubjectsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Adm_Student_Attendance_SubjectsDMO_cnt = _MasterSubjectAllMContext.Adm_studentAttendanceSubjects.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Adm_School_Attendance_Subject_MaxPeriodDMO_cnt = _MasterSubjectAllMContext.SubjectwisePeriodSettingsDMO.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Adm_School_Attendance_Subject_BatchDMO_cnt = _MasterSubjectAllMContext.AdmSchoolAttendanceSubjectBatch.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();
                    var Adm_School_Attendance_Login_User_Class_SubjectsDMO_cnt = _MasterSubjectAllMContext.Adm_schAttLoginUserClassSubjects.Where(t => t.ISMS_Id == data.ISMS_Id).ToList();

                    var preadmissionexamwritten = (from a in _MasterSubjectAllMContext.WrittenTestStudentSubjectWiseMarksDMO
                                                   from b in _MasterSubjectAllMContext.WIrttenTestSubjectWiseMarksDMO
                                                   where (a.PASWM_Id == b.PASWM_Id && a.MI_Id == b.MI_Id && b.ISMS_ID == data.ISMS_Id)
                                                   select b).Distinct().ToList();


                    if (TT_Restricting_Period_SubjectDMO_cnt.Count == 0 && TT_Restricting_Period_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Restricting_Day_SubjectDMO_cnt.Count == 0 && TT_Restricting_Day_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Restricting_Day_PeriodDMO_cnt.Count == 0 && TT_Master_Subject_AbbreviationDMO_cnt.Count == 0 && TT_LABLIB_DetailsDMO_cnt.Count == 0 && TT_Fixing_Period_SubjectDMO_cnt.Count == 0 && TT_Fixing_Period_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Fixing_Day_SubjectDMO_cnt.Count == 0 && TT_Fixing_Day_Staff_ClassSectionDMO_cnt.Count == 0 && TT_Fixing_Day_PeriodDMO_cnt.Count == 0 && TT_Final_Period_Distribution_DetailedDMO_cnt.Count == 0 && TT_Final_Generation_DetailedDMO_cnt.Count == 0 && TT_ConsecutiveDMO_cnt.Count == 0 && TT_Bifurcation_DetailsDMO_cnt.Count == 0 && Preadmission_Subjectwise_Written_MarksDMO_cnt.Count == 0 && Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 && Exm_Yearly_Category_Group_SubjectsDMO_cnt.Count == 0 && Exm_Studentwise_SubjectsDMO_cnt.Count == 0 && Exm_Student_Marks_Process_SubjectwiseDMO_cnt.Count == 0 && Exm_Student_MarksDMO_cnt.Count == 0 && Exm_Master_Group_SubjectsDMO_cnt.Count == 0 && Exm_M_Promotion_SubjectsDMO_cnt.Count == 0 && Exm_Login_Privilege_SubjectsDMO_cnt.Count == 0 && Adm_Student_Attendance_SubjectsDMO_cnt.Count == 0 && Adm_School_Attendance_Subject_MaxPeriodDMO_cnt.Count == 0 && Adm_School_Attendance_Subject_BatchDMO_cnt.Count == 0 && Adm_School_Attendance_Login_User_Class_SubjectsDMO_cnt.Count == 0 && preadmissionexamwritten.Count == 0)
                    {
                        result.ISMS_ActiveFlag = 0;
                        result.UpdatedDate = DateTime.Now;
                        _MasterSubjectAllMContext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                }
                else
                {
                    result.ISMS_ActiveFlag = 1;
                    result.UpdatedDate = DateTime.Now;
                    _MasterSubjectAllMContext.Update(result);
                }
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
                List<IVRM_Master_SubjectsDMO> mastersub = new List<IVRM_Master_SubjectsDMO>();
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

                List<IVRM_Master_SubjectsDMO> mastersub = new List<IVRM_Master_SubjectsDMO>();
                mastersub = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(c => c.MI_Id == id).ToList();

                mast.subject_m_list = mastersub.Distinct().OrderBy(t => t.CreatedDate).ToArray();
                mast.subject_m_listOrder = mastersub.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();
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
        public MasterSubjectAllMDTO SaveMasterSubDetails(MasterSubjectAllMDTO _category)
        {
            IVRM_Master_SubjectsDMO objpge = Mapper.Map<IVRM_Master_SubjectsDMO>(_category);
            try
            {
                if (objpge.ISMS_Id > 0)
                {
                    var result321 = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => (t.ISMS_SubjectName.Equals(objpge.ISMS_SubjectName) || t.ISMS_SubjectCode.Equals(objpge.ISMS_SubjectCode) || t.ISMS_IVRSSubjectName.Equals(objpge.ISMS_IVRSSubjectName)) && t.MI_Id.Equals(objpge.MI_Id) && t.ISMS_Id != objpge.ISMS_Id);
                    if (result321.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Single(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                        result.ISMS_SubjectName = objpge.ISMS_SubjectName;
                        result.ISMS_IVRSSubjectName = objpge.ISMS_IVRSSubjectName;
                        result.ISMS_SubjectNameNew = objpge.ISMS_SubjectNameNew;
                        result.ISMS_SubjectCode = objpge.ISMS_SubjectCode;

                        result.ISMS_Max_Marks = objpge.ISMS_Max_Marks;
                        result.ISMS_Min_Marks = objpge.ISMS_Min_Marks;
                        result.ISMS_ExamFlag = objpge.ISMS_ExamFlag;
                        result.ISMS_PreadmFlag = objpge.ISMS_PreadmFlag;
                        result.ISMS_SubjectFlag = objpge.ISMS_SubjectFlag;
                        result.ISMS_TTFlag = objpge.ISMS_TTFlag;
                        result.ISMS_AttendanceFlag = objpge.ISMS_AttendanceFlag;
                        result.ISMS_LanguageFlg = objpge.ISMS_LanguageFlg;
                        result.ISMS_AtExtraFeeFlg = objpge.ISMS_AtExtraFeeFlg;
                        result.ISMS_BatchAppl = objpge.ISMS_BatchAppl;
                        result.ISMS_ActiveFlag = 1;
                        result.UpdatedDate = DateTime.Now;
                        _MasterSubjectAllMContext.Update(result);
                        var contactExists = _MasterSubjectAllMContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;

                            if (objpge.ISMS_SubjectFlag == 1 && objpge.ISMS_PreadmFlag == 1)
                            {
                                var checksubjectexists = _MasterSubjectAllMContext.WIrttenTestSubjectWiseMarksDMO.Where(a => a.MI_Id == objpge.MI_Id && a.ISMS_ID == objpge.ISMS_Id).ToList();

                                if (checksubjectexists.Count() == 0)
                                {
                                    _category.returnval = true;
                                    try
                                    {
                                        WIrttenTestSubjectWiseMarksDMO dnow = new WIrttenTestSubjectWiseMarksDMO();
                                        dnow.MI_Id = objpge.MI_Id;
                                        dnow.ASMAY_Id = 0;
                                        dnow.ISMS_ID = objpge.ISMS_Id;
                                        dnow.PASWM_Date = DateTime.UtcNow;
                                        dnow.CreatedDate = DateTime.UtcNow;
                                        dnow.UpdatedDate = DateTime.UtcNow;
                                        _MasterSubjectAllMContext.Add(dnow);

                                        var i = _MasterSubjectAllMContext.SaveChanges();
                                        if (i > 0)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _category.returnval = true;
                                        Console.WriteLine(ex.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => (t.ISMS_SubjectName.Equals(objpge.ISMS_SubjectName)
                    || t.ISMS_SubjectCode.Equals(objpge.ISMS_SubjectCode) || t.ISMS_IVRSSubjectName.Equals(objpge.ISMS_IVRSSubjectName))
                    && t.MI_Id.Equals(objpge.MI_Id));
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(t => t.MI_Id == _category.MI_Id).ToList().Count;
                        objpge.ISMS_OrderFlag = row_cnt + 1;
                        objpge.ISMS_ActiveFlag = 1;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _MasterSubjectAllMContext.Add(objpge);

                        if (_category.ISMS_SubjectFlag == 1 && _category.ISMS_PreadmFlag == 1)
                        {
                            WIrttenTestSubjectWiseMarksDMO dmonw = new WIrttenTestSubjectWiseMarksDMO();
                            dmonw.ISMS_ID = objpge.ISMS_Id;
                            dmonw.MI_Id = _category.MI_Id;
                            dmonw.ASMAY_Id = 0;
                            dmonw.PASWM_Date = DateTime.UtcNow;
                            dmonw.CreatedDate = DateTime.UtcNow;
                            dmonw.UpdatedDate = DateTime.UtcNow;
                            _MasterSubjectAllMContext.Add(dmonw);
                        }

                        var contactExists = _MasterSubjectAllMContext.SaveChanges();
                        if (contactExists >= 1)
                        {

                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                List<IVRM_Master_SubjectsDMO> master_subjs = new List<IVRM_Master_SubjectsDMO>();
                master_subjs = _MasterSubjectAllMContext.IVRM_Master_SubjectsDMO.Where(c => c.MI_Id == _category.MI_Id).ToList();
                _category.subject_m_list = master_subjs.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ee)
            {
                _category.returnval = false;
                Console.WriteLine(ee.Message);
            }

            return _category;
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
                            // result.UpdatedDate = DateTime.Now;
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
    }
}
