
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
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
    public class exammastercategoryImpl : Interfaces.exammastercategoryInterface
    {
        private static ConcurrentDictionary<string, exammastercategoryDTO> _login = new ConcurrentDictionary<string, exammastercategoryDTO>();
        public ExamContext _examcontext;
        ILogger<exammastercategoryImpl> _acdimpl;
        public exammastercategoryImpl(ExamContext ttcategory, ILogger<exammastercategoryImpl> _acdimp)
        {
            _examcontext = ttcategory;
            _acdimpl = _acdimp;
        }
        public exammastercategoryDTO savedetail1(exammastercategoryDTO _category)
        {
            Exm_Master_CategoryDMO objpge = Mapper.Map<Exm_Master_CategoryDMO>(_category);
            try
            {
                if (objpge.EMCA_Id > 0)
                {
                    var result321 = _examcontext.Exm_Master_CategoryDMO.Where(t => t.EMCA_CategoryName.Equals(objpge.EMCA_CategoryName) && t.MI_Id.Equals(objpge.MI_Id) && t.EMCA_Id != objpge.EMCA_Id);
                    if (result321.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _examcontext.Exm_Master_CategoryDMO.Single(t => t.EMCA_Id.Equals(objpge.EMCA_Id) && t.MI_Id.Equals(objpge.MI_Id));
                        result.EMCA_CategoryName = objpge.EMCA_CategoryName;
                        result.EMCA_CCECheckingFlg = objpge.EMCA_CCECheckingFlg;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _examcontext.Exm_Master_CategoryDMO.Where(t => t.EMCA_CategoryName.Equals(objpge.EMCA_CategoryName) && t.MI_Id.Equals(objpge.MI_Id));
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        objpge.EMCA_ActiveFlag = true;
                        objpge.EMCA_CCECheckingFlg = objpge.EMCA_CCECheckingFlg;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _examcontext.Add(objpge);
                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
                List<Exm_Master_CategoryDMO> Act_categories = new List<Exm_Master_CategoryDMO>();
                Act_categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == _category.MI_Id).ToList();
                _category.mastetr_category_list = Act_categories.Distinct().ToArray();

                List<Exm_Master_CategoryDMO> categories = new List<Exm_Master_CategoryDMO>();
                categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == _category.MI_Id && c.EMCA_ActiveFlag == true).ToList();
                _category.categorylist = categories.Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _category.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        //public exammastercategoryDTO savedetail2(exammastercategoryDTO acd)
        //{
        //    try
        //    {
        //        var amcl = acd.clssids.Select(t => t.ASMCL_Id).ToArray();

        //        List<Exm_Category_ClassDMO> Allname5 = new List<Exm_Category_ClassDMO>();

        //        Allname5 = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id.Equals(acd.ASMAY_Id) && t.EMCA_Id.Equals(acd.EMCA_Id) && !amcl.Contains(t.ASMCL_Id)).ToList().ToList();

        //        for (int p = 0; p < Allname5.Count; p++)
        //        {
        //            List<Exm_Category_ClassDMO> lorg = new List<Exm_Category_ClassDMO>();

        //            lorg = _examcontext.Exm_Category_ClassDMO.Where(t => t.ECAC_Id == Allname5[p].ECAC_Id).ToList();

        //            if (lorg.Any())
        //            {
        //                _examcontext.Remove(lorg.ElementAt(0));
        //                var flag = _examcontext.SaveChanges();

        //                if (flag == 1)
        //                {
        //                    acd.returnMsg = "update";

        //                    acd.detailslist = (from a in _examcontext.Exm_Master_CategoryDMO
        //                                       from b in _examcontext.Exm_Category_ClassDMO
        //                                       from c in _examcontext.AcademicYear
        //                                       from d in _examcontext.AdmissionClass
        //                                       where (a.MI_Id == acd.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && b.ASMCL_Id == d.ASMCL_Id)
        //                                       group c by new
        //                                       {
        //                                           c.ASMAY_Year,
        //                                           a.EMCA_Id,
        //                                           b.ASMAY_Id,
        //                                           b.ASMCL_Id,
        //                                           d.ASMCL_ClassName,
        //                                           a.EMCA_CategoryName,
        //                                           b.ECAC_ActiveFlag,

        //                                       } into temp
        //                                       select new exammastercategoryDTO
        //                                       {
        //                                           ASMAY_Year = temp.Key.ASMAY_Year,
        //                                           //ECAC_Id = b.ECAC_Id,
        //                                           EMCA_Id = temp.Key.EMCA_Id,
        //                                           ASMAY_Id = temp.Key.ASMAY_Id,
        //                                           ASMCL_Id = temp.Key.ASMCL_Id,
        //                                           ASMCL_ClassName = temp.Key.ASMCL_ClassName,
        //                                           EMCA_CategoryName = temp.Key.EMCA_CategoryName,
        //                                           ECAC_ActiveFlag = temp.Key.ECAC_ActiveFlag,
        //                                       }
        //                          ).ToArray();
        //                }
        //                else
        //                {
        //                    acd.returnMsg = "";
        //                }
        //            }

        //        }

        //        string classname = "";

        //        for (int j = 0; j < acd.clssids.Length; j++)
        //        {



        //            Exm_Category_ClassDMO enq = Mapper.Map<Exm_Category_ClassDMO>(acd);

        //            List<Exm_Category_ClassDMO> Allname2 = new List<Exm_Category_ClassDMO>();

        //            Allname2 = _examcontext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id.Equals(acd.ASMAY_Id) && t.ASMCL_Id.Equals(acd.clssids[j].ASMCL_Id)).ToList().ToList();

        //            if (Allname2.Count > 0)
        //            {


        //            }
        //            else
        //            {
        //                var classid = _examcontext.Masterclasscategory.Where(t => t.MI_Id == acd.MI_Id && t.Is_Active == true && t.ASMAY_Id == acd.ASMAY_Id && t.ASMCL_Id == acd.clssids[j].ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

        //                List<Adm_School_Master_Class_Cat_SecDMO> Allname3 = new List<Adm_School_Master_Class_Cat_SecDMO>();

        //                Allname3 = _examcontext.Adm_School_Master_Class_Cat_SecDMO.Where(t => classid.Contains(t.ASMCC_Id) && acd.secids.Contains(t.ASMS_Id)).ToList().ToList();

        //                if (Allname3.Count > 0)
        //                {
        //                    for (int i = 0; i < Allname3.Count; i++)
        //                    {
        //                        Exm_Category_ClassDMO enq1 = Mapper.Map<Exm_Category_ClassDMO>(acd);
        //                        //enq1.MI_Id = enq.MI_Id;
        //                        enq1.EMCA_Id = acd.EMCA_Id;
        //                        enq1.ASMCL_Id = acd.clssids[j].ASMCL_Id;
        //                        enq1.ASMAY_Id = enq.ASMAY_Id;
        //                        enq1.ASMS_Id = Allname3[i].ASMS_Id;
        //                        enq1.ECAC_ActiveFlag = true;
        //                        enq1.CreatedDate = DateTime.Now;
        //                        enq1.UpdatedDate = DateTime.Now;

        //                        _examcontext.Add(enq1);


        //                        var flag = _examcontext.SaveChanges();
        //                        if (flag == 1)
        //                        {
        //                            acd.returnMsg = "add";

        //                            acd.detailslist = (from a in _examcontext.Exm_Master_CategoryDMO
        //                                               from b in _examcontext.Exm_Category_ClassDMO
        //                                               from c in _examcontext.AcademicYear
        //                                               from d in _examcontext.AdmissionClass
        //                                               where (a.MI_Id == acd.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && b.ASMCL_Id == d.ASMCL_Id)
        //                                               group c by new
        //                                               {
        //                                                   c.ASMAY_Year,
        //                                                   a.EMCA_Id,
        //                                                   b.ASMAY_Id,
        //                                                   b.ASMCL_Id,
        //                                                   d.ASMCL_ClassName,
        //                                                   a.EMCA_CategoryName,
        //                                                   b.ECAC_ActiveFlag,

        //                                               } into temp
        //                                               select new exammastercategoryDTO
        //                                               {
        //                                                   ASMAY_Year = temp.Key.ASMAY_Year,
        //                                                   //ECAC_Id = b.ECAC_Id,
        //                                                   EMCA_Id = temp.Key.EMCA_Id,
        //                                                   ASMAY_Id = temp.Key.ASMAY_Id,
        //                                                   ASMCL_Id = temp.Key.ASMCL_Id,
        //                                                   ASMCL_ClassName = temp.Key.ASMCL_ClassName,
        //                                                   EMCA_CategoryName = temp.Key.EMCA_CategoryName,
        //                                                   ECAC_ActiveFlag = temp.Key.ECAC_ActiveFlag,
        //                                               }
        //                          ).ToArray();
        //                        }
        //                        else
        //                        {
        //                            acd.returnMsg = "";
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    var classname1 = _examcontext.AdmissionClass.Where(t => t.ASMCL_Id == acd.clssids[j].ASMCL_Id).Select(t => t.ASMCL_ClassName).ToArray();

        //                    if (classname == "")
        //                    {
        //                        classname = classname1[0];
        //                    }
        //                    else
        //                    {
        //                        classname = classname + "," + classname1[0];
        //                    }

        //                }
        //            }
        //        }

        //        if (classname != "")
        //        {
        //            acd.msg = "Section are not mapped in admission module for these classes : " + classname;
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        _acdimpl.LogError(ee.Message);
        //        _acdimpl.LogDebug(ee.Message);
        //        Console.WriteLine(ee.Message);
        //    }


        //    return acd;
        //}
        public exammastercategoryDTO savedetail2(exammastercategoryDTO id)
        {
            Exm_Category_ClassDMO objpge = Mapper.Map<Exm_Category_ClassDMO>(id);
            try
            {
                if (objpge.ECAC_Id > 0)
                {
                    exammastercategoryDTO cat = deleterec_cat_cls(id);
                    if (cat.returnval == true)
                    {
                        for (int i = 0; i < id.selected_temp.Length; i++)
                        {
                            for (int j = 0; j < id.selected_temp[i].sections.Length; j++)
                            {
                                var result = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.selected_temp[i].ASMAY_Id && t.ASMCL_Id == id.selected_temp[i].ASMCL_Id && t.ASMS_Id == id.selected_temp[i].sections[j].ASMS_Id);
                                // t.EMCA_Id == id.selected_temp[i].EMCA_Id &&
                                if (result.Count() > 0)
                                {
                                    id.returnduplicatestatus = "Duplicate";
                                }
                                else
                                {
                                    Exm_Category_ClassDMO obj_ccs = new Exm_Category_ClassDMO();
                                    // obj_ccs.ECAC_Id = objpge.ECAC_Id;
                                    obj_ccs.MI_Id = objpge.MI_Id;
                                    obj_ccs.ASMAY_Id = id.selected_temp[i].ASMAY_Id;
                                    obj_ccs.EMCA_Id = id.selected_temp[i].EMCA_Id;
                                    obj_ccs.ASMCL_Id = id.selected_temp[i].ASMCL_Id;
                                    obj_ccs.ASMS_Id = id.selected_temp[i].sections[j].ASMS_Id;
                                    obj_ccs.ECAC_ActiveFlag = true;
                                    obj_ccs.CreatedDate = DateTime.Now;
                                    obj_ccs.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_ccs);
                                }
                            }
                        }

                    }

                }
                else
                {
                    for (int i = 0; i < id.selected_temp.Length; i++)
                    {
                        for (int j = 0; j < id.selected_temp[i].sections.Length; j++)
                        {
                            var result = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.selected_temp[i].ASMAY_Id && t.ASMCL_Id == id.selected_temp[i].ASMCL_Id && t.ASMS_Id == id.selected_temp[i].sections[j].ASMS_Id);
                            // t.EMCA_Id == id.selected_temp[i].EMCA_Id &&
                            if (result.Count() > 0)
                            {
                                id.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                Exm_Category_ClassDMO obj_ccs = new Exm_Category_ClassDMO();
                                //  obj_ccs.ECAC_Id = objpge.ECAC_Id;
                                obj_ccs.MI_Id = objpge.MI_Id;
                                obj_ccs.ASMAY_Id = id.selected_temp[i].ASMAY_Id;
                                obj_ccs.EMCA_Id = id.selected_temp[i].EMCA_Id;
                                obj_ccs.ASMCL_Id = id.selected_temp[i].ASMCL_Id;
                                obj_ccs.ASMS_Id = id.selected_temp[i].sections[j].ASMS_Id;
                                obj_ccs.ECAC_ActiveFlag = true;
                                obj_ccs.CreatedDate = DateTime.Now;
                                obj_ccs.UpdatedDate = DateTime.Now;
                                _examcontext.Add(obj_ccs);
                            }
                        }
                    }
                }
                var contactExists = _examcontext.SaveChanges();
                if (contactExists >= 1)
                {
                    id.returnval = true;
                }
                else
                {
                    id.returnval = false;
                }
                id.category_class_list = (from a in _examcontext.Exm_Master_CategoryDMO
                                          from b in _examcontext.Exm_Category_ClassDMO
                                          from c in _examcontext.AcademicYear
                                          from d in _examcontext.AdmissionClass
                                          where (a.MI_Id == id.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && b.ASMCL_Id == d.ASMCL_Id)
                                          select new exammastercategoryDTO
                                          {
                                              ASMAY_Year = c.ASMAY_Year,
                                              EMCA_Id = a.EMCA_Id,
                                              ASMAY_Id = b.ASMAY_Id,
                                              ASMCL_Id = b.ASMCL_Id,
                                              ASMCL_ClassName = d.ASMCL_ClassName,
                                              EMCA_CategoryName = a.EMCA_CategoryName,
                                              ASMAY_Order = c.ASMAY_Order
                                          }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public exammastercategoryDTO deleterec_cat_cls(exammastercategoryDTO page)
        {

            try
            {
                List<Exm_Category_ClassDMO> lorg = new List<Exm_Category_ClassDMO>();
                var lorg1 = _examcontext.Exm_Category_ClassDMO.Single(t => t.MI_Id == page.MI_Id && t.ECAC_Id == page.ECAC_Id);
                lorg = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == page.MI_Id && t.ASMAY_Id == lorg1.ASMAY_Id && t.EMCA_Id == lorg1.EMCA_Id && t.ASMCL_Id == lorg1.ASMCL_Id).ToList();

                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _examcontext.Remove(lorg.ElementAt(i));
                    }
                }
                var contactExists = _examcontext.SaveChanges();
                if (contactExists >= 1)
                {
                    page.returnval = true;
                }
                else
                {
                    page.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public exammastercategoryDTO get_cate_class(exammastercategoryDTO id)
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

                var cat_clas = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(c => c.MI_Id == id.MI_Id && c.ASMAY_Id == id.ASMAY_Id).Select(s => s.ASMCL_Id).Distinct().ToList();

                var clas_list_N_cat = _examcontext.AdmissionClass.AsNoTracking().Where(c => c.MI_Id == id.MI_Id && c.ASMCL_ActiveFlag == true && !cat_clas.Contains(c.ASMCL_Id)).ToList();
                List<long> pend_sec_cls = new List<long>();
                //   List<long> N_cat_cls = new List<long>();
                foreach (var x in cat_clas)
                {
                    var sectionlist = (from m in _examcontext.Masterclasscategory
                                       from n in _examcontext.AdmSchoolMasterClassCatSec
                                       from o in _examcontext.School_M_Section
                                       where (m.ASMCC_Id == n.ASMCC_Id && n.ASMS_Id == o.ASMS_Id
                                       && o.ASMC_ActiveFlag == 1 && o.MI_Id == id.MI_Id && m.Is_Active == true
                                       && m.ASMCL_Id == x && m.ASMAY_Id == id.ASMAY_Id && o.ASMC_ActiveFlag == 1)
                                       select new School_M_Section
                                       {
                                           ASMS_Id = o.ASMS_Id,
                                           ASMC_SectionName = o.ASMC_SectionName,
                                           ASMC_SectionCode = o.ASMC_SectionCode,
                                           ASMC_Order = o.ASMC_Order,
                                           ASMC_MaxCapacity = o.ASMC_MaxCapacity,
                                           ASMC_ActiveFlag = o.ASMC_ActiveFlag,

                                       }).Distinct().ToList();
                    var cat_cls_secs = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(c => c.MI_Id == id.MI_Id && c.ASMAY_Id == id.ASMAY_Id && c.ASMCL_Id == x).ToList();
                    if (sectionlist.Count != cat_cls_secs.Count && sectionlist.Count > cat_cls_secs.Count)
                    {
                        pend_sec_cls.Add(x);
                    }
                    //  N_cat_cls.Add(x);
                }


                //for(int i = 0; i < cat_clas.Count; i++) { 
                //     foreach(var cls1 in pend_sec_cls)
                //     {
                //         if(cat_clas[i]==cls1)
                //         {
                //             N_cat_cls.RemoveAt(i);
                //             break;
                //         }
                //     }
                // }

                for (int i = 0; i < pend_sec_cls.Count; i++)
                {
                    foreach (var x in cat_clas)
                    {
                        if (x == pend_sec_cls[i])
                        {
                            cat_clas.Remove(x);
                            break;
                        }
                    }
                }

                id.classlist = _examcontext.AdmissionClass.AsNoTracking().Where(c => c.MI_Id == id.MI_Id && c.ASMCL_ActiveFlag == true && !cat_clas.Contains(c.ASMCL_Id)).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public exammastercategoryDTO get_cls_sections(exammastercategoryDTO id)
        {

            try
            {
                // var amcl = id.clssids.Where(c=>c.MI_Id==id.MI_Id).Select(t => t.ASMCL_Id).ToArray();
                //var lorg3 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) && t.EMCA_Id.Equals(id.EMCA_Id) && amcl.Contains(t.ASMCL_Id)).Select(d => d.ASMS_Id).ToArray();

                //var lorg2 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) &&
                //!lorg3.Contains(t.ASMS_Id)).Select(d => d.ASMS_Id).ToArray();

                //List<AdmissionClass> lorg1 = new List<AdmissionClass>();
                //lorg1 = _examcontext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && !lorg2.Contains(t.ASMCL_Id)).ToList();
                //id.classlist = lorg1.ToArray();

                //var lorg4 = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(t => lorg3.Contains(t.ASMCL_Id)).Select(d => d.ASMCL_Id).ToArray();

                //List<AdmissionClass> lorg5 = new List<AdmissionClass>();
                //lorg5 = _examcontext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && lorg4.Contains(t.ASMCL_Id)).ToList();
                //id.binddetails = lorg5.ToArray();



                id.sectionlist = (from m in _examcontext.Masterclasscategory
                                  from n in _examcontext.AdmSchoolMasterClassCatSec
                                  from o in _examcontext.School_M_Section
                                  where (m.ASMCC_Id == n.ASMCC_Id && n.ASMS_Id == o.ASMS_Id
                                  && o.ASMC_ActiveFlag == 1 && o.MI_Id == id.MI_Id && m.Is_Active == true
                                  && m.ASMCL_Id == id.ASMCL_Id && m.ASMAY_Id == id.ASMAY_Id && o.ASMC_ActiveFlag == 1)
                                  select new School_M_Section
                                  {
                                      ASMS_Id = o.ASMS_Id,
                                      ASMC_SectionName = o.ASMC_SectionName,
                                      ASMC_SectionCode = o.ASMC_SectionCode,
                                      ASMC_Order = o.ASMC_Order,
                                      ASMC_MaxCapacity = o.ASMC_MaxCapacity,
                                      ASMC_ActiveFlag = o.ASMC_ActiveFlag,

                                  }).Distinct().ToArray();

                var sections = _examcontext.Exm_Category_ClassDMO.AsNoTracking().Where(s => s.MI_Id == id.MI_Id && s.ASMCL_Id == id.ASMCL_Id && s.ASMAY_Id == id.ASMAY_Id).Distinct().Select(s => s.ASMS_Id).ToList();
                id.selected_sections = sections.ToArray();

                id.avail_sections = (from m in _examcontext.Masterclasscategory
                                     from n in _examcontext.AdmSchoolMasterClassCatSec
                                     from o in _examcontext.School_M_Section
                                     where (m.ASMCC_Id == n.ASMCC_Id && n.ASMS_Id == o.ASMS_Id
                                     && o.ASMC_ActiveFlag == 1 && o.MI_Id == id.MI_Id && m.Is_Active == true
                                     && m.ASMCL_Id == id.ASMCL_Id && m.ASMAY_Id == id.ASMAY_Id && o.ASMC_ActiveFlag == 1 && !sections.Contains(n.ASMS_Id))
                                     select o).Distinct().OrderBy(t => t.ASMC_Order).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public exammastercategoryDTO getdetails(int id)
        {
            exammastercategoryDTO TTMC = new exammastercategoryDTO();
            try
            {

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                TTMC.yearlist = year.Distinct().ToArray();


                List<AdmissionClass> classes = new List<AdmissionClass>();
                classes = _examcontext.AdmissionClass.Where(c => c.MI_Id == id && c.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                TTMC.classlist = classes.Distinct().ToArray();

                List<School_M_Section> sections = new List<School_M_Section>();
                sections = _examcontext.School_M_Section.Where(c => c.MI_Id == id && c.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                TTMC.sectionlist = sections.Distinct().ToArray();

                List<Exm_Master_CategoryDMO> Act_categories = new List<Exm_Master_CategoryDMO>();
                Act_categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == id).ToList();
                TTMC.mastetr_category_list = Act_categories.Distinct().ToArray();

                List<Exm_Master_CategoryDMO> categories = new List<Exm_Master_CategoryDMO>();
                categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == id && c.EMCA_ActiveFlag == true).ToList();
                TTMC.categorylist = categories.Distinct().ToArray();

                TTMC.category_class_list = (from a in _examcontext.Exm_Master_CategoryDMO
                                            from b in _examcontext.Exm_Category_ClassDMO
                                            from c in _examcontext.AcademicYear
                                            from d in _examcontext.AdmissionClass
                                            where (a.MI_Id == id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id
                                            && b.ASMCL_Id == d.ASMCL_Id)
                                            select new exammastercategoryDTO
                                            {
                                                ASMAY_Year = c.ASMAY_Year,
                                                EMCA_Id = a.EMCA_Id,
                                                ASMAY_Id = b.ASMAY_Id,
                                                ASMCL_Id = b.ASMCL_Id,
                                                ASMCL_ClassName = d.ASMCL_ClassName,
                                                EMCA_CategoryName = a.EMCA_CategoryName,
                                                ASMAY_Order = c.ASMAY_Order
                                            }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


                TTMC.get_format_mappeddetails = (from a in _examcontext.EXM_ProgressCard_FormatsDMO
                                                 from b in _examcontext.AcademicYear
                                                 from c in _examcontext.Exm_Master_CategoryDMO
                                                 where (a.ASMAY_Id == b.ASMAY_Id && a.EMCA_Id == c.EMCA_Id && a.MI_Id == id)
                                                 select new exammastercategoryDTO
                                                 {
                                                     ASMAY_Year = b.ASMAY_Year,
                                                     EMCA_Id = a.EMCA_Id,
                                                     ASMAY_Id = b.ASMAY_Id,
                                                     EMCA_CategoryName = c.EMCA_CategoryName,
                                                     ASMAY_Order = b.ASMAY_Order,
                                                     examorpromotionflag = a.EPCFT_ExamFlag,
                                                     EPCFT_Id = a.EPCFT_Id,
                                                     EPCFT_ActiveFlg = a.EPCFT_ActiveFlg
                                                 }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public exammastercategoryDTO getalldetailsviewrecords(exammastercategoryDTO id)
        {

            try
            {
                id.view_cls_sections = (from a in _examcontext.Exm_Master_CategoryDMO
                                        from b in _examcontext.Exm_Category_ClassDMO
                                        from c in _examcontext.School_M_Section
                                        from d in _examcontext.AdmissionClass
                                        where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == id.MI_Id && b.EMCA_Id == a.EMCA_Id && a.EMCA_Id == id.EMCA_Id && b.ASMS_Id == c.ASMS_Id && b.MI_Id == d.MI_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMCL_Id == id.ASMCL_Id && b.ASMAY_Id == id.ASMAY_Id)
                                        select new exammastercategoryDTO
                                        {
                                            ECAC_Id = b.ECAC_Id,
                                            ASMAY_Id = b.ASMAY_Id,
                                            EMCA_Id = b.EMCA_Id,
                                            ASMCL_Id = b.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            EMCA_CategoryName = a.EMCA_CategoryName,
                                            ASMCL_ClassName = d.ASMCL_ClassName,
                                            ASMC_SectionName = c.ASMC_SectionName,
                                            ECAC_ActiveFlag = b.ECAC_ActiveFlag,


                                        }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public exammastercategoryDTO deactivate_sec(exammastercategoryDTO data)
        {
            Exm_Category_ClassDMO pge = Mapper.Map<Exm_Category_ClassDMO>(data);
            if (pge.ECAC_Id > 0)
            {
                var result = _examcontext.Exm_Category_ClassDMO.Single(t => t.ECAC_Id == pge.ECAC_Id);
                if (result.ECAC_ActiveFlag == true)
                {
                    result.ECAC_ActiveFlag = false;
                }
                else
                {
                    result.ECAC_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _examcontext.Update(result);
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
        public exammastercategoryDTO geteventdetails(exammastercategoryDTO _category)
        {
            // exammastercategoryDTO TTMC = new exammastercategoryDTO();
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
        public exammastercategoryDTO getalldetailsviewrecords1(int id)
        {
            exammastercategoryDTO page = new exammastercategoryDTO();
            //try
            //{
            //    List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
            //    events_m = _examcontext.COE_Master_EventsDMO.Where(e => e.COEME_Id == id).ToList();
            //    page.edit_m_event = events_m.ToArray();

            //    //List<TT_Master_Staff_Abbreviation_DMO> lorg = new List<TT_Master_Staff_Abbreviation_DMO>();
            //    ////lorg = _ttcategorycontext.subAbbriList.AsNoTracking().Where(t => t.TTMSUAB_Id.Equals(id)).ToList();
            //    ////page.sujectslistedit = lorg.ToArray();

            //    //page.sujectslistedit = (from MasterStaff in _ttcategorycontext.Master_staff
            //    //                      from TT_Master_Staff_Abbreviation_DMO in _ttcategorycontext.staffAbbriList
            //    //                        where (MasterStaff.IVRMSTAUL_Id == TT_Master_Staff_Abbreviation_DMO.HRME_Id && TT_Master_Staff_Abbreviation_DMO.TTMSAB_Id == id)
            //    //                      select new TTexammastercategoryDTO
            //    //                      {
            //    //                          HRME_Id = MasterStaff.IVRMSTAUL_Id,
            //    //                          TTMSAB_Id = TT_Master_Staff_Abbreviation_DMO.TTMSAB_Id,
            //    //                          TTMSAB_Abbreviation = TT_Master_Staff_Abbreviation_DMO.TTMSAB_Abbreviation
            //    //                      }
            //    //                       ).ToArray();

            //    //List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
            //    //lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.AsNoTracking().Where(t => t.TTMSAB_Id.Equals(id)).ToList();
            //    //page.sujectslistedit = lorg.ToArray();


            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return page;
        }
        public exammastercategoryDTO getalldetailsviewrecords2(int id)
        {
            exammastercategoryDTO page = new exammastercategoryDTO();
            //try
            //{
            //    List<COE_EventsDMO> events_map = new List<COE_EventsDMO>();
            //    events_map = _examcontext.COE_EventsDMO.Where(e => e.COEE_Id == id).ToList();
            //    page.edit_map_event = events_map.ToArray();
            //    var m_id = events_map[0].COEME_Id;
            //    List<COE_Master_EventsDMO> events_m = new List<COE_Master_EventsDMO>();
            //    events_m = _examcontext.COE_Master_EventsDMO.Where(e => e.COEME_Id == m_id).ToList();
            //    page.edit_m_event = events_m.ToArray();

            //    //List<COE_Events_ClassesDMO> class_map = new List<COE_Events_ClassesDMO>();
            //    //class_map = _examcontext.COE_Events_ClassesDMO.Where(e => e.COEE_Id == id).ToList();
            //    //page.edit_stu_class_list = class_map.ToArray();
            //    page.edit_stu_class_list = (from a in _examcontext.COE_Events_ClassesDMO
            //                                from b in _examcontext.AdmissionClass
            //                                where (a.COEE_Id == id && a.ASMCL_Id == b.ASMCL_Id)
            //                                select new exammastercategoryDTO
            //                                {
            //                                    COEEC_Id = a.COEEC_Id,
            //                                    COEE_Id = a.COEE_Id,
            //                                    ASMCL_Id = a.ASMCL_Id,
            //                                    ASMCL_ClassName = b.ASMCL_ClassName,
            //                                }
            //                              ).Distinct().ToArray();


            //    //List<COE_Events_EmployeesDMO> emps_map = new List<COE_Events_EmployeesDMO>();
            //    //emps_map = _examcontext.COE_Events_EmployeesDMO.Where(e => e.COEE_Id == id).ToList();
            //    //page.edit_emp_type_list = emps_map.ToArray();
            //    page.edit_emp_type_list = (from a in _examcontext.COE_Events_EmployeesDMO
            //                               from b in _examcontext.HR_Master_GroupTypeDMO
            //                               where (a.COEE_Id == id && a.HRMGT_Id == b.HRMGT_Id)
            //                               select new exammastercategoryDTO
            //                               {
            //                                   COEEE_Id = a.COEEE_Id,
            //                                   COEE_Id = a.COEE_Id,
            //                                   HRMGT_Id = a.HRMGT_Id,
            //                                   HRMGT_EmployeeGroupType = b.HRMGT_EmployeeGroupType,

            //                               }
            //                            ).Distinct().ToArray();

            //    List<COE_Events_OthersDMO> others_map = new List<COE_Events_OthersDMO>();
            //    others_map = _examcontext.COE_Events_OthersDMO.Where(e => e.COEE_Id == id).ToList();
            //    page.edit_oth_mobilenos_list = others_map.ToArray();

            //    List<COE_Events_ImagesDMO> images_map = new List<COE_Events_ImagesDMO>();
            //    images_map = _examcontext.COE_Events_ImagesDMO.Where(e => e.COEE_Id == id).ToList();
            //    page.edit_images_list = images_map.ToArray();

            //    List<COE_Events_VideosDMO> videos_map = new List<COE_Events_VideosDMO>();
            //    videos_map = _examcontext.COE_Events_VideosDMO.Where(e => e.COEE_Id == id).ToList();
            //    page.edit_videos_list = videos_map.ToArray();

            //    //List<TT_Master_Staff_Abbreviation_DMO> lorg = new List<TT_Master_Staff_Abbreviation_DMO>();
            //    ////lorg = _ttcategorycontext.subAbbriList.AsNoTracking().Where(t => t.TTMSUAB_Id.Equals(id)).ToList();
            //    ////page.sujectslistedit = lorg.ToArray();

            //    //page.sujectslistedit = (from MasterStaff in _ttcategorycontext.Master_staff
            //    //                      from TT_Master_Staff_Abbreviation_DMO in _ttcategorycontext.staffAbbriList
            //    //                        where (MasterStaff.IVRMSTAUL_Id == TT_Master_Staff_Abbreviation_DMO.HRME_Id && TT_Master_Staff_Abbreviation_DMO.TTMSAB_Id == id)
            //    //                      select new TTexammastercategoryDTO
            //    //                      {
            //    //                          HRME_Id = MasterStaff.IVRMSTAUL_Id,
            //    //                          TTMSAB_Id = TT_Master_Staff_Abbreviation_DMO.TTMSAB_Id,
            //    //                          TTMSAB_Abbreviation = TT_Master_Staff_Abbreviation_DMO.TTMSAB_Abbreviation
            //    //                      }
            //    //                       ).ToArray();

            //    //List<TT_Master_Staff_AbbreviationDMO> lorg = new List<TT_Master_Staff_AbbreviationDMO>();
            //    //lorg = _ttcategorycontext.TT_Master_Staff_AbbreviationDMO.AsNoTracking().Where(t => t.TTMSAB_Id.Equals(id)).ToList();
            //    //page.sujectslistedit = lorg.ToArray();


            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return page;
        }
        public exammastercategoryDTO getpageedit1(int id)
        {
            exammastercategoryDTO page = new exammastercategoryDTO();
            try
            {
                List<Exm_Master_CategoryDMO> category_m = new List<Exm_Master_CategoryDMO>();
                category_m = _examcontext.Exm_Master_CategoryDMO.Where(e => e.EMCA_Id == id).ToList();
                page.edit_m_category = category_m.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public exammastercategoryDTO getpageedit2(exammastercategoryDTO page)
        {
            // exammastercategoryDTO page = new exammastercategoryDTO();
            try
            {
                List<Exm_Category_ClassDMO> cate_class_m = new List<Exm_Category_ClassDMO>();
                cate_class_m = _examcontext.Exm_Category_ClassDMO.Where(e => e.MI_Id == page.MI_Id && e.ASMAY_Id == page.ASMAY_Id && e.EMCA_Id == page.EMCA_Id && e.ASMCL_Id == page.ASMCL_Id).ToList();
                page.edit_category_class = cate_class_m.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public exammastercategoryDTO deleterec(int id)
        {
            exammastercategoryDTO page = new exammastercategoryDTO();
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
        public exammastercategoryDTO deactivate1(exammastercategoryDTO data)
        {
            data.already_cnt = false;
            Exm_Master_CategoryDMO pge = Mapper.Map<Exm_Master_CategoryDMO>(data);
            if (pge.EMCA_Id > 0)
            {
                var result = _examcontext.Exm_Master_CategoryDMO.Single(t => t.EMCA_Id == pge.EMCA_Id);
                if (result.EMCA_ActiveFlag == true)
                {
                    var Exm_Category_ClassDMO_cnt = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_Id == data.EMCA_Id).ToList();

                    var Exm_Yearly_CategoryDMO_cnt = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_Id == data.EMCA_Id).ToList();
                    if (Exm_Category_ClassDMO_cnt.Count == 0 && Exm_Yearly_CategoryDMO_cnt.Count == 0)
                    {
                        result.EMCA_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                    //  result.EMCA_ActiveFlag = false;
                }
                else
                {
                    result.EMCA_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                // result.UpdatedDate = DateTime.Now;
                //_examcontext.Update(result);
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
        public exammastercategoryDTO deactivate2(exammastercategoryDTO data)
        {
            data.already_cnt = false;
            Exm_Category_ClassDMO pge = Mapper.Map<Exm_Category_ClassDMO>(data);
            if (pge.ECAC_Id > 0)
            {
                var result = _examcontext.Exm_Category_ClassDMO.Single(t => t.ECAC_Id == pge.ECAC_Id);
                if (result.ECAC_ActiveFlag == true)
                {
                    // var Exm_Yearly_CategoryDMO_cnt = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_Id == result.EMCA_Id).ToList();
                    var Exm_Yearly_CategoryDMO_cnt = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_Id == result.EMCA_Id && t.ASMAY_Id == result.ASMAY_Id).ToList();
                    if (Exm_Yearly_CategoryDMO_cnt.Count == 0)
                    {
                        result.ECAC_ActiveFlag = false;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                    // result.ECAC_ActiveFlag = false;
                }
                else
                {
                    result.ECAC_ActiveFlag = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                // result.UpdatedDate = DateTime.Now;
                //_examcontext.Update(result);
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
        public exammastercategoryDTO delete_event_classes(int id)
        {
            exammastercategoryDTO pagert = new exammastercategoryDTO();
            ////  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            //try
            //{

            //    List<COE_Events_ClassesDMO> lorg = new List<COE_Events_ClassesDMO>();
            //    lorg = _examcontext.COE_Events_ClassesDMO.Where(t => t.COEE_Id == id).ToList();
            //    if (lorg.Any())
            //    {
            //        for (int i = 0; lorg.Count > i; i++)
            //        {
            //            _examcontext.Remove(lorg.ElementAt(i));
            //            var contactExists = _examcontext.SaveChanges();
            //            if (contactExists == 1)
            //            {
            //                pagert.returnval = true;
            //            }
            //            else
            //            {
            //                pagert.returnval = false;
            //            }
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return pagert;
        }
        public exammastercategoryDTO delete_event_emps(int id)
        {
            exammastercategoryDTO pagert = new exammastercategoryDTO();
            ////  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            //try
            //{

            //    List<COE_Events_EmployeesDMO> lorg = new List<COE_Events_EmployeesDMO>();
            //    lorg = _examcontext.COE_Events_EmployeesDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
            //    if (lorg.Any())
            //    {
            //        for (int i = 0; lorg.Count > i; i++)
            //        {
            //            _examcontext.Remove(lorg.ElementAt(i));
            //            var contactExists = _examcontext.SaveChanges();
            //            if (contactExists == 1)
            //            {
            //                pagert.returnval = true;
            //            }
            //            else
            //            {
            //                pagert.returnval = false;
            //            }
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return pagert;
        }
        public exammastercategoryDTO delete_event_othr_mobs(int id)
        {
            exammastercategoryDTO pagert = new exammastercategoryDTO();
            ////  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            //try
            //{

            //    List<COE_Events_OthersDMO> lorg = new List<COE_Events_OthersDMO>();
            //    lorg = _examcontext.COE_Events_OthersDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
            //    if (lorg.Any())
            //    {
            //        for (int i = 0; lorg.Count > i; i++)
            //        {
            //            _examcontext.Remove(lorg.ElementAt(i));
            //            var contactExists = _examcontext.SaveChanges();
            //            if (contactExists == 1)
            //            {
            //                pagert.returnval = true;
            //            }
            //            else
            //            {
            //                pagert.returnval = false;
            //            }
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return pagert;
        }
        public exammastercategoryDTO delete_event_images(int id)
        {
            exammastercategoryDTO pagert = new exammastercategoryDTO();
            ////  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            //try
            //{

            //    List<COE_Events_ImagesDMO> lorg = new List<COE_Events_ImagesDMO>();
            //    lorg = _examcontext.COE_Events_ImagesDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
            //    if (lorg.Any())
            //    {
            //        for (int i = 0; lorg.Count > i; i++)
            //        {
            //            _examcontext.Remove(lorg.ElementAt(i));
            //            var contactExists = _examcontext.SaveChanges();
            //            if (contactExists == 1)
            //            {
            //                pagert.returnval = true;
            //            }
            //            else
            //            {
            //                pagert.returnval = false;
            //            }
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return pagert;
        }
        public exammastercategoryDTO delete_event_videos(int id)
        {
            exammastercategoryDTO pagert = new exammastercategoryDTO();
            ////  TT_LABLIB_DetailsDTO page = new TT_LABLIB_DetailsDTO();
            //try
            //{

            //    List<COE_Events_VideosDMO> lorg = new List<COE_Events_VideosDMO>();
            //    lorg = _examcontext.COE_Events_VideosDMO.Where(t => t.COEE_Id.Equals(id)).ToList();
            //    if (lorg.Any())
            //    {
            //        for (int i = 0; lorg.Count > i; i++)
            //        {
            //            _examcontext.Remove(lorg.ElementAt(i));
            //            var contactExists = _examcontext.SaveChanges();
            //            if (contactExists == 1)
            //            {
            //                pagert.returnval = true;
            //            }
            //            else
            //            {
            //                pagert.returnval = false;
            //            }
            //        }
            //    }

            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}
            return pagert;
        }
        public exammastercategoryDTO Save_ReportCard_Format(exammastercategoryDTO data)
        {
            try
            {
                data.message = "";
                var getcurrent_yearorder = _examcontext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id).ToList();

                int? current_yearorder = getcurrent_yearorder.FirstOrDefault().ASMAY_Order;

                int? previous_yearorder = current_yearorder - 1;

                var getprevious_yearid = _examcontext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Order == previous_yearorder).ToList();

                long previous_yearid = getprevious_yearid.FirstOrDefault().ASMAY_Id;

                var checkprevious_formatdetails = _examcontext.EXM_ProgressCard_FormatsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == previous_yearid).ToList();

                if (checkprevious_formatdetails.Count > 0)
                {
                    var checkcurrentyear_formatdetails = _examcontext.EXM_ProgressCard_FormatsDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.ASMAY_Id == data.ASMAY_Id).ToList();

                    if (checkcurrentyear_formatdetails.Count > 0)
                    {
                        data.message = "Current";
                    }
                    else
                    {
                        var outputval = _examcontext.Database.ExecuteSqlCommand("EXM_ProgressCard_RecordsInsert @p0,@p1,@p2", data.MI_Id,
                            previous_yearid, data.ASMAY_Id);

                        if (outputval > 0)
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
                    data.message = "Previous";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public exammastercategoryDTO deactive_format(exammastercategoryDTO data)
        {
            try
            {
                data.returnval = false;

                var get_result = _examcontext.EXM_ProgressCard_FormatsDMO.Where(a => a.MI_Id == data.MI_Id && a.EPCFT_Id == data.EPCFT_Id).ToList();

                if (get_result.Count > 0)
                {
                    var result = _examcontext.EXM_ProgressCard_FormatsDMO.Single(a => a.MI_Id == data.MI_Id && a.EPCFT_Id == data.EPCFT_Id);
                    result.EPCFT_ActiveFlg = result.EPCFT_ActiveFlg == true ? false : true;
                    result.EPCFT_UpdateDate = DateTime.Now;
                    result.EPCFT_UpdatedBy = data.userId;
                    _examcontext.Update(result);
                    var i = _examcontext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
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
