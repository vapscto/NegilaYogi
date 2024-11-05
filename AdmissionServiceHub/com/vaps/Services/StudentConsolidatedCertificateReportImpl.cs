using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentConsolidatedCertificateReportImpl : Interfaces.StudentConsolidatedCertificateReportInterface
    {
        private static ConcurrentDictionary<string, StudentConsolidatedCertificateReportDTO> _login = new ConcurrentDictionary<string, StudentConsolidatedCertificateReportDTO>();

        public DomainModelMsSqlServerContext _db;
        //public TTContext _ttcontext;
        public StudentConsolidatedCertificateReportImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _db = DomainModelMsSqlServerContext;
            //_ttcontext = ttcntx;
        }
        public StudentConsolidatedCertificateReportDTO GetAcademicYear(StudentConsolidatedCertificateReportDTO data)//int IVRMM_Id
        {
            data.getyear1 = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).ToArray();

            var list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            data.AcademicYear = list.ToArray();

            var Clist = _db.Adm_Certificates_Apply_DMO.OrderByDescending(t => t.ACERTAPP_Id).ToList();
            data.MasterCertificate = Clist.ToArray();
            return data;
        }

        public StudentConsolidateCertificateGetClassParaDTO GetClass(StudentConsolidateCertificateGetClassParaDTO data)//int IVRMM_Id
        {
            var list = (from a in _db.Masterclasscategory
                        from b in _db.School_M_Class
                        where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == a.MI_Id
                              && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true)
                        select new StudentConsolidateCertificateGetClassParaDTO
                        {
                            ASMCL_Id = b.ASMCL_Id,
                            asmcL_ClassName = b.ASMCL_ClassName,
                        }
          ).Distinct().ToArray();


            data.ClassDetails = list;

            return data;
        }



        public StudentConsolidateCertificateGetClassParaDTO GetSectionDetails(StudentConsolidateCertificateGetClassParaDTO data)//int IVRMM_Id
        {
            string asmcids = "0";
            //List<long> ids = new List<long>();
            if (data.Temp_ASMCLIds != null && data.Temp_ASMCLIds.Length > 0)
            {
                foreach (var c in data.Temp_ASMCLIds)
                {
                    //ids.Add(c.ASMC_Id);
                    asmcids = asmcids + "," + c.ASMC_Id;
                }
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Get_Sectionlist_for_ConsCertificate_rpt";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = asmcids });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.getsectionlist = retObject.ToArray();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }


            return data;
        }
        public StudentConsolidateCertificateGetClassParaDTO GetCertificateDetails(StudentConsolidateCertificateGetClassParaDTO data)
        {
            string asmcids = "0";
            //List<long> ids = new List<long>();
            if (data.Temp_ASMCLIds != null && data.Temp_ASMCLIds.Length > 0)
            {
                foreach (var c in data.Temp_ASMCLIds)
                {
                    //ids.Add(c.ASMC_Id);
                    asmcids = asmcids + "," + c.ASMC_Id;
                }
            }
            string asmsids = "0";
            //List<long> ids = new List<long>();
            if (data.Temp_ASMS_Ids != null && data.Temp_ASMS_Ids.Length > 0)
            {
                foreach (var c in data.Temp_ASMS_Ids)
                {
                    //ids.Add(c.ASMC_Id);
                    asmsids = asmsids + "," + c.ASMS_Id;
                }
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Get_ConsCertificate_rpt";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@ASC_ReportType", SqlDbType.VarChar) { Value = data.ASC_ReportType });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = asmcids });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = asmsids });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.GetCertificateDet = retObject.ToArray();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }


            return data;
        }

        public StudentConsolidateCertificateGetClassParaDTO GetStudentDetails(StudentConsolidateCertificateGetClassParaDTO data)
        {

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_Get_ConsCertificate_stud_rpt";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@ASC_ReportType", SqlDbType.VarChar) { Value = data.ASC_ReportType });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Ids });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Ids });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                            {
                                dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                            }
                            retObject.Add((ExpandoObject)dataRow1);
                        }
                    }
                    data.GetstudentDet = retObject.ToArray();
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            }


            return data;
        }
    }
}
