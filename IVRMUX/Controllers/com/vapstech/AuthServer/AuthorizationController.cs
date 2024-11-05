/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See http://github.com/openiddict/openiddict-core for more information concerning
 * the license and the contributors participating to this project.
 */

using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Core;
using OpenIddict.Models;
using DomainModel.Model;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace com.vapstech.AuthServer
{
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        Logindelegate pre = new Logindelegate();

        public AuthorizationController(
            OpenIddictApplicationManager<OpenIddictApplication> applicationManager,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _applicationManager = applicationManager;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("connect/token"), Produces("application/json")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            //Removing Session value when user logs in.
            //Added on 05-03-2018
            HttpContext.Session.Remove("Session_Trip_IsOnline");
            HttpContext.Session.Remove("Session_Trip_MI_Id");
            HttpContext.Session.Remove("Session_Trip_ASMAY_Id");

            Debug.Assert(request.IsTokenRequest(),
                "The OpenIddict binder for ASP.NET Core MVC is not registered. " +
                "Make sure services.AddOpenIddict().AddMvcBinders() is correctly called.");

            regis loginData = new regis();
            ApplicationUser userTemp = new ApplicationUser();

            OpenIdConnectParameter temp = request.GetParameter("ClientId").Value.ToString();
            OpenIdConnectParameter Logintype = request.GetParameter("Logintype").Value.ToString();
            loginData.username = request.Username;
            loginData.password = request.Password;
            loginData.MI_Id = Convert.ToInt32(temp.Value);
            loginData.Logintype = Convert.ToString(Logintype.Value);

            //LoginDTO LoginDTO = new LoginDTO();
            //LoginController LoginController = new LoginController();
            //LoginDTO = LoginController.Getdata(loginData);
            string subdomainname = "";
            //if (HttpContext.Session.GetString("session_subdomainname") != null || HttpContext.Session.GetString("session_subdomainname") != "")
            //{
            //    subdomainname = Convert.ToString(HttpContext.Session.GetString("session_subdomainname"));
            //}

            int virtualid = 0;

            if (Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid")) != 0)
            {
                virtualid = Convert.ToInt32(HttpContext.Session.GetInt32("session_virtualid"));
            }

            //virtualid = 4;
            //if (virtualid == 0)
            //{
            //    return BadRequest(new OpenIdConnectResponse
            //    {
            //        Error = OpenIdConnectConstants.Errors.InvalidGrant,

            //        ErrorDescription = "Kindly reload the application with Some virtual ID"
            //        // ErrorDescription = userdata.message
            //    });
            //}


            if (loginData.MI_Id == 0)
            {
                if (virtualid != 0)
                {
                    CommonDTO cmn = pre.setSessionDataForVirtualSchool(virtualid);
                    HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                    HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                }
                else if (subdomainname != "" && subdomainname != null)
                {
                    CommonDTO cmn = new CommonDTO();
                    cmn.subDomainName = subdomainname;
                    cmn = pre.setSessionDataForVirtualSchoolsubdomain(cmn);
                    HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                    HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                }
                else if (subdomainname != "" && virtualid != 0 && subdomainname != null)
                {
                    CommonDTO cmn = new CommonDTO();
                    cmn.subDomainName = subdomainname;
                    cmn = pre.setSessionDataForVirtualSchoolsubdomain(cmn);
                    HttpContext.Session.SetInt32("Session_MO_Id", cmn.IVRM_MO_Id);
                    HttpContext.Session.SetInt32("Session_MI_Id", cmn.IVRM_MI_Id);
                }
            }

            loginData.MI_Id = loginData.MI_Id;  //loginData.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            var userdata = pre.getregdata(loginData);

            if (userdata.roleId != 0)
            {

                if (userdata.ASMAY_Id != 0)
                {

                    // Added on 16-11-2016 to set MI Id
                    HttpContext.Session.SetInt32("Session_MI_Id", Convert.ToInt32(userdata.MI_ID));
                    HttpContext.Session.SetInt32("Session_MI_Id_Preadmission", Convert.ToInt32(userdata.MI_ID));
                    DateTime subscriptiondate = userdata.subscriptionenddate;

                    DateTime curdate = DateTime.Now;

                    // Added on 16-11-2016
                    HttpContext.Session.SetInt32("RoleId", Convert.ToInt32(userdata.roleId));  // Changed on 5-11-2016
                                                                                               // Added on 25-11-2016
                    HttpContext.Session.SetInt32("UserId", Convert.ToInt32(userdata.userId));

                    HttpContext.Session.SetString("UserName", Convert.ToString(userdata.UserName));

                    HttpContext.Session.SetInt32("Role", Convert.ToInt32(userdata.role));

                    HttpContext.Session.SetString("RoleNme", Convert.ToString(userdata.roleforlogin));

                    HttpContext.Session.SetString("Roleflag", Convert.ToString(userdata.flag));  // Changed on 5-11-2016

                    HttpContext.Session.SetInt32("ASMAY_Id", Convert.ToInt32(userdata.ASMAY_Id));

                    HttpContext.Session.SetString("ASMAY_Year", Convert.ToString(userdata.ASMAY_Year));

                    HttpContext.Session.SetInt32("AMST_Id", Convert.ToInt32(userdata.AMST_Id)); // Changed on 03-06-2017
                    HttpContext.Session.SetInt32("AMCST_Id", Convert.ToInt32(userdata.AMST_Id)); // Changed on 20-06-2020
                    HttpContext.Session.SetInt32("ALMST_Id", Convert.ToInt32(userdata.ALMST_Id)); // Changed on 20-06-2020

                    HttpContext.Session.SetString("Userpassword", Convert.ToString(request.Password));

                    HttpContext.Session.SetInt32("IMFY_Id", Convert.ToInt32(userdata.IMFY_Id));

                    HttpContext.Session.SetInt32("Feecheck", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationStaff", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationPrinicipal", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationManager", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationChairman", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationCollegeStaff", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationCollegePrinicipal", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationCollegeManager", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationCollegeChairman", 0);

                    HttpContext.Session.SetInt32("PaymentNootificationGeneral", 0);
                    HttpContext.Session.SetInt32("smscreditalert", 0);
                    HttpContext.Session.SetInt32("StudentUpdateRequest", 0);
                    HttpContext.Session.SetInt32("Student_OnlineExam", 0);

                    HttpContext.Response.Cookies.Append("IsLogged", "true", new Microsoft.AspNetCore.Http.CookieOptions() { Path = "/", HttpOnly = false, Secure = false });
                    //Response.Cookies.Append("userId",Convert.ToString(lgnDto.userId));
                    //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);

                    if (userdata.disableflag == "ORG")
                    {
                        HttpContext.Session.SetInt32("UserId", 0);
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,

                            ErrorDescription = userdata.message
                            // ErrorDescription = userdata.message
                        });
                    }

                    if (userdata.disableflag == "INT")
                    {

                        HttpContext.Session.SetInt32("UserId", 0);
                        return BadRequest(new OpenIdConnectResponse
                        {

                            Error = OpenIdConnectConstants.Errors.InvalidGrant,

                            ErrorDescription = userdata.message
                            // ErrorDescription = userdata.message
                        });


                    }

                    if (userdata.subscriptionFlag == false)
                    {
                        HttpContext.Session.SetInt32("UserId", 0);
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,
                            ErrorDescription = userdata.message
                            // ErrorDescription = userdata.message
                        });
                    }

                    if (subscriptiondate < curdate)
                    {
                        HttpContext.Session.SetInt32("UserId", 0);
                        return BadRequest(new OpenIdConnectResponse
                        {
                            Error = OpenIdConnectConstants.Errors.InvalidGrant,

                            ErrorDescription = userdata.message
                            // ErrorDescription = userdata.message
                        });
                    }



                }
                else
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "Kindly contact Administrator!!,[ACADEMIC YEAR]"
                        // ErrorDescription = userdata.message
                    });
                }
            }
            else
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    //  ErrorDescription = "The username Does not belongs to this institution."
                    ErrorDescription = userdata.message
                });
                //HttpContext.Response.Cookies.Append("IsLogged", "false");
                //lgnDto.IsLogged = Convert.ToBoolean(Request.Cookies["IsLogged"]);
            }

            // userTemp.Email = user.Email;
            userTemp.UserName = "Sadmin";
            userTemp.PasswordHash = "Sadmin@123";


            if (request.IsPasswordGrantType())
            {
                var user = await _userManager.FindByNameAsync(request.Username);

                if (user == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."

                        // ErrorDescription = userdata.message
                    });
                }

                //if (userTemp.UserName == null)
                //{
                //    return BadRequest(new OpenIdConnectResponse
                //    {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "The username/password couple is invalid."
                //    });
                //}

                // Ensure the user is allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The specified user is not allowed to sign in."
                    });
                }

                //// Reject the token request if two-factor authentication has been enabled by the user.
                //if (_userManager.SupportsUserTwoFactor && await _userManager.GetTwoFactorEnabledAsync(user)) {
                //    return BadRequest(new OpenIdConnectResponse {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "The specified user is not allowed to sign in."
                //    });
                //}

                //// Ensure the user is not already locked out.
                //if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)) {
                //    return BadRequest(new OpenIdConnectResponse {
                //        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                //        ErrorDescription = "The username/password couple is invalid."
                //    });
                //}

                //// Ensure the password is valid.
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    if (_userManager.SupportsUserLockout)
                    {
                        await _userManager.AccessFailedAsync(user);
                    }

                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The username/password couple is invalid."
                    });
                }

                //if (_userManager.SupportsUserLockout)
                //{
                //    await _userManager.ResetAccessFailedCountAsync(user);
                //}

                // Create a new authentication ticket.
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            else if (request.IsRefreshTokenGrantType())
            {
                //var user = await _userManager.FindByNameAsync(request.Username);

                // Retrieve the claims principal stored in the refresh token.
                var info = await HttpContext.Authentication.GetAuthenticateInfoAsync(
                    OpenIdConnectServerDefaults.AuthenticationScheme);

                // Retrieve the user profile corresponding to the refresh token.
                var user = await _userManager.GetUserAsync(info.Principal);
                if (userTemp == null)
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The refresh token is no longer valid."
                    });
                }

                // Ensure the user is still allowed to sign in.
                if (!await _signInManager.CanSignInAsync(user))
                {
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidGrant,
                        ErrorDescription = "The user is no longer allowed to sign in."
                    });
                }

                // Create a new authentication ticket, but reuse the properties stored
                // in the refresh token, including the scopes originally granted.
                var ticket = await CreateTicketAsync(request, user);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }


        private async Task<AuthenticationTicket> CreateTicketAsync(

         OpenIdConnectRequest request, ApplicationUser user,

         Microsoft.AspNetCore.Authentication.AuthenticationProperties properties = null)

        {

            var identity = new ClaimsIdentity(
                   OpenIdConnectServerDefaults.AuthenticationScheme,
                   OpenIdConnectConstants.Claims.Name,
                   OpenIdConnectConstants.Claims.Role);
            // Add a "sub" claim containing the user identifier, and attach
            // the "access_token" destination to allow OpenIddict to store it
            // in the access token, so it can be retrieved from your controllers.
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                "71346D62-9BA5-4B6D-9ECA-755574D628D8",
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Name, request.Username,
                OpenIdConnectConstants.Destinations.AccessToken);


            // ... add other claims, if necessary.
            //var principal = new ClaimsPrincipal(identity);

            // Create a new ClaimsPrincipal containing the claims that

            // will be used to create an id_token, a token or a code.

            var principal = await
