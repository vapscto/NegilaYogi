using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportMasterHouseDessignationImpl:Interfaces.SportMasterHouseDessignationInterface
    {
        private static ConcurrentDictionary<string, SPCC_Master_House_Designation_DTO> _login =
     new ConcurrentDictionary<string, SPCC_Master_House_Designation_DTO>();

        private readonly SportsContext _sportcontext;
        //private readonly castecategoryContext _castecategoryContext;
        private readonly ILogger<SportMasterHouseDessignationImpl> _log;


        public SportMasterHouseDessignationImpl(SportsContext sportcontext, ILogger<SportMasterHouseDessignationImpl> log)
        {
            _sportcontext = sportcontext;
            _log = log;
        }

        public SPCC_Master_House_Designation_DTO GetmastercasteData(SPCC_Master_House_Designation_DTO data)//int IVRMM_Id
        {

            List<SportMasterHouseDessignationDMO> Allname = new List<SportMasterHouseDessignationDMO>();

            try
            {
                Allname = _sportcontext.SportMasterHouseDessignationDMO.ToList();
                data.masterhousename = Allname.ToArray();


                data.GridviewDetails = (from a in _sportcontext.SportMasterHouseDessignationDMO
                                        where (a.MI_Id == data.MI_Id)
                                        select new SPCC_Master_House_Designation_DTO
                                        {
                                            SPCCMHD_Id = a.SPCCMHD_Id,
                                            SPCCMHD_DesignationName = a.SPCCMHD_DesignationName,
                                            SPCCMHD_DesignationDescription = a.SPCCMHD_DesignationDescription,
                                            SPCCMHD_ActiveFlag = a.SPCCMHD_ActiveFlag,
                                        }
                                                  ).Distinct().ToArray();

                if (data.GridviewDetails.Length > 0)
                {
                    data.count = data.GridviewDetails.Length;
                }
                else
                {
                    data.count = 0;
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public SPCC_Master_House_Designation_DTO GetSelectedRowDetails(int ID)
        {
            SPCC_Master_House_Designation_DTO SPCC_Master_House_Designation_DTO = new SPCC_Master_House_Designation_DTO();

            try
            {
                List<SportMasterHouseDessignationDMO> lorg = new List<SportMasterHouseDessignationDMO>();
                lorg = _sportcontext.SportMasterHouseDessignationDMO.Where(t => t.SPCCMHD_Id == ID).ToList();
                SPCC_Master_House_Designation_DTO.GridviewDetails = lorg.ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Caste form error");
                _log.LogDebug(ex.Message);
            }

            return SPCC_Master_House_Designation_DTO;
        }

        public SPCC_Master_House_Designation_DTO deactivate(SPCC_Master_House_Designation_DTO dto)
        {
            try
            {
                SPCC_Master_House_Designation_DTO enq = Mapper.Map<SPCC_Master_House_Designation_DTO>(dto);

                if (enq.SPCCMHD_Id > 0)
                {
                    var check_religinassign = (from a in _sportcontext.SportMasterHouseDessignationDMO
                                               where (a.SPCCMHD_Id == dto.SPCCMHD_Id)
                                               select new SPCC_Master_House_Designation_DTO
                                               {
                                                   SPCCMHD_Id = a.SPCCMHD_Id
                                               }
                                            ).ToList();

                    //if (check_religinassign.Count > 0)
                    //{
                    //    dto.returnVal = true;

                    //    dto.msg = "You Can Not Deactivate This Record It Is Already Mapped With Student";
                    //}
                    //else
                    //{
                    var result = _sportcontext.SportMasterHouseDessignationDMO.Single(t => t.SPCCMHD_Id == enq.SPCCMHD_Id);
                    if (result.SPCCMHD_ActiveFlag == true)
                    {
                        result.SPCCMHD_ActiveFlag = false;
                    }
                    else
                    {
                        result.SPCCMHD_ActiveFlag = true;
                    }
                    result.CreatedDate = result.CreatedDate;
                    result.UpdatedDate = DateTime.Now;
                    _sportcontext.Update(result);
                    var flag = _sportcontext.SaveChanges();
                    if (flag == 1)
                    {
                        dto.returnVal = true;

                        if (result.SPCCMHD_ActiveFlag == true)
                        {
                            dto.msg = "House Designation Activated Successfully.";
                        }
                        else if (result.SPCCMHD_ActiveFlag == false)
                        {
                            dto.msg = "House Designation Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        dto.returnVal = false;
                    }
                    List<SportMasterHouseDessignationDMO> Allname = new List<SportMasterHouseDessignationDMO>();
                    Allname = _sportcontext.SportMasterHouseDessignationDMO.ToList();
                    dto.masterhousename = Allname.ToArray();
                }
                //  }
            }
            catch (Exception ee)
            {
                _log.LogTrace(ee.Message);
                _log.LogDebug(ee.Message);
                _log.LogError(ee.Message);
            }
            return dto;
        }

        public SPCC_Master_House_Designation_DTO mastercasteData(SPCC_Master_House_Designation_DTO mas)
        {

            try
            {
                SportMasterHouseDessignationDMO MM = Mapper.Map<SportMasterHouseDessignationDMO>(mas);
                if (mas.SPCCMHD_Id != 0)
                {

                    var duplicate = _sportcontext.SportMasterHouseDessignationDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMHD_DesignationName == mas.SPCCMHD_DesignationName && t.SPCCMHD_Id != mas.SPCCMHD_Id).ToList();



                    if (duplicate.Count > 0)
                    {
                        mas.msg = "Record Already Exist";
                    }
                    else
                    {

                        var result = _sportcontext.SportMasterHouseDessignationDMO.Single(t => t.MI_Id == mas.MI_Id && t.SPCCMHD_Id == mas.SPCCMHD_Id);

                        result.SPCCMHD_DesignationName = mas.SPCCMHD_DesignationName;
                        result.SPCCMHD_DesignationDescription = mas.SPCCMHD_DesignationDescription;
                        result.SPCCMHD_Id = mas.SPCCMHD_Id;
                        result.MI_Id = mas.MI_Id;

                        result.UpdatedDate = DateTime.Now;
                        result.CreatedDate = result.CreatedDate;
                        _sportcontext.Update(result);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.returnVal_update = true;
                        }
                        else
                        {
                            mas.returnVal_update = false;
                        }

                    }

                }
                else
                {

                    var duplicate_caste_name = _sportcontext.SportMasterHouseDessignationDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMHD_DesignationName == mas.SPCCMHD_DesignationName && t.SPCCMHD_Id != mas.SPCCMHD_Id).ToList();

                    var duplicatecountresult = _sportcontext.SportMasterHouseDessignationDMO.Where(t => t.MI_Id == mas.MI_Id && t.SPCCMHD_DesignationName == mas.SPCCMHD_DesignationName && t.SPCCMHD_Id != mas.SPCCMHD_Id).Count();



                    if (duplicate_caste_name.Count > 0)
                    {
                        mas.duplicate_caste_name_bool = true;
                        return mas;
                    }

                    if (duplicatecountresult == 0)
                    {
                        SportMasterHouseDessignationDMO MM3 = new SportMasterHouseDessignationDMO();
                        MM3.SPCCMHD_DesignationName = mas.SPCCMHD_DesignationName;
                        MM3.SPCCMHD_DesignationDescription = mas.SPCCMHD_DesignationDescription;

                        MM3.CreatedDate = DateTime.Now;
                        MM3.UpdatedDate = DateTime.Now;
                        MM3.SPCCMHD_ActiveFlag = true;
                        MM3.SPCCMHD_Id = mas.SPCCMHD_Id;
                        MM3.MI_Id = mas.MI_Id;
                        _sportcontext.Add(MM3);
                        var flag = _sportcontext.SaveChanges();
                        if (flag > 0)
                        {
                            mas.SPCCMHD_Id = MM3.SPCCMHD_Id;

                            mas.returnVal = true;
                        }
                        else
                        {
                            mas.returnVal = false;
                        }

                    }



                    else
                    {
                        mas.msg = "Record Already Exist";
                    }


                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master House Designation form error");
                _log.LogDebug(ex.Message);
            }
            return mas;
        }
    }
}
