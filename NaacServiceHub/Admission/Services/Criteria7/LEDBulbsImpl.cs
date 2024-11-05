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
    public class LEDBulbsImpl : Interface.Criteria7.LEDBulbsInterface
    {

        public GeneralContext _GeneralContext;
        public LEDBulbsImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_714_LEDBulbs_DTO> loaddata(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_714_LEDBulbs_DTO
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

        public NAAC_AC_714_LEDBulbs_DTO savedatatab1(NAAC_AC_714_LEDBulbs_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC714LEDBU_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_714_LEDBulbsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC714LEDBU_Year == data.NCAC714LEDBU_Year && t.NCAC714LEDBU_LightingsRequirements == data.NCAC714LEDBU_LightingsRequirements && t.NCAC714LEDBU_LughtingLED == data.NCAC714LEDBU_LughtingLED && t.NCAC714LEDBU_OtherSource == data.NCAC714LEDBU_OtherSource).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_714_LEDBulbsDMO obj1 = new NAAC_AC_714_LEDBulbsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC714LEDBU_Year = data.NCAC714LEDBU_Year;
                        obj1.NCAC714LEDBU_LightingsRequirements = data.NCAC714LEDBU_LightingsRequirements;
                        obj1.NCAC714LEDBU_LughtingLED = data.NCAC714LEDBU_LughtingLED;
                        obj1.NCAC714LEDBU_OtherSource = data.NCAC714LEDBU_OtherSource;
                        obj1.NCAC714LEDBU_ActiveFlg = true;
                        obj1.NCAC714LEDBU_CreatedBy = data.UserId;
                        obj1.NCAC714LEDBU_UpdatedBy = data.UserId;
                        obj1.NCAC714LEDBU_StatusFlg = "";
                        obj1.NCAC714LEDBU_CreatedDate = DateTime.Now;
                        obj1.NCAC714LEDBU_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC714LEDBU_Id;
                        if (data.NAACAC7DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_714_LEDBulbs_DTO DocumentsDTO in data.NAACAC7DTO)
                            {
                                NAAC_AC_714_LEDBulbs_FilesDMO obj2 = new NAAC_AC_714_LEDBulbs_FilesDMO();
                                obj2.NCAC714LEDBUF_FileName = DocumentsDTO.NCAC714LEDBUF_FileName;
                                obj2.NCAC714LEDBUF_Filedesc = DocumentsDTO.NCAC714LEDBUF_Filedesc;
                                obj2.NCAC714LEDBUF_FilePath = DocumentsDTO.NCAC714LEDBUF_FilePath;
                                obj2.NCAC714LEDBU_Id = s;
                                obj2.NCAC714LEDBUF_StatusFlg = "";
                                obj2.NCAC714LEDBUF_ActiveFlg = true;
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
                else if (data.NCAC714LEDBU_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_714_LEDBulbsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id).SingleOrDefault();

                    update.NCAC714LEDBU_Year = data.NCAC714LEDBU_Year;
                    update.NCAC714LEDBU_LightingsRequirements = data.NCAC714LEDBU_LightingsRequirements;
                    update.NCAC714LEDBU_LughtingLED = data.NCAC714LEDBU_LughtingLED;
                    update.NCAC714LEDBU_OtherSource = data.NCAC714LEDBU_OtherSource;
                    update.NCAC714LEDBU_UpdatedBy = data.UserId;
                    update.NCAC714LEDBU_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC714LEDBU_Id;
                    if (data.NAACAC7DTO.Count() > 0)
                    {
                        List<long> Fid = new List<long>();
                        foreach (var item in data.NAACAC7DTO)
                        {
                            Fid.Add(item.NCAC714LEDBUF_Id);
                        }
                        var removefile11 = _GeneralContext.NAAC_AC_714_LEDBulbs_FilesDMO.Where(t => t.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id && !Fid.Contains(t.NCAC714LEDBUF_Id)).Distinct().ToList();
                        if (removefile11.Count > 0)
                        {
                            foreach (var item2 in removefile11)
                            {
                                var deactfile = _GeneralContext.NAAC_AC_714_LEDBulbs_FilesDMO.Single(t => t.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id && t.NCAC714LEDBUF_Id == item2.NCAC714LEDBUF_Id);
                                deactfile.NCAC714LEDBUF_ActiveFlg = false;
                                _GeneralContext.Update(deactfile);
                            }
                        }


                        foreach (NAAC_AC_714_LEDBulbs_DTO DocumentsDTO in data.NAACAC7DTO)
                        {
                            if (DocumentsDTO.NCAC714LEDBUF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_714_LEDBulbs_FilesDMO.Where(t => t.NCAC714LEDBUF_Id == DocumentsDTO.NCAC714LEDBUF_Id).FirstOrDefault();
                                filesdata.NCAC714LEDBUF_Filedesc = DocumentsDTO.NCAC714LEDBUF_Filedesc;
                                filesdata.NCAC714LEDBUF_FileName = DocumentsDTO.NCAC714LEDBUF_FileName;
                                filesdata.NCAC714LEDBUF_FilePath = DocumentsDTO.NCAC714LEDBUF_FilePath;
                                _GeneralContext.Update(filesdata);
                               
                            }
                            else
                            {
                                NAAC_AC_714_LEDBulbs_FilesDMO obj2 = new NAAC_AC_714_LEDBulbs_FilesDMO();
                                obj2.NCAC714LEDBUF_FileName = DocumentsDTO.NCAC714LEDBUF_FileName;
                                obj2.NCAC714LEDBUF_Filedesc = DocumentsDTO.NCAC714LEDBUF_Filedesc;
                                obj2.NCAC714LEDBUF_FilePath = DocumentsDTO.NCAC714LEDBUF_FilePath;
                                obj2.NCAC714LEDBU_Id = s;
                                obj2.NCAC714LEDBUF_StatusFlg = "";
                                obj2.NCAC714LEDBUF_ActiveFlg = true;
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

        public NAAC_AC_714_LEDBulbs_DTO deactivYTab1(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_714_LEDBulbsDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id).SingleOrDefault();

                if (result.NCAC714LEDBU_ActiveFlg == true)
                {
                    result.NCAC714LEDBU_ActiveFlg = false;
                }
                else if (result.NCAC714LEDBU_ActiveFlg == false)
                {
                    result.NCAC714LEDBU_ActiveFlg = true;
                }

                result.NCAC714LEDBU_UpdatedDate = DateTime.Now;
                result.NCAC714LEDBU_UpdatedBy = data.UserId;

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

        public NAAC_AC_714_LEDBulbs_DTO editTab1(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_714_LEDBulbsDMO.Where(t =>t.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id).ToList();
                var editfile = _GeneralContext.NAAC_AC_714_LEDBulbs_FilesDMO.Where(t => t.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id&&t.NCAC714LEDBUF_ActiveFlg==true).ToList();
                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_714_LEDBulbs_DTO deleteuploadfile(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                List<NAAC_AC_714_LEDBulbs_FilesDMO> removelist = new List<NAAC_AC_714_LEDBulbs_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_714_LEDBulbs_FilesDMO.Where(t => t.NCAC714LEDBUF_Id == data.NCAC714LEDBUF_Id).ToList();
                foreach (NAAC_AC_714_LEDBulbs_FilesDMO obj1 in removelist)
                {
                    obj1.NCAC714LEDBUF_ActiveFlg = false;
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

        public NAAC_AC_714_LEDBulbs_DTO getData(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_714_LEDBulbsDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC714LEDBU_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_714_LEDBulbs_DTO
                                    {
                                        NCAC714LEDBU_Id = a.NCAC714LEDBU_Id,
                                        MI_Id = a.MI_Id,
                                        NCAC714LEDBU_Year = a.NCAC714LEDBU_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC714LEDBU_LightingsRequirements = a.NCAC714LEDBU_LightingsRequirements,
                                        NCAC714LEDBU_LughtingLED = a.NCAC714LEDBU_LughtingLED,
                                        NCAC714LEDBU_OtherSource = a.NCAC714LEDBU_OtherSource,
                                        NCAC714LEDBU_ActiveFlg = a.NCAC714LEDBU_ActiveFlg,
                                        NCAC714LEDBU_StatusFlg = a.NCAC714LEDBU_StatusFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_714_LEDBulbs_DTO getcommentLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_AC_714_LEDBulbs_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCAC714LEDBUC_RemarksBy == b.Id && a.NCAC714LEDBU_Id == data.NCAC714LEDBU_Id)
                                    select new NAAC_AC_714_LEDBulbs_DTO
                                    {
                                        NCAC714LEDBUC_Remarks = a.NCAC714LEDBUC_Remarks,
                                        NCAC714LEDBUC_Id = a.NCAC714LEDBUC_Id,
                                        NCAC714LEDBUC_RemarksBy = a.NCAC714LEDBUC_RemarksBy,
                                        NCAC714LEDBUC_StatusFlg = a.NCAC714LEDBUC_StatusFlg,
                                        NCAC714LEDBUC_ActiveFlag = a.NCAC714LEDBUC_ActiveFlag,
                                        NCAC714LEDBUC_CreatedBy = a.NCAC714LEDBUC_CreatedBy,
                                        NCAC714LEDBUC_CreatedDate = a.NCAC714LEDBUC_CreatedDate,
                                        NCAC714LEDBUC_UpdatedBy = a.NCAC714LEDBUC_UpdatedBy,
                                        NCAC714LEDBUC_UpdatedDate = a.NCAC714LEDBUC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCAC714LEDBUC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // for file
        public NAAC_AC_714_LEDBulbs_DTO getfilecommentLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.commentlist1 = (from a in _GeneralContext.NAAC_AC_714_LEDBulbs_File_Comments_DMO
                                     from b in _GeneralContext.ApplUser
                                     where (a.NCAC714LEDBUFC_RemarksBy == b.Id && a.NCAC714LEDBUF_Id == data.NCAC714LEDBUF_Id)
                                     select new NAAC_AC_714_LEDBulbs_DTO
                                     {
                                         NCAC714LEDBUF_Id = a.NCAC714LEDBUF_Id,
                                         NCAC714LEDBUFC_Remarks = a.NCAC714LEDBUFC_Remarks,
                                         NCAC714LEDBUFC_Id = a.NCAC714LEDBUFC_Id,
                                         NCAC714LEDBUFC_RemarksBy = a.NCAC714LEDBUFC_RemarksBy,
                                         NCAC714LEDBUFC_StatusFlg = a.NCAC714LEDBUFC_StatusFlg,
                                         NCAC714LEDBUFC_ActiveFlag = a.NCAC714LEDBUFC_ActiveFlag,
                                         NCAC714LEDBUFC_CreatedBy = a.NCAC714LEDBUFC_CreatedBy,
                                         NCAC714LEDBUFC_CreatedDate = a.NCAC714LEDBUFC_CreatedDate,
                                         NCAC714LEDBUFC_UpdatedBy = a.NCAC714LEDBUFC_UpdatedBy,
                                         NCAC714LEDBUFC_UpdatedDate = a.NCAC714LEDBUFC_UpdatedDate,
                                         UserName = b.UserName,
                                     }).Distinct().OrderByDescending(a => a.NCAC714LEDBUFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecommentsLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                NAAC_AC_714_LEDBulbs_Comments_DMO obj1 = new NAAC_AC_714_LEDBulbs_Comments_DMO();
                obj1.NCAC714LEDBUC_Remarks = data.Remarks;
                obj1.NCAC714LEDBUC_RemarksBy = data.UserId;
                obj1.NCAC714LEDBUC_StatusFlg = "";
                obj1.NCAC714LEDBU_Id = data.filefkid;
                obj1.NCAC714LEDBUC_ActiveFlag = true;
                obj1.NCAC714LEDBUC_CreatedBy = data.UserId;
                obj1.NCAC714LEDBUC_UpdatedBy = data.UserId;
                obj1.NCAC714LEDBUC_CreatedDate = DateTime.Now;
                obj1.NCAC714LEDBUC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_714_LEDBulbs_DTO savefilewisecommentsLEDbulb(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                NAAC_AC_714_LEDBulbs_File_Comments_DMO obj1 = new NAAC_AC_714_LEDBulbs_File_Comments_DMO();
                obj1.NCAC714LEDBUFC_Remarks = data.Remarks;
                obj1.NCAC714LEDBUFC_RemarksBy = data.UserId;
                obj1.NCAC714LEDBUFC_StatusFlg = "";
                obj1.NCAC714LEDBUF_Id = data.filefkid;
                obj1.NCAC714LEDBUFC_ActiveFlag = true;
                obj1.NCAC714LEDBUFC_CreatedBy = data.UserId;
                obj1.NCAC714LEDBUFC_UpdatedBy = data.UserId;
                obj1.NCAC714LEDBUFC_UpdatedDate = DateTime.Now;
                obj1.NCAC714LEDBUFC_CreatedDate = DateTime.Now;
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



        //MC
        public NAAC_AC_714_LEDBulbs_DTO savemedicaldatawisecomments(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                NAAC_MC_715_WaterConservFacilities_Comments_DMO obj1 = new NAAC_MC_715_WaterConservFacilities_Comments_DMO();
                obj1.NCMC715WCFC_Remarks = data.Remarks;
                obj1.NCMC715WCFC_RemarksBy = data.UserId;
                obj1.NCMC715WCFC_StatusFlg = "";
                obj1.NCMC715WCF_Id = data.filefkid;
                obj1.NCMC715WCFC_ActiveFlag = true;
                obj1.NCMC715WCFC_CreatedBy = data.UserId;
                obj1.NCMC715WCFC_UpdatedBy = data.UserId;
                obj1.NCMC715WCFC_CreatedDate = DateTime.Now;
                obj1.NCMC715WCFC_UpdatedDate = DateTime.Now;
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
        public NAAC_AC_714_LEDBulbs_DTO getcomment(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.commentlist = (from a in _GeneralContext.NAAC_MC_715_WaterConservFacilities_Comments_DMO
                                    from b in _GeneralContext.ApplUser
                                    where (a.NCMC715WCFC_RemarksBy == b.Id && a.NCMC715WCF_Id == data.NCMC715WCF_Id)
                                    select new NAAC_AC_714_LEDBulbs_DTO
                                    {
                                        NCMC715WCFC_Remarks = a.NCMC715WCFC_Remarks,
                                        NCMC715WCFC_Id = a.NCMC715WCFC_Id,
                                        NCMC715WCFC_RemarksBy = a.NCMC715WCFC_RemarksBy,
                                        NCMC715WCFC_StatusFlg = a.NCMC715WCFC_StatusFlg,
                                        NCMC715WCFC_ActiveFlag = a.NCMC715WCFC_ActiveFlag,
                                        NCMC715WCFC_CreatedBy = a.NCMC715WCFC_CreatedBy,
                                        NCMC715WCFC_CreatedDate = a.NCMC715WCFC_CreatedDate,
                                        NCMC715WCFC_UpdatedBy = a.NCMC715WCFC_UpdatedBy,
                                        NCMC715WCFC_UpdatedDate = a.NCMC715WCFC_UpdatedDate,
                                        UserName = b.UserName,
                                    }).Distinct().OrderByDescending(a => a.NCMC715WCFC_CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public NAAC_AC_714_LEDBulbs_DTO getDataMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_MC_715_WaterConservFacilitiesDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCMC715WCF_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_714_LEDBulbs_DTO
                                    {
                                        NCMC715WCF_Id = a.NCMC715WCF_Id,
                                        MI_Id = a.MI_Id,
                                        NCMC715WCF_Year = a.NCMC715WCF_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCMC715WCF_RainWaterHarvestingFlag = a.NCMC715WCF_RainWaterHarvestingFlag,
                                        NCMC715WCF_BorewellOpenwellRecFlag = a.NCMC715WCF_BorewellOpenwellRecFlag,
                                        NCMC715WCF_StatusFlg = a.NCMC715WCF_StatusFlg,
                                        NCMC715WCF_ConstructionOftanksbundsFlag = a.NCMC715WCF_ConstructionOftanksbundsFlag,
                                        NCMC715WCF_MaintenanceOfWaterbodiesDSFlag = a.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag,
                                        NCMC715WCF_WastewaterrecyclingFlag = a.NCMC715WCF_WastewaterrecyclingFlag,
                                        NCMC715WCF_ActiveFlg = a.NCMC715WCF_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_714_LEDBulbs_DTO saveMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCMC715WCF_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_715_WaterConservFacilitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC715WCF_Year == data.NCMC715WCF_Year && t.NCMC715WCF_RainWaterHarvestingFlag == data.NCMC715WCF_RainWaterHarvestingFlag && t.NCMC715WCF_BorewellOpenwellRecFlag == data.NCMC715WCF_BorewellOpenwellRecFlag && t.NCMC715WCF_ConstructionOftanksbundsFlag == data.NCMC715WCF_ConstructionOftanksbundsFlag && t.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag == data.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag && t.NCMC715WCF_WastewaterrecyclingFlag == data.NCMC715WCF_WastewaterrecyclingFlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_715_WaterConservFacilitiesDMO obj1 = new NAAC_MC_715_WaterConservFacilitiesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC715WCF_Year = data.NCMC715WCF_Year;
                        obj1.NCMC715WCF_RainWaterHarvestingFlag = data.NCMC715WCF_RainWaterHarvestingFlag;
                        obj1.NCMC715WCF_BorewellOpenwellRecFlag = data.NCMC715WCF_BorewellOpenwellRecFlag;
                        obj1.NCMC715WCF_ConstructionOftanksbundsFlag = data.NCMC715WCF_ConstructionOftanksbundsFlag;
                        obj1.NCMC715WCF_WastewaterrecyclingFlag = data.NCMC715WCF_WastewaterrecyclingFlag;
                        obj1.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag = data.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag;
                        obj1.NCMC715WCF_ActiveFlg = true;
                        obj1.NCMC715WCF_StatusFlg = "";
                        obj1.NCMC715WCF_CreatedBy = data.UserId;
                        obj1.NCMC715WCF_UpdatedBy = data.UserId;
                        obj1.NCMC715WCF_CreatedDate = DateTime.Now;
                        obj1.NCMC715WCF_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCMC715WCF_Id;
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
                else if (data.NCMC715WCF_Id > 0)
                {
                    var update = _GeneralContext.NAAC_MC_715_WaterConservFacilitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC715WCF_Id == data.NCMC715WCF_Id).SingleOrDefault();

                    update.NCMC715WCF_Year = data.NCMC715WCF_Year;
                    update.NCMC715WCF_RainWaterHarvestingFlag = data.NCMC715WCF_RainWaterHarvestingFlag;
                    update.NCMC715WCF_BorewellOpenwellRecFlag = data.NCMC715WCF_BorewellOpenwellRecFlag;
                    update.NCMC715WCF_ConstructionOftanksbundsFlag = data.NCMC715WCF_ConstructionOftanksbundsFlag;
                    update.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag = data.NCMC715WCF_MaintenanceOfWaterbodiesDSFlag;
                    update.NCMC715WCF_WastewaterrecyclingFlag = data.NCMC715WCF_WastewaterrecyclingFlag;
                    update.NCMC715WCF_UpdatedBy = data.UserId;
                    update.NCMC715WCF_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCMC715WCF_Id;
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

        public NAAC_AC_714_LEDBulbs_DTO deactivateMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_715_WaterConservFacilitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC715WCF_Id == data.NCMC715WCF_Id).SingleOrDefault();
                if (result.NCMC715WCF_ActiveFlg == true)
                {
                    result.NCMC715WCF_ActiveFlg = false;
                }
                else if (result.NCMC715WCF_ActiveFlg == false)
                {
                    result.NCMC715WCF_ActiveFlg = true;
                }
                result.NCMC715WCF_UpdatedDate = DateTime.Now;
                result.NCMC715WCF_UpdatedBy = data.UserId;

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

        public NAAC_AC_714_LEDBulbs_DTO EditDataMCwater(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_MC_715_WaterConservFacilitiesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC715WCF_Id == data.NCMC715WCF_Id).ToList();
                data.editlisttab1 = edit.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public NAAC_AC_714_LEDBulbs_DTO getDataMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_MC_716_GreenCampusInitiativesDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCMC716GCI_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_714_LEDBulbs_DTO
                                    {
                                        NCMC716GCI_Id = a.NCMC716GCI_Id,
                                        MI_Id = a.MI_Id,
                                        NCMC716GCI_Year = a.NCMC716GCI_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCMC716GCI_RestrictedentryOfAutomobilesFlag = a.NCMC716GCI_RestrictedentryOfAutomobilesFlag,
                                        NCMC716GCI_BatterypoweredvehiclesFlag = a.NCMC716GCI_BatterypoweredvehiclesFlag,
                                        NCMC716GCI_PedestrianFriendlyPathwaysFlag = a.NCMC716GCI_PedestrianFriendlyPathwaysFlag,
                                        NCMC716GCI_BanOnTheuseOfPlasticsFlag = a.NCMC716GCI_BanOnTheuseOfPlasticsFlag,
                                        NCMC716GCI_LandscapingwithtreesplantsFlag = a.NCMC716GCI_LandscapingwithtreesplantsFlag,
                                        NCMC716GCI_ActiveFlg = a.NCMC716GCI_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_714_LEDBulbs_DTO saveMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCMC716GCI_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_716_GreenCampusInitiativesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716GCI_Year == data.NCMC716GCI_Year && t.NCMC716GCI_RestrictedentryOfAutomobilesFlag == data.NCMC716GCI_RestrictedentryOfAutomobilesFlag && t.NCMC716GCI_BatterypoweredvehiclesFlag == data.NCMC716GCI_BatterypoweredvehiclesFlag && t.NCMC716GCI_PedestrianFriendlyPathwaysFlag == data.NCMC716GCI_PedestrianFriendlyPathwaysFlag && t.NCMC716GCI_BanOnTheuseOfPlasticsFlag == data.NCMC716GCI_BanOnTheuseOfPlasticsFlag && t.NCMC716GCI_LandscapingwithtreesplantsFlag == data.NCMC716GCI_LandscapingwithtreesplantsFlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_716_GreenCampusInitiativesDMO obj1 = new NAAC_MC_716_GreenCampusInitiativesDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC716GCI_Year = data.NCMC716GCI_Year;
                        obj1.NCMC716GCI_RestrictedentryOfAutomobilesFlag = data.NCMC716GCI_RestrictedentryOfAutomobilesFlag;
                        obj1.NCMC716GCI_BatterypoweredvehiclesFlag = data.NCMC716GCI_BatterypoweredvehiclesFlag;
                        obj1.NCMC716GCI_PedestrianFriendlyPathwaysFlag = data.NCMC716GCI_PedestrianFriendlyPathwaysFlag;
                        obj1.NCMC716GCI_BanOnTheuseOfPlasticsFlag = data.NCMC716GCI_BanOnTheuseOfPlasticsFlag;
                        obj1.NCMC716GCI_LandscapingwithtreesplantsFlag = data.NCMC716GCI_LandscapingwithtreesplantsFlag;
                        obj1.NCMC716GCI_ActiveFlg = true;
                        obj1.NCMC716GCI_CreatedBy = data.UserId;
                        obj1.NCMC716GCI_UpdatedBy = data.UserId;
                        obj1.NCMC716GCI_CreatedDate = DateTime.Now;
                        obj1.NCMC716GCI_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCMC716GCI_Id;
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
                else if (data.NCMC716GCI_Id > 0)
                {
                    var update = _GeneralContext.NAAC_MC_716_GreenCampusInitiativesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716GCI_Id == data.NCMC716GCI_Id).SingleOrDefault();
                    update.NCMC716GCI_Year = data.NCMC716GCI_Year;
                    update.NCMC716GCI_RestrictedentryOfAutomobilesFlag = data.NCMC716GCI_RestrictedentryOfAutomobilesFlag;
                    update.NCMC716GCI_BatterypoweredvehiclesFlag = data.NCMC716GCI_BatterypoweredvehiclesFlag;
                    update.NCMC716GCI_PedestrianFriendlyPathwaysFlag = data.NCMC716GCI_PedestrianFriendlyPathwaysFlag;
                    update.NCMC716GCI_BanOnTheuseOfPlasticsFlag = data.NCMC716GCI_BanOnTheuseOfPlasticsFlag;
                    update.NCMC716GCI_LandscapingwithtreesplantsFlag = data.NCMC716GCI_LandscapingwithtreesplantsFlag;
                    update.NCMC716GCI_UpdatedBy = data.UserId;
                    update.NCMC716GCI_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCMC716GCI_Id;
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

        public NAAC_AC_714_LEDBulbs_DTO deactivateMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_716_GreenCampusInitiativesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716GCI_Id == data.NCMC716GCI_Id).SingleOrDefault();
                if (result.NCMC716GCI_ActiveFlg == true)
                {
                    result.NCMC716GCI_ActiveFlg = false;
                }
                else if (result.NCMC716GCI_ActiveFlg == false)
                {
                    result.NCMC716GCI_ActiveFlg = true;
                }
                result.NCMC716GCI_UpdatedDate = DateTime.Now;
                result.NCMC716GCI_UpdatedBy = data.UserId;

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

        public NAAC_AC_714_LEDBulbs_DTO EditDataMCgreen(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_MC_716_GreenCampusInitiativesDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716GCI_Id == data.NCMC716GCI_Id).ToList();
                data.editlisttab1 = edit.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_714_LEDBulbs_DTO getDataMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_MC_717_DisabledFriendlyEnvironmentDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCMC717DFE_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_714_LEDBulbs_DTO
                                    {
                                        NCMC717DFE_Id = a.NCMC717DFE_Id,
                                        MI_Id = a.MI_Id,
                                        NCMC717DFE_Year = a.NCMC717DFE_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCMC717DFE_BuiltEnvwithRampsORLiftsFlag = a.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag,
                                        NCMC717DFE_DisabledFriendlyWashroomsFlag = a.NCMC717DFE_DisabledFriendlyWashroomsFlag,
                                        NCMC717DFE_AssistiveTechnologyFacfacMEFlag = a.NCMC717DFE_AssistiveTechnologyFacfacMEFlag,
                                        NCMC717DFE_SignageIncTactilePathssignpostsFlag = a.NCMC717DFE_SignageIncTactilePathssignpostsFlag,
                                        NCMC717DFE_ProvisionForEnquiryScreenReadingFlag = a.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag,
                                        NCMC717DFE_ActiveFlg = a.NCMC717DFE_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_714_LEDBulbs_DTO saveMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCMC717DFE_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_717_DisabledFriendlyEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC717DFE_Year == data.NCMC717DFE_Year && t.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag == data.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag && t.NCMC717DFE_DisabledFriendlyWashroomsFlag == data.NCMC717DFE_DisabledFriendlyWashroomsFlag && t.NCMC717DFE_SignageIncTactilePathssignpostsFlag == data.NCMC717DFE_SignageIncTactilePathssignpostsFlag && t.NCMC717DFE_AssistiveTechnologyFacfacMEFlag == data.NCMC717DFE_AssistiveTechnologyFacfacMEFlag && t.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag == data.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_717_DisabledFriendlyEnvironmentDMO obj1 = new NAAC_MC_717_DisabledFriendlyEnvironmentDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC717DFE_Year = data.NCMC717DFE_Year;
                        obj1.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag = data.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag;
                        obj1.NCMC717DFE_DisabledFriendlyWashroomsFlag = data.NCMC717DFE_DisabledFriendlyWashroomsFlag;
                        obj1.NCMC717DFE_SignageIncTactilePathssignpostsFlag = data.NCMC717DFE_SignageIncTactilePathssignpostsFlag;
                        obj1.NCMC717DFE_AssistiveTechnologyFacfacMEFlag = data.NCMC717DFE_AssistiveTechnologyFacfacMEFlag;
                        obj1.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag = data.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag;
                        obj1.NCMC717DFE_ActiveFlg = true;
                        obj1.NCMC717DFE_CreatedBy = data.UserId;
                        obj1.NCMC717DFE_UpdatedBy = data.UserId;
                        obj1.NCMC717DFE_CreatedDate = DateTime.Now;
                        obj1.NCMC717DFE_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCMC717DFE_Id;
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
                else if (data.NCMC717DFE_Id > 0)
                {
                    var update = _GeneralContext.NAAC_MC_717_DisabledFriendlyEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC717DFE_Id == data.NCMC717DFE_Id).SingleOrDefault();
                    update.NCMC717DFE_Year = data.NCMC717DFE_Year;
                    update.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag = data.NCMC717DFE_BuiltEnvwithRampsORLiftsFlag;
                    update.NCMC717DFE_DisabledFriendlyWashroomsFlag = data.NCMC717DFE_DisabledFriendlyWashroomsFlag;
                    update.NCMC717DFE_SignageIncTactilePathssignpostsFlag = data.NCMC717DFE_SignageIncTactilePathssignpostsFlag;
                    update.NCMC717DFE_AssistiveTechnologyFacfacMEFlag = data.NCMC717DFE_AssistiveTechnologyFacfacMEFlag;
                    update.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag = data.NCMC717DFE_ProvisionForEnquiryScreenReadingFlag;
                    update.NCMC717DFE_UpdatedBy = data.UserId;
                    update.NCMC717DFE_CreatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCMC717DFE_Id;
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

        public NAAC_AC_714_LEDBulbs_DTO deactivateMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_717_DisabledFriendlyEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC717DFE_Id == data.NCMC717DFE_Id).SingleOrDefault();
                if (result.NCMC717DFE_ActiveFlg == true)
                {
                    result.NCMC717DFE_ActiveFlg = false;
                }
                else if (result.NCMC717DFE_ActiveFlg == false)
                {
                    result.NCMC717DFE_ActiveFlg = true;
                }
                result.NCMC717DFE_UpdatedDate = DateTime.Now;
                result.NCMC717DFE_UpdatedBy = data.UserId;

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

        public NAAC_AC_714_LEDBulbs_DTO EditDataMCdisable(NAAC_AC_714_LEDBulbs_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_MC_717_DisabledFriendlyEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC717DFE_Id == data.NCMC717DFE_Id).ToList();
                data.editlisttab1 = edit.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //MC
    }
}
