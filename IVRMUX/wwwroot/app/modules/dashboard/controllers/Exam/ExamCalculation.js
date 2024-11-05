
(function () {
    'use strict';
    angular.module('app').controller('ExamCalculationController', ExamCalculationController)
    ExamCalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams']
    function ExamCalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams) {

        $scope.publis = false;

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
        $scope.datareport = [];
        $scope.BindData = function () {
            apiService.getDATA("ExamCalculation/Getdetails").then(function (promise) {
                $scope.publis = false;
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.exsplt = promise.exmstdlist;
            });
        };

        $scope.OnAcdyear = function (ASMAY_Id) {
            $scope.publis = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamCalculation_SSSE/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classlist;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.emE_Id = "";
                $scope.seclist = [];
                $scope.exsplt = [];
                if (promise.classlist === null || promise.classlist === "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }

            });
        };

        $scope.get_cls_sections = function () {
            $scope.publis = false;
            if ($scope.asmaY_Id !== "" && $scope.asmaY_Id !== undefined && $scope.asmaY_Id !== null) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id
                };

                apiService.create("ExamCalculation_SSSE/get_cls_sections", data).then(function (promise) {
                    $scope.seclist = promise.seclist;
                    $scope.asmS_Id = "";
                    $scope.emE_Id = "";
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

        $scope.onchangesection = function (ASMAY_Id, ASMCL_Id, ASMS_Id) {
            $scope.publis = false;
            $scope.datareport = [];
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id
            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {
                $scope.emE_Id = "";
                $scope.exsplt = promise.exmstdlist;
                if (promise.exmstdlist === null || promise.exmstdlist === "") {
                    swal("Exams are Not Mapped To Selected Class And Section!!!");
                }
            });
        };

        $scope.butttonClose = function () {
            $('#myModalecs').modal('hide');
        }
        $scope.onchangeexam = function () {
            $scope.asmaY_Year = "";
            $scope.asmcL_ClassName = "";
            $scope.asmC_SectionName = "";
            $scope.emE_ExamName = "";
            $('#myModalecs').modal('hide');
            $scope.datareport = [];
            if ($scope.emE_Id > 0) {
                angular.forEach($scope.yearlt, function (dd) {
                    if (dd.asmaY_Id == $scope.asmaY_Id) {
                        $scope.asmaY_Year = dd.asmaY_Year;
                    }
                   
                });
                angular.forEach($scope.clslist, function (dd) {
                    if (dd.asmcL_Id == $scope.asmcL_Id) {
                        $scope.asmcL_ClassName = dd.asmcL_ClassName;
                    }
                   
                });
                angular.forEach($scope.seclist, function (dd) {
                    if (dd.asmS_Id == $scope.asmS_Id) {
                        $scope.asmC_SectionName = dd.asmC_SectionName;
                    }
                   
                });
                angular.forEach($scope.exsplt, function (dd) {
                    if (dd.emE_Id == $scope.emE_Id) {
                        $scope.emE_ExamName = dd.emE_ExamName;
                    }
                   
                });
                $scope.publis = false;
                var data = {
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "EME_Id": $scope.emE_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ExamCalculation_SSSE/onchangeexam", data).then(function (promise) {
                    if (promise.editlist !== null && promise.editlist.length > 0) {
                        $scope.processbtn = false;
                        $scope.savelist = promise.savelist;
                        // IF PUBLISH DATE IS NULL
                        if ($scope.savelist[0].eycE_MarksPublishDate === null) {

                            // ENABLE OR DISABLE THE PUBLISH BUTTON
                            $scope.publis = promise.publishbtn;

                            $scope.processbtn = true;

                            // CONDITION FOR ALREADY PUBLISH MARKS TO GIVE POPUP CONFIRMATION FOR RECALCULATION  
                            $scope.retuvrnvalpublishflag = promise.markscalculatedflag;

                        } else {
                            $scope.publis = promise.publishbtn;
                            $scope.retuvrnvalpublishflag = promise.markscalculatedflag;
                        }

                        // XHECKING FOR LAST DATE TO CALCULATE MARKS WHEN DATE IS NOT NULL

                        if ($scope.savelist[0].eycE_MarksProcessLastDate !== null) {
                            $scope.processbtn = promise.markslastdateflag;
                            if ($scope.processbtn === false) {
                                swal("You Can Not Calculate The Marks Last Date For Calculation Is Completed");
                            }
                        } else {
                            $scope.processbtn = true;
                        }
                        // datareport
                       
                        if (promise.datareport != null && promise.datareport.length > 0) {                           
                            angular.forEach(promise.datareport, function (dd) {
                                if (dd.SourceMarksColorFlag !="Green") {
                                    $scope.datareport.push(dd);                                   
                                }
                              
                            });
                            if ($scope.datareport != null && $scope.datareport.length > 0) {
                                $('#myModalecs').modal('show');
                            }
                            
                        }
                    }
                 
                    else {
                        swal("Marks Entry Not Done For This Selection");
                        $scope.processbtn = true;
                        $scope.publis = false;

                    }

                               

                });
            }
        };


        $scope.submitted = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.saveddata = function (asmaY_Id, asmcL_Id, asmS_Id, emE_Id) {
            if ($scope.myForm.$valid) {
                if ($scope.retuvrnvalpublishflag === true) {
                    swal({
                        title: "Marks Are Already Updated To Student Portal",
                        text: "Do you want to Recalculate The Marks Again ??",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Calculate!",
                        cancelButtonText: "No, Cancel Calculation!",
                        closeOnConfirm: false,
                        closeOnCancel: false
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                var ESTMP_PublishToStudentFlg = 0;
                                var data = {
                                    "ASMAY_Id": asmaY_Id,
                                    "ASMCL_Id": asmcL_Id,
                                    "ASMS_Id": asmS_Id,
                                    "EME_Id": emE_Id,
                                    "ESTMP_PublishToStudentFlg": ESTMP_PublishToStudentFlg
                                };
                                apiService.create("ExamCalculation/Calculation", data).then(function (promise) {
                                    if (promise.returnval === true) {
                                        swal('Marks Calculated Successfully');
                                        $state.reload();
                                    }
                                    else {
                                        swal('Marks Not Calculated !!!');
                                        $state.reload();
                                    }
                                });
                            }
                            else {
                                swal("Marks Calculation Process Is Cancelled");
                            }
                        });
                } else {

                    var ESTMP_PublishToStudentFlg = 0;
                    var data = {
                        "ASMAY_Id": asmaY_Id,
                        "ASMCL_Id": asmcL_Id,
                        "ASMS_Id": asmS_Id,
                        "EME_Id": emE_Id,
                        "ESTMP_PublishToStudentFlg": ESTMP_PublishToStudentFlg
                    };
                    apiService.create("ExamCalculation/Calculation", data).then(function (promise) {

                        if (promise.returnval === true) {
                            swal('Marks Calculated Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Marks Not Calculated !!!');
                            $state.reload();
                        }
                    });
                }
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.saveapporvecal = function (asmaY_Id, asmcL_Id, asmS_Id, emE_Id) {
            if ($scope.myForm.$valid) {
                swal({
                    title: "Do you want to Publish The Marks To Students Portals",
                    //  text: "Do you want to Publish The Marks To Students Portals ??",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Publish!",
                    cancelButtonText: "No, Cancel Publish!",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var ESTMP_PublishToStudentFlg = 0;
                            var data = {
                                "ASMAY_Id": asmaY_Id,
                                "ASMCL_Id": asmcL_Id,
                                "ASMS_Id": asmS_Id,
                                "EME_Id": emE_Id
                            };
                            apiService.create("ExamCalculation_SSSE/saveapporvecal", data).then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Marks Published Successfully');
                                    $state.reload();
                                }
                                else {
                                    swal('Failed To Published Marks');
                                    $state.reload();
                                }
                            });
                        }
                        else {
                            swal("Marks Publish Process Is Cancelled");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }

})();