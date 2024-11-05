
(function () {
    'use strict';
    angular
.module('app')
.controller('SubjectController', SubjectController)

    SubjectController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function SubjectController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.MasterSubjectCl = function () {
            var pageid = 2;
            apiService.getURI("MasterSubject/getalldetails", pageid).
        then(function (promise) {
            $scope.masterSubject = promise.masterSubjectData;
        })
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.DeletRecord = function (employee, SweetAlert) {
             
            $scope.editEmployee = employee.pamS_Id;
            var orgaid = $scope.editEmployee
            var mgs = "";
            if (employee.pamS_ActiveFlag == 0) {
                mgs = "Enable";
            }
            else {
                mgs = "Disable";
            }

            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Subject !!!!!!!!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,  " + mgs + " it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.DeleteURI("MasterSubject/Deletedetails", orgaid).
                   then(function (promise) {

                       $scope.masterSubject = promise.masterSubjectData;


                       if (promise.returnval == true) {
                           swal('Record Disabled Successfully!', 'success');
                       }
                       else if (promise.returnval == false) {
                           swal('Record Enabled Successfully!', 'success');
                       }
                       else {
                           swal('Record Not Disabled/ Enabled Successfully!', 'Failed');
                       }

                       // $scope.orgname = promise.organisationname;
                   })
               }
               else {
                   swal("Record Disable/ Enable Cancelled", "Failed");
               }
           });


            //})
        }

        $scope.cance = function () {
            $state.reload();

            //$scope.PAMS_Id = "";
            //$scope.PAMS_SubjectName = "";
            //$scope.MI_Id = "";
            //$scope.PAMS_MaxMarks = "";
            //$scope.PAMS_SubjectCode = "";
            //$scope.PAMS_MinMarks = "";
            //$scope.PAMS_SubjectFlag = "";
            //$scope.PAMS_ActiveFlag = "";
            ////$scope.MO_Landmark = "";

        }


        $scope.EditMasterSubvalue = function (employee) {
            $scope.editEmployee = employee.pamS_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("MasterSubject/Editdetails", orgid).
            then(function (promise) {

                $scope.PAMS_Id = promise.masterSubjectData[0].pamS_Id;
                $scope.MI_Id = promise.masterSubjectData[0].mI_Id;
                $scope.PAMS_SubjectName = promise.masterSubjectData[0].pamS_SubjectName;
                $scope.PAMS_SubjectCode = promise.masterSubjectData[0].pamS_SubjectCode;
                $scope.PAMS_MinMarks = promise.masterSubjectData[0].pamS_MinMarks;
                $scope.PAMS_MaxMarks = promise.masterSubjectData[0].pamS_MaxMarks;
                $scope.PAMS_SubjectFlag = promise.masterSubjectData[0].pamS_SubjectFlag;
                $scope.PAMS_ActiveFlag = promise.masterSubjectData[0].pamS_ActiveFlag;



                if (promise.masterSubjectData[0].pamS_SubjectFlag == 'W') {

                    $scope.writt = true;
                }
                else
                    if (promise.masterSubjectData[0].pamS_SubjectFlag == 'O')
                {

                        $scope.ora = true;
                }

            })
        }

        $scope.submitted = false;
        $scope.saveMasterdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var activeflag = $scope.PAMS_SubjectFlag
                if (activeflag == 'opteritten') {
                    activeflag = 'W';
                }
                else
                    if (activeflag == 'optoral') {
                        activeflag = 'O';
                    }
                var data = {
                    //"MI_Id": 2,
                    "PAMS_SubjectName": $scope.PAMS_SubjectName,
                    "PAMS_SubjectCode": $scope.PAMS_SubjectCode,
                    "PAMS_Minmarks": $scope.PAMS_MinMarks,
                    "PAMS_MaxMarks": $scope.PAMS_MaxMarks,
                    "PAMS_SubjectFlag": activeflag,
                    "PAMS_Id": $scope.PAMS_Id

                }

                apiService.create("MasterSubject/", data).
                then(function (promise) {
                    if (promise.returnval == "Duplicate") {
                        swal('Record already exists', 'Duplicate!');
                        $state.reload();
                        return;
                    } else {
                        swal('Record Saved/Updated Successfully', 'success');
                        $state.reload();
                    }
                })

            }
        };


    }

})();