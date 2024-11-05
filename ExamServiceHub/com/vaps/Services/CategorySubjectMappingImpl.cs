
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.TT;
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
    public class CategorySubjectMappingImpl : Interfaces.CategorySubjectMappingInterface
    {
        private static ConcurrentDictionary<string, CategorySubjectMappingDTO> _login = new ConcurrentDictionary<string, CategorySubjectMappingDTO>();

        public ExamContext _examcontext;
        ILogger<CategorySubjectMappingImpl> _acdimpl;
        public CategorySubjectMappingImpl(ExamContext ttcategory, ILogger<CategorySubjectMappingImpl> _acdimp)
        {
            _examcontext = ttcategory;
            _acdimpl = _acdimp;
        }
        public CategorySubjectMappingDTO savedetail(CategorySubjectMappingDTO _category)
        {
            Exm_Yearly_CategoryDMO objpge = Mapper.Map<Exm_Yearly_CategoryDMO>(_category);
            Exm_Yearly_Category_GroupDMO objgrp = Mapper.Map<Exm_Yearly_Category_GroupDMO>(_category);
            try
            {
                if (objgrp.EYCG_Id > 0)
                {
                    _category.returnMsg = "Update";
                    var result_grp = _examcontext.Exm_Yearly_Category_GroupDMO.Single(c => c.EYCG_Id == objgrp.EYCG_Id);

                    bool EMG_ElectiveFlg_delete = _examcontext.Exm_Master_GroupDMO.Where(g => g.MI_Id == _category.MI_Id && g.EMG_Id == result_grp.EMG_Id).Select(g => g.EMG_ElectiveFlg).First();

                    // update 
                    var j1 = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.EMCA_Id == objpge.EMCA_Id 
                    && t.EYC_Id == result_grp.EYC_Id);

                    j1.EYC_BasedOnPaperTypeFlg = _category.EYC_BasedOnPaperTypeFlg;
                    _examcontext.Update(j1);

                    if (EMG_ElectiveFlg_delete == false)
                    {
                        var EMCA_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(c => c.MI_Id == _category.MI_Id && c.EYC_Id == result_grp.EYC_Id);

                        var class_list_delete = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == EMCA_Id.EMCA_Id && c.MI_Id == _category.MI_Id).Select(c => c.ASMCL_Id).Distinct().ToList();

                        List<long> classes = new List<long>();
                        foreach (var cls in class_list_delete)
                        {
                            classes.Add(cls);
                        }

                        var section_list_delete = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == EMCA_Id.EMCA_Id && c.MI_Id == _category.MI_Id && classes.Contains(c.ASMCL_Id)).Select(c => c.ASMS_Id).Distinct().ToList();

                        List<long> sectionss = new List<long>();
                        foreach (var secss in section_list_delete)
                        {
                            sectionss.Add(secss);
                        }

                        var temp_stu_array_delete = (from a in _examcontext.Adm_M_Student
                                                     from b in _examcontext.School_Adm_Y_StudentDMO
                                                     where (a.MI_Id == _category.MI_Id && a.AMST_Id == b.AMST_Id && b.ASMAY_Id == _category.ASMAY_Id
                                                     && classes.Contains(b.ASMCL_Id) && sectionss.Contains(b.ASMS_Id))
                                                     select new CategorySubjectMappingDTO
                                                     {
                                                         AMST_Id = b.AMST_Id,
                                                         ASMCL_Id = b.ASMCL_Id,
                                                         ASMS_Id = b.ASMS_Id
                                                     }).Distinct().ToArray();

                        List<long> delete_classes = new List<long>();
                        List<long> delete_sections = new List<long>();
                        List<long> delete_students = new List<long>();

                        foreach (var x in temp_stu_array_delete)
                        {
                            delete_classes.Add(x.ASMCL_Id);
                            delete_sections.Add(x.ASMS_Id);
                            delete_students.Add(x.AMST_Id);
                        }

                        var delete_list_stu_subjs = _examcontext.StudentMappingDMO.Where(s => s.MI_Id == _category.MI_Id && s.ESTSU_ElecetiveFlag == false 
                        && delete_classes.Contains(s.ASMCL_Id) && delete_sections.Contains(s.ASMS_Id) && delete_students.Contains(s.AMST_Id) 
                        && s.EMG_Id == result_grp.EMG_Id).ToList();

                        if (delete_list_stu_subjs.Any())
                        {
                            for (int x = 0; delete_list_stu_subjs.Count > x; x++)
                            {
                                _examcontext.Remove(delete_list_stu_subjs.ElementAt(x));
                            }
                            var contactExist_delete = _examcontext.SaveChanges();
                            if (contactExist_delete >= 1)
                            {
                                _category.returnval = true;
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                        else if (delete_list_stu_subjs.Count == 0)
                        {
                            _category.returnval = true;
                        }

                    }
                    else if (EMG_ElectiveFlg_delete == true)
                    {
                        _category.returnval = true;
                    }

                    if (_category.returnval == true)
                    {
                        var result = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.EMCA_Id == objpge.EMCA_Id).ToList();

                        if (result.Count() > 0)
                        {
                            var result1 = _examcontext.Exm_Yearly_Category_GroupDMO.Where(g => g.EMG_Id == _category.EMG_Id && g.EYC_Id == result[0].EYC_Id && g.EYCG_Id != objgrp.EYCG_Id).Count();
                            if (result1 == 0)
                            {
                                _category.EYC_Id = result[0].EYC_Id;
                                result_grp.EYC_Id = _category.EYC_Id;
                                result_grp.EMG_Id = _category.EMG_Id;

                                result_grp.EYCG_ActiveFlg = true;
                                result_grp.UpdatedDate = DateTime.Now;
                                _examcontext.Update(result_grp);
                                CategorySubjectMappingDTO req = delete_group_subjects(objgrp.EYCG_Id);
                                if (req.returnval == true)
                                {
                                    foreach (var act1 in _category.subj_list)
                                    {
                                        Exm_Yearly_Category_Group_SubjectsDMO objpge2 = Mapper.Map<Exm_Yearly_Category_Group_SubjectsDMO>(_category);
                                        objpge2.ISMS_Id = act1.ISMS_Id;
                                        objpge2.EYCGS_ActiveFlg = true;
                                        objpge2.CreatedDate = DateTime.Now;
                                        objpge2.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(objpge2);
                                    }
                                }
                                bool EMG_ElectiveFlg = _examcontext.Exm_Master_GroupDMO.Where(g => g.MI_Id == _category.MI_Id 
                                && g.EMG_Id == _category.EMG_Id).Select(g => g.EMG_ElectiveFlg).First();
                                if (EMG_ElectiveFlg == false)
                                {
                                    var class_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == _category.EMCA_Id && c.MI_Id == _category.MI_Id).Select(c => c.ASMCL_Id).Distinct().ToList();
                                    List<long> classes = new List<long>();
                                    foreach (var cls in class_list)
                                    {
                                        classes.Add(cls);
                                    }

                                    var section_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id 
                                    && c.EMCA_Id == _category.EMCA_Id && c.MI_Id == _category.MI_Id 
                                    && classes.Contains(c.ASMCL_Id)).Select(c => c.ASMS_Id).Distinct().ToList();

                                    List<long> sectionss = new List<long>();
                                    foreach (var secs in section_list)
                                    {
                                        sectionss.Add(secs);
                                    }

                                    var temp_stu_array = (from a in _examcontext.Adm_M_Student
                                                          from b in _examcontext.School_Adm_Y_StudentDMO
                                                          where (a.MI_Id == _category.MI_Id && a.AMST_Id == b.AMST_Id && b.ASMAY_Id == _category.ASMAY_Id
                                                          && classes.Contains(b.ASMCL_Id) && sectionss.Contains(b.ASMS_Id) && a.AMST_SOL != "WD"
                                                          //&& a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1
                                                          )
                                                          select new CategorySubjectMappingDTO
                                                          {
                                                              AMST_Id = b.AMST_Id,
                                                              ASMCL_Id = b.ASMCL_Id,
                                                              ASMS_Id = b.ASMS_Id
                                                          }).Distinct().ToArray();
                                    foreach (var act2 in temp_stu_array)
                                    {
                                        foreach (var act3 in _category.subj_list)
                                        {
                                            StudentMappingDMO objpge4 = Mapper.Map<StudentMappingDMO>(_category);
                                            var stu_result_cnt = _examcontext.StudentMappingDMO.Where(s => s.MI_Id == _category.MI_Id 
                                            && s.ASMAY_Id == _category.ASMAY_Id && s.ASMCL_Id == act2.ASMCL_Id && s.ASMS_Id == act2.ASMS_Id 
                                            && s.AMST_Id == act2.AMST_Id && s.ISMS_Id == act3.ISMS_Id).Count();
                                            if (stu_result_cnt == 0)
                                            {
                                                objpge4.ASMCL_Id = act2.ASMCL_Id;
                                                objpge4.ASMS_Id = act2.ASMS_Id;
                                                objpge4.AMST_Id = act2.AMST_Id;
                                                objpge4.ISMS_Id = act3.ISMS_Id;
                                                objpge4.ESTSU_ActiveFlg = true;
                                                objpge4.ESTSU_ElecetiveFlag = false;
                                                objpge4.CreatedDate = DateTime.Now;
                                                objpge4.UpdatedDate = DateTime.Now;
                                                objpge4.ESTSU_CreatedBy = _category.UserId;
                                                objpge4.ESTSU_UpdatedBy = _category.UserId;
                                                _examcontext.Add(objpge4);
                                            }
                                        }
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
                                _category.returnval = false;
                            }
                        }
                        else if (result.Count() == 0)
                        {
                            objpge.EYC_Id = 0;
                            objpge.EYC_BasedOnPaperTypeFlg = _category.EYC_BasedOnPaperTypeFlg;
                            objpge.EYC_ActiveFlg = true;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _examcontext.Add(objpge);

                            _category.EYC_Id = objpge.EYC_Id;
                            result_grp.EYC_Id = _category.EYC_Id;
                            result_grp.EMG_Id = objgrp.EMG_Id;
                            result_grp.EYCG_ActiveFlg = true;
                            result_grp.UpdatedDate = DateTime.Now;
                            _examcontext.Update(result_grp);

                            CategorySubjectMappingDTO req = delete_group_subjects(objgrp.EYCG_Id);
                            if (req.returnval == true)
                            {
                                foreach (var act1 in _category.subj_list)
                                {
                                    Exm_Yearly_Category_Group_SubjectsDMO objpge2 = Mapper.Map<Exm_Yearly_Category_Group_SubjectsDMO>(_category);
                                    objpge2.ISMS_Id = act1.ISMS_Id;
                                    objpge2.EYCGS_ActiveFlg = true;
                                    objpge2.CreatedDate = DateTime.Now;
                                    objpge2.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(objpge2);

                                }
                            }
                            bool EMG_ElectiveFlg = _examcontext.Exm_Master_GroupDMO.Where(g => g.MI_Id == _category.MI_Id && g.EMG_Id == objgrp.EMG_Id).Select(g => g.EMG_ElectiveFlg).First();
                            if (EMG_ElectiveFlg == false)
                            {
                                var class_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == _category.EMCA_Id && c.MI_Id == _category.MI_Id).Select(c => c.ASMCL_Id).Distinct().ToList();
                                List<long> classes = new List<long>();
                                foreach (var cls in class_list)
                                {
                                    classes.Add(cls);
                                }

                                var section_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id
                                && c.EMCA_Id == _category.EMCA_Id && c.MI_Id == _category.MI_Id && classes.Contains(c.ASMCL_Id)).Select(c => c.ASMS_Id).Distinct().ToList();

                                List<long> sectionss = new List<long>();
                                foreach (var secs in section_list)
                                {
                                    sectionss.Add(secs);
                                }

                                var temp_stu_array = (from a in _examcontext.Adm_M_Student
                                                      from b in _examcontext.School_Adm_Y_StudentDMO
                                                      where (a.MI_Id == _category.MI_Id && a.AMST_Id == b.AMST_Id && b.ASMAY_Id == _category.ASMAY_Id
                                                      && classes.Contains(b.ASMCL_Id) && sectionss.Contains(b.ASMS_Id) && a.AMST_SOL!= "WD"
                                                      //&& a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1
                                                      )
                                                      select new CategorySubjectMappingDTO
                                                      {
                                                          AMST_Id = b.AMST_Id,
                                                          ASMCL_Id = b.ASMCL_Id,
                                                          ASMS_Id = b.ASMS_Id
                                                      }).Distinct().ToArray();
                                foreach (var act2 in temp_stu_array)
                                {
                                    foreach (var act3 in _category.subj_list)
                                    {
                                        StudentMappingDMO objpge4 = Mapper.Map<StudentMappingDMO>(_category);
                                        var stu_result_cnt = _examcontext.StudentMappingDMO.Where(s => s.MI_Id == _category.MI_Id && s.ASMAY_Id == _category.ASMAY_Id && s.ASMCL_Id == act2.ASMCL_Id && s.ASMS_Id == act2.ASMS_Id && s.AMST_Id == act2.AMST_Id && s.ISMS_Id == act3.ISMS_Id).Count();
                                        if (stu_result_cnt == 0)
                                        {
                                            objpge4.ASMCL_Id = act2.ASMCL_Id;
                                            objpge4.ASMS_Id = act2.ASMS_Id;
                                            objpge4.AMST_Id = act2.AMST_Id;
                                            objpge4.ISMS_Id = act3.ISMS_Id;
                                            objpge4.ESTSU_ActiveFlg = true;
                                            objpge4.ESTSU_ElecetiveFlag = false;
                                            objpge4.CreatedDate = DateTime.Now;
                                            objpge4.UpdatedDate = DateTime.Now;
                                            objpge4.ESTSU_CreatedBy = _category.UserId;
                                            objpge4.ESTSU_UpdatedBy = _category.UserId;
                                            _examcontext.Add(objpge4);
                                        }
                                    }
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
                    }
                }

                else
                {
                    _category.returnMsg = "Add";
                    var result = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id
                    && t.EMCA_Id == objpge.EMCA_Id).ToList();

                    if (result.Count() > 0)
                    {
                        // update
                        var j = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id
                        && t.EMCA_Id == objpge.EMCA_Id && t.EYC_Id == result[0].EYC_Id);
                        j.EYC_BasedOnPaperTypeFlg = _category.EYC_BasedOnPaperTypeFlg;
                        _examcontext.Update(j);

                        var result1 = _examcontext.Exm_Yearly_Category_GroupDMO.Where(g => g.EMG_Id == _category.EMG_Id && g.EYC_Id == result[0].EYC_Id).Count();
                        if (result1 == 0)
                        {
                            _category.EYC_Id = result[0].EYC_Id;
                            Exm_Yearly_Category_GroupDMO objpge1 = Mapper.Map<Exm_Yearly_Category_GroupDMO>(_category);
                            objpge1.EYCG_ActiveFlg = true;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            _examcontext.Add(objpge1);
                            _category.EYCG_Id = objpge1.EYCG_Id;

                            foreach (var act1 in _category.subj_list)
                            {
                                Exm_Yearly_Category_Group_SubjectsDMO objpge2 = Mapper.Map<Exm_Yearly_Category_Group_SubjectsDMO>(_category);
                                objpge2.ISMS_Id = act1.ISMS_Id;
                                objpge2.EYCGS_ActiveFlg = true;
                                objpge2.CreatedDate = DateTime.Now;
                                objpge2.UpdatedDate = DateTime.Now;
                                _examcontext.Add(objpge2);

                            }

                            bool EMG_ElectiveFlg = _examcontext.Exm_Master_GroupDMO.Where(g => g.MI_Id == _category.MI_Id && g.EMG_Id == _category.EMG_Id).Select(g => g.EMG_ElectiveFlg).First();

                            if (EMG_ElectiveFlg == false)
                            {
                                var class_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == _category.EMCA_Id && c.MI_Id == _category.MI_Id).Select(c => c.ASMCL_Id).Distinct().ToList();
                                List<long> classes = new List<long>();
                                foreach (var cls in class_list)
                                {
                                    classes.Add(cls);
                                }

                                var section_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id
                                && c.EMCA_Id == _category.EMCA_Id && c.MI_Id == _category.MI_Id && classes.Contains(c.ASMCL_Id)).Select(c => c.ASMS_Id).Distinct().ToList();

                                List<long> sectionss = new List<long>();
                                foreach (var secs in section_list)
                                {
                                    sectionss.Add(secs);
                                }


                                var temp_stu_array = (from a in _examcontext.Adm_M_Student
                                                      from b in _examcontext.School_Adm_Y_StudentDMO
                                                      where (a.MI_Id == _category.MI_Id && a.AMST_Id == b.AMST_Id
                                                      && b.ASMAY_Id == _category.ASMAY_Id
                                                      && classes.Contains(b.ASMCL_Id) && sectionss.Contains(b.ASMS_Id) && a.AMST_SOL != "WD"
                                                      //&& a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1
                                                      )
                                                      select new CategorySubjectMappingDTO
                                                      {
                                                          AMST_Id = b.AMST_Id,
                                                          ASMCL_Id = b.ASMCL_Id,
                                                          ASMS_Id = b.ASMS_Id
                                                      }).Distinct().ToArray();

                                foreach (var act2 in temp_stu_array)
                                {
                                    foreach (var act3 in _category.subj_list)
                                    {
                                        StudentMappingDMO objpge4 = Mapper.Map<StudentMappingDMO>(_category);
                                        var stu_result_cnt = _examcontext.StudentMappingDMO.Where(s => s.MI_Id == _category.MI_Id && s.ASMAY_Id == _category.ASMAY_Id && s.ASMCL_Id == act2.ASMCL_Id && s.ASMS_Id == act2.ASMS_Id && s.AMST_Id == act2.AMST_Id && s.ISMS_Id == act3.ISMS_Id).Count();
                                        if (stu_result_cnt == 0)
                                        {
                                            objpge4.ASMCL_Id = act2.ASMCL_Id;
                                            objpge4.ASMS_Id = act2.ASMS_Id;
                                            objpge4.AMST_Id = act2.AMST_Id;
                                            objpge4.ISMS_Id = act3.ISMS_Id;
                                            objpge4.ESTSU_ActiveFlg = true;
                                            objpge4.ESTSU_ElecetiveFlag = false;
                                            objpge4.CreatedDate = DateTime.Now;
                                            objpge4.UpdatedDate = DateTime.Now;
                                            objpge4.ESTSU_CreatedBy = _category.UserId;
                                            objpge4.ESTSU_UpdatedBy = _category.UserId;
                                            _examcontext.Add(objpge4);
                                        }
                                    }
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
                        }

                    }

                    else if (result.Count() == 0)
                    {
                        objpge.EYC_ActiveFlg = true;
                        objpge.EYC_BasedOnPaperTypeFlg = _category.EYC_BasedOnPaperTypeFlg;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _examcontext.Add(objpge);

                        _category.EYC_Id = objpge.EYC_Id;
                        Exm_Yearly_Category_GroupDMO objpge1 = Mapper.Map<Exm_Yearly_Category_GroupDMO>(_category);
                        objpge1.EYCG_ActiveFlg = true;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;

                        _examcontext.Add(objpge1);
                        _category.EYCG_Id = objpge1.EYCG_Id;

                        foreach (var act1 in _category.subj_list)
                        {
                            Exm_Yearly_Category_Group_SubjectsDMO objpge2 = Mapper.Map<Exm_Yearly_Category_Group_SubjectsDMO>(_category);
                            objpge2.ISMS_Id = act1.ISMS_Id;
                            objpge2.EYCGS_ActiveFlg = true;
                            objpge2.CreatedDate = DateTime.Now;
                            objpge2.UpdatedDate = DateTime.Now;
                            _examcontext.Add(objpge2);

                        }
                        bool EMG_ElectiveFlg = _examcontext.Exm_Master_GroupDMO.Where(g => g.MI_Id == _category.MI_Id && g.EMG_Id == _category.EMG_Id).Select(g => g.EMG_ElectiveFlg).First();
                        if (EMG_ElectiveFlg == false)
                        {
                            var class_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == _category.EMCA_Id
                            && c.MI_Id == _category.MI_Id).Select(c => c.ASMCL_Id).Distinct().ToList();
                            List<long> classes = new List<long>();
                            foreach (var cls in class_list)
                            {
                                classes.Add(cls);
                            }

                            var section_list = _examcontext.Exm_Category_ClassDMO.Where(c => c.ASMAY_Id == _category.ASMAY_Id && c.EMCA_Id == _category.EMCA_Id
                          && c.MI_Id == _category.MI_Id && classes.Contains(c.ASMCL_Id)).Select(c => c.ASMS_Id).Distinct().ToList();

                            List<long> sectionss = new List<long>();
                            foreach (var secs in section_list)
                            {
                                sectionss.Add(secs);
                            }

                            var temp_stu_array = (from a in _examcontext.Adm_M_Student
                                                  from b in _examcontext.School_Adm_Y_StudentDMO
                                                  where (a.MI_Id == _category.MI_Id && a.AMST_Id == b.AMST_Id && b.ASMAY_Id == _category.ASMAY_Id
                                                  && classes.Contains(b.ASMCL_Id) && sectionss.Contains(b.ASMS_Id) && a.AMST_SOL != "WD"
                                                  // && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1
                                                  )
                                                  select new CategorySubjectMappingDTO
                                                  {
                                                      AMST_Id = b.AMST_Id,
                                                      ASMCL_Id = b.ASMCL_Id,
                                                      ASMS_Id = b.ASMS_Id
                                                  }).Distinct().ToArray();

                            foreach (var act2 in temp_stu_array)
                            {
                                foreach (var act3 in _category.subj_list)
                                {
                                    StudentMappingDMO objpge4 = Mapper.Map<StudentMappingDMO>(_category);
                                    var stu_result_cnt = _examcontext.StudentMappingDMO.Where(s => s.MI_Id == _category.MI_Id && s.ASMAY_Id == _category.ASMAY_Id && s.ASMCL_Id == act2.ASMCL_Id && s.ASMS_Id == act2.ASMS_Id && s.AMST_Id == act2.AMST_Id && s.ISMS_Id == act3.ISMS_Id).Count();
                                    if (stu_result_cnt == 0)
                                    {
                                        objpge4.ASMCL_Id = act2.ASMCL_Id;
                                        objpge4.ASMS_Id = act2.ASMS_Id;
                                        objpge4.AMST_Id = act2.AMST_Id;
                                        objpge4.ISMS_Id = act3.ISMS_Id;
                                        objpge4.ESTSU_ActiveFlg = true;
                                        objpge4.ESTSU_ElecetiveFlag = false;
                                        objpge4.CreatedDate = DateTime.Now;
                                        objpge4.UpdatedDate = DateTime.Now;
                                        objpge4.ESTSU_CreatedBy = _category.UserId;
                                        objpge4.ESTSU_UpdatedBy = _category.UserId;
                                        _examcontext.Add(objpge4);
                                    }
                                }
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
                }

                _category.Grid_Details_list = (from a in _examcontext.Exm_Yearly_CategoryDMO
                                               from b in _examcontext.Exm_Master_CategoryDMO
                                               from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                               from d in _examcontext.Exm_Master_GroupDMO
                                               from e in _examcontext.AcademicYear
                                               where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.EMCA_Id == b.EMCA_Id
                                               && a.EYC_Id == c.EYC_Id && c.EMG_Id == d.EMG_Id && a.MI_Id == e.MI_Id && a.ASMAY_Id == e.ASMAY_Id)
                                               select new CategorySubjectMappingDTO
                                               {
                                                   EYC_Id = a.EYC_Id,
                                                   EYC_BasedOnPaperTypeFlg = a.EYC_BasedOnPaperTypeFlg ,
                                                   ASMAY_Id = a.ASMAY_Id,
                                                   ASMAY_Year = e.ASMAY_Year,
                                                   EMCA_CategoryName = b.EMCA_CategoryName,
                                                   EYCG_Id = c.EYCG_Id,
                                                   EMG_Id = d.EMG_Id,
                                                   EMG_GroupName = d.EMG_GroupName,
                                                   EMG_ElectiveFlg = d.EMG_ElectiveFlg,
                                                   EYC_ActiveFlg = a.EYC_ActiveFlg,
                                                   EYCG_ActiveFlg = c.EYCG_ActiveFlg,
                                                   ASMAY_Order = e.ASMAY_Order
                                               }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return _category;
        }
        public CategorySubjectMappingDTO get_cate_class(CategorySubjectMappingDTO id)
        {

            try
            {
                //List<TT_Category_Class_DMO> lorg = new List<TT_Category_Class_DMO>();
                //lorg = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => t.TTCC_Id.Equals(id)).ToList();
                //acdmc.binddetails = lorg.ToArray();

                //var lorg3 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) && t.EMCA_Id.Equals(id.EMCA_Id) &&  t.MI_Id==id.MI_Id).Select(d => d.ASMCL_Id).ToArray();

                //var lorg2 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) &&
                //!lorg3.Contains(t.ASMCL_Id) &&  t.MI_Id==id.MI_Id).Select(d => d.ASMCL_Id).ToArray();

                //List<AdmissionClass> lorg1 = new List<AdmissionClass>();
                //lorg1 = _examcontext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && !lorg2.Contains(t.ASMCL_Id)).ToList();
                //id.classlist = lorg1.ToArray();

                //var lorg4 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => lorg3.Contains(t.ASMCL_Id) &&  t.MI_Id==id.MI_Id).Select(d => d.ASMCL_Id).ToArray();

                //List<AdmissionClass> lorg5 = new List<AdmissionClass>();
                //lorg5 = _examcontext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && lorg4.Contains(t.ASMCL_Id)).ToList();
                //id.binddetails = lorg5.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public CategorySubjectMappingDTO getdetails(int id)
        {
            CategorySubjectMappingDTO TTMC = new CategorySubjectMappingDTO();
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == id && y.Is_Active == true).OrderByDescending(y => y.ASMAY_Order).ToList();
                TTMC.yearlist = year.Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<Exm_Master_CategoryDMO> categories = new List<Exm_Master_CategoryDMO>();
                categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == id && c.EMCA_ActiveFlag == true).ToList();
                TTMC.categorylist = categories.Distinct().ToArray();

                List<Exm_Master_GroupDMO> groups = new List<Exm_Master_GroupDMO>();
                groups = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == id && t.EMG_ActiveFlag == true).ToList();
                TTMC.grouplist = groups.Distinct().ToArray();

                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1).ToList();
                TTMC.subjectlist = subjects.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                TTMC.Grid_Details_list = (from a in _examcontext.Exm_Yearly_CategoryDMO
                                          from b in _examcontext.Exm_Master_CategoryDMO
                                          from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                          from d in _examcontext.Exm_Master_GroupDMO
                                          from e in _examcontext.AcademicYear
                                          where (a.MI_Id == id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.EMCA_Id == b.EMCA_Id && a.EYC_Id == c.EYC_Id && c.EMG_Id == d.EMG_Id && a.MI_Id == e.MI_Id && a.ASMAY_Id == e.ASMAY_Id)
                                          select new CategorySubjectMappingDTO
                                          {
                                              EYC_Id = a.EYC_Id,
                                              EYC_BasedOnPaperTypeFlg = a.EYC_BasedOnPaperTypeFlg,
                                              ASMAY_Id = a.ASMAY_Id,
                                              ASMAY_Year = e.ASMAY_Year,
                                              EMCA_CategoryName = b.EMCA_CategoryName,
                                              EYCG_Id = c.EYCG_Id,
                                              EMG_Id = d.EMG_Id,
                                              EMG_GroupName = d.EMG_GroupName,
                                              EMG_ElectiveFlg = d.EMG_ElectiveFlg,
                                              EYC_ActiveFlg = a.EYC_ActiveFlg,
                                              EYCG_ActiveFlg = c.EYCG_ActiveFlg,
                                              ASMAY_Order = e.ASMAY_Order
                                          }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public CategorySubjectMappingDTO geteventdetails(CategorySubjectMappingDTO _category)
        {
            // MasterSubjectGroupDTO TTMC = new MasterSubjectGroupDTO();
            //try
            //{

            //    List<COE_Master_EventsDMO> s_m_events = new List<COE_Master_EventsDMO>();
            //    s_m_events = _examcontext.COE_Master_EventsDMO.Where(e => e.MI_Id == _category.MI_Id && e.COEME_Id == _category.COEME_Id).ToList();
            //    _category.selected_master_event = s_m_events.ToArray();


            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return _category;

        }
        public CategorySubjectMappingDTO getalldetailsviewrecords(int id)
        {
            CategorySubjectMappingDTO page = new CategorySubjectMappingDTO();
            try
            {
                page.view_group_subjects = (from a in _examcontext.Exm_Master_CategoryDMO
                                            from b in _examcontext.Exm_Yearly_CategoryDMO
                                            from c in _examcontext.Exm_Master_GroupDMO
                                            from d in _examcontext.Exm_Yearly_Category_GroupDMO
                                            from e in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                            from f in _examcontext.IVRM_School_Master_SubjectsDMO
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == f.MI_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_Id == d.EYC_Id && c.EMG_Id == d.EMG_Id && d.EYCG_Id == e.EYCG_Id && e.ISMS_Id == f.ISMS_Id && d.EYCG_Id == id)
                                            select new CategorySubjectMappingDTO
                                            {
                                                //  EMGS_Id=n.EMGS_Id,
                                                EMG_Id = d.EMG_Id,
                                                EMCA_Id = a.EMCA_Id,
                                                EYC_Id = b.EYC_Id,
                                                EYCG_Id = d.EYCG_Id,
                                                EMG_GroupName = c.EMG_GroupName,
                                                EMCA_CategoryName = a.EMCA_CategoryName,
                                                ISMS_Id = f.ISMS_Id,
                                                ISMS_SubjectName = f.ISMS_SubjectName,
                                                ISMS_SubjectCode = f.ISMS_SubjectCode

                                            }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public CategorySubjectMappingDTO getpageedit(int id)
        {
            CategorySubjectMappingDTO page = new CategorySubjectMappingDTO();
            try
            {
                page.edit_m_group = (from x in _examcontext.Exm_Yearly_CategoryDMO
                                     from y in _examcontext.Exm_Yearly_Category_GroupDMO
                                     where (x.EYC_Id == y.EYC_Id && y.EYCG_Id == id)
                                     select new CategorySubjectMappingDTO
                                     {
                                         EYC_Id = x.EYC_Id,
                                         EYCG_Id = y.EYCG_Id,
                                         ASMAY_Id = x.ASMAY_Id,
                                         EMCA_Id = x.EMCA_Id,
                                         EMG_Id = y.EMG_Id,
                                         EYC_BasedOnPaperTypeFlg = x.EYC_BasedOnPaperTypeFlg,
                                     }).ToArray();

                List<Exm_Yearly_Category_Group_SubjectsDMO> group_subjects_m = new List<Exm_Yearly_Category_Group_SubjectsDMO>();
                group_subjects_m = _examcontext.Exm_Yearly_Category_Group_SubjectsDMO.Where(e => e.EYCG_Id == id).ToList();
                page.edit_m_group_subjects = group_subjects_m.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public CategorySubjectMappingDTO get_category(CategorySubjectMappingDTO page)
        {

            try
            {
                page.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Category_ClassDMO
                                     where (a.MI_Id == page.MI_Id && b.ASMAY_Id == page.ASMAY_Id && a.MI_Id == b.MI_Id && a.EMCA_Id == b.EMCA_Id
                                     && a.EMCA_ActiveFlag == true)
                                     select new CategorySubjectMappingDTO
                                     {
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EMCA_Id = a.EMCA_Id
                                     }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public CategorySubjectMappingDTO get_subjects(CategorySubjectMappingDTO page)
        {

            try
            {


                page.subjectlist = (from a in _examcontext.Exm_Master_GroupDMO
                                    from b in _examcontext.Exm_Master_Group_SubjectsDMO
                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                    where (a.MI_Id == page.MI_Id && a.MI_Id == c.MI_Id && a.EMG_Id == b.EMG_Id && a.EMG_Id == page.EMG_Id && c.ISMS_Id == b.ISMS_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1)
                                    select c).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public CategorySubjectMappingDTO deleterec(int id)
        {
            CategorySubjectMappingDTO page = new CategorySubjectMappingDTO();
            //try
            //{
            //    //List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
            //    //lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.Where(t => t.TTMSAB_Id.Equals(id)).ToList();
            //    //if (lorg.Any())
            //    //{
            //    //    _ttcategorycontext.Remove(lorg.ElementAt(0));
            //    //    var contactExists = _ttcategorycontext.SaveChanges();
            //    //    if (contactExists == 1)
            //    //    {
            //    //        page.returnval = true;
            //    //    }
            //    //    else
            //    //    {
            //    //        page.returnval = false;
            //    //    }
            //    //}

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return page;
        }

        //active deactive 
        public CategorySubjectMappingDTO deactivate(CategorySubjectMappingDTO data)
        {
            data.already_cnt = false;
            Exm_Yearly_Category_GroupDMO pge = Mapper.Map<Exm_Yearly_Category_GroupDMO>(data);
            if (pge.EYCG_Id > 0)
            {
                var result = _examcontext.Exm_Yearly_Category_GroupDMO.Single(t => t.EYCG_Id == pge.EYCG_Id);
                if (result.EYCG_ActiveFlg == true)
                {
                    var Exm_Yearly_Category_ExamsDMO_cnt = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == result.EYC_Id).ToList();
                    var Exm_M_PromotionDMO_cnt = _examcontext.Exm_M_PromotionDMO.Where(t => t.EYC_Id == result.EYC_Id).ToList();
                    if (Exm_Yearly_Category_ExamsDMO_cnt.Count == 0 && Exm_M_PromotionDMO_cnt.Count == 0)
                    {
                        result.EYCG_ActiveFlg = false;
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
                    result.EYCG_ActiveFlg = true;
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
        public CategorySubjectMappingDTO delete_group_subjects(int id)
        {
            CategorySubjectMappingDTO pagert = new CategorySubjectMappingDTO();

            try
            {
                List<Exm_Yearly_Category_Group_SubjectsDMO> lorg = new List<Exm_Yearly_Category_Group_SubjectsDMO>();
                lorg = _examcontext.Exm_Yearly_Category_Group_SubjectsDMO.Where(t => t.EYCG_Id == id).ToList();
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

        /* Category Dates Mapping */
        public CategorySubjectMappingDTO OnLoadCategoryDates(CategorySubjectMappingDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.yearlist = year.Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<Exm_Master_CategoryDMO> categories = new List<Exm_Master_CategoryDMO>();
                categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == data.MI_Id && c.EMCA_ActiveFlag == true).ToList();
                data.categorylist = categories.Distinct().ToArray();

                data.getdetails = (from a in _examcontext.AcademicYear
                                   from b in _examcontext.Exm_Yearly_CategoryDMO
                                   from c in _examcontext.Exm_Master_CategoryDMO
                                   where (a.ASMAY_Id == b.ASMAY_Id && b.EMCA_Id == c.EMCA_Id && b.MI_Id == data.MI_Id)
                                   select new CategorySubjectMappingDTO
                                   {
                                       EYC_Id = b.EYC_Id,
                                       ASMAY_Id = b.ASMAY_Id,
                                       EMCA_Id = b.EMCA_Id,
                                       ASMAY_Year = a.ASMAY_Year,
                                       EMCA_CategoryName = c.EMCA_CategoryName,
                                       ASMAY_Order = a.ASMAY_Order,
                                       EYC_ExamStartDate = b.EYC_ExamStartDate,
                                       EYC_ExamEndDate = b.EYC_ExamEndDate,
                                       EYC_MarksEntryLastDate = b.EYC_MarksEntryLastDate,
                                       EYC_MarksProcessLastDate = b.EYC_MarksProcessLastDate,
                                       EYC_MarksPublishDate = b.EYC_MarksPublishDate
                                   }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CategorySubjectMappingDTO get_categoryDates(CategorySubjectMappingDTO data)
        {
            try
            {
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true)
                                     select a).Distinct().ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CategorySubjectMappingDTO savedatadates(CategorySubjectMappingDTO data)
        {
            try
            {
                var checkrecord = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id).ToList();

                if (checkrecord.Count > 0)
                {
                    var result = _examcontext.Exm_Yearly_CategoryDMO.Single(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id
                     && a.EYC_Id == checkrecord.FirstOrDefault().EYC_Id);

                    result.EYC_ExamStartDate = data.EYC_ExamStartDate;
                    result.EYC_ExamEndDate = data.EYC_ExamEndDate;
                    result.EYC_MarksEntryLastDate = data.EYC_MarksEntryLastDate;
                    result.EYC_MarksProcessLastDate = data.EYC_MarksProcessLastDate;
                    result.EYC_MarksPublishDate = data.EYC_MarksPublishDate;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                    var i = _examcontext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}