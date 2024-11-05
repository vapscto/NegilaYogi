using PreadmissionDTOs.com.vaps.Transport;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace TransportServiceHub.Services
{
    public class CLGBusRoutesDetailsImpl:Interfaces.CLGBusRoutesDetailsInterface
    {
        public TransportContext _context;
        public CLGBusRoutesDetailsImpl(TransportContext o)
        {
            _context = o;
        }
public CLGBusRoutesDetailsDTO loaddata(CLGBusRoutesDetailsDTO data)
        {
            try
            {
                data.yearlist = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.courselist = _context.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).Distinct().OrderByDescending(t => t.AMCO_Order).ToArray();

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGBusRoutesDetailsDTO getbranch(CLGBusRoutesDetailsDTO data)
        {
            try
            {

                data.branchlist = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                   from b in _context.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _context.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGBusRoutesDetailsDTO getsemester(CLGBusRoutesDetailsDTO data)
        {
            try
            {
                data.semesterlist = (from a in _context.CLG_Adm_Master_SemesterDMO
                                     from b in _context.CLG_Adm_College_AY_CourseDMO
                                     from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                     from d in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                     select a).Distinct().ToArray();
                                     //select new CLGBusRoutesDetailsDTO
                                     //{
                                     //    AMSE_Id = a.AMSE_Id,
                                     //    AMSE_SEMName = a.AMSE_SEMName,
                                     //    AMSE_SEMOrder = a.AMSE_SEMOrder,
                                     //}).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
   

        public CLGBusRoutesDetailsDTO getreport(CLGBusRoutesDetailsDTO data)
        {
            try
            {
                if (data.type == "stdcount")
                {


                    data.semesterlist = (from a in _context.CLG_Adm_Master_SemesterDMO
                                         from b in _context.CLG_Adm_College_AY_CourseDMO
                                         from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                         from d in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                         where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == data.AMB_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                         select a).Distinct().ToArray();
                                         //select new CLGBusRoutesDetailsDTO
                                         //{
                                         //    AMSE_Id = a.AMSE_Id,
                                         //    AMSE_SEMName = a.AMSE_SEMName,
                                         //    AMSE_SEMOrder = a.AMSE_SEMOrder,
                                         //}).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();



                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "[Transport_Route_SemWise_Count_Report_new_Collage]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.AMCO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.AMB_Id
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
                            data.griddata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if(data.type== "stdstddetails")
                {
                    List<long> secid1 = new List<long>();
            
                    foreach (var item in data.secidlist)
                    {
                        secid1.Add(item.AMSE_Id);

                    }
                    string secidss = "0";
                    for (int y = 0; y < secid1.Count(); y++)
                    {
                        secidss = secidss + ',' + secid1[y];
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "[Transport_Route_semwise_Count_Report_Details_Collage]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                         SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.AMCO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.AMB_Id
                        });
                      
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                        SqlDbType.VarChar)
                        {
                            Value = secidss
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
                            data.studentgriddata = retObject.ToArray();
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

    }
}
