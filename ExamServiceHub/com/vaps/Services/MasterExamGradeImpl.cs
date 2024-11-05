
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.COE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class MasterExamGradeImpl : Interfaces.MasterExamGradeInterface
    {
        private static ConcurrentDictionary<string, MasterExamGradeDTO> _login = new ConcurrentDictionary<string, MasterExamGradeDTO>();

        public ExamContext _examcontext;
        ILogger<MasterExamGradeImpl> _acdimpl;
        public MasterExamGradeImpl(ExamContext ttcategory)
        {
            _examcontext = ttcategory;
        }
        public MasterExamGradeDTO savedetail(MasterExamGradeDTO _category)
        {
            Exm_Master_GradeDMO objpge = Mapper.Map<Exm_Master_GradeDMO>(_category);

            try
            {
                if (objpge.EMGR_Id > 0)
                {
                    var resultCount = _examcontext.Exm_Master_GradeDMO.Where(t => t.EMGR_GradeName == objpge.EMGR_GradeName && t.MI_Id == objpge.MI_Id && t.EMGR_Id != objpge.EMGR_Id).Count();

                    if (resultCount == 0)
                    {
                        MasterExamGradeDTO req = delete_grade_details(objpge.EMGR_Id);
                        var result = _examcontext.Exm_Master_GradeDMO.Single(t => t.EMGR_Id == objpge.EMGR_Id && t.MI_Id == objpge.MI_Id);

                        result.EMGR_GradeName = objpge.EMGR_GradeName;
                        result.EMGR_MarksPerFlag = objpge.EMGR_MarksPerFlag;
                        result.UpdatedDate = DateTime.Now;
                        result.EMGR_ActiveFlag = true;
                        _examcontext.Update(result);
                        
                        if (req.returnval == true)
                        {
                            foreach (var act1 in _category.sub_list)
                            {
                                Exm_Master_Grade_DetailsDMO objpge1 = new Exm_Master_Grade_DetailsDMO();
                                // Exm_Master_Grade_DetailsDMO objpge1 = Mapper.Map<Exm_Master_Grade_DetailsDMO>(act1);
                                objpge1.EMGR_Id = _category.EMGR_Id;
                                objpge1.EMGD_Name = act1.EMGD_Name;
                                objpge1.EMGD_From= act1.EMGD_From;
                                objpge1.EMGD_To = act1.EMGD_To;
                                objpge1.EMGD_Remarks = act1.EMGD_Remarks;
                                objpge1.EMGD_GradePoints = act1.EMGD_GradePoints;
                                objpge1.EMGD_ActiveFlag = true;
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
                    var result = _examcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == objpge.MI_Id && t.EMGR_GradeName == objpge.EMGR_GradeName).Count();

                    if (result > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else if (result == 0)
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.EMGR_ActiveFlag = true;
                        _examcontext.Add(objpge);
                        _category.EMGR_Id = objpge.EMGR_Id;
                        foreach (var act1 in _category.sub_list)
                        {
                            Exm_Master_Grade_DetailsDMO objpge1 = Mapper.Map<Exm_Master_Grade_DetailsDMO>(act1);

                            objpge1.EMGR_Id = _category.EMGR_Id;
                            objpge1.EMGD_ActiveFlag = true;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            _examcontext.Add(objpge1);
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
                }
                _category.Grade_list = _examcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == _category.MI_Id).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return _category;
        }
        public MasterExamGradeDTO getdetails(int id)
        {
            MasterExamGradeDTO TTMC = new MasterExamGradeDTO();
            try
            {
                TTMC.Grade_list = _examcontext.Exm_Master_GradeDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;
        }
        public MasterExamGradeDTO getalldetailsviewrecords(int id)
        {
            MasterExamGradeDTO page = new MasterExamGradeDTO();
            try
            {
                List<Exm_Master_Grade_DetailsDMO> grade_details = new List<Exm_Master_Grade_DetailsDMO>();
                grade_details = _examcontext.Exm_Master_Grade_DetailsDMO.Where(e => e.EMGR_Id == id).ToList();
                page.view_grade_details = grade_details.ToArray();
                page.EMGR_GradeName = _examcontext.Exm_Master_GradeDMO.Where(t => t.EMGR_Id == id).Select(x => x.EMGR_GradeName).First().ToString();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterExamGradeDTO getpageedit(int id)
        {
            MasterExamGradeDTO page = new MasterExamGradeDTO();
            try
            {
                List<Exm_Master_GradeDMO> grade_m = new List<Exm_Master_GradeDMO>();
                grade_m = _examcontext.Exm_Master_GradeDMO.Where(e => e.EMGR_Id == id).ToList();
                page.edit_m_grade = grade_m.ToArray();

                List<Exm_Master_Grade_DetailsDMO> grade_details_m = new List<Exm_Master_Grade_DetailsDMO>();
                grade_details_m = _examcontext.Exm_Master_Grade_DetailsDMO.Where(e => e.EMGR_Id == id).ToList();
                page.edit_m_grade_details = grade_details_m.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public MasterExamGradeDTO deleterec(int id)
        {
            MasterExamGradeDTO page = new MasterExamGradeDTO();           
            return page;
        }
        public MasterExamGradeDTO deactivate(MasterExamGradeDTO data)
        {
            data.already_cnt = false;
            Exm_Master_GradeDMO pge = Mapper.Map<Exm_Master_GradeDMO>(data);
            if (pge.EMGR_Id > 0)
            {
                var result = _examcontext.Exm_Master_GradeDMO.Single(t => t.EMGR_Id == pge.EMGR_Id);
                if (result.EMGR_ActiveFlag == true)
                {
                    var Exm_Yearly_Category_ExamsDMO_cnt = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToList();

                    var Exm_Yrly_Cat_Exams_SubwiseDMO_cnt = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToList();

                    var Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO_cnt = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToList();

                    var Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO_cnt = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToList();

                    var Exm_M_PromotionDMO_cnt = _examcontext.Exm_M_PromotionDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToList();

                    var Exm_M_Promotion_SubjectsDMO_cnt = _examcontext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMGR_Id == data.EMGR_Id).ToList();

                    if (Exm_Yearly_Category_ExamsDMO_cnt.Count == 0 && Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 
                        && Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO_cnt.Count == 0 && Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO_cnt.Count == 0 
                        && Exm_M_PromotionDMO_cnt.Count == 0 && Exm_M_Promotion_SubjectsDMO_cnt.Count == 0)
                    {
                        result.EMGR_ActiveFlag = false;
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
                    result.EMGR_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }

                var flag = _examcontext.SaveChanges();
                if (flag >= 1)
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
        public MasterExamGradeDTO delete_grade_details(int id)
        {
            MasterExamGradeDTO pagert = new MasterExamGradeDTO();
            try
            {
                List<Exm_Master_Grade_DetailsDMO> lorg = new List<Exm_Master_Grade_DetailsDMO>();
                lorg = _examcontext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == id).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _examcontext.Remove(lorg.ElementAt(i));
                        
                    }
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        pagert.returnval = true;
                    }
                    else
                    {
                        pagert.returnval = false;
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