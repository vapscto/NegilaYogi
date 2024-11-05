using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;

namespace corewebapi18072016.Controllers.com.vapstech.AuthServer
{
    public class GlobalInterceptor: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Controller controller = filterContext.Controller as Controller;
            var descriptor = filterContext.HttpContext.Request.Path.ToString();
            if (!descriptor.Contains("setvirtauldeatilsnew") && !descriptor.Contains("clearsession") && !descriptor.Contains("getMIdata") && !descriptor.Contains("connect") && !descriptor.Contains("getrolewisepage") && !descriptor.Contains("VerifyUserName") && !descriptor.Contains("getOTPForMobile") && !descriptor.Contains("VerifymobileOtp") && !descriptor.Contains("sendforgotpassword") && !descriptor.Contains("getsubdomain") && !descriptor.Contains("ForgotOTPForEmail") && !descriptor.Contains("ForgotEmailstudent") && !descriptor.Contains("Regis") && !descriptor.Contains("paymentresponse") && !descriptor.Contains("ImageUpload") && !descriptor.Contains("VerifyEmailOtp") && !descriptor.Contains("getCourse") && !descriptor.Contains("getBranch") && !descriptor.Contains("getSemester") && !descriptor.Contains("getgateway") && !descriptor.Contains("GetpaymentDetails")  && !descriptor.Contains("paymentsave") && !descriptor.Contains("/api/Changepswd/saveexppassword"))
            {
                if (controller != null)
                {
                    //if (filterContext.HttpContext.Session.GetInt32("Session_MI_Id") == null || filterContext.HttpContext.Session.GetInt32("Session_MI_Id") == 0)
                    //{
                    //    var data = new
                    //    {
                    //        isSessionExpired = true,
                    //    };
                    //    filterContext.Result = new JsonResult(data);
                    //}

                    if (filterContext.HttpContext.Session.GetInt32("UserId") == null || filterContext.HttpContext.Session.GetInt32("UserId") == 0)
                    {
                        var data = new
                        {
                            isSessionExpired = true,
                        };
                        filterContext.Result = new JsonResult(data);
                    }
                }
            }

            //if (filterContext.HttpContext.Session.GetInt32("Session_MI_Id") != null)
            //{
            //    string usernme = filterContext.HttpContext.Session.GetString("UserName");
            //    var details = new
            //    {
            //        multiplewindowdetails = "Window",
            //        username = usernme,
            //    };
            //    filterContext.Result = new JsonResult(details);
            //}

            base.OnActionExecuting(filterContext);
        }
    }
}