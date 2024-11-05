
(function () {
    'use strict';
    angular
        .module('app').controller('CollegecastecategoryController', CollegecastecategoryController)

    CollegecastecategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function CollegecastecategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'imcC_Id';
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

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            apiService.getDATA("Collegecastecategory/Getdetails").then(function (promise) {
                if (promise.count == 0) {
                    swal("No Records Found.....!!");
                    return;
                }
                else {
                    $scope.newuser = promise.castecategoryname;
                    $scope.presentCountgrid = $scope.newuser.length;
                }
            });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        //delete record
        $scope.Deletecastecategorydata = function (DeleteRecord, SweetAlert) {
            // swal(id);
            $scope.deleteId = DeleteRecord.imcC_Id;
            var MdeleteId = $scope.deleteId;
            swal({
                title: "Are You Sure?",
                text: "Do You Want To Delete The Record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.getURI("Collegecastecategory/MasterDeleteModulesDTO", MdeleteId).then(function (promise) {
                            if (promise.message == "Delete") {
                                swal("You Can Not Delete This Record It Is Already Mapped With Student");
                            }
                            else {
                                swal('Record Deleted Successfully');
                            }
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });
        };

        // to Edit Data
        $scope.Editcastecategorydata = function (EditRecord) {

            $scope.EditId = EditRecord.imcC_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("Collegecastecategory/GetSelectedRowDetails/", MEditId).then(function (promise) {
                $scope.IMCC_Id = promise.castecategoryname[0].imcC_Id;
                $scope.name = promise.castecategoryname[0].imcC_CategoryName;
                $scope.description = promise.castecategoryname[0].imcC_CategoryDesc;
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.savecastecategorydata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "IMCC_Id": $scope.IMCC_Id,
                    "IMCC_CategoryName": $scope.name,
                    "IMCC_CategoryDesc": $scope.description,
                }

                apiService.create("Collegecastecategory/", data).then(function (promise) {
                    if (promise.message != null && promise.message != "") {
                        swal(promise.message);
                        $state.reload();
                        return;
                    }

                    if (promise.returnVal == true) {
                        if (promise.messageupdate == "Save") {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.messageupdate == "Update") {
                            swal("Record Updated Successfully");
                        }
                        $state.reload();
                    }
                    else {
                        if (promise.messageupdate == "Save") {
                            swal("Record Failed To Save");
                        }
                        else if (promise.messageupdate == "Update") {
                            swal("Record Failed To Update");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.imcC_CategoryName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.imcC_CategoryDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };
    }

})();