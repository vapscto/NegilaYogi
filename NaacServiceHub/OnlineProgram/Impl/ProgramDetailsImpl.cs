using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.OnlineExam;
using DomainModel.Model.com.vapstech.OnlineProgram;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
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
    public class ProgramDetailsImpl : Interfaces.ProgramDetailsInterface
    {

        private static ConcurrentDictionary<string, OnlineProgramDTO> _login =
          new ConcurrentDictionary<string, OnlineProgramDTO>();

        ILogger<ProgramDetailsImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public ProgramDetailsImpl(DomainModelMsSqlServerContext dbcontext, ILogger<ProgramDetailsImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }


        public OnlineProgramDTO getloaddata(OnlineProgramDTO data)
        {
            try
            {
                data.programlist = _dbContext.ProgramsYearlyDMO.Where(a => a.MI_Id == data.MI_Id && a.PRYR_ActiveFlag==true).ToArray();


                List<OnlineProgramDTO> alldata = new List<OnlineProgramDTO>();
                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Program_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });
                   
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 =  cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.alllist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                
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

                if (data.PDFfile == null || data.PDFfile == " ")
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

                var resultCount1 = _dbContext.ProgramsYearlyPhotosDMO.Where(t=>t.PRYR_Id==data.PRYR_Id).ToList().Count();
                var resultCount2 = _dbContext.ProgramsYearlyVideosDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList().Count();
                if (resultCount1>0 && resultCount2>0)
                {
                        var result1 = _dbContext.ProgramsYearlyPhotosDMO.Single(t => t.PRYR_Id == data.PRYR_Id);
                        var result2 = _dbContext.ProgramsYearlyVideosDMO.Single(t => t.PRYR_Id == data.PRYR_Id);


                    foreach (pgTempDTO ph1 in data.pgTempDTO)
                    {
                        var type1 = ph1.LPMTR_Resources.Substring(ph1.LPMTR_Resources.Length - 3);
                        var type2 = ph1.LPMTR_Resources.Substring(ph1.LPMTR_Resources.Length - 3);

                        if (type1== "jpg")
                        {
                            result1.PRYR_Id = data.PRYR_Id;
                            result1.PRYRP_Photos = ph1.LPMTR_Resources;
                            result1.PRYRP_ActiveFlag = true;
                            result1.UpdatedDate = DateTime.Now;
                            result1.PRYRP_UpdatedBy = data.UserId;
                            _dbContext.Add(result1);
                        }

                        else
                        {
                            result2.PRYR_Id = data.PRYR_Id;
                            result1.PRYRP_Photos = ph1.LPMTR_Resources;
                            result2.PRYRV_ActiveFlag = true;
                            result2.UpdatedDate = DateTime.Now;
                            result2.PRYRV_UpdatedBy = data.UserId;
                            _dbContext.Add(result2);
                        }

                       
                    }
                    
                        _dbContext.Update(result1);
                        _dbContext.Update(result2);
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

                    foreach (pgTempDTO ph1 in data.pgTempDTO)
                    {
                        var type1 = ph1.LPMTR_Resources.Substring(ph1.LPMTR_Resources.Length - 3);
                        var type2 = ph1.LPMTR_Resources.Substring(ph1.LPMTR_Resources.Length - 3);

                        if (type1 == "jpg")
                        {
                            ProgramsYearlyPhotosDMO objpge1 = new ProgramsYearlyPhotosDMO();
                            objpge1.PRYR_Id = data.PRYR_Id;
                            objpge1.PRYRP_Photos = ph1.LPMTR_Resources;
                            objpge1.PRYRP_ActiveFlag = true;
                            objpge1.UpdatedDate = DateTime.Now;
                            objpge1.PRYRP_UpdatedBy = data.UserId;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.PRYRP_CreatedBy = data.UserId;
                            _dbContext.Add(objpge1);
                        }
                        else
                        {
                            ProgramsYearlyVideosDMO objpge2 = new ProgramsYearlyVideosDMO();
                            objpge2.PRYR_Id = data.PRYR_Id;
                            objpge2.PRYRV_Videos = ph1.LPMTR_Resources;
                            objpge2.PRYRV_ActiveFlag = true;
                            objpge2.UpdatedDate = DateTime.Now;
                            objpge2.PRYRV_UpdatedBy = data.UserId;
                            objpge2.CreatedDate = DateTime.Now;
                            objpge2.PRYRV_CreatedBy = data.UserId;
                            _dbContext.Add(objpge2);
                        }

                   }

                    
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
                List<ProgramsYearlyPhotosDMO> photos = new List<ProgramsYearlyPhotosDMO>();
                photos = _dbContext.ProgramsYearlyPhotosDMO.Where(e => e.PRYR_Id == data.PRYR_Id).ToList();
                data.Photolist = photos.ToArray();

                List<ProgramsYearlyVideosDMO> videos = new List<ProgramsYearlyVideosDMO>();
                videos = _dbContext.ProgramsYearlyVideosDMO.Where(e => e.PRYR_Id == data.PRYR_Id).ToList();
                data.Videolist = videos.ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public OnlineProgramDTO deleterecord(OnlineProgramDTO data)
        {
         
            try
            {
                var lorg1 = _dbContext.ProgramsYearlyPhotosDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();
                var lorg2 = _dbContext.ProgramsYearlyVideosDMO.Where(t => t.PRYR_Id == data.PRYR_Id).ToList();

                if (lorg2.Any())
                {
                        _dbContext.Remove(lorg2.ElementAt(0));
                }
                if (lorg1.Any())
                {
                    _dbContext.Remove(lorg1.ElementAt(0));
                }

                //CloudStorageAccount storageAccount;
                //CloudBlobClient cloudBlobClient;

                ////connection is kept in app.config
                //storageAccount =
                //    CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                //cloudBlobClient = storageAccount.CreateCloudBlobClient();

                //Parallel.ForEach(cloudBlobClient.ListContainers(), x =>
                //{
                //    Parallel.ForEach(x.ListBlobs(), y =>
                //    {
                //        ((CloudBlockBlob)y).DeleteIfExists();
                //    });
                //});







                var contactexisttransaction = 0;
                using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _dbContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnresult = true;
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnresult = false;
                    }
                }


            }
            catch (Exception ee)
            {
                data.returnresult = false;
                Console.WriteLine(ee.Message);
            }
            return data;
        }


    }
}
