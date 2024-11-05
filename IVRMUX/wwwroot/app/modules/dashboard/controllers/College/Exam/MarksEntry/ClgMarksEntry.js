(function () {
    'use strict';
    angular.module('app').controller('ClgMarksEntryController', ClgMarksEntryController)
    ClgMarksEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ClgMarksEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.studentListTemp = [];

        $scope.MarkCalculation = false;
     
        $scope.BindData = function () {
            apiService.getDATA("ClgMarksEntry/getalldetails").then(function (promise) {
                $scope.yearlist = promise.getyear;
                $scope.gridOptions.data = "";
            });
        };

        $scope.onchangeyear = function () {
            $scope.studentListTemp = [];

            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semisters_list = [];
            $scope.section_List = [];
            $scope.exam_list = [];
            $scope.subject_list = [];
            $scope.subjectsheme = [];
            $scope.subjectshemetype = [];

            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ClgMarksEntry/onchangeyear", data).then(function (promise) {
                $scope.course_list = promise.courseslist;
            });
        };

        $scope.onchangecourse = function () {

            $scope.branch_list = [];
            $scope.semisters_list = [];
            $scope.section_List = [];
            $scope.exam_list = [];
            $scope.subject_list = [];
            $scope.subjectsheme = [];
            $scope.subjectshemetype = [];
            $scope.studentListTemp = [];

            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ClgMarksEntry/onchangecourse", data).then(function (promise) {
                $scope.branch_list = promise.branchlist;
            });
        };

        // ON CLICK BRANCH //
        $scope.addbranch = function () {
            $scope.semisters_list = [];
            $scope.section_List = [];
            $scope.exam_list = [];
            $scope.subject_list = [];
            $scope.subjectsheme = [];
            $scope.subjectshemetype = [];
            $scope.studentListTemp = [];
            $scope.selectedbranchlist = [];

            angular.forEach($scope.branch_list, function (dd) {
                if (dd.selected === true) {
                    $scope.selectedbranchlist.push({ AMB_Id: dd.amB_Id });
                }
            });
            if ($scope.selectedbranchlist.length > 0) {
                $scope.onchangebranch();
            }
        };

        $scope.onchangebranch = function () {
            $scope.AMSE_Id = "";
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "marksbranch": $scope.selectedbranchlist
            };
            apiService.create("ClgMarksEntry/onchangebranch", data).then(function (promise) {
                $scope.semisters_list = promise.semisters;
            });
        };

        $scope.addsection = function () {
            $scope.selectedsectionlist = [];
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.ISMS_ID = "";

            angular.forEach($scope.section_List, function (dd) {
                if (dd.selected === true) {
                    $scope.selectedsectionlist.push({ ACMS_Id: dd.acmS_Id });
                }
            });            
        };

        $scope.get_exams = function () {
            $scope.ACMS_Id = "";
            $scope.ISMS_ID = "";
            $scope.EME_Id = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";
            $scope.exam_list = [];
            $scope.section_List = [];

            $scope.studentListTemp = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "marksbranch": $scope.selectedbranchlist
            };
            apiService.create("ClgMarksEntry/get_exams", data).then(function (promise) {
                $scope.exam_list = promise.examlist;
                $scope.section_List = promise.sectionlist;
                $scope.subjectgrp_list = promise.subjectgrplist;
            });
        };

        $scope.get_subjects = function () {        
            $scope.subject_list = [];
            $scope.subjectsheme = [];
            $scope.subjectshemetype = [];
            $scope.studentListTemp = [];

            $scope.ISMS_ID = "";
            $scope.ACSS_Id = "";
            $scope.ACST_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "marksbranch": $scope.selectedbranchlist,
                "markssection": $scope.selectedsectionlist
                 
            };
            apiService.create("ClgMarksEntry/get_subjects", data).then(function (promise) {
                $scope.subject_list = promise.subjectgroups;
            });
        };

        $scope.getsubjectscheme = function () {
            $scope.ACSS_Id = "";

            $scope.subjectsheme = [];
            $scope.subjectshemetype = [];
            $scope.studentListTemp = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "marksbranch": $scope.selectedbranchlist,
                "markssection": $scope.selectedsectionlist,
                "ISMS_Id": $scope.ISMS_ID,
                "EME_Id": $scope.EME_Id

            };
            apiService.create("ClgMarksEntry/getsubjectscheme", data).then(function (promise) {
                $scope.subjectsheme = promise.getsubjectschemetype;
            });
        };

        $scope.getsubjectschemetype = function () {
            $scope.ACST_Id = "";
            $scope.subjectshemetype = [];
            $scope.studentListTemp = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "marksbranch": $scope.selectedbranchlist,
                "markssection": $scope.selectedsectionlist,
                "ISMS_Id": $scope.ISMS_ID,
                "ACSS_Id": $scope.ACSS_Id,
                "EME_Id": $scope.EME_Id
            };
            apiService.create("ClgMarksEntry/getsubjectschemetype", data).then(function (promise) {
                $scope.subjectshemetype = promise.getschemetype;
            });
        }; 

        $scope.onsearch = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            $scope.studentListTemp = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": AMCO_Id,
                    "AMSE_Id": AMSE_Id,
                    "markssection": $scope.selectedsectionlist,
                    "marksbranch": $scope.selectedbranchlist,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id

                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ClgMarksEntry/onsearch", data).then(function (promise) {
                   
                    $scope.subMorGFlag = promise.subMorGFlag;
                    $scope.gradname = promise.gradname;
                    $scope.marksdeleteflag = false;
                    $scope.masterinst = promise.configuration;

                    $scope.colarrayall = [];
                    var count = 0;

                    $scope.colarrayall = [
                        { name: 'SLNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                        { name: 'studentname', displayName: 'Student Name', width: 300, cellClass: 'textleft' }
                    ];

                    if ($scope.masterinst !== null && $scope.masterinst.length > 0) {

                        if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {
                            $scope.colarrayall.push({ field: 'amcsT_AdmNo', displayName: 'Adm.NO', width: 100 });
                            $scope.admno = true;
                            count = count + 1;
                        } else {
                            $scope.admno = false;
                        }

                        if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {
                            $scope.colarrayall.push({ field: 'amcsT_RegistrationNo', displayName: 'Reg.NO', width: 100 });
                            $scope.regno = true;
                            count = count + 1;
                        } else {
                            $scope.regno = false;
                        }

                        if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {
                            $scope.colarrayall.push({ field: 'acysT_RollNo', displayName: 'Roll.NO', width: 100 });
                            $scope.rollno = true;
                            count = count + 1;
                        } else {
                            $scope.rollno = false;
                        }

                        if (count === 0) {
                            $scope.colarrayall.push({ field: 'amcsT_AdmNo', displayName: 'Adm.NO', width: 100 });
                            $scope.colarrayall.push({ field: 'acysT_RollNo', displayName: 'Roll.NO', width: 100 });
                            $scope.admno = true;
                            $scope.rollno = true;
                        }
                    } else {
                        $scope.colarrayall.push({ field: 'amcsT_AdmNo', displayName: 'Adm.NO', width: 100 });
                        $scope.colarrayall.push({ field: 'acysT_RollNo', displayName: 'Roll.NO', width: 100 });
                        $scope.admno = true;
                        $scope.rollno = true;
                    }

                    $scope.colarrayall.push({ name: 'totalMarks', displayName: 'Max Marks', width: 100},);
                    $scope.colarrayall.push({ name: 'minMarks', displayName: 'Min Marks', width: 100 });
                    $scope.colarrayall.push({ name: 'marksEnterFor', displayName: 'Marks Enter For', width: 120 });
                    $scope.colarrayall.push({ name: 'obtainmarks', displayName: 'Marks Obtained', cellTemplate: '<div class="ui-grid-cell-contents"><input type="text" ng-model="row.entity.obtainmarks"  style="text-align:center;" allow-pattern="[A-Z0-9+-.]" ng-blur="grid.appScope.changemarks(row.entity.marksEnterFor,row.entity.obtainmarks,row)"  class="form-control" value="0"  min="0" </input></div>' });

                    console.log($scope.colarrayall);

                    $scope.gridOptions = {
                        enableColumnMenus: false,
                        enableFiltering: true,
                        enableHorizontalScrollbar: 0,
                        enableVerticalScrollbar: 0,
                        columnDefs: $scope.colarrayall
                    };

                    $scope.studentListTemp = promise.studentList;

                    $scope.gridOptions.data = promise.studentList;

                    $scope.gridOptions.onRegisterApi = function (gridApi) {
                        $scope.gridApi = gridApi;
                    };

                    console.log(promise.studentList);

                    if (promise.studentList === null || promise.studentList.length === 0) {
                        swal("Students Details Not Found");
                    }

                });
            }
        };

        $scope.submitted = false;

        $scope.SaveMarks = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var flag = "false";

                if ($scope.marksdeleteflag == "true") {
                    swal({
                        title: "Are You Sure?",
                        text: "Marks Already Calculated, If You Update The Marks You Need To Recalculate Marks Again..Do You Want To Continue ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Update It!",
                        cancelButtonText: "Cancel..!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.save(ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id);
                            }
                            else {
                                flag = "false";
                            }
                        });
                }
                else {
                    $scope.save(ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id);
                }
            }
            else {
                swal('Please select required field / Entered Marks are not in correct format....!');
            }
        };

        $scope.save = function (ACMS_Id, AMCO_Id, AMSE_Id, AMB_Id, EME_Id, ISMS_Id) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.savedata = $scope.gridOptions.data;
                var message = "";
                if ($scope.MarkCalculation == true) {
                    message = "Autocalculate";
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "markssection": $scope.selectedsectionlist,
                    "AMCO_Id": AMCO_Id,
                    "AMSE_Id": AMSE_Id,
                    "marksbranch": $scope.selectedbranchlist,
                    "EME_Id": EME_Id,
                    "ISMS_Id": ISMS_Id,
                    "ACSS_Id": $scope.ACSS_Id,
                    "ACST_Id": $scope.ACST_Id,
                    "marksdeleteflag": $scope.marksdeleteflag,
                    "detailsList": $scope.savedata,
                    "message": message

                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ClgMarksEntry/SaveMarks", data).then(function (promise) {
                    if (promise.messagesaveupdate === "true") {
                        $scope.cancle();
                        swal('Data Saved /Updated Successfully');
                    }
                    else {
                        swal('Failed to Save/Update Data');
                    }
                });
            }
        };

        $scope.cancle = function () {
            $state.reload();
        };


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};

        //TO  GEt The Values iN Grid
        $scope.changemarks = function (totalMarks, obtainmarks, row) {
            var flag = "";
            if ($scope.subMorGFlag === "G") {

                flag = "false";

                for (var i = 0; i < $scope.gradname.length; i++) {
                    if ($scope.gradname[i] === obtainmarks) {
                        flag = "true";
                    }
                }

                if (obtainmarks === "AB" || obtainmarks === "ab" || obtainmarks === "L" || obtainmarks === "l" || obtainmarks === "M" || obtainmarks === "m") {
                    flag = "true";
                }

                if (flag === "false") {
                    row.entity.obtainmarks = 0;
                    swal('Entered Grade cant be out of master setting...!');
                }
            }
            else {
                flag = "false";

                if (obtainmarks.match(/[a-z]/i)) {
                    if (obtainmarks === "AB" || obtainmarks === "ab" || obtainmarks === "L" || obtainmarks === "l" || obtainmarks === "M" || obtainmarks === "m") {
                        flag = "true";
                    }
                    if (flag === "false") {
                        row.entity.obtainmarks = 0;
                        swal('Entered value cant be out of master setting...!');
                    }
                }
                else {
                    if (totalMarks < obtainmarks) {
                        row.entity.obtainmarks = 0;
                        swal('Entered marks cant be more than Marks Enter For...!');
                    }
                    if (obtainmarks < 0) {
                        row.entity.obtainmarks = 0;
                        swal('Entered marks cant be in nagative values...!');
                    }
                }
            }
        };


        $scope.onselectSubject = function () {
            $scope.gridOptions.data = "";
        };

        $scope.cancel = function () {
            $state.reload();
            $scope.BindData();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.isOptionsRequired11 = function () {
            return !$scope.branch_list.some(function (options1) {
                return options1.selected;
            });
        };

        $scope.isOptionsRequired12 = function () {
            return !$scope.section_List.some(function (options1) {
                return options1.selected;
            });
        };
    }

})();