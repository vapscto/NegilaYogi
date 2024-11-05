(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterSubjectController', MasterSubjectController)

    MasterSubjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MasterSubjectController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //=====================Load data..........
        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            debugger;
            var pageid = 2;
            apiService.getURI("MasterSubject/getdetails", pageid).then(function (promise) {
                $scope.parentsublist = promise.parentsublist;
                $scope.alldata = promise.alldata;
            })
        }
        //=====================End-----Load-data----//



          //=====================saverecord....
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var data = {
                    "LMS_Id": $scope.lmS_Id,
                    "LMS_SubjectName": $scope.lmS_SubjectName,
                    "LMS_SubjectNo": $scope.lmS_SubjectNo,
                    "LMS_ParentId": $scope.lmS_ParentId,
                    "LMS_ClassNo": $scope.lmS_ClassNo,
                    "LMS_Level": $scope.lmS_Level,
                }
                apiService.create("MasterSubject/Savedata", data)
                    .then(function (promise) {
                        if (promise.returnval != null && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.lmS_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.lmS_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }

                    })

            }
            else {
                $scope.submitted = true;
            }
        };
           //=====================End---saverecord....


        //=====================Edit-record....
        $scope.EditData = function (user) {
            debugger;
            $scope.lmS_Id = user.lmS_Id;
            $scope.lmS_ParentId = user.lmS_ParentId;
            $scope.lmS_SubjectName = user.lmS_SubjectName;
            $scope.lmS_SubjectNo = user.lmS_SubjectNo;
            $scope.lmS_ClassNo = user.lmS_ClassNo;
            $scope.lmS_Level = user.lmS_Level;
         
        }
            //====================End---edit-record....
       


         //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {
            debugger;
            $scope.lmS_Id = user.lmS_Id;

            var dystring = "";
            if (user.lmS_ActiveFlg == 1) {
                dystring = "Deactivate";
            }
            else if (user.lmS_ActiveFlg == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterSubject/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
          //================End----Activation/Deactivation--Record.........


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


        $scope.Clearid = function () {
            $state.reload();
        }
   


    }
})();

