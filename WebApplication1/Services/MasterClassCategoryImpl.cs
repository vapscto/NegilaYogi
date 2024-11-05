using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DomainModel;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using DomainModel.Model;
using AutoMapper;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vaps.admission;

namespace WebApplication1.Services
{
    public class MasterClassCategoryImpl : Interfaces.MasterClassCategoryInterface
    {
        private static ConcurrentDictionary<string, MasterClassCategoryDTO> _login =
           new ConcurrentDictionary<string, MasterClassCategoryDTO>();

        public DomainModelMsSqlServerContext _db;
        ILogger<MasterClassCategoryImpl> _log;
        public MasterClassCategoryImpl(DomainModelMsSqlServerContext db, ILogger<MasterClassCategoryImpl> log)
        {
            _db = db;
            _log = log;

        }
        public MasterClassCategoryDTO getDat(MasterClassCategoryDTO dto)
        {
            try
            {

                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _db.AcademicYear.Where(d => d.Is_Active == true && d.MI_Id == dto.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToList();
                dto.acdYearList = allacademic.ToArray();

                dto.currentYear = _db.AcademicYear.Where(d => d.Is_Active == true && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id).ToArray();


                List<MasterCategory> allcategory = new List<MasterCategory>();
                allcategory = _db.mastercategory.Where(d => d.AMC_ActiveFlag == 1 && d.MI_Id == dto.MI_Id).ToList();
                dto.categoryDrpDwn = allcategory.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _db.School_M_Class.Where(d => d.ASMCL_ActiveFlag == true && d.MI_Id == dto.MI_Id).OrderBy(d => d.ASMCL_Order).ToList();
                dto.classDrpDwn = allclass.ToArray();

                //Enhancement (Class Section Mapping). 16-08-2017 By Sripad Joshi.
                //Class & Section list should come based on this in all combo selection wherever required.
                //For this reason added Section dropdown here.
                dto.sectionList = _db.School_M_Section.Where(d => d.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1).OrderBy(d => d.ASMC_Order).ToArray();



                dto.classcategoryList = (from m in _db.Masterclasscategory
                                         from n in _db.mastercategory
                                         from o in _db.AcademicYear
                                         from p in _db.School_M_Class
                                         where (m.AMC_Id == n.AMC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id
                                         && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1)
                                         select new MasterClassCategoryDTO
                                         {
                                             ASMAY_Id = m.ASMAY_Id,
                                             AMC_Id = m.AMC_Id,
                                             ASMCL_Id = m.ASMCL_Id,
                                             ASMCC_Id = m.ASMCC_Id,
                                             Is_Active = m.Is_Active,
                                             Year = o.ASMAY_Year,
                                             categoryName = n.AMC_Name,
                                             className = p.ASMCL_ClassName
                                         }).OrderByDescending(d => d.ASMCC_Id).ToArray();
                if (dto.classcategoryList.Length > 0)
                {
                    dto.count = dto.classcategoryList.Length;
                }
                else
                {
                    dto.count = 0;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                _log.LogError(ee.Message);
                _log.LogDebug(ee.Message);
                _log.LogTrace(ee.Message);
            }

            return dto;
        }
        public MasterClassCategoryDTO saveData(MasterClassCategoryDTO data)
        {
            try
            {
                string msg = "";

                if (data.ASMCC_Id > 0)
                {
                    var ismapped = _db.Adm_M_Student.Where(d => d.MI_Id == data.MI_Id && d.AMC_Id == data.ASMCC_Id).ToList();
                    //&& d.ASMAY_Id == data.ASMAY_Id
                    if (ismapped.Count > 0)
                    {
                        data.message = "Sorry...You Can't Edit This Record. This Is Already Mapped With Student";
                    }
                    else
                    {
                        //var checkduplicates = _db.Masterclasscategory.Where(d => d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == Convert.ToInt64(data.ASMCL_Id) && d.MI_Id == data.MI_Id).ToList();
                        //if (checkduplicates.Count > 0)
                        //{
                        //    var classname = _db.School_M_Class.Where(d => d.ASMCL_Id == data.ASMCL_Id).ToList();
                        //    msg += " " + classname.FirstOrDefault().ASMCL_ClassName + " ,";
                        //    data.message = msg + " " + "Already Exist With Same Academic Year";
                        //}
                        //else
                        //{

                        var result = _db.Masterclasscategory.Single(t => t.ASMCC_Id == data.ASMCC_Id);

                        result.UpdatedDate = DateTime.Now;
                        result.ASMAY_Id = data.ASMAY_Id;
                        result.AMC_Id = data.AMC_Id;
                        result.AMC_Id = data.AMC_Id;
                        // result.ASMCL_Id = result.ASMCL_Id;
                        // result.ASMAY_Id = result.ASMAY_Id;
                        _db.Update(result);
                        int flag = _db.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnVal = true;
                            data.messagesaveupdate = "Update";
                        }
                        else
                        {
                            data.returnVal = false;
                            data.messagesaveupdate = "Update";
                        }
                        //}
                    }
                }
                else
                {
                    for (int i = 0; i < data.selectedClass.Length; i++)
                    {
                        Masterclasscategory MM = new Masterclasscategory();

                        var check_class = _db.Masterclasscategory.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.selectedClass[i].ASMCL_Id && a.Is_Active == true).ToList();

                        if (check_class.Count() > 0)
                        {
                            var check_class_result = _db.Masterclasscategory.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.selectedClass[i].ASMCL_Id && a.Is_Active == true);
                            MM.ASMCC_Id = check_class_result.ASMCC_Id;
                        }
                        else
                        {
                            MM.CreatedDate = DateTime.Now;
                            MM.Is_Active = true;
                            MM.UpdatedDate = DateTime.Now;
                            MM.ASMCL_Id = data.selectedClass[i].ASMCL_Id;
                            MM.MI_Id = data.MI_Id;
                            MM.AMC_Id = data.AMC_Id;
                            MM.ASMAY_Id = data.ASMAY_Id;
                            _db.Add(MM);
                        }



                        for (int j = 0; j < data.selectedSection.Length; j++)
                        {
                            var checkduplicates = (from m in _db.Masterclasscategory
                                                   from n in _db.AdmSchoolMasterClassCatSec
                                                   where m.ASMCC_Id == n.ASMCC_Id && m.ASMAY_Id == data.ASMAY_Id && m.ASMCL_Id == data.selectedClass[i].ASMCL_Id && m.MI_Id == data.MI_Id && n.ASMS_Id == data.selectedSection[j].ASMS_Id
                                                   select new MasterClassCategoryDTO
                                                   {
                                                       ASMCC_Id = m.ASMCC_Id,
                                                       ASMCCS_Id = n.ASMCCS_Id
                                                   }).ToList();
                            if (checkduplicates.Count > 0)
                            {
                                data.message = "Class Category Already  Exist";
                            }
                            else
                            {
                                AdmSchoolMasterClassCatSec obj = new AdmSchoolMasterClassCatSec();
                                obj.ASMCC_Id = MM.ASMCC_Id;
                                obj.ASMS_Id = data.selectedSection[j].ASMS_Id;
                                obj.ASMCCS_ActiveFlg = true;
                                _db.Add(obj);
                                int flag = _db.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnVal = true;
                                    data.messagesaveupdate = "Save";
                                }
                                else
                                {
                                    data.returnVal = false;
                                    data.messagesaveupdate = "Save";
                                }
                            }
                        }
                    }
                    // var checkduplicates = _db.Masterclasscategory.Where(d => d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == Convert.ToInt64(data.ASMCL_Id) && d.MI_Id == data.MI_Id).ToList();

                    //  if (checkduplicates.Count > 0)
                    //  {
                    // var classname = _db.School_M_Class.Where(d => d.ASMCL_Id == data.ASMCL_Id).ToList();
                    // var sectionname = _db.School_M_Section.Where(d => d.ASMS_Id == data.ASMS_Id).ToList();
                    // msg += "Class: " + classname.FirstOrDefault().ASMCL_ClassName + " and "+"Section: "+ sectionname.FirstOrDefault().ASMC_SectionName+" ,";
                    // data.message = msg + " " + "Already Exist With Same Academic Year";
                    //     data.message = "";
                    //  }
                    //else
                    //{
                    //    MM.CreatedDate = DateTime.Now;
                    //    MM.Is_Active = true;
                    //    MM.UpdatedDate = DateTime.Now;
                    //    MM.ASMCL_Id = data.ASMCL_Id;
                    //    _db.Add(MM);

                    //    AdmSchoolMasterClassCatSec obj = new AdmSchoolMasterClassCatSec();
                    //    obj.ASMCC_Id = MM.ASMCC_Id;
                    //    obj.ASMS_Id = data.ASMS_Id;
                    //    _db.Add(obj);
                    //    int flag = _db.SaveChanges();
                    //    if (flag > 0)
                    //    {
                    //        data.returnVal = true;
                    //        data.messagesaveupdate = "Save";
                    //    }
                    //    else
                    //    {
                    //        data.returnVal = false;
                    //        data.messagesaveupdate = "Save";
                    //    }
                    //}
                }

