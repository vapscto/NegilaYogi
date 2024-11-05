using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Medical;
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
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class StudentProjectImpliment : Interface.StudentProjectInterface
    {
        public GeneralContext _GeneralContext;

        public StudentProjectImpliment(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<StudentProject_DTO> loaddata(StudentProject_DTO data)
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

                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new StudentProject_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.courselist = _GeneralContext.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
                using (var cmd = _GeneralContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "getalldata_NAAC_AC_SProjects_133";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    // use null instead of {} ...ok ok fine i will
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.gridlist = (from a in _GeneralContext.NAAC_MC_SProjects_134_DMO
                                 from y in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.NCMCSP134_Year == y.ASMAY_Id)
                                 select new StudentProject_DTO
                                 {
                                     NCMCSP134_Id = a.NCMCSP134_Id,
                                     MI_Id = a.MI_Id,
                                     NCMCSP134_Year = a.NCMCSP134_Year,
                                     NCMCSP134_NoOfStudentsUndertakingFieldVisits = a.NCMCSP134_NoOfStudentsUndertakingFieldVisits,
                                     NCMCSP134_NoOfStudentsUndertakingClinical = a.NCMCSP134_NoOfStudentsUndertakingClinical,
                                     NCMCSP134_NoOfStudentsUndertakingResearchProjects = a.NCMCSP134_NoOfStudentsUndertakingResearchProjects,
                                     NCMCSP134_NoOfStudentsUndertakingIndustryVisits = a.NCMCSP134_NoOfStudentsUndertakingIndustryVisits,
                                     NCMCSP134_NoOfStudentsUndertakingCommunityPostings = a.NCMCSP134_NoOfStudentsUndertakingCommunityPostings,
                                     ASMAY_Year = y.ASMAY_Year,
                                     ASMAY_Id = y.ASMAY_Id,
                                     NCMCSP134_StatusFlg = a.NCMCSP134_StatusFlg,
                                     NCMCSP134_ApprovedFlg = a.NCMCSP134_ApprovedFlg,
                                     NCMCSP134_ActiveFlag = a.NCMCSP134_ActiveFlag,
                                 }).Distinct().ToArray();

            }
            catch (Exception t)
            {
                Console.WriteLine(t.Message);
            }
            return data;
        }
        public StudentProject_DTO savedata(StudentProject_DTO data)
        {
            try
            {
                if (data.NCACSPR133_Id == 0)
                {
                    for (int i = 0; i < data.studentlstdata.Length; i++)
                    {
                        var tempdata = data.studentlstdata[i].AMCST_Id;

                        var duplicate = _GeneralContext.NAAC_AC_SProjects_133_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMCST_Id == tempdata && t.NCACSPR133_ProjectName == data.NCACSPR133_ProjectName).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.count += 1;
                            data.msg = "Duplicate";
                        }
                        else
                        {
                            data.count1 += 1;
                            NAAC_AC_SProjects_133_DMO obj = new NAAC_AC_SProjects_133_DMO();
                            obj.MI_Id = data.MI_Id;
                            obj.AMCO_Id = data.AMCO_Id;
                            obj.AMB_Id = data.AMB_Id;
                            obj.AMCST_Id = tempdata;
                            obj.NCACSPR133_ProjectName = data.NCACSPR133_ProjectName;
                            obj.NCACSPR133_ActiveFlg = true;
                            obj.NCACSPR133_CreatedBy = data.UserId;
                            obj.NCACSPR133_CreatedDate = DateTime.Now;
                            obj.NCACSPR133_UpdatedDate = DateTime.Now;
                            obj.NCACSPR133_UpdatedBy = data.UserId;
                            obj.NCACSPR133_Year = data.ASMAY_Id;
                            obj.NCACSPR133_StatusFlg = "";
                            obj.NCACSPR133_Remarks = "";
                            _GeneralContext.Add(obj);
                            if (data.filelist.Length > 0)
                            {
                                for (int j = 0; j < data.filelist.Length; j++)
                                {
                                    NAAC_AC_SProjects_133_FilesDMO obj2 = new NAAC_AC_SProjects_133_FilesDMO();
                                    obj2.NCACSPR133F_FileName = data.filelist[j].cfilename;
                                    obj2.NCACSPR133F_Filedesc = data.filelist[j].cfiledesc;
                                    obj2.NCACSPR133F_FilePath = data.filelist[j].cfilepath;
                                    obj2.NCACSPR133_Id = obj.NCACSPR133_Id;
                                    obj2.NCACSPR133F_ActiveFlg = true;
                                    obj2.NCACSPR133F_StatusFlg = "";
                                    obj2.NCACSPR133F_Remarks = "";

                                    _GeneralContext.Add(obj2);
                                }
                            }
                            int s = _GeneralContext.SaveChanges();
                            if (s > 0)
                            {
                                data.msg = "saved";
                            }
                            else
                            {
                                data.msg = "savingFailed";
                            }
                        }
                    }
                }

                else if (data.NCACSPR133_Id > 0)
                {
                    for (int i = 0; i < data.studentlstdata.Length; i++)
                    {
                        var tempdata = data.studentlstdata[i].AMCST_Id;

                        var duplicate = _GeneralContext.NAAC_AC_SProjects_133_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSPR133_Id != data.NCACSPR133_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMCST_Id == tempdata && t.NCACSPR133_ProjectName == data.NCACSPR133_ProjectName).ToList();
                        if (duplicate.Count > 0)
                        {
                            data.count += 1;
                            data.msg = "Duplicate";
                        }
                        else
                        {
                            data.count1 += 1;
                            var update = _GeneralContext.NAAC_AC_SProjects_133_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSPR133_Id == data.NCACSPR133_Id).SingleOrDefault();

                            update.NCACSPR133_Year = data.ASMAY_Id;
                            update.AMCO_Id = data.AMCO_Id;
                            update.AMB_Id = data.AMB_Id;
                            update.AMCST_Id = tempdata;
                            update.NCACSPR133_ProjectName = data.NCACSPR133_ProjectName;
                            update.NCACSPR133_UpdatedDate = DateTime.Now;
                            update.NCACSPR133_UpdatedBy = data.UserId;
                            _GeneralContext.Update(update);

                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Where(t => t.NCACSPR133_Id == data.NCACSPR133_Id).ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}
                            //if (data.filelist.Length > 0)
                            //{
                            //    for (int k = 0; k < data.filelist.Length; k++)
                            //    {
                            //        NAAC_AC_SProjects_133_FilesDMO obj2 = new NAAC_AC_SProjects_133_FilesDMO();
                            //        obj2.NCACSPR133F_FileName = data.filelist[k].cfilename;
                            //        obj2.NCACSPR133F_Filedesc = data.filelist[k].cfiledesc;
                            //        obj2.NCACSPR133F_FilePath = data.filelist[k].cfilepath;
                            //        obj2.NCACSPR133_Id = update.NCACSPR133_Id;

                            //        _GeneralContext.Add(obj2);
                            //    }
                            //}
                            //int s = _GeneralContext.SaveChanges();
                            //if (s > 0)
                            //{
                            //    data.msg = "update";
                            //}
                            //else
                            //{
                            //    data.msg = "updateFailed";
                            //}


                            var CountRemoveFiles = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Where(t => t.NCACSPR133_Id == data.NCACSPR133_Id).ToList();

                            List<long> temparr = new List<long>();
                            //getting all mobilenumbers
                            foreach (var c in data.filelist)
                            {
                                temparr.Add(c.cfileid);
                            }


                            var Phone_Noresultremove = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Where(t => !temparr.Contains(t.NCACSPR133F_Id)
                            && t.NCACSPR133_Id == data.NCACSPR133_Id).ToList();

                            foreach (var ph1 in Phone_Noresultremove)
                            {
                                var resultremove112 = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Single(a => a.NCACSPR133F_Id == ph1.NCACSPR133F_Id);
                                resultremove112.NCACSPR133F_ActiveFlg = false;
                                _GeneralContext.Update(resultremove112);

                            }

                            if (data.filelist.Length > 0)
                            {
                                for (int k = 0; k < data.filelist.Length; k++)
                                {
                                    var resultupload = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Where(a => a.NCACSPR133_Id == data.NCACSPR133_Id
                                    && a.NCACSPR133F_Id == data.filelist[k].cfileid).ToList();
                                    if (resultupload.Count > 0)
                                    {
                                        var resultupdateupload = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Single(a => a.NCACSPR133_Id == data.NCACSPR133_Id
                                        && a.NCACSPR133F_Id == data.filelist[k].cfileid);
                                        resultupdateupload.NCACSPR133F_Filedesc = data.filelist[k].cfiledesc;
                                        resultupdateupload.NCACSPR133F_FileName = data.filelist[k].cfilename;
                                        resultupdateupload.NCACSPR133F_FilePath = data.filelist[k].cfilepath;
                                        _GeneralContext.Update(resultupdateupload);
                                    }
                                    else
                                    {
                                        if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                        {
                                            NAAC_AC_SProjects_133_FilesDMO obj2 = new NAAC_AC_SProjects_133_FilesDMO();
                                            obj2.NCACSPR133F_FileName = data.filelist[k].cfilename;
                                            obj2.NCACSPR133F_Filedesc = data.filelist[k].cfiledesc;
                                            obj2.NCACSPR133F_FilePath = data.filelist[k].cfilepath;
                                            obj2.NCACSPR133_Id = data.NCACSPR133_Id;
                                            obj2.NCACSPR133F_ActiveFlg = true;
                                            obj2.NCACSPR133F_StatusFlg = "";
                                            obj2.NCACSPR133F_Remarks = "";
                                            _GeneralContext.Add(obj2);
                                        }
                                    }
                                }
                            }

                            int s = _GeneralContext.SaveChanges();
                            if (s > 0)
                            {
                                data.msg = "update";
                            }
                            else
                            {
                                data.msg = "updateFailed";
                            }


                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //aded by sanjeev
        public StudentProject_DTO saveadvance(StudentProject_DTO data)
        {
            try
            {
                if (data.advimppln.Length > 0)
                {
                    var Listarray = new ArrayList();
                    var duplicatevalue = new ArrayList();
                    foreach (var I in data.advimppln)
                    {
                        data.ASMAY_Id = 0; data.AMCO_Id = 0; data.AMB_Id = 0; data.AMCST_Id = 0;
                        data.ASMAY_Id = _GeneralContext.Academic.Where(R => R.ASMAY_Year == I.ParticipationYear && R.MI_Id == data.MI_Id && R.Is_Active == true).Select(P => P.ASMAY_Id).FirstOrDefault();
                        data.AMCO_Id = _GeneralContext.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true && t.AMCO_CourseName == I.Course).Select(L => L.AMCO_Id).FirstOrDefault();

                        data.AMB_Id = (from a in _GeneralContext.MasterCourseDMO
                                       from d in _GeneralContext.ClgMasterBranchDMO
                                       from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                       from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                       where (a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == d.AMB_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && d.AMB_BranchName == I.Branch
                                       )
                                       select d.AMB_Id).FirstOrDefault();
                        data.AMCST_Id = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                         from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                         from c in _GeneralContext.MasterCourseDMO
                                         from d in _GeneralContext.ClgMasterBranchDMO
                                         where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == b.ASMAY_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && c.AMCO_ActiveFlag == true && d.AMB_ActiveFlag == true && a.AMCST_AdmNo == I.AdmisionNumber)
                                         select a.AMCST_Id).FirstOrDefault();
                        if (data.ASMAY_Id > 0 && data.AMCO_Id > 0 && data.AMB_Id > 0 && data.AMCST_Id > 0 && I.ProjectName != "")
                        {
                            var duplicate = _GeneralContext.NAAC_AC_SProjects_133_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMCST_Id == data.AMCST_Id && t.NCACSPR133_ProjectName == I.ProjectName).ToList();
                            if (duplicate.Count > 0)
                            {
                                duplicatevalue.Add(I);
                            }
                            else
                            {
                                NAAC_AC_SProjects_133_DMO obj = new NAAC_AC_SProjects_133_DMO();
                                obj.MI_Id = data.MI_Id;
                                obj.AMCO_Id = data.AMCO_Id;
                                obj.AMB_Id = data.AMB_Id;
                                obj.AMCST_Id = data.AMCST_Id;
                                obj.NCACSPR133_ProjectName = I.ProjectName;
                                obj.NCACSPR133_ActiveFlg = true;
                                obj.NCACSPR133_CreatedBy = data.UserId;
                                obj.NCACSPR133_CreatedDate = DateTime.Now;
                                obj.NCACSPR133_UpdatedDate = DateTime.Now;
                                obj.NCACSPR133_UpdatedBy = data.UserId;
                                obj.NCACSPR133_Year = data.ASMAY_Id;
                                obj.NCACSPR133_StatusFlg = "";
                                obj.NCACSPR133_Remarks = "";
                                //added by sanjeev
                                obj.NCACSPR133_FromExelImportFlag = true;
                                obj.NCACSPR133_FreezeFlag = true;

                                _GeneralContext.Add(obj);
                            }
                        }
                        else
                        {
                            Listarray.Add(I);
                        }

                    }

                    data.modalexcelfile = Listarray.ToArray();
                    data.duplicatfile = duplicatevalue.ToArray();
                    int s = _GeneralContext.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "savingFailed";
                    }
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        //added end sanjeev
        public StudentProject_DTO editdata(StudentProject_DTO data)
        {
            try
            {
                List<long> amcoids = new List<long>();
                List<long> ambids = new List<long>();
                List<long> amcstids = new List<long>();
                var edit = _GeneralContext.NAAC_AC_SProjects_133_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCACSPR133_Id == data.NCACSPR133_Id).ToList();
                data.editlist = edit.ToArray();
                if (edit.Count > 0)
                {
                    foreach (var item in edit)
                    {
                        amcoids.Add(item.AMCO_Id);
                    }
                    foreach (var item in edit)
                    {
                        ambids.Add(item.AMB_Id);
                    }
                    foreach (var item in edit)
                    {
                        amcstids.Add(item.AMCST_Id);
                    }
                }
                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_AC_SProjects_133_DMO
                                 where (b.NCACSPR133_Year == a.ASMAY_Id && b.MI_Id == data.MI_Id && b.NCACSPR133_Id == data.NCACSPR133_Id)
                                 select new StudentProject_DTO
                                 {
                                     NCACSPR133_Id = b.NCACSPR133_Id,
                                     NCACSPR133_Year = b.NCACSPR133_Year,
                                     NCACSPR133_ProjectName = b.NCACSPR133_ProjectName,
                                     AMCO_Id = b.AMCO_Id,
                                     AMB_Id = b.AMB_Id,
                                     AMCST_Id = b.AMCST_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     MI_Id = b.MI_Id,
                                 }).Distinct().ToArray();
                var yearids = (from a in _GeneralContext.Adm_College_Yearly_StudentDMO
                               where (amcoids.Contains(a.AMCO_Id) && ambids.Contains(a.AMB_Id) && amcstids.Contains(a.AMCST_Id))
                               select new StudentProject_DTO
                               {
                                   ASMAY_Id = a.ASMAY_Id,
                               }).Distinct().ToList();
                List<long> yeraid = new List<long>();
                foreach (var year in yearids)
                {
                    yeraid.Add(year.ASMAY_Id);
                }
                data.yearlist = (from a in _GeneralContext.Academic
                                 where (yeraid.Contains(a.ASMAY_Id))
                                 select new StudentProject_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.branchlist = (from a in _GeneralContext.MasterCourseDMO
                                   from d in _GeneralContext.ClgMasterBranchDMO
                                   from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == d.AMB_Id && b.MI_Id == data.MI_Id && yeraid.Contains(b.ASMAY_Id) && amcoids.Contains(b.AMCO_Id))
                                   select d).Distinct().OrderBy(t => t.AMB_Order).ToArray();

                data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    from c in _GeneralContext.MasterCourseDMO
                                    from d in _GeneralContext.ClgMasterBranchDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == b.ASMAY_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id && yeraid.Contains(b.ASMAY_Id) && amcoids.Contains(b.AMCO_Id) && ambids.Contains(b.AMB_Id) && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && c.AMCO_ActiveFlag == true && d.AMB_ActiveFlag == true)
                                    select new StudentProject_DTO
                                    {
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_Id = a.AMCST_Id,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_AC_SProjects_133_FilesDMO
                                      where (a.NCACSPR133_Id == data.NCACSPR133_Id && a.NCACSPR133F_ActiveFlg == true)
                                      select new StudentProject_DTO
                                      {
                                          cfilename = a.NCACSPR133F_FileName,
                                          cfilepath = a.NCACSPR133F_FilePath,
                                          cfiledesc = a.NCACSPR133F_Filedesc,
                                          cfileid = a.NCACSPR133F_Id,
                                      }).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentProject_DTO deactiveStudent(StudentProject_DTO data)
        {
            try
            {
                var u = _GeneralContext.NAAC_AC_SProjects_133_DMO.Where(t => t.NCACSPR133_Id == data.NCACSPR133_Id).SingleOrDefault();
                if (u.NCACSPR133_ActiveFlg == true)
                {
                    u.NCACSPR133_ActiveFlg = false;
                }
                else if (u.NCACSPR133_ActiveFlg == false)
                {
                    u.NCACSPR133_ActiveFlg = true;
                }
                u.NCACSPR133_UpdatedDate = DateTime.Now;
                u.NCACSPR133_UpdatedBy = data.UserId;
                _GeneralContext.Update(u);
                int o = _GeneralContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public StudentProject_DTO get_branch(StudentProject_DTO data)
        {
            try
            {
                data.branchlist = (from a in _GeneralContext.MasterCourseDMO
                                   from d in _GeneralContext.ClgMasterBranchDMO
                                   from c in _GeneralContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == d.AMB_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id
                                   )
                                   select d).Distinct().OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception w)
            {
                Console.WriteLine(w.Message);
            }
            return data;
        }
        public StudentProject_DTO get_student(StudentProject_DTO data)
        {
            try
            {
                data.studentlist = (from a in _GeneralContext.Adm_Master_College_StudentDMO
                                    from b in _GeneralContext.Adm_College_Yearly_StudentDMO
                                    from c in _GeneralContext.MasterCourseDMO
                                    from d in _GeneralContext.ClgMasterBranchDMO
                                    where (a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == b.ASMAY_Id && c.AMCO_Id == b.AMCO_Id && d.AMB_Id == b.AMB_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true && b.ACYST_ActiveFlag == 1 && c.AMCO_ActiveFlag == true && d.AMB_ActiveFlag == true)
                                    select new StudentProject_DTO
                                    {
                                        studentname = ((a.AMCST_FirstName == null ? " " : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null ? " " : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null ? " " : a.AMCST_LastName)).Trim(),
                                        AMCST_Id = a.AMCST_Id,
                                        AMCST_AdmNo = a.AMCST_AdmNo,
                                    }).Distinct().OrderBy(t => t.studentname).ToArray();
            }
            catch (Exception r)
            {
                Console.WriteLine(r.Message);
            }
            return data;
        }
        public StudentProject_DTO viewuploadflies(StudentProject_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_SProjects_133_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_SProjects_133_DMO
                                        where (t.NCACSPR133_Id == data.NCACSPR133_Id && t.NCACSPR133_Id == b.NCACSPR133_Id && b.MI_Id == data.MI_Id && t.NCACSPR133F_ActiveFlg == true)
                                        select new StudentProject_DTO
                                        {
                                            cfilename = t.NCACSPR133F_FileName,
                                            cfilepath = t.NCACSPR133F_FilePath,
                                            cfiledesc = t.NCACSPR133F_Filedesc,
                                            NCACSPR133F_Id = t.NCACSPR133F_Id,
                                            NCACSPR133_Id = b.NCACSPR133_Id,
                                            NCACSPR133F_StatusFlg = t.NCACSPR133F_StatusFlg,
                                            NCACSPR133F_ApprovedFlg = t.NCACSPR133F_ApprovedFlg,
                                            MI_Id = b.MI_Id
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public StudentProject_DTO deleteuploadfile(StudentProject_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Where(t => t.NCACSPR133F_Id == data.NCACSPR133F_Id).ToList();
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_SProjects_133_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_SProjects_133_DMO
                                        where (t.NCACSPR133_Id == data.NCACSPR133_Id && t.NCACSPR133_Id == b.NCACSPR133_Id && b.MI_Id == data.MI_Id && t.NCACSPR133F_ActiveFlg == true)
                                        select new StudentProject_DTO
                                        {
                                            cfilename = t.NCACSPR133F_FileName,
                                            cfilepath = t.NCACSPR133F_FilePath,
                                            cfiledesc = t.NCACSPR133F_Filedesc,
                                            NCACSPR133F_Id = t.NCACSPR133F_Id,
                                            NCACSPR133_Id = b.NCACSPR133_Id,
                                            NCACSPR133F_StatusFlg = t.NCACSPR133F_StatusFlg,
                                            NCACSPR133F_ApprovedFlg = t.NCACSPR133F_ApprovedFlg,
                                            MI_Id = b.MI_Id
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }



        public StudentProject_DTO MC_Savedata_134(StudentProject_DTO data)
        {
            try
            {
                if (data.NCMCSP134_Id == 0)
                {


                    var duplicate = _GeneralContext.NAAC_MC_SProjects_134_DMO.Where(t => t.MI_Id == data.MI_Id
                    && t.NCMCSP134_Year == data.NCMCSP134_Year
                    && t.NCMCSP134_NoOfStudentsUndertakingFieldVisits
                    == data.NCMCSP134_NoOfStudentsUndertakingFieldVisits
                    && t.NCMCSP134_NoOfStudentsUndertakingClinical
                    == data.NCMCSP134_NoOfStudentsUndertakingClinical
                    && t.NCMCSP134_NoOfStudentsUndertakingIndustryVisits
                    == data.NCMCSP134_NoOfStudentsUndertakingIndustryVisits
                    && t.NCMCSP134_NoOfStudentsUndertakingCommunityPostings
                    == data.NCMCSP134_NoOfStudentsUndertakingCommunityPostings
                    ).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.msg = "Duplicate";
                    }
                    else
                    {

                        NAAC_MC_SProjects_134_DMO obj5 = new NAAC_MC_SProjects_134_DMO();
                        //obj5.NCMCSP134_Id = data.NCMCSP134_Id;
                        obj5.MI_Id = data.MI_Id;
                        obj5.NCMCSP134_Year = data.NCMCSP134_Year;
                        obj5.NCMCSP134_NoOfStudentsUndertakingFieldVisits = data.NCMCSP134_NoOfStudentsUndertakingFieldVisits;
                        obj5.NCMCSP134_NoOfStudentsUndertakingClinical = data.NCMCSP134_NoOfStudentsUndertakingClinical;
                        obj5.NCMCSP134_NoOfStudentsUndertakingResearchProjects = data.NCMCSP134_NoOfStudentsUndertakingResearchProjects;
                        obj5.NCMCSP134_NoOfStudentsUndertakingIndustryVisits = data.NCMCSP134_NoOfStudentsUndertakingIndustryVisits;
                        obj5.NCMCSP134_NoOfStudentsUndertakingCommunityPostings = data.NCMCSP134_NoOfStudentsUndertakingCommunityPostings;
                        obj5.NCMCSP134_CreatedBy = data.UserId;
                        obj5.NCMCSP134_UpdatedBy = data.UserId;
                        obj5.NCMCSP134_CreateDate = DateTime.Now;
                        obj5.NCMCSP134_UpdatedDate = DateTime.Now;
                        obj5.NCMCSP134_StatusFlg = "";
                        obj5.NCMCSP134_Remarks = "";

                        _GeneralContext.Add(obj5);
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                NAAC_MC_SProjects_134_Files_DMO obj2 = new NAAC_MC_SProjects_134_Files_DMO();
                                obj2.NCMCSP134F_FileName = data.filelist[j].cfilename;
                                obj2.NCMCSP134F_Filedesc = data.filelist[j].cfiledesc;
                                obj2.NCMCSP134F_FilePath = data.filelist[j].cfilepath;
                                obj2.NCMCSP134_Id = obj5.NCMCSP134_Id;
                                obj2.NCMCSP134F_ActiveFlg = true;
                                obj2.NCMCSP134F_StatusFlg = "";
                                obj2.NCMCSP134F_Remarks = "";

                                _GeneralContext.Add(obj2);
                            }
                        }
                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "savingFailed";
                        }
                    }
                }


                else if (data.NCMCSP134_Id > 0)
                {


                    var duplicate = _GeneralContext.NAAC_MC_SProjects_134_DMO.Where(t => t.NCMCSP134_Id != data.NCMCSP134_Id && t.MI_Id == data.MI_Id
                     && t.NCMCSP134_Year == data.NCMCSP134_Year
                     && t.NCMCSP134_NoOfStudentsUndertakingFieldVisits
                     == data.NCMCSP134_NoOfStudentsUndertakingFieldVisits
                     && t.NCMCSP134_NoOfStudentsUndertakingClinical
                     == data.NCMCSP134_NoOfStudentsUndertakingClinical
                     && t.NCMCSP134_NoOfStudentsUndertakingIndustryVisits
                     == data.NCMCSP134_NoOfStudentsUndertakingIndustryVisits
                     && t.NCMCSP134_NoOfStudentsUndertakingCommunityPostings
                     == data.NCMCSP134_NoOfStudentsUndertakingCommunityPostings
                     ).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.msg = "Duplicate";
                    }
                    else
                    {

                        var update = _GeneralContext.NAAC_MC_SProjects_134_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMCSP134_Id == data.NCMCSP134_Id).SingleOrDefault();


                        update.NCMCSP134_Year = data.NCMCSP134_Year;
                        update.NCMCSP134_NoOfStudentsUndertakingFieldVisits = data.NCMCSP134_NoOfStudentsUndertakingFieldVisits;
                        update.NCMCSP134_NoOfStudentsUndertakingClinical = data.NCMCSP134_NoOfStudentsUndertakingClinical;
                        update.NCMCSP134_NoOfStudentsUndertakingResearchProjects = data.NCMCSP134_NoOfStudentsUndertakingResearchProjects;
                        update.NCMCSP134_NoOfStudentsUndertakingIndustryVisits = data.NCMCSP134_NoOfStudentsUndertakingIndustryVisits;
                        update.NCMCSP134_NoOfStudentsUndertakingCommunityPostings = data.NCMCSP134_NoOfStudentsUndertakingCommunityPostings;
                        update.NCMCSP134_UpdatedBy = data.UserId;
                        update.NCMCSP134_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update);



                        var CountRemoveFiles = _GeneralContext.NAAC_MC_SProjects_134_Files_DMO.Where(t => t.NCMCSP134_Id == data.NCMCSP134_Id).ToList();

                        List<long> temparr = new List<long>();
                        //getting all mobilenumbers
                        foreach (var c in data.filelist)
                        {
                            temparr.Add(c.cfileid);
                        }


                        var Phone_Noresultremove = _GeneralContext.NAAC_MC_SProjects_134_Files_DMO.Where(t => !temparr.Contains(t.NCMCSP134F_Id)
                        && t.NCMCSP134_Id == data.NCMCSP134_Id).ToList();

                        foreach (var ph1 in Phone_Noresultremove)
                        {
                            var resultremove112 = _GeneralContext.NAAC_MC_SProjects_134_Files_DMO.Single(a => a.NCMCSP134F_Id == ph1.NCMCSP134F_Id);
                            resultremove112.NCMCSP134F_ActiveFlg = false;
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
                                var resultupload = _GeneralContext.NAAC_MC_SProjects_134_Files_DMO.Where(a => a.NCMCSP134_Id == data.NCMCSP134_Id
                                && a.NCMCSP134F_Id == data.filelist[k].cfileid).ToList();
                                if (resultupload.Count > 0)
                                {
                                    var resultupdateupload = _GeneralContext.NAAC_MC_SProjects_134_Files_DMO.Single(a => a.NCMCSP134_Id == data.NCMCSP134_Id
                                    && a.NCMCSP134F_Id == data.filelist[k].cfileid);
                                    resultupdateupload.NCMCSP134F_Filedesc = data.filelist[k].cfiledesc;
                                    resultupdateupload.NCMCSP134F_FileName = data.filelist[k].cfilename;
                                    resultupdateupload.NCMCSP134F_FilePath = data.filelist[k].cfilepath;
                                    _GeneralContext.Update(resultupdateupload);
                                }
                                else
                                {
                                    if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                    {
                                        NAAC_MC_SProjects_134_Files_DMO obj2 = new NAAC_MC_SProjects_134_Files_DMO();
                                        obj2.NCMCSP134F_FileName = data.filelist[k].cfilename;
                                        obj2.NCMCSP134F_Filedesc = data.filelist[k].cfiledesc;
                                        obj2.NCMCSP134F_FilePath = data.filelist[k].cfilepath;
                                        obj2.NCMCSP134_Id = data.NCMCSP134_Id;
                                        obj2.NCMCSP134F_ActiveFlg = true;
                                        obj2.NCMCSP134F_StatusFlg = "";
                                        obj2.NCMCSP134F_Remarks = "";
                                        _GeneralContext.Add(obj2);
                                    }
                                }
                            }
                        }

                        int row = _GeneralContext.SaveChanges();
                        if (row > 0)
                        {
                            data.msg = "update";
                        }
                        else
                        {
                            data.msg = "failed";
                        }


                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentProject_DTO MC_editdata_134(StudentProject_DTO data)
        {
            try
            {

                data.editlist = (from a in _GeneralContext.Academic
                                 from b in _GeneralContext.NAAC_MC_SProjects_134_DMO
                                 where (b.NCMCSP134_Year == a.ASMAY_Id && b.MI_Id == data.MI_Id && b.NCMCSP134_Id == data.NCMCSP134_Id)
                                 select new StudentProject_DTO
                                 {
                                     NCMCSP134_Id = b.NCMCSP134_Id,
                                     NCMCSP134_Year = b.NCMCSP134_Year,
                                     MI_Id = b.MI_Id,
                                     NCMCSP134_NoOfStudentsUndertakingFieldVisits = b.NCMCSP134_NoOfStudentsUndertakingFieldVisits,
                                     NCMCSP134_NoOfStudentsUndertakingClinical = b.NCMCSP134_NoOfStudentsUndertakingClinical,
                                     NCMCSP134_NoOfStudentsUndertakingResearchProjects = b.NCMCSP134_NoOfStudentsUndertakingResearchProjects,
                                     NCMCSP134_NoOfStudentsUndertakingIndustryVisits = b.NCMCSP134_NoOfStudentsUndertakingIndustryVisits,
                                     NCMCSP134_NoOfStudentsUndertakingCommunityPostings = b.NCMCSP134_NoOfStudentsUndertakingCommunityPostings,

                                 }).Distinct().ToArray();


                data.editFileslist = (from a in _GeneralContext.NAAC_MC_SProjects_134_Files_DMO
                                      where (a.NCMCSP134_Id == data.NCMCSP134_Id && a.NCMCSP134F_ActiveFlg == true)
                                      select new StudentProject_DTO
                                      {
                                          cfilename = a.NCMCSP134F_FileName,
                                          cfilepath = a.NCMCSP134F_FilePath,
                                          cfiledesc = a.NCMCSP134F_Filedesc,
                                          cfileid = a.NCMCSP134F_Id,
                                      }).Distinct().ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public StudentProject_DTO MC_viewuploadflies_134(StudentProject_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_SProjects_134_Files_DMO
                                        from b in _GeneralContext.NAAC_MC_SProjects_134_DMO
                                        where (t.NCMCSP134_Id == data.NCMCSP134_Id && t.NCMCSP134_Id == b.NCMCSP134_Id && b.MI_Id == data.MI_Id && t.NCMCSP134F_ActiveFlg == true)
                                        select new StudentProject_DTO
                                        {
                                            cfilename = t.NCMCSP134F_FileName,
                                            cfilepath = t.NCMCSP134F_FilePath,
                                            cfiledesc = t.NCMCSP134F_Filedesc,
                                            NCMCSP134F_Id = t.NCMCSP134F_Id,
                                            NCMCSP134_Id = b.NCMCSP134_Id,
                                            NCMCSP134F_StatusFlg = t.NCMCSP134F_StatusFlg,
                                            NCMCSP134F_ApprovedFlg = t.NCMCSP134F_ApprovedFlg,
                                            MI_Id = b.MI_Id
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public StudentProject_DTO MC_deleteuploadfile_134(StudentProject_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_SProjects_133_FilesDMO.Where(t => t.NCACSPR133F_Id == data.NCACSPR133F_Id).ToList();
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_SProjects_133_FilesDMO
                                        from b in _GeneralContext.NAAC_AC_SProjects_133_DMO
                                        where (t.NCACSPR133_Id == data.NCACSPR133_Id && t.NCACSPR133_Id == b.NCACSPR133_Id && b.MI_Id == data.MI_Id && t.NCACSPR133F_ActiveFlg == true)
                                        select new StudentProject_DTO
                                        {
                                            cfilename = t.NCACSPR133F_FileName,
                                            cfilepath = t.NCACSPR133F_FilePath,
                                            cfiledesc = t.NCACSPR133F_Filedesc,
                                            NCACSPR133F_Id = t.NCACSPR133F_Id,
                                            NCACSPR133_Id = b.NCACSPR133_Id,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }




        //add row wise comments
        public StudentProject_DTO savemedicaldatawisecomments(StudentProject_DTO data)
        {
            try
            {
                NAAC_MC_SProjects_134_Comments_DMO obj1 = new NAAC_MC_SProjects_134_Comments_DMO();

                obj1.NCMCSP134C_Remarks = data.Remarks;
                obj1.NCMCSP134C_RemarksBy = data.UserId;
                obj1.NCMCSP134C_StatusFlg = "";
                obj1.NCMCSP134_Id = data.filefkid;
                obj1.NCMCSP134C_ActiveFlag = true;
                obj1.NCMCSP134C_CreatedBy = data.UserId;
                obj1.NCMCSP134C_UpdatedBy = data.UserId;
                obj1.NCMCSP134C_CreatedDate = DateTime.Now;
                obj1.NCMCSP134C_UpdatedDate = DateTime.Now;
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
        public StudentProject_DTO savefilewisecomments(StudentProject_DTO data)
        {
            try
            {
                NAAC_MC_SProjects_134_File_Comments_DMO obj1 = new NAAC_MC_SProjects_134_File_Comments_DMO();

                obj1.NCMCSP134FC_Remarks = data.Remarks;
                obj1.NCMCSP134FC_RemarksBy = data.UserId;
                obj1.NCMCSP134FC_StatusFlg = "";
                obj1.NCMCSP134F_Id = data.filefkid;
                obj1.NCMCSP134FC_ActiveFlag = true;
                obj1.NCMCSP134FC_CreatedBy = data.UserId;
                obj1.NCMCSP134FC_UpdatedBy = data.UserId;
                obj1.NCMCSP134FC_CreatedDate = DateTime.Now;
                obj1.NCMCSP134FC_UpdatedDate = DateTime.Now;

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
        public StudentProject_DTO getcomment(StudentProject_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_MC_SProjects_134_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCMCSP134C_RemarksBy == b.Id && a.NCMCSP134_Id == data.NCMCSP134_Id)
                                    select new StudentProject_DTO
                                    {
                                        NCMCSP134C_Remarks = a.NCMCSP134C_Remarks,
                                        NCMCSP134_Id = a.NCMCSP134_Id,
                                        NCMCSP134C_Id = a.NCMCSP134C_Id,
                                        NCMCSP134C_RemarksBy = a.NCMCSP134C_RemarksBy,
                                        NCMCSP134C_StatusFlg = a.NCMCSP134C_StatusFlg,
                                        NCMCSP134C_ActiveFlag = a.NCMCSP134C_ActiveFlag,
                                        NCMCSP134C_CreatedBy = a.NCMCSP134C_CreatedBy,
                                        NCMCSP134C_CreatedDate = a.NCMCSP134C_CreatedDate,
                                        NCMCSP134C_UpdatedBy = a.NCMCSP134C_UpdatedBy,
                                        NCMCSP134C_UpdatedDate = a.NCMCSP134C_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCMCSP134FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public StudentProject_DTO getfilecomment(StudentProject_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_MC_SProjects_134_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCMCSP134FC_RemarksBy == b.Id && a.NCMCSP134F_Id == data.NCMCSP134F_Id)
                                     select new StudentProject_DTO
                                     {
                                         NCMCSP134F_Id = a.NCMCSP134F_Id,
                                         NCMCSP134FC_Remarks = a.NCMCSP134FC_Remarks,
                                         NCMCSP134FC_Id = a.NCMCSP134FC_Id,
                                         NCMCSP134FC_RemarksBy = a.NCMCSP134FC_RemarksBy,
                                         NCMCSP134FC_StatusFlg = a.NCMCSP134FC_StatusFlg,
                                         NCMCSP134FC_ActiveFlag = a.NCMCSP134FC_ActiveFlag,
                                         NCMCSP134FC_CreatedBy = a.NCMCSP134FC_CreatedBy,
                                         NCMCSP134FC_UpdatedBy = a.NCMCSP134FC_UpdatedBy,
                                         NCMCSP134FC_CreatedDate = a.NCMCSP134FC_CreatedDate,
                                         NCMCSP134FC_UpdatedDate = a.NCMCSP134FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCMCSP134FC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        //add row wise comments
        public StudentProject_DTO savedatawisecommentsAffi(StudentProject_DTO data)
        {
            try
            {
                NAAC_AC_SProjects_133_Comments_DMO obj1 = new NAAC_AC_SProjects_133_Comments_DMO();

                obj1.NCACSPR133C_Remarks = data.Remarks;
                obj1.NCACSPR133C_RemarksBy = data.UserId;
                obj1.NCACSPR133C_StatusFlg = "";
                obj1.NCACSPR133_Id = data.filefkid;
                obj1.NCACSPR133C_ActiveFlag = true;
                obj1.NCACSPR133C_CreatedBy = data.UserId;
                obj1.NCACSPR133C_UpdatedBy = data.UserId;
                obj1.NCACSPR133C_CreatedDate = DateTime.Now;
                obj1.NCACSPR133_UpdatedDate = DateTime.Now;
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
        public StudentProject_DTO savefilewisecommentsAffi(StudentProject_DTO data)
        {
            try
            {


                NAAC_AC_SProjects_133_File_Comments_DMO obj1 = new NAAC_AC_SProjects_133_File_Comments_DMO();

                obj1.NCACSPR133FC_Remarks = data.Remarks;
                obj1.NCACSPR133FC_RemarksBy = data.UserId;
                obj1.NCACSPR133FC_StatusFlg = "";
                obj1.NCACSPR133FC_Id = data.filefkid;
                obj1.NCACSPR133FC_ActiveFlag = true;
                obj1.NCACSPR133FC_CreatedBy = data.UserId;
                obj1.NCACSPR133FC_UpdatedBy = data.UserId;
                obj1.NCACSPR133FC_CreatedDate = DateTime.Now;
                obj1.NCACSPR133FC_UpdatedDate = DateTime.Now;

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
        public StudentProject_DTO getcommentAffi(StudentProject_DTO data)
        {
            try
            {

                data.commentlist = (from a in _GeneralContext.NAAC_AC_SProjects_133_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCACSPR133C_RemarksBy == b.Id && a.NCACSPR133_Id == data.NCACSPR133_Id)
                                    select new StudentProject_DTO
                                    {
                                        NCACSPR133C_Remarks = a.NCACSPR133C_Remarks,
                                        NCACSPR133_Id = a.NCACSPR133_Id,
                                        NCACSPR133C_Id = a.NCACSPR133C_Id,
                                        NCACSPR133C_RemarksBy = a.NCACSPR133C_RemarksBy,
                                        NCACSPR133C_StatusFlg = a.NCACSPR133C_StatusFlg,
                                        NCACSPR133C_ActiveFlag = a.NCACSPR133C_ActiveFlag,
                                        NCACSPR133C_CreatedBy = a.NCACSPR133C_CreatedBy,
                                        NCACSPR133C_CreatedDate = a.NCACSPR133C_CreatedDate,
                                        NCACSPR133C_UpdatedBy = a.NCACSPR133C_UpdatedBy,
                                        NCACSPR133_UpdatedDate = a.NCACSPR133_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCACSPR133C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public StudentProject_DTO getfilecommentAffi(StudentProject_DTO data)
        {
            try
            {

                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_SProjects_133_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCACSPR133FC_RemarksBy == b.Id && a.NCACSPR133F_Id == data.NCACSPR133F_Id)
                                     select new StudentProject_DTO
                                     {
                                         NCACSPR133FC_Id = a.NCACSPR133FC_Id,
                                         NCACSPR133FC_Remarks = a.NCACSPR133FC_Remarks,
                                         NCACSPR133F_Id = a.NCACSPR133F_Id,
                                         NCACSPR133FC_RemarksBy = a.NCACSPR133FC_RemarksBy,
                                         NCACSPR133FC_StatusFlg = a.NCACSPR133FC_StatusFlg,
                                         NCACSPR133FC_ActiveFlag = a.NCACSPR133FC_ActiveFlag,
                                         NCACSPR133FC_CreatedBy = a.NCACSPR133FC_CreatedBy,
                                         NCACSPR133FC_CreatedDate = a.NCACSPR133FC_CreatedDate,
                                         NCACSPR133FC_UpdatedBy = a.NCACSPR133FC_UpdatedBy,
                                         NCACSPR133FC_UpdatedDate = a.NCACSPR133FC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCACSPR133C_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public StudentProject_DTO deactiveY(StudentProject_DTO data)
        {
            try
            {
                var u = _GeneralContext.NAAC_MC_SProjects_134_DMO.Where(t => t.NCMCSP134_Id == data.NCMCSP134_Id).SingleOrDefault();
                if (u.NCMCSP134_ActiveFlag == true)
                {
                    u.NCMCSP134_ActiveFlag = false;
                }
                else if (u.NCMCSP134_ActiveFlag == false)
                {
                    u.NCMCSP134_ActiveFlag = true;
                }
                u.NCMCSP134_UpdatedDate = DateTime.Now;
                u.NCMCSP134_UpdatedBy = data.UserId;
                _GeneralContext.Update(u);
                int o = _GeneralContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }




    }
}
