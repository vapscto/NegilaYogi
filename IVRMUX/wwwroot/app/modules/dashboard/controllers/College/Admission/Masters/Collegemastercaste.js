
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegemastercasteController', CollegemastercasteController)

    CollegemastercasteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function CollegemastercasteController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'iC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
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



        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("Collegemastercaste/Getdetails").
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    if (promise.count > 0) {
                        $scope.newuser = promise.gridviewDetails;
                        $scope.presentCountgrid = $scope.newuser.length;
                    }
                    else {
                        swal("No Records Found");
                    }

                    $scope.Categaries = promise.mastercastename;
                })
        };

        //delete record
        $scope.Deletemastercastedata = function (DeleteRecord, SweetAlert) {

            $scope.deleteId = DeleteRecord.iC_Id;
            var MdeleteId = $scope.deleteId;

            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete the Caste ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("Collegemastercaste/MasterDeleteModulesDTO", MdeleteId).then(function (promise) {
                            if (promise.msg != "" && promise.msg != null) {
                                swal(promise.msg);
                                return;
                            }
                            if (promise.returnVal == true) {
                                swal("Caste Deleted Successfully");
                                $state.reload();
                            }
                            else {
                                swal("Failed to Delete the Caste");
                            }
                        })
                    }
                    else {
                        swal("Caste Deletion Cancelled");
                    }

                });
        }

        // to Edit Data
        $scope.Editmastercastedata = function (EditRecord) {

            $scope.EditId = EditRecord.iC_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("Collegemastercaste/GetSelectedRowDetails/", MEditId).
                then(function (promise) {

                    $scope.name = promise.gridviewDetails[0].imC_CasteName;
                    $scope.description = promise.gridviewDetails[0].imC_CasteDesc;
                    $scope.imcC_Id = promise.gridviewDetails[0].imcC_Id;
                    $scope.IC_Id = promise.gridviewDetails[0].imC_Id;
                })
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.savemastercastedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "IC_Id": $scope.IC_Id,
                    "IC_CasteName": $scope.name,
                    "IC_CasteDesc": $scope.description,
                    "IMCC_Id": $scope.imcC_Id
                }
                apiService.create("Collegemastercaste/", data).
                    then(function (promise) {

                        if (promise.msg != "" && promise.msg != null) {
                            swal(promise.msg);
                        }
                        else if (promise.returnVal == true) {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.returnVal_update == true) {
                            swal("Record Updated Successfully");
                        }
                        else if (promise.duplicate_caste_name_bool == true) {
                            swal("Caste Name Already Exists");
                        }
                        else if (promise.returnVal == false) {
                            swal("Failed to Save");
                        }
                        else if (promise.returnVal_update == false) {
                            swal("Failed to Update");
                        }
                        $state.reload();
                    })
            }
        };

        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.iC_CasteName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.iC_CasteDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.categoryName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

    }

})();