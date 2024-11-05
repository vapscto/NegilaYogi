
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterRoleTypeController', MasterRoleTypeController)

    MasterRoleTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache']
    function MasterRoleTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {

        var HostName = location.host;

        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/rolemaster/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/privileges/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
        };

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            var pageid = 2;
            apiService.getURI("MasterRoleType/getalldetails", pageid).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            $scope.arrlist2 = promise.roledata;
            //$scope.totalItems = $scope.pages.length;
            //$scope.numPerPage = 5;
        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        $scope.isOptionsRequired = function () {
            return !$scope.arrlist2.some(function (option) {
                return option.Id;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchsource = function () {
            var entereddata = $scope.search;

            var data = {
                "IVRMRT_Role": $scope.search,
                "IVRMRT_RoleFlag": $scope.type
            }

            apiService.create("MasterRoleType/1", data).
        then(function (promise) {
            $scope.pages = promise.pagesdata;
            swal("searched Successfully");
        })
        }

        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmrT_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterRoleType/deletepages", pageid).
                    then(function (promise) {

                        $scope.pages = promise.pagesdata;

                        if (promise.returnval === "true") {
                            swal('Record Deleted Successfully!');

                            $state.reload();
                        }
                        else if (promise.returnval === "false") {
                            swal('Record Not Deleted!');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }

        $scope.getorgvalue = function (employee, arrlist2) {
            $scope.editEmployee = employee.ivrmrT_Id;
            var pageid = $scope.editEmployee;

            for (var i = 0; i < arrlist2.length; i++) {
                name = arrlist2[i].Id
                if (name == true) {
                    arrlist2[i].Id = "";
                }
            }

            apiService.getURI("MasterRoleType/getdetails", pageid).
            then(function (promise) {
                
                $scope.IVRMRT_Role = promise.pagesdata[0].ivrmrT_Role;
                $scope.IVRMRT_Id = promise.pagesdata[0].ivrmrT_Id;

                for (var i = 0; i < arrlist2.length; i++) {
                    name = arrlist2[i].id
                    if (name == promise.pagesdata[0].id) {
                        arrlist2[i].Id = true
                    }
                }

                $scope.Id = promise.pagesdata[0].id;
                $scope.VI = promise.pagesdata[0].flag;

            })
        }

        var name = "";
        var roleflag = "";

        $scope.clearfields = function (arrlist2) {
            $scope.IVRMRT_Role = "";
            $scope.IVRMRT_Id = "";
             for (var i = 0; i < arrlist2.length; i++) {
                 name = arrlist2[i].Id
                if(name==true)
                {
                    arrlist2[i].Id = "";
                }
             }
             $scope.submitted = false;
             $scope.myForm.$setPristine();
             $scope.myForm.$setUntouched();
             $state.reload();
        }
        $scope.binddata = function (position, arrlist2) {
            angular.forEach(arrlist2, function (option, index) {
                if (position != index)
                    option.Id = false;
            });
        }
        $scope.roledat = {};
        $scope.submitted = false;
        $scope.savepages = function (arrlist2) {
            if($scope.myForm.$valid){
                for (var i = 0; i < arrlist2.length; i++) {
                    name = arrlist2[i].Id
                    if (name == true) {
                        $scope.Id = arrlist2[i].id
                        roleflag = arrlist2[i].ivrmR_Role
                    }
                }

                var data = {
                    "IVRMRT_Role": $scope.IVRMRT_Role,
                    "IVRMR_Id": $scope.Id,
                    "IVRMRT_RoleFlag": $scope.IVRMRT_Role,
                    "IVRMRT_Id": $scope.IVRMRT_Id,
                    "flag": $scope.VI
                }

                apiService.create("MasterRoleType/", data).
                then(function (promise) {

                    if (promise.returnval === "Save" || promise.returnval === "Update") {

                        if (promise.returnval == "Save") {
                            swal('Record Saved Successfully', 'success');
                            $state.reload();
                        }
                        else if(promise.returnval == "Update")
                        {
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
                    else if (promise.returnval == "Duplicate")
                    {
                        swal('Master Roletype Already Exist');

                    }

                })
            }
    else{
        $scope.submitted=true;
        }
           
        };
    }

})();
