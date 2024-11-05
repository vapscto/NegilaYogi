(function () {
    'use strict';
    angular
        .module('app')
        .controller('ProbationaryReport', ProbationaryReport);
    ProbationaryReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache'];
    function ProbationaryReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {        
        //Employeee
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loadData = function () {           
            var id = 2;
            apiService.getURI("ProbationaryReport/getalldetails", id).
                then(function (promise) {                  
                    $scope.staff_types = promise.filltypes;
                    $scope.All_Individual('con');  
                    $scope.grid_view = false;
                })
        };               
            //loading end
                    //validation start
        $scope.interacted = function (field) {
                    return $scope.submitted || field.$dirty;
                };
            $scope.isOptionsRequired = function () {
                return !$scope.staff_types.some(function (options) {
                    return options.selected;
                });
            }
            $scope.isOptionsRequired1 = function () {
                return !$scope.Department_types.some(function (options) {
                    return options.selected;
                });
            }
            $scope.isOptionsRequired2 = function () {
                return !$scope.Designation_types.some(function (options) {
                    return options.selected;
                });
            }
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        //validation end
        //all-individual start
        $scope.All_Individual = function () {
            if ($scope.allind == 'prob')
                $scope.disabledata = false;
            else
                $scope.disabledata = true;
        }
        //all-individual end
        //all check button start
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_departments();
        }
        $scope.all_checkdep = function () {
            var toggleStatus = $scope.deptcheck;
            angular.forEach($scope.Department_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_designation();
        }
        $scope.all_checkdesg = function () {
            var toggleStatus = $scope.desgcheck;
            angular.forEach($scope.Designation_types, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.get_employee();
        }
        //all-check button end
        // fill dep start
        $scope.get_departments = function () {
            $scope.usercheck = $scope.staff_types.every(function (options) {
                return options.selected;
            });
            $scope.deptcheck = "";
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                if ($scope.staff_types[i].selected == true) {
                    if (groupidss == undefined)
                        groupidss = $scope.staff_types[i].hrmgT_Id;
                    else
                        groupidss = groupidss + "," + $scope.staff_types[i].hrmgT_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {                   
                    "multipletype": groupidss,
                }
                apiService.create("ProbationaryReport/get_departments", data).
                    then(function (promise) {
                        $scope.Department_types = promise.departmentdropdown;
                        if ($scope.Department_types.length > 0) {                          
                            $scope.get_designation();
                        }
                    })
            }
            else {
                $scope.Department_types = "";
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }
        //fill department end
        //fill desg start
        $scope.get_designation = function () {
            $scope.deptcheck = $scope.Department_types.every(function (options) {
                return options.selected;
            });
            $scope.get_designationnew();
        }
        $scope.get_designationnew = function () {
            $scope.desgcheck = "";
            var groupidss;
            for (var i = 0; i < $scope.Department_types.length; i++) {
                if ($scope.Department_types[i].selected == true) {
                    if (groupidss == undefined)
                        groupidss = $scope.Department_types[i].hrmD_Id;
                    else
                        groupidss = groupidss + "," + $scope.Department_types[i].hrmD_Id;
                }
            }
            if (groupidss != undefined) {
                var data = {
                    "multipledep": groupidss,
                }
                apiService.create("ProbationaryReport/get_designation", data).
                    then(function (promise) {
                        $scope.Designation_types = promise.designationdropdown;
                        if ($scope.Designation_types.length > 0) {                           
                            $scope.get_employee();
                        }
                    })
            }
            else {
                $scope.Designation_types = "";
                $scope.Employeelst = "";
            }
        }       
        $scope.submitted = false;
        $scope.SearchEmployee = function () {           
            if ($scope.myForm.$valid) {              
                var groupidss = {};
                //if ($scope.allind === null || $scope.allind === undefined) {
                //    swal('Select Report Type');
                //    return;
                //}               
                var data = {
                   
                    "multiplehrmeid": groupidss,
                    "Type": $scope.allind
                }
                apiService.create("ProbationaryReport/getProbationaryReport", data).
                    then(function (promise) {                                            
                        if (promise.employeedetailList !== null && promise.employeedetailList.length > 0) {
                            $scope.grid_view = true;
                            $scope.employeedetailList = promise.employeedetailList;
                        }                       
                        else {
                            $scope.grid_view = false;
                            swal("Record not found.");
                        }
                    });
            }       
            else {
                $scope.submitted = true;
            }
        }
        //TO clear  data
        $scope.cleardata = function () {
            $scope.submitted = false;
            $scope.Department_types = "";
            $scope.Designation_types = "";
            $scope.usercheck = "";
            $scope.deptcheck = "";
            $scope.desgcheck = "";
            $scope.allind = "con";
            $scope.disabledata = true;
            $scope.grid_view = false;
            for (var i = 0; i < $scope.staff_types.length; i++) {
                $scope.staff_types[i].selected = false;
            }
        };
        //clear end
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
        };
        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("BankCash");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };
    }
})();