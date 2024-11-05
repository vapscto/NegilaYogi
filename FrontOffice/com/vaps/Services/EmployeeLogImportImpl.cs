using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOffice.com.vaps.Services
{
    

        public class EmployeeLogImportImpl : Interfaces.EmployeeLogImportInterface
    {
            public FOContext _FOContext;
            public EmployeeLogImportImpl(FOContext context)
            {
                _FOContext = context;
            }

            public EmployeeLogImportDTO Savedata(EmployeeLogImportDTO data)
            {
                try
                {
                    if (data.empDataimport != null)
                    {
                        if (data.empDataimport.Length > 0)
                        {

                            foreach (var item in data.empDataimport)
                            {
                                var HRME_Id = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_EmployeeCode.Trim() == item.EmployeeCode).Select(t => t.HRME_Id).FirstOrDefault();
                                if (HRME_Id > 0)
                                {
                                    var MI_Id = _FOContext.HR_Master_Employee_DMO.Where(t => t.HRME_Id == HRME_Id).Select(t => t.MI_Id).FirstOrDefault();
                                    var FOEP_Id = _FOContext.FO_Emp_Punch.Where(t => t.HRME_Id == HRME_Id && t.FOEP_PunchDate == Convert.ToDateTime(item.PunchDate)).Select(t => t.FOEP_Id).FirstOrDefault();
                                    if (FOEP_Id > 0)
                                    {
                                        var FOEPD_Id = _FOContext.FO_Emp_Punch_Details.Where(t => t.FOEP_Id == FOEP_Id && t.FOEPD_PunchTime == item.PunchIN_Time /*&& t.FOEPD_PunchTime == item.PunchOut_Time*/).Select(t => t.FOEPD_Id).FirstOrDefault();
                                        if (FOEPD_Id > 0)
                                        {

                                            data.message = "Exist";

                                        }
                                        else
                                        {
                                            FO_Emp_Punch_DetailsDMO obj1 = new FO_Emp_Punch_DetailsDMO();
                                            if (item.PunchIN_Time != null && item.PunchIN_Time != "")
                                            {
                                                obj1.MI_Id = data.MI_Id;
                                                obj1.FOEP_Id = FOEP_Id;
                                                obj1.FOEPD_PunchTime = item.PunchIN_Time;
                                                obj1.FOEPD_InOutFlg = "I";
                                                obj1.CreatedDate = DateTime.Now;
                                                obj1.UpdatedDate = DateTime.Now;
                                                obj1.FOEPD_Flag = "1";
                                                _FOContext.Add(obj1);
                                            }
                                            FO_Emp_Punch_DetailsDMO obj3 = new FO_Emp_Punch_DetailsDMO();
                                            if (item.PunchOut_Time != null && item.PunchOut_Time != "")
                                            {
                                                obj3.MI_Id = data.MI_Id;
                                                obj3.FOEP_Id = FOEP_Id;
                                                obj3.FOEPD_PunchTime = item.PunchOut_Time;
                                                obj3.FOEPD_InOutFlg = "O";
                                                obj3.CreatedDate = DateTime.Now;
                                                obj3.UpdatedDate = DateTime.Now;
                                                obj3.FOEPD_Flag = "1";
                                                _FOContext.Add(obj3);
                                            }
                                        }
                                    }
                                    else
                                    {

                                        FO_Emp_PunchDMO obj2 = new FO_Emp_PunchDMO();
                                        obj2.MI_Id = data.MI_Id;
                                        obj2.HRME_Id = HRME_Id;
                                        obj2.FOEP_PunchDate = Convert.ToDateTime(item.PunchDate).Date;
                                        obj2.CreatedDate = DateTime.Now;
                                        obj2.UpdatedDate = DateTime.Now;
                                        obj2.FOEP_Flag = true;
                                        _FOContext.Add(obj2);

                                        FO_Emp_Punch_DetailsDMO obj1 = new FO_Emp_Punch_DetailsDMO();
                                        if (item.PunchIN_Time != null && item.PunchIN_Time != "")
                                        {
                                            obj1.MI_Id = data.MI_Id;
                                            obj1.FOEP_Id = obj2.FOEP_Id;
                                            obj1.FOEPD_PunchTime = item.PunchIN_Time;
                                            obj1.FOEPD_InOutFlg = "I";
                                            obj1.CreatedDate = DateTime.Now;
                                            obj1.UpdatedDate = DateTime.Now;
                                            obj1.FOEPD_Flag = "1";
                                            _FOContext.Add(obj1);
                                        }
                                        FO_Emp_Punch_DetailsDMO obj3 = new FO_Emp_Punch_DetailsDMO();
                                        if (item.PunchOut_Time != null && item.PunchOut_Time != "")
                                        {
                                            obj3.MI_Id = data.MI_Id;
                                            obj3.FOEP_Id = obj2.FOEP_Id;
                                            obj3.FOEPD_PunchTime = item.PunchOut_Time;
                                            obj3.FOEPD_InOutFlg = "O";
                                            obj3.CreatedDate = DateTime.Now;
                                            obj3.UpdatedDate = DateTime.Now;
                                            obj3.FOEPD_Flag = "1";
                                            _FOContext.Add(obj3);
                                        }

                                    }


                                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "FO_PunchDetailsUpdationAfterDownLoad";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                        {
                                            Value = MI_Id

                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date)
                                        {
                                            Value = DateTime.Now.Date
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date)
                                        {
                                            Value = DateTime.Now.Date
                                        });
                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                    }


                                }
                                else
                                {
                                    data.message = "NotExist";

                                }
                            }
                            var n = _FOContext.SaveChanges();
                            if (n > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    data.returnval = false;
                }
                return data;
            }
        }
    
}
