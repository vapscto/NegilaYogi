using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using System.IO;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Transport;
namespace FeeServiceHub.com.vaps.services
{
    public class DefaulterFeeCollectionGraphImpl : interfaces.DefaulterFeeCollectionGraphInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public DefaulterFeeCollectionGraphImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public CategoryWiseFeeCollectionDTO getdetails(CategoryWiseFeeCollectionDTO data)
        {
            try
            {


                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == data.MI_ID).OrderByDescending(y => y.ASMAY_Order).ToList();
                data.adcyear = year.GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                data.terms = _FeeGroupContext.feeTr.Where(y => y.MI_Id == data.MI_ID && y.FMT_ActiveFlag == true).ToArray();
                




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }




        public async Task<CategoryWiseFeeCollectionDTO> radiobtndata([FromBody] CategoryWiseFeeCollectionDTO temp)
        {



            using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "[DefaulterReportCount]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 90000000;
                cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                    SqlDbType.VarChar)
                {
                    Value = temp.MI_ID
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.VarChar)
                {
                    Value = temp.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@FMT_Id",
                  SqlDbType.VarChar)
                {
                    Value = temp.FMT_Id
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    temp.studentalldata = retObject.ToArray();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
            }

        }
    
    }
}
