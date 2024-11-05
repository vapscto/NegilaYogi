using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_Master_CompanyIMPL : Interfaces.PL_Master_CompanyInterface
    {
        public PlacementContext _PlacementContext;
        public PL_Master_CompanyIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        public PL_Master_CompanyDTO loaddata(PL_Master_CompanyDTO data)
        {

            //PL_Master_CompanyDTO dto = new PL_Master_CompanyDTO();
            try
            {
                data.get_Placement = _PlacementContext.PL_Master_CompanyDMO.Where(R => R.MI_Id == data.MI_Id).ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //dto.returnval = "admin";
            }
            return data;

        }
        public PL_Master_CompanyDTO savedata(PL_Master_CompanyDTO data)
        {
            try
            {
                if (data.PLMCOMP_Id != 0)
                {
                    var res = _PlacementContext.PL_Master_CompanyDMO.Where(t => t.PLMCOMP_CompanyName == data.PLMCOMP_CompanyName && t.PLMCOMP_CompanyAddress == data.PLMCOMP_CompanyAddress && t.PLMCOMP_Website == data.PLMCOMP_Website && t.PLMCOMP_FacilityFilePath == data.PLMCOMP_FacilityFilePath && t.MI_Id == data.MI_Id && t.PLMCOMP_Id != data.PLMCOMP_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_Master_CompanyDMO.Single(t => t.PLMCOMP_Id == data.PLMCOMP_Id);
                        result.MI_Id = data.MI_Id;
                        result.PLMCOMP_CompanyName = data.PLMCOMP_CompanyName;
                        result.PLMCOMP_CompanyAddress = data.PLMCOMP_CompanyAddress;
                        result.PLMCOMP_Website = data.PLMCOMP_Website;
                        result.PLMCOMP_FacilityFilePath = data.PLMCOMP_FacilityFilePath;
                        result.PLMCOMP_ActiveFlag = true;
                        result.PLMCOMP_UpdatedDate = DateTime.Now;
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
                    var res = _PlacementContext.PL_Master_CompanyDMO.Where(t => t.PLMCOMP_CompanyName == data.PLMCOMP_CompanyName && t.PLMCOMP_CompanyAddress == data.PLMCOMP_CompanyAddress && t.PLMCOMP_Website == data.PLMCOMP_Website && t.PLMCOMP_FacilityFilePath == data.PLMCOMP_FacilityFilePath && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PL_Master_CompanyDMO supplier = new PL_Master_CompanyDMO();
                        supplier.MI_Id = data.MI_Id;
                        supplier.PLMCOMP_CompanyName = data.PLMCOMP_CompanyName;
                        supplier.PLMCOMP_CompanyAddress = data.PLMCOMP_CompanyAddress;
                        supplier.PLMCOMP_Website = data.PLMCOMP_Website;
                        supplier.PLMCOMP_FacilityFilePath = data.PLMCOMP_FacilityFilePath;                    
                        supplier.PLMCOMP_ActiveFlag = true;
                        supplier.PLMCOMP_CreatedDate = DateTime.Now;
                        supplier.PLMCOMP_UpdatedDate = DateTime.Now;
                        supplier.PLMCOMP_CreatedBy = data.User_Id; 
                        supplier.PLMCOMP_UpdatedBy = data.User_Id;
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

        public PL_Master_CompanyDTO deactive(PL_Master_CompanyDTO data)
        {
            try
            {
                var result = _PlacementContext.PL_Master_CompanyDMO.Single(t => t.PLMCOMP_Id == data.PLMCOMP_Id);

                if (result.PLMCOMP_ActiveFlag == true)
                {
                    result.PLMCOMP_ActiveFlag = false;
                }
                else if (result.PLMCOMP_ActiveFlag == false)
                {
                    result.PLMCOMP_ActiveFlag = true;
                }
                result.PLMCOMP_UpdatedDate = DateTime.Now;
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


    }
}
