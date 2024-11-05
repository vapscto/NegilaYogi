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
    public class ProfessionalEthicsImpl : Interface.Criteria7.ProfessionalEthicsInterface
    {

        public GeneralContext _GeneralContext;
        public ProfessionalEthicsImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_7115_ProfessionalEthics_DTO> loaddata(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_7115_ProfessionalEthics_DTO
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

        public NAAC_AC_7115_ProfessionalEthics_DTO savedatatab1(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC7115PROETH_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_7115_ProfessionalEthicsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7115PROETH_Year == data.NCAC7115PROETH_Year && t.NCAC7115PROETH_URL == data.NCAC7115PROETH_URL).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_7115_ProfessionalEthicsDMO obj1 = new NAAC_AC_7115_ProfessionalEthicsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC7115PROETH_Year = data.NCAC7115PROETH_Year;
                        obj1.NCAC7115PROETH_URL = data.NCAC7115PROETH_URL;
                        obj1.NCAC7115PROETH_ActiveFlg = true;
                        obj1.NCAC7115PROETH_CreatedBy = data.UserId;
                        obj1.NCAC7115PROETH_UpdatedBy = data.UserId;
                        obj1.NCAC7115PROETH_StatusFlg = "";
                        obj1.NCAC7115PROETH_CreatedDate = DateTime.Now;
                        obj1.NCAC7115PROETH_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC7115PROETH_Id;
                        if (data.NAACAC7115DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_7115_ProfessionalEthics_DTO DocumentsDTO in data.NAACAC7115DTO)
                            {
                                NAAC_AC_7115_ProfessionalEthics_FilesDMO obj2 = new NAAC_AC_7115_ProfessionalEthics_FilesDMO();
                                obj2.NCAC7115PROETHF_FileName = DocumentsDTO.NCAC7115PROETHF_FileName;
                                obj2.NCAC7115PROETHF_Filedesc = DocumentsDTO.NCAC7115PROETHF_Filedesc;
                                obj2.NCAC7115PROETHF_FilePath = DocumentsDTO.NCAC7115PROETHF_FilePath;
                                obj2.NCAC7115PROETHF_StatusFlg = "";
                                obj2.NCAC7115PROETHF_ActiveFlg = true;
                                obj2.NCAC7115PROETH_Id = s;
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
                else if (data.NCAC7115PROETH_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_7115_ProfessionalEthicsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id).SingleOrDefault();

                    update.NCAC7115PROETH_Year = data.NCAC7115PROETH_Year;
                    update.NCAC7115PROETH_URL = data.NCAC7115PROETH_URL;
                    update.NCAC7115PROETH_UpdatedBy = data.UserId;
                    update.NCAC7115PROETH_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC7115PROETH_Id;
                    if (data.NAACAC7115DTO.Count() > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.NAACAC7115DTO)
                        {
                            Fid.Add(item.NCAC7115PROETHF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_7115_ProfessionalEthics_FilesDMO.Where(t => t.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id && !Fid.Contains(t.NCAC7115PROETHF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_7115_ProfessionalEthics_FilesDMO.Single(t => t.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id && t.NCAC7115PROETHF_Id == item2.NCAC7115PROETHF_Id);
                                deactfile.NCAC7115PROETHF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }


                        foreach (NAAC_AC_7115_ProfessionalEthics_DTO DocumentsDTO in data.NAACAC7115DTO)
                        {
                            if (DocumentsDTO.NCAC7115PROETHF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_7115_ProfessionalEthics_FilesDMO.Where(t => t.NCAC7115PROETHF_Id == DocumentsDTO.NCAC7115PROETHF_Id).FirstOrDefault();
                                filesdata.NCAC7115PROETHF_Filedesc = DocumentsDTO.NCAC7115PROETHF_Filedesc;
                                filesdata.NCAC7115PROETHF_FileName = DocumentsDTO.NCAC7115PROETHF_FileName;
                                filesdata.NCAC7115PROETHF_FilePath = DocumentsDTO.NCAC7115PROETHF_FilePath;
                                _GeneralContext.Update(filesdata);
                                
                            }
                            else
                            {
                                NAAC_AC_7115_ProfessionalEthics_FilesDMO obj2 = new NAAC_AC_7115_ProfessionalEthics_FilesDMO();
                                obj2.NCAC7115PROETHF_FileName = DocumentsDTO.NCAC7115PROETHF_FileName;
                                obj2.NCAC7115PROETHF_Filedesc = DocumentsDTO.NCAC7115PROETHF_Filedesc;
                                obj2.NCAC7115PROETHF_FilePath = DocumentsDTO.NCAC7115PROETHF_FilePath;
                                obj2.NCAC7115PROETHF_ActiveFlg = true;
                                obj2.NCAC7115PROETHF_StatusFlg = "";
                                obj2.NCAC7115PROETH_Id = s;
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

        public NAAC_AC_7115_ProfessionalEthics_DTO deactivYTab1(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_7115_ProfessionalEthicsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id).SingleOrDefault();

                if (result.NCAC7115PROETH_ActiveFlg == true)
                {
                    result.NCAC7115PROETH_ActiveFlg = false;
                }
                else if (result.NCAC7115PROETH_ActiveFlg == false)
                {
                    result.NCAC7115PROETH_ActiveFlg = true;
                }

                result.NCAC7115PROETH_UpdatedDate = DateTime.Now;
                result.NCAC7115PROETH_UpdatedBy = data.UserId;

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

        public NAAC_AC_7115_ProfessionalEthics_DTO editTab1(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_7115_ProfessionalEthicsDMO.Where(t =>t.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id).ToList();
                var editfile = _GeneralContext.NAAC_AC_7115_ProfessionalEthics_FilesDMO.Where(t => t.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id&&t.NCAC7115PROETHF_ActiveFlg==true).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_7115_ProfessionalEthics_DTO deleteuploadfile(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                List<NAAC_AC_7115_ProfessionalEthics_FilesDMO> removelist = new List<NAAC_AC_7115_ProfessionalEthics_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_7115_ProfessionalEthics_FilesDMO.Where(t => t.NCAC7115PROETHF_Id == data.NCAC7115PROETHF_Id).ToList();
                foreach (NAAC_AC_7115_ProfessionalEthics_FilesDMO obj1 in removelist)
                {
                    obj1.NCAC7115PROETHF_ActiveFlg = false;
                    _GeneralContext.Update(obj1);
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

        public NAAC_AC_7115_ProfessionalEthics_DTO getData(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_7115_ProfessionalEthicsDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC7115PROETH_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_7115_ProfessionalEthics_DTO
                                    {
                                        NCAC7115PROETH_Id = a.NCAC7115PROETH_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC7115PROETH_Year = a.NCAC7115PROETH_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC7115PROETH_URL = a.NCAC7115PROETH_URL,
                                        NCAC7115PROETH_ActiveFlg = a.NCAC7115PROETH_ActiveFlg,
                                        NCAC7115PROETH_StatusFlg = a.NCAC7115PROETH_StatusFlg,
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public NAAC_AC_7115_ProfessionalEthics_DTO getcomment(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_7115_ProfessionalEthics_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC7115PROETHC_RemarksBy == b.Id && a.NCAC7115PROETH_Id == data.NCAC7115PROETH_Id)
                                    select new NAAC_AC_7115_ProfessionalEthics_DTO
                                    {
                                        NCAC7115PROETHC_Remarks = a.NCAC7115PROETHC_Remarks,
                                        NCAC7115PROETHC_Id = a.NCAC7115PROETHC_Id,
                                        NCAC7115PROETHC_RemarksBy = a.NCAC7115PROETHC_RemarksBy,
                                        NCAC7115PROETHC_StatusFlg = a.NCAC7115PROETHC_StatusFlg,
                                        NCAC7115PROETHC_ActiveFlag = a.NCAC7115PROETHC_ActiveFlag,
                                        NCAC7115PROETHC_CreatedBy = a.NCAC7115PROETHC_CreatedBy,
                                        NCAC7115PROETHC_CreatedDate = a.NCAC7115PROETHC_CreatedDate,
                                        NCAC7115PROETHC_UpdatedBy = a.NCAC7115PROETHC_UpdatedBy,
                                        NCAC7115PROETHC_UpdatedDate = a.NCAC7115PROETHC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC7115PROETHC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_7115_ProfessionalEthics_DTO getfilecomment(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_7115_ProfessionalEthics_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC7115PROETHFC_RemarksBy == b.Id && a.NCAC7115PROETHF_Id == data.NCAC7115PROETHF_Id)
                                     select new NAAC_AC_7115_ProfessionalEthics_DTO
                                     {
                                         NCAC7115PROETHF_Id = a.NCAC7115PROETHF_Id,
                                         NCAC7115PROETHFC_Remarks = a.NCAC7115PROETHFC_Remarks,
                                         NCAC7115PROETHFC_Id = a.NCAC7115PROETHFC_Id,
                                         NCAC7115PROETHFC_RemarksBy = a.NCAC7115PROETHFC_RemarksBy,
                                         NCAC7115PROETHFC_StatusFlg = a.NCAC7115PROETHFC_StatusFlg,
                                         NCAC7115PROETHFC_ActiveFlag = a.NCAC7115PROETHFC_ActiveFlag,
                                         NCAC7115PROETHFC_CreatedBy = a.NCAC7115PROETHFC_CreatedBy,
                                         NCAC7115PROETHFC_CreatedDate = a.NCAC7115PROETHFC_CreatedDate,
                                         NCAC7115PROETHFC_UpdatedBy = a.NCAC7115PROETHFC_UpdatedBy,
                                         NCAC7115PROETHFC_UpdatedDate = a.NCAC7115PROETHFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC7115PROETHFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_7115_ProfessionalEthics_DTO savemedicaldatawisecomments(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                NAAC_AC_7115_ProfessionalEthics_Comments_DMO obj1 = new NAAC_AC_7115_ProfessionalEthics_Comments_DMO();
                obj1.NCAC7115PROETHC_Remarks = data.Remarks;
                obj1.NCAC7115PROETHC_RemarksBy = data.UserId;
                obj1.NCAC7115PROETHC_StatusFlg = "";
                obj1.NCAC7115PROETH_Id = data.filefkid;
                obj1.NCAC7115PROETHC_ActiveFlag = true;
                obj1.NCAC7115PROETHC_CreatedBy = data.UserId;
                obj1.NCAC7115PROETHC_UpdatedBy = data.UserId;
                obj1.NCAC7115PROETHC_CreatedDate = DateTime.Now;
                obj1.NCAC7115PROETHC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_7115_ProfessionalEthics_DTO savefilewisecomments(NAAC_AC_7115_ProfessionalEthics_DTO data)
        {
            try
            {
                NAAC_AC_7115_ProfessionalEthics_File_Comments_DMO obj1 = new NAAC_AC_7115_ProfessionalEthics_File_Comments_DMO();
                obj1.NCAC7115PROETHFC_Remarks = data.Remarks;
                obj1.NCAC7115PROETHFC_RemarksBy = data.UserId;
                obj1.NCAC7115PROETHFC_StatusFlg = "";
                obj1.NCAC7115PROETHF_Id = data.filefkid;
                obj1.NCAC7115PROETHFC_ActiveFlag = true;
                obj1.NCAC7115PROETHFC_CreatedBy = data.UserId;
                obj1.NCAC7115PROETHFC_UpdatedBy = data.UserId;
                obj1.NCAC7115PROETHFC_UpdatedDate = DateTime.Now;
                obj1.NCAC7115PROETHFC_CreatedDate = DateTime.Now;
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
