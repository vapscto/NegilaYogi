(function () {
    'use strict';
    angular
        .module('app')
        .controller('AcademicCalenderReportController', AcademicCalenderReportController)

    AcademicCalenderReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function AcademicCalenderReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.report = false;
        $scope.catreport = false;

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

        $scope.BindData = function () {

            var data = {
                "MI_Id": 4
            };

            apiService.create("AcademicCalenderReport/getdetails", data).
                then(function (promise) {
                    $scope.academicYearList = promise.getyear;
                });
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        $scope.getreport = function (obj) {
            $scope.albumNameArraycolumn = [];
            $scope.all = false;
            $scope.getdetails = [];
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id
                };

                apiService.create("AcademicCalenderReport/getreport", data).
                    then(function (promise) {

                        if (promise !== null) {
                            $scope.temp = [];
                            $scope.tempdetails1 = [];

                            $scope.getdetails = promise.getdetails;
                            if ($scope.getdetails.length > 0) {

                                $scope.catreport = true;

                                $scope.getmonthyeardetails = promise.getmonthyeardetails;

                                angular.forEach($scope.getmonthyeardetails, function (dd) {
                                    $scope.tempdetails1 = [];
                                    angular.forEach($scope.getdetails, function (d) {

                                        if (dd.monthid == d.monthid) {
                                            $scope.tempdetails1.push(d);
                                        }
                                    });

                                    $scope.temp.push({ monthid: dd.monthid, monthyear: dd.monthyear, yearname: dd.yearname, tempdetails: $scope.tempdetails1 });
                                });

                                console.log($scope.temp);

                            } else {
                                swal("No Records Found");
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };




        $scope.printData = function (printSectionId) {
            var innerContents = document.getElementById("print").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/NaacReportFeedback/AcademicCalanderReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        };



        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };
    }

})();