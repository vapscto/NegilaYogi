
(function () {
    'use strict';
    angular
.module('app')
.controller('TTStaffHoursController', TTStaffHoursController)

    TTStaffHoursController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function TTStaffHoursController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.staffName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.itemsPerPage = 15;
        $scope.currentPage = 1;
        $scope.searchValue = '';
        $scope.grid_view = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };


        $scope.isOptionsRequired = function () {

            return !$scope.staff_list.some(function (options) {
                return options.stf;
            });
        }


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.grid_view = false;
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];
                $scope.griddata = [];


                angular.forEach($scope.staff_list, function (role) {
                    if (role.stf) $scope.albumNameArray1.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    staffarray: $scope.albumNameArray1,
                }
                apiService.create("TTStaffHours/getrpt", data).
                    then(function (promise) {
                        if (promise.gridweeks != null && promise.gridweeks != "" && promise.gridweeks.length>0) {
                            $scope.griddata = promise.gridweeks;
                            $scope.grid_view = true;
                         

                        }
                        else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                        }
                    })
            }
            else {
                $scope.submitted = true;

            }

        };

        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            
            $scope.rpttyp = "SAWC";
            $scope.all_check();

            apiService.getDATA("TTStaffHours/getdetails").
       then(function (promise) {
           $scope.year_list = promise.acayear;
           $scope.category_list = promise.categorylist;
           $scope.class_list = promise.classlist;
           $scope.temp_classlist = promise.classlist;
           $scope.staff_list = promise.stafflist;
           console.log($scope.staff_list)

       })
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.staff_list.every(function (options) {
                return options.stf;
            });
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_list, function (itm) {
                itm.stf = toggleStatus;
            });
            //if ($scope.usercheck == "1") {
            //    angular.forEach($scope.staff_list, function (role) {
            //        role.stf = true;
            //    })
            //    $scope.stf_flag = true;
            //}
            //else if ($scope.usercheck == "0") {
            //    angular.forEach($scope.staff_list, function (role) {
            //        role.stf = false;
            //    })
            //    $scope.stf_flag = false;
            //}
        }

        //TO clear  data
        $scope.clearid = function () {
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.hrmE_Id = "";
            $scope.usercheck = false;
            // $scope.stf = false;
            $scope.all_check();
            $scope.class_list = $scope.temp_classlist;
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };


    }

})();