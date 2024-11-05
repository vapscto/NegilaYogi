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
    public class InstituteWiseMandatorysettingsImpl : Interfaces.InstituteWiseMandatorysettingsInterface
    {
        public DomainModelMsSqlServerContext _Context;
        public InstituteWiseMandatorysettingsImpl( DomainModelMsSqlServerContext Context)
        {
            _Context = Context;
        }
        public IVRM_Mandatory_Setting_IWDTO getBasicData(IVRM_Mandatory_Setting_IWDTO dto)
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

        public IVRM_Mandatory_Setting_IWDTO getPagedetailsBySelection(IVRM_Mandatory_Setting_IWDTO dto)
        {

            dto.retrunMsg = "";
            List<IVRM_Mandatory_Setting_IW> lorg = new List<IVRM_Mandatory_Setting_IW>();
            try
            {

                lorg = (from ms in _Context.IVRM_Mandatory_Setting
                        where (ms.IVRMP_Id.Equals(dto.IVRMP_Id))
                        select new IVRM_Mandatory_Setting_IW
                        {
                            IVRMP_Id = ms.IVRMP_Id,
                            IVRMMSI_FieldName = ms.IVRMMS_FieldName,
                            IVRMMSI_Ngmodel = ms.IVRMMS_Ngmodel,
                            IVRMMSI_MandatoryFlag = ms.IVRMMS_MandatoryFlag
                        }).ToList();
                dto.pageList = lorg.ToArray();
                return dto;


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }
            return dto;
        }

        public IVRM_Mandatory_Setting_IWDTO SaveUpdate(IVRM_Mandatory_Setting_IWDTO dto)
        {
            dto.retrunMsg = "";
            int count = 0;
            List<IVRM_Mandatory_Setting_IW> DeletableRecords = new List<IVRM_Mandatory_Setting_IW>();
            try
            {
                if (dto.mandatoryfieldList.Count() > 0)
                {

                    var DistinctIVRMMS_Id = dto.mandatoryfieldList.Select(t => t.IVRMMSI_Id).Distinct();

                    DeletableRecords = _Context.IVRM_Mandatory_Setting_IW.Where(i => i.IVRMP_Id.Equals(dto.IVRMP_Id) && i.MI_Id.Equals(dto.MI_Id)).ToList();

                    DeletableRecords = DeletableRecords.Where(i => !DistinctIVRMMS_Id.Contains(i.IVRMMSI_Id) && i.IVRMP_Id.Equals(dto.IVRMP_Id) && i.MI_Id.Equals(dto.MI_Id)).ToList();
                    if (DeletableRecords.Any())
                    {
                        foreach (IVRM_Mandatory_Setting_IW Record in DeletableRecords)
                        {
                            _Context.Remove(Record);

                        }
                        var Exists = _Context.SaveChanges();
                    }

                    foreach (IVRM_Mandatory_Setting_IWDTO mandatoryfieldDTO in dto.mandatoryfieldList)
                    {
                        mandatoryfieldDTO.IVRMP_Id = dto.IVRMP_Id;
                        mandatoryfieldDTO.MI_Id = dto.MI_Id;
                        IVRM_Mandatory_Setting_IW mandatoryfield = Mapper.Map<IVRM_Mandatory_Setting_IW>(mandatoryfieldDTO);

                        if (mandatoryfield.IVRMMSI_Id > 0)
                        {
                            var mandatoryfieldresult = _Context.IVRM_Mandatory_Setting_IW.Single(t => t.IVRMMSI_Id == mandatoryfieldDTO.IVRMMSI_Id);

                            mandatoryfieldresult.UpdatedDate = DateTime.Now;
                            Mapper.Map(mandatoryfieldDTO, mandatoryfieldresult);
                            _Context.Update(mandatoryfieldresult);
                            count = _Context.SaveChanges();
                        }
                        else
                        {
                            mandatoryfield.CreatedDate = DateTime.Now;
                            mandatoryfield.UpdatedDate = DateTime.Now;
                            _Context.Add(mandatoryfield);
                            count = _Context.SaveChanges();
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
        public IVRM_Mandatory_Setting_IWDTO editData(IVRM_Mandatory_Setting_IWDTO dto)
        {

            dto.retrunMsg = "";
            List<IVRM_Mandatory_Setting_IW> lorg = new List<IVRM_Mandatory_Setting_IW>();
            try
            {

                lorg = _Context.IVRM_Mandatory_Setting_IW.AsNoTracking().Where(t => t.IVRMP_Id.Equals(dto.IVRMP_Id) && t.MI_Id.Equals(dto.MI_Id)).ToList();

                if (lorg.Count() > 0)
                {
                    dto.pageList = lorg.ToArray();
                    return dto;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error";
            }

            return dto;
        }
        public IVRM_Mandatory_Setting_IWDTO deactivate(IVRM_Mandatory_Setting_IWDTO dto)
        {
            dto.retrunMsg = "";
            List<IVRM_Mandatory_Setting_IW> lorg = new List<IVRM_Mandatory_Setting_IW>();

            try
            {

                lorg = _Context.IVRM_Mandatory_Setting_IW.Where(t => t.IVRMP_Id.Equals(dto.IVRMP_Id) && t.MI_Id.Equals(dto.MI_Id)).ToList();

                if (lorg.Any())
                {
                    foreach (IVRM_Mandatory_Setting_IW Record in lorg)
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

        public IVRM_Mandatory_Setting_IWDTO GetAllDropdownAndDatatableDetails(IVRM_Mandatory_Setting_IWDTO dto)
        {
            List<IVRM_Mandatory_Setting_IW> datalist = new List<IVRM_Mandatory_Setting_IW>();

            List<MasterPage> pagedropdown = new List<MasterPage>();
            List<MasterPage> pageList = new List<MasterPage>();

            List<Institution> institutiondropdown = new List<Institution>();
            try
            {
                pagedropdown = (from mp in _Context.masterPage
                               from ms in _Context.IVRM_Mandatory_Setting
                               where (mp.IVRMP_Id == ms.IVRMP_Id && mp.IVRMP_MandatoryFlag.Equals(true)) select mp).Distinct().ToList();
                dto.pagedropdown = pagedropdown.ToArray();

                institutiondropdown = _Context.Institution.Where(t => t.MI_ActiveFlag == 1).ToList();
                dto.institutiondropdown = institutiondropdown.ToArray();



              var  pageList1 = (from ms in _Context.IVRM_Mandatory_Setting_IW
                                from mi in _Context.Institution
                                from mp in _Context.masterPage
                                where ms.MI_Id == mi.MI_Id && ms.IVRMP_Id == mp.IVRMP_Id
                                select new IVRM_Mandatory_Setting_IWDTO {
                                        MI_Id = mi.MI_Id,
                                        MI_Name = mi.MI_Name,
                                        IVRMP_Id = mp.IVRMP_Id,
                                        IVRMMP_PageName = mp.IVRMMP_PageName

                            }).Distinct().ToList();

                //var result = pageList1.GroupBy(x => new { x.MI_Id, x.IVRMP_Id });

                dto.pageList = pageList1.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
