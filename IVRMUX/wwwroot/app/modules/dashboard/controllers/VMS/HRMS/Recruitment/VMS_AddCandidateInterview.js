(function () {
    'use strict';
    angular
        .module('app')
        .controller('vmsAddCandidateInterviewController', vmsAddCandidateInterviewController);

    vmsAddCandidateInterviewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter'];
    function vmsAddCandidateInterviewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter) {

        $scope.addjob = {};

        $scope.getserverdate = function () {
            var xmlHttp;
            function srvTime() {
                try {
                    //FF, Opera, Safari, Chrome
                    xmlHttp = new XMLHttpRequest();
                }
                catch (err1) {
                    //IE
                    try {
                        xmlHttp = new ActiveXObject('Msxml2.XMLHTTP');
                    }
                    catch (err2) {
                        try {
                            xmlHttp = new ActiveXObject('Microsoft.XMLHTTP');
                        }
                        catch (eerr3) {
                            //AJAX not supported, use CPU time.
                            alert("AJAX not supported");
                        }
                    }
                }
                xmlHttp.open('HEAD', window.location.href.toString(), false);
                xmlHttp.setRequestHeader("Content-Type", "text/html");
                xmlHttp.send('');
                return xmlHttp.getResponseHeader("Date");
            }
            $scope.today = srvTime();
        }
        $scope.getserverdate();

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            var pageid = 2;
            $scope.taskDate = new Date($scope.today);
            apiService.getURI("AddCandidateInterviewVMS/getalldetails", pageid).then(function (promise) {

                if (promise.candidateDetailsList !== null && promise.candidateDetailsList.length > 0) {
                    $scope.candidateDetailsList = promise.candidateDetailsList;
                }

                if (promise.interviewerList !== null && promise.interviewerList.length > 0) {
                    $scope.interviewerList = promise.interviewerList;
                }
            });            
        };

        // clear form data
        $scope.cancel = function () {
            $state.reload();
        };

        // Edit Single Record
        $scope.EditData = function (record) {
            var id = record.hrcisC_Id;
            apiService.getURI("AddCandidateInterviewVMS/editRecord", id).
                then(function (promise) {

                });
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.savejob = function () {
            if ($scope.myForm.$valid) {
                var datevar = $scope.mrfReq.hrcisC_InterviewDateTime == null ? "" : $filter('date')($scope.mrfReq.hrcisC_InterviewDateTime, "yyyy-MM-dd");
                $scope.mrfReq.hrcisC_InterviewDateTime = datevar;
                $scope.mrfReq.hrcD_Id = $scope.hrcD_Id.hrcD_Id;
                $scope.mrfReq.hrcisC_Interviewer = $scope.hrcisC_Interviewer.id;
                var data = $scope.mrfReq;
                swal({
                    title: "Are you sure?",
                    text: "Do you want to schedule interview ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,schedule !",
                    cancelButtonText: "Cancel..!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {

                            apiService.create("AddCandidateInterviewVMS/", data).
                                then(function (promise) {
                                    if (promise.retrunMsg !== "") {
                                        if (promise.retrunMsg === "Duplicate") {
                                            swal("Record already exist..!!");
                                            return;
                                        }
                                        else if (promise.retrunMsg === "false") {
                                            swal("Record Not saved / Updated..", 'Fail');
                                        }
                                        else if (promise.retrunMsg === "Add") {
                                            swal("Interview Scheduled Successfully.");
                                            $scope.cancel();
                                        }
                                        else if (promise.retrunMsg === "Update") {
                                            swal("Interview Updated Successfully.");
                                            $scope.cancel();
                                        }
                                        else if (promise.retrunMsg === "Notification") {
                                            swal("Notification!");
                                            $scope.cancel();
                                        }
                                        else {
                                            swal("Something went wrong ..!", 'Kindly contact Administrator');
                                        }
                                    }
                                });
                        }
                        else {
                            swal.close();
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };
    }
})();