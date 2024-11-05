(function () {
    'use strict';
    angular
        .module('app')
        .controller('ISM_Client_Project_Master_DocsController', ISM_Client_Project_Master_DocsController)

    ISM_Client_Project_Master_DocsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ISM_Client_Project_Master_DocsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'ISMCLTPRMDOC_Id';
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
            apiService.getURI("ISM_Client_Project/getdata_MDOC", pageid)
                .then(function (promise) {
                    $scope.docmaster_list = promise.docmaster_list;
                    $scope.presentCountgrid = $scope.docmaster_list.length;

                })
        }




        // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
            $scope.edibl = bil.ismcltprmdoC_Id;
            var pageid = $scope.edibl;
            apiService.getURI("ISM_Client_Project/details_MDOC", pageid).
                then(function (promise) {
                    $scope.Id = promise.docmaster_details[0].ismcltprmdoC_Id;
                    $scope.ISMCLTPRMDOC_Name = promise.docmaster_details[0].ismcltprmdoC_Name;
                    $scope.ISMCLTPRMDOC_Description = promise.docmaster_details[0].ismcltprmdoC_Description;

                })
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMCLTPRMDOC_Name": $scope.ISMCLTPRMDOC_Name,
                    "ISMCLTPRMDOC_Description": $scope.ISMCLTPRMDOC_Description,
                    "ISMCLTPRMDOC_Id": $scope.Id
                }
                apiService.create("ISM_Client_Project/SaveEdit_MDOC", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully.');
                           
                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully.');
                            
                        }
                        else if (promise.returndata === "Error") {
                            swal('Operation Failed!!!');
                        }

                        $scope.ISMCLTPRMDOC_Name = "";
                        $scope.ISMCLTPRMDOC_Description = "";
                        $scope.Id = undefined;
                        $scope.loaddata();
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.Clearid = function () {
            $scope.ISMCLTPRMDOC_Name = "";
            $scope.ISMCLTPRMDOC_Description = "";
            $scope.Id = undefined;
        }
        //deactive
        $scope.deactive = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismcltprmdoC_ActiveFlag === false) {
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
                        apiService.create("ISM_Client_Project/deactivate_MDOC", bL).
                            then(function (promise) {

                                if (promise.returndata === 'false') {
                                    swal('Master Document Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Master Document Activated Successfully.');
                                }
                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }


                                $scope.loaddata();
                            });
                    } else {
                        swal("Cancelled");
                        $scope.loaddata();
                    }

                });
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