                data.classcategoryList = (from m in _db.Masterclasscategory
                                          from n in _db.mastercategory
                                          from o in _db.AcademicYear
                                          from p in _db.School_M_Class
                                          from q in _db.School_M_Section
                                          from r in _db.AdmSchoolMasterClassCatSec
                                          where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id && m.MI_Id == data.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1)
                                          select new MasterClassCategoryDTO
                                          {
                                              ASMAY_Id = m.ASMAY_Id,
                                              AMC_Id = m.AMC_Id,
                                              ASMCL_Id = m.ASMCL_Id,
                                              ASMCC_Id = m.ASMCC_Id,
                                              ASMCCS_Id = r.ASMCCS_Id,
                                              Is_Active = m.Is_Active,
                                              Year = o.ASMAY_Year,
                                              categoryName = n.AMC_Name,
                                              className = p.ASMCL_ClassName,
                                              sectionName = q.ASMC_SectionName
                                          }).OrderByDescending(d => d.CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                _log.LogDebug(ex.Message);
                _log.LogTrace(ex.Message);
            }
            return data;
        }
        public MasterClassCategoryDTO Edit(MasterClassCategoryDTO rel)
        {
            //  MasterClassCategoryDTO rel = new MasterClassCategoryDTO();
            try
            {

                rel.classcategoryList = (from m in _db.Masterclasscategory
                                         from n in _db.mastercategory
                                         from o in _db.AcademicYear
                                         from p in _db.School_M_Class
                                         from q in _db.School_M_Section
                                         from r in _db.AdmSchoolMasterClassCatSec
                                         where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id && m.MI_Id == rel.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && m.ASMCC_Id == rel.ASMCC_Id)
                                         select new MasterClassCategoryDTO
                                         {
                                             ASMAY_Id = m.ASMAY_Id,
                                             AMC_Id = m.AMC_Id,
                                             ASMCL_Id = m.ASMCL_Id,
                                             ASMCC_Id = m.ASMCC_Id,
                                             ASMCCS_Id = r.ASMCCS_Id,
                                             ASMS_Id = r.ASMS_Id,
                                             Is_Active = m.Is_Active,
                                             Year = o.ASMAY_Year,
                                             categoryName = n.AMC_Name,
                                             className = p.ASMCL_ClassName,
                                             sectionName = q.ASMC_SectionName
                                         }).ToArray();


                var getsavedsectionlist = (from m in _db.Masterclasscategory
                                           from n in _db.mastercategory
                                           from o in _db.AcademicYear
                                           from p in _db.School_M_Class
                                           from q in _db.School_M_Section
                                           from r in _db.AdmSchoolMasterClassCatSec
                                           where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id 
                                           && r.ASMS_Id == q.ASMS_Id && m.MI_Id == rel.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true 
                                           && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && m.ASMCC_Id == rel.ASMCC_Id)
                                           select new MasterClassCategoryDTO
                                           {
                                               ASMAY_Id = m.ASMAY_Id,
                                               AMC_Id = m.AMC_Id,
                                               ASMCL_Id = m.ASMCL_Id,
                                               ASMCC_Id = m.ASMCC_Id,
                                               ASMCCS_Id = r.ASMCCS_Id,
                                               ASMS_Id = r.ASMS_Id,
                                               Is_Active = m.Is_Active,
                                               Year = o.ASMAY_Year,
                                               categoryName = n.AMC_Name,
                                               className = p.ASMCL_ClassName,
                                               sectionName = q.ASMC_SectionName
                                           }).ToArray();

                rel.getsavedsectionlist = getsavedsectionlist.ToArray();


                //List<Masterclasscategory> relig = new List<Masterclasscategory>();
                //relig = _db.Masterclasscategory.Where(t => t.ASMCC_Id == rel.ASMCC_Id).ToList();
                //rel.classcategoryList = relig.ToArray();

                //List<AdmSchoolMasterClassCatSec> clsec = new List<AdmSchoolMasterClassCatSec>();
                //clsec = _db.AdmSchoolMasterClassCatSec.Where(t => t.ASMCCS_Id == rel.ASMCCS_Id).ToList();
                //rel.classsectionList = clsec.ToArray();

                //List<School_M_Class> allclass = new List<School_M_Class>();
                //allclass = _db.School_M_Class.Where(d => d.ASMCL_Id == relig.FirstOrDefault().ASMCL_Id).ToList();
                //rel.classDrpDwn = allclass.ToArray();

                //List<School_M_Section> section = new List<School_M_Section>();
                //section = _db.School_M_Section.Where(d => d.ASMS_Id == clsec.FirstOrDefault().ASMS_Id).ToList();
                //rel.sectionList = allclass.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                _log.LogDebug(ex.Message);
                _log.LogTrace(ex.Message);
            }
            return rel;
        }
        public MasterClassCategoryDTO deleterec(int id)
        {
            MasterClassCategoryDTO org = new MasterClassCategoryDTO();
            List<Masterclasscategory> lorg = new List<Masterclasscategory>(); // Mapper.Map<Organisation>(org);

            try
            {
                lorg = _db.Masterclasscategory.Where(t => t.ASMCC_Id == id).ToList();

                if (lorg.Any())
                {
                    _db.Remove(lorg.ElementAt(0));
                    var flag = _db.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnVal = true;
                    }
                    else
                    {
                        org.returnVal = false;
                    }
                }
                org.classcategoryList = (from m in _db.Masterclasscategory
                                         from n in _db.mastercategory
                                         from o in _db.AcademicYear
                                         from p in _db.School_M_Class
                                         where (m.AMC_Id == n.AMC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && m.MI_Id == lorg.FirstOrDefault().MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1)
                                         select new MasterClassCategoryDTO
                                         {
                                             ASMAY_Id = m.ASMAY_Id,
                                             AMC_Id = m.AMC_Id,
                                             ASMCL_Id = m.ASMCL_Id,
                                             ASMCC_Id = m.ASMCC_Id,
                                             Is_Active = m.Is_Active,
                                             Year = o.ASMAY_Year,
                                             categoryName = n.AMC_Name,
                                             className = p.ASMCL_ClassName
                                         }).OrderByDescending(d => d.CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                org.message = "You Can Not Delete This Record. It Is Already Mapped With Student";
                _log.LogError(ex.Message);
                _log.LogDebug(ex.Message);
                _log.LogTrace(ex.Message);
            }

            return org;
        }
        public MasterClassCategoryDTO deactivate(MasterClassCategoryDTO dto)
        {
            try
            {

                Masterclasscategory enq = Mapper.Map<Masterclasscategory>(dto);

                var check_classcategory_used = _db.Adm_M_Student.Where(t => t.AMC_Id == enq.ASMCC_Id).ToList();
                if (check_classcategory_used.Count == 0)
                {
                    if (enq.ASMCC_Id > 0)
                    {
                        var result = _db.Masterclasscategory.Single(t => t.ASMCC_Id == enq.ASMCC_Id);
                        if (result.Is_Active == true)
                        {
                            result.Is_Active = false;
                        }
                        else
                        {
                            result.Is_Active = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        _db.Update(result);

                        var getsectionlist = _db.AdmSchoolMasterClassCatSec.Where(a => a.ASMCC_Id == enq.ASMCC_Id).ToList();
                        if (getsectionlist.Count() > 0)
                        {
                            for(int k=0; k< getsectionlist.Count; k++)
                            {
                                var resultnew = _db.AdmSchoolMasterClassCatSec.Single(a => a.ASMCC_Id == enq.ASMCC_Id && a.ASMCCS_Id == getsectionlist[k].ASMCCS_Id);
                                if (resultnew.ASMCCS_ActiveFlg == true)
                                {
                                    resultnew.ASMCCS_ActiveFlg = false;
                                }
                                else
                                {
                                    resultnew.ASMCCS_ActiveFlg = true;
                                }

                                _db.Update(resultnew);
                            }
                        }

                        var flag = _db.SaveChanges();
                        if (flag >0)
                        {
                            dto.returnVal = true;
                        }
                        else
                        {
                            dto.returnVal = false;
                        }

                        dto.classcategoryList = (from m in _db.Masterclasscategory
                                                 from n in _db.mastercategory
                                                 from o in _db.AcademicYear
                                                 from p in _db.School_M_Class
                                                 from q in _db.School_M_Section
                                                 from r in _db.AdmSchoolMasterClassCatSec
                                                 where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id
                                                 && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1)
                                                 select new MasterClassCategoryDTO
                                                 {
                                                     ASMAY_Id = m.ASMAY_Id,
                                                     AMC_Id = m.AMC_Id,
                                                     ASMCL_Id = m.ASMCL_Id,
                                                     ASMCC_Id = m.ASMCC_Id,
                                                     ASMCCS_Id = r.ASMCCS_Id,
                                                     Is_Active = m.Is_Active,
                                                     Year = o.ASMAY_Year,
                                                     categoryName = n.AMC_Name,
                                                     className = p.ASMCL_ClassName,
                                                     sectionName = q.ASMC_SectionName
                                                 }).OrderByDescending(d => d.CreatedDate).ToArray();
                    }
                }
                else
                {
                    dto.msgdeactive = "Deactive";
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                _log.LogDebug(ex.Message);
                _log.LogTrace(ex.Message);
            }
            return dto;
        }
        public MasterClassCategoryDTO searchByColumn(MasterClassCategoryDTO dto)
        {
            try
            {
                if (dto.SearchColumn == "1")
                {
                    dto.classcategoryList = (from m in _db.Masterclasscategory
                                             from n in _db.mastercategory
                                             from o in _db.AcademicYear
                                             from p in _db.School_M_Class
                                             from q in _db.School_M_Section
                                             from r in _db.AdmSchoolMasterClassCatSec
                                             where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id
                                             && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && n.AMC_Name.Contains(dto.Input))
                                             select new MasterClassCategoryDTO
                                             {
                                                 ASMAY_Id = m.ASMAY_Id,
                                                 AMC_Id = m.AMC_Id,
                                                 ASMCL_Id = m.ASMCL_Id,
                                                 ASMCC_Id = m.ASMCC_Id,
                                                 ASMCCS_Id = r.ASMCCS_Id,
                                                 Is_Active = m.Is_Active,
                                                 Year = o.ASMAY_Year,
                                                 categoryName = n.AMC_Name,
                                                 className = p.ASMCL_ClassName,
                                                 sectionName = q.ASMC_SectionName
                                             }).OrderByDescending(d => d.CreatedDate).ToArray();
                    if (dto.classcategoryList.Length > 0)
                    {
                        dto.count = dto.classcategoryList.Length;
                    }
                    else
                    {
                        dto.count = 0;
                    }
                }
                else if (dto.SearchColumn == "2")
                {
                    try
                    {
                        dto.classcategoryList = (from m in _db.Masterclasscategory
                                                 from n in _db.mastercategory
                                                 from o in _db.AcademicYear
                                                 from p in _db.School_M_Class
                                                 from q in _db.School_M_Section
                                                 from r in _db.AdmSchoolMasterClassCatSec
                                                 where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id
                                                 && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && o.ASMAY_Year.Contains(dto.Input))
                                                 select new MasterClassCategoryDTO
                                                 {
                                                     ASMAY_Id = m.ASMAY_Id,
                                                     AMC_Id = m.AMC_Id,
                                                     ASMCL_Id = m.ASMCL_Id,
                                                     ASMCC_Id = m.ASMCC_Id,
                                                     ASMCCS_Id = r.ASMCCS_Id,
                                                     Is_Active = m.Is_Active,
                                                     Year = o.ASMAY_Year,
                                                     categoryName = n.AMC_Name,
                                                     className = p.ASMCL_ClassName,
                                                     sectionName = q.ASMC_SectionName
                                                 }).OrderByDescending(d => d.CreatedDate).ToArray();
                        if (dto.classcategoryList.Length > 0)
                        {
                            dto.count = dto.classcategoryList.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex.Message);
                        _log.LogDebug(ex.Message);
                        _log.LogTrace(ex.Message);
                    }

                }
                else if (dto.SearchColumn == "3")
                {
                    try
                    {
                        dto.classcategoryList = (from m in _db.Masterclasscategory
                                                 from n in _db.mastercategory
                                                 from o in _db.AcademicYear
                                                 from p in _db.School_M_Class
                                                 from q in _db.School_M_Section
                                                 from r in _db.AdmSchoolMasterClassCatSec
                                                 where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id
                                                 && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && p.ASMCL_ClassName.Contains(dto.Input))
                                                 select new MasterClassCategoryDTO
                                                 {
                                                     ASMAY_Id = m.ASMAY_Id,
                                                     AMC_Id = m.AMC_Id,
                                                     ASMCL_Id = m.ASMCL_Id,
                                                     ASMCC_Id = m.ASMCC_Id,
                                                     ASMCCS_Id = r.ASMCCS_Id,
                                                     Is_Active = m.Is_Active,
                                                     Year = o.ASMAY_Year,
                                                     categoryName = n.AMC_Name,
                                                     className = p.ASMCL_ClassName,
                                                     sectionName = q.ASMC_SectionName
                                                 }).OrderByDescending(d => d.CreatedDate).ToArray();
                        if (dto.classcategoryList.Length > 0)
                        {
                            dto.count = dto.classcategoryList.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex.Message);
                        _log.LogDebug(ex.Message);
                        _log.LogTrace(ex.Message);
                    }

                }
                else if (dto.SearchColumn == "4")
                {
                    try
                    {
                        dto.classcategoryList = (from m in _db.Masterclasscategory
                                                 from n in _db.mastercategory
                                                 from o in _db.AcademicYear
                                                 from p in _db.School_M_Class
                                                 from q in _db.School_M_Section
                                                 from r in _db.AdmSchoolMasterClassCatSec
                                                 where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id
                                                 && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && q.ASMC_SectionName.Contains(dto.Input))
                                                 select new MasterClassCategoryDTO
                                                 {
                                                     ASMAY_Id = m.ASMAY_Id,
                                                     AMC_Id = m.AMC_Id,
                                                     ASMCL_Id = m.ASMCL_Id,
                                                     ASMCC_Id = m.ASMCC_Id,
                                                     ASMCCS_Id = r.ASMCCS_Id,
                                                     Is_Active = m.Is_Active,
                                                     Year = o.ASMAY_Year,
                                                     categoryName = n.AMC_Name,
                                                     className = p.ASMCL_ClassName,
                                                     sectionName = q.ASMC_SectionName
                                                 }).OrderByDescending(d => d.CreatedDate).ToArray();
                        if (dto.classcategoryList.Length > 0)
                        {
                            dto.count = dto.classcategoryList.Length;
                        }
                        else
                        {
                            dto.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex.Message);
                        _log.LogDebug(ex.Message);
                        _log.LogTrace(ex.Message);
                    }

                }
                else
                {
                    dto.classcategoryList = (from m in _db.Masterclasscategory
                                             from n in _db.mastercategory
                                             from o in _db.AcademicYear
                                             from p in _db.School_M_Class
                                             from q in _db.School_M_Section
                                             from r in _db.AdmSchoolMasterClassCatSec
                                             where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id && r.ASMS_Id == q.ASMS_Id
                                             && m.MI_Id == dto.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1)
                                             select new MasterClassCategoryDTO
                                             {
                                                 ASMAY_Id = m.ASMAY_Id,
                                                 AMC_Id = m.AMC_Id,
                                                 ASMCL_Id = m.ASMCL_Id,
                                                 ASMCC_Id = m.ASMCC_Id,
                                                 ASMCCS_Id = r.ASMCCS_Id,
                                                 Is_Active = m.Is_Active,
                                                 Year = o.ASMAY_Year,
                                                 categoryName = n.AMC_Name,
                                                 className = p.ASMCL_ClassName,
                                                 sectionName = q.ASMC_SectionName
                                             }).OrderByDescending(d => d.CreatedDate).ToArray();
                    if (dto.classcategoryList.Length > 0)
                    {
                        dto.count = dto.classcategoryList.Length;
                    }
                    else
                    {
                        dto.count = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                _log.LogDebug(ex.Message);
                _log.LogTrace(ex.Message);
            }
            return dto;
        }

        public MasterClassCategoryDTO viewrecordspopup(MasterClassCategoryDTO data)
        {
            try
            {
                data.viewsectionlist = (from m in _db.Masterclasscategory
                                        from n in _db.mastercategory
                                        from o in _db.AcademicYear
                                        from p in _db.School_M_Class
                                        from q in _db.School_M_Section
                                        from r in _db.AdmSchoolMasterClassCatSec
                                        where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id
                                        && r.ASMS_Id == q.ASMS_Id && m.MI_Id == data.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true
                                        && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && m.ASMCC_Id == data.ASMCC_Id && m.ASMAY_Id == data.ASMAY_Id
                                        && m.ASMCL_Id == data.ASMCL_Id)
                                        select new MasterClassCategoryDTO
                                        {
                                            ASMCCS_Id = r.ASMCCS_Id,
                                            ASMCC_Id = r.ASMCC_Id,
                                            ASMCCS_ActiveFlg = r.ASMCCS_ActiveFlg,
                                            Year = o.ASMAY_Year,
                                            ASMAY_Id=m.ASMAY_Id,
                                            ASMCL_Id=m.ASMCL_Id,
                                            ASMS_Id=r.ASMS_Id,
                                            categoryName = n.AMC_Name,
                                            className = p.ASMCL_ClassName,
                                            sectionName = q.ASMC_SectionName,
                                            ASMC_order = q.ASMC_Order
                                        }).Distinct().OrderBy(a => a.ASMC_order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterClassCategoryDTO deactivesection(MasterClassCategoryDTO data)
        {
            try
            {

                var checklist = (from a in _db.School_Adm_Y_StudentDMO
                                 from b in _db.AdmSchoolMasterClassCatSec
                                 from c in _db.Masterclasscategory
                                 where (c.ASMCC_Id == b.ASMCC_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && c.ASMAY_Id == data.ASMAY_Id
                                 && a.ASMS_Id == data.ASMS_Id && b.ASMCCS_Id == data.ASMCCS_Id && b.ASMCCS_ActiveFlg == true && a.ASMCL_Id==data.ASMCL_Id)
                                 select new MasterClassCategoryDTO
                                 {
                                     ASMS_Id = a.ASMS_Id
                                 }).ToList();

                // var checksectionused = _db.School_Adm_Y_StudentDMO.Where(a => a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id).ToList();

                if (checklist.Count() > 0)
                {
                    data.msgdeactive = "Deactive";
                }
                else
                {
                    var result = _db.AdmSchoolMasterClassCatSec.Single(a => a.ASMCCS_Id == data.ASMCCS_Id);
                    if (result.ASMCCS_ActiveFlg == true)
                    {
                        result.ASMCCS_ActiveFlg = false;
                    }
                    else
                    {
                        result.ASMCCS_ActiveFlg = true;
                    }
                    _db.Update(result);
                    int i = _db.SaveChanges();
                    if (i > 0)
                    {
                        data.returnVal = true;
                    }
                    else
                    {
                        data.returnVal = false;
                    }
                }

                data.viewsectionlist = (from m in _db.Masterclasscategory
                                        from n in _db.mastercategory
                                        from o in _db.AcademicYear
                                        from p in _db.School_M_Class
                                        from q in _db.School_M_Section
                                        from r in _db.AdmSchoolMasterClassCatSec
                                        where (m.AMC_Id == n.AMC_Id && m.ASMCC_Id == r.ASMCC_Id && m.ASMAY_Id == o.ASMAY_Id && m.ASMCL_Id == p.ASMCL_Id
                                        && r.ASMS_Id == q.ASMS_Id && m.MI_Id == data.MI_Id && o.Is_Active == true && p.ASMCL_ActiveFlag == true
                                        && n.AMC_ActiveFlag == 1 && q.ASMC_ActiveFlag == 1 && m.ASMCC_Id == data.ASMCC_Id)
                                        select new MasterClassCategoryDTO
                                        {
                                            ASMCCS_Id = r.ASMCCS_Id,
                                            ASMCC_Id = r.ASMCC_Id,
                                            ASMCCS_ActiveFlg = r.ASMCCS_ActiveFlg,
                                            ASMAY_Id = m.ASMAY_Id,
                                            ASMCL_Id = m.ASMCL_Id,
                                            ASMS_Id = r.ASMS_Id,
                                            Year = o.ASMAY_Year,
                                            categoryName = n.AMC_Name,
                                            className = p.ASMCL_ClassName,
                                            sectionName = q.ASMC_SectionName,
                                            ASMC_order = q.ASMC_Order
                                        }).Distinct().OrderBy(a => a.ASMC_order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnVal = false;
            }
            return data;
        }


    }
}
