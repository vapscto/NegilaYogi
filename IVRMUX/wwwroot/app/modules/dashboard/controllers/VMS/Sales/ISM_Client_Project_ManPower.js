(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Client_Project_ManPowerController', ISM_Client_Project_ManPowerController)

    ISM_Client_Project_ManPowerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ISM_Client_Project_ManPowerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'ISMCLTPRBOM_Id';
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
            apiService.getURI("ISM_Client_Project/getdata_MP", pageid)
                .then(function (promise) {
                    $scope.clientproject_dd = promise.clientproject_dd;
                     $scope.mp_list = promise.mp_list;
                    $scope.presentCountgrid = $scope.mp_list.length;
                })
        }

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMCLTPR_Id": $scope.ISMMCLTPR_Id,
                    "ISMCLTPRMP_ResourceName": $scope.ISMCLTPRMP_ResourceName,
                    "ISMCLTPRMP_Qty": $scope.ISMCLTPRMP_Qty,
                    "ISMCLTPRMP_Remarks": $scope.ISMCLTPRMP_Remarks,
                    "ISMCLTPRMP_Id": $scope.Id
                }
                apiService.create("ISM_Client_Project/SaveEdit_MP", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully');
                           
                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully');
                           
                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully');
                           
                        }
                        $state.reload();
                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        //edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ISMCLTPRMP_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_MP", pageid).
                then(function (promise) {
                    $scope.Id = promise.mp_details[0].ISMCLTPRMP_Id;
                    $scope.client_project_name = promise.mp_details[0].client_project_name;
                    $scope.ISMMCLTPR_Id = promise.mp_details[0].ISMMCLTPR_Id;
                    $scope.ISMCLTPRMP_ResourceName = promise.mp_details[0].ISMCLTPRMP_ResourceName;
                    $scope.ISMCLTPRMP_Qty = promise.mp_details[0].ISMCLTPRMP_Qty;
                    $scope.ISMCLTPRMP_Remarks = promise.mp_details[0].ISMCLTPRMP_Remarks;

                })
        };



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
            if (flr.ISMCLTPRMP_ActiveFlag === false) {
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
                        apiService.create("ISM_Client_Project/deactivate_MP", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Client Project Bill Of Material Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Client Project Bill Of Material Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $scope.getAllDetail();
                            });
                    } else {
                        swal("Cancelled");
                        $scope.getAllDetail();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



