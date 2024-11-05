using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.HRMS;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using static DomainModel.Model.com.vapstech.admission.QRCode_GenerationDMO;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class QRCodeGenerationImpl : Interfaces.QRCode_Generation_Interface
    {
        public DomainModelMsSqlServerContext _Context;
        public QRCodeGenerationImpl(DomainModelMsSqlServerContext OrganisationContext)
        {
            _Context = OrganisationContext;
        }

        public QRCode_GenerationDTO Getdetails(QRCode_GenerationDTO data)
        {

            try
            {

                data.YearList = _Context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public QRCode_GenerationDTO StaffGetdetails(QRCode_GenerationDTO data)
        {

            try
            {
                string hrmE_id = "0";

                List<long> emp = new List<long>();
                foreach (var item in data.Staffqrlistarray12)
                {
                    emp.Add(item.hrmE_id);
                }

                for (int c = 0; c < emp.Count(); c++)
                {
                    hrmE_id = hrmE_id + ',' + emp[c];
                    
                }

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Usp_Staff_QRCode_Generation_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                  //  cmd.Parameters.Add(new SqlParameter("@Mi_id",
                  //SqlDbType.BigInt)
                  //  {
                  //      Value = data.MI_Id
                  //  });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                SqlDbType.VarChar)
                    {
                        Value = hrmE_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
           SqlDbType.VarChar)
                    {
                        Value = data.Flag
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.StaffList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public QRCode_GenerationDTO SaveQR_Code(QRCode_GenerationDTO data)
        {

            try
            {
                
                if (data.qrlistarray.Length > 0)
                {

                    foreach (var qr in data.qrlistarray)
                    {
                        var duplicate = _Context.QR_Code_GenerateDMO.Where(x => x.Amst_Id == qr.Amst_Id).ToList();
                        if (duplicate.Count == 0)
                        {
                            QR_Code_GenerateDMO Qrcode = new QR_Code_GenerateDMO();
                            Qrcode.MI_Id = data.MI_Id;
                            Qrcode.Amst_Id = qr.Amst_Id;
                            Qrcode.IQRGD_CreatedBy = data.User_Id;
                            Qrcode.IQRGD_URL = data.IQRGD_URL;
                            Qrcode.IQRGD_QRCode = qr.IQRGD_QRCode;
                            Qrcode.IQRGD_CreatedDate = DateTime.Now;
                            Qrcode.IQRGD_UpdatedBy = data.User_Id;
                            Qrcode.IQRGD_UpdatedDate = DateTime.Now;
                            _Context.Add(Qrcode);
                        }
                        else
                        {
                            data.Message = "recordexist";
                        }
                    }

                }

                var res = _Context.SaveChanges();
                if (res > 0)
                {
                    data.Message = "saved";
                }
                else
                {
                    data.Message = "recordexist";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public QRCode_GenerationDTO STAFFSaveQR_Code(QRCode_GenerationDTO data)
        {
            try
            {
                if (data.Staffqrlistarray1.Length > 0)
                {
                    foreach (var qr in data.Staffqrlistarray1)
                    {
                        var duplicate = _Context.Staff_QRCode_Generation_DetailsDMO.Where(x => x.HRME_Id == qr.hrmE_id).ToList();
                        if (duplicate.Count == 0)
                        {
                            Staff_QRCode_Generation_DetailsDMO STAFFQrcode = new Staff_QRCode_Generation_DetailsDMO();
                            STAFFQrcode.MI_Id = data.MI_Id;
                            STAFFQrcode.HRME_Id = qr.hrmE_id;
                            STAFFQrcode.SQRGD_CreatedBy = data.User_Id;
                            STAFFQrcode.SQRGD_URL = data.SQRGD_URL;
                            STAFFQrcode.SQRGD_QRCode = qr.SQRGD_QRCode;
                            STAFFQrcode.SQRGD_CreatedDate = DateTime.Now;
                            STAFFQrcode.SQRGD_UpdatedBy = data.User_Id;
                            STAFFQrcode.SQRGD_UpdatedDate = DateTime.Now;
                            _Context.Add(STAFFQrcode);
                        }
                        else
                        {
                            data.Message = "recordexist";
                        }
                    }

                }

                var res = _Context.SaveChanges();
                if (res > 0)
                {
                    data.Message = "saved";
                }
                else
                {
                    data.Message = "recordexist";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;

        }
        public QRCode_GenerationDTO get_classes(QRCode_GenerationDTO data)
        {
            try
            {

                data.ClassList = (from a in _Context.AcademicYear
                                  from b in _Context.School_M_Class
                                  from c in _Context.Masterclasscategory
                                  where (a.ASMAY_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.ASMAY_Id == data.ASMAY_Id
                                  && b.ASMCL_ActiveFlag == true && a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && c.Is_Active == true)
                                  select new QRCode_GenerationDTO
                                  {
                                      ASMCL_Id = b.ASMCL_Id,
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                      ASMCL_ClassOrder = b.ASMCL_Order
                                  }).Distinct().OrderBy(a => a.ASMCL_ClassOrder).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public QRCode_GenerationDTO get_cls_sections(QRCode_GenerationDTO data)
        {
            try
            {
                data.SectionList = (
                                   from a in _Context.School_M_Section

                                   from c in _Context.School_M_Class
                                   where (c.ASMCL_Id == data.ASMCL_Id &&
                                   a.ASMC_ActiveFlag == 1 && c.ASMCL_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                   select new QRCode_GenerationDTO
                                   {
                                       ASMS_Id = a.ASMS_Id,
                                       ASMC_SectionName = a.ASMC_SectionName,
                                       ASMC_SectionOrder = a.ASMC_Order
                                   }).Distinct().OrderBy(a => a.ASMC_SectionOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public QRCode_GenerationDTO GetStudents(QRCode_GenerationDTO data)
        {
            try
            {
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "QRCode_Studentlist";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id

                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                  SqlDbType.VarChar)
                    {

                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                 SqlDbType.VarChar)
                    {

                        Value = data.Flag
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.StudentQRlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }            
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public QRCode_GenerationDTO QRReportDetails(QRCode_GenerationDTO data)
        {
            try
            {

                string hrmE_id = "0";

                List<long> emp = new List<long>();
                foreach (var item in data.Staffqrlistarray12)
                {
                    emp.Add(item.hrmE_id);
                }

                for (int c = 0; c < emp.Count(); c++)
                {
                    hrmE_id = hrmE_id + ',' + emp[c];

                }

                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Usp_Staff_QRCode_Generation_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                  //  cmd.Parameters.Add(new SqlParameter("@MI_ID",
                  //SqlDbType.BigInt)
                  //  {
                  //      Value = data.MI_Id
                  //  });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.Flag

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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.StaffReportList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }





        public QRCode_GenerationDTO getBasicData(QRCode_GenerationDTO dto)
        {
            dto = GetAllDropdownAndDatatableDetails(dto);
            return dto;
        }
        public QRCode_GenerationDTO GetAllDropdownAndDatatableDetails(QRCode_GenerationDTO dto)
        {
            List<HR_Employee_Salary> SalaryCalculation = new List<HR_Employee_Salary>();
            //   List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_LeaveYearDMO> leaveyear = new List<HR_Master_LeaveYearDMO>();
            List<HR_Master_EmployeeType> EmployeeTypelist = new List<HR_Master_EmployeeType>();
            List<HR_Master_GroupType> GroupTypelist = new List<HR_Master_GroupType>();
            List<HR_Master_Department> Departmentlist = new List<HR_Master_Department>();
            List<HR_Master_Designation> Designationlist = new List<HR_Master_Designation>();
            List<HR_Master_CourseDMO> Courselist = new List<HR_Master_CourseDMO>();
            List<HR_Master_Grade> Gradelist = new List<HR_Master_Grade>();
            List<HR_PROCESSDMO> PROCESSList = new List<HR_PROCESSDMO>();
            List<MasterEmployee> emp = new List<MasterEmployee>();
            try
            {
                PROCESSList = (from ao in _Context.HR_Process_Auth_OrderNoDMO
                               from pa in _Context.HR_PROCESSDMO
                               from cc in _Context.Staff_User_Login
                               where (pa.HRPA_Id == ao.HRPA_Id && ao.IVRMUL_Id == cc.IVRMSTAUL_Id && cc.Id == dto.LogInUserId)


                               select pa
                            ).ToList();

                if (PROCESSList.Count() > 0)
                {
                    List<long> groupTypeIdList = PROCESSList.Select(t => t.HRMGT_Id).Distinct().ToList();
                    List<long> hrmD_IdList = PROCESSList.Select(t => t.HRMD_Id).Distinct().ToList();
                    List<long> hrmdeS_IdList = PROCESSList.Select(t => t.HRMDES_Id).Distinct().ToList();

                    emp = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id))).OrderBy(t => t.HRME_EmployeeOrder).ToList();
                    dto.employeedropdown = emp.ToArray();

                    //GroupTypelist
                    GroupTypelist = _Context.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && groupTypeIdList.Contains(Convert.ToInt64(t.HRMGT_Id)) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _Context.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmD_IdList.Contains(Convert.ToInt64(t.HRMD_Id)) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _Context.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && hrmdeS_IdList.Contains(Convert.ToInt64(t.HRMDES_Id)) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();

                }
                else
                {
                    dto.employeedropdown = _Context.HR_Master_Employee_DMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRME_ActiveFlag == true).OrderBy(t => t.HRME_EmployeeOrder).ToArray();

                    //GroupTypelist
                    GroupTypelist = _Context.HR_Master_GroupType.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMGT_ActiveFlag == true).OrderBy(t => t.HRMGT_Order).ToList();
                    dto.groupTypedropdown = GroupTypelist.ToArray();

                    //Departmentlist
                    Departmentlist = _Context.HR_Master_Department.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMD_ActiveFlag == true).OrderBy(t => t.HRMD_Order).ToList();
                    dto.departmentdropdown = Departmentlist.ToArray();

                    //Designationlist
                    Designationlist = _Context.HR_Master_Designation.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMDES_ActiveFlag == true).OrderBy(t => t.HRMDES_Order).ToList();
                    dto.designationdropdown = Designationlist.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dto;
        }

        public QRCode_GenerationDTO get_depts(QRCode_GenerationDTO data)
        {
            try
            {
                data.departmentdropdown = (from a in _Context.HRGroupDeptDessgDMO
                                           from b in _Context.HR_Master_Department
                                           where (a.MI_Id == data.MI_Id && a.HRMD_Id == b.HRMD_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMD_ActiveFlag == true)
                                           select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public QRCode_GenerationDTO get_desig(QRCode_GenerationDTO data)
        {
            try
            {
                data.designationdropdown = (from a in _Context.HRGroupDeptDessgDMO
                                            from b in _Context.HR_Master_Designation
                                            where (a.MI_Id == data.MI_Id && a.HRMDES_Id == b.HRMDES_Id && data.hrmgT_IdList.Contains(a.HRMGT_Id) && data.hrmD_IdList.Contains(a.HRMD_Id) && b.MI_Id.Equals(data.MI_Id) && b.HRMDES_ActiveFlag == true)
                                            select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public QRCode_GenerationDTO FilterEmployeedetailsBySelection(QRCode_GenerationDTO dto)
        {
            List<MasterEmployee> employe = new List<MasterEmployee>();
            try
            {
                if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();

                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() > 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmdeS_IdList.Contains(t.HRMDES_Id) && t.HRME_ActiveFlag == true).ToList();
                }
                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() > 0 && dto.hrmgT_IdList.Count() == 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmD_IdList.Contains(t.HRMD_Id) && t.HRME_ActiveFlag == true).ToList();
                }

                else if (dto.hrmdeS_IdList.Count() == 0 && dto.hrmD_IdList.Count() == 0 && dto.hrmgT_IdList.Count() > 0)
                {
                    //employee
                    employe = _Context.MasterEmployee.Where(t => t.MI_Id.Equals(dto.MI_Id) && dto.hrmgT_IdList.Contains(t.HRMGT_Id) && t.HRME_ActiveFlag == true).ToList();
                }

                dto.employeedropdown = employe.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public string GetEmployeeMobileNumbers(long empId)
        {
            string mobileNumbers = "";

            List<long> temparr = new List<long>();
            //getting all mobilenumbers
            try
            {
                var Phone_Noresult = _Context.Emp_MobileNo.Where(t => t.HRME_Id == empId).ToList();
                foreach (Multiple_Mobile_DMO ph1 in Phone_Noresult)
                {
                    temparr.Add(ph1.HRMEMNO_MobileNo);
                }
                string combindedString =

                mobileNumbers = string.Join(", ", temparr.ToArray());
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee);
            }



            return mobileNumbers;
        }
        public string GetEmployeeEmailIds(long empId)
        {
            string EmailIds = "";
            List<string> temparr = new List<string>();
            try
            {
                var Email_Idresult = _Context.Emp_Email_Id.Where(t => t.HRME_Id == empId).ToList();
                foreach (Multiple_Email_DMO ph1 in Email_Idresult)
                {
                    temparr.Add(ph1.HRMEM_EmailId);
                }
                EmailIds = string.Join(", ", temparr.ToArray());
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee);
            }

            return EmailIds;
        }


        public QRCode_GenerationDTO QRcodegeneration(QRCode_GenerationDTO data)
        {
            try
            {
                if (data.Qrcodegeneration.Length > 0)
                {
                    string valueqr = "";

                    for (var i = 0; i < data.Qrcodegeneration.Length; ++i)
                    {
                        valueqr = data.Qrcodegeneration[i].hrmE_id.ToString();

                        Bitmap bitmap = new Bitmap(100, 100);


                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                        QRCode QrCode = new QRCode(QrCodeInfo);
                        Bitmap QrBitmap = QrCode.GetGraphic(60);

                        byte[] byteArray = BitmapToByteArray(QrBitmap);
                        string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));

                       // byte[] bytes = Convert.FromBase64String( QrUri);

                        //using (MemoryStream ms = new MemoryStream(bytes))
                        //{
                        //    Image image = Image.FromStream(ms);
                        //    image.Save("output.jpg", ImageFormat.Jpeg);
                        //}

                        var duplicate = _Context.Staff_QRCode_Generation_DetailsDMO.Where(x => x.HRME_Id == Convert.ToInt64 (valueqr)).ToList();
                        if (duplicate.Count == 0)
                        {
                            Staff_QRCode_Generation_DetailsDMO STAFFQrcode = new Staff_QRCode_Generation_DetailsDMO();
                            STAFFQrcode.MI_Id = data.MI_Id;
                            STAFFQrcode.HRME_Id = Convert.ToInt64( valueqr);
                            STAFFQrcode.SQRGD_CreatedBy = data.User_Id;
                            STAFFQrcode.SQRGD_URL = data.SQRGD_URL;
                            STAFFQrcode.SQRGD_QRCode = QrUri;
                            STAFFQrcode.SQRGD_CreatedDate = DateTime.Now;
                            STAFFQrcode.SQRGD_UpdatedBy = data.User_Id;
                            STAFFQrcode.SQRGD_UpdatedDate = DateTime.Now;
                            _Context.Add(STAFFQrcode);
                        }
                        else
                        {
                            data.Message = "recordexist";
                        }

                    }
                }

                var res = _Context.SaveChanges();
                if (res > 0)
                {
                    data.Message = "saved";
                }
                else
                {
                    data.Message = "recordexist";
                }
                try
                {
                    string hrmE_id = "0";

                    List<long> emp = new List<long>();
                    foreach (var item in data.Qrcodegeneration)
                    {
                        emp.Add(item.hrmE_id);
                    }

                    for (int c = 0; c < emp.Count(); c++)
                    {
                        hrmE_id = hrmE_id + ',' + emp[c];

                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Usp_Staff_QRCode_Generation_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                      //  cmd.Parameters.Add(new SqlParameter("@Mi_id",
                      //SqlDbType.BigInt)
                      //  {
                      //      Value = data.MI_Id
                      //  });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                        {
                            Value = hrmE_id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FLAG",
               SqlDbType.VarChar)
                        {
                            Value = "StaffDetailsQR"
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.StaffList = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;

        }

        public QRCode_GenerationDTO StudentQRCode(QRCode_GenerationDTO data)
        {
            try
            {
                if (data.qrlistarray12.Length > 0)
                {
                    string valueqr = "";

                    for (var i = 0; i < data.qrlistarray12.Length; ++i)
                    {
                        valueqr = data.qrlistarray12[i].Amst_Id.ToString();

                        Bitmap bitmap = new Bitmap(100, 100);


                        QRCodeGenerator QrGenerator = new QRCodeGenerator();
                        QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(valueqr, QRCodeGenerator.ECCLevel.Q);
                        QRCode QrCode = new QRCode(QrCodeInfo);
                        Bitmap QrBitmap = QrCode.GetGraphic(60);

                        byte[] byteArray = BitmapToByteArray(QrBitmap);
                        string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(byteArray));

                        // byte[] bytes = Convert.FromBase64String( QrUri);

                        //using (MemoryStream ms = new MemoryStream(bytes))
                        //{
                        //    Image image = Image.FromStream(ms);
                        //    image.Save("output.jpg", ImageFormat.Jpeg);
                        //}

                        var duplicate = _Context.QR_Code_GenerateDMO.Where(x => x.Amst_Id == data.qrlistarray12[i].Amst_Id).ToList();
                        if (duplicate.Count == 0)
                        {
                            QR_Code_GenerateDMO Qrcode = new QR_Code_GenerateDMO();
                            Qrcode.MI_Id = data.MI_Id;
                            Qrcode.Amst_Id = data.qrlistarray12[i].Amst_Id;
                            Qrcode.IQRGD_CreatedBy = data.User_Id;
                            Qrcode.IQRGD_URL = data.IQRGD_URL;
                            Qrcode.IQRGD_QRCode = QrUri;
                            Qrcode.IQRGD_CreatedDate = DateTime.Now;
                            Qrcode.IQRGD_UpdatedBy = data.User_Id;
                            Qrcode.IQRGD_UpdatedDate = DateTime.Now;
                            _Context.Add(Qrcode);
                        }
                        else
                        {
                            data.Message = "recordexist";
                        }

                    }
                }

                var res = _Context.SaveChanges();
                if (res > 0)
                {
                    data.Message = "saved";
                }
                else
                {
                    data.Message = "recordexist";
                }
                try
                {
                    string Amst_id = "0";

                    List<long> stu = new List<long>();
                    foreach (var item in data.qrlistarray12)
                    {
                        stu.Add(item.Amst_Id);
                    }

                    for (int c = 0; c < stu.Count(); c++)
                    {
                        Amst_id = Amst_id + ',' + stu[c];

                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "QRCode_Studentlist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id

                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.VarChar)
                        {

                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FLAG",
                     SqlDbType.VarChar)
                        {

                            Value = data.Flag
                        });
                    //    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    //SqlDbType.VarChar)
                    //    {

                    //        Value = Amst_id
                    //    });

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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.StudentQRlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;

        }



        public static byte[] BitmapToByteArray(Bitmap QrBitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                QrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }





    }
}
