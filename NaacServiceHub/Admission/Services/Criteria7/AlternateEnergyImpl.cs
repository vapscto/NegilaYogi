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
    public class AlternateEnergyImpl : Interface.Criteria7.AlternateEnergyInterface
    {

        public GeneralContext _GeneralContext;
        public AlternateEnergyImpl(GeneralContext para)
        {
            _GeneralContext = para;
        }

        public async  Task<NAAC_AC_713_AlternateEnergy_DTO> loaddata(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                data.institutionlist = (from a in _GeneralContext.Institution
                                        from b in _GeneralContext.UserRoleWithInstituteDMO
                                        where (a.MI_Id == b.MI_Id && a.MI_ActiveFlag == 1 && b.Id == data.UserId)
                                        select new NAAC_AC_713_AlternateEnergy_DTO
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

        public NAAC_AC_713_AlternateEnergy_DTO getData(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_AC_713_AlternateEnergyDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCAC713ALTENE_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_713_AlternateEnergy_DTO
                                    {
                                        NCAC713ALTENE_Id = a.NCAC713ALTENE_Id,
                                        NCAC713ALTENE_Year = a.NCAC713ALTENE_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCAC713ALTENE_PowerRequirements = a.NCAC713ALTENE_PowerRequirements,
                                        NCAC713ALTENE_TotalPowerReq = a.NCAC713ALTENE_TotalPowerReq,
                                        NCAC713ALTENE_EnergySource = a.NCAC713ALTENE_EnergySource,
                                        NCAC713ALTENE_EnergyUsed = a.NCAC713ALTENE_EnergyUsed,
                                        NCAC713ALTENE_EnergySupplied = a.NCAC713ALTENE_EnergySupplied,
                                        NCAC713ALTENE_ActiveFlg = a.NCAC713ALTENE_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_713_AlternateEnergy_DTO savedatatab1(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCAC713ALTENE_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_AC_713_AlternateEnergyDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC713ALTENE_Year == data.NCAC713ALTENE_Year && t.NCAC713ALTENE_PowerRequirements == data.NCAC713ALTENE_PowerRequirements && t.NCAC713ALTENE_TotalPowerReq == data.NCAC713ALTENE_TotalPowerReq && t.NCAC713ALTENE_EnergySource == data.NCAC713ALTENE_EnergySource && t.NCAC713ALTENE_EnergyUsed == data.NCAC713ALTENE_EnergyUsed && t.NCAC713ALTENE_EnergySupplied == data.NCAC713ALTENE_EnergySupplied).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_AC_713_AlternateEnergyDMO obj1 = new NAAC_AC_713_AlternateEnergyDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCAC713ALTENE_Year = data.NCAC713ALTENE_Year;
                        obj1.NCAC713ALTENE_PowerRequirements = data.NCAC713ALTENE_PowerRequirements;
                        obj1.NCAC713ALTENE_TotalPowerReq = data.NCAC713ALTENE_TotalPowerReq;
                        obj1.NCAC713ALTENE_EnergySource = data.NCAC713ALTENE_EnergySource;
                        obj1.NCAC713ALTENE_EnergyUsed = data.NCAC713ALTENE_EnergyUsed;
                        obj1.NCAC713ALTENE_EnergySupplied = data.NCAC713ALTENE_EnergySupplied;
                        obj1.NCAC713ALTENE_ActiveFlg = true;
                        obj1.NCAC713ALTENE_CreatedBy = data.UserId;
                        obj1.NCAC713ALTENE_UpdatedBy = data.UserId;
                        obj1.NCAC713ALTENE_CreatedDate = DateTime.Now;
                        obj1.NCAC713ALTENE_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCAC713ALTENE_Id;
                        if (data.NAACAC7DTO.Count() > 0)
                        {
                            foreach (NAAC_AC_713_AlternateEnergy_DTO DocumentsDTO in data.NAACAC7DTO)
                            {
                                NAAC_AC_713_AlternateEnergy_FilesDMO obj2 = new NAAC_AC_713_AlternateEnergy_FilesDMO();
                                obj2.NCAC713ALTENEF_FileName = DocumentsDTO.NCAC713ALTENEF_FileName;
                                obj2.NCAC713ALTENEF_Filedesc = DocumentsDTO.NCAC713ALTENEF_Filedesc;
                                obj2.NCAC713ALTENEF_FilePath = DocumentsDTO.NCAC713ALTENEF_FilePath;
                                obj2.NCAC713ALTENE_Id = s;
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
                else if (data.NCAC713ALTENE_Id > 0)
                {
                    var update = _GeneralContext.NAAC_AC_713_AlternateEnergyDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC713ALTENE_Id == data.NCAC713ALTENE_Id).SingleOrDefault();

                    update.NCAC713ALTENE_Year = data.NCAC713ALTENE_Year;
                    update.NCAC713ALTENE_PowerRequirements = data.NCAC713ALTENE_PowerRequirements;
                    update.NCAC713ALTENE_TotalPowerReq = data.NCAC713ALTENE_TotalPowerReq;
                    update.NCAC713ALTENE_EnergySource = data.NCAC713ALTENE_EnergySource;
                    update.NCAC713ALTENE_EnergyUsed = data.NCAC713ALTENE_EnergyUsed;
                    update.NCAC713ALTENE_EnergySupplied = data.NCAC713ALTENE_EnergySupplied;
                    update.NCAC713ALTENE_UpdatedBy = data.UserId;
                    update.NCAC713ALTENE_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCAC713ALTENE_Id;
                    if (data.NAACAC7DTO.Count() > 0)
                    {
                        foreach (NAAC_AC_713_AlternateEnergy_DTO DocumentsDTO in data.NAACAC7DTO)
                        {
                            if (DocumentsDTO.NCAC713ALTENEF_Id > 0)
                            {
                                var filesdata = _GeneralContext.NAAC_AC_713_AlternateEnergy_FilesDMO.Where(t => t.NCAC713ALTENEF_Id == DocumentsDTO.NCAC713ALTENEF_Id).FirstOrDefault();
                                filesdata.NCAC713ALTENEF_Filedesc = DocumentsDTO.NCAC713ALTENEF_Filedesc;
                                filesdata.NCAC713ALTENEF_FileName = DocumentsDTO.NCAC713ALTENEF_FileName;
                                filesdata.NCAC713ALTENEF_FilePath = DocumentsDTO.NCAC713ALTENEF_FilePath;
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
                                NAAC_AC_713_AlternateEnergy_FilesDMO obj2 = new NAAC_AC_713_AlternateEnergy_FilesDMO();
                                obj2.NCAC713ALTENEF_FileName = DocumentsDTO.NCAC713ALTENEF_FileName;
                                obj2.NCAC713ALTENEF_Filedesc = DocumentsDTO.NCAC713ALTENEF_Filedesc;
                                obj2.NCAC713ALTENEF_FilePath = DocumentsDTO.NCAC713ALTENEF_FilePath;
                                obj2.NCAC713ALTENE_Id = s;
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

                    //var duplicate = _GeneralContext.NAAC_AC_713_AlternateEnergyDMO.Where(t => t.MI_Id == data.MI_Id && t.NCAC713ALTENE_Year == data.NCAC713ALTENE_Year && t.NCAC713ALTENE_PowerRequirements == data.NCAC713ALTENE_PowerRequirements && t.NCAC713ALTENE_TotalPowerReq == data.NCAC713ALTENE_TotalPowerReq && t.NCAC713ALTENE_EnergySource == data.NCAC713ALTENE_EnergySource && t.NCAC713ALTENE_EnergyUsed == data.NCAC713ALTENE_EnergyUsed && t.NCAC713ALTENE_EnergySupplied == data.NCAC713ALTENE_EnergySupplied).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.duplicate = true;
                    //}
                    //else
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_713_AlternateEnergy_DTO deactivYTab1(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_AC_713_AlternateEnergyDMO.Where(t =>t.NCAC713ALTENE_Id == data.NCAC713ALTENE_Id).SingleOrDefault();

                if (result.NCAC713ALTENE_ActiveFlg == true)
                {
                    result.NCAC713ALTENE_ActiveFlg = false;
                }
                else if (result.NCAC713ALTENE_ActiveFlg == false)
                {
                    result.NCAC713ALTENE_ActiveFlg = true;
                }

                result.NCAC713ALTENE_UpdatedDate = DateTime.Now;
                result.NCAC713ALTENE_UpdatedBy = data.UserId;

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

        public NAAC_AC_713_AlternateEnergy_DTO editTab1(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_AC_713_AlternateEnergyDMO.Where(t =>t.NCAC713ALTENE_Id == data.NCAC713ALTENE_Id).ToList();
                var editfile = _GeneralContext.NAAC_AC_713_AlternateEnergy_FilesDMO.Where(t => t.NCAC713ALTENE_Id == data.NCAC713ALTENE_Id).ToList();

                data.editlisttab1 = edit.ToArray();
                data.editfilelist = editfile.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_713_AlternateEnergy_DTO deleteuploadfile(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                List<NAAC_AC_713_AlternateEnergy_FilesDMO> removelist = new List<NAAC_AC_713_AlternateEnergy_FilesDMO>();
                removelist = _GeneralContext.NAAC_AC_713_AlternateEnergy_FilesDMO.Where(t => t.NCAC713ALTENEF_Id == data.NCAC713ALTENEF_Id).ToList();
                foreach (NAAC_AC_713_AlternateEnergy_FilesDMO obj1 in removelist)
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

        public NAAC_AC_713_AlternateEnergy_DTO getDataMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                data.allacademicyear = _GeneralContext.Academic.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.alldatalist = (from a in _GeneralContext.NAAC_MC_713_AlternateEnergyDMO
                                    from b in _GeneralContext.Academic
                                    where (a.MI_Id == data.MI_Id && a.NCMC713ALTENE_Year == b.ASMAY_Id && b.MI_Id == a.MI_Id)
                                    select new NAAC_AC_713_AlternateEnergy_DTO
                                    {
                                        NCMC713ALTENE_Id = a.NCMC713ALTENE_Id,
                                        NCMC713ALTENE_Year = a.NCMC713ALTENE_Year,
                                        ASMAY_Year = b.ASMAY_Year,
                                        NCMC713ALTENE_SolarenergyFlag = a.NCMC713ALTENE_SolarenergyFlag,
                                        NCMC713ALTENE_WindenergyFlag = a.NCMC713ALTENE_WindenergyFlag,
                                        NCMC713ALTENE_SensorbasedEnergyFlag = a.NCMC713ALTENE_SensorbasedEnergyFlag,
                                        NCMC713ALTENE_BiogasPlantFlag = a.NCMC713ALTENE_BiogasPlantFlag,
                                        NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag = a.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag,
                                        NCMC713ALTENE_ActiveFlg = a.NCMC713ALTENE_ActiveFlg
                                    }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_713_AlternateEnergy_DTO savedatatabMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            long s = 0;
            try
            {
                if (data.NCMC713ALTENE_Id == 0)
                {
                    var duplicate = _GeneralContext.NAAC_MC_713_AlternateEnergyDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC713ALTENE_Year == data.NCMC713ALTENE_Year && t.NCMC713ALTENE_SolarenergyFlag == data.NCMC713ALTENE_SolarenergyFlag && t.NCMC713ALTENE_WindenergyFlag == data.NCMC713ALTENE_WindenergyFlag && t.NCMC713ALTENE_SensorbasedEnergyFlag == data.NCMC713ALTENE_SensorbasedEnergyFlag && t.NCMC713ALTENE_BiogasPlantFlag == data.NCMC713ALTENE_BiogasPlantFlag && t.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag == data.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        NAAC_MC_713_AlternateEnergyDMO obj1 = new NAAC_MC_713_AlternateEnergyDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.NCMC713ALTENE_Year = data.NCMC713ALTENE_Year;
                        obj1.NCMC713ALTENE_SolarenergyFlag = data.NCMC713ALTENE_SolarenergyFlag;
                        obj1.NCMC713ALTENE_WindenergyFlag = data.NCMC713ALTENE_WindenergyFlag;
                        obj1.NCMC713ALTENE_SensorbasedEnergyFlag = data.NCMC713ALTENE_SensorbasedEnergyFlag;
                        obj1.NCMC713ALTENE_BiogasPlantFlag = data.NCMC713ALTENE_BiogasPlantFlag;
                        obj1.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag = data.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag;
                        obj1.NCMC713ALTENE_ActiveFlg = true;
                        obj1.NCMC713ALTENE_UpdatedBy = data.UserId;
                        obj1.NCMC713ALTENE_CreatedBy = data.UserId;
                        obj1.NCMC713ALTENE_CreatedDate = DateTime.Now;
                        obj1.NCMC713ALTENE_UpdatedDate = DateTime.Now;
                        _GeneralContext.Add(obj1);
                        _GeneralContext.SaveChanges();
                        s = obj1.NCMC713ALTENE_Id;
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
                else if (data.NCMC713ALTENE_Id > 0)
                {
                    var update = _GeneralContext.NAAC_MC_713_AlternateEnergyDMO.Where(t => t.MI_Id == data.MI_Id && t.NCMC713ALTENE_Id == data.NCMC713ALTENE_Id).SingleOrDefault();
                    update.NCMC713ALTENE_Year = data.NCMC713ALTENE_Year;
                    update.NCMC713ALTENE_SolarenergyFlag = data.NCMC713ALTENE_SolarenergyFlag;
                    update.NCMC713ALTENE_WindenergyFlag = data.NCMC713ALTENE_WindenergyFlag;
                    update.NCMC713ALTENE_SensorbasedEnergyFlag = data.NCMC713ALTENE_SensorbasedEnergyFlag;
                    update.NCMC713ALTENE_BiogasPlantFlag = data.NCMC713ALTENE_BiogasPlantFlag;
                    update.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag = data.NCMC713ALTENE_LEDbulbsORPowerefficEquipFlag;
                    update.NCMC713ALTENE_UpdatedBy = data.UserId;
                    update.NCMC713ALTENE_UpdatedDate = DateTime.Now;
                    _GeneralContext.Update(update);
                    _GeneralContext.SaveChanges();
                    s = update.NCMC713ALTENE_Id;
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

        public NAAC_AC_713_AlternateEnergy_DTO editTabMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                var edit = _GeneralContext.NAAC_MC_713_AlternateEnergyDMO.Where(t => t.NCMC713ALTENE_Id == data.NCMC713ALTENE_Id).ToList();
                data.editlisttab1 = edit.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public NAAC_AC_713_AlternateEnergy_DTO deactivateMC(NAAC_AC_713_AlternateEnergy_DTO data)
        {
            try
            {
                var result = _GeneralContext.NAAC_MC_713_AlternateEnergyDMO.Where(t => t.NCMC713ALTENE_Id == data.NCMC713ALTENE_Id).SingleOrDefault();

                if (result.NCMC713ALTENE_ActiveFlg == true)
                {
                    result.NCMC713ALTENE_ActiveFlg = false;
                }
                else if (result.NCMC713ALTENE_ActiveFlg == false)
                {
                    result.NCMC713ALTENE_ActiveFlg = true;
                }
                result.NCMC713ALTENE_UpdatedDate = DateTime.Now;
                result.NCMC713ALTENE_UpdatedBy = data.UserId;

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
    }
}
