using PreadmissionDTOs.com.vaps.OnlineProgram;
using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.OnlineExam;
using DomainModel.Model.com.vapstech.OnlineProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Impl
{
    public class GuestDetailsImpl : Interfaces.GuestDetailsInterface
    {

        private static ConcurrentDictionary<string, OnlineProgramDTO> _login =
        new ConcurrentDictionary<string, OnlineProgramDTO>();

        ILogger<GuestDetailsImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public GuestDetailsImpl(DomainModelMsSqlServerContext dbcontext, ILogger<GuestDetailsImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }


        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            try
            {
                data.programlist = _dbContext.ProgramsYearlyDMO.Where(a => a.MI_Id == data.MI_Id && a.PRYR_ActiveFlag==true).ToArray();



                data.alllist = (from a in _dbContext.ProgramsYearlyDMO
                                 from b in _dbContext.ProgramsYearlyGuestDMO
                                 where (a.PRYR_Id == b.PRYR_Id && a.MI_Id == data.MI_Id)
                                 select new OnlineProgramDTO
                                 {
                                     PRYR_Id=b.PRYRG_Id,
                                     programname=a.PRYR_ProgramName,
                                     PRYRG_GuestType=b.PRYRG_GuestType,
                                     PRYRG_GuestName=b.PRYRG_GuestName,
                                     PRYRG_GuestPhoneNo=b.PRYRG_GuestPhoneNo,
                                     PRYRG_GuestEmailId=b.PRYRG_GuestEmailId,
                                    // PRYRG_GuestProfile=b.PRYRG_GuestProfile,
                                   //  PRYRG_GuestPhoto=b.PRYRG_GuestPhoto,
                                     //PRYRG_GuestVideo=b.PRYRG_GuestVideo
                                 }).Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



        public OnlineProgramDTO savedetail(OnlineProgramDTO data)
        {

            try
            {
                if(data.PDFfile==null || data.PDFfile == " ")
                {
                    data.PDFfile = " ";
                }
                if (data.Imagefile == null || data.Imagefile == " ")
                {
                    data.Imagefile = " ";
                }
                if (data.Videofile == null || data.Videofile == " ")
                {
                    data.Videofile = " ";
                }

                var resultCount1 = _dbContext.ProgramsYearlyGuestDMO.Where(a=>a.PRYRG_Id == data.PRYRG_Id).ToList().Count();
              
                if (resultCount1 > 0)
                {
                
                        var result1 = _dbContext.ProgramsYearlyGuestDMO.Single(t => t.PRYRG_Id == data.PRYRG_Id);

                                result1.PRYR_Id = data.PRYR_Id;
                                result1.PRYRG_GuestType = data.PRYRG_GuestType;
                                result1.PRYRG_GuestPhoneNo = data.PRYRG_GuestPhoneNo;
                                result1.PRYRG_GuestName = data.PRYRG_GuestName;
                                result1.PRYRG_GuestEmailId = data.PRYRG_GuestEmailId;
                               // result1.PRYRG_GuestSpeech = data.PRYRG_GuestSpeech;
                               // result1.PRYRG_GuestProfile = data.PDFfile;
                               // result1.PRYRG_GuestPhoto = data.Imagefile;
                               // result1.PRYRG_GuestVideo = data.Videofile;
                                result1.UpdatedDate = DateTime.Now;
                                result1.PRYRG_UpdatedBy = data.UserId;
                                _dbContext.Add(result1);
                    
                        _dbContext.Update(result1);
                      
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            data.returnresult = true;
                        }
                        else
                        {
                            data.returnresult = false;
                        }
                    
                }
                else
                {
                    
                            ProgramsYearlyGuestDMO objpge1 = new ProgramsYearlyGuestDMO();
                            objpge1.PRYR_Id = data.PRYR_Id;
                            objpge1.PRYRG_GuestType = data.PRYRG_GuestType;
                            objpge1.PRYRG_GuestName = data.PRYRG_GuestName;
                            objpge1.PRYRG_GuestPhoneNo = data.PRYRG_GuestPhoneNo;
                            objpge1.PRYRG_GuestEmailId = data.PRYRG_GuestEmailId;
                        //    objpge1.PRYRG_GuestProfile = data.PDFfile;
                         //   objpge1.PRYRG_GuestPhoto = data.Imagefile;
                         //   objpge1.PRYRG_GuestVideo = data.Videofile;
                         //   objpge1.PRYRG_GuestSpeech = data.PRYRG_GuestSpeech;
                            objpge1.PRYRG_ActiveFlag = true;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            objpge1.PRYRG_CreatedBy = data.UserId;
                            objpge1.PRYRG_UpdatedBy = data.UserId;
                    _dbContext.Add(objpge1);

                    
                      var contactExists = _dbContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnresult = true;
                    }
                    else
                    {
                        data.returnresult = false;
                    }
                    // }
                }



            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }



        public OnlineProgramDTO getalldetailsviewrecords(OnlineProgramDTO data)
        {

            try
            {
              
                data.Photolist = _dbContext.ProgramsYearlyGuestDMO.Where(e => e.PRYRG_Id == data.PRYRG_Id).ToArray();
              
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public OnlineProgramDTO getdetails(OnlineProgramDTO data)
        {
            try
            {
                data.alllist = _dbContext.ProgramsYearlyGuestDMO.Where(a => a.PRYRG_Id==data.PRYRG_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public OnlineProgramDTO deleterecord(OnlineProgramDTO data)
        {
            bool returnresult = false;
            try
            {
                var delete = _dbContext.ProgramsYearlyGuestDMO.Where(t => t.PRYRG_Id == data.PRYRG_Id).ToList();
                try
                {
                    if (delete.Any())
                    {
                        _dbContext.Remove(delete.ElementAt(0));

                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            data.returnresult = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            data.returnresult = returnresult;
                        }
                    }
                }
                catch (Exception ee)
                {

                    Console.WriteLine(ee.Message);
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
