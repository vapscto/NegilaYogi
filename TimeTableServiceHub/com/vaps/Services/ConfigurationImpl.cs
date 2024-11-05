using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class ConfigurationImpl:Interfaces.ConfigurationInterface
    {    

        private readonly TTContext _contextobj;

        public ConfigurationImpl(TTContext config)
        {
            _contextobj = config;
        }
        public TTConfigurationDTO savedetail(TTConfigurationDTO _config)
        {
            TT_ConfigurationDMO objpge = Mapper.Map<TT_ConfigurationDMO>(_config);
            try
            {
                if (objpge.TTC_Id > 0)
                {
                    var result = _contextobj.TT_ConfigurationDMO.Single(t => t.TTC_Id == objpge.TTC_Id);
                    result.MI_Id = objpge.MI_Id;
                    result.TTC_StaffwiseContiniousPeriods = objpge.TTC_StaffwiseContiniousPeriods;
                    result.TTC_CTConstraintFlg = objpge.TTC_CTConstraintFlg;
                    result.TTC_CTConstraintNoOfDays = objpge.TTC_CTConstraintNoOfDays;
                    result.TTC_MaxMinPeriodCheckingFlg = objpge.TTC_MaxMinPeriodCheckingFlg;
                    result.UpdatedDate = DateTime.Now;

                    _contextobj.Update(result);
                    var contactExists = _contextobj.SaveChanges();

                    if (contactExists == 1)
                    {
                        _config.returnduplicatestatus = "Update";
                    }
                    else
                    {
                        _config.returnduplicatestatus = "NotUpdate";
                    }
                }
                else
                {
                    var res = _contextobj.TT_ConfigurationDMO.Where(t => t.MI_Id == objpge.MI_Id && t.TTC_Id == objpge.TTC_Id).ToList();
                    if (res.Count() > 0)
                    {
                        _config.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _contextobj.Add(objpge);
                        var contactExists = _contextobj.SaveChanges();
                        if (contactExists == 1)
                        {
                            _config.returnduplicatestatus = "Save";
                        }
                        else
                        {
                            _config.returnduplicatestatus = "NotSave";
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _config;
        }


        public TTConfigurationDTO getdetails(TTConfigurationDTO TTConfig)
        {
            try
            {
                TTConfig.Detailslist = _contextobj.TT_ConfigurationDMO.Where(t => t.MI_Id == TTConfig.MI_Id).ToList().ToArray();
            }
            catch (Exception ee)
            {
                 Console.WriteLine(ee.Message);
            }
            return TTConfig;

        }

    }
}
