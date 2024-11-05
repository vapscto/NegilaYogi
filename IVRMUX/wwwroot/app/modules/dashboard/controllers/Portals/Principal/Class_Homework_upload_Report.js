(function () {
    'use strict';
    angular
        .module('app')
        .controller('Class_Homework_upload_ReportController', Class_Homework_upload_ReportController)

    Class_Homework_upload_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function Class_Homework_upload_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.searchValue = '';

        $scope.AttRptDropdownList = function () {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = 10;
            $scope.printstudents = [];
            $scope.tt = true;
            //apiService.get("CareerReport/getdetails/").then(function (promise) {

            //    $scope.yearDropdown = promise.academicList;
            //    $scope.classDropdown = promise.classlist;
            //    $scope.sectionDropdown = promise.sectionList;


            //});
        }
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        
        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;



        $scope.onclickloaddata = function () {
        };

        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.catreport = true;
        $scope.submitted = false;

        $scope.columnTotal = [];
        $scope.angularData =
            {
                'nameList': []
            };

        $scope.vals = [];
        $scope.getreport = function () {
            $scope.printstudents = [];
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                var data = {
                    "upload_flg": $scope.homeclass
                }
                apiService.create("CareerReport/get_home_classwork", data)
                    .then(function (promise) {
                         if (promise.home_class_work_reports.length) {
                            $scope.home_class_work_reports = promise.home_class_work_reports;
                             $scope.presentCountgrid = promise.home_class_work_reports.length;
                             if ($scope.home_class_work_reports[0].topic === '1b' ) {
                                 $scope.check_1 = false;
                             }
                             else {
                                 $scope.check_1 = true;
                             }
                             
                        }
                        else {
                             swal('No Record Found')
                            
                        }
                       
                    })

            } else {
                $scope.submitted = true;
            }
        }

     

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Clearid = function () {
            $state.reload();
            
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printData = function (printSectionId) {
            if ($scope.home_class_work_reports !== null && $scope.home_class_work_reports.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function (export_id) {

            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'WireWorkbenchDataExport');

                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
    }
})();
