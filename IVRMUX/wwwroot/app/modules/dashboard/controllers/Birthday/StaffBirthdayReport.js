

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BirthDayController', BirthDayController)

    BirthDayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'superCache', 'Excel', '$timeout', '$filter']
    function BirthDayController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, superCache, Excel, $timeout, $filter) {
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

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.staffs, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.staffs.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }
        $scope.exportToExcel = function (tableId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please select records to be exported");
            }

        }
        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
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
                swal("Please select records to be Printed");
            }
        }

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
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

        $scope.ShowReport = function (obj) {
            $scope.printstudents = [];
            $scope.staffs = [];
            var fromdate1 = "";
            var todate1 = "";
            if ($scope.all1 == 1) {
                $scope.obj.month = 0;
                fromdate1 = new Date(obj.fromdate).toDateString();
                todate1 = new Date(obj.todate).toDateString();
            }
            else {
                fromdate1 = new Date().toDateString();
                todate1 = new Date().toDateString();
            }
            $scope.searchValue = "";
            var data = {
                // "ASMAY_Id": $scope.ASMAY_Id,
                "months": $scope.obj.month, 
                //  "day": $scope.days,
                // "days1": $scope.days1,
                //   "days2": $scope.days2,
                //"flag": $scope.flag,
                "all1": $scope.all1,
                "Fromdate": fromdate1,
                "Todate": todate1,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                apiService.create("BirthDay/getdetails", data).
                    then(function (promise) {
                        if (promise.count > 0) {
                            $scope.staffs = promise.staffDetails;
                            $scope.presentCountgrid = $scope.staffs.length;
                            console.log($scope.staffs);
                            angular.forEach($scope.staffs, function (objectt) {
                                if (objectt.HRME_EmployeeFirstName.length > 0) {
                                    var string = objectt.HRME_EmployeeFirstName
                                    objectt.HRME_EmployeeFirstName = string.replace(/  +/g, ' ');
                                }
                            })
                            if (promise.staffDetails[0].HRME_Id > 0) {
                                $scope.staffBirthday = true;
                            }
                            else {
                                $scope.staffBirthday = false;
                            }
                            $scope.count = promise.count;
                        }
                        else {
                            swal("No Records Found");
                            $scope.count = 0;
                        }
                    })
            }
        };


        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#tabel1').html());
            e.preventDefault();
        });



    }

})();