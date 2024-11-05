(function () {
    'use strict';
    angular.module('app').controller('GeneralSendSMSSRKVSController', GeneralSendSMSSRKVSController)
    GeneralSendSMSSRKVSController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function GeneralSendSMSSRKVSController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.getstudentlist = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.searchValue = "";
        $scope.GradeWise = true;
        $scope.obj.snd_sms = false;
        $scope.obj.snd_email = false;

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

            $scope.generateddate = new Date($scope.today);
        };

        $scope.getserverdate();

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };
        $scope.onsectionchange = function () {
            $scope.grade_list = [];
            $scope.obj.EMGR_Id = "";
            $scope.getexamlist = [];
            $scope.JSHSReport = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("JSHSExamReports/get_Exam_grade", data).then(function (promise) {
                $scope.grade_list = promise.getallgrade;
                $scope.getexamlist = promise.getexam;

                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.termlistd = [];
            $scope.studentlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.class_list = [];
            $scope.termlist = [];
            $scope.AMST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_classes", data).then(function (promise) {
                $scope.class_list = promise.getclasslist;
            });
        };

        $scope.onclasschange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.termlistd = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.AMST_Id = "";
            $scope.section_list = [];
            $scope.getstudentlist = [];
            $scope.ASMS_Id = "";
            $scope.searchValue = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.obj.all;
            angular.forEach($scope.employeeid, function (itm) {
                itm.selected = toggleStatus;

            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.optionToggled1 = function () {

            $scope.all = $scope.stumarkdetails.every(function (options) {

                return options.selected;
            });


        };
        $scope.SendMSG = function () {
            $scope.studentTemp = [];
            if ($scope.employeeid != null && $scope.employeeid.length > 0) {
                angular.forEach($scope.employeeid, function (itm) {
                    if (itm.selected == true) {
                        var Template = ""; var Tempmail = "";
                        var SmsMailText = ""; var Templatetwo = ""
                        for (var i = 0; i < itm.plannerdetails.length; i++) {
                            if (itm.AMST_ID == itm.plannerdetails[i].AMST_ID) {
                                if (Template != null && Template != "") {
                                    Template += ",%0a" + itm.plannerdetails[i].SMS;
                                    Templatetwo += ",<br/>" + itm.plannerdetails[i].SMS;
                                }
                                else {
                                    Template += itm.plannerdetails[i].SMS;
                                    Templatetwo += itm.plannerdetails[i].SMS;
                                }

                            }
                        }
                       // Tempmail = "Dear Parent ,%0a Name:" + itm.AMST_Firstname + "%0a Reg No :" + itm.AMST_AdmNo + " %0a Exam: Annual Exam IX  \n\n\n" + Template + "\n" + itm.Total + "\n Percentage:" + itm.Percentage + "\n Regards,\n SRKVS";
                        Tempmail = "Dear Parent ,%0a Name:" + itm.AMST_Firstname + "%0a Reg No :" + itm.AMST_AdmNo + " %0a Exam: Final Exam VIII  \n\n" + Template +  "\n Thank You SRKVS";
                        SmsMailText = "Dear Parent ,<br/>Name:   " + itm.AMST_Firstname + "<br/>Reg No :  " + itm.AMST_AdmNo + "<br/>Exam:   Final Exam VIII <br/>  " + Templatetwo +  "<br/>Thank You SRKVS";
                        $scope.studentTemp.push({
                            AMST_MobileNo: itm.AMST_MobileNo,
                            mes: Tempmail,
                            AMST_Id: itm.AMST_Id,
                            AMST_emailId: itm.AMST_emailId,
                            SmsMailText: SmsMailText
                        });
                    }


                });
            }

            if ($scope.studentTemp != null && $scope.studentTemp.length > 0) {
                swal({
                    title: "Are you sure?",
                    text: "Do you want to Send SMS ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Send it!",
                    cancelButtonText: "Cancel !",
                    closeOnConfirm: false,
                    closeOnCancel: true
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                studentlistdto: $scope.studentTemp,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "ASMCL_Id": $scope.ASMCL_Id,
                                "ASMS_Id": $scope.ASMS_Id,
                                "EME_Id": 1,
                                "radiotype": "SRkvs",
                                "snd_sms": $scope.obj.snd_sms,
                                "snd_email": $scope.obj.snd_email,

                            };
                            apiService.create("General_SendSMS/savedetail", data).then(function (promise) {
                                if (promise.smsStatus === 'sent') {

                                    swal("SMS / EMAIL  sent Sucess !");
                                }
                                else {
                                    swal("SMS / EMAIL Not  sent ")
                                }
                                $state.reload();
                            });
                        }
                        else {
                            swal("SMS And Mail Sending Cancelled.");
                        }
                    });

            }
            else {
                swal("Please select Atleast One Student !");
            }


        };
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.submitted = true;
            $scope.stumarkdetails = [];
            if ($scope.myForm.$valid) {
                var data = {};
                if ($scope.GradeWise == true) {
                    $scope.termlisttemp = [];
                    angular.forEach($scope.getexamlist, function (term) {
                        if (term.EME_Id === true) {
                            $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                        }
                    });

                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "EMGR_Id": $scope.obj.EMGR_Id,
                        "examlist": $scope.termlisttemp,
                        "ECTEX_NotApplToTotalFlg": true
                    };

                    apiService.create("JSHSExamReports/getmultiple_exam_cumulative_report", data).then(function (promise) {

                        if (promise.getcumulativereportdetails != null && promise.getcumulativereportdetails.length > 0) {
                            $scope.stumarkdetails = promise.getcumulativereportdetails;
                            $scope.employee = $scope.stumarkdetails[0].AMST_Firstname;

                            $scope.employeeid = [];

                            angular.forEach($scope.stumarkdetails, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        AMST_ID: dev.AMST_ID,
                                        AMST_Firstname: dev.AMST_Firstname,
                                        AMST_AdmNo: dev.AMST_AdmNo,
                                        AMST_MobileNo: dev.AMST_MobileNo,
                                        Total: dev.Total,
                                        Percentage: dev.Percentage,
                                        AMST_emailId: dev.AMST_emailId,

                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.AMST_ID === dev.AMST_ID) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            AMST_ID: dev.AMST_ID,
                                            AMST_Firstname: dev.AMST_Firstname,
                                            AMST_AdmNo: dev.AMST_AdmNo,
                                            AMST_MobileNo: dev.AMST_MobileNo,
                                            Total: dev.Total,
                                            Percentage: dev.Percentage,
                                            AMST_emailId: dev.AMST_emailId,

                                        });
                                    }
                                }
                            });


                            console.log($scope.employeeid);

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];

                                angular.forEach($scope.stumarkdetails, function (dd) {
                                    if (dd.AMST_ID === ddd.AMST_ID) {

                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;

                            });


                            console.log($scope.employeeid);
                            // console.log($scope.templist);
                            angular.forEach($scope.employeeid, function (dd) {
                                dd.selected = false;
                            });

                        }
                        else {
                            swal("No Record  Found..... !!");
                            $scope.stumarkdetails = "";
                        }

                    });

                }
                else {
                    data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "TotalGrade": ""

                    };

                    apiService.create("General_SendSMS/SrkvsSerach", data).then(function (promise) {

                        if (promise.studentlist != null && promise.studentlist.length > 0) {
                            $scope.stumarkdetails = promise.studentlist;
                            $scope.employee = $scope.stumarkdetails[0].AMST_Firstname;

                            $scope.employeeid = [];

                            angular.forEach($scope.stumarkdetails, function (dev) {
                                if ($scope.employeeid.length === 0) {
                                    $scope.employeeid.push({
                                        AMST_ID: dev.AMST_ID,
                                        AMST_Firstname: dev.AMST_Firstname,
                                        AMST_AdmNo: dev.AMST_AdmNo,
                                        AMST_MobileNo: dev.AMST_MobileNo,
                                        Total: dev.Total,
                                        Percentage: dev.Percentage,
                                        AMST_emailId: dev.AMST_emailId,

                                    });
                                } else if ($scope.employeeid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.employeeid, function (emp) {
                                        if (emp.AMST_ID === dev.AMST_ID) {
                                            intcount += 1;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.employeeid.push({
                                            AMST_ID: dev.AMST_ID,
                                            AMST_Firstname: dev.AMST_Firstname,
                                            AMST_AdmNo: dev.AMST_AdmNo,
                                            AMST_MobileNo: dev.AMST_MobileNo,
                                            Total: dev.Total,
                                            Percentage: dev.Percentage,
                                            AMST_emailId: dev.AMST_emailId,

                                        });
                                    }
                                }
                            });


                            console.log($scope.employeeid);

                            angular.forEach($scope.employeeid, function (ddd) {
                                $scope.templist = [];

                                angular.forEach($scope.stumarkdetails, function (dd) {
                                    if (dd.AMST_ID === ddd.AMST_ID) {

                                        $scope.templist.push(dd);
                                    }
                                });
                                ddd.plannerdetails = $scope.templist;

                            });


                            console.log($scope.employeeid);
                            // console.log($scope.templist);
                            angular.forEach($scope.employeeid, function (dd) {
                                dd.selected = false;
                            });

                        }
                        else {
                            swal("No Record  Found..... !!");
                            $scope.stumarkdetails = "";
                        }

                    });
                }

            } else {
                $scope.submitted = true;
            }
        };

        //to print
        $scope.print_HHS02 = function () {
            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/ND_6_8_ReportCardPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };





        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();