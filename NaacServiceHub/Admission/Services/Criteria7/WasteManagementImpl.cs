using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccessMsSqlServerProvider.NAAC;
using DomainModel.Model.NAAC.Admission;
using DomainModel.Model.NAAC.Admission.Criteria7;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class WasteManagementImpl : Interface.Criteria7.WasteManagementInterface
    {

        public GeneralContext _GeneralContext;
        public WasteManagementImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_718_WasteManagement_DTO> loaddata(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_718_WasteManagement_DTO
                                        {
                                            MI_Id = a.MI_Id,
                                            MI_Name = a.MI_Name
                                        }).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_718_WasteManagement_DTO savedatatab1(NAAC_AC_718_WasteManagement_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC718WAMAN_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_718_WasteManagementDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC718WAMAN_Year == data.NCAC718WAMAN_Year && t.NCAC718WAMAN_Expenditure == data.NCAC718WAMAN_Expenditure).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_718_WasteManagementDMO obj1 = new NAAC_AC_718_WasteManagementDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC718WAMAN_Year = data.NCAC718WAMAN_Year;
                        obj1.NCAC718WAMAN_Expenditure = data.NCAC718WAMAN_Expenditure;
                        obj1.NCAC718WAMAN_ActiveFlg = true;
                        obj1.NCAC718WAMAN_CreatedBy = data.UserId;
                        obj1.NCAC718WAMAN_UpdatedBy = data.UserId;
                        obj1.NCAC718WAMAN_CreatedDate = DateTime.Now;
                        obj1.NCAC718WAMAN_UpdatedDate = DateTime.Now;
                        obj1.NCAC718WAMAN_StatusFlg = "";
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC718WAMAN_Id;
                        if (data.NAACAC7DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_718_WasteManagement_DTO DocumentsDTO in data.NAACAC7DTO)
                            {
                                NAAC_AC_718_WasteManagement_FilesDMO obj2 = new NAAC_AC_718_WasteManagement_FilesDMO();
                                obj2.NCAC718WAMANF_FileName = DocumentsDTO.NCAC718WAMANF_FileName;
                                obj2.NCAC718WAMANF_Filedesc = DocumentsDTO.NCAC718WAMANF_Filedesc;
                                obj2.NCAC718WAMANF_FilePath = DocumentsDTO.NCAC718WAMANF_FilePath;
                                obj2.NCAC718WAMAN_Id = s;
                                obj2.NCAC718WAMANF_StatusFlg ="";
                                obj2.NCAC718WAMANF_ActiveFlg =true;
                                _GeneralContext.Add(obj2);
                               
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
                else if (data.NCAC718WAMAN_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_718_WasteManagementDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id).SingleOrDefault();

                    update.NCAC718WAMAN_Year = data.NCAC718WAMAN_Year;
                    update.NCAC718WAMAN_Expenditure = data.NCAC718WAMAN_Expenditure;
                    update.NCAC718WAMAN_UpdatedBy = data.UserId;
                    update.NCAC718WAMAN_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC718WAMAN_Id;
                    if (data.NAACAC7DTO.Count() > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.NAACAC7DTO)
                        {
                            Fid.Add(item.NCAC718WAMANF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_718_WasteManagement_FilesDMO.Where(t => t.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id && !Fid.Contains(t.NCAC718WAMANF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_718_WasteManagement_FilesDMO.Single(t => t.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id && t.NCAC718WAMANF_Id == item2.NCAC718WAMANF_Id);
                                deactfile.NCAC718WAMANF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }


                        foreach (NAAC_AC_718_WasteManagement_DTO DocumentsDTO in data.NAACAC7DTO)
                        {
                            if (DocumentsDTO.NCAC718WAMANF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_718_WasteManagement_FilesDMO.Where(t => t.NCAC718WAMANF_Id == DocumentsDTO.NCAC718WAMANF_Id).FirstOrDefault();
                                filesdata.NCAC718WAMANF_Filedesc = DocumentsDTO.NCAC718WAMANF_Filedesc;
                                filesdata.NCAC718WAMANF_FileName = DocumentsDTO.NCAC718WAMANF_FileName;
                                filesdata.NCAC718WAMANF_FilePath = DocumentsDTO.NCAC718WAMANF_FilePath;
                                _GeneralContext.Update(filesdata);
                                
                            }
                            else
                            {
                                NAAC_AC_718_WasteManagement_FilesDMO obj2 = new NAAC_AC_718_WasteManagement_FilesDMO();
                                obj2.NCAC718WAMANF_FileName = DocumentsDTO.NCAC718WAMANF_FileName;
                                obj2.NCAC718WAMANF_Filedesc = DocumentsDTO.NCAC718WAMANF_Filedesc;
                                obj2.NCAC718WAMANF_FilePath = DocumentsDTO.NCAC718WAMANF_FilePath;
                                obj2.NCAC718WAMAN_Id = s;
                                obj2.NCAC718WAMANF_ActiveFlg = true;
                                obj2.NCAC718WAMANF_StatusFlg = "";
                                _GeneralContext.Add(obj2);
                                
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_718_WasteManagement_DTO deactivYTab1(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_718_WasteManagementDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id).SingleOrDefault();

                if (result.NCAC718WAMAN_ActiveFlg == true)
                {
                    result.NCAC718WAMAN_ActiveFlg = false;
                }
                else if (result.NCAC718WAMAN_ActiveFlg == false)
                {
                    result.NCAC718WAMAN_ActiveFlg = true;
                }

                result.NCAC718WAMAN_UpdatedDate = DateTime.Now;
                result.NCAC718WAMAN_UpdatedBy = data.UserId;

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

        public NAAC_AC_718_WasteManagement_DTO editTab1(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_718_WasteManagementDMO.Where(t =>  t.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id).ToList();
                var editfile = _GeneralContext.NAAC_AC_718_WasteManagement_FilesDMO.Where(t => t.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id&&t.NCAC718WAMANF_ActiveFlg==true).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_718_WasteManagement_DTO deleteuploadfile(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                List<NAAC_AC_718_WasteManagement_FilesDMO> removelist = new List<NAAC_AC_718_WasteManagement_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_718_WasteManagement_FilesDMO.Where(t => t.NCAC718WAMANF_Id == data.NCAC718WAMANF_Id).ToList();
                foreach (NAAC_AC_718_WasteManagement_FilesDMO obj1 in removelist)
                {
                    obj1.NCAC718WAMANF_ActiveFlg = false;
                    _GeneralContext.Update(obj1);
                    //  _GeneralContext.Remove(obj1);
                    _GeneralContext.SaveChanges();
                    data.retrunMsg = "Deleted";
                    data.returnval = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_718_WasteManagement_DTO getData(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_718_WasteManagementDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC718WAMAN_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_718_WasteManagement_DTO
                                    {
                                        NCAC718WAMAN_Id = a.NCAC718WAMAN_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC718WAMAN_Year = a.NCAC718WAMAN_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC718WAMAN_Expenditure = a.NCAC718WAMAN_Expenditure,
                                        NCAC718WAMAN_ActiveFlg = a.NCAC718WAMAN_ActiveFlg,
                                        NCAC718WAMAN_StatusFlg = a.NCAC718WAMAN_StatusFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_718_WasteManagement_DTO getcomment(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_718_WasteManagement_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC718WAMANC_RemarksBy == b.Id && a.NCAC718WAMAN_Id == data.NCAC718WAMAN_Id)
                                    select new NAAC_AC_718_WasteManagement_DTO
                                    {
                                        NCAC718WAMANC_Remarks = a.NCAC718WAMANC_Remarks,
                                        NCAC718WAMANC_Id = a.NCAC718WAMANC_Id,
                                        NCAC718WAMANC_RemarksBy = a.NCAC718WAMANC_RemarksBy,
                                        NCAC718WAMANC_StatusFlg = a.NCAC718WAMANC_StatusFlg,
                                        NCAC718WAMANC_ActiveFlag = a.NCAC718WAMANC_ActiveFlag,
                                        NCAC718WAMANC_CreatedBy = a.NCAC718WAMANC_CreatedBy,
                                        NCAC718WAMANC_CreatedDate = a.NCAC718WAMANC_CreatedDate,
                                        NCAC718WAMANC_UpdatedBy = a.NCAC718WAMANC_UpdatedBy,
                                        NCAC718WAMANC_UpdatedDate = a.NCAC718WAMANC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC718WAMANC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_718_WasteManagement_DTO getfilecomment(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_718_WasteManagement_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC718WAMANFC_RemarksBy == b.Id && a.NCAC718WAMANF_Id == data.NCAC718WAMANF_Id)
                                     select new NAAC_AC_718_WasteManagement_DTO
                                     {
                                         NCAC718WAMANF_Id = a.NCAC718WAMANF_Id,
                                         NCAC718WAMANFC_Remarks = a.NCAC718WAMANFC_Remarks,
                                         NCAC718WAMANFC_Id = a.NCAC718WAMANFC_Id,
                                         NCAC718WAMANFC_RemarksBy = a.NCAC718WAMANFC_RemarksBy,
                                         NCAC718WAMANFC_StatusFlg = a.NCAC718WAMANFC_StatusFlg,
                                         NCAC718WAMANFC_ActiveFlag = a.NCAC718WAMANFC_ActiveFlag,
                                         NCAC718WAMANFC_CreatedBy = a.NCAC718WAMANFC_CreatedBy,
                                         NCAC718WAMANFC_CreatedDate = a.NCAC718WAMANFC_CreatedDate,
                                         NCAC718WAMANFC_UpdatedBy = a.NCAC718WAMANFC_UpdatedBy,
                                         NCAC718WAMANFC_UpdatedDate = a.NCAC718WAMANFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC718WAMANFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_718_WasteManagement_DTO savemedicaldatawisecomments(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                NAAC_AC_718_WasteManagement_Comments_DMO obj1 = new NAAC_AC_718_WasteManagement_Comments_DMO();
                obj1.NCAC718WAMANC_Remarks = data.Remarks;
                obj1.NCAC718WAMANC_RemarksBy = data.UserId;
                obj1.NCAC718WAMANC_StatusFlg = "";
                obj1.NCAC718WAMAN_Id = data.filefkid;
                obj1.NCAC718WAMANC_ActiveFlag = true;
                obj1.NCAC718WAMANC_CreatedBy = data.UserId;
                obj1.NCAC718WAMANC_UpdatedBy = data.UserId;
                obj1.NCAC718WAMANC_CreatedDate = DateTime.Now;
                obj1.NCAC718WAMANC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_718_WasteManagement_DTO savefilewisecomments(NAAC_AC_718_WasteManagement_DTO data)
        {
            try
            {
                NAAC_AC_718_WasteManagement_File_Comments_DMO obj1 = new NAAC_AC_718_WasteManagement_File_Comments_DMO();
                obj1.NCAC718WAMANFC_Remarks = data.Remarks;
                obj1.NCAC718WAMANFC_RemarksBy = data.UserId;
                obj1.NCAC718WAMANFC_StatusFlg = "";
                obj1.NCAC718WAMANF_Id = data.filefkid;
                obj1.NCAC718WAMANFC_ActiveFlag = true;
                obj1.NCAC718WAMANFC_CreatedBy = data.UserId;
                obj1.NCAC718WAMANFC_UpdatedBy = data.UserId;
                obj1.NCAC718WAMANFC_UpdatedDate = DateTime.Now;
                obj1.NCAC718WAMANFC_CreatedDate = DateTime.Now;
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
