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
using DomainModel.Model.com.vaps.admission;


namespace FeeServiceHub.com.vaps.services
{
    public class FeeRemittanceCertificateImpl : interfaces.FeeRemittanceCertificateInterface
    {
        private static ConcurrentDictionary<string, FeeGroupWiseStudentReportDTO> _login =
             new ConcurrentDictionary<string, FeeGroupWiseStudentReportDTO>();

        public FeeGroupContext _db;

        public FeeRemittanceCertificateImpl(FeeGroupContext db)
        {
            _db = db;
        }

        public FeeRemittanceCertificateDTO getInitailData(int mi_id)
        {
            FeeRemittanceCertificateDTO ctdo = new FeeRemittanceCertificateDTO();
            
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(y=>y.Is_Active==true && y.MI_Id==mi_id).ToList();
                ctdo.YearList = allyear.Distinct().ToArray();
                ctdo.DateList = _db.Fee_Payment.Select(d => d.FYP_Receipt_No).ToArray();

                List<FeeHeadDMO> feeheadnames = new List<FeeHeadDMO>();
                feeheadnames = _db.feehead.Where(f => f.FMH_ActiveFlag ==true && f.MI_Id == mi_id).ToList();
                ctdo.feehead_Name_list = feeheadnames.ToArray();
            }
             
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        
        public FeeRemittanceCertificateDTO getAdm_Name(FeeRemittanceCertificateDTO Clscatag)
        {
            try
            {
                List<FeeRemittanceCertificateDTO> result = new List<FeeRemittanceCertificateDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "fee_remittance_certificate_StuName";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@asmay_Id", SqlDbType.Int) { Value = Clscatag.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = Clscatag.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.Int) { Value = Clscatag.Adm_no_name});
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["Name"] != System.DBNull.Value)
                                {
                                    result.Add(new FeeRemittanceCertificateDTO
                                    {
                                        Student_Name = dataReader["Name"].ToString(),
                                        AMST_Id = int.Parse(dataReader["AMST_Id"].ToString()),
                                    });
                                }

                                //Clscatag.Student_Name_List = result.Where(s=>s.Student_Name!=null ).ToArray();
                                Clscatag.Student_Name_List = result.GroupBy(x => x.AMST_Id).Select(x => x.First()).ToArray();

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
            return Clscatag;
        }

        public FeeRemittanceCertificateDTO SearchData(FeeRemittanceCertificateDTO Clscatag)
        {
            DateTime? Receipt_Date_null = null;
            try
            {
                List<FeeRemittanceCertificateDTO> result = new List<FeeRemittanceCertificateDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "fee_remittance_certificate_Grid_View";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@asmay_Id", SqlDbType.Int) { Value = Clscatag.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = Clscatag.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@amst_id", SqlDbType.Int) { Value = Clscatag.AMST_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                if (dataReader["FYP_Date"] != System.DBNull.Value)
                                {
                                    Receipt_Date_null = Convert.ToDateTime(dataReader["FYP_Date"].ToString());
                                }
                                else
                                {
                                    Receipt_Date_null = null;
                                }
                                result.Add(new FeeRemittanceCertificateDTO
                                {
                                    AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                    ASMCL_ClassName = dataReader["Class_Name"].ToString(),
                                    StudentName = dataReader["Student_Name"].ToString(),
                                    Father_Name= dataReader["AMST_FatherName"].ToString(),
                                    Mother_Name = dataReader["AMST_MotherName"].ToString(),
                                    Receipt_Date= Receipt_Date_null,
                                    Receipt_Number= dataReader["FYP_Receipt_No"].ToString(),
                                    Tuition_Fee_Amount= Convert.ToDecimal(dataReader["FYP_Tot_Amount"].ToString()),

                            });
                                Clscatag.Fee_rem_cer_list = result.ToArray();
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
            return Clscatag;
        }


    }
}
