(function () {
    'use strict';
    angular.module('app').controller('NDS_JrSrKG_ProgressCardReportController', NDS_JrSrKG_ProgressCardReportController)
    NDS_JrSrKG_ProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function NDS_JrSrKG_ProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails_temp = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.datesuf1 = "";
        var frommonth = "";
        $scope.imgname = "";
        $scope.grandeTotal = true;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        }
        else {
            $scope.imgname = "";
        }

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
            // $scope.maxDatedof = new Date($scope.today);

        };

        $scope.getserverdate();

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
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
            $scope.class_list = [];
            $scope.termlist = [];
            $scope.getstudentlist = [];
            $scope.AMST_Id = "";
            $scope.getexamlist = [];
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
            $scope.getstudentlist = [];
            $scope.AMST_Id = "";
            $scope.getexamlist = [];
            $scope.section_list = [];
            $scope.ASMS_Id = "";
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        $scope.onsectionchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.AMST_Id = "";
            $scope.getexamlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "flagtype": "NDSJrKGSrKg"
            };
            apiService.create("JSHSExamReports/GetStudentDetails", data).then(function (promise) {
                $scope.getstudentlist = promise.getstudentlist;
                $scope.getexamlist = promise.getexam;
                if ($scope.getexamlist === null || $scope.getexamlist.length === 0) {
                    swal("Exam Details Not Found For This Selection");
                }
            });
        };


        $scope.isOptionsRequired1 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };

        $scope.Onchangestudent = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];

        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.examwiseremarks = [];
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.submitted = true;
            $scope.getexamwisetotaldetails = [];
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                $scope.selectedexamlist = [];

                angular.forEach($scope.getexamlist, function (dd) {
                    if (dd.EME_Id) {
                        $scope.selectedexamlist.push({ EME_Id: dd.emE_Id, EME_ExamName: dd.emE_ExamName });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "examlist": $scope.selectedexamlist
                };

                apiService.create("JSHSExamReports/nds_JrSrKG_report", data).then(function (promise) {


                    $scope.JSHSReport = true;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    $scope.getstudentwisesubjectsubsubjectlist = promise.getstudentwisesubjectsubsubjectlist;
                    $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;
                    $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                    $scope.clstchname = promise.clstchname;
                    $scope.getexamwisetotaldetails = promise.getexamwisetotaldetails;
                    if ($scope.clstchname !== null && $scope.clstchname.length > 0) {
                        $scope.classteachername = $scope.clstchname[0].hrmE_EmployeeFirstName;
                    }

                    $scope.stuname = $scope.studentdetails[0].studentname;
                    $scope.asmcL_ClassName = $scope.studentdetails[0].classname;
                    $scope.asmC_SectionName = $scope.studentdetails[0].sectionname;
                    $scope.amaY_RollNo = $scope.studentdetails[0].rollno;
                    $scope.dob = $scope.studentdetails[0].dob;
                    $scope.ASMAY_Year = $scope.studentdetails[0].ASMAY_Year;
                    $scope.amsT_PerStreet = $scope.studentdetails[0].AMST_PerStreet;
                    $scope.amsT_PerArea = $scope.studentdetails[0].AMST_PerArea;
                    $scope.amsT_PerCity = $scope.studentdetails[0].AMST_PerCity;
                    $scope.ivrmmS_Name = $scope.studentdetails[0].ivrmms_name;
                    $scope.ivrmmC_CountryName = $scope.studentdetails[0].IVRMMC_CountryName;
                    $scope.addressd1 = $scope.studentdetails[0].addressd1;
                    $scope.amsT_PerPincode = $scope.studentdetails[0].amsT_PerPincode;
                    $scope.mobileno = $scope.studentdetails[0].mobileno;
                    $scope.AMST_Photoname = $scope.studentdetails[0].AMST_Photoname;
                    $scope.amst_dob = new Date($scope.studentdetails[0].amst_dob);

                    $scope.getdate = $scope.amst_dob.getDate();
                    $scope.getyear = $scope.amst_dob.getFullYear();
                    var months = $scope.amst_dob.getMonth();

                    $scope.index = $scope.ordinal_suffix_of1($scope.getdate);
                    $scope.getmonth = $scope.getmontnames(months);

                    $scope.studenwisesubjects = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.studenwisesubjects = [];
                        angular.forEach($scope.getstudentwisesubjectlist, function (stusubj) {
                            if (stu.AMST_Id === stusubj.AMST_Id) {
                                $scope.studenwisesubjects.push(stusubj);
                            }
                        });
                        stu.studentsubjects = $scope.studenwisesubjects;
                    });
                    $scope.temp_subsubject = [];
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.temp_subsubject = [];
                        angular.forEach(stu.studentsubjects, function (stu_subj) {
                            $scope.temp_subsubject = [];
                            angular.forEach($scope.getstudentwisesubjectsubsubjectlist, function (subsubj) {
                                if (subsubj.ISMS_Id === stu_subj.ISMS_Id) {
                                    $scope.temp_subsubject.push(subsubj);
                                }
                            });
                            stu_subj.subsubject = $scope.temp_subsubject;
                        });
                    });


                    //Attendance Details
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.temp_attednancd = [];
                        angular.forEach($scope.getstudentwiseattendancedetails, function (stu_att) {
                            if (stu.AMST_Id === stu_att.AMST_Id) {
                                $scope.temp_attednancd.push(stu_att);
                            }
                        });
                        stu.attendance = $scope.temp_attednancd;
                    });

                    //Marks Details
                    angular.forEach($scope.studentdetails, function (stu) {
                        $scope.temp_marks = [];
                        angular.forEach($scope.getstudentmarksdetails, function (stu_att) {
                            if (stu.AMST_Id === stu_att.AMST_Id) {
                                $scope.temp_marks.push(stu_att);
                            }
                        });
                        stu.studentmarks = $scope.temp_marks;
                    });


                    $scope.getpromotionremarksdetails = promise.getpromotionremarksdetails;
                    if ($scope.getpromotionremarksdetails !== null && $scope.getpromotionremarksdetails.length > 0) {
                        $scope.promotedclass = $scope.getpromotionremarksdetails[0].eprD_Remarks;
                    }
                    $scope.examwiseremarks = promise.examwiseremarks;
                    console.log($scope.studentdetails);
                });
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/NDS/NDS_1_5_ReportCardPdf.css" />' +
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

        $scope.ordinal_suffix_of1 = function (datesufix1) {
            var j = datesufix1 % 10,
                k = datesufix1 % 100;
            if (j == 1 && k != 11) {
                $scope.datesuf1 = "st";
                return $scope.datesuf1;
            }
            if (j == 2 && k != 12) {
                $scope.datesuf1 = "nd";
                return $scope.datesuf1;
            }
            if (j == 3 && k != 13) {
                $scope.datesuf1 = "rd";
                return $scope.datesuf1;
            }
            $scope.datesuf1 = "th";
            return $scope.datesuf1;
        };

        //month names
        $scope.getmontnames = function (monthid) {
            frommonth = "";
            switch (monthid) {

                case 0:
                    frommonth = "JANUARY";
                    break;
                case 1:
                    frommonth = "FEBRUARY";
                    break;
                case 2:
                    frommonth = "MARCH";
                    break;
                case 3:
                    frommonth = "APRIL";
                    break;
                case 4:
                    frommonth = "MAY";
                    break;
                case 5:
                    frommonth = "JUNE";
                    break;
                case 6:
                    frommonth = "JULY";
                    break;
                case 7:
                    frommonth = "AUGUST";
                    break;
                case 8:
                    frommonth = "SEPTEMBER";
                    break;
                case 9:
                    frommonth = "OCTOBER";
                    break;
                case 10:
                    frommonth = "NOVEMBER";
                    break;
                case 11:
                    frommonth = "DECEMBER";
                    break;
                default:
                    frommonth = "";
                    break;
            }
            return frommonth;
        };
    }
})();