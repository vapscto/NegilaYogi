(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Client_Project_BOMrController', ISM_Client_Project_BOMrController)

    ISM_Client_Project_BOMrController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ISM_Client_Project_BOMrController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        //$scope.sortKey = 'ISMCLTPRBOM_Id';
        $scope.sortKey = '';
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
            apiService.getURI("ISM_Client_Project/getdata_BOM", pageid)
                .then(function (promise) {
                    $scope.clientproject_dd = promise.clientproject_dd;
                    $scope.components_dd = promise.components_dd;
                    $scope.bom_list = promise.bom_list;
                    $scope.presentCountgrid = $scope.bom_list.length;
                })
        }

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMMCLTPR_Id": $scope.ISMMCLTPR_Id,
                    "ISMCLTC_Id": $scope.ISMCLTC_Id,
                    "ISMCLTPRBOM_Qty": $scope.ISMCLTPRBOM_Qty,
                    "ISMCLTPRBOM_Remarks": $scope.ISMCLTPRBOM_Remarks,
                    "ISMCLTPRBOM_Id": $scope.Id
                }
                apiService.create("ISM_Client_Project/SaveEdit_BOM", data).
                    then(function (promise) {
                        

                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully');
                            $state.reload();
                            //$scope.ISMMCLTPR_Id = "";
                            //$scope.ISMCLTC_Id = "";
                            //$scope.ISMCLTPRBOM_Qty = "";
                            //$scope.ISMCLTPRBOM_Remarks = "";
                            //$scope.Id =undefined;
                            //$scope.getAllDetail();
                            //    return;
                            }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully');
                            $state.reload();
                            //$scope.ISMMCLTPR_Id = "";
                            //$scope.ISMCLTC_Id = "";
                            //$scope.ISMCLTPRBOM_Qty = "";
                            //$scope.ISMCLTPRBOM_Remarks = "";
                            //$scope.Id = undefined;
                            //$scope.getAllDetail();
                            //    return;
                            }
                        else if (promise.returndata === "Error") {
                                swal('Record Not Saved/Updated successfully');
                                return;
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
            $scope.edibl = bil.ISMCLTPRBOM_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_BOM", pageid).
                then(function (promise) {
                    $scope.Id = promise.bom_details[0].ISMCLTPRBOM_Id;
                    $scope.client_project_name = promise.bom_details[0].client_project_name;
                    $scope.ISMMCLTPR_Id = promise.bom_details[0].ISMMCLTPR_Id;
                    $scope.ismcltC_Name = promise.bom_details[0].ISMCLTC_Name;
                    $scope.ISMCLTC_Id = promise.bom_details[0].ISMCLTC_Id;
                    $scope.ISMCLTPRBOM_Qty = promise.bom_details[0].ISMCLTPRBOM_Qty;
                    $scope.ISMCLTPRBOM_Remarks = promise.bom_details[0].ISMCLTPRBOM_Remarks;

                })
        };

      

        $scope.cancel = function () {
            $scope.ISMMCLTPR_Id = "";
            $scope.ISMCLTC_Id = "";
            $scope.ISMCLTPRBOM_Qty = "";
            $scope.ISMCLTPRBOM_Remarks = "";
        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ISMCLTPRBOM_ActiveFlag === false) { 
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
                        apiService.create("ISM_Client_Project/deactivate_BOM", flr).
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
                        $state.reload();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



