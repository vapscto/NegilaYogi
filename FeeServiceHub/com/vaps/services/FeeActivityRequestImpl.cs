using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vaps.admission;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeActivityRequestImpl : interfaces.FeeActivityRequestInterface
    {
        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public FeeActivityRequestImpl(FeeGroupContext YearlyFeeGroupMappingContext)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
        }

        public Adm_Master_ActivitiesDTO deletedata(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                for (int i = 0; i < data.headnames.Length; i++)
                {
                    var result1 = _YearlyFeeGroupMappingContext.Adm_Student_Activities.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                    && t.ASACT_Id == data.headnames[i].ASA_Id).SingleOrDefault();
                    result1.ASA_ActiveFlg = false;
                    _YearlyFeeGroupMappingContext.Update(result1);
                }

                var retdata = _YearlyFeeGroupMappingContext.SaveChanges();
                if (retdata >= 1)
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

        public Adm_Master_ActivitiesDTO getdata(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                data.getstudata = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                   from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_Id == data.AMST_Id)
                                   select new Adm_Master_ActivitiesDTO
                                   {
                                       AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                        (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                        (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                       AMST_AdmNo = a.AMST_AdmNo,
                                       AMAY_RollNo = b.AMAY_RollNo,
                                       AMST_MobileNo = a.AMST_MobileNo,
                                       AMST_emailId = a.AMST_emailId
                                   }).ToArray();

                data.getstusaveddata = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                        from d in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                        from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                        from f in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                        where (a.AMST_Id == b.AMST_Id && a.AMST_Id == c.AMST_Id && c.AMA_Id == d.AMA_Id && d.FMG_Id == f.FMG_Id && d.FMH_Id == f.FMH_Id && e.FMH_Id == f.FMH_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_Id == data.AMST_Id && c.ASA_ActiveFlg == true && c.ASA_ApprovedFlg == false && d.AMA_ActiveFlg==true)
                                        select new Adm_Master_ActivitiesDTO
                                        {
                                            AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                             (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                             (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            AMA_Id = d.AMA_Id,
                                            ASA_Id = c.ASACT_Id,
                                            FMH_FeeName = e.FMH_FeeName,
                                            FMA_Amount = f.FMA_Amount
                                        }).Distinct().ToArray();

                var skipsaveddata = (from a in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                     where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.ASA_ActiveFlg == true && a.ASA_ApprovedFlg == false)
                                     select a.AMA_Id).ToList();

                data.fetheaddata = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                    from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                    from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                    from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                    where (a.FMG_Id == d.FMG_Id && a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.MI_Id == data.MI_Id && a.FMG_Id == d.FMG_Id && a.FMH_Id == d.FMH_Id && d.FTI_Id == e.FTI_Id && d.ASMAY_Id == data.ASMAY_Id && !skipsaveddata.Contains(a.AMA_Id) && d.FMA_Amount > 0 && c.FMH_ActiveFlag==true && b.FMG_ActiceFlag== true && a.AMA_ActiveFlg == true)
                                    select new Adm_Master_ActivitiesDTO
                                    {
                                        FMH_FeeName = c.FMH_FeeName,
                                        FMA_Amount = d.FMA_Amount,
                                        FTI_Name = e.FTI_Name,
                                        AMA_Id = a.AMA_Id
                                    }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Adm_Master_ActivitiesDTO loadapproval(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = allyear.Distinct().ToArray();

                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = (from a in _YearlyFeeGroupMappingContext.School_M_Class
                             from b in _YearlyFeeGroupMappingContext.Masterclasscategory
                             from c in _YearlyFeeGroupMappingContext.AcademicYear
                             where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true && c.Is_Active == true
                             && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                             select a).Distinct().OrderBy(a => a.ASMCL_Order).ToList();
                data.classlist = classlist.ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = sectionlist.ToArray();

                data.getstusaveddataheadview = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                        from d in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                        from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                        from f in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    
                                        where (a.AMST_Id == b.AMST_Id && a.AMST_Id == c.AMST_Id && c.AMA_Id == d.AMA_Id && d.FMG_Id == f.FMG_Id && d.FMH_Id == f.FMH_Id && e.FMH_Id == f.FMH_Id  && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1  && c.ASA_ActiveFlg == true && c.ASA_ApprovedFlg == true)
                                        select new Adm_Master_ActivitiesDTO
                                        {
                                            AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                             (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                             (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            AMA_Id = d.AMA_Id,
                                            ASA_Id = c.ASACT_Id,
                                            FMH_FeeName = e.FMH_FeeName,
                                            FMA_Amount = f.FMA_Amount,
                                            ASA_ApprovedFlg=c.ASA_ApprovedFlg,
                                            AMST_Id = a.AMST_Id,
                                        }).Distinct().ToArray();

                data.getstusaveddata = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                        from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                        from d in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                        from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                        from f in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                        from g in _YearlyFeeGroupMappingContext.School_M_Class
                                        from h in _YearlyFeeGroupMappingContext.school_M_Section
                                        where (a.AMST_Id == b.AMST_Id && a.AMST_Id == c.AMST_Id && c.AMA_Id == d.AMA_Id && d.FMG_Id == f.FMG_Id && d.FMH_Id == f.FMH_Id && e.FMH_Id == f.FMH_Id && b.ASMCL_Id == g.ASMCL_Id && b.ASMS_Id == h.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && c.ASA_ActiveFlg == true && c.ASA_ApprovedFlg == true)
                                        select new Adm_Master_ActivitiesDTO
                                        {
                                            AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                             (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                             (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                            AMST_AdmNo = a.AMST_AdmNo,
                                            AMAY_RollNo = b.AMAY_RollNo,
                                            AMST_MobileNo = a.AMST_MobileNo,
                                            AMST_emailId = a.AMST_emailId,
                                            ASA_ApprovedFlg = c.ASA_ApprovedFlg,
                                            AMST_Id = a.AMST_Id,
                                              ASMC_SectionName = h.ASMC_SectionName,
                                            ASMCL_ClassName = g.ASMCL_ClassName,
                                           // ASA_UpdatedDate=c.ASA_UpdatedDate
                                        }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Adm_Master_ActivitiesDTO savedata(Adm_Master_ActivitiesDTO data)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            try
            {
                var duplicatecheck = _YearlyFeeGroupMappingContext.Adm_Student_Activities.Where(d => d.AMST_Id == data.AMST_Id && d.ASA_ActiveFlg==true).ToList();
                if (duplicatecheck.Count() > 0)
                {
                    for (int i = 0; i < duplicatecheck.Count; i++)
                    {
                       // duplicatecheck[i].ASA_UpdatedBy = data.headnames[i].AMST_Id;
                        duplicatecheck[i].ASA_UpdatedDate = indianTime;
                        _YearlyFeeGroupMappingContext.Update(duplicatecheck[i]);
                    }
                    for (int i = 0; i < data.headnames.Length; i++)
                    {
                        Adm_Student_Activities admstu = new Adm_Student_Activities();
                        admstu.MI_Id = data.MI_Id;
                        admstu.ASMAY_Id = data.ASMAY_Id;
                        admstu.AMA_Id = data.headnames[i].AMA_Id;
                        admstu.AMST_Id = data.AMST_Id;
                        admstu.ASA_ApprovedFlg = false;
                        admstu.ASA_ApprovedBy = 0;
                        admstu.ASA_ActiveFlg = true;
                        admstu.ASA_CreatedBy = data.userid;
                        admstu.ASA_CreatedDate = indianTime;
                        admstu.ASA_UpdatedBy = data.headnames[i].AMST_Id;
                        admstu.ASA_UpdatedDate = indianTime;
                        _YearlyFeeGroupMappingContext.Add(admstu);
                    }
                    var retval = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (retval >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    for (int i = 0; i < data.headnames.Length; i++)
                    {
                        Adm_Student_Activities admstu = new Adm_Student_Activities();
                        admstu.MI_Id = data.MI_Id;
                        admstu.ASMAY_Id = data.ASMAY_Id;
                        admstu.AMA_Id = data.headnames[i].AMA_Id;
                        admstu.AMST_Id = data.AMST_Id;
                        admstu.ASA_ApprovedFlg = false;
                        admstu.ASA_ApprovedBy = 0;
                        admstu.ASA_ActiveFlg = true;
                        admstu.ASA_CreatedBy = data.userid;
                        admstu.ASA_CreatedDate = indianTime;
                        admstu.ASA_UpdatedBy = data.headnames[i].AMST_Id;
                        admstu.ASA_UpdatedDate = indianTime;
                        _YearlyFeeGroupMappingContext.Add(admstu);
                    }
                    var retval = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (retval >= 1)
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Adm_Master_ActivitiesDTO saveGroupdata(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                int data1 = 0;

                for (int i = 0; i < data.headnames.Length; i++)
                {
                    //var asaid = (from a in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                    //             from b in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                    //             where (a.AMA_Id==b.AMA_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id==data.ASMAY_Id && b.FMG_Id== data.headnames[i].FMG_Id && b.FMH_Id == data.headnames[i].FMH_Id && a.ASA_ActiveFlg==true && b.AMA_ActiveFlg==true)
                    //             select a.ASA_Id).Distinct().ToList();

                    var result1 = _YearlyFeeGroupMappingContext.Adm_Student_Activities.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id 
                    && t.ASACT_Id == data.headnames[i].ASA_Id).SingleOrDefault();

                    result1.ASA_ApprovedFlg = true;
                    result1.ASA_ApprovedBy = data.userid;
                    result1.ASA_UpdatedDate = indianTime;
                    result1.ASA_ApprovedBy = data.userid;
                    result1.ASA_UpdatedBy = data.userid;
                    _YearlyFeeGroupMappingContext.Update(result1);
                }

                var contactexisttransaction = 0;
                using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                        dbCtxTxn.Commit();

                        for (int j = 0; j < data.headnames.Length; j++)
                        {
                            var asaid = (from a in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                         from b in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                         where (a.AMA_Id == b.AMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASACT_Id == data.headnames[j].ASA_Id && a.ASA_ActiveFlg == true && b.AMA_ActiveFlg == true)
                                         select new Adm_Master_ActivitiesDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                             FMH_Id = b.FMH_Id
                                         }).Distinct().ToArray();

                            data1 = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Fee_Mapping_After_Approval_For_Activityfee @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.ASMAY_Id, data.headnames[j].AMST_Id, asaid[0].FMG_Id, asaid[0].FMH_Id, data.userid);
                        }

                        if (data1 < 1)
                        {
                            dbCtxTxn.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                if (contactexisttransaction >= 1)
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

        public Adm_Master_ActivitiesDTO searching(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                    switch (data.searchType)
                    {
                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                                from b in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.AMA_Id == b.AMA_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && d.ASMS_Id == f.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && (((c.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_MiddleName.ToUpper().Trim()) == true ? str : c.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMST_LastName.ToUpper().Trim()) == true ? str : c.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchtext) || c.AMST_LastName.ToUpper().StartsWith(data.searchtext)))
                                                select new Adm_Master_ActivitiesDTO
                                                {
                                                    AMST_Id = c.AMST_Id,
                                                    AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "0" ? "" : c.AMST_FirstName) +
                                        (c.AMST_MiddleName == null || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                        (c.AMST_LastName == null || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                                    ASMC_SectionName = f.ASMC_SectionName,
                                                    ASMCL_ClassName = e.ASMCL_ClassName,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    ASA_ApprovedFlg=b.ASA_ApprovedFlg,
                                                }
                  ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
                            break;
                        case "1":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                                from b in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.AMA_Id == b.AMA_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && e.ASMCL_ClassName.Equals(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id) 
                                                select new Adm_Master_ActivitiesDTO
                                                {
                                                    AMST_Id = c.AMST_Id,
                                                    AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "0" ? "" : c.AMST_FirstName) +
                                        (c.AMST_MiddleName == null || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                        (c.AMST_LastName == null || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                                    ASMC_SectionName = f.ASMC_SectionName,
                                                    ASMCL_ClassName = e.ASMCL_ClassName,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    ASA_ApprovedFlg = b.ASA_ApprovedFlg,
                                                }
                  ).Distinct().OrderBy(t => t.ASMCL_ClassName).ToArray();
                            break;
                        case "2":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                                from b in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.AMA_Id == b.AMA_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                                select new Adm_Master_ActivitiesDTO
                                                {
                                                    AMST_Id = c.AMST_Id,
                                                    AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "0" ? "" : c.AMST_FirstName) +
                                        (c.AMST_MiddleName == null || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                        (c.AMST_LastName == null || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                                    ASMC_SectionName = f.ASMC_SectionName,
                                                    ASMCL_ClassName = e.ASMCL_ClassName,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    ASA_ApprovedFlg = b.ASA_ApprovedFlg,

                                                }
                  ).Distinct().OrderBy(t => t.ASMC_SectionName).ToArray();
                            break;
                        case "3":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                                from b in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                where (a.AMA_Id == b.AMA_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                                select new Adm_Master_ActivitiesDTO
                                                {
                                                    AMST_Id = c.AMST_Id,
                                                    AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "0" ? "" : c.AMST_FirstName) +
                                        (c.AMST_MiddleName == null || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                        (c.AMST_LastName == null || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                                    ASMC_SectionName = f.ASMC_SectionName,
                                                    ASMCL_ClassName = e.ASMCL_ClassName,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    ASA_ApprovedFlg = b.ASA_ApprovedFlg,
                                                }
                  ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
                            break;
                        case "4":
                            var date_format = data.searchdate.ToString("dd/MM/yyyy");

                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                                from b in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                    // from g in list
                                                where (a.AMA_Id == b.AMA_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id &&  b.ASA_UpdatedDate.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy")) 
                                                select new Adm_Master_ActivitiesDTO
                                                {
                                                    AMST_Id = c.AMST_Id,
                                                    AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "0" ? "" : c.AMST_FirstName) +
                                        (c.AMST_MiddleName == null || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                        (c.AMST_LastName == null || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                                    ASMC_SectionName = f.ASMC_SectionName,
                                                    ASMCL_ClassName = e.ASMCL_ClassName,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    ASA_ApprovedFlg = b.ASA_ApprovedFlg,
                                                }
               ).Distinct().ToArray();

                            break;
                        case "5":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                                from b in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                                from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                                from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                                from e in _YearlyFeeGroupMappingContext.admissioncls
                                                from f in _YearlyFeeGroupMappingContext.school_M_Section
                                                from g in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                where (a.AMA_Id == b.AMA_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && g.FMH_Id==a.FMH_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && g.FMH_FeeName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
                                                select new Adm_Master_ActivitiesDTO
                                                {
                                                    AMST_Id = c.AMST_Id,
                                                    AMST_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "0" ? "" : c.AMST_FirstName) +
                                        (c.AMST_MiddleName == null || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                        (c.AMST_LastName == null || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                                    ASMC_SectionName = f.ASMC_SectionName,
                                                    ASMCL_ClassName = e.ASMCL_ClassName,
                                                    AMST_AdmNo = c.AMST_AdmNo,
                                                    ASA_ApprovedFlg = b.ASA_ApprovedFlg,
                                                }
                  ).Distinct().OrderBy(t => t.FMH_FeeName).ToArray();
                            break;
                    }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Adm_Master_ActivitiesDTO viewacaclslst(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = (from a in _YearlyFeeGroupMappingContext.School_M_Class
                             from b in _YearlyFeeGroupMappingContext.Masterclasscategory
                             from c in _YearlyFeeGroupMappingContext.AcademicYear
                             where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true && c.Is_Active == true
                             && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                             select a).Distinct().OrderBy(a => a.ASMCL_Order).ToList();
                data.classlist = classlist.ToArray();

                //var sectiondata = (from a in _YearlyFeeGroupMappingContext.school_M_Section
                //                   from b in _YearlyFeeGroupMappingContext.AdmSchoolMasterClassCatSec
                //                   from c in _YearlyFeeGroupMappingContext.Masterclasscategory
                //                   where (b.ASMCC_Id == c.ASMCC_Id && c.ASMAY_Id==data.ASMAY_Id && c.MI_Id==data.MI_Id)
                //                   select a
                //                ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();

                List<School_M_Section> sectionlist = new List<School_M_Section>();
                sectionlist = _YearlyFeeGroupMappingContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = sectionlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public Adm_Master_ActivitiesDTO viewstudentlist(Adm_Master_ActivitiesDTO data)
        {
            try
            {
                data.fetheadstudata = (from c in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                       from d in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                       from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                       from f in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                       from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                       where (c.AMA_Id == d.AMA_Id && d.FMG_Id == f.FMG_Id && d.FMH_Id == f.FMH_Id && e.FMH_Id == f.FMH_Id && c.AMST_Id==b.AMST_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && c.ASA_ActiveFlg == true && c.ASA_ApprovedFlg == false && b.ASMCL_Id==data.ASMCL_Id && b.ASMS_Id==data.ASMS_Id)
                                       select new Adm_Master_ActivitiesDTO
                                       {
                                           FMH_FeeName = e.FMH_FeeName,
                                           FMA_Amount = f.FMA_Amount,
                                           AMST_Id = c.AMST_Id,
                                           ASA_Id=c.ASACT_Id
                                       }).Distinct().OrderBy(t => t.ASA_CreatedDate).ToArray();

                data.fetheaddata = (from a in _YearlyFeeGroupMappingContext.Adm_M_Student
                                    from b in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                    from c in _YearlyFeeGroupMappingContext.Adm_Student_Activities
                                    from d in _YearlyFeeGroupMappingContext.Adm_Master_Activities
                                    from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                    from f in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_Id == c.AMST_Id && c.AMA_Id == d.AMA_Id && d.FMG_Id == f.FMG_Id && d.FMH_Id == f.FMH_Id && e.FMH_Id == f.FMH_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && c.ASA_ActiveFlg == true && c.ASA_ApprovedFlg == false && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && e.FMH_ActiveFlag == true)
                                    select new Adm_Master_ActivitiesDTO
                                    {
                                        AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                         (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                         (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        AMST_MobileNo = a.AMST_MobileNo,
                                        AMST_emailId = a.AMST_emailId,
                                        AMST_Id = a.AMST_Id,
                                        ASA_CreatedDate = DateTime.Now,
                                    }).Distinct().OrderBy(t => t.ASA_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}