(function () {
    'use strict';
    angular.module('app').controller('masterreligionController', masterreligionController)

    masterreligionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$filter', 'superCache']
    function masterreligionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $filter, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.loadgrid = function () {
           
            var pageid = 1;
            apiService.getURI("Master_ExamQualified_Class/getalldata", pageid).then(function (promise) {
                $scope.ExamQualifiedClass = promise.examQualifiedClass
                

            });
           
        }

        $scope.saveClassdata = function () {
           
            var data = {
                "IMQC_Id": $scope.IMQC_Id,
                    "IMQC_ExamName": $scope.IMQC_ExamName,

                    
                };
            apiService.create("Master_ExamQualified_Class/SaveClass", data).then(function (promise) {


                    if (promise.message == "saved") {
                        swal("Records saved successfully");
                    }
                    else if (promise.message == "Notsaved") {
                        swal("Record Not saved !");
                }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exist !");
                    }
                    else if (promise.message == "admin") {
                        swal("Please Contact Administror !");
                    }
                $state.reload();
                });
           

        }


        $scope.editdata = function (qualified) {
            $scope.qualifiedid = qualified.imqC_Id;
            var orgid = $scope.qualifiedid;
           
            apiService.getURI("Master_ExamQualified_Class/Editdetails", orgid).
                then(function (promise) {
                    $scope.EditExamQualifiedClass = promise.editExamQualifiedClass;
                    $scope.IMQC_Id = promise.editExamQualifiedClass[0].imqC_Id;
                    $scope.IMQC_ExamName = promise.editExamQualifiedClass[0].imqC_ExamName;

                })
        }


        $scope.deactiveCat = function (Qualified, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (Qualified.imqC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Master_ExamQualified_Class/deactiveCat/", Qualified).then(function (promise) {
                            if (promise.message != null) {
                                swal(promise.message);
                            }
                            else {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "Successfully");
                                    $state.reload();
                                }
                                else {
                                    swal(confirmmgs + " " + " Successfully");
                                    $state.reload();
                                }
                            }
                            $scope.onpageload();

                        });

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }

                });
        }
    }
})();