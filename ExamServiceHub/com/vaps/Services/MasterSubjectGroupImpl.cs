
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
    public class MasterSubjectGroupImpl : Interfaces.MasterSubjectGroupInterface
    {
        private static ConcurrentDictionary<string, MasterSubjectGroupDTO> _login =
       new ConcurrentDictionary<string, MasterSubjectGroupDTO>();


        public ExamContext _examcontext;
        ILogger<MasterSubjectGroupImpl> _acdimpl;
        public MasterSubjectGroupImpl(ExamContext ttcategory)
        {
            _examcontext = ttcategory;
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
                        //result.MI_Id = objpge.MI_Id;
                        result.UpdatedDate = DateTime.Now;
                        result.EMG_ActiveFlag = true;

                        _examcontext.Update(result);

                        MasterSubjectGroupDTO req = delete_group_subjects(objpge.EMG_Id);

                        if (req.returnval == true)
                        {
                            foreach (var act1 in _category.subj_list)
                            {
                                Exm_Master_Group_SubjectsDMO objpge1 = Mapper.Map<Exm_Master_Group_SubjectsDMO>(_category);

                                //  objpge1.EMGR_Id = _category.EMGR_Id;
                                objpge1.ISMS_Id = act1.ISMS_Id;
                                objpge1.EMGS_ActiveFlag = true;
                                objpge1.CreatedDate = DateTime.Now;
                                // objpge1.CreatedDate = result.CreatedDate;
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
                        _category.EMG_Id = objpge.EMG_Id;


                        foreach (var act1 in _category.subj_list)
                        {
                            Exm_Master_Group_SubjectsDMO objpge1 = Mapper.Map<Exm_Master_Group_SubjectsDMO>(_category);

                            //  objpge1.EMGR_Id = _category.EMGR_Id;
                            objpge1.ISMS_Id = act1.ISMS_Id;
                            objpge1.EMGS_ActiveFlag = true;
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

                _category.Group_list = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == _category.MI_Id).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return _category;
        }

        public MasterSubjectGroupDTO get_cate_class(MasterSubjectGroupDTO id)
        {

            try
            {
                //List<TT_Category_Class_DMO> lorg = new List<TT_Category_Class_DMO>();
                //lorg = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => t.TTCC_Id.Equals(id)).ToList();
                //acdmc.binddetails = lorg.ToArray();

                //var lorg3 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) && t.EMCA_Id.Equals(id.EMCA_Id)).Select(d => d.ASMCL_Id).ToArray();

                //var lorg2 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) &&
                //!lorg3.Contains(t.ASMCL_Id)).Select(d => d.ASMCL_Id).ToArray();

                //List<AdmissionClass> lorg1 = new List<AdmissionClass>();
                //lorg1 = _examcontext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && !lorg2.Contains(t.ASMCL_Id)).ToList();
                //id.classlist = lorg1.ToArray();

                //var lorg4 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => lorg3.Contains(t.ASMCL_Id)).Select(d => d.ASMCL_Id).ToArray();

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
        public MasterSubjectGroupDTO getdetails(int id)
        {
            MasterSubjectGroupDTO TTMC = new MasterSubjectGroupDTO();
            try
            {
                TTMC.Group_list = _examcontext.Exm_Master_GroupDMO.Where(t => t.MI_Id == id).Distinct().ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == id && c.ISMS_ActiveFlag==1 &&c.ISMS_ExamFlag==1).ToList();
                TTMC.subjectlist = subjects.Distinct().OrderBy(t=>t.ISMS_OrderFlag).ToArray();

                //List<Exm_Master_CategoryDMO> Act_categories = new List<Exm_Master_CategoryDMO>();
                //Act_categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == id ).ToList();
                //TTMC.mastetr_category_list = Act_categories.Distinct().ToArray();

                //List<Exm_Master_CategoryDMO> categories = new List<Exm_Master_CategoryDMO>();
                //categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == id && c.EMCA_ActiveFlag==true).ToList();
                //TTMC.categorylist = categories.Distinct().ToArray();

                //TTMC.category_class_list = (from a in _examcontext.Exm_Master_CategoryDMO
                //                         from b in _examcontext.Exm_Category_ClassDMO
                //                         from c in _examcontext.AcademicYear
                //                         from d in _examcontext.AdmissionClass
                //                            where (a.MI_Id == id &&  a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id==c.MI_Id && a.MI_Id==d.MI_Id && b.ASMCL_Id==d.ASMCL_Id)
                //                         select new MasterSubjectGroupDTO
                //                         {
                //                             ASMAY_Year = c.ASMAY_Year,
                //                             ECAC_Id = b.ECAC_Id,
                //                             EMCA_Id = a.EMCA_Id,
                //                             ASMAY_Id=b.ASMAY_Id,
                //                             ASMCL_Id=b.ASMCL_Id,
                //                             ASMCL_ClassName=d.ASMCL_ClassName,
                //                             EMCA_CategoryName = a.EMCA_CategoryName,
                //                             ECAC_ActiveFlag = b.ECAC_ActiveFlag,
                //                         }
                //                  ).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }


        public MasterSubjectGroupDTO geteventdetails(MasterSubjectGroupDTO _category)
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


        public MasterSubjectGroupDTO getalldetailsviewrecords(int id)
        {
            MasterSubjectGroupDTO page = new MasterSubjectGroupDTO();
            try
            {
                page.view_group_subjects = (from m in _examcontext.Exm_Master_GroupDMO
                                   from n in _examcontext.Exm_Master_Group_SubjectsDMO
                                   from l in _examcontext.IVRM_School_Master_SubjectsDMO
                                   where (m.MI_Id==l.MI_Id && m.EMG_Id==id && m.EMG_Id==n.EMG_Id && n.ISMS_Id==l.ISMS_Id)
                                   select new MasterSubjectGroupDTO
                                   {
                                       EMGS_Id=n.EMGS_Id,
                                       EMG_Id=m.EMG_Id,
                                       EMG_GroupName=m.EMG_GroupName,
                                       ISMS_Id=n.ISMS_Id,
                                       ISMS_SubjectName=l.ISMS_SubjectName,
                                       ISMS_SubjectCode=l.ISMS_SubjectCode,
                                       EMGS_ActiveFlag=n.EMGS_ActiveFlag,

                                   }).ToArray();

                //List<Exm_Master_Group_SubjectsDMO> group_subjects = new List<Exm_Master_Group_SubjectsDMO>();
                //group_subjects = _examcontext.Exm_Master_Group_SubjectsDMO.Where(e => e.EMG_Id == id).ToList();
                //page.view_group_subjects = group_subjects.ToArray();
                //page.EMG_GroupName = _examcontext.Exm_Master_GroupDMO.Where(t => t.EMG_Id == id).Select(x => x.EMG_GroupName).First().ToString();

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
                List<Exm_Master_Group_SubjectsDMO> group_subjects_m = new List<Exm_Master_Group_SubjectsDMO>();
                group_subjects_m = _examcontext.Exm_Master_Group_SubjectsDMO.Where(e => e.EMG_Id == id).ToList();
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
        public MasterSubjectGroupDTO deactivate(MasterSubjectGroupDTO data)
        {
            data.already_cnt = false;
            Exm_Master_GroupDMO pge = Mapper.Map<Exm_Master_GroupDMO>(data);
            if (pge.EMG_Id > 0)
            {
                var result = _examcontext.Exm_Master_GroupDMO.Single(t => t.EMG_Id == pge.EMG_Id);
                if (result.EMG_ActiveFlag == true)
                {
                    var Exm_Yearly_Category_GroupDMO_cnt = _examcontext.Exm_Yearly_Category_GroupDMO.Where(t => t.EMG_Id == data.EMG_Id).ToList();
                    var Exm_Studentwise_Subjects_cnt = _examcontext.StudentMappingDMO.Where(t => t.EMG_Id == data.EMG_Id).ToList();
                    if (Exm_Yearly_Category_GroupDMO_cnt.Count == 0 && Exm_Studentwise_Subjects_cnt.Count == 0)
                    {
                        result.EMG_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                   // result.EMG_ActiveFlag = false;
                }
                else
                {
                    result.EMG_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                // result.UpdatedDate = DateTime.Now;
                // _examcontext.Update(result);
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
            //  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            try
            {

                List<Exm_Master_Group_SubjectsDMO> lorg = new List<Exm_Master_Group_SubjectsDMO>();
                lorg = _examcontext.Exm_Master_Group_SubjectsDMO.Where(t => t.EMG_Id == id).ToList();
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
