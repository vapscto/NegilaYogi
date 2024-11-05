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
    public class Master_External_TrainingTypeImpl : Interfaces.Master_External_TrainingTypeInterface
    {
        public VMSContext _context;
        public Master_External_TrainingTypeImpl(VMSContext _con)
        {
            _context = _con;
        }
        public Master_External_TrainingTypeDTO onloaddata(Master_External_TrainingTypeDTO data)
        {
            try
            {
                data.getloaddetails = _context.Master_External_TrainingTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Master_External_TrainingTypeDTO saverecord(Master_External_TrainingTypeDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.HRMETRTY_Id > 0)
                {
                    var checkduplicate = _context.Master_External_TrainingTypeDMO.Where(a => a.MI_Id == data.MI_Id
                     && a.HRMETRTY_ExternalTrainingType.Equals(data.HRMETRTY_ExternalTrainingType) && a.HRMETRTY_Id != data.HRMETRTY_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkresult = _context.Master_External_TrainingTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRTY_Id == data.HRMETRTY_Id);
                        checkresult.HRMETRTY_ExternalTrainingType = data.HRMETRTY_ExternalTrainingType;
                        checkresult.HRMETRTY_MinimumTrainingHrs = data.HRMETRTY_MinimumTrainingHrs;
                        checkresult.HRMETRTY_Id = data.HRMETRTY_Id;
                        checkresult.HRMETRTY_UpdatedBy = data.Userid;
                        checkresult.HRMETRTY_UpdatedDate = indiantime0;
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
                    Master_External_TrainingTypeDMO obj = new Master_External_TrainingTypeDMO();

                    var checkduplicate = _context.Master_External_TrainingTypeDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.HRMETRTY_ExternalTrainingType == data.HRMETRTY_ExternalTrainingType).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        obj.MI_Id = data.MI_Id;
                        obj.HRMETRTY_ExternalTrainingType = data.HRMETRTY_ExternalTrainingType;
                        obj.HRMETRTY_MinimumTrainingHrs = data.HRMETRTY_MinimumTrainingHrs;
                        obj.HRMETRTY_Id = data.HRMETRTY_Id;
                        obj.HRMETRTY_ActiveFlag = true;
                        obj.HRMETRTY_CreatedBy = data.Userid;
                        obj.HRMETRTY_UpdatedBy = data.Userid;
                        obj.HRMETRTY_CreatedDate = indiantime0;
                        obj.HRMETRTY_UpdatedDate = indiantime0;
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

        public Master_External_TrainingTypeDTO deactiveY(Master_External_TrainingTypeDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkactivestatus = _context.Master_External_TrainingTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRTY_Id == data.HRMETRTY_Id);

                if (checkactivestatus.HRMETRTY_ActiveFlag == true)
                {
                    var resultdeactive = _context.Master_External_TrainingTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRTY_Id == data.HRMETRTY_Id);

                    if (resultdeactive.HRMETRTY_ActiveFlag == true)
                    {
                        resultdeactive.HRMETRTY_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.HRMETRTY_ActiveFlag = true;
                    }

                    resultdeactive.HRMETRTY_UpdatedDate = indiantime0;
                    resultdeactive.HRMETRTY_UpdatedBy = data.Userid;
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
                    var resultdeactive = _context.Master_External_TrainingTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.HRMETRTY_Id == data.HRMETRTY_Id);

                    if (resultdeactive.HRMETRTY_ActiveFlag == true)
                    {
                        resultdeactive.HRMETRTY_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.HRMETRTY_ActiveFlag = true;
                    }

                    resultdeactive.HRMETRTY_UpdatedDate = indiantime0;
                    resultdeactive.HRMETRTY_UpdatedBy = data.Userid;
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
