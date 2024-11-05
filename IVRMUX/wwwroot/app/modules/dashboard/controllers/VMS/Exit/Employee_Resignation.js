(function () {
    'use strict';
    angular
        .module('app')
        .controller('Employee_ResignationController', Employee_ResignationController)
    Employee_ResignationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout', '$q']
    function Employee_ResignationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout, $q) {

        $scope.submitted = false;
        $scope.resignview = false;
        $scope.currentdate = new Date();
        $scope.reliving_date = new Date($scope.currentdate.getFullYear(), $scope.currentdate.getMonth(), $scope.currentdate.getDate() + 90);
        $scope.tentative_Leaving_Date = new Date($scope.reliving_date);

        $scope.CalDate = function (date1, date2) {
            var diffnew = new Date(
                date1.getDay() - date2.getDay()
            );
            var oneDay = 24 * 60 * 60 * 1000;
            var message = Math.round(Math.abs((date1.getTime() - date2.getTime()) / (oneDay)));
            return message;
        };

        $scope.Generate = function () {
            if ($scope.myForm.$valid) {
                $scope.resignview = true;
            }
            else { $scope.submitted = true; }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.loadData = function () {
            var getid = 2;
            apiService.getURI("Exit_Employee/loadEmployeeData", getid).then(function (promise) {

                if (promise.companydetails != null && promise.companydetails.length > 0) {
                    $scope.companydetails = promise.companydetails[0];
                }
                
                if (promise.employeedata !== null && promise.employeedata.length > 0) {
                    $scope.hrmE_EmployeeFirstName = promise.employeedata[0].employeename;
                    $scope.hrmE_DOJ = promise.employeedata[0].hrmE_DOJ;
                    $scope.hrmdeS_DesignationName = promise.employeedata[0].hrmdeS_DesignationName;
                    $scope.hrmE_EmailId = promise.employeedata[0].hrmE_EmailId;
                    $scope.hrmE_MobileNo = promise.employeedata[0].hrmE_MobileNo;

                    $scope.hrmE_PerStreet = promise.employeedata[0].hrmE_PerStreet;
                    $scope.hrmE_PerArea = promise.employeedata[0].hrmE_PerArea;
                    $scope.hrmE_PerCity = promise.employeedata[0].hrmE_PerCity;
                    $scope.hrmE_PerPincode = promise.employeedata[0].hrmE_PerPincode;
                }

                var joindate = new Date($filter('date')(new Date(promise.employeedata[0].hrmE_DOJ).toDateString(), "yyyy/MM/dd"));
                var leftdate = new Date($filter('date')(new Date($scope.currentdate).toDateString(), "yyyy/MM/dd"));
                var exp = $scope.CalDate(leftdate, joindate);
                $scope.duration = exp + 1;

                $scope.getdeviationreport = promise.getdeviationreport[0];
                $scope.reason_list_dd = promise.reason_list_dd;
            });
        };

        $scope.SendMail = function () {
            $scope.tmplt = [];
            var Template = document.getElementById("calltemplate").innerHTML;
            $scope.tmplt.push({ hrmE_EmployeeCode: "1", TemplateString: Template });

            if ($scope.myForm.$valid) {
                var data = {
                    "ManagerMailid": $scope.mngrEmailId,
                    "TeamLeadMailid": $scope.teamleadEmailId,
                    "ISMRESGMRE_Id": $scope.ismresgmrE_Id,
                    Template: Template
                };
                apiService.create("Exit_Employee/sendResignationMail", data).
                    then(function (promise) {
                        if (promise.retrunMsg == "Success") {
                            swal('Resignation Sent Successfully.');
                        }
                        else if (promise.retrunMsg == "Exist") {
                            swal('Duplicate Resignation Can not be Sent.');
                        }
                        else {
                            swal('Operation Failed!!');
                        }
                    });
            }
            else { $scope.submitted = true; }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("calltemplate").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
    }
})();