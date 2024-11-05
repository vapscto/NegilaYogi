(function () {
    'use strict';
    angular
        .module('app')
        .controller('PFChallenController', PFChallenController)

    PFChallenController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PFChallenController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object

        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;




        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("PFChallen/getalldetails", pageid).then(function (promise) {

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


        $scope.pfreport = [];
        $scope.EmployeeDis = false;
        //Search employee
        $scope.submitted = false;
        $scope.SearchEmployee = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.mI_Pincode = [];
                $scope.monthArray = [];
                $scope.yearArray = [];

                var data = {};


                data = $scope.Employee;

                apiService.create("PFChallen/getEmployeedetailsBySelection", data).
                    then(function (promise) {
                        $scope.EmployeeDis = true;
                        if (promise.pfreport !== null && promise.pfreport.length > 0) {

                            $scope.pfreport = promise.pfreport;
                        }

                        if (promise.hreS_Month != "") {
                            $scope.month = promise.hreS_Month;

                            var single_object = $filter('filter')($scope.monthdropdown, function (d) {
                                return d.ivrM_Month_Name === $scope.month;
                            })[0].ivrM_Month_Id;

                            var ivrM_Month_Id = single_object.toString();
                            var monthArray = ivrM_Month_Id.split('');
                            if (monthArray.length == 1) {
                                $scope.monthArray[0] = 0;
                                $scope.monthArray[1] = monthArray[0];
                            } else {
                                $scope.monthArray = monthArray;
                            }

                        }
                        if (promise.hreS_Year != "") {
                            $scope.year = promise.hreS_Year.toString();
                            $scope.yearArray = $scope.year.split('');
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

                            var pincode = $scope.institution.mI_Pincode.toString();
                            $scope.mI_Pincode = pincode.split('');

                        }


                    })
            }

        }





        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $scope.Employee = {};
            $scope.pfreport = [];
            $scope.mI_Pincode = [];
            $scope.yearArray = [];
            $scope.monthArray = [];
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
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


    }


})();