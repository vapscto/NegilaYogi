
login.controller("loginCtrl", ['$rootScope', '$scope', '$state', '$location', 'loginService', 'Flash', 'apiService', '$stateParams', 'AuthService', '$cookieStore', '$http', 'FormSubmitter', '$q', '$filter', '$timeout',
    function ($rootScope, $scope, $state, $location, loginService, Flash, apiService, $stateParams, AuthService, $cookieStore, $http, FormSubmitter, $q, $filter, $timeout) {
        //right click and console has been disabled
        //document.addEventListener('contextmenu', event => event.preventDefault());
        //$(document).keydown(function (event) {
        //    if (event.keyCode == 123) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 73) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 'I'.charCodeAt(0)) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.shiftKey && event.keyCode == 'J'.charCodeAt(0)) {
        //        return false;
        //    }
        //    else if (event.ctrlKey && event.keyCode == 'U'.charCodeAt(0)) {
        //        return false;
        //    }
        //});
        //end
        //< !--Counter For Preadmission Start-- >
        $scope.show = false;
        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();
        var stopped;
        var stopped1;
        $scope.timer = null;
        $scope.livehide = false;
        $scope.countdown = function () {
            stopped = $timeout(function () {
                // Set the date we're counting down to
                var countDownDate = new Date($scope.cdate).getTime();
                // Update the count down every 1 second   
                // Get today's date and time
                $scope.getserverdate();
                var now = new Date($scope.today).getTime();
                // Find the distance between now and the count down date
                var distance = countDownDate - now;
                // Time calculations for days, hours, minutes and seconds
                var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                // If the count down is over, write some text 
                if (minutes <= 0 && seconds <= 0 && hours <= 0 && days <= 0) {
                    $scope.prereg = false;
                    $scope.live = false;
                    $scope.countdownstop();
                }
                else {
                    // Output the result in an element with id="demo"
                    $scope.cdays = days;
                    $scope.hours = hours;
                    $scope.minutes = minutes;
                    $scope.seconds = seconds;
                    $scope.live = true;
                    //$scope.timer = days + "D " + hours + "H "
                    //    + minutes + "M " + seconds + "S ";
                }
                $scope.countdown();
            }, 1000);
        };
        $scope.countdownstop = function () {
            stopped1 = $timeout(function () {
                $scope.getserverdate();
                var dnow = new Date($scope.today);
                if ($scope.cedate >= dnow) {
                    $scope.live = false;
                }
                else {
                    $scope.livehide = false;
                    $scope.prereg = true;
                    $scope.stop();
                }
                $scope.countdownstop();
            }, 1000);
        };
        $scope.stop = function () {
            $timeout.cancel(stopped);
            $interval.cancel(stopped);
        };

        $scope.stopc = function () {
            $timeout.cancel(stopped1);
            $interval.cancel(stopped1);
        };
        //< !--Counter For Preadmission End-- >
        var mi_id;
        var HostName = location.host;
        $scope.buttonClicked = "";
        $scope.toggleModal = function (btnClicked) {
            $scope.buttonClicked = btnClicked;
            $scope.showModal = !$scope.showModal;
        };
        $scope.sendmobotp = false;
        $scope.sendemailotp = false;
        $scope.usermorebefore = true;
        $scope.Paymentnotdone = true;
        $scope.Paymentdone = false;
        $scope.alumni = false;
        $scope.Careers = false;
        $scope.alumnicollege = false;
        $scope.ProspectuseScreen = true;
        $scope.PaymentMode = false;
        var vm = this;
        vm.getUser = {};
        vm.setUser = {};
        //vm.getRegUser = {};
        vm.signIn = true;
        $scope.SelectedFileForUploadz = [];
        $scope.loaddded = false;
        var onlineprofilepicpath;
        $scope.calcel = function () {
            //vm.getRegUser = '';
            //$scope.submitted = false;
            //$scope.vm.registrationForm.$setPristine();
            //$scope.vm.registrationForm.$setUntouched();
            //vm.signIn = true;
            $state.reload();
            vm.signIn = true;
        }
        //Draggable Modal Start
        //$("#myModal3").draggable({
        //    handle: ".modal-header"
        //});
        var searchObject = $location.search();
        $scope.classdesable = true;
        // alert(searchObject.status);
        if (searchObject.status == "failure") {
            swal("Payment Unsuccessfull");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "created") {
            swal("Payment Successfull", "Account created successfully!!");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "checkadmin") {
            swal('Please contact administrator!!');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Username Already exist .. !!") {
            swal('Username Already exist .. !!');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }

        $scope.onYearCahnge = function (acdYId) {
            apiService.getURI("ApplicationForm/getCourse/", acdYId).then(function (promise) {
                if (promise.courses != null) {
                    $scope.courses = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.courses = "";
                }
            });
        }

        $scope.onCourseChange = function (courseId, asmyid) {
            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": asmyid,
                    "ACAYC_Id": selectedData[0].acayC_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {
                    if (promise.branches != null) {
                        $scope.branches = promise.branches;
                        $scope.obj.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branches = "";
                    }
                })
            }
        }

        $scope.onBranchchange = function (branchId, asmyid) {
            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": asmyid,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {
                    if (promise.semesters != null) {
                        $scope.semesters = promise.semesters;
                    }
                    else {
                        swal("No Semester Is Mapped To Selected Branch");
                        $scope.semesters = "";
                    }
                })
            }
        }
        $scope.onYearCahngeLeft = function (acdYId) {
            apiService.getURI("ApplicationForm/getCourse/", acdYId).then(function (promise) {

                if (promise.courses != null) {
                    $scope.coursesleft = promise.courses;
                }
                else {
                    swal("No Course Is Mapped To Selected Academic Year");
                    $scope.coursesleft = "";
                }
            });
        }
        $scope.onCourseChangeLeft = function (courseId, asmyid) {
            var selectedData = $filter('filter')($scope.courses, { 'amcO_Id': courseId });
            if (selectedData != "") {
                var data = {
                    "AMCO_Id": courseId,
                    "ASMAY_Id": asmyid,
                    "ACAYC_Id": selectedData[0].acayC_Id
                }
                apiService.create("ApplicationForm/getBranch/", data).then(function (promise) {
                    if (promise.branches != null) {
                        $scope.branchesleft = promise.branches;
                        $scope.obj.AMCOC_Id = "";
                        if (promise.studentCategory != null) {
                            $scope.obj.AMCOC_Id = promise.studentCategory[0].amcoC_Id;
                        }
                        else {
                            swal("To get Student Category.Please Map Selected Course to Some category");
                        }
                    }
                    else {
                        swal("No Branch Is Mapped To Selected Course");
                        $scope.branchesleft = "";
                    }
                })
            }
        }
        $scope.onBranchchangeLeft = function (branchId, asmyid) {
            var selectedData = $filter('filter')($scope.branches, { 'amB_Id': branchId });
            if (branchId != "") {
                var data = {
                    "AMB_Id": branchId,
                    "ASMAY_Id": asmyid,
                    "ACAYCB_Id": selectedData[0].acaycB_Id
                }
                apiService.create("ApplicationForm/getSemester/", data).then(function (promise) {
                    if (promise.semesters != null) {
                        $scope.semestersleft = promise.semesters;
                    }
                    else {
                        swal("No Semester Is Mapped To Selected Branch");
                        $scope.semestersleft = "";
                    }
                })
            }
        }
        if ($state.current.name == 'login') {
            getVirtual();
        }
        function getVirtual() {
            var virtualSchoolId = 0;
            if ($stateParams.virtualId != "") {
                virtualSchoolId = $stateParams.virtualId
            }
            var data = {
                "subdomainname": virtualSchoolId,
                "hostname": HostName
            }
            apiService.create("Login/setvirtauldeatilsnew", data).then(function (promise) {
                mi_id = promise.mi_id;
                if (promise.subDomainName != null && promise.subDomainName != "0") {
                    if (promise.noint == true) {
                        swal("Please Enter Correct School Subdomain..!!");
                        window.location.href = "http://localhost:57606/#/login/";
                    }
                    else {
                        var genconfig = promise.ivrmconfiglist;
                        $scope.institutemiid = promise.subDomainName;
                        if (promise.fillinstitution != null && promise.fillinstitution != undefined && promise.fillinstitution.length > 0) {
                            $scope.collegeschool = promise.fillinstitution[0].mI_SchoolCollegeFlag;
                            $scope.institut = promise.fillinstitution;
                        }
                        var conlistlist = promise.configlist;
                        localStorage.setItem("Startconfig", JSON.stringify(genconfig));
                        $scope.configotp = promise.ivrmconfiglist;
                        $scope.noint = promise.noint;
                        $scope.emailarrayLis = promise.emailarrayList;
                        $scope.phonearrayLis = promise.phonearrayList;
                        $scope.getaccyear = promise.allyearlist;
                        $scope.getclass = promise.allclasslist;
                        if (promise.registration == true) {
                            $scope.prereg = false;
                            //$scope.crereg = false;
                            //$scope.live = false;
                            $scope.filepre = true;
                            $scope.preadmissionmsg = "";
                        }
                        else {
                            $scope.prereg = true;
                            //$scope.crereg = true;
                            //$scope.live = true;
                            $scope.filepre = false;
                            $scope.preadmissionmsg = "Preadmission Registration is closed for the current date..!!";
                        }
                        if (promise.payment == true) {
                            $scope.Paymentnotdone = false;
                            $scope.Paymentdone = true;
                        }
                        else {
                            $scope.Paymentnotdone = true;
                            $scope.Paymentdone = false;
                        }
                        if ($scope.configotp != null && $scope.configotp != undefined && $scope.configotp.length > 0) {
                            angular.forEach($scope.configotp, function (object) {
                                $scope.moboto = object.ivrmgC_MobileValOTPFlag;
                                $scope.emailboto = object.ivrmgC_emailValOTPFlag;
                                if ($scope.moboto == true) {
                                    $scope.sendmobotp = true;
                                }
                                else {
                                    $scope.sendmobotp = false;
                                }
                                if ($scope.emailboto == true) {
                                    $scope.sendemailotp = true;
                                } else {
                                    $scope.sendemailotp = false;
                                }
                            })
                        }
                        if ($scope.institut != null && $scope.institut != undefined && $scope.institut.length > 0) {
                            angular.forEach($scope.institut, function (object) {
                                $scope.background = object.mI_BackgroundImage;
                                $scope.logo = object.mI_Logo;
                                $scope.show = true;
                                $scope.Aboutinstitute = object.mI_AboutInstitute;
                                $scope.instituteContactDetails = object.mI_ContactDetails;
                                $scope.instituteHelpFile = object.mI_HelpFile;
                                $scope.ivrs = object.mI_IVRSVirtualNo;
                            })
                        }
                        if ($scope.emailarrayLis != null && $scope.emailarrayLis != undefined && $scope.emailarrayLis.length > 0) {
                            angular.forEach($scope.emailarrayLis, function (object) {
                                $scope.emailinst = object.miE_EmailId;

                            })
                        }
                        if ($scope.phonearrayLis != null && $scope.phonearrayLis != undefined && $scope.phonearrayLis.length > 0) {
                            angular.forEach($scope.phonearrayLis, function (object) {
                                $scope.phoneinst = object.mipN_PhoneNo;
                            })
                        }
                        $scope.loaddded = true;
                        $scope.trustorint = true;
                        $scope.livehide = true;
                    }
                    //< !--Counter For Preadmission Start-- >
                    if ($scope.prereg === true) {
                        $scope.cdate = new Date(promise.prestartdate);
                        $scope.cedate = new Date(promise.presenddate);
                        $scope.getserverdate();
                        var dnow = new Date($scope.today);
                        if ($scope.cedate >= dnow) {
                            $scope.countdown();
                        }
                    }
                    else {
                        $scope.cdate = new Date(promise.prestartdate);
                        $scope.cedate = new Date(promise.presenddate);
                        $scope.today = new Date(promise.createdDate);
                        $scope.countdownstop();
                    }
                    //< !--Counter For Preadmission Start-- >
                }
                else {
                    $scope.institut = promise.fillinstitution;
                    $scope.institutiondetails = promise.institutiondetails;
                    $scope.emailarrayLis = promise.emailarrayList;
                    $scope.phonearrayLis = promise.phonearrayList;
                    if ($scope.institut != null && $scope.institut != undefined && $scope.institut.length > 0) {
                        angular.forEach($scope.institut, function (object) {
                            $scope.logo = object.mO_Logo;
                            $scope.Aboutinstitute = object.mO_AboutInstitute;
                            $scope.instituteContactDetails = object.mO_ContactDetails;
                        })
                    }
                    $scope.show = true;
                    if ($scope.emailarrayLis != null && $scope.emailarrayLis != undefined && $scope.emailarrayLis.length > 0) {
                        angular.forEach($scope.emailarrayLis, function (object) {
                            $scope.emailinst = object.moE_EmailId;
                        })
                    }
                    if ($scope.phonearrayLis != null && $scope.phonearrayLis != undefined && $scope.phonearrayLis.length > 0) {
                        angular.forEach($scope.phonearrayLis, function (object) {
                            $scope.phoneinst = object.mopN_PhoneNo;
                        })
                    }

                    $scope.Paymentnotdone = true;
                    $scope.Paymentdone = false;
                    $scope.loaddded = true;
                    $scope.crereg = true;
                    $scope.live = false;
                    $scope.prereg = true;
                    $scope.filepre = false;
                    $scope.trustorint = false;
                }
                if (promise.multiplewindowvalue == "differentlogin") {
                    $state.go('app.homepage');
                    //swal("You have already logged in with the same credentials in another window.This is for kind information!!")
                }
                else if (promise.multiplewindowvalue == "logout") {
                    $state.go('login');
                }
            });
        }

        $scope.acedmicYearchange = function (startyear, endyear) {
            var startyr = 0;
            var endyr = 0;
            if (startyear !== undefined || startyear !== null) {
                angular.forEach($scope.getaccyear, function (objectt) {
                    if (objectt.asmaY_Id == startyear) {
                        startyr = objectt.asmaY_Order;
                    }
                });
            }
            if (endyear !== undefined || endyear !== null) {
                angular.forEach($scope.getaccyear, function (objectt) {
                    if (objectt.asmaY_Id == endyear) {
                        endyr = objectt.asmaY_Order;
                    }
                });
            }
            if (endyr !== 0 && startyr !== 0) {
                if (endyr < startyr) {
                    $scope.getaccyear = $scope.getaccyear;
                    $scope.vm.getRegUser.asmaY_Id = "";
                    $scope.vm.getRegUser.asmaY_Idleft = "";
                    swal("Select proper Start and End Academic years!");
                }
            }
        };

        $scope.classchange = function (startclass, endclass) {
            var startcl = 0;
            var endcl = 0;
            if (startclass != undefined || startclass != null) {
                angular.forEach($scope.getclass, function (objectt) {
                    if (objectt.asmcL_Id == startclass) {
                        startcl = objectt.asmcL_Order;
                    }
                })
            }

            if (endclass != undefined || endclass != null) {
                angular.forEach($scope.getclass, function (objectt) {
                    if (objectt.asmcL_Id == endclass) {
                        endcl = objectt.asmcL_Order;
                    }
                })
            }
            if (startcl != 0 && endcl != 0) {
                if (endcl < startcl) {
                    $scope.getclassfill = $scope.getclass;
                    $scope.vm.getRegUser.classidadmitted = "";
                    $scope.vm.getRegUser.classid = "";
                    swal("Select proper Start and End Classes!");
                }
            }
        }
        $scope.Back = function () {
            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;
        }
        $scope.alumniclick = function () {
            $scope.alumni = true;
            vm.signIn = false;
            $scope.Paymentdone = false;
            $scope.Paymentnotdone = false;
        }
        $scope.Careersclick = function () {
            $scope.Careers = true;
            vm.signIn = false;
            $scope.Paymentdone = false;
            $scope.Paymentnotdone = false;
        }
        $scope.alumniclickcollege = function () {
            $scope.alumnicollege = true;
            vm.signIn = false;
            $scope.Paymentdone = false;
            $scope.Paymentnotdone = false;
        }
        $scope.ForgetEmailInput = false;
        $scope.forgetEmailOtp = false;
        $scope.forgetemailOtpbutton = false;
        $scope.forgetemailResendOtpbutton = false;
        $scope.forgetemailOtpMatch = false;
        $scope.forgetemailOtpNotMatch = false;
        $scope.forgetEmailOtp = false;

        $scope.forgetmobileInput = false;
        $scope.forgetmobileInputotp = false;
        $scope.forgetmobileOtpbutton = false;
        $scope.forgetmobileResendOtpbutton = false;
        $scope.forgetmobileOtpMatch = false;
        $scope.forgetmobileOtpNotMatch = false;

        $scope.resetpassword = false;
        $scope.EmailMobileConform = false;
        $scope.UserNameSubmit = true;

        $scope.checkUsername = function (Username) {
            if (Username === "" || Username === undefined) {
                swal("Please Enter User name!");
            }
            else {
                var mobno = {
                    "UserName": Username
                }
                apiService.create("Login/VerifyUserName", mobno).then(function (promise) {
                    swal(promise);
                    if (promise.userNameVerifyStatus === "Success") {
                        $scope.EmailMobileConform = true;
                        $scope.forgetUserName = true;
                        $scope.UserNameSubmit = false;
                        $scope.forgetEMaildisable = true;
                        $scope.forgetMobiledisable = true;
                        $scope.vm.getRegUser.forgetEmail_id = promise.email;
                        $scope.vm.getRegUser.mobileNo = promise.mobileNo;
                    }
                    else {
                        swal("User Name Not Found..!");
                        $scope.EmailMobileConform = false;
                        $scope.forgetUserName = false;
                    }
                })
            }
        }

        $scope.forgetEmailOrMobileOption = function (Type) {
            if (Type === "EMAIL") {
                $scope.ForgetEmailInput = true;
                $scope.forgetEmailOtp = false;
                $scope.forgetemailOtpbutton = true;
                $scope.forgetemailResendOtpbutton = false;
                $scope.forgetemailOtpMatch = false;
                $scope.forgetemailOtpNotMatch = false;

                $scope.forgetmobileInput = false;
                $scope.forgetmobileInputotp = false;
                $scope.forgetmobileOtpbutton = false;
                $scope.forgetmobileResendOtpbutton = false;
                $scope.forgetmobileOtpMatch = false;
                $scope.forgetmobileOtpNotMatch = false;
            }
            else if (Type === "MOBILE") {
                $scope.forgetmobileInput = true;
                $scope.forgetmobileInputotp = false;
                $scope.forgetmobileOtpbutton = true;
                $scope.forgetmobileResendOtpbutton = false;
                $scope.forgetmobileOtpMatch = false;
                $scope.forgetmobileOtpNotMatch = false;
                $scope.ForgetEmailInput = false;
                $scope.forgetEmailOtp = false;
                $scope.forgetemailOtpbutton = false;
                $scope.forgetemailResendOtpbutton = false;
                $scope.forgetemailOtpMatch = false;
                $scope.forgetemailOtpNotMatch = false;
            }
        }

        $scope.forgetemailOtp = function (forgetEmail, usertype) {
            if (forgetEmail === "" || forgetEmail === undefined) {
                swal("Please Enter Email Id!");
            }
            else {
                var mobno = {
                    "Email": forgetEmail,
                    "clickedlinkname": $scope.clickedlinkname,
                    "usertype": usertype
                }
                apiService.create("Login/ForgotOTPForEmail", mobno).then(function (promise) {
                    if ($scope.clickedlinkname == 'forgotpwd') {
                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = true;
                        $scope.forgetemailResendOtpbutton = true;
                        swal(promise.message);
                    }
                    else if ($scope.clickedlinkname == 'forgotname') {

                        if (promise != null && promise != '') {
                            if (promise.userlist != null && promise.userlist.length > 0) {
                                $scope.usermore = true;
                                $scope.usermorebefore = false;
                                $scope.userlist = promise.userlist;

                            }
                            else {
                                $scope.usermore = false;
                                $scope.usermorebefore = true;
                            }

                            swal(promise.message);
                            $scope.clearforgot();
                            $('#myModal3').modal('hide');
                        }
                        else {
                            $scope.clearforgot();
                        }
                    }
                })
            }
        }
        $scope.ForgotEmailstudent = function (usertype) {
            var mobno = {
                "AMST_Id": usertype,
            }
            apiService.create("Login/ForgotEmailstudent", mobno).then(function (promise) {
                if (promise.message != null) {
                    swal(promise.message);
                    $scope.clearforgot();
                    $('#myModal3').modal('hide');
                }
            })
        }
        $scope.VerifyforgetEmailOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {
                var mobno = {
                    "EMAILOTP": data
                }
                apiService.create("Login/VerifyEmailOtp", mobno).then(function (promise) {
                    if (promise === "Success") {
                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = false;
                        $scope.forgetemailOtpMatch = true;
                        $scope.forgetemailOtpNotMatch = false;
                        $scope.forgetemailResendOtpbutton = false;
                        $scope.forgetEMaildisable = true;
                        $scope.resetpassword = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!");
                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = true;
                        $scope.forgetemailOtpMatch = false;
                        $scope.forgetemailOtpNotMatch = true;
                        $scope.forgetemailResendOtpbutton = true;
                        $scope.forgetEMaildisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");
                        $scope.forgetemailOtpbutton = false;
                        $scope.forgetEmailOtp = true;
                        $scope.forgetemailOtpMatch = false;
                        $scope.forgetemailOtpNotMatch = true;
                        $scope.forgetemailResendOtpbutton = true;
                        $scope.forgetEMaildisable = false;
                    }
                })
            }
        }
        $scope.forgetmobileOtp = function (MobileNO, UserName) {
            if (MobileNO === "" || MobileNO === undefined) {
                swal("Please Enter Mobile Number!");
            }
            else {
                var mobno = {
                    "mobileNo": MobileNO,
                    "UserName": UserName
                }
                apiService.create("Login/getOTPForMobile", mobno).then(function (promise) {
                    swal(promise);
                    $scope.forgetmobileInputotp = true;
                    $scope.forgetmobileOtpbutton = false;
                    $scope.forgetmobileResendOtpbutton = true;
                })
            }
        }
        $scope.VerifyforgetmobileOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {
                var mobno = {
                    "MOBILEOTP": data
                }
                apiService.create("Login/VerifymobileOtp", mobno).then(function (promise) {
                    if (promise === "Success") {
                        $scope.forgetmobileInputotp = false;
                        $scope.forgetmobileOtpbutton = false;
                        $scope.forgetmobileResendOtpbutton = false;
                        $scope.forgetmobileOtpMatch = true;
                        $scope.forgetmobileOtpNotMatch = false;
                        $scope.forgetMobiledisable = true;
                        $scope.resetpassword = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");
                        $scope.forgetmobileInputotp = true;
                        $scope.forgetmobileOtpbutton = false;
                        $scope.forgetmobileResendOtpbutton = false;
                        $scope.forgetmobileOtpMatch = false;
                        $scope.forgetmobileOtpNotMatch = true;
                        $scope.forgetMobiledisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");
                        $scope.forgetmobileInputotp = true;
                        $scope.forgetmobileOtpbutton = false;
                        $scope.forgetmobileResendOtpbutton = false;
                        $scope.forgetmobileOtpMatch = false;
                        $scope.forgetmobileOtpNotMatch = true;
                        $scope.forgetMobiledisable = false;
                    }
                })
            }
        }
        $scope.MobileOtp = false;
        $scope.mobileOtpbutton = true;
        $scope.mobileOtpMatch = false;
        $scope.mobileOtpNotMatch = false;
        $scope.mobileOtp = function (MobileNO, UserName) {
            if (MobileNO === "" || MobileNO === undefined) {
                swal("Please Enter Mobile Number!");
            }
            else {
                var mobno = {
                    "mobileNo": MobileNO,
                    "UserName": UserName
                }
                apiService.create("Login/getOTPForMobile", mobno).then(function (promise) {
                    swal(promise);
                    $scope.mobileOtpbutton = false;
                    $scope.MobileOtp = true;
                    $scope.sendmobotp = false;
                    $scope.mobileResendOtpbutton = true;
                })
            }
        }
        $scope.VerifymobileOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {
                var mobno = {
                    "MOBILEOTP": data
                }
                apiService.create("Login/VerifymobileOtp", mobno).then(function (promise) {
                    if (promise === "Success") {
                        $scope.mobileOtpbutton = false;
                        $scope.MobileOtp = false;
                        $scope.mobileOtpMatch = true;
                        $scope.mobileOtpNotMatch = false;
                        $scope.mobileResendOtpbutton = false;
                        $scope.Mobiledisable = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");
                        $scope.mobileOtpbutton = false;
                        $scope.MobileOtp = true;
                        $scope.mobileOtpMatch = false;
                        $scope.mobileOtpNotMatch = true;
                        $scope.mobileResendOtpbutton = true;
                        $scope.Mobiledisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");
                        $scope.mobileOtpbutton = false;
                        $scope.MobileOtp = true;
                        $scope.mobileOtpMatch = false;
                        $scope.mobileOtpNotMatch = true;
                        $scope.mobileResendOtpbutton = true;
                        $scope.Mobiledisable = false;
                    }
                })
            }
        }
        $scope.EmailOtp = false;
        $scope.emailOtpbutton = true;
        $scope.emailOtpMatch = false;
        $scope.emailOtpNotMatch = false;
        $scope.emailOtp = function (EmailId, UserName) {
            if (EmailId === "" || EmailId === undefined) {
                swal("Please Enter Email Id!");
            }
            else {
                var mobno = {
                    "Email": EmailId,
                    "UserName": UserName
                }
                apiService.create("Login/getOTPForEmail", mobno).then(function (promise) {
                    swal(promise);
                    $scope.emailOtpbutton = false;
                    $scope.EmailOtp = true;
                    $scope.sendemailotp = false;
                    $scope.emailResendOtpbutton = true;
                })
            }
        }
        $scope.VerifyemailOtp = function (data) {
            if (data === "" || data === undefined) {
                swal("Please Enter OTP Number!");
            }
            else {
                var mobno = {
                    "EMAILOTP": data
                }
                apiService.create("Login/VerifyEmailOtp", mobno).then(function (promise) {
                    if (promise === "Success") {
                        $scope.emailOtpbutton = false;
                        $scope.EmailOtp = false;
                        $scope.emailOtpMatch = true;
                        $scope.emailOtpNotMatch = false;
                        $scope.emailResendOtpbutton = false;
                        $scope.EMaildisable = true;
                    }
                    else if (promise === "Fail") {
                        swal("OTP Mismatch!!");
                        $scope.emailOtpbutton = false;
                        $scope.EmailOtp = true;
                        $scope.emailOtpMatch = false;
                        $scope.emailOtpNotMatch = true;
                        $scope.emailResendOtpbutton = true;
                        $scope.EMaildisable = false;
                    }
                    else {
                        swal("OTP Expired. Please resend OTP");

                        $scope.emailOtpbutton = false;
                        $scope.EmailOtp = true;
                        $scope.emailOtpMatch = false;
                        $scope.emailOtpNotMatch = true;
                        $scope.emailResendOtpbutton = true;
                        $scope.EMaildisable = false;
                    }
                })
            }
        }
        $scope.submitted1 = false;
        vm.forgotpassword = function (data) {
            if ($scope.forgotpasswordForm.$valid) {
                if (data.password2 != data.Confirmpassword) {
                    swal('Confirm Password is Not Match with New Password');
                    return;
                }
                var dt = { "newPassword": data.password2, "UserName": data.userName, "ForgetPasswordSelection": $scope.Type }
                if (data.Confirmpassword == data.password2) {
                    apiService.create("Login/sendforgotpassword", dt).then(function (promise) {
                        if (promise.message == "Success") {
                            $scope.clearforgot();
                            swal("Successfully Reset Your Password, Please Login With Your New Password..!");
                            //$state.go('login' + promise.subDomainName)
                            $('#modelClose').trigger('click');
                            $scope.vm.getRegUser = "";
                            if (promise.subDomainName == null || promise.subDomainName == "") {
                                window.location.href = "http://localhost:57606/#/login/";
                            }
                            else {
                                window.location.href = "http://localhost:57606/#/login/" + promise.subDomainName;
                            }
                        }
                        else {
                            swal(promise.message);
                        }
                    });
                }
                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }
            else {
                $scope.submitted1 = true;
            }
        }
        $scope.clearforgot = function () {
            $scope.vm.getRegUser = "";
            $scope.EmailMobileConform = false;
            $scope.ForgetEmailInput = false;
            $scope.forgetEmailOtp = false;
            $scope.UserNameSubmit = true;
            $scope.forgetUserName = false;
            $scope.Type = false;
            $scope.forgetEmail_id = "";
            $scope.forgetEmailOTP = "";
            $scope.forgetEMaildisable = false;
            $scope.forgetmobileInput = false;
            $scope.usermore = false;
            $scope.usermorebefore = true;
            $scope.resetpassword = false;
            var mobno = {
                "EMAILOTP": 1
            }
            apiService.create("Login/getsubdomain", mobno).
                then(function (promise) {
                    if (promise.subDomainName != null) {
                        window.location.href = "http://localhost:57606/#/login/" + promise.subDomainName;
                    }
                    else {
                        window.location.href = "http://localhost:57606/#/login/";
                    }
                })
        };
        // forgot password added on 10-11-2016
        vm.DrawCaptcha = function () {
            var a = Math.ceil(Math.random() * 10) + '';
            var b = Math.ceil(Math.random() * 10) + '';
            var c = Math.ceil(Math.random() * 10) + '';
            var d = Math.ceil(Math.random() * 10) + '';
            var e = Math.ceil(Math.random() * 10) + '';
            var f = Math.ceil(Math.random() * 10) + '';
            var g = Math.ceil(Math.random() * 10) + '';
            var code = a + ' ' + b + ' ' + ' ' + c + ' ' + d + ' ' + e + ' ' + f + ' ' + g;
            $scope.vm.getUser.txtCaptcha = code;
        }

        vm.loginNEW = function (Username, Password, txtCaptcha, txtInput) {
            if ($scope.vm.loginForm.$valid) {
                if (txtCaptcha != null && txtCaptcha.length > 0) {
                    txtCaptcha = txtCaptcha.split(' ').join('');
                }
                if (txtInput != undefined) {
                    txtInput = txtInput.split(' ').join('');
                }
                if (txtCaptcha == txtInput) {
                    var data = {
                        "Username": Username,
                        "password": Password
                    }
                    apiService.get("Login/getMIdata", data).then(function (promise) {
                        if (promise.message != '' && promise.message != null) {
                            swal(promise.message);
                        }
                        else {
                            $scope.viewpopup = "false";

                            $scope.MIdata = promise.mIdata;

                            if (promise.mIdata != null && promise.mIdata.length > 1) {
                                $('#institution').modal('show');
                            }
                            else if (promise.mIdata != null && promise.mIdata.length == 1) {
                                vm.login(Username, Password, promise.mIdata[0].mI_Id);
                            }
                            else {
                                $scope.institution = '#';
                            }
                        }
                    });
                }
                else {
                    swal("Please Enter Correct Captcha Number..!");
                }
            }
        };
        ////access login
        vm.login = function (Username, Password, txtCaptcha, txtInput) {
            if (txtCaptcha != null && txtCaptcha.length > 0) {
                txtCaptcha = txtCaptcha.split(' ').join('');
            }
            if (txtInput != undefined) {
                txtInput = txtInput.split(' ').join('');
            }
            if (Username != undefined || Username != null) {
                if (Password != undefined || Password != null) {
                    if (txtCaptcha == txtInput) {
                        var data = {
                            "Username": Username,
                            "password": Password,
                            "ClientId": mi_id,
                            "Logintype": "Web"
                        }
                        apiService.get("Login", data).then(function (promise) {
                            if (promise.error != "invalid_grant") {
                                if (promise.roleId != 0) {
                                    localStorage.setItem('testObject', JSON.stringify());
                                    var data = {
                                        "Username": Username,
                                        "password": Password,
                                        "ClientId": mi_id,
                                        "Logintype": "Web"
                                    }
                                    apiService.create("Login/getrolewisepage", data).then(function (promise) {
                                        if (promise.filldashpagemap != null && promise.filldashpagemap != undefined && promise.filldashpagemap.length > 0) {
                                            if (promise.pageexists.length > 0) {
                                                $scope.daspgenme = promise.filldashpagemap[0].ivrmP_Dasboard_PageName
                                            }
                                            else {
                                                $scope.daspgenme = "app.homepage";
                                            }
                                        }
                                        else {
                                            $scope.daspgenme = "app.homepage";
                                        }
                                        $state.go($scope.daspgenme);
                                    })
                                    // $state.go($scope.dashhmepage);
                                }
                                else {
                                    grecaptcha.reset();
                                    swal(promise.message);
                                }
                            }
                            else {
                                $scope.error_description = promise.error_description;
                                if (promise.error_description == "expired") {
                                    $scope.curpwd = "";
                                    $scope.newpwd = "";
                                    $scope.cnfpwd = "";
                                    $scope.submitted = false;
                                    if (Username != 'vaps super admin' && Username != 'sadmin') {
                                        $scope.contentforpasswordexpired="Password Is Expired"
                                        $('#changepassword').modal('show');
                                    }
                                }
                                else if (promise.error_description == "Alumni") {
                                    swal("Alumni Membership Not Yet Approved..!!!")
                                }
                                else if (promise.error_description == "FirstTimeLogin") {
                                    $scope.curpwd = "";
                                    $scope.newpwd = "";
                                    $scope.cnfpwd = "";
                                    $scope.submitted = false;
                                    if (Username != 'vaps super admin' && Username != 'sadmin') {
                                        $scope.contentforpasswordexpired = "";
                                        $('#changepassword').modal('show');
                                    }
                                }
                                else {
                                    $('.modal-backdrop').remove();
                                    swal(promise.error_description)
                                }
                            }
                        });
                    }
                    else {
                        swal("Please Enter Correct Captcha Number..!");
                    }
                }
                else {
                    swal("Please Enter Password..!");
                }
            }
            else {
                swal("Please Enter Username..!");
            }
        };

        $scope.submitted = false;
        $scope.SubmitForm = function (Username) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.newpwd != $scope.cnfpwd) {
                    swal('Confirm Password is Not Match with New Password!!');
                    return;
                }
                else if ($scope.curpwd == $scope.newpwd) {
                    swal('New Password should not be same as old password!!');
                    return;
                }
                var data = {
                    "password": $scope.curpwd,
                    "new_password": $scope.newpwd,
                    "username": Username,
                }
                apiService.create("Changepswd/", data).then(function (promise) {
                    if (promise.returnMsg === "Success") {
                        swal('Password changed Successfully', 'success');
                        $('#changepassword').modal('hide');
                        //$state.reload();
                    }
                    else if (promise.returnMsg === "fail") {
                        swal('Kindly enter valid Current password', 'Password is incorrect');
                        return;
                    }
                    else if (promise.returnMsg != "") {
                        swal(promise.returnMsg);
                        return;
                    }
                    else if (promise.returnMsg == "Error") {
                        swal('Password Not Changed', 'Failed');
                        $state.reload();
                    }
                })
            }
        };

        $scope.SubmitForm1 = function (Username) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.newpwd != $scope.cnfpwd) {
                    swal('Confirm Password is Not Match with New Password!!');
                    return;
                }
                else if ($scope.curpwd == $scope.newpwd) {
                    swal('New Password should not be same as old password!!');
                    return;
                }
                var data = {
                    "password": $scope.curpwd,
                    "new_password": $scope.newpwd,
                    "username": Username,
                    "Logintype": "Web",
                    "changepasswordtypeflag": $scope.error_description
                }
                apiService.create("Changepswd/saveexppassword", data).then(function (promise) {
                    if (promise.returnMsg === "Success") {
                        swal('Password changed Successfully', 'success');
                        $('#changepassword').modal('hide');
                        //$state.reload();
                    }
                    else if (promise.returnMsg === "fail") {
                        swal('Kindly enter valid Current password', 'Password is incorrect');
                        return;
                    }
                    else if (promise.returnMsg != "") {
                        swal(promise.returnMsg);
                        return;
                    }
                    else if (promise.returnMsg == "Error") {
                        swal('Password Not Changed', 'Failed');
                        $state.reload();
                    }
                })
            }
        };

        vm.lock_disable = function (data) {
            var data = {
                "Username": $scope.uName,
                "password": data.pwd
            }
            apiService.get("Login", data).then(function (promise) {
                if (promise.userId != 0) {

                    $state.go('app.homepage');
                }
                else {
                    swal(promise.message);
                    //$state.reload();
                    $scope.vm.getLockUser.pwd = "";
                }
            });
        };
        $scope.SelectedFileForUploadz = [];
        $scope.selectFileforUploadz = function (input) {
            $scope.SelectedFileForUploadz = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#blahh')
                        .attr('src', e.target.result)
                        .width(133)
                        .height(120);
                };
                reader.readAsDataURL(input.files[0]);
                Uploadprofile();
            } else if (input.files[0].type != "image/jpeg") {
                swal("Please Upload the JPEG file");
                return;
            } else if (input.files[0].size > 2097152) {
                swal("Image size should be less than 2MB");
                return;
            }
        }
        $scope.SelectedFileForUploadzc = [];
        $scope.selectFileforUploadzc = function (input) {
            $scope.SelectedFileForUploadzc = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#blahhc')
                        .attr('src', e.target.result)
                        .width(133)
                        .height(120);
                };
                reader.readAsDataURL(input.files[0]);
                Uploadprofilec();
            } else if (input.files[0].type != "image/jpeg") {
                swal("Please Upload the JPEG file");
                return;
            } else if (input.files[0].size > 2097152) {
                swal("Image size should be less than 2MB");
                return;
            }
        }
        function Uploadprofile() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepicpreadmission", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    onlineprofilepicpath = d;
                    //$scope.reg.PASR_Student_Pic_Path = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        function Uploadprofilec() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadzc.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzc[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepicpreadmission", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    // swal(d);
                    onlineprofilepicpath = d;
                    //$scope.reg.PASR_Student_Pic_Path = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        function Uploads() {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }
            formData.append("Id", pasR_Id);
        };

        $scope.getOTP = function (data) {
            var mobno = {
                "mobileNo": data.mobileNo,
                "UserName": data.userName
            }
            apiService.create("Login/getOTP", mobno).then(function (promise) {
                swal(promise);
            })
        }

        $scope.pamentsave = function () {
            var payu_URL = $scope.payu_URL;
            var url = payu_URL;
            var method = 'POST';
            var params = {
                "Key": $scope.key,
                "txnid": $scope.txnid,
                "amount": $scope.amount,
                "productinfo": $scope.productinfo,
                "firstname": $scope.firstname,
                "email": $scope.email,
                "phone": $scope.phone,
                "surl": $scope.surl,
                "furl": $scope.furl,
                "hash": $scope.hash,
                "udf1": $scope.udf1,
                "udf2": $scope.udf2,
                "udf3": $scope.udf3,
                "udf4": $scope.udf4,
                "udf5": $scope.udf5,
                "udf6": $scope.udf6,
                "service_provider": $scope.service_provider,
                "hash_string": $scope.hash_string
            }
            FormSubmitter.submit(url, method, params);
        }

        $scope.submitted = false;
        vm.register = function (data) {
            if ($scope.vm.registrationForm.$valid) {
                var vm = this;
                vm.getUser = {};
                vm.setUser = {};
                if (data.Confirmpassword == data.Password) {
                    var formData = new FormData();
                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);
                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);
                    $http.post("/api/Login/paynow", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            //if (response.data != "" && response.data != "created") {
                            //    swal(response.data);
                            //}
                            //else if (response.data == "created") {
                            //    swal("Account created successfully");
                            //    vm.signIn = true;
                            //    $scope.vm.getRegUser = "";
                            //    $scope.vm.getRegUser.Name = "";
                            //    $scope.vm.getRegUser.Email_id = "";
                            //    $scope.vm.getRegUser.Mobile = "";
                            //    $scope.vm.getRegUser.Username = "";
                            //    $scope.vm.getRegUser.Password = "";
                            //    $scope.vm.getRegUser.Confirmpassword = "";
                            //    vm.DrawCaptcha();
                            //    $state.go('Login');
                            //}
                            //else {
                            //    swal(response.data);
                            //}
                            if (response.data.description == "Username Already exist .. !!" || response.data.description == "Email id Already exist .. !!") {
                                swal(response.data.description);
                                //$state.reload();
                            }
                            else {
                                if (response.data.paymentDetailsList != null) {
                                    $scope.PASR_FirstName = response.data.paymentDetailsList[0].firstname;
                                    $scope.PASR_emailId = response.data.paymentDetailsList[0].email;
                                    $scope.PASR_MobileNo = response.data.paymentDetailsList[0].phone;
                                    $scope.Pasr_amount = response.data.paymentDetailsList[0].amount;
                                    $scope.key = response.data.paymentDetailsList[0].marchanT_ID;
                                    $scope.txnid = response.data.paymentDetailsList[0].trans_id;
                                    $scope.amount = response.data.paymentDetailsList[0].amount;
                                    $scope.productinfo = response.data.paymentDetailsList[0].productinfo;
                                    $scope.firstname = response.data.paymentDetailsList[0].firstname;
                                    $scope.email = response.data.paymentDetailsList[0].email;
                                    $scope.phone = response.data.paymentDetailsList[0].phone;
                                    $scope.surl = response.data.paymentDetailsList[0].surl;
                                    $scope.furl = response.data.paymentDetailsList[0].furl;
                                    $scope.hash = response.data.paymentDetailsList[0].hash;
                                    $scope.udf1 = response.data.paymentDetailsList[0].udf1;
                                    $scope.udf2 = response.data.paymentDetailsList[0].udf2;
                                    $scope.udf3 = response.data.paymentDetailsList[0].udf3;
                                    $scope.udf4 = response.data.paymentDetailsList[0].udf4;
                                    $scope.udf5 = response.data.paymentDetailsList[0].udf5;
                                    $scope.udf6 = response.data.paymentDetailsList[0].udf6;
                                    $scope.service_provider = response.data.paymentDetailsList[0].service_provider;
                                    $scope.hash_string = response.data.paymentDetailsList[0].hash_string;
                                    $scope.payu_URL = response.data.paymentDetailsList[0].payu_URL;
                                    $scope.PaymentMode = true;
                                    $scope.ProspectuseScreen = false;
                                    swal.close();
                                    showConfirmButton: false
                                }
                                else {
                                    swal("Please Contact Administrator!!");
                                    $state.reload();
                                }
                            }
                        })
                }
                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.GetpaymentDetails = function (payobj, data) {
            var data = {
                "Donation_Amount": 1,
                "AlumniName": data.Name,
                "Email_id": data.Email_id,
                "Mobile": data.Mobile,
                "admission": data.admission1,
                "FPGD_PGName": payobj,
                "MI_Idnew": $scope.MI_Idnew,
            }
            apiService.create("Login/GetpaymentDetails", data).then(function (promise) {
                if (promise.paymentgateway > 0 || promise.paymentgateway != null) {
                    $scope.paymentgateway = promise.paymentgateway
                    $scope.FMA_Amount = promise.alumnifee[0].FMA_Amount;
                    $scope.ASMAY_Id = promise.alumnifee[0].ASMAY_Id;
                    $scope.MI_Id = promise.paymentgateway[0].mI_Id;
                    $scope.SaltKey = $scope.paymentgateway[0].fpgD_SaltKey;
                    $scope.orderId = promise.orderId;
                    $scope.paygtw = payobj;
                    $scope.institutioname = promise.institution[0].mI_Name;
                    $scope.logo = promise.institution[0].mI_Logo;
                    $scope.wordamount = toWordsconver($scope.FMA_Amount);
                }
            })
        }
        $scope.razorpay = function (data) {
            var options = {
                "key": $scope.SaltKey,
                "amount": $scope.FMA_Amount * 100,
                //"amount": 200,
                "name": $scope.institutioname,
                "order_id": $scope.orderid,
                "currency": "INR",
                "description": "ONLINE PAYMENT",
                //"image": "https://bdcampusstrg.blob.core.windows.net/files/Razorpay.png",
                "image": $scope.logo,
                "handler": function (response) {
                    $scope.paymentid = response.razorpay_payment_id;
                    if ($scope.vm.registrationForm.$valid) {
                        var vm = this;
                        vm.getUser = {};
                        vm.setUser = {};
                        if ($scope.Confirmpassword == $scope.Password) {
                            var data1 = {
                                "Name": data.Name,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ReceiptNo": response.razorpay_payment_id,
                                "orderId": $scope.orderId,
                                "Donation_Amount": $scope.FMA_Amount,
                                "onlinepreadminfilepath": onlineprofilepicpath,
                                "Email_id": data.Email_id,
                                "Mobileno": data.Mobile,
                                "Username": data.Username,
                                "admittedyer": data.asmaY_Id,
                                "leftyear": data.asmaY_Idleft,
                                "leftclassid": data.classid,
                                "classidadmitted": data.classidadmitted,
                                "Password": $scope.Confirmpassword,
                                "admission": data.admission1,
                                "Otpmobl": $scope.moboto,
                                "Otpemail": $scope.emailboto,
                                "Alumni": 'Alumni',
                                "paygtw": $scope.paygtw
                            };
                            apiService.create("Login/paymentsave", data1).then(function (promise) {
                                if (promise.returnval == "true") {
                                    //swal({
                                    //    title: "Confirmed!",
                                    //    text: "Your Donation Payment Confirmed",
                                    //    type: "success"
                                    //})
                                    swal('Account Created Successfully,Thank You')
                                }
                                else {
                                    swal("sorry!", "Your Donation Payment is not Confirmed.", "Error");
                                }
                                $state.reload();
                            })
                        }
                        else {
                            swal("Password Mismatch..!");
                            return;
                        }
                    }
                    else {
                        $scope.submitted = true;
                    }
                },
                "prefill": {
                    "name": data.Name,
                    "email": data.Email_id,
                    "contact": data.Mobileno
                },
                "theme": {
                    "color": "#F37254"
                },
            };
            var rzp1 = new Razorpay(options);
            rzp1.open();
            e.preventDefault();
        }
        $scope.submitted = false;
        vm.registeralumni = function (data) {
            if ($scope.vm.registrationFormalumni.$valid) {
                $scope.Confirmpassword = data.Confirmpassword;
                $scope.Password = data.Password;
                var dt = {
                    "ALUReg": 'AlumniRegister'
                }
                apiService.create("Login/getgateway", dt).then(function (promise) {
                    if (promise.gencon[0].ivrmgC_AlumniRegCompFlg == true) {
                        if (promise.fillpaymentgateway != null || promise.fillpaymentgateway.length > 0) {
                            $scope.paymenttest = promise.fillpaymentgateway;
                            $scope.MI_Idnew = promise.fillpaymentgateway[0].mI_Idnew;
                        }
                        else {
                            swal("No Payment Gateway Mapping..!!");
                        }
                        $('#alumniregister').modal('show');
                    }
                    else {
                        if ($scope.vm.registrationForm.$valid) {
                            var vm = this;
                            vm.getUser = {};
                            vm.setUser = {};
                            if (data.Confirmpassword == data.Password) {
                                var formData = new FormData();
                                formData.append("onlinepreadminfilepath", onlineprofilepicpath);
                                formData.append("Name", data.Name);
                                formData.append("Email_id", data.Email_id);
                                formData.append("Mobileno", data.Mobile);
                                formData.append("Username", data.Username);
                                formData.append("admittedyer", data.asmaY_Id);
                                formData.append("leftyear", data.asmaY_Idleft);
                                formData.append("leftclassid", data.classid);
                                formData.append("classidadmitted", data.classidadmitted);
                                formData.append("Password", data.Password);
                                formData.append("admission", data.admission1);
                                formData.append("Otpmobl", $scope.moboto);
                                formData.append("Otpemail", $scope.emailboto);
                                formData.append("Alumni", "Alumni");
                                //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);
                                $http.post("/api/Login/Regis", formData,
                                    {
                                        withCredentials: true,
                                        headers: { 'Content-Type': undefined },
                                        transformRequest: angular.identity
                                    }).then(function (response) {
                                        if (response.data != "" && response.data != "created") {
                                            swal(response.data);
                                        }
                                        else if (response.data == "created") {
                                            swal("Registered successfully");
                                            $state.reload();
                                        }
                                        else {
                                            swal(response.data);
                                        }
                                    })
                            }
                            else {
                                swal("Password Mismatch..!");
                                return;
                            }
                        }
                        else {
                            $scope.submitted = true;
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;
        vm.registeralumnicollege = function (data) {
            if ($scope.vm.registrationForm.$valid) {
                var vm = this;
                vm.getUser = {};
                vm.setUser = {};
                if (data.Confirmpassword == data.Password) {
                    var formData = new FormData();
                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);
                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("admittedyer", data.asmaY_Id);
                    formData.append("leftyear", data.asmaY_Idleft);
                    formData.append("courseadmittted", data.courseadmittted);
                    formData.append("courseleft", data.courseleft);
                    formData.append("branchadmittted", data.branchadmittted);
                    formData.append("branchleft", data.branchleft);
                    formData.append("semadmittted", data.semadmittted);
                    formData.append("semleft", data.semleft);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    formData.append("Alumni", "Alumni");
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);
                    $http.post("/api/Login/Regis", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            if (response.data != "" && response.data != "created") {
                                swal(response.data);
                            }
                            else if (response.data == "created") {
                                swal("Registered successfully");
                                $state.reload();
                            }
                            else {
                                swal(response.data);
                            }
                        })
                }
                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.submitted = false;
        vm.registernotpay = function (data) {
            if ($scope.vm.registrationForm.$valid) {
                var vm = this;
                vm.getUser = {};
                vm.setUser = {};
                if (data.Confirmpassword == data.Password) {
                    var formData = new FormData();
                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);
                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    formData.append("Intid", data.Int);
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);
                    $http.post("/api/Login/Regis", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            if (response.data != "" && response.data != "created" && response.data != "No") {
                                swal(response.data);
                            }
                            else if (response.data == "created") {
                                swal("Account created successfully", "Please  fill the application form!!");
                               // vm.signIn = true;
                                if (vm.signIn == false) {
                                    vm.signIn = true;
                                }
                                else {
                                    vm.signIn = true;
                                }
                                $scope.vm.getRegUser = "";
                                $scope.vm.getRegUser.Name = "";
                                $scope.vm.getRegUser.Email_id = "";
                                $scope.vm.getRegUser.Mobile = "";
                                $scope.vm.getRegUser.Username = "";
                                $scope.vm.getRegUser.Password = "";
                                $scope.vm.getRegUser.Confirmpassword = "";
                                vm.DrawCaptcha();
                               // $state.go('Login');
                                $scope.vm.login(data.Username, data.Password, vm.getUser.txtCaptcha, vm.getUser.txtCaptcha)

                            }
                            else if (response.data == "No") {
                                swal("Preadmission is not open for current date", "You can't register now !!");
                                $state.reload();
                            }
                            else {
                                swal(response.data);
                            }
                        });
                }
                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.submittedc = false;
        vm.Careersregisternotpay = function (data) {
            if ($scope.vm.CareersregistrationForm.$valid) {
                var vm = this;
                vm.getUser = {};
                vm.setUser = {};
                if (data.Confirmpassword == data.Password) {
                    var formData = new FormData();
                    formData.append("onlinepreadminfilepath", onlineprofilepicpath);
                    formData.append("Name", data.Name);
                    formData.append("Email_id", data.Email_id);
                    formData.append("Mobileno", data.Mobile);
                    formData.append("Username", data.Username);
                    formData.append("Password", data.Password);
                    formData.append("Otpmobl", $scope.moboto);
                    formData.append("Otpemail", $scope.emailboto);
                    formData.append("Intid", data.Int);
                    formData.append("Special", "Careers");
                    //formData.append("transnumbconfigurationsettingsss", $scope.transnumbconfigurationsettingsassign[0]);
                    $http.post("/api/Login/Regis", formData,
                        {
                            withCredentials: true,
                            headers: { 'Content-Type': undefined },
                            transformRequest: angular.identity
                        }).then(function (response) {
                            if (response.data != "" && response.data != "created" && response.data != "No") {
                                swal(response.data);
                            }
                            else if (response.data == "created") {
                                swal("Account created successfully", "Please  fill the application form!!");
                               // vm.signIn = true;
                                if (vm.signIn == false) {
                                    vm.signIn = true;
                                }
                                else {
                                    vm.signIn = true;
                                }
                                $scope.vm.getRegUser = "";
                                $scope.vm.getRegUser.Name = "";
                                $scope.vm.getRegUser.Email_id = "";
                                $scope.vm.getRegUser.Mobile = "";
                                $scope.vm.getRegUser.Username = "";
                                $scope.vm.getRegUser.Password = "";
                                $scope.vm.getRegUser.Confirmpassword = "";
                                vm.DrawCaptcha();
                               // $state.go('Login');
                                $scope.vm.login(data.Username, data.Password, vm.getUser.txtCaptcha, vm.getUser.txtCaptcha)

                            }
                            else if (response.data == "No") {
                                swal("Preadmission is not open for current date", "You can't register now !!");
                                $state.reload();
                            }
                            else {
                                swal(response.data);
                            }
                        });
                }
                else {
                    swal("Password Mismatch..!");
                    return;
                }
            }
            else {
                $scope.submittedc = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interactedc = function (field) {
            return $scope.submittedc;
        };

        $scope.resetalldata = function () {
            $state.reload();
        }
        var th_val = ['', 'Thousand', 'Million', 'Billion', 'Trillion'];
        var dg_val = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn_val = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw_val = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
        function toWordsconver(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s))
                return 'not a number ';
            var x_val = s.indexOf('.');
            if (x_val == -1)
                x_val = s.length;
            if (x_val > 15)
                return 'too big';
            var n_val = s.split('');
            var str_val = '';
            var sk_val = 0;
            for (var i = 0; i < x_val; i++) {
                if ((x_val - i) % 3 == 2) {
                    if (n_val[i] == '1') {
                        str_val += tn_val[Number(n_val[i + 1])] + ' ';
                        i++;
                        sk_val = 1;
                    } else if (n_val[i] != 0) {
                        str_val += tw_val[n_val[i] - 2] + ' ';
                        sk_val = 1;
                    }
                } else if (n_val[i] != 0) {
                    str_val += dg_val[n_val[i]] + ' ';
                    if ((x_val - i) % 3 == 0)
                        str_val += 'hundred ';
                    sk_val = 1;
                }
                if ((x_val - i) % 3 == 1) {
                    if (sk_val)
                        str_val += th_val[(x_val - i - 1) / 3] + ' ';
                    sk_val = 0;
                }
            }
            if (x_val != s.length) {
                var y_val = s.length;
                str_val += 'point ';
                for (var i = x_val + 1; i < y_val; i++)
                    str_val += dg_val[n_val[i]] + ' ';
            }
            return str_val.replace(/\s+/g, ' ');
        }
        //Added on 02-02-2018.
        $scope.forgotpwd = function () {
            $scope.clickedlinkname = "forgotpwd";
            $scope.ForgetEmailInput = false;
            $scope.EmailMobileConform = false;
        }
        $scope.forgotname = function () {
            $scope.clickedlinkname = "forgotname";
            $scope.ForgetEmailInput = true;
            $scope.EmailMobileConform = true;
        }
    }]);
