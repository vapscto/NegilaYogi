using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
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
    public class CategorySeatDistributionImpl : CategorySeatDistributionInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;

        public CategorySeatDistributionImpl(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }

        public CategorySeatDistributionDTO getdetails1(CategorySeatDistributionDTO data)
        {
            try
            {

                List<CategorySeatDistributionDTO> result = new List<CategorySeatDistributionDTO>();

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Category_getDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new CategorySeatDistributionDTO
                                { 
                                    AMB_Id = Convert.ToInt32(dataReader["AMB_Id"].ToString()),
                                    ACQ_Id = Convert.ToInt32(dataReader["ACQ_Id"].ToString()),
                                    ACQ_QuotaName = dataReader["ACQ_QuotaName"].ToString(),
                                    ACQC_CategoryName = dataReader["ACQC_CategoryName"].ToString(),
                                    AMCO_CourseName = dataReader["AMCO_CourseName"].ToString(),
                                    AMB_BranchName = dataReader["AMB_BranchName"].ToString(),
                                    AMSE_SEMName = dataReader["AMSE_SEMName"].ToString(),
                                    count = Convert.ToInt32(dataReader["admitted_seats"].ToString())
                                });
                                data.datareport = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }


                

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CategorySeatDistributionDTO getdetails(CategorySeatDistributionDTO data)
        {
            try
            {

                data.acdlist = _clgadmctxt.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.courselist = _clgadmctxt.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _clgadmctxt.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semlist = _clgadmctxt.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.quotalist = _clgadmctxt.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }


        public CategorySeatDistributionDTO onreport(CategorySeatDistributionDTO data)
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
                List<CategorySeatDistributionDTO> result = new List<CategorySeatDistributionDTO>();

                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Category_seat_distribution";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.Int) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@amco_id", SqlDbType.Int) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@amse_id", SqlDbType.Int) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@amb_id", SqlDbType.Int) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@acq_id", SqlDbType.VarChar) { Value = quota_id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new CategorySeatDistributionDTO
                                {
                                    ACSCD_SeatNos = Convert.ToInt32(dataReader["ACSCD_SeatNos"].ToString()),
                                    AMB_Id = Convert.ToInt32(dataReader["AMB_Id"].ToString()),
                                    ACQ_Id = Convert.ToInt32(dataReader["ACQ_Id"].ToString()),
                                    ACQ_QuotaName = dataReader["ACQ_QuotaName"].ToString(),
                                    ACQC_CategoryName = dataReader["ACQC_CategoryName"].ToString(),
                                    AMCO_CourseName = dataReader["AMCO_CourseName"].ToString(),
                                    AMB_BranchName = dataReader["AMB_BranchName"].ToString(),
                                    AMSE_SEMName = dataReader["AMSE_SEMName"].ToString(),
                                    count = Convert.ToInt32(dataReader["admitted_seats"].ToString())
                                });
                                data.datareport = result.ToArray();
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
