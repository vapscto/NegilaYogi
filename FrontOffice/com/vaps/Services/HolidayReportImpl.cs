using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class HolidayReportImpl:Interfaces.HolidayReportInterface
    {
        private static ConcurrentDictionary<string, MasterHolidayDTO> _login =
     new ConcurrentDictionary<string, MasterHolidayDTO>();

        public DomainModelMsSqlServerContext _db;
        public FOContext _frnt;


        public HolidayReportImpl(DomainModelMsSqlServerContext db, FOContext FO)
        {
            _db = db;
            _frnt = FO;
        }
        public MasterHolidayDTO getdata(int id)
        {
            MasterHolidayDTO dto = new MasterHolidayDTO();
            //  dto.yeardropdown = _db.AcademicYear.Where(d => d.MI_Id == id && d.Is_Active == true).ToArray();
            dto.yeardropdown = (from m in _frnt.HR_Master_LeaveYearDMO
                                where m.MI_Id == id && m.HRMLY_ActiveFlag.Equals(true)
                                select new AcademicDTO
                                {
                                    ASMAY_Id = m.HRMLY_Id,
                                    ASMAY_Year = m.HRMLY_LeaveYear,
                                    HRMLY_LeaveYearOrder = m.HRMLY_LeaveYearOrder
                                }).OrderBy(t => t.HRMLY_LeaveYearOrder).ToArray();
            dto.holidayType = _frnt.holidayWorkingDayType.Where(d => d.MI_Id == id && d.FOHWDT_ActiveFlg == true).ToArray();

            List<TT_Master_DayDMO> days = new List<TT_Master_DayDMO>();
            days = _frnt.TT_Master_DayDMO.Where(d => d.MI_Id == id).ToList();
            dto.dayslist = days.ToArray();

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "master_holiday_report";
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@mi_id",
                   SqlDbType.BigInt)
                {
                    Value = id
                });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //var data = cmd.ExecuteNonQuery();

                try
                {
                    // var data = cmd.ExecuteNonQuery();

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
                    dto.report_list = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            }
            return dto;
        }

        public MasterHolidayDTO ReportList(MasterHolidayDTO stu1)
        {
            try
            {
                MasterHolidayDTO acdmc = new MasterHolidayDTO();
                stu1.report_list = (from b in _frnt.holidaydate
                                    from c in _frnt.holidayWorkingDayType
                                    where (b.MI_Id == c.MI_Id && b.HRMLY_Id == stu1.HRMLY_Id && b.FOHWDT_Id == stu1.FOHWDT_Id && b.FOMHWD_ActiveFlg == true)
                                    select new MasterHolidayDTO
                                    {
                                        FOMHWD_HolidayWDName = b.FOMHWDD_Name,
                                        FOMHWDD_Date = b.FOMHWDD_ToDate,
                                        //FOMHWDD_Date = c.FOMHWDD_Date
                                    }
                                   ).Distinct().ToArray();
                if (stu1.report_list.Length > 0)
                {
                    stu1.count = stu1.report_list.Length;
                }
                else
                {
                    stu1.count = 0;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu1;

        }
    }
}
