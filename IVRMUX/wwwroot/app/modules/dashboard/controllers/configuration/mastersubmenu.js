

(function () {
    'use strict';
    angular
.module('app')
.controller('masterSubMenuController', masterSubMenuController)

    masterSubMenuController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$window', 'superCache']
    function masterSubMenuController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/mastermenu/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMenuPageMapping/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };



        $scope.search = '';
        $scope.filterOnLocation = function (newuser) {
            return angular.lowercase(newuser.modulename).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(newuser.ivrmmM_MenuName).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(newuser.subMenuName).indexOf(angular.lowercase($scope.search)) >= 0 || JSON.stringify(newuser.ivrmmM_MenuOrder).indexOf($scope.search) >= 0;
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("MasterSubMenu/Getdetails").
       then(function (promise) {
           $scope.modulename = promise.masterModulesname;
           $scope.masterMainMenuName = promise.masterMainMenuName;
           $scope.GridDetails = promise.gridDetails;
       })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        };


        $scope.editEmployee = {}
        $scope.DeleteMasterSubMenudata = function (employee) {
            $scope.editEmployee = employee.ivrmmM_Id;
            var MdeleteId = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do you want to delete record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterSubMenu/MasterDeleteSubMenuDTO", MdeleteId).
                    then(function (promise) {

                        //if (promise.returnval == true) {
                        //    swal('Record Deleted successfully!', 'success');
                        //    $state.reload();
                        //}
                        //else {
                        //    swal('Record Not Deleted', 'Failed');
                        //}

                        if (promise.returnval == "true") {
                            swal('Record Deleted Successfully!', 'success');
                            $state.reload();
                        }
                        else if (promise.returnval == "false") {
                            swal('Record Cant Be Deleted!!');
                        }

                        $state.reload();

                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }


        $scope.EditMasterSubMenudata = function (EditRecord) {
            $scope.EditId = EditRecord.ivrmmM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("MasterSubMenu/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.IVRMMM_Id = promise.masterModulesname[0].ivrmmM_Id;
                $scope.IVRMMM_Id_select = promise.masterModulesname[0].ivrmmM_Id_select;
                $scope.IVRMM_Id = promise.masterModulesname[0].ivrmM_Id;
                $scope.MainMainMenuName = promise.masterModulesname[0].ivrmmM_MenuName;
                $scope.MainSubMenuName = promise.masterModulesname[0].subMenuName;
                $scope.Order = promise.masterModulesname[0].ivrmmM_MenuOrder;

            })
        };

        $scope.cance = function () {
            $scope.IVRMMM_Id = "";
            $state.reload();
        }


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.submitted = false;
        $scope.saveMasterSubMenudata = function () {

            $scope.submitted = true;

            if ($scope.frmmodule.$valid) {


                var data = {
                    "IVRMMM_Id": $scope.IVRMMM_Id,
                    "IVRMMM_MenuName": $scope.MainSubMenuName,
                    "IVRMM_Id": $scope.IVRMM_Id,
                    "IVRMMM_ParentId": $scope.IVRMMM_Id_select,
                    "IVRMMM_PageNonPageFlag": "False",
                    "IVRMMM_MenuOrder": $scope.Order
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterSubMenu/", data).
                        then(function (promise) {

                            if (promise.returnval == "Add") {
                                swal('Record Saved Successfully', 'Success');
                                $state.reload();
                            }
                            else if (promise.returnval == "Update") {
                                swal('Record Updated Successfully', 'Success');
                                $state.reload();
                            }
                            else if (promise.returnval == "false") {
                                swal('Record Not Saved/Updated Successfully');
                            }
                            else if (promise.returnval == "Duplicate") {
                                swal('Sub Menu Already Exists', 'Duplicate!');
                                return;
                            }
                            else if (promise.returnval == "DuplicateOrder") {
                                swal('Sub Menu Order already exists', 'Duplicate Order');
                                return;
                            }
                        })
            };
        }
    }

})();