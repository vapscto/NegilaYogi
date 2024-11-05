using DataAccessMsSqlServerProvider.com.vapstech.Alumni;
using DomainModel.Model.com.vapstech.Alumni;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Service
{
    public class Alumni_Student_FriendRequestImpl:Interface.Alumni_Student_FriendRequestInterface
    {
        public AlumniContext _AlumniContext;
        public Alumni_Student_FriendRequestImpl(AlumniContext alum)
        {
            _AlumniContext = alum;
        }
        public Alumni_FriendRequestDTO getdata(Alumni_FriendRequestDTO dto)
        {
            try
            {
                var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                               from b in _AlumniContext.Alumni_User_LoginDMO
                               where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == dto.User_id)
                               select new AlumniStudentDTO
                               {
                                   ALMST_Id = Convert.ToInt64(a.ALMST_Id)
                               }).ToList();

                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniStudentRequestsearch";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = "a" });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "ALL" });
                    cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.BigInt) { Value = details[0].ALMST_Id });

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
                        dto.searchResult = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

               

            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
                            }
            return dto;
        }

        public Alumni_FriendRequestDTO getsearch_data(Alumni_FriendRequestDTO dto)
        {
            string Where = "";
            int count = dto.condition.Count;
            for (int i = 0; i < dto.field.Count; i++)
            {

                if (dto.Operator[i].ToString() == "like")
                {
                    if (dto.stuStatus != "all")
                    {
                        if (count > i)
                        {
                            if (dto.condition[i] != null)
                            {
                                if (dto.field[i].ToString() == "StudentName")
                                {
                                    Where += "  " + "(ALMST_FirstName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or  " +
                                        "ALMST_MiddleName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or " +
                                        "ALMST_LastName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' )"
                                          + " " + dto.condition[i].ToString();
                                }

                                else
                                {
                                    Where += " " + dto.field[i].ToString() + " " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%'" + " " + dto.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (dto.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(ALMST_FirstName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or  " +
                                    "ALMST_MiddleName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or " +
                                    "ALMST_LastName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' )";
                            }
                            else
                            {
                                Where += " " + dto.field[i].ToString() + " " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%'" + dto.stuStatus;
                            }
                        }
                    }
                }
                else
                {
                    if (dto.stuStatus != "all")
                    {
                        if (count > i)
                        {
                            if (dto.condition[i] != null)
                            {
                                if (dto.field[i].ToString() == "StudentName")
                                {
                                    Where += "  " + "(ALMST_FirstName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or " +
                                       "ALMST_MiddleName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or " +
                                       "ALMST_LastName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' )"
                                       + " " + dto.condition[i].ToString();
                                }
                                else
                                {
                                    Where += " " + dto.field[i].ToString() + " " + dto.Operator[i].ToString() + " '" + dto.value[i].ToString() + "'" + dto.stuStatus + "'" + dto.condition[i].ToString();
                                }
                            }
                        }
                        else
                        {
                            if (dto.field[i].ToString() == "StudentName")
                            {
                                Where += "  " + "(ALMST_FirstName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or " +
                                   "ALMST_MiddleName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' or" +
                                   "ALMST_LastName " + dto.Operator[i].ToString() + " '%" + dto.value[i].ToString() + "%' )"
                                   ;
                            }
                            else
                            {
                                Where += " " + dto.field[i].ToString() + " " + dto.Operator[i].ToString() + " '" + dto.value[i].ToString() + "'";
                            }
                        }
                    }
                }

            }
            var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                           from b in _AlumniContext.Alumni_User_LoginDMO
                           where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == dto.User_id)
                           select new AlumniStudentDTO
                           {
                               ALMST_Id = Convert.ToInt64(a.ALMST_Id)
                           }).ToList();

            List<AlumniStudentDTO> result = new List<AlumniStudentDTO>();
            using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "AlumniStudentRequestsearch";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Where", SqlDbType.VarChar) { Value = Where });
                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = dto.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Search" });
                cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.BigInt) { Value = details[0].ALMST_Id });
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
                    dto.searchResult = retObject.ToArray();
                    if (dto.searchResult.Length > 0)
                    {
                        dto.count = dto.searchResult.Length;
                    }
                    else
                    {
                        dto.count = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return dto;
        }

        public Alumni_FriendRequestDTO sendrequest (Alumni_FriendRequestDTO dto)
        {
            try
            {
                var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                               from b in _AlumniContext.Alumni_User_LoginDMO
                               where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == dto.User_id)
                               select new AlumniStudentDTO
                               {
                                   ALMST_Id = Convert.ToInt64(a.ALMST_Id)
                               }).ToList();

               

                foreach (var item in dto.requestalumni)
                {
                    Alumni_Student_FriendsDMO SF = new Alumni_Student_FriendsDMO();
                    SF.ALMST_Id = details[0].ALMST_Id;
                    SF.ALSFRND_FriendsId = item.ALMST_Id;
                    SF.ALSFRND_RequestDate = DateTime.Today;
                    SF.ALSFRND_ActiveFlg = true;
                    SF.ALSFRND_CreatedBy = dto.User_id;
                    SF.ALSFRND_CreatedDate = DateTime.Today;
                    _AlumniContext.Add(SF);
                }
               var cnt= _AlumniContext.SaveChanges();
                if(cnt>0)
                {
                    dto.message = "Success";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public Alumni_FriendRequestDTO viewprofile(Alumni_FriendRequestDTO dto)
        {
            try
            {
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniProfile";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.BigInt) { Value =dto.ALMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Profile" });

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
                        dto.almst_profile = retObject.ToArray();

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
            return dto;
        }

        //================accept request
        public Alumni_FriendRequestDTO getdata_request(Alumni_FriendRequestDTO dto)
        {
            try
            {
                var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                               from b in _AlumniContext.Alumni_User_LoginDMO
                               where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == dto.User_id)
                               select new AlumniStudentDTO
                               {
                                   ALMST_Id = Convert.ToInt64(a.ALMST_Id)
                               }).ToList();
                using (var cmd = _AlumniContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AlumniRequestList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = dto.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ALMST_Id", SqlDbType.BigInt) { Value = details[0].ALMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@Type", SqlDbType.VarChar) { Value = "Request" });

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
                        dto.friendrequestlist = retObject.ToArray();

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
            return dto;
        }

        public Alumni_FriendRequestDTO FriendrequestAccept(Alumni_FriendRequestDTO dto)
        {
            try
            {
                bool test = false;
                var aceptfalg = "";
                if (dto.Cancel=="Cancel")
                {
                     test = false;
                    aceptfalg = "REJECT";
                }
                else
                {
                     test = true;
                    aceptfalg = "ACCEPT";
                }
                var details = (from a in _AlumniContext.AlumniUserRegistrationDMO
                               from b in _AlumniContext.Alumni_User_LoginDMO
                               where (a.ALSREG_Id == b.ALSREG_Id && b.IVRMUL_Id == dto.User_id)
                               select new AlumniStudentDTO
                               {
                                   ALMST_Id = Convert.ToInt64(a.ALMST_Id)
                               }).ToList();



                foreach (var item in dto.requestalumniaccept)
                {
                    Alumni_Student_FriendRequestDMO SF = new Alumni_Student_FriendRequestDMO();
                    SF.ALMST_Id = details[0].ALMST_Id;
                    SF.ALSFRNDREQ_FriendsReqId = item.ALMST_Id;
                    SF.ALSFRNDREQ_RequestDate = item.ALSFRND_RequestDate;
                    SF.ALSFRNDREQ_AcceptedDate = DateTime.Today;
                    SF.ALSFRNDREQ_AcceptFlg = test;
                    SF.ALSFRNDREQ_ActiveFlg = true;
                    SF.ALSFRNDREQ_CreatedBy = dto.User_id;
                    SF.ALSFRNDREQ_UpdatedBy = dto.User_id;
                    SF.ALSFRNDREQ_CreatedDate = DateTime.Today;
                    SF.ALSFRNDREQ_UpdatredDate = DateTime.Today;
                    _AlumniContext.Add(SF);
                }
                foreach (var item in dto.requestalumniaccept)
                {
                    var SF1 = _AlumniContext.Alumni_Student_FriendsDMO_con.Single(a => a.ALSFRND_FriendsId == details[0].ALMST_Id && a.ALMST_Id == item.ALMST_Id);
                    
                    SF1.ALSFRND_AcceptedDate = DateTime.Today;
                    SF1.ALSFRND_AcceptFlag = aceptfalg;
                    SF1.ALSFRND_UpdatedBy = dto.User_id;
                    SF1.ALSFRND_UpdatedDate = DateTime.Today;
                   
                    _AlumniContext.Update(SF1);
                }

                var cnt = _AlumniContext.SaveChanges();
                if (cnt > 0)
                {
                    dto.message = "Success";
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
