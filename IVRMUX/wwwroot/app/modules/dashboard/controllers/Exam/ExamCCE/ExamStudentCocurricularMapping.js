
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamStudentCOCourricularmappingController', ExamStudentCOCourricularmappingController)

    ExamStudentCOCourricularmappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamStudentCOCourricularmappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.edit = false;       

        $scope.studentdataload = function () {
            apiService.getDATA("exammasterCoCurricular/studentdataload").
                then(function (promise) {
                    $scope.gridOptions.data = promise.loaddata;
                    $scope.exammasterpersonalityname = promise.personlaitylist;
                    $scope.yearlist = promise.yearlist;
                    $scope.examlist = promise.examlist;
                });
        };

        $scope.onchangeyear = function () {

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.EP_Id = "";
            $scope.IVRM_Month_Id = "";
            $scope.studentList = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            };
            apiService.create("exammasterCoCurricular/onchangeyear", data).then(function (promise) {

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
            $scope.EP_Id = "";
            $scope.IVRM_Month_Id = "";
            $scope.studentList = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("exammasterCoCurricular/onchangeclass", data).then(function (promise) {
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
            $scope.EP_Id = "";
            $scope.IVRM_Month_Id = "";
            $scope.studentList = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };
            apiService.create("exammasterCoCurricular/onchangesection", data).then(function (promise) {
                if (promise != null) {
                    $scope.monthlist = promise.monthlist;
                    if (promise.personlaitylist != null) {
                        $scope.cocurricularlist = promise.personlaitylist;
                    } else {
                        swal("No cocurricular Data Found ");
                    }
                } else {
                    swal("No Record Found");
                }
            });
        };

        $scope.submitted = false;

        $scope.searchdata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ECC_Id": $scope.ECC_Id
                };
                apiService.create("exammasterCoCurricular/searchdata", data).then(function (promise) {

                    if (promise != null) {

                        var count = 0;
                        $scope.configurationsettings = promise.configuration;

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

                        else {
                            $scope.sortKey = "studentname";
                        }



                        if ($scope.configurationsettings != null) {
                            if ($scope.configurationsettings.length > 0) {
                                if ($scope.configurationsettings[0].exmConfig_RegnoColumnDisplay == true) {
                                    $scope.regno = true;
                                    count = count + 1;

                                } else {
                                    $scope.regno = false;
                                }

                                if ($scope.configurationsettings[0].exmConfig_AdmnoColumnDisplay == true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                } else {
                                    $scope.admno = false;
                                }

                                if ($scope.configurationsettings[0].exmConfig_RollnoColumnDisplay == true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.rollno = false;
                                }

                                if (count == 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }
                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }

                        } else {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        if (promise.studentList != null) {
                            $scope.studentList = promise.studentList;
                            $scope.savedata = promise.savedata;
                            angular.forEach($scope.studentList, function (ddd) {
                                angular.forEach($scope.savedata, function (dddd) {

                                    if (ddd.amsT_Id === dddd.amsT_Id) {
                                        ddd.remarks = dddd.escoM_Remarks;
                                        ddd.checked = true;
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


        $scope.isOptionsRequired = function () {
            return !$scope.studentList.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted1 = function (field1) {
            return $scope.submitted1;
        };


        $scope.save = function () {
            $scope.SelectedStudentScore = [];

            if ($scope.myForm1.$valid) {
                $scope.SelectedStudentScore.length = 0;
                for (var i = 0; i < $scope.studentList.length; i++) {
                    if ($scope.studentList[i].checked == true) {
                        $scope.SelectedStudentScore.push({
                            "AMST_Id": $scope.studentList[i].amsT_Id,
                            "ESCO_Remarks": $scope.studentList[i].remarks
                        });
                    }
                }

                var obj = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "ECC_Id": $scope.ECC_Id,
                    "Temp_studentdata": $scope.SelectedStudentScore
                };

                apiService.create("exammasterCoCurricular/savemapping", obj).then(function (promise) {
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
                { name: 'ecC_CoCurricularName', displayName: 'CoCurricular Name' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'asmcL_ClassName', displayName: 'Class' },
                { name: 'asmC_SectionName', displayName: 'Section' },
                { name: 'monthname', displayName: 'Exam' },
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
        //fix the order drag
        //ConfigA is an Items
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


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.IsHidden1 = true;

        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        };

        $scope.IsHidden2 = true;

        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        };   

        // TO Save The Data
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "EP_Id": $scope.per_Id,
                    "EP_PersonlaityCode": $scope.per_Code,
                    "EP_PersonlaityName": $scope.per_Name
                };

                apiService.create("exammasterCoCurricular/savedetails", data).
                    then(function (promise) {
                        $scope.newuser = promise.exammastername;

                        if (promise.returnval === true) {
                            if (promise.per_Id === 0 || promise.per_Id < 0) {
                                swal('Record saved successfully');
                            }
                            else if (promise.per_Id > 0) {
                                swal('Record updated successfully');
                            }

                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record already exist');
                        }
                        else {
                            if (promise.per_Id === 0 || promise.per_Id < 0) {
                                swal('Failed to save, please contact administrator');
                            }
                            else if (promise.per_Id > 0) {
                                swal('Failed to update, please contact administrator');
                            }
                        }
                        $scope.cancel();
                        $scope.BindData();
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        // to Edit Data
        $scope.Editexammasterdata = function (EditRecord) {

            apiService.create("exammasterCoCurricular/editmappingdetails", EditRecord).
                then(function (promise) {

                    if (promise != null) {

                        var count = 0;
                        $scope.configurationsettings = promise.configuration;

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

                        else {
                            $scope.sortKey = "studentname";
                        }



                        if ($scope.configurationsettings != null) {
                            if ($scope.configurationsettings.length > 0) {
                                if ($scope.configurationsettings[0].exmConfig_RegnoColumnDisplay == true) {
                                    $scope.regno = true;
                                    count = count + 1;

                                } else {
                                    $scope.regno = false;
                                }

                                if ($scope.configurationsettings[0].exmConfig_AdmnoColumnDisplay == true) {
                                    $scope.admno = true;
                                    count = count + 1;
                                } else {
                                    $scope.admno = false;
                                }

                                if ($scope.configurationsettings[0].exmConfig_RollnoColumnDisplay == true) {
                                    $scope.rollno = true;
                                    count = count + 1;
                                } else {
                                    $scope.rollno = false;
                                }

                                if (count == 0) {
                                    $scope.admno = true;
                                    $scope.rollno = true;
                                }
                            } else {
                                $scope.admno = true;
                                $scope.rollno = true;
                            }

                        } else {
                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                        $scope.yearlist = promise.yearlist;
                        $scope.ASMAY_Id = $scope.yearlist[0].asmaY_Id;

                        $scope.classlist = promise.classlist;
                        $scope.ASMCL_Id = $scope.classlist[0].asmcL_Id;

                        $scope.sectionlist = promise.sectionlist;
                        $scope.ASMS_Id = $scope.sectionlist[0].asmS_Id;

                        $scope.cocurricularlist = promise.personlaitylist;
                        $scope.ECC_Id = $scope.cocurricularlist[0].ecC_Id;

                        $scope.examlist = promise.examlist;
                        $scope.EME_Id = $scope.examlist[0].emE_Id;

                        $scope.edit = true;

                        if (promise.studentList != null) {
                            $scope.studentList = promise.studentList;
                            $scope.savedata = promise.savedata;
                            angular.forEach($scope.studentList, function (ddd) {
                                angular.forEach($scope.savedata, function (dddd) {

                                    if (ddd.amsT_Id === dddd.amsT_Id) {
                                        ddd.remarks = dddd.escoM_Remarks;
                                        ddd.checked = true;
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