using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class NAAC_EContent_434_Impl : Interface.NAAC_EContent_434_Interface
    {
        public GeneralContext _GeneralContext;
        public NAAC_EContent_434_Impl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public NAAC_AC_434_EContent_DTO loaddata(NAAC_AC_434_EContent_DTO data)
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
                data.allgridlist = _GeneralContext.NAAC_AC_434_EContent_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_434_EContent_DTO savedata(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                if (data.NCAC434ECT_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_434_EContent_DMO.Where(t => t.NCAC434ECT_DevFacilityName == data.NCAC434ECT_DevFacilityName && t.NCAC434ECT_LinkName == data.NCAC434ECT_LinkName && t.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_434_EContent_DMO obj1 = new NAAC_AC_434_EContent_DMO();

                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC434ECT_DevFacilityName = data.NCAC434ECT_DevFacilityName;
                        obj1.NCAC434ECT_LinkName = data.NCAC434ECT_LinkName;
                        obj1.NCAC434ECT_ActiveFlg = true;
                        obj1.NCAC434ECT_CreatedBy = data.UserId;
                        obj1.NCAC434ECT_UpdatedBy = data.UserId;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.NCAC434ECT_StatusFlg = "";
                        obj1.MI_Id = data.MI_Id;
                        _GeneralContext.Add(obj1);

                        if (data.filelist.Length > 0)
                        {
                            for (int i = 0; i < data.filelist.Length; i++)
                            {
                                if (data.filelist[i].cfilepath != null)
                                {
                                    NAAC_AC_434_EContent_Files_DMO obj2 = new NAAC_AC_434_EContent_Files_DMO();

                                    obj2.NCAC434ECTF_FileName = data.filelist[i].cfilename;
                                    obj2.NCAC434ECTF_Filedesc = data.filelist[i].cfiledesc;
                                    obj2.NCAC434ECTF_FilePath = data.filelist[i].cfilepath;
                                    obj2.NCAC434ECT_Id = obj1.NCAC434ECT_Id;
                                    obj2.NCAC434ECTF_StatusFlg = "";
                                    obj2.NCAC434ECTF_ActiveFlg = true;

                                    _GeneralContext.Add(obj2);
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
                else if (data.NCAC434ECT_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_434_EContent_DMO.Where(t => t.NCAC434ECT_Id != data.NCAC434ECT_Id && t.NCAC434ECT_DevFacilityName == data.NCAC434ECT_DevFacilityName && t.NCAC434ECT_LinkName == data.NCAC434ECT_LinkName && t.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_434_EContent_DMO.Where(t => t.NCAC434ECT_Id == data.NCAC434ECT_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCAC434ECT_DevFacilityName = data.NCAC434ECT_DevFacilityName;
                        update.NCAC434ECT_LinkName = data.NCAC434ECT_LinkName;
                        update.NCAC434ECT_UpdatedBy = data.UserId;
                        update.MI_Id = data.MI_Id;
                        update.UpdatedDate = DateTime.Now;

                        _GeneralContext.Update(update);

                        
                        if (data.filelist.Length > 0)
                        {
                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC434ECTF_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_434_EContent_Files_DMO.Where(t => t.NCAC434ECT_Id == data.NCAC434ECT_Id && !Fid.Contains(t.NCAC434ECTF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_434_EContent_Files_DMO.Single(t => t.NCAC434ECT_Id == data.NCAC434ECT_Id && t.NCAC434ECTF_Id == item2.NCAC434ECTF_Id);
                                    deactfile.NCAC434ECTF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);

                                }

                            }


                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_434_EContent_Files_DMO.Where(t => t.NCAC434ECT_Id == data.NCAC434ECT_Id && t.NCAC434ECTF_StatusFlg != "approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}

                            foreach (NAAC_AC_434_EContent_DTO DocumentsDTO in data.filelist)
                            {

                                if (DocumentsDTO.NCAC434ECTF_Id > 0 && DocumentsDTO.NCAC434ECTF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_434_EContent_Files_DMO.Where(t => t.NCAC434ECTF_Id == DocumentsDTO.NCAC434ECTF_Id).FirstOrDefault();
                                        filesdata.NCAC434ECTF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC434ECTF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC434ECTF_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);
                                        
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC434ECTF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_434_EContent_Files_DMO obj2 = new NAAC_AC_434_EContent_Files_DMO();
                                            obj2.NCAC434ECTF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC434ECTF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC434ECTF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC434ECTF_StatusFlg = "";
                                            obj2.NCAC434ECTF_ActiveFlg = true;
                                            obj2.NCAC434ECT_Id = data.NCAC434ECT_Id;
                                            _GeneralContext.Add(obj2);
                                           
                                        }
                                    }
                                }
                            }

                        }
                        int flag = _GeneralContext.SaveChanges();
                        if (flag > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_434_EContent_DTO editdata(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                data.editlist = _GeneralContext.NAAC_AC_434_EContent_DMO.Where(t => t.NCAC434ECT_Id == data.NCAC434ECT_Id && t.MI_Id == data.MI_Id).ToArray();
                data.editFileslist = (from t in _GeneralContext.NAAC_AC_434_EContent_Files_DMO
                                      //from b in _GeneralContext.NAAC_AC_434_EContent_DMO
                                      where (t.NCAC434ECT_Id == data.NCAC434ECT_Id&&t.NCAC434ECTF_ActiveFlg==true )
                                      select new NAAC_AC_434_EContent_DTO
                                      {
                                          cfilename = t.NCAC434ECTF_FileName,
                                          cfilepath = t.NCAC434ECTF_FilePath,
                                          cfiledesc = t.NCAC434ECTF_Filedesc,
                                          NCAC434ECTF_StatusFlg = t.NCAC434ECTF_StatusFlg,
                                          NCAC434ECTF_Id = t.NCAC434ECTF_Id,
                                          NCAC434ECT_Id = t.NCAC434ECT_Id,

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_434_EContent_DTO deactiveStudent(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_434_EContent_DMO.Where(t => t.NCAC434ECT_Id == data.NCAC434ECT_Id && t.MI_Id == data.MI_Id).Single();
                if (result.NCAC434ECT_ActiveFlg == true)
                {
                    result.NCAC434ECT_ActiveFlg = false;
                }
                else if (result.NCAC434ECT_ActiveFlg == false)
                {
                    result.NCAC434ECT_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.NCAC434ECT_UpdatedBy = data.UserId;
                result.MI_Id = data.MI_Id;
                _GeneralContext.Update(result);
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
        public NAAC_AC_434_EContent_DTO viewuploadflies(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_434_EContent_Files_DMO
                                    
                                        where (t.NCAC434ECT_Id == data.NCAC434ECT_Id&&t.NCAC434ECTF_ActiveFlg==true )
                                        select new NAAC_AC_434_EContent_DTO
                                        {
                                            cfilename = t.NCAC434ECTF_FileName,
                                            cfilepath = t.NCAC434ECTF_FilePath,
                                            cfiledesc = t.NCAC434ECTF_Filedesc,
                                            NCAC434ECTF_Id = t.NCAC434ECTF_Id,
                                            NCAC434ECT_Id = t.NCAC434ECT_Id,
                                            NCAC434ECTF_StatusFlg = t.NCAC434ECTF_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public NAAC_AC_434_EContent_DTO deleteuploadfile(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
        
                var result = _GeneralContext.NAAC_AC_434_EContent_Files_DMO.Where(t => t.NCAC434ECTF_Id == data.NCAC434ECTF_Id).SingleOrDefault();
                result.NCAC434ECTF_ActiveFlg = false;
                _GeneralContext.Update(result);

                int row = _GeneralContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_434_EContent_Files_DMO

                                        where (t.NCAC434ECT_Id == data.NCAC434ECT_Id&&t.NCAC434ECTF_ActiveFlg==true)
                                        select new NAAC_AC_434_EContent_DTO
                                        {
                                            cfilename = t.NCAC434ECTF_FileName,
                                            cfilepath = t.NCAC434ECTF_FilePath,
                                            cfiledesc = t.NCAC434ECTF_Filedesc,
                                            NCAC434ECTF_Id = t.NCAC434ECTF_Id,
                                            NCAC434ECT_Id = t.NCAC434ECT_Id,
                                            NCAC434ECTF_StatusFlg = t.NCAC434ECTF_StatusFlg,

                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }


        public NAAC_AC_434_EContent_DTO getcomment(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_434_EContent_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC434ECTC_RemarksBy == b.Id && a.NCAC434ECT_Id == data.NCAC434ECT_Id)
                                    select new NAAC_AC_434_EContent_DTO
                                    {
                                        NCAC434ECTC_Remarks = a.NCAC434ECTC_Remarks,
                                        NCAC434ECTC_Id = a.NCAC434ECTC_Id,
                                        NCAC434ECTC_RemarksBy = a.NCAC434ECTC_RemarksBy,
                                        NCAC434ECTC_StatusFlg = a.NCAC434ECTC_StatusFlg,
                                        NCAC434ECTC_ActiveFlag = a.NCAC434ECTC_ActiveFlag,
                                        NCAC434ECTC_CreatedBy = a.NCAC434ECTC_CreatedBy,
                                        NCAC434ECTC_CreatedDate = a.NCAC434ECTC_CreatedDate,
                                        NCAC434ECTC_UpdatedBy = a.NCAC434ECTC_UpdatedBy,
                                        NCAC434ECTC_UpdatedDate = a.NCAC434ECTC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC434ECTC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_434_EContent_DTO getfilecomment(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_434_EContent_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC434ECTFC_RemarksBy == b.Id && a.NCAC434ECTF_Id == data.NCAC434ECTF_Id)
                                     select new NAAC_AC_434_EContent_DTO
                                     {
                                         NCAC434ECTF_Id = a.NCAC434ECTF_Id,
                                         NCAC434ECTFC_Remarks = a.NCAC434ECTFC_Remarks,
                                         NCAC434ECTFC_Id = a.NCAC434ECTFC_Id,
                                         NCAC434ECTFC_RemarksBy = a.NCAC434ECTFC_RemarksBy,
                                         NCAC434ECTFC_StatusFlg = a.NCAC434ECTFC_StatusFlg,
                                         NCAC434ECTFC_ActiveFlag = a.NCAC434ECTFC_ActiveFlag,
                                         NCAC434ECTFC_CreatedBy = a.NCAC434ECTFC_CreatedBy,
                                         NCAC434ECTFC_CreatedDate = a.NCAC434ECTFC_CreatedDate,
                                         NCAC434ECTFC_UpdatedBy = a.NCAC434ECTFC_UpdatedBy,
                                         NCAC434ECTFC_UpdatedDate = a.NCAC434ECTFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC434ECTFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_434_EContent_DTO savemedicaldatawisecomments(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                NAAC_AC_434_EContent_Comments_DMO obj1 = new NAAC_AC_434_EContent_Comments_DMO();
                obj1.NCAC434ECTC_Remarks = data.Remarks;
                obj1.NCAC434ECTC_RemarksBy = data.UserId;
                obj1.NCAC434ECTC_StatusFlg = "";
                obj1.NCAC434ECT_Id = data.filefkid;
                obj1.NCAC434ECTC_ActiveFlag = true;
                obj1.NCAC434ECTC_CreatedBy = data.UserId;
                obj1.NCAC434ECTC_UpdatedBy = data.UserId;
                obj1.NCAC434ECTC_CreatedDate = DateTime.Now;
                obj1.NCAC434ECTC_UpdatedDate = DateTime.Now;
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

        // for file adding
        public NAAC_AC_434_EContent_DTO savefilewisecomments(NAAC_AC_434_EContent_DTO data)
        {
            try
            {
                NAAC_AC_434_EContent_File_Comments_DMO obj1 = new NAAC_AC_434_EContent_File_Comments_DMO();
                obj1.NCAC434ECTFC_Remarks = data.Remarks;
                obj1.NCAC434ECTFC_RemarksBy = data.UserId;
                obj1.NCAC434ECTFC_StatusFlg = "";
                obj1.NCAC434ECTF_Id = data.filefkid;
                obj1.NCAC434ECTFC_ActiveFlag = true;
                obj1.NCAC434ECTFC_CreatedBy = data.UserId;
                obj1.NCAC434ECTFC_UpdatedBy = data.UserId;
                obj1.NCAC434ECTFC_UpdatedDate = DateTime.Now;
                obj1.NCAC434ECTFC_CreatedDate = DateTime.Now;
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

    }
}
