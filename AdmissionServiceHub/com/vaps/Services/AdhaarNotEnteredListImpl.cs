using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AdhaarNotEnteredListImpl : Interfaces.AdhaarNotEnteredListInterface
    {
        public StudentAttendanceReportContext _db;
        public DomainModelMsSqlServerContext _dbcontext;

        ILogger<AdhaarNotEnteredListImpl> _acdimpl;
        public AdhaarNotEnteredListImpl(StudentAttendanceReportContext db, ILogger<AdhaarNotEnteredListImpl> acdimpl, DomainModelMsSqlServerContext dbcontext)
        {
            _db = db;
            _acdimpl = acdimpl;
            _dbcontext = dbcontext;
        }

        public async Task<AdhaarNotEnteredListDTO> getInitailData(AdhaarNotEnteredListDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d => d.MI_Id == ctdo.miid && d.Is_Active == true).ToListAsync();
                ctdo.academicList = allyear.OrderByDescending(y => y.ASMAY_Order).ToArray();

                List<MasterAcademic> defaultyear = new List<MasterAcademic>();
                defaultyear = _db.academicYear.Where(t => t.MI_Id == ctdo.miid && t.Is_Active == true && t.ASMAY_Id == ctdo.ASMAY_Id).ToList();
                ctdo.academicListdefault = defaultyear.OrderByDescending(a => a.ASMAY_Order).ToArray();


                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _db.admissionClass.Where(s => s.MI_Id == ctdo.miid && s.ASMCL_ActiveFlag == true).ToList();
                ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = _db.masterSection.Where(y => y.MI_Id == ctdo.miid && y.ASMC_ActiveFlag == 1).ToList();
                ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();

                ctdo.countrylist = _db.CountryDMO.OrderBy(c => c.IVRMMC_CountryName).ToArray();
                ctdo.statelist = _db.StateDMO.OrderBy(d => d.IVRMMS_Name).ToArray();

                ctdo.entryType = (from a in _db.AttendanceEntryTypeDMO
                                  where (a.MI_Id == ctdo.miid && a.ASMAY_Id == ctdo.ASMAY_Id)
                                  select new AdhaarNotEnteredListDTO
                                  {
                                      ASAET_Att_Type = a.ASAET_Att_Type,

                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<AdhaarNotEnteredListDTO> getserdata(AdhaarNotEnteredListDTO ctdo)
        {
            var asmclid = "0";
            var asmsid = "0";
            //int k = 0;
            if (ctdo.type == '1' || ctdo.type == 1)
            {
                var classlist1 = (from a in _db.admissionyearstudent
                                  from c in _db.admissionClass
                                  where (c.ASMCL_Id == a.ASMCL_Id
                                  && c.MI_Id == ctdo.miid && a.ASMAY_Id == ctdo.ASMAY_Id

                                  && c.ASMCL_ActiveFlag == true)
                                  select new StudentAttendanceReportDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      asmcL_ClassName = c.ASMCL_ClassName,
                                  }).Distinct().ToList();

                for (int i = 0; i < classlist1.Count; i++)
                {
                    if (i == 0)
                    {
                        asmclid = classlist1[i].ASMCL_Id.ToString();
                    }
                    else
                    {
                        asmclid = asmclid + ',' + classlist1[i].ASMCL_Id.ToString();
                    }
                }

                var sectionlist1 = (from a in _db.admissionyearstudent
                                    from c in _db.masterSection
                                    where (c.ASMS_Id == a.ASMS_Id
                                    && c.MI_Id == ctdo.miid && a.ASMAY_Id == ctdo.ASMAY_Id
                                    && c.ASMC_ActiveFlag == 1)
                                    select new AdhaarNotEnteredListDTO
                                    {
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = c.ASMC_SectionName,
                                    }
                                ).Distinct().ToList();

                for (int i = 0; i < sectionlist1.Count; i++)
                {
                    if (i == 0)
                    {
                        asmsid = sectionlist1[i].ASMS_Id.ToString();
                    }
                    else
                    {
                        asmsid = asmsid + ',' + sectionlist1[i].ASMS_Id.ToString();
                    }
                }
            }
            else
            {
                if (ctdo.ASMC_Id == 0)
                {
                    asmclid = ctdo.ASMCL_Id.ToString();

                    var sectiondata = (from a in _db.admissionClass
                                       from b in _db.masterSection
                                       from c in _db.academicYear
                                       from d in _db.Masterclasscategory
                                       from e in _db.AdmSchoolMasterClassCatSec

                                       where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id && d.MI_Id == ctdo.miid && d.ASMAY_Id == ctdo.ASMAY_Id && d.ASMCL_Id == ctdo.ASMCL_Id && e.ASMCCS_ActiveFlg == true)
                                       select b
                             ).Distinct().OrderBy(g => g.ASMC_Order).ToList();


                    if (sectiondata.Count > 0)
                    {
                        for (int i = 0; i < sectiondata.Count; i++)
                        {
                            if (i == 0)
                            {
                                asmsid = sectiondata[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + sectiondata[i].ASMS_Id.ToString();
                            }
                        }
                    }
                }
                else
                {
                    asmclid = ctdo.ASMCL_Id.ToString();
                    asmsid = ctdo.ASMC_Id.ToString();
                }
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "AdhaarNotEnteredList";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(ctdo.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.NVarChar)
                {
                    Value = asmclid
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.NVarChar)
                {
                    Value = asmsid
                });

                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(ctdo.miid)
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
                    ctdo.AdhaarNotEnteredList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
                }
                return ctdo;
            }
        }
        public async Task<AdhaarNotEnteredListDTO> getsection(AdhaarNotEnteredListDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                

                ctdo.SectionList = (from a in _db.AdmSchoolMasterClassCatSec
                                    from b in _db.Masterclasscategory
                                    from c in _db.admissionClass
                                    from d in _db.masterSection
                                    where (a.ASMCC_Id == b.ASMCC_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCCS_ActiveFlg == true
                                    && b.Is_Active == true && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1 && b.MI_Id == ctdo.miid
                                    && b.ASMCL_Id == ctdo.ASMCL_Id && b.ASMAY_Id == ctdo.ASMAY_Id)
                                    select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public async Task<AdhaarNotEnteredListDTO> getclass(AdhaarNotEnteredListDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {


                ctdo.classlist = (from a in _db.admissionClass
                                  from b in _db.Masterclasscategory
                                  from c in _db.academicYear
                                  where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true
                                  && c.Is_Active == true && b.ASMAY_Id == ctdo.ASMAY_Id)
                                  select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }


        //classchange
        public async Task<ClassChangeDTO> getInitailyear(ClassChangeDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d => d.MI_Id == ctdo.MI_Id && d.Is_Active == true).ToListAsync();
                ctdo.academicList = allyear.OrderByDescending(y => y.ASMAY_Order).ToArray();






            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<ClassChangeDTO> searchdataclass(ClassChangeDTO ctdo)
        {


            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AdmissionClassChange";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.ASMAY_Id)
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
                        ctdo.dataclass = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
                    }
                    return ctdo;
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
            }
            return ctdo;
        }

        public async Task<AdhaarNotEnteredListDTO> getstudents(AdhaarNotEnteredListDTO ctdo)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Students_NotPromoted_CACY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.ASMAY_Id)
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
                        ctdo.searchdatalist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
                    }
                    return ctdo;
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
            }
            return ctdo;
        }

        public async Task<AdhaarNotEnteredListDTO> Getcountrystatedata(AdhaarNotEnteredListDTO ctdo)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CountryStateWiseStudentList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@Country_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.countrytype)
                    });
                    cmd.Parameters.Add(new SqlParameter("@State_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.statetype)
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(ctdo.type)
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
                        ctdo.studatalist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
                    }
                    return ctdo;
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error In Adhaar Entered List Report :" + ex.Message);
            }
            return ctdo;
        }
        public async Task<AdhaarNotEnteredListDTO> getEntryType(AdhaarNotEnteredListDTO data)
        {

            try
            {

                data.entryType = (from a in _db.AttendanceEntryTypeDMO
                                  where (a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id)
                                  select new AdhaarNotEnteredListDTO
                                  {
                                      
                                      ASAET_Att_Type = a.ASAET_Att_Type,

                                  }).Distinct().ToArray();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<AdhaarNotEnteredListDTO> getClassEntryType(AdhaarNotEnteredListDTO data)
        {

            try
            {
                List<string> Atdtype = new List<string>();
                foreach (var e in data.ASAET_Att_TypeArray)
                {
                    Atdtype.Add(e.ASAET_Att_Type);
                }

                data.classlist = (from a in _db.admissionClass
                                  from b in _db.AttendanceEntryTypeDMO
                                  where (a.ASMCL_Id == b.ASMCL_Id && a.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id && Atdtype.Contains(b.ASAET_Att_Type))
                                  select a).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<AdhaarNotEnteredListDTO> getsectionlist(AdhaarNotEnteredListDTO data)
        {

            try
            {

                List<long> classid = new List<long>();
                foreach (var e in data.classlsttwo)
                {
                    classid.Add(e.ASMCL_Id);
                }
                data.entryType = _db.AttendanceEntryTypeDMO.Where(a => a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id && classid.Contains(a.ASMCL_Id)).
                    Select(a => a.ASAET_Att_Type).Distinct().ToArray();


                //data.entryType = (from a in _db.AttendanceEntryTypeDMO
                //                  from b in _db.admissionClass
                //                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                //                   && d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id && classid.Contains(d.ASMCL_Id) && e.ASMCCS_ActiveFlg == true)
                //                   select b
                //               ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();

                var sectiondata = (from a in _db.admissionClass
                                   from b in _db.masterSection
                                   from c in _db.academicYear
                                   from d in _db.Masterclasscategory
                                   from e in _db.AdmSchoolMasterClassCatSec
                                   where (a.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && c.ASMAY_Id == d.ASMAY_Id && d.ASMCC_Id == e.ASMCC_Id
                                   && d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id && classid.Contains(d.ASMCL_Id) && e.ASMCCS_ActiveFlg == true)
                                   select b
                                 ).Distinct().OrderBy(g => g.ASMC_Order).ToArray();


                if (sectiondata.Length > 0)
                {
                    data.fillsection = sectiondata.ToArray();
                }
                //else
                //{
                //    List<School_M_Section> secname = new List<School_M_Section>();
                //    secname = _Classwisestudentdetailscontext.school_M_Section.Where(s => s.MI_Id == data.mid && s.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToList();
                //    data.fillsection = secname.ToArray();
                //}


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<AdhaarNotEnteredListDTO> getAttendencenotDoneReport(AdhaarNotEnteredListDTO data)
        {

            try
            {
                string Class_Id = "0";
                foreach (var e in data.classlsttwo)
                {
                    Class_Id = Class_Id + "," + e.ASMCL_Id; // Sectionid.Add(e.ASMCL_Id);
                }
                string Section_Id = "0";
                foreach (var e in data.sectionlistarray)
                {
                    Section_Id = Section_Id + "," + e.ASMS_Id; // Sectionids.Add(e.ASMS_Id);
                }

                var FromDate = data.fromdate.Date.ToString("yyyy-MM-dd");
                var ToDate = data.todate.Date.ToString("yyyy-MM-dd");

                if (data.flag == "Entered")
                {

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Attendancestaffentry_countdetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.miid)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = Class_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                        {
                            Value = Section_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                        {
                            Value = FromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                        {
                            Value = ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ENTRY_TYPE", SqlDbType.VarChar)
                        {
                            Value = data.ASAET_Att_Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_ID", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.VarChar)
                        {
                            Value = data.flag
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
                            data.attendenceNotDoneReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if(data.flag == "Not Entered")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Attendancestaffentry_Notdone";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.miid)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                        {
                            Value = Convert.ToInt64(data.ASMAY_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                        {
                            Value = Class_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                        {
                            Value = Section_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                        {
                            Value = FromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                        {
                            Value = ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ENTRY_TYPE", SqlDbType.VarChar)
                        {
                            Value = data.ASAET_Att_Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_ID", SqlDbType.VarChar)
                        {
                            Value = ""
                        });
                        //cmd.Parameters.Add(new SqlParameter("@TYPE", SqlDbType.VarChar)
                        //{
                        //    Value = data.flag
                        //});

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
                            }
                            data.attendenceNotDoneReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public AdhaarNotEnteredListDTO emailsend(AdhaarNotEnteredListDTO data)
        {

            Email Email = new Email(_dbcontext);
            for (var i = 0; i < data.ASAET_Att_TypeArray.Length; i++)
            {
                if (data.ASAET_Att_TypeArray[i].AMST_emailId != "null" && data.ASAET_Att_TypeArray[i].AMST_emailId != null && data.ASAET_Att_TypeArray[i].AMST_emailId != "")
                {
                   // , data.ASAET_Att_TypeArray[i].ASMCL_className, data.ASAET_Att_TypeArray[i].ASMC_SectionName, data.ASAET_Att_TypeArray[i].NOTENTERED_DATE
                        data.success = Email.sendmail_absentees(data.miid, data.ASAET_Att_TypeArray[i].AMST_emailId, "NotEntered", data.ASAET_Att_TypeArray[i].hrmE_Id, data.ASAET_Att_TypeArray[i].employeename, data.ASAET_Att_TypeArray[i].fromdate, data.ASAET_Att_TypeArray[i].ASMCL_className, data.ASAET_Att_TypeArray[i].ASMC_SectionName);
                  
                }
            }
            return data;
        }
    }

}

