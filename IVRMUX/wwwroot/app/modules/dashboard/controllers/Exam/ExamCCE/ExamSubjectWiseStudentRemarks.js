(function () {
    'use strict';
    angular.module('app').controller('ExamSubjectWiseStudentRemarksController', ExamSubjectWiseStudentRemarksController)
    ExamSubjectWiseStudentRemarksController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamSubjectWiseStudentRemarksController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.edit = false;
        $scope.submitted = false;

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };

        $scope.studentdataload = function () {
            apiService.getDATA("exammasterRemak/Subjectwise_studentdataload").then(function (promise) {
                $scope.gridOptions.data = promise.loaddata;
                $scope.loaddata = promise.loaddata;
                $scope.yearlist = promise.yearlist;
                $scope.classlist = promise.classlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
            });
        };

        $scope.onchangeyear = function () {
            $scope.ASMCL_Id = "";
            $scope.classlist = [];
            $scope.ASMS_Id = "";
            $scope.sectionlist = [];
            $scope.EME_Id = "";
            $scope.examlist = [];
            $scope.ISMS_Id = "";
            $scope.subjectlist = [];
            $scope.EP_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            };
            apiService.create("exammasterRemak/Subjectwise_onchangeyear", data).then(function (promise) {
                if (promise != null) {
                    if (promise.classlist != null) {
                        $scope.classlist = promise.classlist;
                    } else {
                        swal("No Record Found");
                    }
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.ASMS_Id = "";
            $scope.sectionlist = [];
            $scope.EME_Id = "";
            $scope.examlist = [];
            $scope.ISMS_Id = "";
            $scope.subjectlist = [];
            $scope.EP_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("exammasterRemak/Subjectwise_onchangeclass", data).then(function (promise) {
                if (promise != null) {
                    if (promise.sectionlist != null) {
                        $scope.sectionlist = promise.sectionlist;
                    } else {
                        swal("No Record Found");
                    }
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.EME_Id = "";
            $scope.examlist = [];
            $scope.ISMS_Id = "";
            $scope.subjectlist = [];
            $scope.EP_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("exammasterRemak/Subjectwise_onchangesection", data).then(function (promise) {
                if (promise != null) {
                    $scope.examlist = promise.examlist;
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.onchangeexam = function () {
            $scope.ISMS_Id = "";
            $scope.subjectlist = [];
            $scope.EP_Id = "";
            $scope.studentList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "EME_Id": $scope.EME_Id
            };
            apiService.create("exammasterRemak/Subjectwise_onchangeexam", data).then(function (promise) {
                if (promise != null) {
                    if (promise.subjectlist != null) {
                        $scope.subjectlist = promise.subjectlist;
                    } else {
                        swal("No Record Found");
                    }
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.onchangesubject = function () {
            $scope.studentList = [];
        };

        $scope.searchdata = function () {
            $scope.studentList = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ISMS_Id
                };
                apiService.create("exammasterRemak/Subjectwise_searchdata", data).then(function (promise) {
                    if (promise != null) {
                        var count = 0;
                        $scope.sortKey = "studentname";

                        $scope.configurationsettings = promise.configuration;
                        if ($scope.configurationsettings !== null && $scope.configurationsettings.length > 0) {
                            if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "Name") {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "AdmNo") {
                                $scope.sortKey = "amsT_AdmNo";
                            }
                            else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "RollNo") {
                                $scope.sortKey = "amaY_RollNo";
                            }
                            else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "RegNo") {
                                $scope.sortKey = "amsT_RegistrationNo";
                            }
                        }
                        $scope.regno = false;
                        $scope.admno = false;
                        $scope.rollno = false;

                        if ($scope.configurationsettings !== null && $scope.configurationsettings.length > 0) {
                            if ($scope.configurationsettings[0].exmConfig_RegnoColumnDisplay == true) {
                                $scope.regno = true;
                                count = count + 1;
                            }
                            if ($scope.configurationsettings[0].exmConfig_AdmnoColumnDisplay == true) {
                                $scope.admno = true;
                                count = count + 1;
                            }
                            if ($scope.configurationsettings[0].exmConfig_RollnoColumnDisplay == true) {
                                $scope.rollno = true;
                                count = count + 1;
                            }
                        }

                        if (count == 0) {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        if (promise.studentList != null && promise.studentList.length > 0) {
                            $scope.studentList = promise.studentList;
                            $scope.savedata = promise.savedata;
                            $scope.getstudentmarks = promise.getstudentmarks;

                            angular.forEach($scope.studentList, function (ddd) {
                                angular.forEach($scope.savedata, function (dddd) {
                                    if (ddd.amsT_Id === dddd.amsT_Id) {
                                        ddd.remarks = dddd.essepcR_Remarks;
                                        ddd.ESPCSR_Id = dddd.essepcR_Id;
                                        ddd.checked = true;
                                    }
                                });

                                angular.forEach($scope.getstudentmarks, function (mrks) {
                                    if (ddd.amsT_Id === mrks.amsT_Id) {
                                        ddd.obtainedmarks = mrks.estmpS_ObtainedMarks;
                                        ddd.totalmaxmarks = mrks.estmpS_MaxMarks;
                                        ddd.percentage = mrks.estmpS_Percentage;
                                    }
                                });
                            });

                        } else {
                            swal("No Records Found");
                        }

                    } else {
                        swal("No Records Found");
                    }
                    console.log($scope.studentList);

                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.save = function () {
            $scope.SelectedStudentScore = [];

            if ($scope.myForm1.$valid) {
                $scope.SelectedStudentScore.length = 0;
                for (var i = 0; i < $scope.studentList.length; i++) {
                    if ($scope.studentList[i].checked == true) {
                        $scope.SelectedStudentScore.push({
                            "AMST_Id": $scope.studentList[i].amsT_Id,
                            "EMER_Remarks": $scope.studentList[i].remarks
                        });
                    }
                }

                var obj = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "Temp_studentdata_Remarks": $scope.SelectedStudentScore
                };

                apiService.create("exammasterRemak/SubjectWise_savemapping", obj).then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record Saved /Update Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal("Failed to save / Update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'asmcL_ClassName', displayName: 'Class' },
                { name: 'asmC_SectionName', displayName: 'Section' },
                { name: 'emE_ExamName', displayName: 'Exam' },
                { name: 'ismS_SubjectName', displayName: 'Subject' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.Editexammasterdata(row.entity);"><md-tooltip md-direction="down">Edit</md-tooltip> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }

        };

        $scope.resetLists = function () {
            $scope.configA = {
                onUpdate: function (evt) {
                    var itemEl = evt.item;
                }
            };
        };

        $scope.init = function () {
            $scope.resetLists();
        };

        $scope.init();

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {
            $scope.studentList = [];
            $scope.submitted = true;
            apiService.create("exammasterRemak/SubjectWise_editmappingdetails", EditRecord).then(function (promise) {
                if (promise != null) {
                    var count = 0;
                    $scope.sortKey = "studentname";
                    $scope.configurationsettings = promise.configuration;
                    if ($scope.configurationsettings !== null && $scope.configurationsettings.length > 0) {
                        if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "Name") {
                            $scope.sortKey = "studentname";
                        }
                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "AdmNo") {
                            $scope.sortKey = "amsT_AdmNo";
                        }
                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "RollNo") {
                            $scope.sortKey = "amaY_RollNo";
                        }
                        else if ($scope.configurationsettings[0].exmConfig_Recordsearchtype == "RegNo") {
                            $scope.sortKey = "amsT_RegistrationNo";
                        }
                    }
                    $scope.regno = false;
                    $scope.admno = false;
                    $scope.rollno = false;

                    if ($scope.configurationsettings != null && $scope.configurationsettings.length > 0) {
                        if ($scope.configurationsettings[0].exmConfig_RegnoColumnDisplay == true) {
                            $scope.regno = true;
                            count = count + 1;
                        }
                        if ($scope.configurationsettings[0].exmConfig_AdmnoColumnDisplay == true) {
                            $scope.admno = true;
                            count = count + 1;
                        }
                        if ($scope.configurationsettings[0].exmConfig_RollnoColumnDisplay == true) {
                            $scope.rollno = true;
                            count = count + 1;
                        }
                    }
                    if (count == 0) {
                        $scope.admno = true;
                        $scope.rollno = true;
                    }

                    $scope.yearlist = promise.yearlist;
                    $scope.ASMAY_Id = $scope.yearlist[0].asmaY_Id;
                    $scope.classlist = promise.classlist;
                    $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;
                    $scope.sectionlist = promise.sectionlist;
                    $scope.ASMS_Id = $scope.sectionlist[0].asmS_Id;
                    $scope.examlist = promise.examlist;
                    $scope.EME_Id = $scope.examlist[0].emE_Id;
                    $scope.subjectlist = promise.subjectlist;
                    $scope.ISMS_Id = $scope.subjectlist[0].ismS_Id;
                    $scope.edit = true;

                    if (promise.studentList != null && promise.studentList.length > 0) {
                        $scope.studentList = promise.studentList;
                        $scope.savedata = promise.savedata;
                        $scope.getstudentmarks = promise.getstudentmarks;
                        angular.forEach($scope.studentList, function (ddd) {
                            angular.forEach($scope.savedata, function (dddd) {
                                if (ddd.amsT_Id === dddd.amsT_Id) {
                                    ddd.remarks = dddd.essepcR_Remarks;
                                    ddd.ESSEPCR_Id = dddd.essepcR_Id;
                                    ddd.checked = true;
                                }
                            });

                            angular.forEach($scope.getstudentmarks, function (mrks) {
                                if (ddd.amsT_Id === mrks.amsT_Id) {
                                    ddd.obtainedmarks = mrks.estmpS_ObtainedMarks;
                                    ddd.totalmaxmarks = mrks.estmpS_MaxMarks;
                                    ddd.percentage = mrks.estmpS_Percentage;
                                }
                            });
                        });

                        $scope.scroll();

                    } else {
                        swal("No Records Found");
                    }

                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.studentList.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.interacted1 = function (field1) {
            return $scope.submitted1;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.toggleAll_S = function (stas) {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.studentList, function (itm) {
                itm.checked = toggleStatus;

            });
        };

        $scope.optionToggled_S = function () {
            $scope.checkall = $scope.studentList.every(function (itm) { return itm.checked; });
        };
    }
})();