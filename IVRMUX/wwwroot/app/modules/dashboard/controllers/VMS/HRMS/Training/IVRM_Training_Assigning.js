(function () {
    'use strict';
    angular.module('app').controller('IVRM_Training_AssigningController', IVRM_Training_AssigningController)

    IVRM_Training_AssigningController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q']
    function IVRM_Training_AssigningController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q) {


        $scope.submitted = false;
        $scope.obj = {};
        $scope.hstep = 1;
        $scope.mstep = 1;

        // time//////////////////////////////////////////////////////

        $scope.validatemax = function (maxdata) {

            var dsttimee = $scope.obj.IVRMTT_TentetiveStartTime;
            $scope.sresult = $filter('date')(dsttimee, 'HH:mm:ss a');
            $scope.eresult = $filter('date')(maxdata, 'HH:mm:ss a');
            var startTime = moment($scope.sresult, "HH:mm:ss a");
            var endTime = moment($scope.eresult, "HH:mm:ss a");
            var duration = moment.duration(endTime.diff(startTime));
            var hours = parseInt(duration.asHours());
            var minutes = parseInt(duration.asMinutes()) - hours * 60;
            var finlrst = hours + ":" + minutes;

            $scope.tmin = new Date($scope.today);
            $scope.tmin.setHours(hours);
            $scope.tmin.setMinutes(minutes);
            $scope.tmax = new Date($scope.today);
            $scope.tmax.setHours(hours);
            $scope.tmax.setMinutes(minutes);

            $scope.ttst = new Date($scope.today);
            $scope.ttst.setHours(hours);
            $scope.ttst.setMinutes(minutes);

            $scope.htmax = new Date($scope.today);
            $scope.htmax.setHours(hours);
            $scope.htmax.setMinutes(minutes);

            if (maxdata >= new Date($scope.obj.IVRMTT_TentetiveStartTime)) {
                $scope.totimemax = maxdata;
                var hh = $scope.totimemax.getHours();
                var mm = $scope.totimemax.getMinutes();
                $scope.max = maxdata;

                $scope.max.setMinutes(hh);
                $scope.max.setMinutes(mm);
            }
            else {
                $scope.obj.IVRMTT_TentetiveEndTime = 0;
            }
        };

        $scope.validateTomintime = function (timedata) {

            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;
            $scope.min.setHours(hh);
            $scope.min.setMinutes(mm);
            $scope.minlnc = timedata;
            $scope.minlnc.setHours(hh);
            $scope.minlnc.setMinutes(mm);
            $scope.obj.IVRMTT_TentetiveEndTime = 0;
        };
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.obj.IVRMTT_TrainingDate = new Date();
        //end//////////////////////////////////////////////////////////////////////////////////////////////////////////
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IVRTM_Training/assignonload", pageid).then(function (promise) {
                $scope.getloadgrid = promise.getloaddetails;
                $scope.gridassigned = promise.griddata;
                $scope.get_emp = promise.emp_deatils;
            });
        };

        $scope.saveData = function (obj) {
            if ($scope.myForm.$valid) {
                $scope.IVRMTMT_Status = 'Pending';
                var data = {
                    "IVRMTT_Id": $scope.IVRMTT_Id,
                    "IVRMTMT_Id": $scope.IVRMTMT_Id,
                    "IVRMTMT_TrainerId": obj.hrmE_Id.hrmE_Id,
                    "IVRMTMT_TrainerName": obj.hrmE_Id.hrmE_EmployeeFirstName,
                    "IVRMTMT_TrainerEmail": obj.hrmE_Id.hrmeM_EmailId,
                    "IVRMTT_TrainerPhone": obj.hrmE_Id.hrmemnO_MobileNo,
                    "IVRMTMT_Status": $scope.IVRMTMT_Status,
                    "IVRMTT_TrainingStartTime": $filter('date')(obj.IVRMTT_TentetiveStartTime, "HH:mm"),
                    "IVRMTT_TrainingToTime": $filter('date')(obj.IVRMTT_TentetiveEndTime, "HH:mm"),
                    "IVRMTT_TrainingDate": new Date($scope.IVRMTT_TrainingDate).toDateString(),
                };
                apiService.create("IVRTM_Training/saveassign", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                        else if (promise.message == "Add") {

                            swal("Record Saved Successfully");
                        }
                        else if (promise.message == "Update") {

                            swal("Record Updated Successfully");
                        }
                        else if (promise.message == "Error") {
                            swal("Failed To Save/ Update Record");
                        } else {
                            swal("Something Went Wrong Contact Administrator");
                        }
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.EditData = function (obj) {
            $scope.IVRMTT_Id = obj.IVRMTT_Id;
            $scope.IVRMTT_TentetiveDate = "";
            $scope.IVRMTT_TentetiveStartTime = "";
            $scope.IVRMTT_TentetiveEndTime = "";
            $scope.IVRMTT_TrainingMode = obj.IVRMTT_TrainingMode;
            $scope.IVRMTT_EmployeeName = obj.IVRMTT_EmployeeName;
            $scope.IVRMTT_TentetiveDate = obj.IVRMTT_TentetiveDate;
            $scope.IVRMTT_TentetiveStartTime = moment(obj.IVRMTT_TentetiveStartTime, 'h:mm a').format();
            $scope.IVRMTT_TentetiveEndTime = moment(obj.IVRMTT_TentetiveEndTime, 'h:mm a').format();
            $scope.IVRMTT_TentetiveDate = new Date(obj.IVRMTT_TentetiveDate);
            $scope.obj.IVRMTT_TentetiveStartTime = moment(obj.IVRMTT_TentetiveStartTime, 'h:mm a').format();
            $scope.obj.IVRMTT_TentetiveEndTime = moment(obj.IVRMTT_TentetiveEndTime, 'h:mm a').format();
            apiService.create("IVRTM_Training/EditDetails/", obj).then(function (promise) {
                $scope.appliedlist = promise.trainingdetails;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };



    }
})();

