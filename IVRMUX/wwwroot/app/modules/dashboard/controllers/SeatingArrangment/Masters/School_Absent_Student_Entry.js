(function () {
    'use strict';
    angular.module('app').controller('School_Absent_Student_EntryController', School_Absent_Student_EntryController)
    School_Absent_Student_EntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function School_Absent_Student_EntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {

        $scope.obj = {};
        $scope.obj.checkall = false;

        var paginationformasters = 0;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.search = "";
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.loaddata = function () {
            var data = 2;
            apiService.getURI("School_Absent_Student_Entry/GetAbsentStudentLoadData", data).then(function (promise) {
                if (promise !== null) {
                    $scope.yearlst = promise.getYearList;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                    $scope.classlist = promise.getClassList;

                    angular.forEach($scope.yearlst, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.mindate = new Date(dd.asmaY_From_Date);
                            $scope.maxdate = new Date(dd.asmaY_To_Date);
                        }
                    });
                }
            });
        };

        $scope.OnChangeYear = function () {
            angular.forEach($scope.yearlst, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    $scope.mindate = new Date(dd.asmaY_From_Date);
                    $scope.maxdate = new Date(dd.asmaY_To_Date);
                }
            });

            $scope.classlist = [];
            $scope.ASMCL_Id = "";
            $scope.sectionlist = [];
            $scope.ASMS_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.ESAABSTUSCH_ExamDate = null;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("School_Absent_Student_Entry/OnChangeYear", data).then(function (promise) {
                $scope.classlist = promise.getClassList;
            });
        };

        $scope.OnChangeClass = function () {
            $scope.sectionlist = [];
            $scope.ASMS_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.ESAABSTUSCH_ExamDate = null;
            $scope.examlist = [];
            $scope.roomlist = [];
            $scope.examslot = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("School_Absent_Student_Entry/OnChangeClass", data).then(function (promise) {
                $scope.sectionlist = promise.getSectionList;
            });
        };

        $scope.OnChangeSection = function () {
            $scope.subjectlist = [];
            $scope.examlist = [];
            $scope.roomlist = [];
            $scope.examslot = [];
            $scope.ISMS_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.ESAABSTUSCH_ExamDate = null;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
            };
            apiService.create("School_Absent_Student_Entry/OnChangeSection", data).then(function (promise) {
                $scope.subjectlist = promise.getSubjectList;
                $scope.examlist = promise.getExamList;
                $scope.roomlist = promise.getRoomList;
                $scope.examslot = promise.getSlotList;
            });
        };

        $scope.OnChangeSubject = function () {
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.ESAABSTUSCH_ExamDate = null;
        };

        $scope.SearchData = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAROOM_Id": $scope.ESAROOM_Id,
                    "ESAABSTUSCH_ExamDate": new Date($scope.ESAABSTUSCH_ExamDate).toDateString()
                };
                apiService.create("School_Absent_Student_Entry/SearchData", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.GetStudentList = promise.getStudentList;
                        $scope.GetSavedStudentList = promise.getSavedStudentList;
                        angular.forEach($scope.GetStudentList, function (dd) {
                            dd.checked = false;
                            dd.ESAABSTUSCH_Id = 0;
                            angular.forEach($scope.GetSavedStudentList, function (d) {
                                if (dd.amsT_Id === d.amsT_Id) {
                                    dd.checked = true;
                                    dd.ESAABSTUSCH_Id = d.esaabstuscH_Id;
                                }
                            });
                        });
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.SaveData = function () {

            if ($scope.myForm2.$valid) {

                $scope.tempstudents = [];

                angular.forEach($scope.GetStudentList, function (d) {
                    if (d.checked) {
                        $scope.tempstudents.push({ AMST_Id: d.amsT_Id, ESAABSTUSCH_Id: d.ESAABSTUSCH_Id });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAROOM_Id": $scope.ESAROOM_Id,
                    "tempstudents": $scope.tempstudents,
                    "ESAABSTUSCH_ExamDate": new Date($scope.ESAABSTUSCH_ExamDate).toDateString(),
                };
                apiService.create("School_Absent_Student_Entry/SaveData", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnValue === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.OnChangeall = function (checkall) {
            var toggleStatus = checkall;
            angular.forEach($scope.GetStudentList, function (itm) {
                itm.checked = toggleStatus;
            });
        };

        $scope.OnChangeInd = function () {
            $scope.obj.checkall = $scope.GetStudentList.every(function (itm) { return itm.checked; });
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.GetStudentList.some(function (options) {
                return options.checked;
            });
        };
    }
})();


