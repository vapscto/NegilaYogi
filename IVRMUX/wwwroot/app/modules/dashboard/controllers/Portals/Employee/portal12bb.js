(function () {
    'use strict';
    angular
        .module('app')
        .controller('portal12bbController', portal12bbController)

    portal12bbController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']

    function portal12bbController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {



        $scope.LoadData = function () {

            $scope.showweekly = false;
            $scope.showday_d = true;
            var data = {}
            apiService.getDATA("EmployeeForm12BB/getalldetails", data)
                .then(function (promise) {

                    if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {
                        $scope.leaveyeardropdown = promise.leaveyeardropdown;
                    }
                    $scope.hrmE_EmployeeFirstName = promise.empDetails[0].hrmE_EmployeeFirstName;
                    $scope.hrmE_Id = promise.empDetails[0].hrmE_Id;
                    $scope.hrme_address = promise.empDetails[0].hrme_address;
                    $scope.hrmE_PFAccNo = promise.empDetails[0].hrmE_PFAccNo;
                    $scope.hrmE_FatherName = promise.empDetails[0].hrmE_FatherName;
                    $scope.hrmE_PerCity = promise.empDetails[0].hrmE_PerCity;
                    $scope.date = new Date();
                    $scope.hrmdeS_DesignationName = promise.designation[0].hrmdeS_DesignationName;

                    $scope.Form12bb = promise.investmentdetails;

                })


        };


        $scope.onselectyear = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "IMFY_Id": $scope.IncTax.imfY_Id,

                }
             



                apiService.create("EmployeeForm12BB/getsalaryalldetails/", data).
                    then(function (promise) {
                        if (promise.retrunMsg !== "") {

                         
                            if (promise.investmentdetails !== null && promise.investmentdetails.length > 0) {
                                $scope.EmployeeDis = true;
                                $scope.finyear = promise.finyear;
                                $scope.Form12bb = promise.investmentdetails;
                                $scope.hreidO_RentPaid=$scope.Form12bb[0].hreidO_RentPaid;
                                $scope.hreidO_NameOfLandLord = $scope.Form12bb[0].hreidO_NameOfLandLord;
                                $scope.hreidO_LandLordPAN = $scope.Form12bb[0].hreidO_LandLordPAN;

                                $scope.hreidO_AddressEvidence = $scope.Form12bb[0].hreidO_AddressEvidence;

                                $scope.hreidO_TravelConcession = $scope.Form12bb[0].hreidO_TravelConcession;
                                $scope.hreidO_ConcessionEvidence = $scope.Form12bb[0].hreidO_ConcessionEvidence;

                                $scope.hreidO_InterestPaid = $scope.Form12bb[0].hreidO_InterestPaid;
                                $scope.hreidO_LenderName = $scope.Form12bb[0].hreidO_LenderName;

                                $scope.hreidO_LenderName = $scope.Form12bb[0].hreidO_LenderName;
                                $scope.hreidO_LenderAddress = $scope.Form12bb[0].hreidO_LenderAddress;
                                $scope.hreidO_LenderPAN = $scope.Form12bb[0].hreidO_LenderPAN;
                                $scope.hreidO_LandLordAddress=$scope.Form12bb[0].hreidO_LandLordAddress;

                                $scope.hreidO_FinanceInst = $scope.Form12bb[0].hreidO_FinanceInst;
                                $scope.hreidO_Employer = $scope.Form12bb[0].hreidO_Employer;
                                $scope.hreidO_Others = $scope.Form12bb[0].hreidO_Others;
                                $scope.hreidO_InterestEvidence = $scope.Form12bb[0].hreidO_InterestEvidence;
                                $scope.hreidO_LNameEvidence = $scope.Form12bb[0].hreidO_LNameEvidence;
                                $scope.hreidO_LAddressEvidence = $scope.Form12bb[0].hreidO_LAddressEvidence;
                                $scope.hreidO_LenderPAN = $scope.Form12bb[0].hreidO_LenderPAN;
                                $scope.hreidO_InstEvidence = $scope.Form12bb[0].hreidO_InstEvidence;
                                $scope.hreidO_EmpEvidence = $scope.Form12bb[0].hreidO_EmpEvidence;
                                $scope.hreidO_OthersEvidence = $scope.Form12bb[0].hreidO_OthersEvidence;
                                $scope.hreidO_NameEvidence = $scope.Form12bb[0].hreidO_NameEvidence;
                                $scope.hreidO_AddressEvidence = $scope.Form12bb[0].hreidO_AddressEvidence;
                                $scope.hreidO_RentPaidEvidence = $scope.Form12bb[0].hreidO_RentPaidEvidence;

                                $scope.chapterlist = promise.chapterlist;
                            }
                           // $scope.cancel();
                        }
                    })
                $scope.cleardata = function () {
                    $scope.Employee = {};
                    $scope.employeeSalaryslipDetails = [];
                    $scope.submitted = false;
                    $scope.groupTypeselectedAll = false;
                    $scope.departmentselectedAll = false;
                    $scope.designationselectedAll = false;
                    $scope.employeeSelectedAll = false;
                    if ($scope.EmployeeDis) {
                        $scope.EmployeeDis = false;
                    }
                    $scope.search = "";

                    $scope.myForm.$setPristine();
                    $scope.myForm.$setUntouched();
                    $scope.LoadData();

                }

                $scope.printToCart = function (Baldwin) {
                    var innerContents = document.getElementById("Baldwin").innerHTML;
                    var popupWinindow = window.open('');
                    popupWinindow.document.open();
                    popupWinindow.document.write('<html><head>' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                    popupWinindow.document.close();
                }
            }
        };

    }
}) ();
