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
    public class ProgramMasterImpl : Interfaces.ProgramMasterInterface
    {
        private static ConcurrentDictionary<string, OnlineProgramDTO> _login =
           new ConcurrentDictionary<string, OnlineProgramDTO>();

        ILogger<YearlyProgramImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public ProgramMasterImpl(DomainModelMsSqlServerContext dbcontext, ILogger<YearlyProgramImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }


        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            try
            {

                data.Typelist = _dbContext.ProgramsMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.levellist = _dbContext.ProgramsMasterLevelDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

              












            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public OnlineProgramDTO savedatatype(OnlineProgramDTO data)
        {

            var res = _dbContext.ProgramsMasterTypeDMO.Where(t => t.MI_Id == data.MI_Id && t.PRMTY_Id == data.PRMTY_Id).ToList();
            if (res.Count > 0)
            {
                var objpge1 = _dbContext.ProgramsMasterTypeDMO.Single(t => t.PRMTY_Id == data.PRMTY_Id);
                objpge1.MI_Id = data.MI_Id;
                objpge1.PRMTY_ProgramType = data.programname;
                objpge1.PRMTY_ProgramTypeDes = data.PRMTLE_IdDesc;
                objpge1.PRMTY_ActiveFlg = true;
                objpge1.PRMTY_CreatedDate = DateTime.Now;
                objpge1.PRMTY_UpdatedDate = DateTime.Now;
                objpge1.PRMTY_CreatedBy = data.UserId;
                objpge1.PRMTY_UpdatedBy = data.UserId;
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

            else
            {
                try
                {

                    var res1 = _dbContext.ProgramsMasterTypeDMO.Where(t => t.PRMTY_ProgramType.Equals(data.programname) && t.MI_Id == data.MI_Id).ToList();

                    if (res1.Count > 0)
                    {
                        data.returnresult = false;
                        data.message = "Duplicate";
                    }

                    else
                    {

                        ProgramsMasterTypeDMO objpge1 = new ProgramsMasterTypeDMO();

                        objpge1.MI_Id = data.MI_Id;
                        objpge1.PRMTY_ProgramType = data.programname;
                        objpge1.PRMTY_ProgramTypeDes = data.PRMTLE_IdDesc;
                        objpge1.PRMTY_ActiveFlg = true;
                        objpge1.PRMTY_CreatedDate = DateTime.Now;
                        objpge1.PRMTY_UpdatedDate = DateTime.Now;
                        objpge1.PRMTY_CreatedBy = data.UserId;
                        objpge1.PRMTY_UpdatedBy = data.UserId;
                        

                        _dbContext.ProgramsMasterTypeDMO.Add(objpge1);

                        
                    }
                    
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


        public OnlineProgramDTO savedatalevel(OnlineProgramDTO data)
        {

            var res = _dbContext.ProgramsMasterLevelDMO.Where(t => t.MI_Id == data.MI_Id && t.PRMTLE_Id == data.PRMTLE_Id).ToList();
            if (res.Count > 0)
            {
                var objpge1 = _dbContext.ProgramsMasterLevelDMO.Single(t => t.PRMTLE_Id == data.PRMTLE_Id);
                objpge1.MI_Id = data.MI_Id;
                objpge1.PRMTLE_ProgramLevel = data.programname;
                objpge1.PRMTLE_ProgramLevelDes = data.PRMTLE_IdDesc;
                objpge1.PRMTLE_ActiveFlg = true;
                objpge1.PRMTLE_CreatedDate = DateTime.Now;
                objpge1.PRMTLE_UpdatedDate = DateTime.Now;
                objpge1.PRMTLE_CreatedBy = data.UserId;
                objpge1.PRMTLE_UpdatedBy = data.UserId;
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

            else
            {
                try
                {

                    var res1 = _dbContext.ProgramsMasterLevelDMO.Where(t => t.PRMTLE_ProgramLevel.Equals(data.programname) && t.MI_Id == data.MI_Id).ToList();

                    if (res1.Count > 0)
                    {
                        data.returnresult = false;
                        data.message = "Duplicate";
                    }

                    else
                    {

                        ProgramsMasterLevelDMO objpge1 = new ProgramsMasterLevelDMO();

                        objpge1.MI_Id = data.MI_Id;
                        objpge1.PRMTLE_ProgramLevel = data.programname;
                        objpge1.PRMTLE_ProgramLevelDes = data.PRMTLE_IdDesc;
                        objpge1.PRMTLE_ActiveFlg = true;
                        objpge1.PRMTLE_CreatedDate = DateTime.Now;
                        objpge1.PRMTLE_UpdatedDate = DateTime.Now;
                        objpge1.PRMTLE_CreatedBy = data.UserId;
                        objpge1.PRMTLE_UpdatedBy = data.UserId;
                    

                        _dbContext.ProgramsMasterLevelDMO.Add(objpge1);


                    }

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

                data.programlist = _dbContext.ProgramsYearlyDMO.Where(a => a.MI_Id == data.MI_Id && a.PRYR_Id==data.PRYR_Id).ToArray();
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    

        public OnlineProgramDTO deactivelevel(OnlineProgramDTO acd)
        {
            try
            {
                
                    var result = _dbContext.ProgramsMasterLevelDMO.Single(t => t.PRMTLE_Id == acd.PRMTLE_Id);
                   
                        if (result.PRMTLE_ActiveFlg == true)
                        {
                            result.PRMTLE_ActiveFlg = false;
                        }
                        else
                        {
                            result.PRMTLE_ActiveFlg = true;
                        }
                        result.PRMTLE_UpdatedDate = DateTime.Now;
                        result.PRMTLE_UpdatedBy = acd.UserId;
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


        public OnlineProgramDTO deactivetype(OnlineProgramDTO acd)
        {
            try
            {


                var result = _dbContext.ProgramsMasterTypeDMO.Single(t => t.PRMTY_Id == acd.PRMTY_Id);

                if (result.PRMTY_ActiveFlg == true)
                {
                    result.PRMTY_ActiveFlg = false;
                }
                else
                {
                    result.PRMTY_ActiveFlg = true;
                }
                result.PRMTY_UpdatedDate = DateTime.Now;
                result.PRMTY_UpdatedBy = acd.UserId;
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

        public OnlineProgramDTO editlevel(OnlineProgramDTO data)
        {
            try
            {
               
                data.levellist = _dbContext.ProgramsMasterLevelDMO.Where(a => a.MI_Id == data.MI_Id && a.PRMTLE_Id==data.PRMTLE_Id).ToArray();
                
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }



        public OnlineProgramDTO edittype(OnlineProgramDTO data)
        {
            try
            {


                data.Typelist = _dbContext.ProgramsMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.PRMTY_Id==data.PRMTY_Id).ToArray();

              
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }




    }
}
