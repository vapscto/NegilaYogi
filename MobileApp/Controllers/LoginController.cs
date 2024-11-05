using Microsoft.AspNetCore.Mvc;
using MobileApp.Delegates;
using PreadmissionDTOs;
using Newtonsoft.Json;
using DomainModel.Model;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.mobile;
using PreadmissionDTOs.com.vaps.Fees;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MobileApp.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        Logindelegate pre = new Logindelegate();

        //[Route("MIdata")]
        //public JObject MIdata([FromBody] regis reg)
        //{
        //    LoginDTO lgnDto = pre.getMIdata(reg);
        //    LoginMDTO lgn = new LoginMDTO();
        //    if (lgnDto.MIdata != null)
        //    {
        //        if (lgnDto.MIdata.Length > 0)
        //        {
        //            Institution temp = (Institution)lgnDto.MIdata.GetValue(0);
        //            lgn.MI_Id = temp.MI_Id;
        //            lgn.MI_Name = temp.MI_Name;
        //        }
        //    }
        //    lgn.message = lgnDto.message;
        //    var myContent = JsonConvert.SerializeObject(lgn);
        //    JObject json = JObject.Parse(myContent);
        //    return json;
        //}

        public JObject Post([FromBody] regis reg)
        {
            LoginDTO lgnDto = pre.getMIdata(reg);
            LoginMDTO dt = new LoginMDTO();
            if (lgnDto.MIdata != null)
            {
                if (lgnDto.MIdata.Length > 0)
                {
                    Institution temp = (Institution)lgnDto.MIdata.GetValue(0);
                    dt.MI_Id = temp.MI_Id;
                    dt.MI_Name = temp.MI_Name;
                    reg.MI_Id = dt.MI_Id;
                    LoginDTO lgnDto1 = pre.getregdata(reg);
                    dt.AMST_Id = lgnDto1.AMST_Id;
                    dt.ASMAY_Id = lgnDto1.ASMAY_Id;
                    dt.ASMAY_Year = lgnDto1.ASMAY_Year;

                    dt.userId = Convert.ToInt32(lgnDto1.userId);
                    dt.message = lgnDto1.message;
                    dt.roleforlogin = lgnDto1.roleforlogin;
                    dt.empcode = lgnDto1.empcode;
                    

                }
                else
                {
                    dt.message = lgnDto.message;
                }
            }
            else
            {
                dt.message = lgnDto.message;
            }
            // dt.message = lgnDto.message;
            var myContent = JsonConvert.SerializeObject(dt);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        //public JObject Post([FromBody] regis reg)
        //{
        //    LoginDTO lgnDto = pre.getregdata(reg);
        //    LoginMDTO dt = new LoginMDTO();
        //    dt.AMST_Id = lgnDto.AMST_Id;
        //    dt.ASMAY_Id = lgnDto.ASMAY_Id;
        //    dt.ASMAY_Year = lgnDto.ASMAY_Year;
        //    dt.message = lgnDto.message;
        //    var myContent = JsonConvert.SerializeObject(dt);
        //    JObject json = JObject.Parse(myContent);
        //    return json;
        //}
        [Route("StudentDetails")]
        public JObject GetStudentDetails([FromBody] StudentdetDTO.input data)
        {
            StudentdetDTO lgnDto = pre.getstudetails(data);
            //StudentdetDTO lgn = new StudentdetDTO();
            //if(lgnDto.stulist !=null)
            //{
            //    if (lgnDto.stulist.Length > 0)
            //    {
            //         lgn = (StudentdetDTO)lgnDto.stulist.GetValue(0);                  
            //    }
            //}            
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("StudentAttendance")]
        public JObject getAttendetails([FromBody] StudentAttendanceDTO.input data)
        {
            StudentAttendanceDTO lgnDto = pre.stuattend(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("StudentYearlyAttendance")]
        public JObject getYAttendetails([FromBody] StudentYAttendanceDTO.input data)
        {
            StudentYAttendanceDTO lgnDto = pre.stuYattend(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("StudentFeeDetails")]
        public JObject Feedetails([FromBody] StudentFeeDetailsDTO.input data)
        {
            StudentFeeDetailsDTO lgnDto = pre.stufeedetails(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("StudentFeeTerm")]
        public JObject StudentFeeTerm([FromBody] StudentFeeTermDTO.input data)
        {
            StudentFeeTermDTO lgnDto = pre.StudentFeeTerm(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("CalenderofEvents")]
        public JObject CalenderofEvents([FromBody] COEDTO.input data)
        {
            COEDTO lgnDto = pre.CalenderofEvents(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("Examid")]
        public JObject Examid([FromBody] ExamDTO.input data)
        {

            ExamDTO.examid lgnDto = pre.Examid(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("ExamMarks")]
        public JObject Examdetails([FromBody] ExamDTO.input data)
        {
            ExamDTO lgnDto = pre.Examdetails(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("OnlinePayment")]
        public JObject OnlinePayment([FromBody] OnlinePaymentDTO.input data)
        {
            PaymentDetails lgnDto = pre.generatehashsequence(data);
            //  OnlinePaymentDTO pgmodule = Mapper.Map<OnlinePaymentDTO>(lgnDto);

            OnlinePaymentDTO pgmodule = new OnlinePaymentDTO();
            pgmodule.MARCHANT_ID = lgnDto.MARCHANT_ID;
            pgmodule.trans_id = lgnDto.trans_id;
            pgmodule.Seq = lgnDto.Seq;
            pgmodule.amount = lgnDto.amount.ToString();
            pgmodule.productinfo = lgnDto.productinfo;
            pgmodule.firstname = lgnDto.firstname;
            pgmodule.email = lgnDto.email;
            pgmodule.phone = lgnDto.phone;
            pgmodule.udf1 = lgnDto.udf1;
            pgmodule.udf2 = lgnDto.udf2;
            pgmodule.udf3 = lgnDto.udf3;
            pgmodule.udf4 = lgnDto.udf4;
            pgmodule.udf5 = lgnDto.udf5;
            pgmodule.udf6 = lgnDto.udf6;
            pgmodule.SaltKey = lgnDto.SaltKey;
            pgmodule.payu_URL = lgnDto.payu_URL;
            pgmodule.transaction_response_url = lgnDto.transaction_response_url;
            pgmodule.status = lgnDto.status;
            pgmodule.service_provider = lgnDto.service_provider;
            pgmodule.productinfoObj = lgnDto.productinfoObj;
            //pgmodule.productinfoObj = JsonConvert.DeserializeObject<FeeSlplitOnlinePayment>(lgnDto.productinfo, new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Objects
            //});
            //pgmodule.PaymentDetailsList = lgnDto.PaymentDetailsList;
            if(lgnDto.PaymentDetailsList !=null)
            {
                if (lgnDto.PaymentDetailsList.Length > 0)
                {
                    PaymentDetails lgnDtonew = (PaymentDetails)lgnDto.PaymentDetailsList.GetValue(0);
                    pgmodule.hash = lgnDtonew.hash;
                }
            }
           
            



            var myContent = JsonConvert.SerializeObject(pgmodule);
            JObject json = JObject.Parse(myContent);
            return json;
        }

        [Route("Timetable")]
        public JObject Timetable([FromBody] TTDTO.input data)
        {
            TTDTO lgnDto = pre.Timetable(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }

        [Route("EmployeePortal_SalaryD")]
        public JObject EmployeePortal_SalaryD([FromBody] EmpPortalDTO.Input data)
        {
            EmpPortalDTO lgnDto = pre.EmployeePortal_SalaryD(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("EmployeePortal_PunchD")]
        public JObject EmployeePortal_PunchD([FromBody] EmployeePunchDTO.Input data)
        {
            EmployeePunchDTO lgnDto = pre.EmployeePortal_PunchD(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("EmployeePortal_StudentAttrndence")]
        public JObject EmployeePortal_StudentAttrndence([FromBody] EmployeePortal_StudentAttrndenceDTO.Input data)
        {
            EmployeePortal_StudentAttrndenceDTO lgnDto = pre.EmployeePortal_StudentAttrndence(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("EmployeePortalTimeTableD")]
        public JObject EmployeePortalTimeTableD([FromBody] EmployeePortalTimeTableDTO.Input data)
        {
            EmployeePortalTimeTableDTO lgnDto = pre.EmployeePortalTimeTableD(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }

        [Route("EmployeePortalStudentReportCard")]
        public JObject EmployeePortalStudentReportCard([FromBody] EmployeePortalStudentReportCardDTO.Input data)
        {
            EmployeePortalStudentReportCardDTO lgnDto = pre.EmployeePortalStudentReportCard(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }

        [Route("EmployeePortalStudentSearchD")]
        public JObject EmployeePortalStudentSearchD([FromBody] EmployeePortalStudentSearchDTO.Input data)
        {
            EmployeePortalStudentSearchDTO lgnDto = pre.EmployeePortalStudentSearchD(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("EmployeePortalLeaveD")]
        public JObject EmployeePortalLeaveD([FromBody] EmployeePortalLeaveDTO.Input data)
        {
            EmployeePortalLeaveDTO lgnDto = pre.EmployeePortalLeaveD(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
        [Route("EmployeePortalDetails")]
        public JObject EmployeePortalDetails([FromBody] EmployeeDTO.input data)
        {
            EmployeeDTO lgnDto = pre.EmployeePortalDetails(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }

        [Route("EmployeeDetails")]
        public JObject EmployeeDetails([FromBody] EmployeeloginDTO.input data)
        {
            EmployeeloginDTO lgnDto = pre.EmployeeDetails(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }


        [Route("EmployeesalaryDetails")]
        public JObject EmployeesalaryDetails([FromBody] EmployeeSalaryDTO.input data)
        {
            EmployeeSalaryDTO lgnDto = pre.EmployeesalaryDetails(data);
            var myContent = JsonConvert.SerializeObject(lgnDto);
            JObject json = JObject.Parse(myContent);
            return json;
        }
    }
}
