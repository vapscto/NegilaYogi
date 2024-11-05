using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class TotalSeatAllotmentImpl : TotalSeatAllotmentInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public TotalSeatAllotmentImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public TotalSeatAllotmentDTO getdetails(TotalSeatAllotmentDTO data)
        {
            try
            {
                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public TotalSeatAllotmentDTO onselectAcdYear(TotalSeatAllotmentDTO data)
        {
            try
            {
                data.courselist = (from a in _clgadmctxt.MasterCourseDMO
                                   from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public TotalSeatAllotmentDTO onselectCourse(TotalSeatAllotmentDTO data)
        {
            try
            {
                var semisterlist = (from a in _clgadmctxt.CLG_Adm_Master_SemesterDMO
                                    from b in _clgadmctxt.CLG_Adm_College_AY_CourseDMO
                                    from c in _clgadmctxt.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _clgadmctxt.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public TotalSeatAllotmentDTO onreport(TotalSeatAllotmentDTO data)
        {
            try
            {
                var quota_id = "";
                var quotaid = "";
                for (int i = 0; i < data.TempararyArrayListcoloumn.Length; i++)
                {
                    string Id = data.TempararyArrayListcoloumn[i].ACQ_Id.ToString();
                    if (Id != "0" && Id != null)
                    {
                        quotaid = Id + "," + quotaid;
                    }
                }
                quota_id = quotaid.TrimEnd(',');
                List<TotalSeatAllotmentDTO> result = new List<TotalSeatAllotmentDTO>();
                List<TotalSeatAllotmentDTO> result1 = new List<TotalSeatAllotmentDTO>();
                List<TotalSeatAllotmentDTO> result2 = new List<TotalSeatAllotmentDTO>();

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Total_seat_allotment";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.Int) { Value = data.AMCO_Id });   
                    cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.Int) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@acqq_id", SqlDbType.VarChar) { Value = quota_id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TotalSeatAllotmentDTO
                                {
                                    ACSCD_SeatNos = Convert.ToInt32(dataReader["ACSCD_SeatNos"].ToString()),
                                    AMB_Id = Convert.ToInt32(dataReader["AMB_Id"].ToString()),
                                    ACQ_Id = Convert.ToInt32(dataReader["ACQ_Id"].ToString()),
                                    ACQ_QuotaName = dataReader["ACQ_QuotaName"].ToString(),
                                    AMCO_CourseName = dataReader["AMCO_CourseName"].ToString(),
                                    AMB_BranchName = dataReader["AMB_BranchName"].ToString(),
                                    AMSE_SEMName = dataReader["AMSE_SEMName"].ToString(),
                                    count = Convert.ToInt32(dataReader["admitted_seats"].ToString())
                                });
                                data.datareport = result.ToArray();
                            }
                            dataReader.NextResult();
                            while (dataReader.Read())
                            {
                                result1.Add(new TotalSeatAllotmentDTO
                                {
                                    acqid = Convert.ToInt32(dataReader["acqid"].ToString()),
                                    totalseats = Convert.ToInt32(dataReader["totalseats"].ToString()),
                                    admitted = Convert.ToInt32(dataReader["admitted"].ToString()),
                                    vac = Convert.ToInt32(dataReader["vac"].ToString())
                                   
                                });
                                data.datareport1 = result1.ToArray();
                            }
                            dataReader.NextResult();
                            while (dataReader.Read())
                            {
                                result2.Add(new TotalSeatAllotmentDTO
                                {
                                    amb = Convert.ToInt32(dataReader["amb"].ToString()),
                                    totalseats = Convert.ToInt32(dataReader["totalseats"].ToString()),
                                    admitted = Convert.ToInt32(dataReader["admitted"].ToString()),
                                    vac = Convert.ToInt32(dataReader["vac"].ToString())

                                });
                                data.datareport2 = result2.ToArray();
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
