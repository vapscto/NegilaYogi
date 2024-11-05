(function () {
    'use strict';
    angular
.module('app')
.controller('PFForm10Controller', PFForm10Controller)

    PFForm10Controller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFForm10Controller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

       
        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFForm10/getalldetails", pageid).then(function (promise) {

                if (promise.leaveyeardropdown !== null && promise.leaveyeardropdown.length > 0) {

                    $scope.leaveyeardropdown = promise.leaveyeardropdown;
                }

                if (promise.monthdropdown !== null && promise.monthdropdown.length > 0) {
                    $scope.monthdropdown = promise.monthdropdown;
                }

                //if (promise.employeedropdown !== null && promise.employeedropdown.length > 0) {
                //    $scope.employeedropdown = promise.employeedropdown;
                //}



            })
        }


        $scope.pfreport= [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.payrollStandard = {};
                $scope.pfreport = [];
               var data = $scope.Employee;

                apiService.create("PFForm10/getEmployeedetailsBySelection", data).
                            then(function (promise) {
                                $scope.EmployeeDis = true;
                                if (promise.pfreport !== null && promise.pfreport.length > 0) {

                                    $scope.pfreport = promise.pfreport;
                                }

                                if (promise.hreS_Month != "") {
                                    $scope.month = promise.hreS_Month;
                                }
                                if (promise.hreS_Year != "") {
                                    $scope.year = promise.hreS_Year;
                                }
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

                                        instuteAddress = instuteAddress + ', ' + $scope.institution.mI_Address2;

                                    }

                                    if ($scope.institution.mI_Address3 != null && $scope.institution.mI_Address3 != "") {

                                        instuteAddress = instuteAddress + ', ' + $scope.institution.mI_Address3;

                                    }

                                    $scope.CurrentInstuteAddress = instuteAddress;
                                }



                            })
            }

        }



        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.pfreport= [];
            $scope.submitted = false;
            $scope.payrollStandard = {};
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
            $scope.search = "";
            $scope.CurrentInstuteAddress = "";
            $scope.institution = {};
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