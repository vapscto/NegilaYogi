
(function () {
    'use strict';
    angular.module('app').controller('IndividualExam_StudentWiseMarksPublishController', IndividualExam_StudentWiseMarksPublishController)
    IndividualExam_StudentWiseMarksPublishController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams']
    function IndividualExam_StudentWiseMarksPublishController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams) {

        $scope.publis = false;
        $scope.submitted = false;
        $scope.obj = {};
        $scope.obj.SMSFLAG = false;
        $scope.obj.EMAILFLAG = false;
        $scope.obj.feeinstallmentcheckbox = false;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        $scope.BindData = function () {
            apiService.getDATA("ExamCalculation_SSSE/Getdetails").then(function (promise) {
                $scope.publis = false;
                $scope.yearlt = promise.yearlist;
                $scope.Feetermlist = promise.feetermlist;
                $scope.Feegrouplist = promise.feegrouplist;

                $scope.all = true;
                angular.forEach($scope.Feegrouplist, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };

        $scope.onselectradio = function () {
            $scope.datareport = [];
            $scope.obj.emE_Id = "";
            $scope.submitted = false;
        };

        $scope.OnAcdyear = function (ASMAY_Id) {
            $scope.publis = false;
            $scope.obj.SMSFLAG = false;
            $scope.obj.EMAILFLAG = false;
            $scope.submitted = false;
            $scope.datareport = [];
            var data = {
                "ASMAY_Id": $scope.obj.asmaY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamCalculation_SSSE/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classlist;
                $scope.obj.asmcL_Id = "";
                $scope.obj.asmS_Id = "";
                $scope.obj.emE_Id = "";
                $scope.seclist = [];
                $scope.exsplt = [];
                if (promise.classlist === null || promise.classlist === "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }

            });
        };

        $scope.get_cls_sections = function () {
            $scope.publis = false;
            $scope.obj.SMSFLAG = false;
            $scope.obj.EMAILFLAG = false;
            $scope.submitted = false;
            $scope.datareport = [];
            if ($scope.obj.asmaY_Id !== "" && $scope.obj.asmaY_Id !== undefined && $scope.obj.asmaY_Id !== null) {
                var data = {
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "ASMCL_Id": $scope.obj.asmcL_Id
                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.obj.asmS_Id = "";
                    $scope.obj.emE_Id = "";
                    $scope.exsplt = [];
                    if (promise.seclist === null || promise.seclist === "") {
                        swal("Sections are Not Mapped To Selected Class!!!");
                    }
                });
            }
            else {
                swal("Please Select Academic Year  First !!!");
                $scope.asmcL_Id = "";
            }
        };

        $scope.ChangeOfSection = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {
            $scope.publis = false;
            $scope.obj.SMSFLAG = false;
            $scope.obj.EMAILFLAG = false;
            $scope.submitted = false;
            $scope.datareport = [];
            var data = {
                "ASMS_Id": $scope.obj.asmS_Id,
                "ASMCL_Id": $scope.obj.asmcL_Id,
                "ASMAY_Id": $scope.obj.asmaY_Id,
                "radiotype": $scope.radiotype,
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamCalculation_SSSE/ChangeOfSection", data).then(function (promise) {
                if ($scope.radiotype === "Exam") {
                   
                    $scope.exsplt = promise.exmstdlist;
                    if (promise.exmstdlist === null || promise.exmstdlist === "") {
                        swal("Exams are Not Mapped To Selected Class And Section!!!");
                    }
                } else {
                    if (promise.editlist !== null && promise.editlist.length > 0) {
                        $scope.processbtn = true;
                    } else {
                        swal("Marks Calculation Not Done");
                        $scope.processbtn = false;
                        $scope.publis = false;
                    }
                }

            });
        };


        $scope.CheckMarksCalculated = function () {
            $scope.datareport = [];
            $scope.submitted = false;
            if ($scope.obj.emE_Id > 0) {
                $scope.publis = false;
                $scope.obj.SMSFLAG = false;
                $scope.obj.EMAILFLAG = false;
                $scope.datareport = [];
                var data = {
                    "ASMS_Id": $scope.obj.asmS_Id,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "EME_Id": $scope.obj.emE_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ExamCalculation_SSSE/CheckMarksCalculated", data).then(function (promise) {
                    if (promise.editlist !== null && promise.editlist.length > 0) {
                        $scope.processbtn = true;
                    } else {
                        swal("Marks Calculation Not Done");
                        $scope.processbtn = false;
                        $scope.publis = false;
                    }
                });
            }
        };

        $scope.OnChangeInstallment = function () {
            $scope.datareport = [];
            $scope.obj.SMSFLAG = false;
            $scope.obj.EMAILFLAG = false;
            $scope.submitted = false;
        };        

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.SearchStudent = function (obj) {
            $scope.datareport = [];
            $scope.obj.SMSFLAG = false;
            $scope.obj.EMAILFLAG = false;
            if ($scope.myForm.$valid) {
                var fmTId = 0;
                $scope.TempGroupList = [];
                if (obj.feeinstallmentcheckbox) {
                    fmTId = obj.FMT_Id;

                    angular.forEach($scope.Feegrouplist, function (dd) {
                        $scope.TempGroupList.push(dd.fmG_Id);
                    });
                }
                var data = {
                    "ASMAY_Id": obj.asmaY_Id,
                    "ASMCL_Id": obj.asmcL_Id,
                    "ASMS_Id": obj.asmS_Id,
                    "EME_Id": $scope.radiotype === "Exam" ? obj.emE_Id : 0,
                    "radiotype": $scope.radiotype,
                    "FMT_Id": fmTId,
                    "TempGroupList": $scope.TempGroupList,
                    "feeinstallmentcheckbox": obj.feeinstallmentcheckbox,

                };
                apiService.create("ExamCalculation_SSSE/SearchStudent", data).then(function (promise) {
                    if (promise.datareport !== null && promise.datareport.length > 0) {
                        $scope.datareport = promise.datareport;

                        angular.forEach($scope.datareport, function (dd) {
                            dd.checkedvalue = false;
                        });
                        $scope.all123 = false;
                    }
                    else {
                        swal('Student Details Not Found');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.SaveStudentStatus = function (saveobj) {
            $scope.selectedstudents = [];

            angular.forEach($scope.datareport, function (dd) {
                if (dd.checkedvalue === true) {
                    $scope.selectedstudents.push({
                        AMST_Id: dd.amsT_Id, ESTMP_Id: dd.estmP_Id, AMST_FirstName: dd.amsT_FirstName,
                        ESTMP_PublishToStudentFlg: dd.estmP_PublishToStudentFlg
                    });
                }
            });

            if ($scope.selectedstudents.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.obj.asmaY_Id,
                    "ASMCL_Id": $scope.obj.asmcL_Id,
                    "ASMS_Id": $scope.obj.asmS_Id,
                    "EME_Id": $scope.radiotype === "Exam" ? $scope.obj.emE_Id : 0,
                    "radiotype": $scope.radiotype,
                    "SMSFLAG": $scope.obj.SMSFLAG,
                    "EMAILFLAG": $scope.obj.EMAILFLAG,
                    "selectedstudentsmarks": $scope.selectedstudents
                };
                apiService.create("ExamCalculation_SSSE/SaveStudentStatus", data).then(function (promise) {
                    if (promise.returnval === true) {
                        $scope.datareport = [];
                        $scope.selectedstudents = [];
                        swal("Records Updated");
                    } else {
                        swal("Failed To Update Record");
                    }
                });
            } else {
                swal("Select The Students To Change The Status");
            }
        };

        $scope.cancel = function () {
            $scope.obj.asmaY_Id = "";
            $scope.obj.asmcL_Id = "";
            $scope.obj.asmS_Id = "";
            $scope.obj.emE_Id = "";
            $scope.processbtn = false;
            $scope.datareport = [];
            $scope.selectedstudents = [];
            $scope.clslist = [];
            $scope.seclist = [];
            $scope.exsplt = [];
            $scope.radiotype = 'Exam';
        };

        $scope.optionToggled123 = function () {
            $scope.obj.all123 = $scope.datareport.every(function (itm) { return itm.checkedvalue; });
        };

        $scope.toggleAll123 = function (all123) {
            angular.forEach($scope.datareport, function (dd) {
                dd.checkedvalue = all123;
            });
        };


        $scope.OnClickAll = function () {
            angular.forEach($scope.Feegrouplist, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.Feegrouplist.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.Feegrouplist.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

    }
})();