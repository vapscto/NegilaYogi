

(function () {
    'use strict';
    angular
.module('app')
.controller('masterMenuController', masterMenuController)

    masterMenuController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache']
    function masterMenuController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {


        //$http.get('/Angular/AllStudents') // added an '/' along with deleting Controller portion
        // .then(function (response) {
        //     $scope.newuser = response.data
        // })


        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/privileges/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/mastersubmenu/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("MasterMainMenu/Getdetails").
       then(function (promise) {
           $scope.modulename = promise.masterModulesname;
           $scope.GridDetails = promise.gridDetails;
       })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        };


        $scope.editEmployee = {}
        $scope.DeleteMasterMainMenudata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmmM_Id;
            var MdeleteId = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do you want to Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterMainMenu/MasterDeleteMainMenuDTO", MdeleteId).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal('Record Deleted Successfully!', 'Success');
                            $state.reload();
                        }
                        else if (promise.returnval == "false") {
                            swal('Record Cont Be Deleted!!');
                        }

                        $state.reload();
                       
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }


        $scope.EditMasterMainMenudata = function (EditRecord) {
            $scope.EditId = EditRecord.ivrmmM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("MasterMainMenu/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.IVRMMM_Id = promise.masterModulesname[0].ivrmmM_Id;
                $scope.IVRMM_Id = promise.masterModulesname[0].ivrmM_Id;
                $scope.MainMainMenuName = promise.masterModulesname[0].ivrmmM_MenuName;
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
        $scope.saveMasterMainMenudata = function () {

            $scope.submitted = true;

            if ($scope.frmmodule.$valid) {


                var data = {
                    "IVRMMM_Id": $scope.IVRMMM_Id,
                    "IVRMMM_MenuName": $scope.MainMainMenuName,
                    "IVRMM_Id": $scope.IVRMM_Id,
                    "IVRMMM_ParentId":0,
                    "IVRMMM_PageNonPageFlag":"False",
                    "IVRMMM_MenuOrder":1
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterMainMenu/", data).
                        then(function (promise) {
                            if (promise.returnval == "Save" || promise.returnval == "Update") {
                                if (promise.returnval == "Save") {
                                    swal('Record Saved Successfully', 'success');
                                    $state.reload();
                                }
                                else if (promise.returnval == "Update") {
                                    swal('Record Updated Successfully', 'success');
                                    $state.reload();
                                }
                            }

                            else if (promise.returnval == "NotSave" || promise.returnval == "NotUpdate") {
                                if (promise.returnval == "NotSave") {
                                    swal('Record Not Saved');
                                    $state.reload();
                                }
                                else if (promise.returnval == "NotUpdate") {
                                    swal('Record Not Updated');
                                    $state.reload();

                                }
                            }
                            else if (promise.returnval == "Duplicate") {
                                swal('Master Main Menu Already Exist');

                            }


                            
                        })
            };
        }
    }

})();