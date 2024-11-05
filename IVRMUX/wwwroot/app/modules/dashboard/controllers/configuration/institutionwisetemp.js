
(function () {
    'use strict';

    angular.module('app').controller('institutionwisetempController', institutionwisetempController);

    institutionwisetempController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q','superCache'];

    function institutionwisetempController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $route, $q, superCache) {

        $scope.getBasicDropdowns = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5

            apiService.getURI("InstitutionTemplate/getbasicdropdowns", 2).then(function (promise) {
                $scope.instituteList = promise.instituteList;
                $scope.templates = promise.templates;
                $scope.InstTempList = promise.instituteTemplates;

                $scope.predicate = 'sno';
                $scope.reverse = false;
                $scope.currentPage = 1;
                $scope.order = function (predicate) {
                    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
                    $scope.predicate = predicate;
                };
                $scope.arrlist9 = promise.instituteTemplates;
                $scope.totalItems = $scope.arrlist9.length;
                $scope.numPerPage = 10;
                $scope.paginate = function (value) {
                    var begin, end, index;
                    begin = ($scope.currentPage - 1) * $scope.numPerPage;
                    end = begin + $scope.numPerPage;
                    index = $scope.arrlist9.indexOf(value);
                    return (begin <= index && index < end);
                };

            });

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        function LoadBasicData()
        {
            apiService.getURI("InstitutionTemplate/getbasicdropdowns", 2).then(function (promise) {
                $scope.instituteList = promise.instituteList;
                $scope.templates = promise.templates;
                $scope.InstTempList = promise.instituteTemplates;

                $scope.predicate = 'sno';
                $scope.reverse = false;
                $scope.currentPage = 1;
                $scope.order = function (predicate) {
                    $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
                    $scope.predicate = predicate;
                };
                $scope.arrlist9 = promise.instituteTemplates;
                $scope.totalItems = $scope.arrlist9.length;
                $scope.numPerPage = 10;
                $scope.paginate = function (value) {
                    var begin, end, index;
                    begin = ($scope.currentPage - 1) * $scope.numPerPage;
                    end = begin + $scope.numPerPage;
                    index = $scope.arrlist9.indexOf(value);
                    return (begin <= index && index < end);
                };

            });
        }

        $scope.edit = function (Id) {
            apiService.getURI("InstitutionTemplate/getEditData", Id).then(function (promise) {
                $scope.ivrmiT_Id = promise.instEditlist[0].ivrmiT_Id;
                $scope.ivrmT_Id = promise.instEditlist[0].ivrmT_Id;
                $scope.ivrmiT_MI_Id = promise.instEditlist[0].ivrmiT_MI_Id;
                //$scope.ivrmiT_Category_Id = promise.instEditlist[0].ivrmiT_Category_Id;
                //$scope.ivrmiT_ActiveFlag = promise.instEditlist[0].ivrmiT_ActiveFlag;
                //$scope.ivrmiT_DeleteFlag = promise.instEditlist[0].ivrmiT_DeleteFlag;
                //LoadBasicData();
            })
        }

        //delete record
        $scope.delete = function (id, SweetAlert) {
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("InstitutionTemplate/deletedetails", id).
                    then(function (promise) {
                        swal('Record Deleted Successfully..!', 'success');
                        LoadBasicData();
                    })
                }
                else {
                    swal("Record Deletion Cancelled", "Ok");
                }
            });
        }

        $scope.deactive = function (user, SweetAlert) {

            var status = user.ivrmiT_ActiveFlag;
            var msg = '';
            var msgResp = '';
            if (status == 0) {
                msg = "Activate";
                msgResp = "Activated";
            }
            else {
                msg = "Deactivate";
                msgResp = "Deactivated";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + msg + " record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + msg + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("InstitutionTemplate/deactive", user.ivrmiT_Id).
                    then(function (promise) {
                        swal('Record ' + msgResp + '  successfully', 'success');
                        LoadBasicData();
                    })
                }
                else {
                    swal("Record " + msg + "  Cancelled");
                }
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.saveInstituteTemplate = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "IVRMIT_Id": $scope.ivrmiT_Id,
                    "IVRMIT_MI_Id": $scope.ivrmiT_MI_Id,
                    "IVRMT_Id": $scope.ivrmT_Id,
                    "IVRMIT_Category_Id": $scope.ivrmiT_Category_Id,
                    "IVRMIT_ActiveFlag": $scope.ivrmiT_ActiveFlag,
                    "IVRMIT_DeleteFlag": $scope.ivrmiT_DeleteFlag
                }

                apiService.create("InstitutionTemplate/", data).then(function (promise) {
                    if (promise != null) {

                        if (promise.returnMsg != "" && promise.returnMsg != null) {

                            if (promise.returnMsg == "duplicate")
                            {
                                swal("Record already exist");
                                return;
                            }
                            else if (promise.returnMsg == "add") {
                                swal("Template Added successfully");
                                $state.reload();
                            }
                            else if (promise.returnMsg == "update") {
                                swal("Template Updated successfully");
                                $state.reload();
                            }
                            else if (promise.returnMsg == "error") {
                                swal('Something went wrong', 'Try again later');
                                $state.reload();
                            }
                          
                        } else {
                            swal('Something went wrong', 'Try again later');
                            $state.reload();
                        }

                    }
                    else {
                        swal('Something went wrong', 'Try again later');
                        $state.reload();
                    }
                   
                });
            }
            else {
                $scope.submitted = true;
            }
           
        }


        $scope.clearid = function () {
            $state.reload();
        }
    }
})();
