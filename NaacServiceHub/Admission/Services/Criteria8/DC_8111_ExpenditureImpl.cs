using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission.Criteria8;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Services.Criteria8
{
    public class DC_8111_ExpenditureImpl : Interface.Criteria8.DC_8111_ExpenditureInterface
    {
        public GeneralContext _GeneralContext;
        public DC_8111_ExpenditureImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }
        public async Task<DC_8111_ExpenditureDTO> loaddata(DC_8111_ExpenditureDTO data)
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
                                 select new NAAC_811MC_NEET_DTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();
                data.alldata = (from y in _GeneralContext.Academic
                                from b in _GeneralContext.DC_8111_ExpenditureDMO
                                where (y.ASMAY_Id == b.NCDC8111E_Year && b.MI_Id == data.MI_Id)
                                select new DC_8111_ExpenditureDTO
                                {
                                    NCDC8111E_Id = b.NCDC8111E_Id,
                                    NCDC8111E_Year = b.NCDC8111E_Year,
                                    ASMAY_Year = y.ASMAY_Year,
                                    MI_Id = data.MI_Id,
                                    NCDC8111E_DentalMaterialsName = b.NCDC8111E_DentalMaterialsName,
                                    NCDC8111E_Expenditure = b.NCDC8111E_Expenditure,

                                    NCDC8111E_ActiveFlag = b.NCDC8111E_ActiveFlag,
                                }).Distinct().OrderBy(t => t.NCDC8111E_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO savedata(DC_8111_ExpenditureDTO data)
        {
            try
            {
                if (data.NCDC8111E_Id == 0)
                {
                    var duplicate = _GeneralContext.DC_8111_ExpenditureDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDC8111E_Year == data.ASMAY_Id && t.NCDC8111E_DentalMaterialsName == data.NCDC8111E_DentalMaterialsName && t.NCDC8111E_Expenditure == data.NCDC8111E_Expenditure).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.count += 1;
                        data.msg = "Duplicate";
                    }
                    else
                    {
                        data.count1 += 1;
                        DC_8111_ExpenditureDMO obj = new DC_8111_ExpenditureDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.NCDC8111E_DentalMaterialsName = data.NCDC8111E_DentalMaterialsName;
                        obj.NCDC8111E_Expenditure = data.NCDC8111E_Expenditure;
                        obj.NCDC8111E_Year = data.ASMAY_Id;
                        obj.NCDC8111E_StatusFlg = "";
                        obj.NCDC8111E_ActiveFlag = true;
                        obj.NCDC8111E_CreatedBy = data.UserId;
                        obj.NCDC8111E_UpdatedBy = data.UserId;
                        obj.NCDC8111E_CreatedDate = DateTime.Now;
                        obj.NCDC8111E_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj);
                        if (data.filelist.Length > 0)
                        {
                            for (int j = 0; j < data.filelist.Length; j++)
                            {
                                if (data.filelist[0].cfilepath != null)
                                {


                                    DC_8111_ExpenditureFilesDMO obj2 = new DC_8111_ExpenditureFilesDMO();
                                    //obj2.MI_Id = data.MI_Id;
                                    obj2.NCDC8111E_Id = obj.NCDC8111E_Id;
                                    obj2.NCDC8111EF_FileName = data.filelist[j].cfilename;
                                    obj2.NCMC811NEETF_FileDesc = data.filelist[j].cfiledesc;
                                    obj2.NCDC8111EF_FilePath = data.filelist[j].cfilepath;
                                    obj2.NCDC8111E_Id = obj.NCDC8111E_Id;
                                    obj2.NCDC8111EF_ActiveFlg = true;
                                    obj2.NCDC8111EF_StatusFlg = "";
                                    obj2.NCDC8111EF_CreatedBy = data.UserId;
                                    obj2.NCDC8111EF_UpdatedBy = data.UserId;
                                    obj2.NCDC8111EF_CreatedDate = DateTime.Now;
                                    obj2.NCDC8111EF_UpdatedDate = DateTime.Now;
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
                else if (data.NCDC8111E_Id > 0)
                {
                    var duplicate = _GeneralContext.DC_8111_ExpenditureDMO.Where(b => b.MI_Id == data.MI_Id && b.NCDC8111E_Id != data.NCDC8111E_Id && b.NCDC8111E_Year == data.ASMAY_Id && b.NCDC8111E_Expenditure == data.NCDC8111E_Expenditure && b.NCDC8111E_DentalMaterialsName == data.NCDC8111E_DentalMaterialsName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.count += 1;
                        data.msg = "Duplicate";
                    }
                    else
                    {
                        var update1 = _GeneralContext.DC_8111_ExpenditureDMO.Where(t => t.MI_Id == data.MI_Id && t.NCDC8111E_Id == data.NCDC8111E_Id).SingleOrDefault();
                        update1.NCDC8111E_Year = data.ASMAY_Id;
                        update1.MI_Id = data.MI_Id;
                        update1.NCDC8111E_Expenditure = data.NCDC8111E_Expenditure;
                        update1.NCDC8111E_DentalMaterialsName = data.NCDC8111E_DentalMaterialsName;                       
                        update1.NCDC8111E_UpdatedBy = data.UserId;
                        update1.NCDC8111E_UpdatedDate = DateTime.Now;
                        _GeneralContext.Update(update1);

                        //var CountRemoveFiles = _GeneralContext.DC_8111_ExpenditureFilesDMO.Where(t => t.NCDC8111E_Id == data.NCDC8111E_Id).ToList();
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

                        //        if (data.filelist[0].cfilepath != null)
                        //        {


                        //            DC_8111_ExpenditureFilesDMO obj2 = new DC_8111_ExpenditureFilesDMO();
                        //           // obj2.MI_Id = data.MI_Id;
                        //            obj2.NCDC8111E_Id = update1.NCDC8111E_Id;
                        //            obj2.NCDC8111EF_FileName = data.filelist[k].cfilename;
                        //            obj2.NCMC811NEETF_FileDesc = data.filelist[k].cfiledesc;
                        //            obj2.NCDC8111EF_FilePath = data.filelist[k].cfilepath;
                        //            obj2.NCDC8111EF_ActiveFlg = true;
                        //            obj2.NCDC8111EF_StatusFlg = "";
                        //            obj2.NCDC8111EF_CreatedBy = data.UserId;
                        //            obj2.NCDC8111EF_UpdatedBy = data.UserId;
                        //            obj2.NCDC8111EF_CreatedDate = DateTime.Now;
                        //            obj2.NCDC8111EF_UpdatedDate = DateTime.Now;
                        //            _GeneralContext.Add(obj2);
                        //        }
                        //    }
                        //}
                        //else if (CountRemoveFiles.Count == 0)
                        //{
                        //    if (data.filelist.Length > 0)
                        //    {
                        //        for (int i = 0; i < data.filelist.Length; i++)
                        //        {
                        //            if (data.filelist[0].cfilepath != null)
                        //            {
                        //                DC_8111_ExpenditureFilesDMO obj2 = new DC_8111_ExpenditureFilesDMO();
                        //                obj2.NCDC8111E_Id = update1.NCDC8111E_Id;
                        //               // obj2.MI_Id = data.MI_Id;
                        //                //obj2.NCMC811NEETF_ActiveFlg = true;
                        //                obj2.NCDC8111EF_FileName = data.filelist[i].cfilename;
                        //                obj2.NCMC811NEETF_FileDesc = data.filelist[i].cfiledesc;
                        //                obj2.NCDC8111EF_FilePath = data.filelist[i].cfilepath;                                       
                        //                obj2.NCDC8111EF_CreatedBy = data.UserId;
                        //                obj2.NCDC8111EF_UpdatedBy = data.UserId;
                        //                obj2.NCDC8111EF_CreatedDate = DateTime.Now;
                        //                obj2.NCDC8111EF_UpdatedDate = DateTime.Now;
                        //                _GeneralContext.Add(obj2);
                        //            }
                        //        }
                        //    }
                        //}

                        if (data.filelist.Length > 0)
                        {


                            List<long> Fid = new List<long>();
                            foreach (var item in data.filelist)
                            {
                                Fid.Add(item.NCDC8111EF_Id);
                            }
                            var removefile11 = _GeneralContext.DC_8111_ExpenditureFilesDMO.Where(t => t.NCDC8111E_Id == data.NCDC8111E_Id && !Fid.Contains(t.NCDC8111EF_Id)).Distinct().ToList();
                            if (removefile11.Count > 0)
                            {
                                foreach (var item2 in removefile11)
                                {
                                    var deactfile = _GeneralContext.DC_8111_ExpenditureFilesDMO.Single(t => t.NCDC8111E_Id == data.NCDC8111E_Id && t.NCDC8111EF_Id == item2.NCDC8111EF_Id);
                                    deactfile.NCDC8111EF_ActiveFlg = false;
                                    _GeneralContext.Update(deactfile);
                                }
                            }


                            foreach (DC_8111_ExpenditureDTO DocumentsDTO in data.filelist)
                            {
                                if (DocumentsDTO.NCDC8111EF_Id > 0 && DocumentsDTO.NCDC8111EFC_StatusFlg != "approved")
                                {
                                    if (DocumentsDTO.cfilepath != null)
                                    {

                                        var filesdata = _GeneralContext.DC_8111_ExpenditureFilesDMO.Where(t => t.NCDC8111EF_Id == DocumentsDTO.NCDC8111EF_Id).FirstOrDefault();
                                        filesdata.NCMC811NEETF_FileDesc = DocumentsDTO.cfiledesc;
                                        filesdata.NCDC8111EF_FileName = DocumentsDTO.cfilename;
                                        filesdata.NCDC8111EF_FilePath = DocumentsDTO.cfilepath;


                                        _GeneralContext.Update(filesdata);

                                    }
                                }
                                else
                                {

                                    if (DocumentsDTO.NCDC8111EF_Id == 0)
                                    {
                                        if (DocumentsDTO.cfilepath != null)
                                        {
                                            DC_8111_ExpenditureFilesDMO obj2 = new DC_8111_ExpenditureFilesDMO();
                                            obj2.NCDC8111EF_FileName = DocumentsDTO.cfilename;
                                            obj2.NCMC811NEETF_FileDesc = DocumentsDTO.cfiledesc;
                                            obj2.NCDC8111EF_FilePath = DocumentsDTO.cfilepath;
                                            obj2.NCDC8111EF_StatusFlg = "";
                                            obj2.NCDC8111EF_ActiveFlg = true;
                                            obj2.NCDC8111E_Id = data.NCDC8111E_Id;
                                            _GeneralContext.Add(obj2);

                                        }
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
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO editdata(DC_8111_ExpenditureDTO data)
        {
            try
            {
                var edit = _GeneralContext.DC_8111_ExpenditureDMO.Where(t => t.NCDC8111E_Id == data.NCDC8111E_Id).ToList();
                data.editlist = edit.ToArray();
                data.yearlist = (from a in _GeneralContext.Academic
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new DC_8111_ExpenditureDTO
                                 {
                                     ASMAY_Id = a.ASMAY_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                 }).Distinct().ToArray();

                data.editFileslist = (from a in _GeneralContext.DC_8111_ExpenditureFilesDMO
                                      where (a.NCDC8111E_Id == data.NCDC8111E_Id)
                                      select new DC_8111_ExpenditureDTO
                                      {

                                          cfilename = a.NCDC8111EF_FileName,
                                          cfilepath = a.NCDC8111EF_FilePath,
                                          cfiledesc = a.NCMC811NEETF_FileDesc,
                                      }).Distinct().ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO deactivY(DC_8111_ExpenditureDTO data)
        {
            try
            {
                var result = _GeneralContext.DC_8111_ExpenditureDMO.Where(t => t.NCDC8111E_Id == data.NCDC8111E_Id).SingleOrDefault();
                if (result.NCDC8111E_ActiveFlag == true)
                {
                    result.NCDC8111E_ActiveFlag = false;
                }
                else if (result.NCDC8111E_ActiveFlag == false)
                {
                    result.NCDC8111E_ActiveFlag = true;
                }
                result.NCDC8111E_UpdatedDate = DateTime.Now;
                result.NCDC8111E_UpdatedBy = data.UserId;
                result.MI_Id = data.MI_Id;
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
        public DC_8111_ExpenditureDTO viewuploadflies(DC_8111_ExpenditureDTO data)
        {
            try
            {
                data.viewuploadflies = (from t in _GeneralContext.DC_8111_ExpenditureFilesDMO

                                        where (t.NCDC8111E_Id == data.NCDC8111E_Id && t.NCDC8111EF_ActiveFlg==true)
                                        select new DC_8111_ExpenditureDTO
                                        {
                                            cfilename = t.NCDC8111EF_FileName,
                                            cfilepath = t.NCDC8111EF_FilePath,
                                            cfiledesc = t.NCMC811NEETF_FileDesc,
                                            NCDC8111EF_Id = t.NCDC8111EF_Id,
                                            NCDC8111E_Id = t.NCDC8111E_Id,

                                            //NCMC811NEETF_ActiveFlg = true,             
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO deleteuploadfile(DC_8111_ExpenditureDTO data)
        {
            try
            {
                var result = _GeneralContext.DC_8111_ExpenditureFilesDMO.Where(t => t.NCDC8111EF_Id == data.NCDC8111EF_Id).SingleOrDefault();
                // _GeneralContext.Remove(result);
                result.NCDC8111EF_ActiveFlg = false;
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
                data.viewuploadflies = (from t in _GeneralContext.DC_8111_ExpenditureFilesDMO
                                        where (t.NCDC8111E_Id == data.NCDC8111E_Id && t.NCDC8111EF_ActiveFlg==true)
                                        select new DC_8111_ExpenditureDTO
                                        {
                                            cfilename = t.NCDC8111EF_FileName,
                                            cfilepath = t.NCDC8111EF_FilePath,
                                            cfiledesc = t.NCMC811NEETF_FileDesc,
                                            NCDC8111EF_Id = t.NCDC8111EF_Id,
                                            NCDC8111E_Id = t.NCDC8111E_Id,
                                            //NCMC811NEETF_ActiveFlg = true,
                                        }).Distinct().ToArray();
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO getcomment(DC_8111_ExpenditureDTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.DC_8111_Expenditure_CommentsDMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCDC8111EC_RemarksBy == b.Id && a.NCDC8111E_Id == data.NCDC8111E_Id)
                                    select new DC_8111_ExpenditureDTO
                                    {

                                        NCDC8111EC_Remarks = a.NCDC8111EC_Remarks,
                                        NCDC8111EC_Id = a.NCDC8111EC_Id,
                                        NCDC8111EC_RemarksBy = a.NCDC8111EC_RemarksBy,
                                        NCDC8111EC_StatusFlg = a.NCDC8111EC_StatusFlg,
                                        NCDC8111EC_ActiveFlag = a.NCDC8111EC_ActiveFlag,
                                        NCDC8111EC_CreatedBy = a.NCDC8111EC_CreatedBy,
                                        NCDC8111EC_CreatedDate = a.NCDC8111EC_CreatedDate,
                                        NCDC8111EC_UpdatedBy = a.NCDC8111EC_UpdatedBy,
                                        NCDC8111EC_UpdatedDate = a.NCDC8111EC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO getfilecomment(DC_8111_ExpenditureDTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.DC_8111_Expenditure_File_CommentsDMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCDC8111EFC_RemarksBy == b.Id && a.NCDC8111EF_Id == data.NCDC8111EF_Id)
                                     select new DC_8111_ExpenditureDTO
                                     {
                                         NCDC8111EF_Id = a.NCDC8111EF_Id,
                                         NCDC8111EFC_Remarks = a.NCDC8111EFC_Remarks,
                                         NCDC8111EFC_Id = a.NCDC8111EFC_Id,
                                         NCDC8111EFC_RemarksBy = a.NCDC8111EFC_RemarksBy,
                                         NCDC8111EFC_StatusFlg = a.NCDC8111EFC_StatusFlg,
                                         NCDC8111EFC_ActiveFlag = a.NCDC8111EFC_ActiveFlag,
                                         NCDC8111EFC_CreatedBy = a.NCDC8111EFC_CreatedBy,
                                         NCDC8111EFC_CreatedDate = a.NCDC8111EFC_CreatedDate,
                                         NCDC8111EFC_UpdatedBy = a.NCDC8111EFC_UpdatedBy,
                                         NCDC8111EFC_UpdatedDate = a.NCDC8111EFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO savecomments(DC_8111_ExpenditureDTO data)
        {
            try
            {
                DC_8111_Expenditure_CommentsDMO obj1 = new DC_8111_Expenditure_CommentsDMO();
                obj1.NCDC8111EC_Remarks = data.Remarks;
                obj1.NCDC8111EC_RemarksBy = data.UserId;
                obj1.NCDC8111EC_StatusFlg = "";
                obj1.NCDC8111E_Id = data.filefkid;
                obj1.NCDC8111EC_ActiveFlag = true;
                obj1.NCDC8111EC_CreatedBy = data.UserId;
                obj1.NCDC8111EC_UpdatedBy = data.UserId;
                obj1.NCDC8111EC_CreatedDate = DateTime.Now;
                obj1.NCDC8111EC_UpdatedDate = DateTime.Now;
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public DC_8111_ExpenditureDTO savefilewisecomments(DC_8111_ExpenditureDTO data)
        {
            try
            {
                DC_8111_Expenditure_File_CommentsDMO obj1 = new DC_8111_Expenditure_File_CommentsDMO();
                obj1.NCDC8111EFC_Remarks = data.Remarks;
                obj1.NCDC8111EFC_RemarksBy = data.UserId;
                obj1.NCDC8111EFC_StatusFlg = "";
                obj1.NCDC8111EF_Id = data.filefkid;
                obj1.NCDC8111EFC_ActiveFlag = true;
                obj1.NCDC8111EFC_CreatedBy = data.UserId;
                obj1.NCDC8111EFC_UpdatedBy = data.UserId;
                obj1.NCDC8111EFC_CreatedDate = DateTime.Now;
                obj1.NCDC8111EFC_UpdatedDate = DateTime.Now;
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