_signInManager.CreateUserPrincipalAsync(user);



            // Create a new authentication ticket holding the user identity.

            var ticket = new AuthenticationTicket(principal, properties,

                OpenIdConnectServerDefaults.AuthenticationScheme);



            if (!request.IsRefreshTokenGrantType())

            {

                // Set the list of scopes granted to the client application.

                // Note: the offline_access scope must be granted

                // to allow OpenIddict to return a refresh token.

                ticket.SetScopes(new[]

                {

                    OpenIdConnectConstants.Scopes.OpenId,

                    OpenIdConnectConstants.Scopes.Email,

                    OpenIdConnectConstants.Scopes.Profile,

                    OpenIdConnectConstants.Scopes.OfflineAccess,

                    OpenIddictConstants.Scopes.Roles

                }.Intersect(request.GetScopes()));

            }



            ticket.SetResources("http://localhost:51572/");



            // Note: by default, claims are NOT automatically included in             the access and identity tokens.

            // To allow OpenIddict to serialize them, you must attach them a             destination, that specifies

            // whether they should be included in access tokens, in identity tokens or in both.




            foreach (var claim in ticket.Principal.Claims)

            {

                // Never include the security stamp in the access and identity tokens, as it's a secret value.

                //if (claim.Type ==_identityOptions.Value.ClaimsIdentity.SecurityStampClaimType)

                //{

                //    continue;

                //}



                var destinations = new List<string> { OpenIdConnectConstants.Destinations.AccessToken };


                // Only add the iterated claim to the id_token if the  corresponding scope was granted to the client application.

                // The other claims will only be added to the access_token, which is encrypted when using the default format.

                if ((claim.Type == OpenIdConnectConstants.Claims.Name &&
ticket.HasScope(OpenIdConnectConstants.Scopes.Profile)) ||

                    (claim.Type == OpenIdConnectConstants.Claims.Email &&
ticket.HasScope(OpenIdConnectConstants.Scopes.Email)) ||

                    (claim.Type == OpenIdConnectConstants.Claims.Role &&
ticket.HasScope(OpenIddictConstants.Claims.Roles)))

                {


                    destinations.Add(OpenIdConnectConstants.Destinations.IdentityToken);

                }



                claim.SetDestinations(destinations);

            }



            return ticket;

        }

    }


    //private async Task<AuthenticationTicket> CreateTicketAsync(OpenIdConnectRequest request, ApplicationUser user, AuthenticationProperties properties = null) {
    //        // Create a new ClaimsPrincipal containing the claims that
    //        // will be used to create an id_token, a token or a code.
    //        //var principal = await _signInManager.CreateUserPrincipalAsync(user);
    //        ClaimsPrincipal principal = new ClaimsPrincipal();
    //        ClaimsIdentity cid = new ClaimsIdentity();
    //        Claim clm = new Claim(ClaimTypes.NameIdentifier, "test");

    //        cid.AddClaim(clm);
    //        principal.AddIdentity(cid);
    //        //// Note: by default, claims are NOT automatically included in the access and identity tokens.
    //        //// To allow OpenIddict to serialize them, you must attach them a destination, that specifies
    //        //// whether they should be included in access tokens, in identity tokens or in both.

    //        //foreach (var claim in principal.Claims) {
    //        //    // In this sample, every claim is serialized in both the access and the identity tokens.
    //        //    // In a real world application, you'd probably want to exclude confidential claims
    //        //    // or apply a claims policy based on the scopes requested by the client application.
    //        //    claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken,
    //        //                          OpenIdConnectConstants.Destinations.IdentityToken);
    //        //}

    //        // Create a new authentication ticket holding the user identity.
    //        var ticket = new AuthenticationTicket(
    //            principal, new AuthenticationProperties(),
    //            OpenIdConnectServerDefaults.AuthenticationScheme);

    //        // Set the list of scopes granted to the client application.
    //        // Note: the offline_access scope must be granted
    //        // to allow OpenIddict to return a refresh token.
    //        ticket.SetScopes(new[] {
    //            OpenIdConnectConstants.Scopes.OpenId,
    //            OpenIdConnectConstants.Scopes.Email,
    //            OpenIdConnectConstants.Scopes.Profile,
    //            OpenIdConnectConstants.Scopes.OfflineAccess,
    //            OpenIddictConstants.Scopes.Roles
    //        }.Intersect(request.GetScopes()));

    //        return ticket;
    //    }
    //}
}