(function () {
    'use strict';
    angular
        .module('app')
        .controller('Collegedepartmentcoursebranchmappingreportcontroller', Collegedepartmentcoursebranchmappingreportcontroller)

    Collegedepartmentcoursebranchmappingreportcontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel','$timeout']
    function Collegedepartmentcoursebranchmappingreportcontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {


        $scope.report = false;
        $scope.getreport1 = [];

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));

        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings !== null) {
            if (admfigsettings.length !== 0 && admfigsettings.length !== null && admfigsettings.length !== undefined) {
                logopath = admfigsettings[0].asC_Logo_Path;
            } else {
                logopath = "";
            }
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            var id = 2;
            apiService.getURI("Collegedepartmentcoursebranchmapping/Getdetailsreport", id).
                then(function (promise) {
                    $scope.deptlist = promise.deptlist;
                    $scope.courselist = promise.courselist;
                });
        };


        $scope.getreport = function () {
            $scope.report = false;
            $scope.getreport1 = [];

            if ($scope.myForm.$valid) {

                var data = {
                    "HRMD_Id": $scope.HRMD_Id,
                    "AMCO_Id": $scope.AMCO_Id
                };

                apiService.create("Collegedepartmentcoursebranchmapping/getreport", data).then(function (promise) {
                    if (promise !== null) {

                        if (promise.getreport !== null) {
                            $scope.getreport1 = promise.getreport;
                            if ($scope.getreport1.length > 0) {
                                $scope.report = true;

                                angular.forEach($scope.deptlist, function (dd) {

                                    if (dd.hrmD_Id === parseInt($scope.HRMD_Id)) {

                                        $scope.deptname = dd.hrmD_DepartmentName;
                                    }
                                });
                                
                                angular.forEach($scope.courselist, function (dd) {

                                    if (dd.amcO_Id === parseInt($scope.AMCO_Id)) {

                                        $scope.coursename = dd.amcO_CourseName;
                                    }
                                });


                            } else {
                                $scope.report = false;
                            }

                        } else {
                            swal("No Records Found");
                            $scope.report = false;
                        }
                    } else {
                        swal("No Records Found");
                        $scope.report = false;
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'DepartmentCourseBranchSemesterReport');
            $timeout(function () {
                location.href = exportHref;
            }, 100);
        };


    }

})();