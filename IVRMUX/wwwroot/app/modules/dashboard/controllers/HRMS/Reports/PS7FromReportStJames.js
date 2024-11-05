(function () {
    'use strict';
    angular
        .module('app')
        .controller('PS7FromReportStJamesController', PS7FromReportStJamesController)

    PS7FromReportStJamesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function PS7FromReportStJamesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        //object




        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;

            $scope.all_check_empl = function () {
                var checkStatus = $scope.empl;
                var count = 0;
                angular.forEach($scope.employeedropdown, function (itm) {
                    itm.selected = checkStatus;
                    if (itm.selected == true) {
                        count += 1;
                    }
                    else {
                        count = 0;
                    }
                });
            }
            $scope.isOptionsRequired3 = function () {
                return !$scope.employeedropdown.some(function (options) {
                    return options.selected;
                });
            }

            $scope.addColumn4 = function () {

                $scope.empl = $scope.employeedropdown.every(function (options) {
                    return options.selected;
                });
            }


            apiService.getURI("PS7andPS8FormReport/getalldetails", pageid).then(function (promise) {

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
            $scope.albumNameArray1 = [];
            angular.forEach($scope.employeedropdown, function (role) {
                if (role.selected) $scope.albumNameArray1.push(role);
            })

            if ($scope.myForm.$valid) {
                var data = {
                    "HRES_Year": $scope.hreS_Year,
                    employee: $scope.albumNameArray1,
                }
                apiService.create("PS7andPS8FormReport/getEmployeedetailsBySelection", data).then(function (promise) {

                    if (promise.pfreport.length > 0 && promise.pfreport != null) {
                        $scope.employeeDetails = promise.employeeDetails;

                        $scope.pfreport = promise.pfreport;
                        $scope.nextyear = $scope.hreS_Year;
                        $scope.nextyear++;


                       

                        angular.forEach($scope.employeeDetails, function (emp) {
                            emp.Totalamountofwages = 0;
                            emp.Totalpensionamount = 0;
                            angular.forEach($scope.pfreport, function (itm) {
                                if (emp.hrmE_Id == itm.HRME_Id) {
                                    
                                    emp.Totalamountofwages = emp.Totalamountofwages + itm.sumcondition;
                                    emp.Totalpensionamount = emp.Totalpensionamount + itm.pensionamout;
                                }
                                
                            });
                        });
                    }

                    $scope.temp = [];


                    $scope.temp.push({
                        Certified: "Certified",
                        Month_Idone: "March"
                    })






                })
            }

        }



        //Clear data
        $scope.Employee = {};
        $scope.cleardata = function () {
            $state.reload();

        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.disableGrid = function () {
            if ($scope.EmployeeDis) {
                $scope.EmployeeDis = false;
            }
        }

        //$scope.printToCart = function (Baldwin) {
        //    var innerContents = document.getElementById("Baldwin").innerHTML;
        //    var popupWinindow = window.open('');
        //    popupWinindow.document.open();
        //    popupWinindow.document.write('<html><head>' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdf.css" />' +
        //        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        //        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        //    popupWinindow.document.close();
        //}

        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                // '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/EMPPFSchemePdfnew.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/VIKASATCREPORT/VIKASATCReportPdf1.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


    }


})();