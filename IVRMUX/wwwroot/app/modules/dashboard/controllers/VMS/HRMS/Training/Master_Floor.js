(function () {
    'use strict';
    angular
        .module('app')
        .controller('FloorController', floorController)

    floorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function floorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'HRMF_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

  

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Training_Master/getdata_F", pageid)
                .then(function (promise) {
                    $scope.floor = promise.floor_list;
                    $scope.building = promise.building_list;
                    $scope.presentCountgrid = $scope.floor.length;
                })
        }

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMF_FloorName": $scope.Floor,
                    "HRMB_Id": $scope.hrmB_Id,
                    "HRMF_Id": $scope.Id
                }
                apiService.create("Training_Master/SaveEdit_F", data).
                    then(function (promise) {
                        if (promise.returnvalue !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                                return;
                            }
                            else if (promise.returnvalue === "false") {
                                swal('Record Not Saved/Updated successfully');
                                return;
                            }
                            else if (promise.returnvalue === "Duplicate") {
                                swal('Category Already Exist');
                                $state.reload();
                                return;
                            }
                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
      
        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.hrmF_Id;
            var pageid = $scope.edibl;
            apiService.getURI("Training_Master/details_F", pageid).
                then(function (promise) {
                    $scope.Id = promise.floor_list[0].hrmF_Id;
                    $scope.Floor = promise.floor_list[0].hrmF_FloorName;
                    $scope.hrmB_Title = promise.floor_list[0].hrmB_BuildingName;
                    $scope.hrmB_Id = promise.floor_list[0].hrmB_Id;

                })
        };

        //delete
        //$scope.deleteemp = {}
        //$scope.delete = function (del) {
        //    $scope.deleteemp = del.id;
        //    var emp = $scope.deleteemp;
        //    apiService.getURI("Emp/delete", emp).then(function (promise) {
        //        if (promise.returnval === "Delete") {
        //            swal('Record Delete Successfully');

        //        }
        //        else {
        //            wal('Record not Delete Successfully');
        //        }
        //        $scope.student = promise.emplist;
        //        $scope.presentCountgrid = $scope.student.length;
        // })
        //}

        $scope.cancel = function () {
            $state.reload();
        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.hrmF_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Training_Master/deactivate_F", flr).
                            then(function (promise) {

                                if (promise.hrmF_ActiveFlag === true) {
                                    if (promise.returnval === false) {
                                        swal('Master Floor Deactivated Successfully');
                                    }
                                }
                                else if (promise.hrmF_ActiveFlag === false) {
                                    swal('Master Floor Activated Successfully');
                                }
                                //   }

                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                        $state.reload();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



