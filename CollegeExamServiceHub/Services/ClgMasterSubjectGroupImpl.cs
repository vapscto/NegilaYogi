using AutoMapper;
using CollegeExamServiceHub.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgMasterSubjectGroupImpl : ClgMasterSubjectGroupInterface
    {
        private static ConcurrentDictionary<string, MasterSubjectGroupDTO> _login =
      new ConcurrentDictionary<string, MasterSubjectGroupDTO>();

        private readonly ClgExamContext _examcontext;
        public ClgMasterSubjectGroupImpl(ClgExamContext obj)
        {
            _examcontext = obj;
        }
        public MasterSubjectGroupDTO savedetail(MasterSubjectGroupDTO _category)
        {
            Exm_Master_GroupDMO objpge = Mapper.Map<Exm_Master_GroupDMO>(_category);

            try
            {
                if (objpge.EMG_Id > 0)
                {
                    var resultCount = _examcontext.Exm_Master_GroupDMO.Where(t => t.EMG_GroupName == objpge.EMG_GroupName && t.MI_Id == objpge.MI_Id && t.EMG_Id != objpge.EMG_Id).Count();

                    if (resultCount == 0)
                    {
                        var result = _examcontext.Exm_Master_GroupDMO.Single(t => t.EMG_Id == objpge.EMG_Id && t.MI_Id == objpge.MI_Id);

                        result.EMG_GroupName = objpge.EMG_GroupName;
                        result.EMG_TotSubjects = objpge.EMG_TotSubjects;
                        result.EMG_MaxAplSubjects = objpge.EMG_MaxAplSubjects;
                        result.EMG_MinAplSubjects = objpge.EMG_MinAplSubjects;
                        result.EMG_BestOff = objpge.EMG_BestOff;
                        result.EMG_ElectiveFlg = objpge.EMG_ElectiveFlg;
                        result.UpdatedDate = DateTime.Now;
                        result.EMG_ActiveFlag = true;

                        _examcontext.Update(result);

                        MasterSubjectGroupDTO req = delete_group_subjects(objpge.EMG_Id);

                        if (req.returnval == true)
                        {
                            foreach (var act1 in _category.subj_list)
                            {
                                Exm_Col_Master_Group_SubjectsDMO objpge1 = Mapper.Map<Exm_Col_Master_Group_SubjectsDMO>(_category);

                                objpge1.ISMS_Id = act1.ISMS_Id;
                                objpge1.ECMGS_ActiveFlag = true;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                _examcontext.Add(objpge1);

                            }
                        }

                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                        return _category;
                    }
                }
                else
                {
                    var result = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == objpge.MI_Id && t.EMG_GroupName == objpge.EMG_GroupName).Count();
                    if (result > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.EMG_ActiveFlag = true;
                        _examcontext.Add(objpge);
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                            _category.EMG_Id = objpge.EMG_Id;
                            foreach (var act1 in _category.subj_list)
                            {
                                Exm_Col_Master_Group_SubjectsDMO objpge1 = Mapper.Map<Exm_Col_Master_Group_SubjectsDMO>(_category);
                                objpge1.ISMS_Id = act1.ISMS_Id;
                                objpge1.ECMGS_ActiveFlag = true;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                _examcontext.Add(objpge1);
                                var contactExists1 = _examcontext.SaveChanges();
                            }
                            _category.EMG_Id = 0;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                _category.Group_list = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == _category.MI_Id).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return _category;
        }


        public MasterSubjectGroupDTO getdetails(int id)
        {
            MasterSubjectGroupDTO TTMC = new MasterSubjectGroupDTO();
            try
            {
                TTMC.Group_list = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == id).Distinct().ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1).ToList();
                TTMC.subjectlist = subjects.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }


        public MasterSubjectGroupDTO geteventdetails(MasterSubjectGroupDTO _category)
        {
           
            return _category;

        }


        public Exm_Col_Master_Group_SubjectsDTO getalldetailsviewrecords(int id)
        {
            Exm_Col_Master_Group_SubjectsDTO page = new Exm_Col_Master_Group_SubjectsDTO();
            try
            {
                page.view_group_subjects = (from m in _examcontext.Exm_Master_GroupDMO
                                            from n in _examcontext.Exm_Col_Master_Group_SubjectsDMO
                                            from l in _examcontext.IVRM_School_Master_SubjectsDMO
                                            where (m.MI_Id == l.MI_Id && m.EMG_Id == id && m.EMG_Id == n.EMG_Id && n.ISMS_Id == l.ISMS_Id)
                                            select new Exm_Col_Master_Group_SubjectsDTO
                                            {
                                                ECMGS_Id = n.ECMGS_Id,
                                                EMG_Id = m.EMG_Id,
                                                EMG_GroupName = m.EMG_GroupName,
                                                ISMS_Id = n.ISMS_Id,
                                                ISMS_SubjectName = l.ISMS_SubjectName,
                                                ISMS_SubjectCode = l.ISMS_SubjectCode,
                                                ECMGS_ActiveFlag = n.ECMGS_ActiveFlag,

                                            }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public MasterSubjectGroupDTO getpageedit(int id)
        {
            MasterSubjectGroupDTO page = new MasterSubjectGroupDTO();
            try
            {
                List<Exm_Master_GroupDMO> group_m = new List<Exm_Master_GroupDMO>();
                group_m = _examcontext.Exm_Master_GroupDMO.Where(e => e.EMG_Id == id).ToList();
                page.edit_m_group = group_m.ToArray();
                List<Exm_Col_Master_Group_SubjectsDMO> group_subjects_m = new List<Exm_Col_Master_Group_SubjectsDMO>();
                group_subjects_m = _examcontext.Exm_Col_Master_Group_SubjectsDMO.Where(e => e.EMG_Id == id).ToList();
                page.edit_m_group_subjects = group_subjects_m.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterSubjectGroupDTO deleterec(int id)
        {
            MasterSubjectGroupDTO page = new MasterSubjectGroupDTO();
           
            return page;
        }

        //active deactive 
        public MasterSubjectGroupDTO deactivate(MasterSubjectGroupDTO data)
        {
            data.already_cnt = false;
            Exm_Master_GroupDMO pge = Mapper.Map<Exm_Master_GroupDMO>(data);
            if (pge.EMG_Id > 0)
            {
                var result = _examcontext.Exm_Master_GroupDMO.Single(t => t.EMG_Id == pge.EMG_Id);
                if (result.EMG_ActiveFlag == true)
                {
                   // var Exm_Yearly_Category_GroupDMO_cnt = _examcontext.Exm_Col_Master_Group_SubjectsDMO.Where(t => t.EMG_Id == data.EMG_Id).ToList();
                    var Exm_Studentwise_Subjects_cnt = _examcontext.Exm_Col_Studentwise_SubjectsDMO.Where(t => t.EMG_Id == data.EMG_Id).ToList();
                    if (Exm_Studentwise_Subjects_cnt.Count == 0)
                    {
                        result.EMG_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                }
                else
                {
                    result.EMG_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                var flag = _examcontext.SaveChanges();
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
        public MasterSubjectGroupDTO delete_group_subjects(int id)
        {
            MasterSubjectGroupDTO pagert = new MasterSubjectGroupDTO();
            try
            {
                List<Exm_Col_Master_Group_SubjectsDMO> lorg = new List<Exm_Col_Master_Group_SubjectsDMO>();
                lorg = _examcontext.Exm_Col_Master_Group_SubjectsDMO.Where(t => t.EMG_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _examcontext.Remove(lorg.ElementAt(i));
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }
                else if (lorg.Count == 0)
                {
                    pagert.returnval = true;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

    }
}
