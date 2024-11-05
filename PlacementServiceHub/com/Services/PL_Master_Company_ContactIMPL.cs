using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class PL_Master_Company_ContactIMPL : Interfaces.PL_Master_Company_ContactInterface
    {
        public PlacementContext _PlacementContext;
        public PL_Master_Company_ContactIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        public PL_Master_Company_ContactDTO loaddata(PL_Master_Company_ContactDTO data)
        {
            //PL_Master_Company_ContactDTO dto = new PL_Master_Company_ContactDTO();
            try
            {
                data.get_Company = _PlacementContext.PL_Master_CompanyDMO.Distinct().ToArray();
                data.get_contact = (from a in _PlacementContext.PL_Master_CompanyDMO
                                    from b in _PlacementContext.PL_Master_Company_ContactDMO
                                    where (a.PLMCOMP_Id == b.PLMCOMP_Id)
                                    select new PL_Master_Company_ContactDTO
                                    {
                                        PLMCOMP_CompanyName = a.PLMCOMP_CompanyName,
                                        PLMCOMPCON_ContactPersonName = b.PLMCOMPCON_ContactPersonName,
                                        PLMCOMPCON_EmailId = b.PLMCOMPCON_EmailId,
                                        PLMCOMPCON_Designation = b.PLMCOMPCON_Designation,
                                        PLMCOMPCON_ContactNo = b.PLMCOMPCON_ContactNo,
                                        PLMCOMPCON_ActiveFlag = b.PLMCOMPCON_ActiveFlag,
                                        PLMCOMPCON_Id = b.PLMCOMPCON_Id,
                                        PLMCOMP_Id = a.PLMCOMP_Id,

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //dto.returnval = "admin";
            }
            return data;
        }
        public PL_Master_Company_ContactDTO savedata(PL_Master_Company_ContactDTO data)
        {
            try
            {
                if (data.PLMCOMPCON_Id != 0)
                {
                    var res = _PlacementContext.PL_Master_Company_ContactDMO.Where(t => t.PLMCOMPCON_ContactPersonName == data.PLMCOMPCON_ContactPersonName && t.PLMCOMPCON_Designation == data.PLMCOMPCON_Designation && t.PLMCOMPCON_EmailId == data.PLMCOMPCON_EmailId && t.PLMCOMPCON_ContactNo == data.PLMCOMPCON_ContactNo && t.PLMCOMP_Id == data.PLMCOMP_Id && t.PLMCOMPCON_Id != data.PLMCOMPCON_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.PL_Master_Company_ContactDMO.Single(t => t.PLMCOMPCON_Id == data.PLMCOMPCON_Id);
                        //result.MI_Id = data.MI_Id;
                        result.PLMCOMP_Id = data.PLMCOMP_Id;
                        result.PLMCOMPCON_ContactPersonName = data.PLMCOMPCON_ContactPersonName;
                        result.PLMCOMPCON_Designation = data.PLMCOMPCON_Designation;
                        result.PLMCOMPCON_EmailId = data.PLMCOMPCON_EmailId;
                        result.PLMCOMPCON_ContactNo = data.PLMCOMPCON_ContactNo;
                        result.PLMCOMPCON_ActiveFlag = true;
                        result.PLMCOMPCON_UpdatedDate = DateTime.Now;
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
                    var res = _PlacementContext.PL_Master_Company_ContactDMO.Where(t => t.PLMCOMPCON_ContactPersonName == data.PLMCOMPCON_ContactPersonName && t.PLMCOMPCON_Designation == data.PLMCOMPCON_Designation && t.PLMCOMPCON_EmailId == data.PLMCOMPCON_EmailId && t.PLMCOMPCON_ContactNo == data.PLMCOMPCON_ContactNo).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PL_Master_Company_ContactDMO supplier = new PL_Master_Company_ContactDMO();
                        //supplier.MI_Id = data.MI_Id;

                        supplier.PLMCOMP_Id = data.PLMCOMP_Id;
                        supplier.PLMCOMPCON_ContactPersonName = data.PLMCOMPCON_ContactPersonName;
                        supplier.PLMCOMPCON_Designation = data.PLMCOMPCON_Designation;
                        supplier.PLMCOMPCON_EmailId = data.PLMCOMPCON_EmailId;
                        supplier.PLMCOMPCON_ContactNo = data.PLMCOMPCON_ContactNo;
                        supplier.PLMCOMPCON_ActiveFlag = true;
                        supplier.PLMCOMPCON_CreatedDate = DateTime.Now;
                        supplier.PLMCOMPCON_UpdatedDate = DateTime.Now;
                        supplier.PLMCOMPCON_CreatedBy = data.User_Id;
                        supplier.PLMCOMPCON_UpdatedBy = data.User_Id;
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

        public PL_Master_Company_ContactDTO deactive(PL_Master_Company_ContactDTO data)
        {
            try
            {
                var result = _PlacementContext.PL_Master_Company_ContactDMO.Single(t => t.PLMCOMPCON_Id == data.PLMCOMPCON_Id);

                if (result.PLMCOMPCON_ActiveFlag == true)
                {
                    result.PLMCOMPCON_ActiveFlag = false;
                }
                else if (result.PLMCOMPCON_ActiveFlag == false)
                {
                    result.PLMCOMPCON_ActiveFlag = true;
                }
                result.PLMCOMPCON_UpdatedDate = DateTime.Now;
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
