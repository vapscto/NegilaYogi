using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_CI_Schedule_CompanyIMPL : Interfaces.PL_CI_Schedule_CompanyInterface
    {
        public PlacementContext _PlacementContext;
        public PL_CI_Schedule_CompanyIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        public PL_CI_Schedule_CompanyDTO loaddata(PL_CI_Schedule_CompanyDTO data)
        {

            //PL_CI_Schedule_CompanyDTO dto = new PL_CI_Schedule_CompanyDTO();
            try
            {
                data.get_Company = _PlacementContext.PL_Master_CompanyDMO.Distinct().ToArray();
                data.get_details = _PlacementContext.PL_CampusInterview_ScheduleDMO.Distinct().ToArray();




                // data.get_Schdule = _place
                //            select a.PLMCOMP_CompanyName , c.PLCISCHCOMJT_JobTitle, b.PLCISCHCOM_DriveFromDate , b.PLCISCHCOM_DriveToDate , b.PLCISCHCOM_JobDetails
                //from PL_Master_Company a
                //inner join PL_CI_Schedule_Company b on b.PLMCOMP_Id = a.PLMCOMP_Id
                // inner join PL_CI_Schedule_Company_JobTitle c on c.PLCISCHCOM_Id = b.PLCISCHCOM_Id

                data.get_Schdule = (from a in _PlacementContext.PL_CI_Schedule_CompanyDMO
                                    from b in _PlacementContext.PL_Master_CompanyDMO
                                    from c in _PlacementContext.PL_CampusInterview_ScheduleDMO
                                    where (a.PLMCOMP_Id == b.PLMCOMP_Id && a.PLCISCH_Id == c.PLCISCH_Id)
                                    select new PL_CI_Schedule_CompanyDTO
                                    {
                                        PLMCOMP_CompanyName = b.PLMCOMP_CompanyName,
                                        PLCISCH_JobDetails = c.PLCISCH_JobDetails,
                                        PLCISCHCOM_DriveFromDate = a.PLCISCHCOM_DriveFromDate,
                                        PLCISCHCOM_DriveToDate = a.PLCISCHCOM_DriveToDate,
                                        PLCISCHCOM_JobDetails = a.PLCISCHCOM_JobDetails,
                                        PLCISCHCOM_ActiveFlag = a.PLCISCHCOM_ActiveFlag,
                                        PLCISCHCOM_Id = a.PLCISCHCOM_Id,
                                        PLCISCH_Id = c.PLCISCH_Id,
                                        PLMCOMP_Id = b.PLMCOMP_Id,

                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return data;
        }
        public PL_CI_Schedule_CompanyDTO savedata(PL_CI_Schedule_CompanyDTO data)
        {
            try
            {
                if (data.PLCISCHCOM_Id != 0)
                {
                    var res = _PlacementContext.PL_CI_Schedule_CompanyDMO.Where(t => t.PLCISCHCOM_DriveFromDate == data.PLCISCHCOM_DriveFromDate && t.PLCISCHCOM_DriveToDate == data.PLCISCHCOM_DriveToDate && t.PLCISCHCOM_JobDetails == data.PLCISCHCOM_JobDetails && t.PLCISCHCOM_Id == data.PLCISCHCOM_Id && t.PLMCOMP_Id == data.PLMCOMP_Id && t.PLCISCHCOM_Id != data.PLCISCHCOM_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_CI_Schedule_CompanyDMO.Single(t => t.PLCISCHCOM_Id == data.PLCISCHCOM_Id);
                        //result.MI_Id = data.MI_Id;
                        result.PLMCOMP_Id = data.PLMCOMP_Id;
                        result.PLCISCH_Id = data.PLCISCH_Id;
                      
                        result.PLCISCHCOM_DriveFromDate = data.PLCISCHCOM_DriveFromDate;
                        result.PLCISCHCOM_DriveToDate = data.PLCISCHCOM_DriveToDate;
                        result.PLCISCHCOM_JobDetails = data.PLCISCHCOM_JobDetails;
                        result.PLCISCHCOM_ActiveFlag = true;
                        result.PLCISCHCOM_UpdatedDate = DateTime.Now;
                        _PlacementContext.Update(result);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var res = _PlacementContext.PL_CI_Schedule_CompanyDMO.Where(t => t.PLCISCHCOM_DriveFromDate == data.PLCISCHCOM_DriveFromDate && t.PLCISCHCOM_DriveToDate == data.PLCISCHCOM_DriveToDate && t.PLCISCHCOM_JobDetails == data.PLCISCHCOM_JobDetails).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PL_CI_Schedule_CompanyDMO supplier = new PL_CI_Schedule_CompanyDMO();
                        //supplier.MI_Id = data.MI_Id;

                        supplier.PLMCOMP_Id = data.PLMCOMP_Id;
                        supplier.PLCISCH_Id = data.PLCISCH_Id;
                        supplier.PLCISCHCOM_DriveFromDate = data.PLCISCHCOM_DriveFromDate;
                        supplier.PLCISCHCOM_DriveToDate = data.PLCISCHCOM_DriveToDate;
                        supplier.PLCISCHCOM_JobDetails = data.PLCISCHCOM_JobDetails;
                        supplier.PLCISCHCOM_ActiveFlag = true;
                        supplier.PLCISCHCOM_CreatedDate = DateTime.Now;
                        supplier.PLCISCHCOM_UpdatedDate = DateTime.Now;
                        supplier.PLCISCHCOM_CreatedBy = data.User_Id;
                        supplier.PLCISCHCOM_UpdatedBy = data.User_Id;
                        _PlacementContext.Add(supplier);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
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
                data.message = "Error";
                //_logInv.LogInformation("Supplier savedata :" + ex.Message);
            }
            return data;
        }





        public PL_CI_Schedule_CompanyDTO deactive(PL_CI_Schedule_CompanyDTO data)
        {
            try
            {
                var result = _PlacementContext.PL_CI_Schedule_CompanyDMO.Single(t => t.PLCISCHCOM_Id == data.PLCISCHCOM_Id);

                if (result.PLCISCHCOM_ActiveFlag == true)
                {
                    result.PLCISCHCOM_ActiveFlag = false;
                }
                else if (result.PLCISCHCOM_ActiveFlag == false)
                {
                    result.PLCISCHCOM_ActiveFlag = true;
                }
                result.PLCISCHCOM_UpdatedDate = DateTime.Now;
                _PlacementContext.Update(result);
                int returnval = _PlacementContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }




        public PL_CI_Schedule_CompanyDTO editdetails(PL_CI_Schedule_CompanyDTO dto)        {            try            {

                dto.editdata = _PlacementContext.PL_CI_Schedule_CompanyDMO.Where(t => t.PLCISCHCOM_Id == dto.PLCISCHCOM_Id).ToArray();            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }
    }
}
