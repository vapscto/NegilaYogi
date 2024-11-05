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
using System.Data;
using System.Data.SqlClient;

namespace FeeServiceHub.com.vaps.services
{
   
    public class FeeInstallmentDetailsImpl :interfaces.FeeInstallmentDetailsInterface
    {

        private static ConcurrentDictionary<string, FeeInstallmentsDetailsDTO> _login =
         new ConcurrentDictionary<string, FeeInstallmentsDetailsDTO>();

        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _db;
        DateTime? fromDate = null;
        DateTime? toDate = null;
        DateTime? applDate = null;
        DateTime? dueDate = null;
        //string fromDate;
        //string toDate ;
        //string applDate ;
        //string dueDate;
        string installment_name;
        string installmet_type;
        string number_of_installment;

        public FeeInstallmentDetailsImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }



        public FeeInstallmentsDetailsDTO getdetails(FeeInstallmentsDetailsDTO id)
        {
            
          
            try
            {
                List<FeeInstallmentsDetailsDTO> result = new List<FeeInstallmentsDetailsDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeInstallmentDetailsReportData_new";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_id", SqlDbType.BigInt)
                    {
                        Value=id.MI_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Asmay_id", SqlDbType.BigInt)
                    {
                        Value=id.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["FTIDD_FromDate"] != System.DBNull.Value)
                                {
                                   fromDate = Convert.ToDateTime(dataReader["FTIDD_FromDate"]);
                                }
                                else
                                {
                                    fromDate = null;
                                }
                                if (dataReader["FTIDD_ToDate"] != System.DBNull.Value)
                                {
                                    toDate = Convert.ToDateTime(dataReader["FTIDD_ToDate"]);
                                }
                                else
                                {
                                    toDate = null;
                                }
                                if (dataReader["ApplicableDate"] != System.DBNull.Value)
                                {
                                    applDate = Convert.ToDateTime(dataReader["ApplicableDate"]);
                                }
                                else
                                {
                                    applDate = null;
                                }
                                if (dataReader["FTIDD_DueDate"] != System.DBNull.Value)
                                {
                                    dueDate = Convert.ToDateTime(dataReader["FTIDD_DueDate"]);
                                }
                                else
                                {
                                    dueDate = null;
                                }
                                if (dataReader["Installment_Name"] != System.DBNull.Value)
                                {
                                    installment_name = Convert.ToString(dataReader["Installment_Name"]);
                                }
                                else
                                {
                                    installment_name = "NOT AVAILABLE";
                                }

                                 if (dataReader["Installment_Type"] != System.DBNull.Value)
                                {
                                    installmet_type = Convert.ToString(dataReader["Installment_Type"]);
                                }
                                else
                                {
                                    installmet_type = "NOT AVAILABLE";
                                }

                                if (dataReader["No_of_Installments"] != System.DBNull.Value)
                                {
                                    number_of_installment = Convert.ToString(dataReader["No_of_Installments"]);
                                }
                                else
                                {
                                    number_of_installment = "NOT AVAILABLE";
                                }


                                result.Add(new FeeInstallmentsDetailsDTO
                                {
                                    FMI_Installment_Type = installmet_type,
                                    FMI_No_Of_Installments =Convert.ToInt64(number_of_installment),
                                    FMI_Name = installment_name,
                                    From_Date =fromDate,
                                    To_Date = toDate,
                                    Applicable_Date = applDate,
                                    Due_Date = dueDate
                                });
                                id.InstallmentDatalist = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return id;
        }



    }
}
