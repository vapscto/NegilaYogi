using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using PreadmissionDTOs.com.vaps.Fees;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace FeeServiceHub.com.vaps.services
{
    public class ClassCategoryReportImp : interfaces.ClassCategoryReportInterface
    {
        private static ConcurrentDictionary<string, ClassCategoryReportDTO> _login =
             new ConcurrentDictionary<string, ClassCategoryReportDTO>();

        public DomainModelMsSqlServerContext _db;

        public ClassCategoryReportImp(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }

        public ClassCategoryReportDTO getInitailData(int mi_id)
        {
            ClassCategoryReportDTO ctdo = new ClassCategoryReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.AcademicYear.Where(y=>y.Is_Active==true && y.MI_Id==mi_id).OrderByDescending(y => y.ASMAY_Order).ToList();
                ctdo.YearList = allyear.Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public ClassCategoryReportDTO SearchData(ClassCategoryReportDTO Clscatag)
        {
          
            try
            {
                List<ClassCategoryReportDTO> result = new List<ClassCategoryReportDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "getClassCategoryReportData";
                    cmd.CommandText = "FeeMastersReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Clscatag.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Clscatag.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.VarChar) { Value = Clscatag.type });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                            Clscatag.searchdatalist = retObject.Distinct().ToArray();
                        }
                        //using (var dataReader = cmd.ExecuteReader())
                        //{
                        //    while (dataReader.Read())
                        //    {
                        //        var dataRow = new ExpandoObject() as IDictionary<string, object>;
                        //        for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                        //        {

                        //            dataRow.Add(
                        //                   dataReader.GetName(iFiled),
                        //                   dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                        //               );
                        //        }
                        //        //result.Add(new ClassCategoryReportDTO
                        //        //{
                        //        //    //FMCC_ClassCategoryName = dataReader["FMCC_ClassCategoryName"].ToString(),
                        //        //    //ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                        //        //    //ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),


                        //        //});
                        //        Clscatag.searchdatalist = dataRow.ToArray();
                        //    }
                        //}
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

