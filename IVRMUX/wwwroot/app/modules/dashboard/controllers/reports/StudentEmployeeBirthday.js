

(function () {
    'use strict';
    angular
.module('app')
.controller('studentbirthdayController1', studentbirthdayController1)

    studentbirthdayController1.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'superCache', 'Excel', '$timeout', '$filter']
    function studentbirthdayController1($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, superCache, Excel, $timeout, $filter) {
        $scope.searchValue = "";
        $scope.tadprint = false;
        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.students = [];
        $scope.printstudents = [];
        $scope.currentPage = 1;
        // $scope.itemsPerPage = 10;
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.obj = {};

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

        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.printData = function (printSectionId) {
          var data = "grid_print";
            var innerContents = document.getElementById(data).innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet"  href="css/print/Admission/Studentempdetails/StudentBirthdayPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


        $scope.loadyear = function () {
            apiService.getURI("studentbirthday/getYear", 1).
               then(function (promise) {
                   $scope.yeardrpdwn = promise.accyear;
               })
        }
        $scope.monthdays = function () {
            if ($scope.all1 == 1) {
                $scope.days1 = "";
                $scope.days2 = "";
            }
            else if ($scope.all1 == 0) {
                $scope.days = "";
            }
        }
        $scope.searchValue = "";
        $scope.submitted = false;
        $scope.ShowReport = function (obj) {
            $scope.printstudents = [];
            if ($scope.myForm.$valid) {
                var fromdate1 = "";
                var todate1 = "";
                if ($scope.all1 == 1) {
                    fromdate1 = new Date(obj.fromdate).toDateString();
                    todate1 = new Date(obj.todate).toDateString();
                    $scope.obj.month = 0;
                }
                else {
                    fromdate1 = new Date().toDateString();
                    todate1 = new Date().toDateString();
                }

                var data = {
                    "months": $scope.obj.month,
                    "flag": $scope.flag,
                    "all1": $scope.all1,
                    "Fromdate": fromdate1,
                    "Todate": todate1,
                }
                apiService.create("studentbirthday/getdetailsadd", data).
          then(function (promise) {
              if (promise.count > 0) {

                  $scope.items = promise.studentDetails;
                  $scope.presentCountgrid = $scope.items.length;
                  $scope.count = $scope.items.length;
                  $scope.schooldetails = promise.schooldetails;
                  $scope.img = $scope.schooldetails[0].asC_Logo_Path;
              }
              else {
                  swal("No Records Found");
                  $scope.count = 0;
              }
          })
            }
            else {
                $scope.submitted = true;
            }
        };

    }

})();