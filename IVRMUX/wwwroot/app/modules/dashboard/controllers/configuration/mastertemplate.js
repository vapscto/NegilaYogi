
(function () {
    'use strict';
    angular
.module('app')
        .controller('templateController', templateController)

    templateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function templateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.temp = {};
        $scope.onload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            apiService.getURI("MasterTemplate/getalldetails", pageid).
            then(function (promise) {
                $scope.arrlist = promise.pageDrpdwn;
                $scope.students = promise.templateList;
                //$rootScope.$state = favoriteCookie;
            })
            
        }

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.clearid=function()
        {
            $scope.temp = {};
            $scope.search = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.onload();
        }

        $scope.edit = function (employee) {
            $scope.editEmployee = employee.ivrmT_Id;
            var templId = $scope.editEmployee;

            apiService.getURI("MasterTemplate/getdetails", templId).
            then(function (promise) {
                if (promise.templateList != null && promise.templateList.length > 0) {
                    $scope.temp = promise.templateList[0];
                }
               
            })
        }
        $scope.submitted = false;
        $scope.savetmpldata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = $scope.temp;

                apiService.create("MasterTemplate/", data)

                .then(function (promise) {
                    if (promise.returnval == "true") {
                        swal('Record Saved/Updated Successfully', 'success'); 
                        $scope.clearid();
                    }
                    else if (promise.returnval == "duplicate") {
                        swal('Template Name Already exist');
                    }
                    else {
                        swal('Operation Failed', 'Failed');
                    }
                })
            };
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.delete = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ivrmT_Id;
            //var tmplt = employee.ivrmtmL_State_Provider;
            var orgaid = $scope.editEmployee

            var mgs = "";
            var confirmmgs = "";
            if (employee.is_Active == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }


            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("MasterTemplate/deletedetails", orgaid).
                    then(function (promise) {
                        $scope.students = promise.templateList;
                        if (promise.returnval === "Activated") {
                            swal('Record Activated Successfully!', 'success');
                        } else if (promise.returnval === "Deactivated") {
                            swal('Record Deactivated Successfully!', 'success');
                        }
                        else {
                            swal('Record Not Activated /Deactivated', 'Failed');
                        }
                    })
                }
                else {
                    swal(" Cancelled", "Ok");
                }
            });
        }
    }
})();

