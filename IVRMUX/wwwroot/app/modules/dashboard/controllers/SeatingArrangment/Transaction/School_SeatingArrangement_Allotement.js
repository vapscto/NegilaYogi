(function () {
    'use strict';
    angular.module('app').controller('School_SeatingArrangement_AllotmentController', School_SeatingArrangement_AllotmentController)

    School_SeatingArrangement_AllotmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function School_SeatingArrangement_AllotmentController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {

        $scope.tempcldrlst = [];
        $scope.searchbtn = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.obj = {};
        $scope.obj.searchValue = '';
        //$scope.ESAEDATESCH_ExamDate = new Date();

        $scope.GetRoomList = [];
        $scope.GetExamDateloaddata = function () {
            var id = 1;
            apiService.getURI("School_Exam_Date_Room/SchoolSAAllotmentloaddata", id).then(function (promise) {
                $scope.yearlst = promise.getAcademicYearList;
                $scope.examlist = promise.getExamList;              
                $scope.slotlist = promise.getExamSlotList;
                $scope.ASMAY_Id = promise.asmaY_Id;
            });
        };

        $scope.OnChangeyear = function () {
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            $scope.EME_Id = "";            
            $scope.ESAESLOT_Id = "";           
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.OnChangeexam = function () {
            $scope.GetRoomList = [];             
            $scope.ESAUE_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.GetaSavedRoomDetails = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
        };      

        $scope.OnChangeslot = function () {
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
        };      

        $scope.GetSearchExamDateData = function () {
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,                    
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAEDATESCH_ExamDate": new Date($scope.ESAEDATESCH_ExamDate).toDateString()
                };
                apiService.create("School_Exam_Date_Room/SchoolGenerateSeatAllotment", data).then(function (promise) {
                    if (promise !== null) {

                        if (promise.returnValue === true) {
                            swal("Successfully Alloted Seats");
                        } else {
                            swal("Failed Alloted Seats");
                        }
                        $scope.cleardata();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        }; 

        $scope.cleardata = function () {
            $scope.ASMAY_Id = "";            
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";          
            $scope.ESAEDATESCH_ExamDate = null;           
            $scope.yearlst = [];
            $scope.examlist = [];            
            $scope.submitted = false;            
            $scope.slotlist = [];         
            $scope.GetExamDateloaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();