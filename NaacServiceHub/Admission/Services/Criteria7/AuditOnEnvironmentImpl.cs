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
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Services.Criteria7
{
    public class AuditOnEnvironmentImpl : Interface.Criteria7.AuditOnEnvironmentInterface
    {

        public GeneralContext _GeneralContext;
        public AuditOnEnvironmentImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_MC_716_AuditOnEnvironment_DTO> loaddata(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_MC_716_AuditOnEnvironment_DTO
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

        public NAAC_MC_716_AuditOnEnvironment_DTO savedatatab1(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCMC716AOE_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_716_AuditOnEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716AOE_Year == data.NCMC716AOE_Year && t.NCMC716AOE_GreenauditFlag == data.NCMC716AOE_GreenauditFlag && t.NCMC716AOE_EnergyAuditFlag == data.NCMC716AOE_EnergyAuditFlag && t.NCMC716AOE_EnvironmentAuditFlag == data.NCMC716AOE_EnvironmentAuditFlag && t.NCMC716AOE_CleanandgreenCampusRecognitionsFlag == data.NCMC716AOE_CleanandgreenCampusRecognitionsFlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_716_AuditOnEnvironmentDMO obj1 = new NAAC_MC_716_AuditOnEnvironmentDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC716AOE_Year = data.NCMC716AOE_Year;
                        obj1.NCMC716AOE_GreenauditFlag = data.NCMC716AOE_GreenauditFlag;
                        obj1.NCMC716AOE_EnergyAuditFlag = data.NCMC716AOE_EnergyAuditFlag;
                        obj1.NCMC716AOE_EnvironmentAuditFlag = data.NCMC716AOE_EnvironmentAuditFlag;
                        obj1.NCMC716AOE_CleanandgreenCampusRecognitionsFlag = data.NCMC716AOE_CleanandgreenCampusRecognitionsFlag;
                        obj1.NCMC716AOE_ActiveFlag = true;
                        obj1.NCMC716AOE_CreatedBy = data.UserId;
                        obj1.NCMC716AOE_UpdatedBy = data.UserId;
                        obj1.NCMC716AOE_CreatedDate = DateTime.Now;
                        obj1.NCMC716AOE_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCMC716AOE_Id;
                        if (data.NAACMC716DTO.Count() > 0)
                        {
                            foreach (NAAC_MC_716_AuditOnEnvironment_DTO DocumentsDTO in data.NAACMC716DTO)
                            {
                                NAAC_MC_716_AuditOnEnvironment_FilesDMO obj2 = new NAAC_MC_716_AuditOnEnvironment_FilesDMO();
                                obj2.NCMC716AOEF_FileName = DocumentsDTO.NCMC716AOEF_FileName;
                                obj2.NCMC716AOEF_Filedesc = DocumentsDTO.NCMC716AOEF_Filedesc;
                                obj2.NCMC716AOEF_FilePath = DocumentsDTO.NCMC716AOEF_FilePath;
                                obj2.NCMC716AOE_Id = s;
                                _GeneralContext.Add(obj2);
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
                        else if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.NCMC716AOE_Id > 0)
                {
                    var update = _GeneralContext.NAAC_MC_716_AuditOnEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716AOE_Id == data.NCMC716AOE_Id).SingleOrDefault();

                    update.NCMC716AOE_Year = data.NCMC716AOE_Year;
                    update.NCMC716AOE_GreenauditFlag = data.NCMC716AOE_GreenauditFlag;
                    update.NCMC716AOE_EnergyAuditFlag = data.NCMC716AOE_EnergyAuditFlag;
                    update.NCMC716AOE_EnvironmentAuditFlag = data.NCMC716AOE_EnvironmentAuditFlag;
                    update.NCMC716AOE_CleanandgreenCampusRecognitionsFlag = data.NCMC716AOE_CleanandgreenCampusRecognitionsFlag;
                    update.NCMC716AOE_UpdatedBy = data.UserId;
                    update.NCMC716AOE_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCMC716AOE_Id;
                    if (data.NAACMC716DTO.Count() > 0)
                    {
                        foreach (NAAC_MC_716_AuditOnEnvironment_DTO DocumentsDTO in data.NAACMC716DTO)
                        {
                            if (DocumentsDTO.NCMC716AOEF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_MC_716_AuditOnEnvironment_FilesDMO.Where(t => t.NCMC716AOEF_Id == DocumentsDTO.NCMC716AOEF_Id).FirstOrDefault();
                                filesdata.NCMC716AOEF_Filedesc = DocumentsDTO.NCMC716AOEF_Filedesc;
                                filesdata.NCMC716AOEF_FileName = DocumentsDTO.NCMC716AOEF_FileName;
                                filesdata.NCMC716AOEF_FilePath = DocumentsDTO.NCMC716AOEF_FilePath;
                                _GeneralContext.Update(filesdata);
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
                            else
                            {
                                NAAC_MC_716_AuditOnEnvironment_FilesDMO obj2 = new NAAC_MC_716_AuditOnEnvironment_FilesDMO();
                                obj2.NCMC716AOEF_FileName = DocumentsDTO.NCMC716AOEF_FileName;
                                obj2.NCMC716AOEF_Filedesc = DocumentsDTO.NCMC716AOEF_Filedesc;
                                obj2.NCMC716AOEF_FilePath = DocumentsDTO.NCMC716AOEF_FilePath;
                                obj2.NCMC716AOE_Id = s;
                                _GeneralContext.Add(obj2);
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
                    else if (s > 0)
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

        public NAAC_MC_716_AuditOnEnvironment_DTO deactivYTab1(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_716_AuditOnEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716AOE_Id == data.NCMC716AOE_Id).SingleOrDefault();

                if (result.NCMC716AOE_ActiveFlag == true)
                {
                    result.NCMC716AOE_ActiveFlag = false;
                }
                else if (result.NCMC716AOE_ActiveFlag == false)
                {
                    result.NCMC716AOE_ActiveFlag = true;
                }

                result.NCMC716AOE_UpdatedDate = DateTime.Now;
                result.NCMC716AOE_UpdatedBy = data.UserId;

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

        public NAAC_MC_716_AuditOnEnvironment_DTO editTab1(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_MC_716_AuditOnEnvironmentDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC716AOE_Id == data.NCMC716AOE_Id).ToList();
                var editfile = _GeneralContext.NAAC_MC_716_AuditOnEnvironment_FilesDMO.Where(t => t.NCMC716AOE_Id == data.NCMC716AOE_Id).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_MC_716_AuditOnEnvironment_DTO deleteuploadfile(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            try
            {
                List<NAAC_MC_716_AuditOnEnvironment_FilesDMO> removelist = new List<NAAC_MC_716_AuditOnEnvironment_FilesDMO>();
                removelist = _GeneralContext.NAAC_MC_716_AuditOnEnvironment_FilesDMO.Where(t => t.NCMC716AOEF_Id == data.NCMC716AOEF_Id).ToList();
                foreach(NAAC_MC_716_AuditOnEnvironment_FilesDMO obj1 in removelist)
                {
                    _GeneralContext.Remove(obj1);
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

        public NAAC_MC_716_AuditOnEnvironment_DTO getData(NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_MC_716_AuditOnEnvironmentDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCMC716AOE_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_MC_716_AuditOnEnvironment_DTO
                                    {
                                        NCMC716AOE_Id = a.NCMC716AOE_Id,
                                        MI_Id = a.MI_Id,
                                        NCMC716AOE_Year = a.NCMC716AOE_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCMC716AOE_GreenauditFlag = a.NCMC716AOE_GreenauditFlag,
                                        NCMC716AOE_EnergyAuditFlag = a.NCMC716AOE_EnergyAuditFlag,
                                        NCMC716AOE_EnvironmentAuditFlag = a.NCMC716AOE_EnvironmentAuditFlag,
                                        NCMC716AOE_CleanandgreenCampusRecognitionsFlag = a.NCMC716AOE_CleanandgreenCampusRecognitionsFlag,
                                        NCMC716AOE_ActiveFlag = a.NCMC716AOE_ActiveFlag
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
} 
