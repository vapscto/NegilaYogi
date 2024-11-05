(function () {
    'use strict';
    angular.module('app').controller('TBSchoolPrimaryController', TBSReportSchoolController)
    TBSReportSchoolController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$window']
    function TBSReportSchoolController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $window) {

        $scope.JSHSReport = false;
        $scope.getstudentmarksdetails = [];
        $scope.termlisttemp = [];
        $scope.studentlistd = [];
        $scope.getstudentlist = [];
        $scope.grade_list = [];
        $scope.termlistd = [];
        $scope.obj = {};
        $scope.imgname = "";
        $scope.grandeTotal = true;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            $scope.imgname = admfigsettings[0].asC_Logo_Path;
        }
        else {
            $scope.imgname = "";
        }
        $scope.report = true
        $scope.showtemplate = function () {
            if (value == report) {
                $scope.report = true
                $scope.templat = false

            }
            else {
                $scope.report = false
                $scope.templat = true
            }
        }


        $scope.BindData = function () {
            var pageid = 2;
            apiService.getURI("JSHSExamReports/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyearlist;
            });
        };

        $scope.onyearchange = function () {
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails = [];
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
            $scope.getstudentmarksdetails = [];
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
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("JSHSExamReports/get_sections", data).then(function (promise) {
                $scope.section_list = promise.getsectionlist;
            });
        };

        $scope.onsectionchange = function () {

            $scope.getstudentmarksdetails = [];
            $scope.termlisttemp = [];
            $scope.studentlistd = [];
            $scope.termlist = [];
            $scope.grade_list = [];
            $scope.termlistd = [];
            $scope.studentlist = [];
            $scope.getstudentlist = [];
            $scope.grade_list = [];
            $scope.AMST_Id = "";
            $scope.EMGR_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "flagtype": "Exam",

            };
            apiService.create("JSHSExamReports/GetStudentDetails", data).then(function (promise) {
                $scope.getstudentlist = promise.getstudentlist;
                $scope.grade_list = promise.getgradelist;

                $scope.all = true;
                angular.forEach($scope.getstudentlist, function (dd) {
                    dd.AMST_Ids = true;
                });
                $scope.getexamlist = promise.getexam;
                if ($scope.getexamlist !== null && $scope.getexamlist.length > 0) {
                    $scope.examlist = $scope.getexamlist;
                } else {
                    swal("No Term Is Mapped For Selected Details");
                    $scope.termlist = [];
                }
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };




        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.getstudentmarksdetails = [];
            $scope.submitted = true;
            $scope.getstudentwiseattendancedetails = [];
            $scope.examwiseremarks = [];
            $scope.getstudentwisetermwisedetails = [];
            $scope.groupwiseexamlist = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.Year = "";
            $scope.getsubjects = [];
            $scope.YearlySkillAreaStudentWise = [];
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                $scope.Temp_AmstId = [];
                $scope.termlisttemp = [];
                $scope.groupwiseexamlist = [];
                $scope.examlistgrade = [];
                $scope.clstchname = "";
                angular.forEach($scope.getexamlist, function (term) {
                    if (term.EME_Id === true) {
                        $scope.termlisttemp.push({ EME_Id: term.emE_Id, EME_ExamName: term.emE_ExamName });
                    }
                });
                angular.forEach($scope.getstudentlist, function (dd) {
                    if (dd.AMST_Ids) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });
                //
                angular.forEach($scope.year_list, function (dd) {
                    if (dd.asmaY_Id == $scope.ASMAY_Id) {
                        $scope.Year = dd.asmaY_Year;
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId,
                    "examlist": $scope.termlisttemp,
                    "EMGR_Id": 0,
                };

                apiService.create("JSHSExamReports/TBSPrimarySchool", data).then(function (promise) {
                    $scope.studentdetails = promise.getstudentdetails;        
                    $scope.clstchname = promise.clstchname[0].hrmE_EmployeeFirstName;
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    if ($scope.getstudentwisesubjectlist != null && $scope.getstudentwisesubjectlist.length > 0) {
                        $scope.employeeid = [];
                        angular.forEach($scope.getstudentwisesubjectlist, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push(dev);
                            }
                            else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.ISMS_Id === dev.ISMS_Id && emp.AMST_Id === dev.AMST_Id) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    $scope.employeeid.push(dev);


                                }
                            }
                        })

                    
                        //studentdetails
                        if (promise.getstudentwiseattendancedetails != null && promise.getstudentwiseattendancedetails.length > 0) {
                            angular.forEach($scope.studentdetails, function (dev) {
                                $scope.attendancedetails = [];
                                angular.forEach(promise.getstudentwiseattendancedetails, function (dd) {
                                    if (dev.AMST_Id == dd.AMST_Id) {
                                        $scope.attendancedetails.push(dd);
                                       
                                    }
                                });
                                dev.getstudentwiseattendancedetails = $scope.attendancedetails;
                            });
                        }
                        if (promise.examwiseremarks != null && promise.examwiseremarks.length > 0) {

                        }
                        //examwiseremarks
                       


                    }
                    else {
                        swal("Student Are Not Maped");
                    }

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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };



        $scope.template = function () {
            var innerContents = document.getElementById("template").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +
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
        $scope.isOptionsRequired2 = function () {
            return !$scope.getexamlist.some(function (options) {
                return options.EME_Id;
            });
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.getstudentlist.some(function (options) {
                return options.AMST_Ids;
            });
        };

        $scope.OnClickAll = function () {
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
            angular.forEach($scope.getstudentlist, function (dd) {
                dd.AMST_Ids = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.getstudentlist.every(function (itm) { return itm.AMST_Ids; });
            $scope.JSHSReport = false;
            $scope.studentdetails = [];
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();