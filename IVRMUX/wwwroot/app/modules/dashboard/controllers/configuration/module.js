

(function () {
    'use strict';
    angular
.module('app')
.controller('MasterModulesController', MasterModulesController)

    MasterModulesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache']
    function MasterModulesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {


        //$http.get('/Angular/AllStudents') // added an '/' along with deleting Controller portion
        // .then(function (response) {
        //     $scope.newuser = response.data
        // })


        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSetting/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/masterpage/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        $scope.BindData = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("MasterModules/Getdetails").
       then(function (promise) {
           $scope.newuser = promise.masterModulesname;
       })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        };


        //$scope.DeleteMasterModulesdata = function (DeleteRecord) {
        //    var confirmPopup = confirm('Are you sure you want to delete this item?');
        //    if (confirmPopup === true) {
        //        $scope.deleteId = DeleteRecord.ivrmM_Id;
        //        var MdeleteId = $scope.deleteId;
        //        apiService.DeleteURI("MasterModules/MasterDeleteModulesDTO", MdeleteId)

        //        $scope.$apply();

        //        swal("Record Deleted Successfully");
        //        $scope.saved = "Record Deleted Successfully";

        //        $scope.BindData();

        //    }

        //};


        $scope.editEmployee = {}
        $scope.DeleteMasterModulesdata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmM_Id;
            var MdeleteId = $scope.editEmployee;

            var mgs = "";
            var confirmmgs = "";
            if (employee.module_ActiveFlag == 1) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterModules/MasterDeleteModulesDTO", MdeleteId).
                    then(function (promise) {

                        if (promise.returnval == "true") {
                            swal('Record Successfully ' + confirmmgs);
                        }
                        else {
                            swal('Record not successfully ' + confirmmgs);
                        }

                        $state.reload();
                       
                    })
                }
                else {
                    swal("Record  " + mgs + " Cancelled");
                }
            });
        }


        $scope.EditMasterModulesdata = function (EditRecord) {
            $scope.EditId = EditRecord.ivrmM_Id;
            var MEditId = $scope.EditId;
            apiService.getURI("MasterModules/GetSelectedRowDetails/", MEditId).
            then(function (promise) {

                $scope.newuser.IVRMM_Id = promise.masterModulesname[0].ivrmM_Id;
                $scope.newuser.name = promise.masterModulesname[0].ivrmM_ModuleName;
                $scope.newuser.description = promise.masterModulesname[0].ivrmM_ModuleDesc;


            })
        };

        $scope.cance = function () {
            $state.reload();
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.submitted = false;
        $scope.saveMasterModulesdata = function () {

            $scope.submitted = true;

            if ($scope.frmmodule.$valid) {

                var activeflag = $scope.module_ActiveFlag

                activeflag = 1;

                var data = {
                    "IVRMM_ModuleName": $scope.newuser.name,
                    "IVRMM_ModuleDesc": $scope.newuser.description,
                    "Module_ActiveFlag": activeflag,
                    "IVRMM_Id": $scope.newuser.IVRMM_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("MasterModules/", data).
                        then(function (promise) {
                          
                            if (promise.returnval == "Save" && promise.returnduplicatestatus != "Duplicate") {
                                swal('Record Saved Successfully', 'success');
                                $state.reload();
                            }
                            else if (promise.returnval == "NotSave" && promise.returnduplicatestatus != "Duplicate") {
                                swal('Record Not Saved');
                                $state.reload();
                            }
                            else if (promise.returnval == "Update" && promise.returnduplicatestatus != "Duplicate") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnval == "NotUpdate" && promise.returnduplicatestatus != "Duplicate") {
                                swal('Record Not Updated');
                                $state.reload();
                            }
                            else if (promise.returnduplicatestatus == "Duplicate") {
                                swal('Module Name Already Exist');
                            }
                          

                        })
            };
        }
    }

})();