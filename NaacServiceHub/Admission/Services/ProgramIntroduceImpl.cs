using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using Microsoft.EntityFrameworkCore;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class ProgramIntroduceImpl : Interface.ProgramIntroduceInterface
    {
        public GeneralContext _GeneralContext;
        public ProgramIntroduceImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<NAAC_AC_Programs_112_DTO> loaddata(NAAC_AC_Programs_112_DTO data)
        {
            try
            {

                var institutionlist = (from a in _GeneralContext.Institution
                                       from b in _GeneralContext.UserRoleWithInstituteDMO
                                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                                       select a).Distinct().OrderBy(t => t.MI_Name).ToList();
                data.institutionlist = institutionlist.ToArray();
                if (data.MI_Id == 0)
                {
                    if (institutionlist.Count > 0)
                    {
                        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    }
                }

                if (institutionlist.Count > 0)
                {
                    data.InstitutionTypeFlg = institutionlist.FirstOrDefault().MI_NAAC_InstitutionTypeFlg;
                    data.MI_SchoolCollegeFlag = institutionlist.FirstOrDefault().MI_SchoolCollegeFlag;

                }

                data.MasterCourseList = (from a in _GeneralContext.MasterCourseDMO
                                         from b in _GeneralContext.ClgMasterBranchDMO
                                         where (a.MI_Id == Convert.ToInt64(data.MI_Id) && a.MI_Id == b.MI_Id && a.AMCO_ActiveFlag == true && b.AMB_ActiveFlag == true)
                                         select new NAAC_AC_Programs_112_DTO
                                         {
                                             AMCO_Id = a.AMCO_Id,
                                             AMCO_CourseName = a.AMCO_CourseName + ":" + b.AMB_BranchName,
                                         }).Distinct().ToArray();

                data.alldata = (from a in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                from b in _GeneralContext.MasterCourseDMO
                                from c in _GeneralContext.ClgMasterBranchDMO
                                from y in _GeneralContext.Academic
                                where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.NCACMPR112_IntroYear == y.ASMAY_Id
                                && b.AMCO_Id == a.AMCO_Id && c.AMB_Id == a.AMB_Id && b.AMCO_ActiveFlag == true && c.AMB_ActiveFlag == true && y.Is_Active == true)
                                select new NAAC_AC_Programs_112_DTO
                                {
                                    NCACMPR112_Id = a.NCACMPR112_Id,
                                    NCACMPR112_IntroYear = a.NCACMPR112_IntroYear,
                                    NCACPMR112_DiscontinuedYear = a.NCACPMR112_DiscontinuedYear,
                                    NCACMPR112_DiplomaCertName = a.NCACMPR112_DiplomaCertName,
                                    AMCO_Id = a.AMCO_Id,
                                    AMB_Id = a.AMB_Id,
                                    MI_Id = a.MI_Id,
                                    NCACMPR112_DiscontinuedFlg = a.NCACMPR112_DiscontinuedFlg,
                                    NCACMPR112_ActiveFlg = a.NCACMPR112_ActiveFlg,
                                    AMCO_CourseName = b.AMCO_CourseName,
                                    AMCO_CourseCode = b.AMCO_CourseCode,
                                    AMB_BranchName = c.AMB_BranchName,
                                    AMB_BranchCode = c.AMB_BranchCode,
                                    discontinuedname = (_GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == a.NCACPMR112_DiscontinuedYear).FirstOrDefault().ASMAY_Year),
                                    ASMAY_Year = y.ASMAY_Year,

                                }).Distinct().OrderBy(t => t.NCACMPR112_Id).ToArray();

                var prgm = (from a in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                            where (a.MI_Id == data.MI_Id && a.NCACMPR112_ActiveFlg == true)
                            select new NAAC_AC_Programs_112_DTO
                            {
                                NCACMPR112_Id = a.NCACMPR112_Id,
                                NCACMPR112_DiplomaCertName = a.NCACMPR112_DiplomaCertName,
                                AMCO_Id = a.AMCO_Id,
                                AMB_Id = a.AMB_Id
                            }).Distinct().ToList();
                if (prgm.Count > 0)
                {
                    data.programlist = prgm.ToArray();
                }

                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new NAAC_AC_Programs_112_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.discontinuedyearlist = (from a in _GeneralContext.Academic
                                             where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                             select new NAAC_AC_Programs_112_DTO
                                             {
                                                 discontinuedid = a.ASMAY_Id,
                                                 discontinuedname = a.ASMAY_Year,
                                             }).Distinct().ToArray();
                data.mappedlistdata = (from a in _GeneralContext.NAAC_AC_Programs_112_DMO
                                       from b in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                       from y in _GeneralContext.Academic
                                       from curs in _GeneralContext.MasterCourseDMO
                                       from brnch in _GeneralContext.ClgMasterBranchDMO
                                       where (a.MI_Id == data.MI_Id && b.NCACMPR112_Id == a.NCACMPR112_Id && a.NCACPR112_Year == y.ASMAY_Id && a.AMCO_Id == curs.AMCO_Id && a.AMB_Id == brnch.AMB_Id)
                                       select new NAAC_AC_Programs_112_DTO
                                       {
                                           NCACPR112_Id = a.NCACPR112_Id,
                                           AMCO_CourseName = curs.AMCO_CourseName,
                                           AMB_BranchName = brnch.AMB_BranchName,
                                           NCACPR112_ActiveFlg = a.NCACPR112_ActiveFlg,
                                           NCACPR112_Date = a.NCACPR112_Date,
                                           ASMAY_Year = y.ASMAY_Year,
                                           NCACMPR112_DiplomaCertName = b.NCACMPR112_DiplomaCertName,
                                           NCACPR112_ApprovedFlg = a.NCACPR112_ApprovedFlg,
                                           NCACPR112_StatusFlg = a.NCACPR112_StatusFlg,
                                           MI_Id = a.MI_Id,
                                       }).Distinct().ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO savedata(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.AMCO_Id = (from t in _GeneralContext.Adm_Course_Branch_MappingDMO
                                where (t.AMB_Id == data.AMB_Id && t.MI_Id == data.MI_Id)
                                select t).Single().AMCO_Id;

                if (data.NCACMPR112_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.NCACMPR112_IntroYear == data.NCACMPR112_IntroYear).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_Master_Programs_112_DMO obj = new NAAC_AC_Master_Programs_112_DMO();
                        // obj.NCACMPR112_Id = data.NCACMPR112_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.AMCO_Id = data.AMCO_Id;
                        obj.AMB_Id = data.AMB_Id;
                        obj.NCACMPR112_DiplomaCertName = data.NCACMPR112_DiplomaCertName;
                        obj.NCACMPR112_IntroYear = data.NCACMPR112_IntroYear;
                        obj.NCACMPR112_ActiveFlg = true;
                        obj.NCACMPR112_CreatedBy = data.UserId;
                        obj.NCACMPR112_UpdatedBy = data.UserId;
                        obj.NCACMPR112_CreatedDate = DateTime.Now;
                        obj.NCACMPR112_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj);
                        int s = _GeneralContext.SaveChanges();
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
                else if (data.NCACMPR112_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Where(t => t.NCACMPR112_Id != data.NCACMPR112_Id && t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.NCACMPR112_IntroYear == data.NCACMPR112_IntroYear).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Single(t => t.NCACMPR112_Id == data.NCACMPR112_Id);
                        update.MI_Id = data.MI_Id;
                        update.AMCO_Id = data.AMCO_Id;
                        update.AMB_Id = data.AMB_Id;
                        update.NCACMPR112_DiplomaCertName = data.NCACMPR112_DiplomaCertName;
                        update.NCACMPR112_IntroYear = data.NCACMPR112_IntroYear;
                        update.NCACMPR112_ActiveFlg = true;
                        update.NCACMPR112_CreatedBy = data.UserId;
                        update.NCACMPR112_UpdatedBy = data.UserId;
                        update.NCACMPR112_CreatedDate = DateTime.Now;
                        update.NCACMPR112_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update);
                        int s = _GeneralContext.SaveChanges();
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
                data.alldata = (from a in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                from b in _GeneralContext.MasterCourseDMO
                                from c in _GeneralContext.ClgMasterBranchDMO
                                where (a.MI_Id == b.MI_Id && b.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && b.AMCO_ActiveFlag == true && c.AMB_ActiveFlag == true)
                                select new NAAC_AC_Programs_112_DTO
                                {
                                    NCACMPR112_Id = a.NCACMPR112_Id,
                                    NCACMPR112_IntroYear = a.NCACMPR112_IntroYear,
                                    ASMAY_Id = a.NCACPMR112_DiscontinuedYear,
                                    NCACMPR112_DiplomaCertName = a.NCACMPR112_DiplomaCertName,
                                    AMCO_Id = a.AMCO_Id,
                                    AMB_Id = a.AMB_Id,
                                    NCACMPR112_DiscontinuedFlg = a.NCACMPR112_DiscontinuedFlg,
                                    NCACMPR112_ActiveFlg = a.NCACMPR112_ActiveFlg,
                                    AMCO_CourseName = b.AMCO_CourseName,
                                    AMCO_CourseCode = b.AMCO_CourseCode,
                                    AMB_BranchName = c.AMB_BranchName,
                                    AMB_BranchCode = c.AMB_BranchCode
                                }).Distinct().OrderBy(t => t.NCACMPR112_Id).ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO editdata(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Where(t => t.NCACMPR112_Id == data.NCACMPR112_Id).ToList();
                data.editlist = edit.ToArray();

                //data.yearlist = (from a in _GeneralContext.Academic
                //                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                //                 select new NAAC_AC_Programs_112_DTO
                //                 {
                //                     ASMAY_Id = a.ASMAY_Id,
                //                     ASMAY_Year = a.ASMAY_Year,
                //                     ASMAY_From_Date = a.ASMAY_From_Date,
                //                     ASMAY_To_Date = a.ASMAY_To_Date,
                //                 }).Distinct().ToArray();
                //data.discontinuedyearlist = (from a in _GeneralContext.Academic
                //                             where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                //                             select new NAAC_AC_Programs_112_DTO
                //                             {
                //                                 discontinuedid = a.ASMAY_Id,
                //                                 discontinuedname = a.ASMAY_Year,
                //                             }).Distinct().ToArray();

                //data.masterbranchList = (from a in _GeneralContext.Adm_Course_Branch_MappingDMO
                //                         from b in _GeneralContext.ClgMasterBranchDMO
                //                         where (a.AMCO_Id == data.AMCO_Id && a.MI_Id == data.MI_Id && a.AMB_Id == b.AMB_Id)
                //                         select new NAAC_AC_Programs_112_DTO
                //                         {
                //                             AMB_Id = b.AMB_Id,
                //                             AMB_BranchName = b.AMB_BranchName
                //                         }).Distinct().ToArray();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO deactivY(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Where(t => t.NCACMPR112_Id == data.NCACMPR112_Id).SingleOrDefault();
                if (result.NCACMPR112_ActiveFlg == true)
                {
                    result.NCACMPR112_ActiveFlg = false;
                }
                else if (result.NCACMPR112_ActiveFlg == false)
                {
                    result.NCACMPR112_ActiveFlg = true;
                }
                result.NCACMPR112_UpdatedDate = DateTime.Now;
                result.NCACMPR112_UpdatedBy = data.UserId;
                _GeneralContext.Update(result);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO get_Discontinuedflagdata(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                var mappedProgram = (from a in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                     where (a.MI_Id == data.MI_Id && a.NCACMPR112_Id == data.NCACMPR112_Id)
                                     select new NAAC_AC_Programs_112_DTO
                                     {
                                         NCACMPR112_Id = a.NCACMPR112_Id,
                                         MI_Id = a.MI_Id,
                                         AMCO_Id = a.AMCO_Id,
                                         AMB_Id = a.AMB_Id,
                                         NCACMPR112_DiplomaCertName = a.NCACMPR112_DiplomaCertName,
                                         NCACMPR112_IntroYear = a.NCACMPR112_IntroYear,
                                         NCACMPR112_DiscontinuedFlg = a.NCACMPR112_DiscontinuedFlg,
                                         NCACPMR112_DiscontinuedYear = a.NCACPMR112_DiscontinuedYear,
                                         NCACMPR112_DiscontinuedReason = a.NCACMPR112_DiscontinuedReason,
                                         NCACMPR112_ActiveFlg = a.NCACMPR112_ActiveFlg,
                                     }).Distinct().ToList();

                data.mappedProgram = mappedProgram.ToArray();
                long s = 0;
                if (mappedProgram.Count > 0)
                {
                    s = mappedProgram.SingleOrDefault().NCACMPR112_IntroYear;
                }

                int year_order = (from a in _GeneralContext.Academic
                                  where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == s)
                                  select a.ASMAY_Order).SingleOrDefault();

                data.discontinuedyearlist = (from a in _GeneralContext.Academic
                                             where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Order >= year_order)
                                             select new NAAC_AC_Programs_112_DTO
                                             {
                                                 discontinuedid = a.ASMAY_Id,
                                                 discontinuedname = a.ASMAY_Year,
                                             }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO saveContinued(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                var update2 = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Single(t => t.MI_Id == data.MI_Id && t.NCACMPR112_Id == data.NCACMPR112_Id);
                update2.NCACMPR112_DiscontinuedFlg = true;
                update2.NCACPMR112_DiscontinuedYear = data.NCACPMR112_DiscontinuedYear;
                update2.NCACMPR112_DiscontinuedReason = data.NCACMPR112_DiscontinuedReason;
                update2.NCACMPR112_UpdatedBy = data.UserId;
                update2.NCACMPR112_UpdatedDate = DateTime.Now;
                _GeneralContext.Update(update2);
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
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

        public NAAC_AC_Programs_112_DTO savemappingdata(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                if (data.NCACPR112_Id == 0)
                {
                    data.AMCO_Id = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Single(t => t.NCACMPR112_Id == data.NCACMPR112_Id).AMCO_Id;
                    data.AMB_Id = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Single(t => t.NCACMPR112_Id == data.NCACMPR112_Id).AMB_Id;

                    var duplicate = _GeneralContext.NAAC_AC_Programs_112_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.NCACPR112_Year == data.NCACPR112_Year).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_Programs_112_DMO obj1 = new NAAC_AC_Programs_112_DMO();

                        obj1.NCACMPR112_Id = data.NCACMPR112_Id;//foreign

                        //obj1.NCACPR112_Id = data.NCACPR112_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.AMCO_Id = data.AMCO_Id;
                        obj1.AMB_Id = data.AMB_Id;
                        obj1.NCACPR112_ActiveFlg = true;
                        obj1.NCACPR112_CreatedBy = data.UserId;
                        obj1.NCACPR112_UpdatedBy = data.UserId;
                        obj1.NCACPR112_CreatedDate = DateTime.Now;
                        obj1.NCACPR112_UpdatedDate = DateTime.Now;
                        obj1.NCACPR112_Year = data.NCACPR112_Year;
                        obj1.NCACPR112_Date = data.NCACPR112_Date;
                        obj1.NCACPR112_DeptName = data.NCACPR112_DeptName;
                        obj1.NCACPR112_RevcarriedSyllabusYerars = data.NCACPR112_RevcarriedSyllabusYerars;
                        obj1.NCACPR112_RevisionYear = data.NCACPR112_RevisionYear;
                        obj1.NCACPR112_StatusFlg = "";
                        obj1.NCACPR112_Remarks = "";

                        _GeneralContext.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                NAAC_AC_Programs_112_FilesDMO obj2 = new NAAC_AC_Programs_112_FilesDMO();
                                obj2.NCACPR112F_FileName = data.filelist[j].cfilename;
                                obj2.NCACPR112F_Filedesc = data.filelist[j].cfiledesc;
                                obj2.NCACPR112F_FilePath = data.filelist[j].cfilepath;
                                obj2.NCACPR112_Id = obj1.NCACPR112_Id;
                                obj2.NCACPR112F_ActiveFlag = true;
                                obj2.NCACPR112F_StatusFlg = "";
                                obj2.NCACPR112F_Remarks = "";

                                _GeneralContext.Add(obj2);
                            }
                        }

                        int row = _GeneralContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                else if (data.NCACPR112_Id > 0)
                {
                    data.AMCO_Id = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Single(t => t.NCACMPR112_Id == data.NCACMPR112_Id).AMCO_Id;
                    data.AMB_Id = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Single(t => t.NCACMPR112_Id == data.NCACMPR112_Id).AMB_Id;

                    var duplicate = _GeneralContext.NAAC_AC_Programs_112_DMO.Where(t => t.NCACPR112_Id != data.NCACPR112_Id && t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.NCACPR112_Year == data.NCACPR112_Year).ToList();

                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update2 = _GeneralContext.NAAC_AC_Programs_112_DMO.Single(t => t.MI_Id == data.MI_Id && t.NCACPR112_Id == data.NCACPR112_Id);

                        update2.NCACPR112_UpdatedBy = data.UserId;
                        update2.NCACPR112_UpdatedDate = DateTime.Now;
                        update2.NCACMPR112_Id = data.NCACMPR112_Id;
                        update2.NCACPR112_Year = data.NCACPR112_Year;
                        update2.NCACPR112_Date = data.NCACPR112_Date;
                        update2.NCACPR112_DeptName = data.NCACPR112_DeptName;
                        update2.NCACPR112_RevcarriedSyllabusYerars = data.NCACPR112_RevcarriedSyllabusYerars;
                        update2.NCACPR112_RevisionYear = data.NCACPR112_RevisionYear;

                        _GeneralContext.Update(update2);

                        var CountRemoveFiles = _GeneralContext.NAAC_AC_Programs_112_FilesDMO.Where(t => t.NCACPR112_Id == data.NCACPR112_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.filelist)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_AC_Programs_112_FilesDMO.Where(t => !temparr.Contains(t.NCACPR112F_Id)
                        && t.NCACPR112_Id == data.NCACPR112_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_AC_Programs_112_FilesDMO.Single(a => a.NCACPR112F_Id == ph1.NCACPR112F_Id);
                            resultremove112.NCACPR112F_ActiveFlag = false;
                            _GeneralContext.Update(resultremove112);

                        }

                        //if (CountRemoveFiles.Count > 0)
                        //{
                        // foreach (var RemoveFiles in CountRemoveFiles)
                        // {
                        // _GeneralContext.Remove(RemoveFiles);
                        // }
                        //}

                        if (data.filelist.Length > 0)
                        {
                            for (int k = 0; k < data.filelist.Length; k++)
                            {
                                var resultupload = _GeneralContext.NAAC_AC_Programs_112_FilesDMO.Where(a => a.NCACPR112_Id == data.NCACPR112_Id
                                && a.NCACPR112F_Id == data.filelist[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_AC_Programs_112_FilesDMO.Single(a => a.NCACPR112_Id == data.NCACPR112_Id
                                    && a.NCACPR112F_Id == data.filelist[k].cfileid);
                                    resultupdateupload.NCACPR112F_Filedesc = data.filelist[k].cfiledesc;
                                    resultupdateupload.NCACPR112F_FileName = data.filelist[k].cfilename;
                                    resultupdateupload.NCACPR112F_FilePath = data.filelist[k].cfilepath;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                    {
                                        NAAC_AC_Programs_112_FilesDMO obj2 = new NAAC_AC_Programs_112_FilesDMO();
                                        obj2.NCACPR112F_FileName = data.filelist[k].cfilename;
                                        obj2.NCACPR112F_Filedesc = data.filelist[k].cfiledesc;
                                        obj2.NCACPR112F_FilePath = data.filelist[k].cfilepath;
                                        obj2.NCACPR112_Id = update2.NCACPR112_Id;
                                        obj2.NCACPR112F_ActiveFlag = true;
                                        obj2.NCACPR112F_StatusFlg = "";
                                        obj2.NCACPR112F_Remarks = "";
                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }

                        int row = _GeneralContext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
                data.mappedlistdata = (from a in _GeneralContext.NAAC_AC_Programs_112_DMO
                                       from b in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                       from y in _GeneralContext.Academic
                                       from curs in _GeneralContext.MasterCourseDMO
                                       from brnch in _GeneralContext.ClgMasterBranchDMO
                                       where (a.MI_Id == data.MI_Id && b.NCACMPR112_Id == a.NCACMPR112_Id && a.NCACPR112_Year == y.ASMAY_Id && a.AMCO_Id == curs.AMCO_Id && a.AMB_Id == brnch.AMB_Id)
                                       select new NAAC_AC_Programs_112_DTO
                                       {
                                           NCACPR112_Id = a.NCACPR112_Id,
                                           AMCO_CourseName = curs.AMCO_CourseName,
                                           AMB_BranchName = brnch.AMB_BranchName,
                                           NCACPR112_ActiveFlg = a.NCACPR112_ActiveFlg,
                                           NCACPR112_Date = a.NCACPR112_Date,
                                           ASMAY_Year = y.ASMAY_Year,
                                           NCACMPR112_DiplomaCertName = b.NCACMPR112_DiplomaCertName,
                                           MI_Id = a.MI_Id,
                                           NCACPR112_StatusFlg = a.NCACPR112_StatusFlg,
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_Programs_112_DTO deactivYTab2(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_Programs_112_DMO.Single(t => t.NCACPR112_Id == data.NCACPR112_Id && t.MI_Id == data.MI_Id);
                if (result.NCACPR112_ActiveFlg == true)
                {
                    result.NCACPR112_ActiveFlg = false;
                }
                else if (result.NCACPR112_ActiveFlg == false)
                {
                    result.NCACPR112_ActiveFlg = true;
                }
                result.NCACPR112_UpdatedBy = data.UserId;
                result.NCACPR112_UpdatedDate = DateTime.Now;
                _GeneralContext.Update(result);
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                    data.mappedlistdata = (from a in _GeneralContext.NAAC_AC_Programs_112_DMO
                                           from b in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                           from y in _GeneralContext.Academic
                                           from curs in _GeneralContext.MasterCourseDMO
                                           from brnch in _GeneralContext.ClgMasterBranchDMO
                                           where (a.MI_Id == data.MI_Id && b.NCACMPR112_Id == a.NCACMPR112_Id && a.NCACPR112_Year == y.ASMAY_Id && a.AMCO_Id == curs.AMCO_Id && a.AMB_Id == brnch.AMB_Id)
                                           select new NAAC_AC_Programs_112_DTO
                                           {
                                               NCACPR112_Id = a.NCACPR112_Id,
                                               AMCO_CourseName = curs.AMCO_CourseName,
                                               AMB_BranchName = brnch.AMB_BranchName,
                                               NCACPR112_ActiveFlg = a.NCACPR112_ActiveFlg,
                                               NCACPR112_Date = a.NCACPR112_Date,
                                               ASMAY_Year = y.ASMAY_Year,
                                               MI_Id = a.MI_Id,
                                               NCACPR112_StatusFlg = a.NCACPR112_StatusFlg,
                                           }).Distinct().ToArray();
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
        public NAAC_AC_Programs_112_DTO edittab2(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.edittab2data = _GeneralContext.NAAC_AC_Programs_112_DMO.Where(t => t.NCACPR112_Id == data.NCACPR112_Id).ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_AC_Programs_112_FilesDMO
                                      where (a.NCACPR112_Id == data.NCACPR112_Id && a.NCACPR112F_ActiveFlag == true)
                                      select new NAAC_AC_Programs_112_DTO
                                      {
                                          cfilename = a.NCACPR112F_FileName,
                                          cfilepath = a.NCACPR112F_FilePath,
                                          cfiledesc = a.NCACPR112F_Filedesc,
                                          cfileapproved = a.NCACPR112F_ApprovedFlg,
                                          cfilestatus = a.NCACPR112F_StatusFlg,
                                          cfileid = a.NCACPR112F_Id,
                                          cfileactive = a.NCACPR112F_ActiveFlag

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO viewuploadflies(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_Programs_112_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_Programs_112_DMO
                                        where (t.NCACPR112_Id == data.NCACPR112_Id && t.NCACPR112_Id == b.NCACPR112_Id && b.MI_Id == data.MI_Id
                                        && t.NCACPR112F_ActiveFlag == true)
                                        select new NAAC_AC_Programs_112_DTO
                                        {
                                            cfilename = t.NCACPR112F_FileName,
                                            cfilepath = t.NCACPR112F_FilePath,
                                            cfiledesc = t.NCACPR112F_Filedesc,
                                            NCACPR112F_Id = t.NCACPR112F_Id,
                                            NCACPR112_Id = b.NCACPR112_Id,
                                            NCACPR112F_StatusFlg = t.NCACPR112F_StatusFlg,
                                            NCACPR112F_ApprovedFlg = t.NCACPR112F_ApprovedFlg,
                                            MI_Id = b.MI_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO deleteuploadfile(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_Programs_112_FilesDMO.Where(t => t.NCACPR112F_Id == data.NCACPR112F_Id).ToList();
                if (result.Count > 0)
                {
                    foreach (var resultid in result)
                    {
                        _GeneralContext.Remove(resultid);
                    }
                }
                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_Programs_112_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_Programs_112_DMO
                                        where (t.NCACPR112_Id == data.NCACPR112_Id && t.NCACPR112_Id == b.NCACPR112_Id && b.MI_Id == data.MI_Id && t.NCACPR112F_ActiveFlag == true)
                                        select new NAAC_AC_Programs_112_DTO
                                        {
                                            cfilename = t.NCACPR112F_FileName,
                                            cfilepath = t.NCACPR112F_FilePath,
                                            cfiledesc = t.NCACPR112F_Filedesc,
                                            NCACPR112F_Id = t.NCACPR112F_Id,
                                            NCACPR112_Id = b.NCACPR112_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO get_branch(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.AMCO_Id = (from t in _GeneralContext.Adm_Course_Branch_MappingDMO
                                where (t.AMB_Id == data.AMB_Id && t.MI_Id == data.MI_Id)
                                select t).Single().AMCO_Id;

                data.masterbranchList = (from a in _GeneralContext.Adm_Course_Branch_MappingDMO
                                         from b in _GeneralContext.ClgMasterBranchDMO
                                         where (a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.MI_Id == data.MI_Id && a.AMB_Id == b.AMB_Id)
                                         select new NAAC_AC_Programs_112_DTO
                                         {
                                             AMB_Id = b.AMB_Id,
                                             AMB_BranchName = b.AMB_BranchName
                                         }).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_Programs_112_DTO get_program(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.programlist = (from a in _GeneralContext.NAAC_AC_Master_Programs_112_DMO
                                    where (a.MI_Id == data.MI_Id && a.NCACMPR112_IntroYear == data.ASMAY_Id && a.NCACMPR112_ActiveFlg == true)
                                    select a).Distinct().ToArray();

                int order = _GeneralContext.Academic.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.Is_Active == true).Single().ASMAY_Order;
                if (order > 0)
                {
                    data.discontinuedyearlist = (from a in _GeneralContext.Academic
                                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Order >= order)
                                                 select new NAAC_AC_Programs_112_DTO
                                                 {
                                                     discontinuedid = a.ASMAY_Id,
                                                     discontinuedname = a.ASMAY_Year,
                                                 }).Distinct().ToArray();
                }
                else
                {
                    data.discontinuedyearlist = (from a in _GeneralContext.Academic
                                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                                 select new NAAC_AC_Programs_112_DTO
                                                 {
                                                     discontinuedid = a.ASMAY_Id,
                                                     discontinuedname = a.ASMAY_Year,
                                                 }).Distinct().ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<NAAC_AC_Programs_112_DTO> get_Course(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Naac_get_Clg_CourseBranch";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.coursebrnchlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        //add row wise comments
        public NAAC_AC_Programs_112_DTO savemedicaldatawisecomments(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                NAAC_AC_Programs_112_Comments_DMO obj1 = new NAAC_AC_Programs_112_Comments_DMO();

                obj1.NCACPR112C_Remarks = data.Remarks;
                obj1.NCACPR112C_RemarksBy = data.UserId;
                obj1.NCACPR112C_StatusFlg = "";
                obj1.NCACPR112_Id = data.filefkid;
                obj1.NCACPR112C_ActiveFlag = true;
                obj1.NCACPR112C_CreatedBy = data.UserId;
                obj1.NCACPR112C_UpdatedBy = data.UserId;
                obj1.NCACPR112C_CreatedDate = DateTime.Now;
                obj1.NCACPR112C_UpdatedDate = DateTime.Now;
                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //add file wise comments
        public NAAC_AC_Programs_112_DTO savefilewisecomments(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                NAAC_AC_Programs_112_File_Comments_DMO obj1 = new NAAC_AC_Programs_112_File_Comments_DMO();

                obj1.NCACPR112FC_Remarks = data.Remarks;
                obj1.NCACPR112FC_RemarksBy = data.UserId;
                obj1.NCACPR112FC_StatusFlg = "";
                obj1.NCACPR112F_Id = data.filefkid;
                obj1.NCACPR112FC_ActiveFlag = true;
                obj1.NCACPR112FC_CreatedBy = data.UserId;
                obj1.NCACPR112FC_UpdatedBy = data.UserId;
                obj1.NCACPR112FC_CreatedDate = DateTime.Now;
                obj1.NCACPR112FC_UpdatedDate = DateTime.Now;

                _GeneralContext.Add(obj1);
                int s = _GeneralContext.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        // view row wise comments
        public NAAC_AC_Programs_112_DTO getcomment(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_Programs_112_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACPR112C_RemarksBy == b.Id && a.NCACPR112_Id == data.NCACPR112_Id)
                                    select new NAAC_AC_Programs_112_DTO
                                    {
                                        NCACPR112C_Remarks = a.NCACPR112C_Remarks,
                                        NCACPR112C_Id = a.NCACPR112C_Id,
                                        NCACPR112_Id = a.NCACPR112_Id,
                                        NCACPR112C_RemarksBy = a.NCACPR112C_RemarksBy,
                                        NCACPR112C_StatusFlg = a.NCACPR112C_StatusFlg,
                                        NCACPR112C_ActiveFlag = a.NCACPR112C_ActiveFlag,
                                        NCACPR112C_CreatedBy = a.NCACPR112C_CreatedBy,
                                        NCACPR112C_CreatedDate = a.NCACPR112C_CreatedDate,
                                        NCACPR112C_UpdatedDate = a.NCACPR112C_UpdatedDate,
                                        NCACPR112C_UpdatedBy = a.NCACPR112C_UpdatedBy,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACPR112C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public NAAC_AC_Programs_112_DTO getfilecomment(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_Programs_112_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACPR112FC_RemarksBy == b.Id && a.NCACPR112F_Id == data.NCACPR112F_Id)
                                     select new NAAC_AC_Programs_112_DTO
                                     {
                                         NCACPR112F_Id = a.NCACPR112F_Id,
                                         NCACPR112FC_Remarks = a.NCACPR112FC_Remarks,
                                         NCACPR112FC_Id = a.NCACPR112FC_Id,
                                         NCACPR112FC_RemarksBy = a.NCACPR112FC_RemarksBy,
                                         NCACPR112FC_StatusFlg = a.NCACPR112FC_StatusFlg,
                                         NCACPR112FC_ActiveFlag = a.NCACPR112FC_ActiveFlag,
                                         NCACPR112FC_CreatedBy = a.NCACPR112FC_CreatedBy,
                                         NCACPR112FC_CreatedDate = a.NCACPR112FC_CreatedDate,
                                         NCACPR112FC_UpdatedBy = a.NCACPR112FC_UpdatedBy,
                                         NCACPR112FC_UpdatedDate = a.NCACPR112FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACPR112FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //added by sanjeeev

        public NAAC_AC_Programs_112_DTO saveadvanceAsync(NAAC_AC_Programs_112_DTO data)
        {
            try
            {
                if (data.advimppln.Length > 0)
                {                  
                    var Listarray = new ArrayList();
                    var duplicatevalue = new ArrayList();
                    data.message = "";
                    var rowno = 1;              
                   foreach (var I in data.advimppln)                   
                    {
                        rowno += 1;
                        data.ASMAY_Id = 0; data.AMCO_Id = 0; data.AMB_Id = 0;
                        data.ASMAY_Id = _GeneralContext.Academic.Where(R => R.ASMAY_Year == I.YearOfIntroduction && R.MI_Id == data.MI_Id && R.Is_Active == true).Select(P => P.ASMAY_Id).FirstOrDefault();
                        if (data.ASMAY_Id > 0)
                        {
                            data.AMCO_Id = (from a in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                            from b in _GeneralContext.MasterCourseDMO
                                            from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                            where (a.AMCO_Id == b.AMCO_Id && c.ACAYC_Id == a.ACAYC_Id && a.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMCO_CourseName == I.ProgramName)
                                            select b.AMCO_Id
                                          ).FirstOrDefault();

                            if (data.AMCO_Id > 0)
                            {
                                data.AMB_Id = (from a in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                               from b in _GeneralContext.ClgMasterBranchDMO
                                               from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                               where (c.AMB_Id == b.AMB_Id && c.ACAYC_Id == a.ACAYC_Id && a.ACAYC_ActiveFlag == true && c.ACAYCB_ActiveFlag == true && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMB_BranchName == I.BranchName)
                                               select b.AMB_Id
                                         ).FirstOrDefault();
                            }                        
                            if (data.ASMAY_Id > 0 && data.AMCO_Id > 0 && data.AMB_Id > 0)
                            {
                                var duplicate = _GeneralContext.NAAC_AC_Master_Programs_112_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.NCACMPR112_IntroYear == data.ASMAY_Id).ToList();
                                if (duplicate.Count > 0)
                                {
                                    //  duplicatevalue.Add(I);
                                    Listarray.Add(I);

                                }
                                else
                                {
                                    NAAC_AC_Master_Programs_112_DMO obj = new NAAC_AC_Master_Programs_112_DMO();
                                    obj.MI_Id = data.MI_Id;
                                    obj.AMCO_Id = data.AMCO_Id;
                                    obj.AMB_Id = data.AMB_Id;
                                    obj.NCACMPR112_DiplomaCertName = I.CertificateName;
                                    obj.NCACMPR112_IntroYear = data.ASMAY_Id;
                                    obj.NCACMPR112_ActiveFlg = true;
                                    obj.NCACMPR112_CreatedBy = data.UserId;
                                    obj.NCACMPR112_UpdatedBy = data.UserId;
                                    obj.NCACMPR112_FromExelImportFlag = true;
                                    obj.NCACMPR112_FreezeFlag = true;
                                    obj.NCACMPR112_CreatedDate = DateTime.Now;
                                    obj.NCACMPR112_UpdatedDate = DateTime.Now;
                                    _GeneralContext.Add(obj);
                                }
                            }
                            else
                            {
                                Listarray.Add(I);
                            }
                        }
                        else
                        {
                            Listarray.Add(I);
                        }
                    }
                    data.modal = Listarray.ToArray();
                    //data.modalduplicate = duplicatevalue.ToArray();
                    int s = _GeneralContext.SaveChanges();
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
