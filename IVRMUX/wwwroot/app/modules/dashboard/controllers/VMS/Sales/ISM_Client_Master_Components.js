(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISMclientmastercomponentsController', ISMclientmastercomponentsController)

    ISMclientmastercomponentsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ISMclientmastercomponentsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'ISMCLTC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

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
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("ISM_Client_Project/getdate_cmc", pageid)
                .then(function (promise) {
                    $scope.components_list = promise.components_list; 
                    $scope.presentCountgrid = $scope.components_list.length;

                })
        }




        // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ismcltC_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_cmc", pageid).
                then(function (promise) {
                    $scope.Id = promise.components_details[0].ismcltC_Id;
                    $scope.ISMCLTC_Name = promise.components_details[0].ismcltC_Name;
                    $scope.ISMCLTC_Description = promise.components_details[0].ismcltC_Description;

                })
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMCLTC_Name": $scope.ISMCLTC_Name,
                    "ISMCLTC_Description": $scope.ISMCLTC_Description,
                    "ISMCLTC_Id": $scope.Id
                }
                apiService.create("ISM_Client_Project/SaveEdit_cmc", data).
                    then(function (promise) {
                   

                        if (promise.returndata === "Update") {
                                swal('Record Updated Successfully.');
                                $state.reload();
                            }
                        else if (promise.returndata === "Add") {
                                swal('Record Saved Successfully.');
                                $state.reload();
                            }
                        else if (promise.returndata === "Error") {
                                swal('Operation Failed!!!');
                            }
                          

                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.Clearid = function () {
            $scope.ISMCLTC_Name = "";
            $scope.ISMCLTC_Description = "";
        }
        //deactive
        $scope.deactive = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismcltC_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Components Name?",
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
                        apiService.create("ISM_Client_Project/deactivate_cmc", bL).
                            then(function (promise) {
                                
                                if (promise.returndata === 'false') {
                                        swal('Master Components  Deactivated Successfully.');
                                    }

                                else if (promise.returndata === 'true') {
                                    swal('Master Components Activated Successfully.');
                                }
                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }
                             

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



