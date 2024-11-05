using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MandatorysettingsImpl : Interfaces.MandatorysettingsInterface
    {
        public DomainModelMsSqlServerContext _Context;
        public MandatorysettingsImpl(DomainModelMsSqlServerContext Context)
        {
            _Context = Context;
        }
        public IVRM_Mandatory_SettingDTO getBasicData(IVRM_Mandatory_SettingDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;
        }

        //Onchange

        public IVRM_Mandatory_SettingDTO getPagedetailsBySelection(IVRM_Mandatory_SettingDTO dto)
        {

            dto.retrunMsg = "";
            try
            {
                List<IVRM_Mandatory_Setting> lorg = new List<IVRM_Mandatory_Setting>();
                lorg = _Context.IVRM_Mandatory_Setting.AsNoTracking().Where(t => t.IVRMP_Id.Equals(dto.IVRMP_Id)).ToList();
                dto.pageList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        public IVRM_Mandatory_SettingDTO SaveUpdate(IVRM_Mandatory_SettingDTO dto)
        {
            dto.retrunMsg = "";
            int count = 0;
            List<IVRM_Mandatory_Setting> DeletableRecords = new List<IVRM_Mandatory_Setting>();
            try
            {
                if (dto.mandatoryfieldList.Count() > 0)
                {

                    var DistinctIVRMMS_Id = dto.mandatoryfieldList.Select(t=>t.IVRMMS_Id).Distinct();

                    DeletableRecords = _Context.IVRM_Mandatory_Setting.Where(i => i.IVRMP_Id.Equals(dto.IVRMP_Id)).ToList();

                    DeletableRecords = DeletableRecords.Where(i => !DistinctIVRMMS_Id.Contains(i.IVRMMS_Id) && i.IVRMP_Id.Equals(dto.IVRMP_Id)).ToList();
                    if (DeletableRecords.Any())
                    {
                        foreach (IVRM_Mandatory_Setting Record in DeletableRecords)
                        {
                            _Context.Remove(Record);

                        }
                        var Exists = _Context.SaveChanges();
                    }

                    foreach (IVRM_Mandatory_SettingDTO mandatoryfieldDTO in dto.mandatoryfieldList)
                    {
                        mandatoryfieldDTO.IVRMP_Id = dto.IVRMP_Id;
                        IVRM_Mandatory_Setting mandatoryfield = Mapper.Map<IVRM_Mandatory_Setting>(mandatoryfieldDTO);

                        if (mandatoryfield.IVRMMS_Id > 0)
                        {
                            var mandatoryfieldresult = _Context.IVRM_Mandatory_Setting.Single(t => t.IVRMMS_Id == mandatoryfieldDTO.IVRMMS_Id);
                            
                            mandatoryfieldresult.UpdatedDate = DateTime.Now;
                            Mapper.Map(mandatoryfieldDTO, mandatoryfieldresult);
                            _Context.Update(mandatoryfieldresult);
                            count= _Context.SaveChanges();
                        }
                        else
                        {
                            mandatoryfield.CreatedDate = DateTime.Now;
                            mandatoryfield.UpdatedDate = DateTime.Now;
                            _Context.Add(mandatoryfield);
                            count= _Context.SaveChanges();
                        }
                    }


                }
                if (count > 0)
                {
                    dto.retrunMsg = "Added";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;
        }
        public IVRM_Mandatory_SettingDTO editData(int id)
        {

            IVRM_Mandatory_SettingDTO dto = new IVRM_Mandatory_SettingDTO();
            dto.retrunMsg = "";
            try
            {
                List<IVRM_Mandatory_Setting> lorg = new List<IVRM_Mandatory_Setting>();
                lorg = _Context.IVRM_Mandatory_Setting.AsNoTracking().Where(t => t.IVRMP_Id.Equals(id)).ToList();
                dto.pageList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;
        }
        public IVRM_Mandatory_SettingDTO deactivate(IVRM_Mandatory_SettingDTO dto)
        {
            dto.retrunMsg = "";
            List<IVRM_Mandatory_Setting> lorg = new List<IVRM_Mandatory_Setting>();

            try
            {
                lorg = _Context.IVRM_Mandatory_Setting.Where(t => t.IVRMP_Id.Equals(dto.IVRMP_Id)).ToList();

                if (lorg.Any())
                {
                    foreach (IVRM_Mandatory_Setting Record in lorg)
                    {
                        _Context.Remove(Record);

                    }

                    var contactExists = _Context.SaveChanges();
                    if (contactExists == 1)
                    {

                        dto.retrunMsg = "Deleted";
                    }
                    else
                    {

                        dto.retrunMsg = "NotDeleted";
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error";
            }

            return dto;
        }

        public IVRM_Mandatory_SettingDTO GetAllDropdownAndDatatableDetails(IVRM_Mandatory_SettingDTO dto)
        {
            List<IVRM_Mandatory_Setting> datalist = new List<IVRM_Mandatory_Setting>();
           
            List<MasterPage> pagedropdown = new List<MasterPage>();
            List<MasterPage> pageList = new List<MasterPage>();
            try
            {
                pagedropdown = _Context.masterPage.Where(t=>t.IVRMP_MandatoryFlag.Equals(true)).ToList();
                dto.pagedropdown = pagedropdown.ToArray();


                pageList = (from ms in _Context.IVRM_Mandatory_Setting
                               from mp in _Context.masterPage
                               where ms.IVRMP_Id == mp.IVRMP_Id
                               select mp).Distinct().ToList();

                dto.pageList = pageList.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
