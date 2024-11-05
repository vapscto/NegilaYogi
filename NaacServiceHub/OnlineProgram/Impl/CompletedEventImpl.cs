using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.OnlineExam;
using DomainModel.Model.com.vapstech.OnlineProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Impl
{
    public class CompletedEventImpl : Interfaces.CompletedEventInterface
    {

        private static ConcurrentDictionary<string, OnlineProgramDTO> _login =
         new ConcurrentDictionary<string, OnlineProgramDTO>();

        ILogger<CompletedEventImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public CompletedEventImpl(DomainModelMsSqlServerContext dbcontext, ILogger<CompletedEventImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }


        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            try
            {

                data.programlist = _dbContext.ProgramsYearlyDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

               
                data.fillyear = (from a in _dbContext.ProgramsYearlyDMO
                                 from b in _dbContext.ProgramsYearlyActivitiesDMO
                                 where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id)
                                                  select new OnlineProgramDTO
                                                  {
                                                      PRYRA_Id=b.PRYRA_Id,
                                                      programname = a.PRYR_ProgramName,
                                                      Eventname = b.PRYRA_ActivityName,
                                                      start_time = b.PRYRA_StartTime,
                                                      end_time = b.PRYRA_Duration,
                                                      description = b.PRYRA_Description,
                                                      PRYRA_ActiveFlag = b.PRYRA_ActiveFlag
                                                  }
                         ).Distinct().OrderByDescending(t => t.PRYRA_Id).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public OnlineProgramDTO Savedata(OnlineProgramDTO data)
        {

            var res = _dbContext.ProgramsYearlyActivitiesDMO.Where(t => t.PRYRA_Id==data.PRYRA_Id).ToList();
            if (res.Count > 0)
            {

                var res1 = _dbContext.ProgramsYearlyActivitiesDMO.Where(t => t.PRYRA_ActivityName.Equals(data.Eventname) &&  t.PRYR_Id == data.PRYR_Id).ToList();

                if (res1.Count > 0)
                {
                    data.returndt = "Duplicate";
                }

                else
                {
                    var objpge1 = _dbContext.ProgramsYearlyActivitiesDMO.Single(t => t.PRYRA_Id == data.PRYRA_Id);
                    objpge1.PRYR_Id = data.PRYR_Id;
                    objpge1.PRYRA_ActivityName = data.Eventname;
                    objpge1.PRYRA_StartTime = data.start_time;
                    objpge1.PRYRA_Duration = data.end_time;
                    objpge1.PRYRA_Description = data.description;
                    objpge1.PRYRA_ActiveFlag = true;
                    objpge1.UpdatedDate = DateTime.Now;
                    objpge1.PRYRA_UpdatedBy = data.UserId;
                    _dbContext.Update(objpge1);
                    var contactExists = _dbContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnresult = true;
                        data.message = "Update";
                    }
                    else
                    {
                        data.returnresult = false;
                        data.message = "Not Update";
                    }
                }

            }

            else
            {
                try
                {
                    ProgramsYearlyActivitiesDMO objpge1 = new ProgramsYearlyActivitiesDMO();
                    objpge1.PRYR_Id = data.PRYR_Id;
                    objpge1.PRYRA_ActivityName = data.Eventname;
                    objpge1.PRYRA_StartTime = data.start_time;
                    objpge1.PRYRA_Duration = data.end_time;
                    objpge1.PRYRA_Description = data.description;
                    objpge1.PRYRA_ActiveFlag = true;
                    objpge1.CreatedDate = DateTime.Now;
                    objpge1.UpdatedDate = DateTime.Now;
                    objpge1.PRYRA_CreatedBy = data.UserId;
                    objpge1.PRYRA_UpdatedBy = data.UserId;
                    _dbContext.Add(objpge1);

                    var contactExists = _dbContext.SaveChanges();
                    if (contactExists == 1 && contactExists == 1)
                    {
                        data.returnresult = true;
                        data.message = "Saved";
                    }
                    else
                    {
                        data.returnresult = false;
                        data.message = "Not Saved";
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return data;
        }


        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            try
            {

                data.programlist = _dbContext.ProgramsYearlyActivitiesDMO.Where(a => a.PRYRA_Id == data.PRYRA_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



        public OnlineProgramDTO deactivate(OnlineProgramDTO acd)
        {
            try
            {


                var result = _dbContext.ProgramsYearlyActivitiesDMO.Single(t => t.PRYRA_Id == acd.PRYRA_Id);

                if (result.PRYRA_ActiveFlag == true)
                {
                    result.PRYRA_ActiveFlag = false;
                }
                else
                {
                    result.PRYRA_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _dbContext.Update(result);
                var flag = _dbContext.SaveChanges();
                if (flag == 1)
                {
                    acd.returnresult = true;
                }
                else
                {
                    acd.returnresult = false;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
    }
}
