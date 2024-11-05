using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model.com.vapstech.VMS.Training;
using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class Master_External_TrainingCentersIMPL : Interfaces.Master_External_TrainingCentersInterface
    {
        public VMSContext _context;
        public Master_External_TrainingCentersIMPL(VMSContext _con)
        {
            _context = _con;
        }

        public Master_External_TrainingCentersDTO onloaddata(Master_External_TrainingCentersDTO data)
        {
            try
            {
                data.getloaddetails = _context.Master_External_TrainingCentersDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Master_External_TrainingCentersDTO saverecord(Master_External_TrainingCentersDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.HRMETRCEN_Id > 0)
                {
                    var checkduplicate = _context.Master_External_TrainingCentersDMO.Where(a => a.MI_Id == data.MI_Id
                     && a.HRMETRCEN_TrainingCenterName.Equals(data.HRMETRCEN_TrainingCenterName) && a.HRMETRCEN_Id != data.HRMETRCEN_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkresult = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRCEN_Id == data.HRMETRCEN_Id);
                        checkresult.HRMETRCEN_TrainingCenterName = data.HRMETRCEN_TrainingCenterName;
                        checkresult.HRMETRCEN_ContactPersonName = data.HRMETRCEN_ContactPersonName;
                        checkresult.HRMETRCEN_ContactNo = data.HRMETRCEN_ContactNo;
                        checkresult.HRMETRCEN_ContactEmailId = data.HRMETRCEN_ContactEmailId;
                        checkresult.HRMETRCEN_CenterAddress = data.HRMETRCEN_CenterAddress;
                        checkresult.HRMETRCEN_UpdatedBy = data.Userid;
                        checkresult.HRMETRCEN_UpdatedDate = indiantime0;
                        _context.Update(checkresult);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    Master_External_TrainingCentersDMO obj = new Master_External_TrainingCentersDMO();

                    var checkduplicate = _context.Master_External_TrainingCentersDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.HRMETRCEN_TrainingCenterName == data.HRMETRCEN_TrainingCenterName).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        obj.MI_Id = data.MI_Id;
                        obj.HRMETRCEN_TrainingCenterName = data.HRMETRCEN_TrainingCenterName;
                        obj.HRMETRCEN_ContactPersonName = data.HRMETRCEN_ContactPersonName;
                        obj.HRMETRCEN_ContactNo = data.HRMETRCEN_ContactNo;
                        obj.HRMETRCEN_ContactEmailId = data.HRMETRCEN_ContactEmailId;
                        obj.HRMETRCEN_CenterAddress = data.HRMETRCEN_CenterAddress;
                        obj.HRMETRCEN_ActiveFlag = true;
                        obj.HRMETRCEN_CreatedBy = data.Userid;
                        obj.HRMETRCEN_UpdatedBy = data.Userid;
                        obj.HRMETRCEN_CreatedDate = indiantime0;
                        obj.HRMETRCEN_UpdatedDate = indiantime0;
                        _context.Add(obj);
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public Master_External_TrainingCentersDTO deactiveY(Master_External_TrainingCentersDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkactivestatus = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRCEN_Id == data.HRMETRCEN_Id);

                if (checkactivestatus.HRMETRCEN_ActiveFlag == true)
                {
                    var resultdeactive = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRCEN_Id == data.HRMETRCEN_Id);

                    if (resultdeactive.HRMETRCEN_ActiveFlag == true)
                    {
                        resultdeactive.HRMETRCEN_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.HRMETRCEN_ActiveFlag = true;
                    }

                    resultdeactive.HRMETRCEN_UpdatedDate = indiantime0;
                    resultdeactive.HRMETRCEN_UpdatedBy = data.Userid;
                    _context.Update(resultdeactive);

                    var i = _context.SaveChanges();
                    if (i > 0)
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
                    var resultdeactive = _context.Master_External_TrainingCentersDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRCEN_Id == data.HRMETRCEN_Id);

                    if (resultdeactive.HRMETRCEN_ActiveFlag == true)
                    {
                        resultdeactive.HRMETRCEN_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.HRMETRCEN_ActiveFlag = true;
                    }

                    resultdeactive.HRMETRCEN_UpdatedDate = indiantime0;
                    resultdeactive.HRMETRCEN_UpdatedBy = data.Userid;
                    _context.Update(resultdeactive);

                    var i = _context.SaveChanges();
                    if (i > 0)
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




    }
}
