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
    public class StudentWiseFeeCollectionImpl : interfaces.StudentWiseFeeCollectionInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public StudentWiseFeeCollectionImpl(FeeGroupContext frgContext)
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

                data.classlist = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == data.MI_ID).ToArray();
                data.sectionlist = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_ID).ToArray();


                data.studentname = (from a in _FeeGroupContext.Adm_M_Student
                                   
                                    where (a.AMST_ActiveFlag == 1 && a.MI_Id == data.MI_ID )
                                    select new CategoryWiseFeeCollectionDTO
                                    {
                                        amstid = a.AMST_Id,
                                        AMST_Firstname = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim()

                                    }
                          ).Distinct().ToArray();

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
                cmd.CommandText = "Headwisefeeamtdetails";
                cmd.CommandType = CommandType.StoredProcedure;
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
                cmd.Parameters.Add(new SqlParameter("@AMST_Id",
              SqlDbType.VarChar)
                {
                    Value = temp.amstid
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

        public async Task<CategoryWiseFeeCollectionDTO> onchangeacademic([FromBody] CategoryWiseFeeCollectionDTO temp)
        {


            try {
                temp.classlist = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == temp.MI_ID).ToArray();
                temp.sectionlist = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == temp.MI_ID).ToArray();

                temp.studentname=(from a in _FeeGroupContext.Adm_M_Student
                                  from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  where (a.AMST_Id==b.AMST_Id && a.AMST_ActiveFlag==1 && a.MI_Id==temp.MI_ID && b.ASMAY_Id==temp.ASMAY_Id  )
                                  select new CategoryWiseFeeCollectionDTO
                                  {
                                      amstid = a.AMST_Id,
                                      AMST_Firstname = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim()

                                  }
                          ).Distinct().ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
        }
        public async Task<CategoryWiseFeeCollectionDTO> onselectclass([FromBody] CategoryWiseFeeCollectionDTO temp)
        {


            try { 
          
                temp.studentname=(from a in _FeeGroupContext.Adm_M_Student
                                  from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  where (a.AMST_Id==b.AMST_Id && a.AMST_ActiveFlag==1 && a.MI_Id==temp.MI_ID && b.ASMAY_Id==temp.ASMAY_Id && b.ASMCL_Id==temp.ASMCL_Id)
                                  select new CategoryWiseFeeCollectionDTO
                                  {
                                      amstid = a.AMST_Id,
                                      AMST_Firstname = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim()

                                  }
                          ).Distinct().ToArray();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
        }
        public async Task<CategoryWiseFeeCollectionDTO> onselectsection([FromBody] CategoryWiseFeeCollectionDTO temp)
        {


            try {

                temp.studentname = (from a in _FeeGroupContext.Adm_M_Student
                                    from b in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_ActiveFlag == 1 && a.MI_Id == temp.MI_ID && b.ASMAY_Id == temp.ASMAY_Id && b.ASMCL_Id == temp.ASMCL_Id && b.ASMS_Id == temp.ASMS_Id)
                                    select new CategoryWiseFeeCollectionDTO
                                    {
                                        amstid = a.AMST_Id,
                                        AMST_Firstname = ((a.AMST_FirstName == null || a.AMST_FirstName == "0" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim() 
                                    
                                    }
                          ).Distinct().ToArray();

            }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
        }

    }
}
