using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services
{
    public class Naac_ICTImpl : Interface.Naac_ICTInterface
    {
        public GeneralContext _GeneralContext;
        public Naac_ICTImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public Naac_ICT_DTO loaddata(Naac_ICT_DTO data)
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
               
                data.allgridlist =_GeneralContext.NAAC_AC_413_ICT_DMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Naac_ICT_DTO savedata(Naac_ICT_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC413ICT_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_413_ICT_DMO.Where(t => t.NCAC413ICT_RoomNo == data.NCAC413ICT_RoomNo && t.NCAC413ICT_ICTFacility == data.NCAC413ICT_ICTFacility && t.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_413_ICT_DMO obj1 = new NAAC_AC_413_ICT_DMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC413ICT_RoomNo = data.NCAC413ICT_RoomNo;
                        obj1.NCAC413ICT_ICTFacility = data.NCAC413ICT_ICTFacility;                       
                        obj1.NCAC413ICT_ActiveFlg = true;
                        obj1.NCAC413ICT_CreatedBy = data.UserId;
                        obj1.NCAC413ICT_UpdatedBy = data.UserId;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.NCAC413ICT_StatusFlg = "";
                        obj1.UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[j].cfilepath != null)
                                {

                               
                                NAAC_AC_413_ICT_FilesDMO obj2 = new NAAC_AC_413_ICT_FilesDMO();                              
                                obj2.NCAC413ICTF_FileName = data.filelist[j].cfilename;
                                obj2.NCAC413ICTF_Filedesc = data.filelist[j].cfiledesc;
                                obj2.NCAC413ICTF_FilePath = data.filelist[j].cfilepath;                                   
                                obj2.NCAC413ICT_Id = obj1.NCAC413ICT_Id;
                                    obj2.NCAC413ICTF_ActiveFlg = true;
                                    obj2.NCAC413ICTF_StatusFlg = "";

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
                else if (data.NCAC413ICT_Id > 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_413_ICT_DMO.Where(t => t.NCAC413ICT_Id != data.NCAC413ICT_Id && t.NCAC413ICT_RoomNo == data.NCAC413ICT_RoomNo && t.NCAC413ICT_ICTFacility == data.NCAC413ICT_ICTFacility && t.MI_Id == data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _GeneralContext.NAAC_AC_413_ICT_DMO.Where(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && t.MI_Id == data.MI_Id).Single();

                        update.NCAC413ICT_RoomNo = data.NCAC413ICT_RoomNo;
                        update.NCAC413ICT_ICTFacility = data.NCAC413ICT_ICTFacility;                       
                        update.NCAC413ICT_UpdatedBy = data.UserId;

                        update.UpdatedDate = DateTime.Now;
                        update.MI_Id = data.MI_Id;
                        _GeneralContext.Update(update);

                        
                        if (data.filelist.Count() > 0)
                        {

                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCAC413ICTF_Id);
                            }
                            var removefile11 = _GeneralContext.NAAC_AC_413_ICT_FilesDMO.Where(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && !Fid.Contains(t.NCAC413ICTF_Id)).Distinct().ToList();

                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.NAAC_AC_413_ICT_FilesDMO.Single(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && t.NCAC413ICTF_Id == item2.NCAC413ICTF_Id);
                                    deactfile.NCAC413ICTF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);

                                }

                            }

                            //var CountRemoveFiles = _GeneralContext.NAAC_AC_413_ICT_FilesDMO.Where(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && t.NCAC413ICTF_StatusFlg != "approved").ToList();
                            //if (CountRemoveFiles.Count > 0)
                            //{
                            //    foreach (var RemoveFiles in CountRemoveFiles)
                            //    {
                            //        _GeneralContext.Remove(RemoveFiles);
                            //    }
                            //}

                            foreach (Naac_ICT_DTO DocumentsDTO in data.filelist)
                            {
                                



                                if (DocumentsDTO.NCAC413ICTF_Id > 0 && DocumentsDTO.NCAC413ICTF_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.NAAC_AC_413_ICT_FilesDMO.Where(t => t.NCAC413ICTF_Id == DocumentsDTO.NCAC413ICTF_Id).FirstOrDefault();
                                        filesdata.NCAC413ICTF_Filedesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCAC413ICTF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCAC413ICTF_FilePath = DocumentsDTO.cfilepath;
                                        

                                        _GeneralContext.Update(filesdata);
                                       
                                       
                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCAC413ICTF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            NAAC_AC_413_ICT_FilesDMO obj2 = new NAAC_AC_413_ICT_FilesDMO();
                                            obj2.NCAC413ICTF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCAC413ICTF_Filedesc = DocumentsDTO.cfiledesc;
                                            obj2.NCAC413ICTF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCAC413ICTF_StatusFlg = "";
                                            obj2.NCAC413ICTF_ActiveFlg = true;
                                            obj2.NCAC413ICT_Id = data.NCAC413ICT_Id;
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
        public Naac_ICT_DTO editdata(Naac_ICT_DTO data)
        {
            try
            {
                data.editlist = _GeneralContext.NAAC_AC_413_ICT_DMO.Where(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && t.MI_Id == data.MI_Id).ToArray();

                data.editFileslist = (from a in _GeneralContext.NAAC_AC_413_ICT_FilesDMO
                                      where (a.NCAC413ICT_Id == data.NCAC413ICT_Id&&a.NCAC413ICTF_ActiveFlg==true)
                                      select new Naac_ICT_DTO
                                      {
                                          cfilename = a.NCAC413ICTF_FileName,
                                          cfilepath = a.NCAC413ICTF_FilePath,
                                          cfiledesc = a.NCAC413ICTF_Filedesc,
                                          NCAC413ICTF_Id = a.NCAC413ICTF_Id,
                                          NCAC413ICT_Id = a.NCAC413ICT_Id,
                                          NCAC413ICTF_StatusFlg = a.NCAC413ICTF_StatusFlg,

                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Naac_ICT_DTO deactivRow(Naac_ICT_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_413_ICT_DMO.Where(t => t.NCAC413ICT_Id == data.NCAC413ICT_Id && t.MI_Id == data.MI_Id).Single();
                if (result.NCAC413ICT_ActiveFlg==true)
                {
                    result.NCAC413ICT_ActiveFlg = false;
                }
                else if (result.NCAC413ICT_ActiveFlg==false)
                {
                    result.NCAC413ICT_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.NCAC413ICT_UpdatedBy = data.UserId;
                result.MI_Id = data.MI_Id;
                _GeneralContext.Update(result);
                int row=_GeneralContext.SaveChanges();
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

        public Naac_ICT_DTO getcomment(Naac_ICT_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_413_ICT_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC413ICTC_RemarksBy == b.Id && a.NCAC413ICT_Id == data.NCAC413ICT_Id)
                                    select new Naac_ICT_DTO
                                    {
                                        NCAC413ICTC_Remarks = a.NCAC413ICTC_Remarks,
                                        NCAC413ICTC_Id = a.NCAC413ICTC_Id,
                                        NCAC413ICTC_RemarksBy = a.NCAC413ICTC_RemarksBy,
                                        NCAC413ICTC_StatusFlg = a.NCAC413ICTC_StatusFlg,
                                        NCAC413ICTC_ActiveFlag = a.NCAC413ICTC_ActiveFlag,
                                        NCAC413ICTC_CreatedBy = a.NCAC413ICTC_CreatedBy,
                                        NCAC413ICTC_CreatedDate = a.NCAC413ICTC_CreatedDate,
                                        NCAC413ICTC_UpdatedBy = a.NCAC413ICTC_UpdatedBy,
                                        NCAC413ICTC_UpdatedDate = a.NCAC413ICTC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a=>a.NCAC413ICTC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public Naac_ICT_DTO getfilecomment(Naac_ICT_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_413_ICT_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC413ICTFC_RemarksBy == b.Id && a.NCAC413ICTF_Id == data.NCAC413ICTF_Id)
                                     select new Naac_ICT_DTO
                                     {
                                         NCAC413ICTF_Id = a.NCAC413ICTF_Id,
                                         NCAC413ICTFC_Remarks = a.NCAC413ICTFC_Remarks,
                                         NCAC413ICTFC_Id = a.NCAC413ICTFC_Id,
                                         NCAC413ICTFC_RemarksBy = a.NCAC413ICTFC_RemarksBy,
                                         NCAC413ICTFC_StatusFlg = a.NCAC413ICTFC_StatusFlg,
                                         NCAC413ICTFC_ActiveFlag = a.NCAC413ICTFC_ActiveFlag,
                                         NCAC413ICTFC_CreatedBy = a.NCAC413ICTFC_CreatedBy,
                                         NCAC413ICTFC_CreatedDate = a.NCAC413ICTFC_CreatedDate,
                                         NCAC413ICTFC_UpdatedBy = a.NCAC413ICTFC_UpdatedBy,
                                         NCAC413ICTFC_UpdatedDate = a.NCAC413ICTFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC413ICTFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Naac_ICT_DTO savemedicaldatawisecomments(Naac_ICT_DTO data)
        {
            try
            {
                NAAC_AC_413_ICT_Comments_DMO obj1 = new NAAC_AC_413_ICT_Comments_DMO();
                obj1.NCAC413ICTC_Remarks = data.Remarks;
                obj1.NCAC413ICTC_RemarksBy = data.UserId;
                obj1.NCAC413ICTC_StatusFlg = "";
                obj1.NCAC413ICT_Id = data.filefkid;
                obj1.NCAC413ICTC_ActiveFlag = true;
                obj1.NCAC413ICTC_CreatedBy = data.UserId;
                obj1.NCAC413ICTC_UpdatedBy = data.UserId;
                obj1.NCAC413ICTC_CreatedDate = DateTime.Now;
                obj1.NCAC413ICTC_UpdatedDate = DateTime.Now;
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
        public Naac_ICT_DTO savefilewisecomments(Naac_ICT_DTO data)
        {
            try
            {
                NAAC_AC_413_ICT_File_Comments_DMO obj1 = new NAAC_AC_413_ICT_File_Comments_DMO();
                obj1.NCAC413ICTFC_Remarks = data.Remarks;
                obj1.NCAC413ICTFC_RemarksBy = data.UserId;
                obj1.NCAC413ICTFC_StatusFlg = "";
                obj1.NCAC413ICTF_Id = data.filefkid;
                obj1.NCAC413ICTFC_ActiveFlag = true;
                obj1.NCAC413ICTFC_CreatedBy = data.UserId;
                obj1.NCAC413ICTFC_UpdatedBy = data.UserId;
                obj1.NCAC413ICTFC_UpdatedDate = DateTime.Now;
                obj1.NCAC413ICTFC_CreatedDate = DateTime.Now;
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

        public Naac_ICT_DTO viewuploadflies(Naac_ICT_DTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_413_ICT_FilesDMO
                                       
                                        where (t.NCAC413ICT_Id == data.NCAC413ICT_Id&&t.NCAC413ICTF_ActiveFlg==true )
                                        select new Naac_ICT_DTO
                                        {
                                            cfilename = t.NCAC413ICTF_FileName,
                                            cfilepath = t.NCAC413ICTF_FilePath,
                                            cfiledesc = t.NCAC413ICTF_Filedesc,
                                            NCAC413ICTF_Id = t.NCAC413ICTF_Id,
                                            NCAC413ICT_Id = t.NCAC413ICT_Id,
                                            NCAC413ICTF_StatusFlg = t.NCAC413ICTF_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public Naac_ICT_DTO deleteuploadfile(Naac_ICT_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_413_ICT_FilesDMO.Where(t => t.NCAC413ICTF_Id == data.NCAC413ICTF_Id).SingleOrDefault();
                result.NCAC413ICTF_ActiveFlg = false;
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

                //if (result.Count > 0)
                //{
                //    foreach (var resultid in result)
                //    {
                //        _GeneralContext.Remove(resultid);
                //    }
                //}
                //int row = _GeneralContext.SaveChanges();
                //if (row > 0)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
                data.viewuploadflies = (from t in _GeneralContext.NAAC_AC_413_ICT_FilesDMO

                                        where (t.NCAC413ICT_Id == data.NCAC413ICT_Id&&t.NCAC413ICTF_ActiveFlg==true)
                                        select new Naac_ICT_DTO
                                        {
                                            cfilename = t.NCAC413ICTF_FileName,
                                            cfilepath = t.NCAC413ICTF_FilePath,
                                            cfiledesc = t.NCAC413ICTF_Filedesc,
                                            NCAC413ICTF_Id = t.NCAC413ICTF_Id,
                                            NCAC413ICT_Id = t.NCAC413ICT_Id,
                                            NCAC413ICTF_StatusFlg = t.NCAC413ICTF_StatusFlg,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
