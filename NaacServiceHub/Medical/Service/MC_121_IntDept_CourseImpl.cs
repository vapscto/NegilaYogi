using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Medical;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Service
{
    public class MC_121_IntDept_CourseImpl : Interface.MC_121_IntDept_CourseInterface
    {
        public GeneralContext _GeneralContext;
        public MC_121_IntDept_CourseImpl(GeneralContext parameter)
        {
            _GeneralContext = parameter;
        }

        public MC_121_IntDept_Course_DTO loaddata(MC_121_IntDept_Course_DTO data)
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
                                 select new MC_121_IntDept_Course_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();


                data.departmentlist = _GeneralContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_Id).ToArray();

                data.alldata = (from a in _GeneralContext.NAAC_MC_121_IntDept_CourseDMO
                                from b in _GeneralContext.HR_Master_Department
                                from c in _GeneralContext.MasterCourseDMO
                                from y in _GeneralContext.Academic
                                where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && a.AMCO_Id == c.AMCO_Id && a.ASMAY_Id == y.ASMAY_Id)
                                select new MC_121_IntDept_Course_DTO
                                {
                                    NMC121IDC_Id = a.NMC121IDC_Id,
                                    MI_Id = a.MI_Id,
                                    ASMAY_Id = a.ASMAY_Id,
                                    HRMD_Id = a.HRMD_Id,
                                    AMCO_Id = a.AMCO_Id,
                                    NMC121IDC_ActiveFlag = a.NMC121IDC_ActiveFlag,
                                    NMC121IDC_NoOfCourse = a.NMC121IDC_NoOfCourse,
                                    AMCO_CourseName = c.AMCO_CourseName,
                                    HRMD_DepartmentName = b.HRMD_DepartmentName,
                                    ASMAY_Year = y.ASMAY_Year,
                                    NMC121IDC_ApprovedFlg = a.NMC121IDC_ApprovedFlg,
                                    NMC121IDC_StatusFlg = a.NMC121IDC_StatusFlg,
                                }).Distinct().OrderByDescending(t => t.NMC121IDC_Id).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public MC_121_IntDept_Course_DTO savedata(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                if (data.NMC121IDC_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_121_IntDept_CourseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.HRMD_Id == data.HRMD_Id && t.AMCO_Id == data.AMCO_Id && t.NMC121IDC_NoOfCourse == data.NMC121IDC_NoOfCourse).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_121_IntDept_CourseDMO obj = new NAAC_MC_121_IntDept_CourseDMO();

                        obj.NMC121IDC_Id = data.NMC121IDC_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.HRMD_Id = data.HRMD_Id;
                        obj.AMCO_Id = data.AMCO_Id;
                        obj.NMC121IDC_NoOfCourse = data.NMC121IDC_NoOfCourse;
                        obj.NMC121IDC_ActiveFlag = true;
                        obj.NMC121IDC_CreatedBy = data.UserId;
                        obj.NMC121IDC_UpdatedBy = data.UserId;
                        obj.NMC121IDC_CreatedDate = DateTime.Now;
                        obj.NMC121IDC_UpdatedDate = DateTime.Now;
                        obj.NMC121IDC_StatusFlg = "";
                        obj.NMC121IDC_Remarks = "";

                        _GeneralContext.Add(obj);

                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {
                                    NAAC_MC_121_IntDept_Course_FilesDMO obj2 = new NAAC_MC_121_IntDept_Course_FilesDMO();

                                    obj2.NMC121IDCF_FileName = data.filelist[j].cfilename;
                                    obj2.NMC121IDCF_FileDesc = data.filelist[j].cfiledesc;
                                    obj2.NMC121IDCF_FilePath = data.filelist[j].cfilepath;
                                    obj2.NMC121IDC_Id = obj.NMC121IDC_Id;
                                    obj2.NMC121IDCF_CreatedBy = data.UserId;
                                    obj2.NMC121IDCF_UpdatedBy = data.UserId;
                                    obj2.NMC121IDCF_CreatedDate = DateTime.Now;
                                    obj2.NMC121IDCF_UpdatedDate = DateTime.Now;
                                    obj2.NMC121IDCF_ActiveFlg = true;
                                    obj2.NMC121IDCF_StatusFlg = "";
                                    obj2.NMC121IDCF_Remarks = "";
                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                        int s = _GeneralContext.SaveChanges();
                        if (s > 0)
                        {
                            data.msg = "saved";
                            data.returnval = true;
                        }
                        else
                        {
                            data.msg = "notsaved";
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NMC121IDC_Id > 0)
                {
                    var update1 = _GeneralContext.NAAC_MC_121_IntDept_CourseDMO.Where(t => t.MI_Id == data.MI_Id && t.NMC121IDC_Id == data.NMC121IDC_Id).SingleOrDefault();

                    update1.ASMAY_Id = data.ASMAY_Id;
                    update1.HRMD_Id = data.HRMD_Id;
                    update1.AMCO_Id = data.AMCO_Id;
                    update1.NMC121IDC_NoOfCourse = data.NMC121IDC_NoOfCourse;
                    update1.NMC121IDC_UpdatedBy = data.UserId;
                    update1.NMC121IDC_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update1);

                 

                    var CountRemoveFiles = _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO.Where(b => b.NMC121IDC_Id == data.NMC121IDC_Id).ToList();

                    List<long> temparr = new List<long>();
                    //getting all mobilenumbers
                    foreach (var c in data.filelist)
                    {
                        temparr.Add(c.cfileid);
                    }


                    var Phone_Noresultremove = _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO.Where(c => !temparr.Contains(c.NMC121IDCF_Id)
                    && c.NMC121IDC_Id == data.NMC121IDC_Id).ToList();

                    foreach (var ph1 in Phone_Noresultremove)
                    {
                        var resultremove112 = _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO.Single(a => a.NMC121IDCF_Id == ph1.NMC121IDCF_Id);
                        resultremove112.NMC121IDCF_ActiveFlg = false;
                        _GeneralContext.Update(resultremove112);

                    }

                  

                    if (data.filelist.Length > 0)
                    {
                        for (int k = 0; k < data.filelist.Length; k++)
                        {
                            var resultupload = _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO.Where(a => a.NMC121IDC_Id == data.NMC121IDC_Id
                            && a.NMC121IDCF_Id == data.filelist[k].cfileid).ToList();
                            if (resultupload.Count > 0)
                            {
                                var resultupdateupload = _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO.Single(a => a.NMC121IDC_Id == data.NMC121IDC_Id
                                && a.NMC121IDCF_Id == data.filelist[k].cfileid);
                                resultupdateupload.NMC121IDCF_FileDesc = data.filelist[k].cfiledesc;
                                resultupdateupload.NMC121IDCF_FileName = data.filelist[k].cfilename;
                                resultupdateupload.NMC121IDCF_FilePath = data.filelist[k].cfilepath;
                                resultupdateupload.NMC121IDCF_UpdatedBy = data.UserId;
                                resultupdateupload.NMC121IDCF_UpdatedDate = DateTime.Now;

                                _GeneralContext.Update(resultupdateupload);
                            }
                            else
                            {
                                if (data.filelist[k].cfilepath != null && data.filelist[k].cfilepath != "")
                                {
                                    NAAC_MC_121_IntDept_Course_FilesDMO obj2 = new NAAC_MC_121_IntDept_Course_FilesDMO();
                                    obj2.NMC121IDCF_FileName = data.filelist[k].cfilename;
                                    obj2.NMC121IDCF_FileDesc = data.filelist[k].cfiledesc;
                                    obj2.NMC121IDCF_FilePath = data.filelist[k].cfilepath;
                                    obj2.NMC121IDC_Id = data.NMC121IDC_Id;
                                    obj2.NMC121IDCF_CreatedBy = data.UserId;
                                    obj2.NMC121IDCF_UpdatedBy = data.UserId;
                                    obj2.NMC121IDCF_CreatedDate = DateTime.Now;
                                    obj2.NMC121IDCF_UpdatedDate = DateTime.Now;
                                    obj2.NMC121IDCF_ActiveFlg = true;
                                    obj2.NMC121IDCF_StatusFlg = "";
                                    obj2.NMC121IDCF_Remarks = "";

                                    _GeneralContext.Add(obj2);
                                }
                            }
                        }
                    }

                    int s = _GeneralContext.SaveChanges();
                    if (s > 0)
                    {
                        data.msg = "updated";
                        data.returnval = true;
                    }
                    else
                    {
                        data.msg = "notupdated";
                        data.returnval = false;
                    }

                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public MC_121_IntDept_Course_DTO editdata(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                var edit = (from a in _GeneralContext.NAAC_MC_121_IntDept_CourseDMO
                            where (a.NMC121IDC_Id == data.NMC121IDC_Id)
                            select new MC_121_IntDept_Course_DTO
                            {
                                NMC121IDC_Id = a.NMC121IDC_Id,
                                AMCO_Id = a.AMCO_Id,
                                ASMAY_Id = a.ASMAY_Id,
                                HRMD_Id = a.HRMD_Id,
                                MI_Id = a.MI_Id,
                                NMC121IDC_NoOfCourse = a.NMC121IDC_NoOfCourse,
                            }).Distinct().ToList();

                data.editlist = edit.Distinct().ToArray();

                if (edit.Count > 0)
                {
                    data.AMCO_Id = edit[0].AMCO_Id;
                    data.ASMAY_Id = edit[0].ASMAY_Id;
                    data.HRMD_Id = edit[0].HRMD_Id;
                    data.MI_Id = edit[0].MI_Id;

                    data.courselist = (from a in _GeneralContext.MasterCourseDMO
                                       from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                       where (b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id
                                       && a.MI_Id == b.MI_Id && a.AMCO_Id == b.AMCO_Id)
                                       select a).Distinct().OrderBy(t => t.AMCO_Id).ToArray();

                    data.editcourselist = (from a in _GeneralContext.MasterCourseDMO
                                           from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                                           where (b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id
                                           && a.AMCO_Id == data.AMCO_Id && a.MI_Id == b.MI_Id
                                           && a.AMCO_Id == b.AMCO_Id)
                                           select a).Distinct().OrderBy(t => t.AMCO_Id).ToArray();


                    //data.departmentlist = _GeneralContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).Distinct().OrderBy(t => t.HRMD_Id).ToArray();


                    //data.editdeprt = (from a in _GeneralContext.HR_Master_Department
                    //                  where (a.MI_Id == data.MI_Id && a.HRMD_Id == data.HRMD_Id)
                    //                  select a).Distinct().ToArray();
                }



                data.editFileslist = (from a in _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO
                                      where (a.NMC121IDC_Id == data.NMC121IDC_Id && a.NMC121IDCF_ActiveFlg==true)
                                      select new MC_121_IntDept_Course_DTO
                                      {
                                          cfilename = a.NMC121IDCF_FileName,
                                          cfilepath = a.NMC121IDCF_FilePath,
                                          cfiledesc = a.NMC121IDCF_FileDesc,
                                          NMC121IDCF_Id = a.NMC121IDCF_Id,
                                          NMC121IDC_Id = a.NMC121IDC_Id,
                                          cfileid = a.NMC121IDCF_Id,
                                        
                                      }).Distinct().ToArray();


            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public MC_121_IntDept_Course_DTO deactivY(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_121_IntDept_CourseDMO.Where(t => t.NMC121IDC_Id == data.NMC121IDC_Id).SingleOrDefault();

                if (result.NMC121IDC_ActiveFlag == true)
                {
                    result.NMC121IDC_ActiveFlag = false;
                }
                else if (result.NMC121IDC_ActiveFlag == false)
                {
                    result.NMC121IDC_ActiveFlag = true;
                }

                result.NMC121IDC_UpdatedDate = DateTime.Now;
                result.NMC121IDC_UpdatedBy = data.UserId;

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
        public async Task<MC_121_IntDept_Course_DTO> get_Course(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                //data.courselist = (from a in _GeneralContext.MasterCourseDMO
                //                   from b in _GeneralContext.CLG_Adm_College_AY_CourseDMO
                //                   where (b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id &&
                //                   a.MI_Id == b.MI_Id && a.AMCO_Id == b.AMCO_Id)
                //                   select a).Distinct().OrderBy(t => t.AMCO_Id).ToArray();

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
                        data.courselist = retObject.ToArray();
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
        public MC_121_IntDept_Course_DTO viewuploadflies(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO
                                        from b in _GeneralContext.NAAC_MC_121_IntDept_CourseDMO
                                        where (t.NMC121IDC_Id == data.NMC121IDC_Id && t.NMC121IDC_Id == b.NMC121IDC_Id && b.MI_Id == data.MI_Id && t.NMC121IDCF_ActiveFlg == true)
                                        select new MC_121_IntDept_Course_DTO
                                        {
                                            cfilename = t.NMC121IDCF_FileName,
                                            cfilepath = t.NMC121IDCF_FilePath,
                                            cfiledesc = t.NMC121IDCF_FileDesc,
                                            NMC121IDCF_Id = t.NMC121IDCF_Id,
                                            NMC121IDC_Id = b.NMC121IDC_Id,
                                            MI_Id = b.MI_Id,
                                            NMC121IDCF_StatusFlg = t.NMC121IDCF_StatusFlg,
                                            NMC121IDCF_ApprovedFlg = t.NMC121IDCF_ApprovedFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public MC_121_IntDept_Course_DTO deleteuploadfile(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO.Where(t => t.NMC121IDCF_Id == data.NMC121IDCF_Id).ToList();
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
                data.viewuploadflies = (from t in _GeneralContext.NAAC_MC_121_IntDept_Course_FilesDMO
                                        from b in _GeneralContext.NAAC_MC_121_IntDept_CourseDMO
                                        where (t.NMC121IDC_Id == data.NMC121IDC_Id && t.NMC121IDC_Id == b.NMC121IDC_Id && b.MI_Id == data.MI_Id && t.NMC121IDCF_ActiveFlg==true)
                                        select new MC_121_IntDept_Course_DTO
                                        {
                                            cfilename = t.NMC121IDCF_FileName,
                                            cfilepath = t.NMC121IDCF_FilePath,
                                            cfiledesc = t.NMC121IDCF_FileDesc,
                                            NMC121IDCF_Id = t.NMC121IDCF_Id,
                                            NMC121IDC_Id = b.NMC121IDC_Id,
                                            MI_Id = b.MI_Id,
                                            NMC121IDCF_StatusFlg = t.NMC121IDCF_StatusFlg,
                                            NMC121IDCF_ApprovedFlg = t.NMC121IDCF_ApprovedFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }

        //add row wise comments
        public MC_121_IntDept_Course_DTO savemedicaldatawisecomments(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                NAAC_MC_121_IntDept_Course_Comments_DMO obj1 = new NAAC_MC_121_IntDept_Course_Comments_DMO();

                obj1.NMC121IDCC_Id = data.NMC121IDCC_Id;
                obj1.NMC121IDCC_Remarks = data.Remarks;
                obj1.NMC121IDCC_RemarksBy = data.UserId;
                obj1.NMC121IDCC_StatusFlg = "";
                obj1.NMC121IDC_Id = data.filefkid;
                obj1.NMC121IDCC_ActiveFlag = true;
                obj1.NMC121IDCC_CreatedBy = data.UserId;
                obj1.NMC121IDCC_UpdatedBy = data.UserId;
                obj1.NMC121IDCC_CreatedDate = DateTime.Now;
                obj1.NMC121IDCC_UpdatedDate = DateTime.Now;
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
        public MC_121_IntDept_Course_DTO savefilewisecomments(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                NAAC_MC_121_IntDept_Course_File_Comments_DMO obj1 = new NAAC_MC_121_IntDept_Course_File_Comments_DMO();

                obj1.NMC121IDCFC_Remarks = data.Remarks;
                obj1.NMC121IDCFC_RemarksBy = data.UserId;
                obj1.NMC121IDCFC_StatusFlg = "";
                obj1.NMC121IDCF_Id = data.filefkid;
                obj1.NMC121IDCFC_ActiveFlag = true;
                obj1.NMC121IDCFC_CreatedBy = data.UserId;
                obj1.NMC121IDCFC_UpdatedBy = data.UserId;
                obj1.NMC121IDCFC_CreatedDate = DateTime.Now;
                obj1.NMC121IDCFC_UpdatedDate = DateTime.Now;

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
        public MC_121_IntDept_Course_DTO getcomment(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_MC_121_IntDept_Course_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NMC121IDCC_RemarksBy == b.Id && a.NMC121IDC_Id == data.NMC121IDC_Id)
                                    select new MC_121_IntDept_Course_DTO
                                    {
                                        NMC121IDCC_Remarks = a.NMC121IDCC_Remarks,
                                        NMC121IDC_Id = a.NMC121IDC_Id,
                                        NMC121IDCC_Id = a.NMC121IDCC_Id,
                                        NMC121IDCC_RemarksBy = a.NMC121IDCC_RemarksBy,
                                        NMC121IDCC_StatusFlg = a.NMC121IDCC_StatusFlg,
                                        NMC121IDCC_ActiveFlag = a.NMC121IDCC_ActiveFlag,
                                        NMC121IDCC_CreatedBy = a.NMC121IDCC_CreatedBy,
                                        NMC121IDCC_CreatedDate = a.NMC121IDCC_CreatedDate,
                                        NMC121IDCC_UpdatedBy = a.NMC121IDCC_UpdatedBy,
                                        NMC121IDCC_UpdatedDate = a.NMC121IDCC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NMC121IDCC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // view file wise comments
        public MC_121_IntDept_Course_DTO getfilecomment(MC_121_IntDept_Course_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_MC_121_IntDept_Course_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NMC121IDCFC_RemarksBy == b.Id && a.NMC121IDCF_Id == data.NMC121IDCF_Id)
                                     select new MC_121_IntDept_Course_DTO
                                     {
                                         NMC121IDCF_Id = a.NMC121IDCF_Id,
                                         NMC121IDCFC_Remarks = a.NMC121IDCFC_Remarks,
                                         NMC121IDCFC_Id = a.NMC121IDCFC_Id,
                                         NMC121IDCFC_RemarksBy = a.NMC121IDCFC_RemarksBy,
                                         NMC121IDCFC_StatusFlg = a.NMC121IDCFC_StatusFlg,
                                         NMC121IDCFC_ActiveFlag = a.NMC121IDCFC_ActiveFlag,
                                         NMC121IDCFC_CreatedBy = a.NMC121IDCFC_CreatedBy,
                                         NMC121IDCFC_CreatedDate = a.NMC121IDCFC_CreatedDate,
                                         NMC121IDCFC_UpdatedBy = a.NMC121IDCFC_UpdatedBy,
                                         NMC121IDCFC_UpdatedDate = a.NMC121IDCFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NMC121IDCFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
