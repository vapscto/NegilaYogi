using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class CLGTTSubjectWiseReportImpl : Interfaces.CLGTTSubjectWiseReportInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public CLGTTSubjectWiseReportImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

       

        public CLGTTSubjectWiseReportDTO getdetails(CLGTTSubjectWiseReportDTO data)
        {
           
            try
            {
                data.year = _ttcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(p => p.ASMAY_Year).ToArray();
                data.courselist = _ttcontext.MasterCourseDMO.Where(t =>t.MI_Id==data.MI_Id&&t.AMCO_ActiveFlag == true).ToArray();
                //data.branchlist = _ttcontext.ClgMasterBranchDMO.Where(t =>t.MI_Id==data.MI_Id&& t.AMB_ActiveFlag == true).ToArray();
                data.sectionlist = _ttcontext.Adm_College_Master_SectionDMO.Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();

                data.subjectlist = _ttcontext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == data.MI_Id && t.ISMS_ActiveFlag == 1).OrderByDescending(t => t.ISMS_SubjectName).ToArray();
                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id == data.MI_Id).ToList().ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMD_ActiveFlag == true).ToArray();

                //List<IVRM_School_Master_SubjectsDMO> sub = new List<IVRM_School_Master_SubjectsDMO>();
                //sub = _ttcontext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == id && t.ISMS_ActiveFlag == 1).ToList();
                //TTMB.subdrp = sub.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public CLGTTSubjectWiseReportDTO getbranch(CLGTTSubjectWiseReportDTO data)
        {
            try
            { 
             List<long> amcoids = new List<long>();
            foreach (var item in data.coursels)
                       {
                           amcoids.Add(item.AMCO_Id);
                       }
                data.branchlist=(from a in _ttcontext.MasterCourseDMO
                                from u in _ttcontext.ClgMasterBranchDMO
                                from c in _ttcontext.CLG_Adm_College_AY_Course_BranchDMO
                                from b in _ttcontext.CLG_Adm_College_AY_CourseDMO
                                where(a.MI_Id==b.MI_Id&&a.MI_Id==u.MI_Id&&a.AMCO_Id==b.AMCO_Id&&b.ACAYC_Id==c.ACAYC_Id&&c.AMB_Id==u.AMB_Id&&b.MI_Id==data.MI_Id 
                              &&data.ASMAY_Id== b.ASMAY_Id&& amcoids.Contains(b.AMCO_Id)) select u).Distinct().OrderBy(t => t.AMB_Order).ToArray();             
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
   public CLGTTSubjectWiseReportDTO getsemister(CLGTTSubjectWiseReportDTO data)
        {
            try
            {
                List<long> amcoids = new List<long>();
                foreach(var item in data.coursels)
                {
                    amcoids.Add(item.AMCO_Id);
                }
                List<long> ambrids = new List<long>();
                foreach(var item in data.branchls)
                {
                    ambrids.Add(item.AMSE_Id);
                }


         //           CLG_Adm_Master_SemesterDMO
         //CLG_Adm_College_AY_Course_Branch_SemesterDMO 


        data.semisterlist=(from a in _ttcontext.MasterCourseDMO
                                               from u in _ttcontext.ClgMasterBranchDMO
                                               from c in _ttcontext.CLG_Adm_College_AY_Course_BranchDMO
                                               from b in _ttcontext.CLG_Adm_College_AY_CourseDMO
                                               from e in _ttcontext.CLG_Adm_Master_SemesterDMO
                                               from f in _ttcontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                           where (a.MI_Id == b.MI_Id && a.MI_Id == u.MI_Id && a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == u.AMB_Id && b.MI_Id == data.MI_Id&& data.ASMAY_Id == b.ASMAY_Id && amcoids.Contains(b.AMCO_Id)/*&& ambrids.Contains(u.AMB_Id)*/ && e.MI_Id==data.MI_Id&&e.MI_Id==f.MI_Id&&e.AMSE_Id==f.AMSE_Id&&c.ACAYCB_Id==f.ACAYCB_Id)
                                               select e).Distinct().OrderBy(t => t.AMSE_SEMOrder).ToArray();   
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGTTSubjectWiseReportDTO savedetail(CLGTTSubjectWiseReportDTO data)
        {

                try
                {

                    string courseids = "0";
                    for (int d = 0; d < data.classarray.Count(); d++)
                    {
                    courseids = courseids + ',' + data.classarray[d].AMCO_Id;
                    }
                string brids = "0";
                    for(int y = 0; y < data.brharray.Count(); y++)
                {
                    brids = brids + ',' + data.brharray[y].AMB_Id;
                }

                string semids = "0";
                for(int s = 0; s < data.semarray.Count(); s++)
                {
                    semids = semids + ',' + data.semarray[s].AMSE_Id;
                }

                string secids = "0";
                for(int j = 0; j < data.secarray.Count(); j++)
                {
                    secids = secids + ',' + data.secarray[j].ACMS_Id;
                }
                string subjids = "0";
                for(int l = 0; l < data.subarray.Count(); l++)
                {
                    subjids = subjids + ',' + data.subarray[l].ISMS_Id;
                }

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_GET_SUBJECTWISE_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });


                        cmd.Parameters.Add(new SqlParameter("@AMCO_Ids",
                        SqlDbType.NVarChar)
                        {
                            Value = courseids
                        });

                    cmd.Parameters.Add(new SqlParameter("@AMB_Ids",
                   SqlDbType.NVarChar)
                    {
                        Value = brids
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMSE_Ids",
                      SqlDbType.NVarChar)
                    {
                        Value = semids
                    });



                    cmd.Parameters.Add(new SqlParameter("@ACMS_Ids",
                    SqlDbType.NVarChar)
                    {
                        Value = secids
                    });


                    cmd.Parameters.Add(new SqlParameter("@ISMSIDs",
                   SqlDbType.NVarChar)
                    {
                        Value = subjids
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
                            data.getreportdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
