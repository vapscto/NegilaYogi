using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeGroupYearlyImpl : interfaces.FeeGroupYearlyInterface
    {
        private static ConcurrentDictionary<string, FeeYearlyGroupDTO> _login1 =
  new ConcurrentDictionary<string, FeeYearlyGroupDTO>();

        public FeeGroupContext _FeeGroupContext;
        public FeeGroupYearlyImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public FeeYearlyGroupDTO SaveGroupData(FeeYearlyGroupDTO FGpage)
        {
            bool returnresult = false;
            FeeYearGroupDMO feepge = Mapper.Map<FeeYearGroupDMO>(FGpage);
            // FeeGroupDMO feepge = new FeeGroupDMO();
            string retval = "";
            try
            {
                if (FGpage.FYG_Id > 0)
                {
                    var result = _FeeGroupContext.Yearlygroups.Single(t => t.FYG_Id == feepge.FYG_Id);

                    result.MI_Id = feepge.MI_Id;
                    result.ASMAY_Id = feepge.ASMAY_Id;
                    result.FMG_Id = feepge.FMG_Id;
                    result.FYG_ActiveFlag = feepge.FYG_ActiveFlag;
                    result.user_id = feepge.user_id;

                    _FeeGroupContext.Update(result);
                    var contactExists = _FeeGroupContext.SaveChanges();

                    if (contactExists == 1)
                    {
                        returnresult = true;
                        FGpage.returnval = returnresult;
                    }
                    else
                    {
                        returnresult = false;
                        FGpage.returnval = returnresult;
                    }
                }
                else
                {
                    var result = _FeeGroupContext.Yearlygroups.Where(t => t.FYG_Id == feepge.FYG_Id);

                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        _FeeGroupContext.Add(feepge);
                        var contactExists = _FeeGroupContext.SaveChanges();

                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }

                List<FeeYearGroupDMO> allpages1 = new List<FeeYearGroupDMO>();
                allpages1 = _FeeGroupContext.Yearlygroups.OrderBy(t => t.FYG_Id).ToList();
                FGpage.groupYearData = allpages1.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
    }
}