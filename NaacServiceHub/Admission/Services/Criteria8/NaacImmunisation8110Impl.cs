using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission.Criteria8;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria8
{
    public class NaacImmunisation8110Impl:Interface.Criteria8.NaacImmunisation8110Interface
    {


        public GeneralContext _context;
        public NaacImmunisation8110Impl(GeneralContext y)
        {
            _context = y;
        }

        public NAAC_MC_8110_Immunisation_DTO loaddata(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                var institutionlist = (from a in _context.Institution
                                       from b in _context.UserRoleWithInstituteDMO
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

                data.allacademicyear = _context.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToArray();
                data.alldata1 = (from a in _context.Academic
                                 from b in _context.NAAC_MC_8110_Immunisation_DMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == a.MI_Id && a.Is_Active == true && b.NCMC8110IMM_Year == a.ASMAY_Id)
                                 select new NAAC_MC_8110_Immunisation_DTO
                                 {
                                     NCMC8110IMM_Id = b.NCMC8110IMM_Id,
                                     NCMC8110IMM_Year = b.NCMC8110IMM_Year,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMC8110IMM_NoOfAdmStudents = b.NCMC8110IMM_NoOfAdmStudents,
                                     NCMC8110IMM_NoOfImmuStudents = b.NCMC8110IMM_NoOfImmuStudents,
                                     NCMC8110IMM_ActiveFlg = b.NCMC8110IMM_ActiveFlg,
                                     MI_Id=data.MI_Id

                                 }).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_MC_8110_Immunisation_DTO save(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                if (data.NCMC8110IMM_Id == 0)
                {
                    var duplicate = _context.NAAC_MC_8110_Immunisation_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC8110IMM_Year == data.asmaY_Id && t.NCMC8110IMM_Id != 0 && t.NCMC8110IMM_NoOfAdmStudents == data.NCMC8110IMM_NoOfAdmStudents&&t.NCMC8110IMM_NoOfImmuStudents==data.NCMC8110IMM_NoOfImmuStudents).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_8110_Immunisation_DMO rrr = new NAAC_MC_8110_Immunisation_DMO();
                        rrr.MI_Id = data.MI_Id;
                        rrr.NCMC8110IMM_NoOfAdmStudents = data.NCMC8110IMM_NoOfAdmStudents;
                        rrr.NCMC8110IMM_NoOfImmuStudents = data.NCMC8110IMM_NoOfImmuStudents;

                        rrr.NCMC8110IMM_Year = data.asmaY_Id;
                        rrr.NCMC8110IMM_CreatedDate = DateTime.Now;
                        rrr.NCMC8110IMM_UpdatedDate = DateTime.Now;
                        rrr.NCMC8110IMM_ActiveFlg = true;
                        rrr.NCMC8110IMM_CreatedBy = data.UserId;
                        rrr.NCMC8110IMM_UpdatedBy = data.UserId;
                        _context.Add(rrr);
                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {

                                
                                NAAC_MC_8110_Immunisation_Files_DMO obj2 = new NAAC_MC_8110_Immunisation_Files_DMO();
                                obj2.NCMC8110IMM_Id = rrr.NCMC8110IMM_Id;
                                obj2.MI_Id = data.MI_Id;
                                obj2.NCMC8110IMMF_FileName = data.filelist[i].cfilename;
                                obj2.NCMC8110IMMF_FileDesc = data.filelist[i].cfiledesc;
                                obj2.NCMC8110IMMF_FilePath = data.filelist[i].cfilepath;
                                obj2.NCMC8110IMMF_CreatedBy = data.UserId;
                                obj2.NCMC8110IMMF_UpdatedBy = data.UserId;
                                obj2.NCMC8110IMMF_CreatedDate = DateTime.Now;
                                obj2.NCMC8110IMMF_UpdatedDate = DateTime.Now;
                                //obj2.NCMC8110IMMF_StatusFlg = "";
                                obj2.NCMC8110IMMF_ActiveFlg = true;
                                _context.Add(obj2);
                                }
                            }
                        }

                        int y = _context.SaveChanges();
                        if (y > 0)
                        {
                            data.msg = "saved";
                        }
                        else
                        {
                            data.msg = "Failed";
                        }
                    }
                }
                else if (data.NCMC8110IMM_Id > 0)
                {
                    var duplicate = _context.NAAC_MC_8110_Immunisation_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC8110IMM_NoOfAdmStudents == data.NCMC8110IMM_NoOfAdmStudents&&t.NCMC8110IMM_NoOfImmuStudents==data.NCMC8110IMM_NoOfImmuStudents && t.NCMC8110IMM_Year == data.asmaY_Id && t.NCMC8110IMM_Id != data.NCMC8110IMM_Id).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var yy = _context.NAAC_MC_8110_Immunisation_DMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC8110IMM_Id == data.NCMC8110IMM_Id).SingleOrDefault();

                        yy.NCMC8110IMM_UpdatedBy = data.UserId;
                        yy.NCMC8110IMM_NoOfAdmStudents = data.NCMC8110IMM_NoOfAdmStudents;
                        yy.NCMC8110IMM_NoOfImmuStudents = data.NCMC8110IMM_NoOfImmuStudents;
                        yy.NCMC8110IMM_Year = data.asmaY_Id;
                        yy.MI_Id = data.MI_Id;
                        yy.NCMC8110IMM_UpdatedDate = DateTime.Now;
                        _context.Update(yy);

                        var CountRemoveFiles = _context.NAAC_MC_8110_Immunisation_Files_DMO.Where(t => t.NCMC8110IMM_Id == data.NCMC8110IMM_Id).ToList();
                        if (CountRemoveFiles.Count > 0)
                        {
                            foreach (var RemoveFiles in CountRemoveFiles)
                            {
                                _context.Remove(RemoveFiles);
                            }
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {

                                   
                                    NAAC_MC_8110_Immunisation_Files_DMO obj2 = new NAAC_MC_8110_Immunisation_Files_DMO();
                                    obj2.NCMC8110IMM_Id = yy.NCMC8110IMM_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.NCMC8110IMMF_FileName = data.filelist[i].cfilename;
                                    obj2.NCMC8110IMMF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCMC8110IMMF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCMC8110IMMF_CreatedBy = data.UserId;
                                    obj2.NCMC8110IMMF_UpdatedBy = data.UserId;
                                    obj2.NCMC8110IMMF_ActiveFlg = true;
                                   // obj2.NCMC8110IMMF_StatusFlg ="";
                                    obj2.NCMC8110IMMF_CreatedDate = DateTime.Now;
                                    obj2.NCMC8110IMMF_UpdatedDate = DateTime.Now;
                                    _context.Add(obj2);
                                    }
                                }
                            }
                        }
                        else if (CountRemoveFiles.Count == 0)
                        {
                            if (data.filelist.Length > 0)
                            {
                                for (int i = 0; i < data.filelist.Length; i++)
                                {
                                    if (data.filelist[0].cfilepath != null)
                                    {

                                   
                                    NAAC_MC_8110_Immunisation_Files_DMO obj2 = new NAAC_MC_8110_Immunisation_Files_DMO();
                                    obj2.NCMC8110IMM_Id = yy.NCMC8110IMM_Id;
                                    obj2.MI_Id = data.MI_Id;
                                    obj2.NCMC8110IMMF_FileName = data.filelist[i].cfilename;
                                    obj2.NCMC8110IMMF_FileDesc = data.filelist[i].cfiledesc;
                                    obj2.NCMC8110IMMF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCMC8110IMMF_CreatedBy = data.UserId;
                                    obj2.NCMC8110IMMF_UpdatedBy = data.UserId;
                                       // obj2.NCMC8110IMMF_StatusFlg = "";
                                        obj2.NCMC8110IMMF_ActiveFlg = true;
                                    obj2.NCMC8110IMMF_CreatedDate = DateTime.Now;
                                    obj2.NCMC8110IMMF_UpdatedDate = DateTime.Now;
                                    _context.Add(obj2);
                                    }
                                }
                            }
                        }
                        
                        var r = _context.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
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
        public NAAC_MC_8110_Immunisation_DTO deactive(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                var u = _context.NAAC_MC_8110_Immunisation_DMO.Where(t => t.NCMC8110IMM_Id == data.NCMC8110IMM_Id).SingleOrDefault();
                if (u.NCMC8110IMM_ActiveFlg == true)
                {
                    u.NCMC8110IMM_ActiveFlg = false;
                }
                else if (u.NCMC8110IMM_ActiveFlg == false)
                {
                    u.NCMC8110IMM_ActiveFlg = true;
                }
                u.NCMC8110IMM_UpdatedDate = DateTime.Now;
                u.NCMC8110IMM_UpdatedBy = data.UserId;
                u.MI_Id = data.MI_Id;
                _context.Update(u);
                int o = _context.SaveChanges();
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
        public NAAC_MC_8110_Immunisation_DTO EditData(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                data.editlist = (from a in _context.Academic
                                 from b in _context.NAAC_MC_8110_Immunisation_DMO
                                 where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.NCMC8110IMM_Year && b.MI_Id == data.MI_Id && b.NCMC8110IMM_Id == data.NCMC8110IMM_Id)
                                 select new NAAC_MC_8110_Immunisation_DTO
                                 {
                                     NCMC8110IMM_Id = b.NCMC8110IMM_Id,
                                     NCMC8110IMM_NoOfAdmStudents = b.NCMC8110IMM_NoOfAdmStudents,
                                     NCMC8110IMM_NoOfImmuStudents = b.NCMC8110IMM_NoOfImmuStudents,
                                     NCMC8110IMM_Year = b.NCMC8110IMM_Year,
                                     MI_Id=data.MI_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     NCMC8110IMM_StatusFlg = b.NCMC8110IMM_StatusFlg
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _context.NAAC_MC_8110_Immunisation_Files_DMO
                                      where (a.NCMC8110IMM_Id == data.NCMC8110IMM_Id)
                                      select new NAAC_MC_8110_Immunisation_DTO
                                      {
                                          cfilename = a.NCMC8110IMMF_FileName,
                                          cfilepath = a.NCMC8110IMMF_FilePath,
                                          cfiledesc = a.NCMC8110IMMF_FileDesc,
                                      }).Distinct().ToArray();
            }
            catch (Exception d)
            {
                Console.WriteLine(d.Message);
            }
            return data;
        }

        public NAAC_MC_8110_Immunisation_DTO viewuploadflies(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                data.viewuploadflies = (from a in _context.NAAC_MC_8110_Immunisation_Files_DMO
                                        where (a.NCMC8110IMM_Id == data.NCMC8110IMM_Id && a.NCMC8110IMMF_ActiveFlg==true)
                                        select new NAAC_MC_8110_Immunisation_DTO
                                        {
                                            cfilename = a.NCMC8110IMMF_FileName,
                                            cfilepath = a.NCMC8110IMMF_FilePath,
                                            cfiledesc = a.NCMC8110IMMF_FileDesc,
                                            NCMC8110IMMF_Id = a.NCMC8110IMMF_Id,
                                            NCMC8110IMM_Id = a.NCMC8110IMM_Id,
                                        }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;

        }
        public NAAC_MC_8110_Immunisation_DTO deleteuploadfile(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                var res = _context.NAAC_MC_8110_Immunisation_Files_DMO.Where(t => t.NCMC8110IMMF_Id == data.NCMC8110IMMF_Id).SingleOrDefault();
                //_context.Remove(res);
                res.NCMC8110IMMF_ActiveFlg = false;
                _context.Update(res);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from a in _context.NAAC_MC_8110_Immunisation_Files_DMO
                                        where (a.NCMC8110IMM_Id == data.NCMC8110IMM_Id && a.NCMC8110IMMF_ActiveFlg==true)
                                        select new NAAC_MC_8110_Immunisation_DTO
                                        {
                                            NCMC8110IMM_Id = a.NCMC8110IMM_Id,
                                            NCMC8110IMMF_Id = a.NCMC8110IMMF_Id,
                                            cfilename = a.NCMC8110IMMF_FileName,
                                            cfilepath = a.NCMC8110IMMF_FilePath,
                                            cfiledesc = a.NCMC8110IMMF_FileDesc,


                                        }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_MC_8110_Immunisation_DTO getcomment(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                data.commentlist = (from a in _context.NAAC_MC_8110_Immunisation_CommentsDMO
                                    from b in _context.ApplUser
                                    where (a.NCMC8110IMMC_RemarksBy == b.Id && a.NCMC8110IMM_Id == data.NCMC8110IMM_Id)
                                    select new NAAC_MC_8110_Immunisation_DTO
                                    {

                                        NCMC8110IMMC_Remarks = a.NCMC8110IMMC_Remarks,
                                        NCMC8110IMM_Id = a.NCMC8110IMM_Id,
                                        NCMC8110IMMC_RemarksBy = a.NCMC8110IMMC_RemarksBy,
                                        NCMC8110IMMC_StatusFlg = a.NCMC8110IMMC_StatusFlg,
                                        NCMC8110IMMC_ActiveFlag = a.NCMC8110IMMC_ActiveFlag,
                                        NCMC8110IMMC_CreatedBy = a.NCMC8110IMMC_CreatedBy,
                                        NCMC8110IMMC_CreatedDate = a.NCMC8110IMMC_CreatedDate,
                                        NCMC8110IMMC_UpdatedBy = a.NCMC8110IMMC_UpdatedBy,
                                        NCMC8110IMMC_UpdatedDate = a.NCMC8110IMMC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_MC_8110_Immunisation_DTO getfilecomment(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _context.NAAC_MC_8110_Immunisation_File_CommentsDMO
                                     from b in _context.ApplUser
                                     where (a.NCMC8110IMMFC_RemarksBy == b.Id && a.NCMC8110IMMF_Id == data.NCMC8110IMMF_Id)
                                     select new NAAC_MC_8110_Immunisation_DTO
                                     {
                                         NCMC8110IMMF_Id = a.NCMC8110IMMF_Id,
                                         NCMC8110IMMFC_Remarks = a.NCMC8110IMMFC_Remarks,
                                         NCMC8110IMMFC_Id = a.NCMC8110IMMFC_Id,
                                         NCMC8110IMMFC_RemarksBy = a.NCMC8110IMMFC_RemarksBy,
                                         NCMC8110IMMFC_StatusFlg = a.NCMC8110IMMFC_StatusFlg,
                                         NCMC8110IMMFC_ActiveFlag = a.NCMC8110IMMFC_ActiveFlag,
                                         NCMC8110IMMFC_CreatedBy = a.NCMC8110IMMFC_CreatedBy,
                                         NCMC8110IMMFC_CreatedDate = a.NCMC8110IMMFC_CreatedDate,
                                         NCMC8110IMMFC_UpdatedBy = a.NCMC8110IMMFC_UpdatedBy,
                                         NCMC8110IMMFC_UpdatedDate = a.NCMC8110IMMFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public NAAC_MC_8110_Immunisation_DTO savecomments(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                NAAC_MC_8110_Immunisation_CommentsDMO obj1 = new NAAC_MC_8110_Immunisation_CommentsDMO();
                obj1.NCMC8110IMMC_Remarks = data.Remarks;
                obj1.NCMC8110IMMC_RemarksBy = data.UserId;
                obj1.NCMC8110IMMC_StatusFlg = "";
                obj1.NCMC8110IMM_Id = data.filefkid;
                obj1.NCMC8110IMMC_ActiveFlag = true;
                obj1.NCMC8110IMMC_CreatedBy = data.UserId;
                obj1.NCMC8110IMMC_UpdatedBy = data.UserId;
                obj1.NCMC8110IMMC_CreatedDate = DateTime.Now;
                obj1.NCMC8110IMMC_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public NAAC_MC_8110_Immunisation_DTO savefilewisecomments(NAAC_MC_8110_Immunisation_DTO data)
        {
            try
            {
                NAAC_MC_8110_Immunisation_File_CommentsDMO obj1 = new NAAC_MC_8110_Immunisation_File_CommentsDMO();
                obj1.NCMC8110IMMFC_Remarks = data.Remarks;
                obj1.NCMC8110IMMFC_RemarksBy = data.UserId;
                obj1.NCMC8110IMMFC_StatusFlg = "";
                obj1.NCMC8110IMMF_Id = data.filefkid;
                obj1.NCMC8110IMMFC_ActiveFlag = true;
                obj1.NCMC8110IMMFC_CreatedBy = data.UserId;
                obj1.NCMC8110IMMFC_UpdatedBy = data.UserId;
                obj1.NCMC8110IMMFC_CreatedDate = DateTime.Now;
                obj1.NCMC8110IMMFC_UpdatedDate = DateTime.Now;
                _context.Add(obj1);
                int s = _context.SaveChanges();
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
    }
}
