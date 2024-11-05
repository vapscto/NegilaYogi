using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class AdmissionStatusImpl : Interfaces.AdmissionStatusInterface
    {
        private static ConcurrentDictionary<string, AdmissionStatusDTO> _login =
            new ConcurrentDictionary<string, AdmissionStatusDTO>();

        public AdmissionStatusContext _AdmissionStatusContext;

        public AdmissionStatusImpl(AdmissionStatusContext AdmissionStatusContext)
        {
            _AdmissionStatusContext = AdmissionStatusContext;
        }

        public AdmissionStatusDTO deletedata(AdmissionStatusDTO org)
        {
            try
            {
                var lorg = _AdmissionStatusContext.AdmissionStatus.Single(t => t.PAMST_Id.Equals(org.PAMST_Id) && t.MI_Id==org.MI_Id);
                //added by 02/02/2017

                lorg.UpdatedDate = DateTime.Now;
                lorg.active = 1;
                    _AdmissionStatusContext.Update(lorg);
                    var flag = _AdmissionStatusContext.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnval = true;
                    }
                    else
                    {
                        org.returnval = false;
                    }

                List<AdmissionStatus> allacademic = new List<AdmissionStatus>();
                allacademic = _AdmissionStatusContext.AdmissionStatus.Where(t=>t.active==0).ToList();
                org.academicstatuslist = allacademic.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }

        public AdmissionStatusDTO editdetail(int id)
        {
            AdmissionStatusDTO dta = new AdmissionStatusDTO();
            try
            {
                List<AdmissionStatus> allacademic = new List<AdmissionStatus>();
                allacademic = _AdmissionStatusContext.AdmissionStatus.Where(t=>t.PAMST_Id==id).ToList();
                dta.academicstatuslist = allacademic.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dta;
        }

        public AdmissionStatusDTO getallDetails(AdmissionStatusDTO acdmc)
        {
            try
            {
                List<AdmissionStatus> allacademic = new List<AdmissionStatus>();
                allacademic = _AdmissionStatusContext.AdmissionStatus.Where(t=>t.active==0).ToList();
                acdmc.academicstatuslist = allacademic.ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }

        public AdmissionStatusDTO savedataa(AdmissionStatusDTO data)
        {
            try
            {
                AdmissionStatus MM = Mapper.Map<AdmissionStatus>(data);

                if (data.PAMST_Id != 0)
                {
                    var result = _AdmissionStatusContext.AdmissionStatus.Single(t => t.PAMST_Id == data.PAMST_Id);

                 
                    result.PAMST_Status = data.PAMST_Status;
                    result.PAMST_StatusFlag = data.PAMST_StatusFlag;
                    //added by 02/02/2017
          
                    result.UpdatedDate = DateTime.Now;
                    _AdmissionStatusContext.Update(result);
                    var contactExists = _AdmissionStatusContext.SaveChanges();

                    if (contactExists == 1)
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
                    var result = _AdmissionStatusContext.AdmissionStatus.Where(t => t.PAMST_Status == data.PAMST_Status);
                    if (result.Count() >= 1)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {  //added by 02/02/2017
                        MM.CreatedDate = DateTime.Now;
                        MM.UpdatedDate = DateTime.Now;
                        _AdmissionStatusContext.Add(MM);
                        var contactExists = _AdmissionStatusContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

                List<AdmissionStatus> allacademic = new List<AdmissionStatus>();
                allacademic = _AdmissionStatusContext.AdmissionStatus.Where(t=>t.active==0).ToList();
                data.academicstatuslist = allacademic.ToArray();

                return data;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
