(function () {
    'use strict';
    angular
.module('app')
.controller('EmployeeServiceRecordReportController', EmployeeServiceRecordReportController)
    EmployeeServiceRecordReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function EmployeeServiceRecordReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object
        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.Employee = {};
        $scope.Employee.FormatType = "Format1";
        $scope.Employee.Working = true;
        $scope.Employee.Left = true;
        $scope.Employee.AllOrIndividual = "All";
        $scope.Employee.FIAL = 'FullDetails';
        $scope.Employee.AWL = "AllF2";

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("EmployeeServiceRecordReport/getalldetails", pageid).then(function (promise) {
                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdownF1 = promise.employeedropdown;
                    $scope.employeedropdownF2 = promise.employeedropdown;
                }
                if (promise.employeeTypedropdown !== null && promise.employeeTypedropdown.length > 0) {
                    $scope.employeeTypedropdown = promise.employeeTypedropdown;
                }
            })
        }

        //Filter Employee details
        $scope.FilterEmployeeData = function () {
            $scope.employeedropdownF1 = [];
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var data = $scope.Employee;
            apiService.create("EmployeeServiceRecordReport/FilterEmployeeData", data).
                           then(function (promise) {
                               if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                                   $scope.employeedropdownF1 = promise.employeedropdown;
                               }
                           })
        }

        //Filter Employee details format 2
        $scope.FilterEmployeeDataF2 = function () {
            $scope.employeedropdownF2 = [];
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            var data = $scope.Employee;
            if ($scope.Employee.AllOrIndividual === "All") {
                return;
            }
            if ($scope.Employee.TypeOrEmployee === "Type") {
                return;
            }

            $scope.Employee.HRME_Id = 0;
            apiService.create("EmployeeServiceRecordReport/FilterEmployeeData", data).
                           then(function (promise) {
                               $scope.Employee.HRME_Id = "";
                               if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                                   $scope.employeedropdownF2 = promise.employeedropdown;
                               }
                           })
        }


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        $scope.institutionDetails = {};
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.institutionDetails = {};
                if ($scope.Employee.FormatType == 'Format2' && $scope.Employee.AllOrIndividual == "All") {
                    $scope.Employee.HRME_Id = 0;
                }

                $scope.Employee.institutionDetails =$scope.institutionDetails;

                var data = $scope.Employee;

                
                apiService.create("EmployeeServiceRecordReport/getEmployeedetailsBySelection", data).
                            then(function (promise) {

                                if (promise.institutionDetails != null) {
                                    $scope.institutionDetails = promise.institutionDetails;

                                    //  $('#blah').attr('src', 'https://bdcampusstrg.blob.core.windows.net/files/' + $scope.institutionDetails.mi_id + "/" + "EmployeeProfilePics" + "/" + $scope.institutionDetails.mI_Logo);

                                    var instuteAddress = "";
                                    if ($scope.institutionDetails.mI_Address1 != null && $scope.institutionDetails.mI_Address1 != "") {

                                        instuteAddress = $scope.institutionDetails.mI_Address1;

                                    }
                                    if ($scope.institutionDetails.mI_Address2 != null && $scope.institutionDetails.mI_Address2 != "") {

                                        instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address2;

                                    }

                                    if ($scope.institutionDetails.mI_Address3 != null && $scope.institutionDetails.mI_Address3 != "") {

                                        instuteAddress = instuteAddress + ',' + $scope.institutionDetails.mI_Address3;

                                    }

                                    $scope.CurrentInstuteAddress = instuteAddress;

                                }


                                if ($scope.Employee.FormatType === "Format1") {
                                    if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                                        $scope.EmployeeDis = true;

                                        $scope.EmployeeData = promise.employeeDetails[0];
                                    } else {
                                        $scope.EmployeeDis = false;
                                        swal('No Record found to display .. !');
                                        return;
                                    }

                                } else if ($scope.Employee.FormatType === "Format2") {

                                    if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                                        $scope.EmployeeDis = true;

                                        $scope.employeeDetails = promise.employeeDetails;
                                    }
                                    else {
                                        $scope.EmployeeDis = false;
                                        swal('No Record found to display .. !');
                                        return;
                                    }
                                }
                                
                                else {
                                    $scope.EmployeeDis = false;
                                    swal('No Record found to display .. !');
                                    return;
                                }
                            })
            }
        }


        //Clear data
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.Employee.FormatType = "Format1";
            $scope.Employee.Working = true;
            $scope.Employee.Left = true;
            $scope.Employee.AllOrIndividual = "All";
            $scope.Employee.FIAL = 'FullDetails';
            $scope.Employee.AWL = 'AllF2';
            $scope.employeeDetails = [];
            $scope.submitted = false;
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onLoadGetData();
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.onClickFormatOne = function () {

            $scope.Employee.HRME_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.onClickFormatTwo = function () {

            $scope.Employee.HRME_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.onchageDropdownValue = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        $scope.GetAllOrIndividualEmployee = function () {

            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        //$scope.printData = function () {
        //    var divToPrint = document.getElementById("Table");
        //    var newWin = window.open();
        //    newWin.document.write(divToPrint.outerHTML);
        //    newWin.print();
        //    newWin.close();
        //    // $state.reload();
        //}

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
})();