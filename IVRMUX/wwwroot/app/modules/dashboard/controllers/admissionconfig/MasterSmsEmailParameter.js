
(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterSmsEmailParameterController', MasterSmsEmailParameterController)

    MasterSmsEmailParameterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function MasterSmsEmailParameterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.sortKey = 'imcC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
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
            apiService.getDATA("MasterSmsEmailParameter/Getdetails").then(function (promise) {
              
                $scope.parameterlist = promise.parameterlist;
                   
              
            });
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        //delete record
        $scope.Deletecastecategorydata = function (DeleteRecord, SweetAlert) {
            // swal(id);
            $scope.deleteId = DeleteRecord.ismP_ID;
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
                        apiService.DeleteURI("MasterSmsEmailParameter/deletedata", MdeleteId).then(function (promise) {
                            if (promise.message === "Delete") {
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

          
            apiService.create("MasterSmsEmailParameter/edit/", EditRecord).then(function (promise) {
                $scope.ISMP_ID = promise.editlist[0].ismP_ID;
                $scope.ISMP_NAME = promise.editlist[0].ismP_NAME;
                $scope.ISMP_Query = promise.editlist[0].ismP_Query;
                $scope.ISMP_ParameterDesc = promise.editlist[0].ismP_ParameterDesc;
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ISMP_ID": $scope.ISMP_ID,
                    "ISMP_NAME": $scope.ISMP_NAME,
                    "ISMP_Query": $scope.ISMP_Query,
                    "ISMP_ParameterDesc": $scope.ISMP_ParameterDesc
                };

                apiService.create("MasterSmsEmailParameter/Savedata", data).then(function (promise) {
                    if (promise.message !== null && promise.message !== "") {
                        swal(promise.message);
                        $state.reload();
                        return;
                    }

                    if (promise.returnVal === true) {
                        if (promise.messageupdate === "Save") {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.messageupdate === "Update") {
                            swal("Record Updated Successfully");
                        }
                        $state.reload();
                    }
                    else {
                        if (promise.messageupdate === "Save") {
                            swal("Record Failed To Save");
                        }
                        else if (promise.messageupdate === "Update") {
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
        };

        $scope.searchValue = '';
      
    }

})();