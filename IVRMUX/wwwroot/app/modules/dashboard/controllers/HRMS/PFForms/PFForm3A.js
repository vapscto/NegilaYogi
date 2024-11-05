(function () {
    'use strict';
    angular
.module('app')
.controller('PFForm3AController', PFForm3AController)

    PFForm3AController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFForm3AController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;


        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFForm3A/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                //if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                //    $scope.monthdropdown = promise.monthdropdown;
                //}

                if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                    $scope.employeedropdown = promise.employeedropdown;
                }

            })
        }


        $scope.employeeDetails = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.selectedempDetails = {};
                $scope.finYearFromDate = "";
                $scope.finYearToDate = "";
                $scope.institution = {};
                $scope.payrollStandard = {};

                var data = $scope.Employee;

                apiService.create("PFForm3A/getEmployeedetailsBySelection", data).
                            then(function (promise) {




                                if (promise.pfreport !== null && promise.pfreport.length > 0) {

                                    $scope.pfreport = promise.pfreport;
                                }
                                if (promise.finYearFromDate !="") {
                                    $scope.finYearFromDate = promise.finYearFromDate;
                                }
                                if (promise.finYearToDate != "") {
                                    $scope.finYearToDate = promise.finYearToDate;
                                }

                                //
                                if (promise.fatherHusbandName != "") {
                                    $scope.fatherHusbandName = promise.fatherHusbandName;
                                }
                                //payroll standard

                                if (promise.payrollStandard !== null && promise.payrollStandard.length > 0) {

                                    $scope.payrollStandard = promise.payrollStandard[0];
                                }

                                //Institution Details
                                if (promise.institutionDetails !== null && promise.institutionDetails.length > 0) {

                                    $scope.institution = promise.institutionDetails[0];

                                    var instuteAddress = "";
                                    if ($scope.institution.mI_Address1 != null && $scope.institution.mI_Address1 != "") {

                                        instuteAddress = $scope.institution.mI_Address1;

                                    }
                                    if ($scope.institution.mI_Address2 != null && $scope.institution.mI_Address2 != "") {

                                        instuteAddress = instuteAddress + ',' + $scope.institution.mI_Address2;

                                    }

                                    if ($scope.institution.mI_Address3 != null && $scope.institution.mI_Address3 != "") {

                                        instuteAddress = instuteAddress + ',' + $scope.institution.mI_Address3;

                                    }

                                    $scope.CurrentInstuteAddress = instuteAddress;
                                }

                                if (promise.retrunMsg == "NoFinYear") {

                                    swal('Financial Year Not Mapped..!!','Kindly contact Administrator..!!')

                                    return;

                                } else if (promise.retrunMsg == "Error") {

                                    swal('Something went wrong..!!', 'Kindly contact Administrator..!!')

                                    return;

                                } else {

                                    //Employee Details
                                    if (promise.employeeDetails !== null && promise.employeeDetails.length > 0) {
                                        $scope.EmployeeDis = true;

                                        $scope.empdetails = promise.employeeDetails[0];


                                        var EmployeeName = "";
                                        if ($scope.empdetails.hrmE_EmployeeFirstName != null && $scope.empdetails.hrmE_EmployeeFirstName != "") {

                                            EmployeeName = $scope.empdetails.hrmE_EmployeeFirstName;

                                        }
                                        if ($scope.empdetails.hrmE_EmployeeMiddleName != null && $scope.empdetails.hrmE_EmployeeMiddleName != "") {

                                            EmployeeName = EmployeeName + ' ' + $scope.empdetails.hrmE_EmployeeMiddleName;

                                        }

                                        if ($scope.empdetails.hrmE_EmployeeLastName != null && $scope.empdetails.hrmE_EmployeeLastName != "") {

                                            EmployeeName = EmployeeName + ' ' + $scope.empdetails.hrmE_EmployeeLastName;

                                        }

                                        $scope.FullName = $scope.empdetails.hrmE_EmployeeFirstName + ' ' + $scope.empdetails.hrmE_EmployeeMiddleName + ' ' + $scope.empdetails.hrmE_EmployeeLastName

                                        $scope.pfAccountNo = $scope.empdetails.hrmE_PFAccNo;


                                        $scope.hrmE_LeavingReason = $scope.empdetails.hrmE_LeavingReason;
                                        $scope.hrmE_DOL = new Date($scope.empdetails.hrmE_DOL);
                                    }
                                    else {
                                        $scope.EmployeeDis = false;
                                        swal('No Record found to display .. !');
                                        return;
                                    }

                                }

                               



                            })
            }

        }



      

        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.employeeDetails = [];
            $scope.submitted = false;
            $scope.finYearFromDate = "";
            $scope.finYearToDate = "";
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

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

       

        $scope.getAmountofWagesTotal = function () {
            var total = 0;
            if ($scope.pfreport !=null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.amountofWages;
                }
            }
           
            return total;
        }


        $scope.getpfAmountTotal = function () {
            var total = 0;
            if ($scope.pfreport !=null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.pfAmount;
                }
            }
           
            return total;
        }
        $scope.gethreS_EPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_EPF;
                }
            }
           
            return total;
        }
        $scope.gethreS_FPFTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += product.hreS_FPF;
                }
            }
           
            return total;
        }

        $scope.getTotal = function () {
            var total = 0;
            if ($scope.pfreport != null) {
                for (var i = 0; i < $scope.pfreport.length; i++) {
                    var product = $scope.pfreport[i];
                    total += (product.hreS_EPF + product.hreS_FPF);
                }
            }
           
            return total;
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
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }


})();