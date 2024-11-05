(function () {
    'use strict';
    angular.module('app', ['ngSanitize']).controller('CBS_MultiplReportController', CBS_MultiplReportController)

    CBS_MultiplReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout',]
    function CBS_MultiplReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, ) {

        $scope.promotionwiseremarks = false;
        $scope.examwiseremarks = false;
        $scope.Flag = "all";
        $scope.submitted = false;
        $scope.Left_Flag = true;
        $scope.Deactive_Flag = true;
        $scope.studentlist = [];
        $scope.configuration = [];
        $scope.getsubjectlist = [];
        $scope.reportdata = [];
        $scope.subjectwisetotal = [];
        $scope.studentwisetotal = [];
        $scope.getsubjectgrouplist = [];
        $scope.studentlistdetails = [];
        $scope.getstudentmarksdetails_temp = [];
        $scope.getreportdetails = true;
        $scope.details_report = true;
        $scope.subjectrank = true;
        $scope.Left_FlagAverage = false;
        var paginationformasters = '';
        var ivrmcofigsettings = [];
        var count = 0;
        var copty;
        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }




        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];

        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var logopath = "";
        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        $scope.SuperAverage = function () {
            if ($scope.Left_FlagAverage == true) {
                $scope.empG_GroupName = "";
            }
        }
        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("PromotionReportDetails/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
            });
        };
        $scope.fonts = [
            { name: '10px', size: '10px ', class: 'font10' },
            { name: '11px', size: '11px ', class: 'font11' },
            { name: '12px', size: '12px ', class: 'font12' },
            { name: '13px', size: '13px ', class: 'font13' },
            { name: '14px', size: '14px ', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' },
            { name: '18px', size: '18px', class: 'font18' },
            { name: '25px', size: '25px', class: 'font25' }
        ];


        $scope.print_flag = true;

        $scope.onchangeyear = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_GroupName = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("PromotionReportDetails/onchangeyear", data).then(function (Promise) {
                $scope.classlist = Promise.allclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_GroupName = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("PromotionReportDetails/onchangeclass", data).then(function (promise) {
                $scope.sectionlist = promise.allsectionlist;
            });
        };

        $scope.onchangesection = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.studentlistdetails = [];
            $scope.getstudentmarksdetails_temp = [];
            $scope.empG_GroupName = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("PromotionReportDetails/onchangesection", data).then(function (promise) {

                if (promise !== null) {
                    $scope.studentlistdetails = promise.studentlistdetails;
                    $scope.subjectwisetotal = promise.subjectwisetotal;

                    $scope.all = true;
                    angular.forEach($scope.studentlistdetails, function (dd) {
                        dd.checkedsub = true;
                    });
                }
            });
        };

        $scope.OnChangeLeftFlag = function () {
            $scope.getreportdetails = false;
            $scope.details_report = false;
            $scope.studentlist = [];
            $scope.configuration = [];
            $scope.getsubjectlist = [];
            $scope.reportdata = [];
            $scope.subjectwisetotal = [];
            $scope.studentwisetotal = [];
            $scope.getsubjectgrouplist = [];
            $scope.getstudentmarksdetails_temp = [];
            $scope.studentlistdetails = [];
            $scope.empG_GroupName = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("PromotionReportDetails/onchangesection", data).then(function (promise) {
                $scope.studentlistdetails = promise.studentlistdetails;

                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };

        $scope.saveddata = function (obj) {
            $scope.empG_DistplayNametemp = "";
            $scope.JSHSReport = false;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getstudentwiseattendancedetails = [];
            $scope.submitted = true;
            $scope.studentwisemarks = [];
            $scope.studentdetails = [];
            $scope.getexamwisetotaldetails = [];
            $scope.ExamWise_PaperType = [];
            $scope.getparticipatedetails = [];
            $scope.Accdemic = "";
            if ($scope.myForm.$valid) {
                $scope.termlisttemp = [];
                $scope.Temp_AmstId = [];

                angular.forEach($scope.studentlistdetails, function (dd) {
                    if (dd.checkedsub) {
                        $scope.Temp_AmstId.push({ AMST_Id: dd.amsT_Id });
                    }
                });
                angular.forEach($scope.yearlist, function (dd) {
                    if (dd.asmaY_Id == $scope.asmaY_Id) {
                        $scope.Accdemic = dd.asmaY_Year;
                    }
                });
                angular.forEach($scope.classlist, function (dd) {
                    if (dd.asmcL_Id == $scope.asmcL_Id) {
                        $scope.classs = dd.asmcL_ClassName;
                    }
                });
                angular.forEach($scope.sectionlist, function (dd) {
                    if (dd.asmS_Id == $scope.asmS_Id) {
                        $scope.clasection = dd.asmC_SectionName;
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "Temp_AmstIds": $scope.Temp_AmstId,
                    "EMPG_GroupName": $scope.empG_GroupName,
                    "flag": "cumulative",
                };

                apiService.create("JSHSExamReports/PromotionReportI_IV", data).then(function (promise) {
                    $scope.JSHSReport = true;
                    $scope.studentdetails = promise.getstudentdetails;
                    $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                    //attendence
             
                   

                    if ($scope.getstudentwisesubjectlist != null && $scope.getstudentwisesubjectlist.length > 0) {
                        $scope.employeeid = [];
                        $scope.employeeid = [];
                        angular.forEach($scope.getstudentwisesubjectlist, function (dev) {
                            if ($scope.employeeid.length === 0) {
                                $scope.employeeid.push(dev);
                            }
                            else if ($scope.employeeid.length > 0) {
                                var intcount = 0;
                                angular.forEach($scope.employeeid, function (emp) {
                                    if (emp.ISMS_SubjectName === dev.ISMS_SubjectName) {
                                        intcount += 1;
                                    }
                                });
                                if (intcount === 0) {
                                    if (dev.ESG_IdNew != 0 && dev.colspan > 0) {
                                        $scope.employeeid.push(dev);
                                    }
                                    if (dev.GropuFlag == 0) {
                                        $scope.employeeid.push(dev);
                                    }


                                }
                            }
                        })




                    }
                    $scope.TotalApplicable = []; $scope.TotalApplicableNew = [];
                    if ($scope.employeeid != null && $scope.employeeid.length > 0) {

                        angular.forEach($scope.employeeid, function (emp) {
                            $scope.TotalApplicable.push({
                                Column: "TOTAL",
                                ISMS_Id: emp.ISMS_Id
                            })
                            $scope.TotalApplicable.push({
                                Column: "AVERAGE",
                                ISMS_Id: emp.ISMS_Id
                            })

                        });
                    }
                    //
                    $scope.MarksTotal = [];
                    if ($scope.TotalApplicable != null && $scope.TotalApplicable.length > 0) {

                        angular.forEach($scope.TotalApplicable, function (emp) {
                            angular.forEach($scope.getstudentwisesubjectlist, function (dev) {
                                if (dev.ISMS_Id == emp.ISMS_Id && (dev.GropuFlag == false || dev.colspan > 0)) {
                                    $scope.MarksTotal.push({
                                        ISMS_Id: emp.ISMS_Id,
                                        Column: emp.Column,
                                        YeralyGroupObtMarks: dev.YeralyGroupObtMarks,
                                        ESTMPPSG_GroupObtMarks: dev.ESTMPPSG_GroupObtMarks,
                                        AMST_Id: dev.AMST_Id,
                                        GroupMarks: dev.GroupMarks,
                                        YearlyMarks: dev.YearlyMarks,
                                        ESG_IdNew: dev.ESG_IdNew,
                                        GropuFlag: dev.GropuFlag,
                                        EYCES_MarksDisplayFlg: dev.EYCES_MarksDisplayFlg,
                                        EYCES_GradeDisplayFlg: dev.EYCES_GradeDisplayFlg
                                    })
                                }
                            });
                        })


                    }

                    //adedd By 
                    $scope.getgradelist = [];
                    $scope.getgradelist = promise.getgradelist;

                });
            } else {
                $scope.submitted = true;
            }
        };



        $scope.printToCart = function () {

            var innerContents = document.getElementById("HHS02").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/InvoicePdf.css" />' +                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');            popupWinindow.document.close();

        }
        $scope.exportToExcel = function (tableIds) {

            var excelname = "Tabulation Sheet ";
            var exportHref = Excel.tableToExcel(tableIds, 'Tabulation Sheet');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.termChange = function () {
            $scope.getstudentmarksdetails_temp = [];
        }
        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlistdetails, function (dd) {
                dd.checkedsub = $scope.all;
            });
            $scope.getstudentmarksdetails_temp = [];
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlistdetails.every(function (itm) { return itm.checkedsub; });
            $scope.getstudentmarksdetails_temp = [];
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlistdetails.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();