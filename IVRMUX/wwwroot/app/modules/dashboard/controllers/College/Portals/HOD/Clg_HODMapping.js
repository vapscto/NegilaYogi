
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgDashboardController', ClgDashboardController)

    ClgDashboardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']
    function ClgDashboardController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {
        
        $scope.tempcldrlst = [];
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            $scope.search2 = "";
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;

            var pageid = 2;
            apiService.getDATA("ClgHODDashboard/Getdetails").then(function (promise) {
                
                $scope.employee = promise.query01;
              //  $scope.classes = promise.classlist;
                $scope.hod = promise.hodlist;
                $scope.branch = promise.branchlist;
                $scope.saved_HODS = promise.saved_hods;
                $scope.saved_HODS_CLS = promise.saved_hods_cls;
                $scope.saved_HODS_STF = promise.saved_hods_stf;

            });
        }

        

        //=====================================================Selection data
        $scope.all_check_empl = function (empl) {

            var toggleStatus4 = empl;
            angular.forEach($scope.employee, function (itm) {
                itm.emple = toggleStatus4;
            });
        }
        $scope.addColumn3 = function () {
            $scope.empl = $scope.employee.every(function (itm) { return itm.emple; });
        };

        $scope.all_check_empl2 = function (listemp) {

            var toggleStatus4 = listemp;
            angular.forEach($scope.employee, function (itm) {
                itm.emple2 = toggleStatus4;
            });
        }
        $scope.addColumn4 = function () {
            $scope.listemp = $scope.employee.every(function (itm) { return itm.emple2; });
        };


        $scope.all_check_cls = function (listclass) {

            var toggleStatus4 = listclass;
            angular.forEach($scope.branch, function (itm) {
                itm.class = toggleStatus4;
            });
        }
        $scope.addColumn5 = function () {
            $scope.listclass = $scope.branch.every(function (itm) { return itm.class; });
        };
        //====================================================================End


        //=================================================save for hod
        $scope.savemasterHOD = function () {
            debugger;
            if ($scope.myForm.$valid) {
                $scope.albumNameArray1 = [];
                angular.forEach($scope.employee, function (role) {
                    if (role.emple) $scope.albumNameArray1.push(role);
                })

                var data = {
                    employee: $scope.albumNameArray1,
                    "IHOD_Flg": $scope.ihoD_Flg,
                }

                apiService.create("ClgHODDashboard/savedetail", data).
                    then(function (promise) {


                        if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Some Record Already Exist.');
                        }

                        if (promise.returnsavestatus === 'saved') {
                            swal('Record Saved Successfully.');
                            $state.reload();
                        }
                        else if (promise.returnupdatestatus === 'updated') {
                            swal('Record Updated Successfully.');
                            $state.reload();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus != 'Duplicate') {
                            swal("Failed to Save/Update");
                        }
                    })
                $state.reload();
            }
            else {
                $scope.submitted = true;
            }

        };
        //=============================================================End

        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.ihoD_Id = user.ihoD_Id;

            var dystring = "";
            if (user.ihoD_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.ihoD_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("ClgHODDashboard/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //==============================================================End



        //=====================================Class Mapping For Hod
        $scope.mapHOD = function () {

            // $scope.submitted = true;
            if ($scope.myForm2.$valid) {
                $scope.albumNameArray1 = [];
                $scope.albumclass = [];

                angular.forEach($scope.employee, function (role) {
                    if (role.emple2) $scope.albumNameArray1.push(role);
                })

                angular.forEach($scope.branch, function (role) {
                    if (role.class) $scope.albumclass.push(role);
                })

                var data = {

                    employee: $scope.albumNameArray1,
                    class_lst: $scope.albumclass,
                    "IHOD_Id": $scope.ihod_Id
                }

                apiService.create("ClgHODDashboard/mappHOD", data).
                    then(function (promise) {


                        if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Some Record Already Exist.');
                        }

                        if (promise.returnsavestatus === 'saved') {
                            swal('Record Saved Successfully.');
                            $state.reload();
                        }
                        else if (promise.returnupdatestatus === 'updated') {
                            swal('Record Updated Successfully.');
                            $state.reload();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus != 'Duplicate') {
                            swal("Failed to Save/Update");
                        }
                    })
                $state.reload();
            }
            else {
                $scope.submitted2 = true;
            }

        };
        //===========================================End
        $scope.isOptionsRequired23 = function () {
            return !$scope.employee.some(function (item) {
                return item.emple;
            });
        }
        $scope.isOptionsRequired113 = function () {
            return !$scope.branch.some(function (item) {
                return item.class;
            });
        }
        $scope.isOptionsRequired243 = function () {
            return !$scope.employee.some(function (item) {
                return item.emple2;
            });
        }

        $scope.clear1 = function () {
            angular.forEach($scope.employee, function (tt) {
                tt.emple = false;
            })
            $scope.empl = "";
            $scope.ihoD_Flg = "";
            $scope.search = "";
            $scope.submitted = false;
        }

        $scope.clear2 = function () {
            $scope.ihod_Id = "";
            angular.forEach($scope.employee, function (ss) {
                ss.emple2 = false;
            })
            $scope.listemp = "";

            angular.forEach($scope.branch, function (cc) {
                cc.class = false;
            })
            $scope.listclass = "";
            $scope.search2 = "";
            $scope.submitted2 = false;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.sort = function (key) {
            $scope.reverse2 = ($scope.sortKey == key) ? !$scope.reverse2 : $scope.reverse2;
            $scope.sortKey = key;
        }



    };
})();

