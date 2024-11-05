(function () {
    'use strict';
    angular
        .module('app')
        .controller('Homework_staff_UploadReportController', Homework_staff_UploadReportController);
    Homework_staff_UploadReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter'];
    function Homework_staff_UploadReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !==null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }      
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !==null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.report = '';
        $scope.headerimg = false;
        $scope.searchValue = '';
        $scope.presentCountgrid = 0;
        $scope.imgname = logopath;
        $scope.itemsPerPage = paginationformasters;
        $scope.drop_catlist = function () {
            $scope.Section_list = [];
            var data = {
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("Homework_staff_UploadReport/get_load_onchange", data).then(function
                (promise) {
                $scope.Section_list = promise.section_list;
               
                if (promise.section_list.length == 0) {
                    $scope.submitted = false;
                    $state.reload();
                }
                //swal('Record Not Available!!!');               
            });

        };
        //$scope.change_year = function () {
        //    var data = {
        //        "ASMAY_Id": $scope.asmaY_Id               
        //    }
        //    apiService.create("Homework_staff_UploadReport/getOnchange", data).then(function (promise) {
                
        //        $scope.Select_list = promise.select_list;
               
        //    });
        //}
        $scope.isOptionsRequired1 = function () {
            return !$scope.Section_list.some(function (options) {
                return options.selected;
            });
        };
        //get data
        $scope.getAllDetail = function () {
            $scope.currentPage = 1;           
            var pageid = 2;
            apiService.getURI("Homework_staff_UploadReport/getAllDetail", pageid).then(function (promise) {
                $scope.Select_list = promise.select_list;
                           
            });
        }       
        $scope.exportToExcel = function () {
            if ($scope.getReport.length > 0) {
                var exportHref = Excel.tableToExcel(tablegrp_print, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
        };
        $scope.printData = function () {
            if ($scope.getReport.length > 0) {                
                var innerContents = document.getElementById("tablegrp_print").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
               // $scope.imgdiv = true;
                $scope.headerimg = true;
                popupWinindow.document.close();
            }
        };
        $scope.submitted = false;
        $scope.rePortdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.sectionselect = [];
                angular.forEach($scope.Section_list, function (itm) {
                    if (itm.selected) {
                        $scope.sectionselect.push({
                            ASMS_Id: itm.asmS_Id
                        });
                    }
                });
                var data = {
                    "IHW_DateFrom": new Date($scope.ismbE_FromDate).toDateString(),
                    "IHW_DateTo": new Date($scope.ismbE_ToDate).toDateString(),
                    "ASMCL_Id": $scope.ASMCL_Id,                  
                    "ASMSId_Filter": $scope.sectionselect
                }
                apiService.create("Homework_staff_UploadReport/getReport", data).then(function (promise) {
                    $scope.getReport = promise.getReport;
                    if (promise.getReport.length > 0) {
                        for (var i = 0; i < $scope.getReport.length; i++)
                         {
                            $scope.notuploadcount = (Number($scope.getReport[i].StudentsCount) - Number($scope.getReport[i].SubStudentsCount));                           
                            $scope.getReport[i].notuploadcount = $scope.notuploadcount;
                        }
                        $scope.report = true;
                        $scope.presentCountgrid = promise.getReport.length;
                    }
                    
                    else {
                        swal("Record Not Found....!!");
                        $state.reload();
                    }                   
                });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.EmailAll_Click = function () {
            if ($scope.chkemailAll === true) {
                angular.forEach($scope.getview, function (itm) {
                    itm.chkemail = true;
                });
            }
            else {
                angular.forEach($scope.getview, function (itm) {
                    itm.chkemail = false;
                });
            }           
        };
        //SMSAll_Click
        $scope.SMSAll_Click = function () {
            if ($scope.chksmslAll === true) {
                angular.forEach($scope.getview, function (itm) {
                    itm.chksms = true;
                });
            }
            else {
                angular.forEach($scope.getview, function (itm) {
                    itm.chksms = false;
                });
            }
        };
        //SMSEMAIL
        $scope.SMSEMAIL = function (cl) {
           // getview
            $scope.emailclick = [];
            $scope.smsclick = []; 
            var flagstring = "homework";
            angular.forEach($scope.getview, function (itm) {
                if (itm.chkemail == true) {
                    $scope.emailclick.push({
                        AMST_Id: itm.amsT_Id,
                        studentname: itm.studentName
                    });
                }
            });
            angular.forEach($scope.getview, function (itm) {
                if (itm.chksms == true) {
                    $scope.smsclick.push({
                        AMST_Id: itm.amsT_Id,
                        studentname: itm.studentName
                    });
                }
            });
            var data = {
                "studentsms": $scope.smsclick,
                "studentemail": $scope.emailclick,
                "flagstring": flagstring
            }
            apiService.create("Homework_staff_UploadReport/smsemail", data).then(function (promise) {
                $scope.all_checkCCCC();
                swal("Email /sms sent success fully");
            });
        };
        $scope.getView = function (cl) {
            $scope.staffname = cl.EmpName;
            $scope.subjectname = cl.ISMS_SubjectName;
            $scope.Topic = cl.IHW_Topic;
            $scope.ClassName = cl.ASMCL_ClassName;
            //ISMS_SubjectName  // IHW_Topic  //ASMCL_ClassName //ASMC_SectionName
            $scope.sectionname = cl.ASMC_SectionName;
            $scope.dateone = cl.IHW_Date;
            $scope.datetwo = cl.IHW_Date;
            $scope.Datess = cl.IHW_Date;
            var data = {
                "ASMCL_Id": cl.ASMCL_Id,
                "ASMS_Id": cl.ASMS_Id,
                "ISMS_Id": cl.ISMS_Id,
                "IHW_DateFrom": cl.IHW_Date,
                "IHW_DateTo": cl.IHW_Date
                //"IHW_DateFrom": new Date($scope.ismbE_FromDate).toDateString(),
                //"IHW_DateTo": new Date($scope.ismbE_ToDate).toDateString(),
            }
            apiService.create("Homework_staff_UploadReport/getOnchange", data).then(function (promise) {
                $scope.getview = promise.getview;
                $scope.presentCountgridtwo = promise.getview.length;
                $('#viewDeatils').modal('show');
                $scope.imgnamess = $scope.imgname;

            });
        };
        $scope.printDataModal = function () {

            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Fees/MonthEndReportPdf.css" />' +
                '<link type="text/css" media="print" href="css/print/PrintPdf_Bootstrap.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');

            $scope.headerimg = true;
            popupWinindow.document.close();

        };
        $scope.cancel = function () {
            $scope.submitted = false;
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.all_checkCCCC = function () {
            var asmS_Id = $scope.asmS_Id;
            var count = 0;
            angular.forEach($scope.Section_list, function (itm) {
                itm.selected = asmS_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        }
        $scope.togchkbxCCCC = function () {
            $scope.asmS_Id = $scope.Section_list.every(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.Section_list.some(function (options) {
                return options.selected;
            });
        }
    }
})();