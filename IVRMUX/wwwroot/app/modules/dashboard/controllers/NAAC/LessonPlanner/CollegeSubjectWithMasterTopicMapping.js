(function () {
    'use strict';
    angular.module('app').controller('CollegeSubjTopicMappingController', CollegeSubjTopicMappingController)
    CollegeSubjTopicMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$sce', '$window']
    function CollegeSubjTopicMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $sce, $window) {

        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.teacherdocuupload = {};
        $scope.btn = false;

        //$scope.file1 = "https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf";
        //$scope.file2 = "https://bdcampusstrg.blob.core.windows.net/files/4/LessonPlanner/880039db-67c2-466b-a770-585b572beda2.pdf";

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
            var file = "";
            document.getElementById("pdfviewdd").innerHTML = "";
            $http.get(imagedownload, { responseType: 'arraybuffer' })
                .success(function (response) {
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);
                    $scope.content = $sce.trustAsResourceUrl(fileURL);
                    var htmlElements = '<embed id="pdfview" src="' + $scope.content + '"  style="width: 100%;" height="1000" type="application/pdf" />';
                    document.getElementById("pdfviewdd").innerHTML = htmlElements;

                    $('#showpdf').modal('show');
                });
        };

        $scope.onviewppt = function (filepath, filename) {
            var imagedownload = filepath;
            $scope.contantppt = "";
            var fileURL = "";
            var file = "";
            $scope.contantppt = $sce.trustAsResourceUrl(filepath);
            $('#showppt').modal('show');
        };

        $scope.BindData = function () {
            var id = 2;

            $scope.ASMAY_IdNew = "";
            $scope.AMB_IdNew = "";
            $scope.AMSE_IdNew = "";
            $scope.LPMMTC_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.ISMS_IdNew = "";
            $scope.subjectlistnew = [];
            $scope.mastersemesternew = [];
            $scope.mastercoursenew = [];
            $scope.masterbranchnew = [];
            $scope.masterunitnew = [];
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.AMCO_IdNew = "";
            $scope.mastercoursenew = [];

            apiService.getURI("SchoolSubjectWithMasterTopicMapping/Getcollegedetails", id).then(function (promise) {
                if (promise !== null) {
                    $scope.masteryear = promise.getyear;
                    $scope.masteryearnew = promise.getyear;
                    $scope.gridOptions.data = promise.getdetails;
                }
            });
        };

        //Table 
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SLNO', displayName: 'SL NO', width: 100, enableFiltering: false, enableSorting: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year', width: 200 },
                { name: 'amcO_CourseName', displayName: 'Course', width: 200 },
                { name: 'amB_BranchName', displayName: 'Branch', width: 200 },
                { name: 'amsE_SEMName', displayName: 'Semester', width: 200 },
                { name: 'ismS_SubjectName', displayName: 'Subject Name', width: 200 },
                { name: 'lpmU_UnitName', displayName: 'Unit', width: 200 },
                { name: 'lpmmtC_TopicName', displayName: 'Main Topic', width: 100 },
                { name: 'lpmtC_TopicName', displayName: 'Topic Name', width: 100 },
                { name: 'lpmtC_LessonPlan', displayName: 'Description', width: 100 },
                { name: 'lpmtC_TotalHrs', displayName: 'Total Hours', width: 100 },
                { name: 'lpmtC_TotalPeriods', displayName: 'Total Periods', width: 100 },
                {
                    name: 'teacher', displayName: 'Teacher Guide', width: 100, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewteacherguides(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'student', displayName: 'Student Guide', width: 100, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewstudentguides(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'material', displayName: 'Material', width: 100, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewmaterials(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    name: 'reference', displayName: 'References', width: 100, enableFiltering: false, enableSorting: false, cellTemplate: '<div class="grid-action-cell">' +
                        '<a   href="javascript:void(0)" data-toggle="modal" data-target="#popup11" data-backdrop="static" ng-click="grid.appScope.viewrefernce(row.entity);"> <i class="fa fa-eye text-blue" ></i></a>  &nbsp; &nbsp;' + '</div>'
                },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', width: 100, enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                        '<a ng-if="row.entity.lpmtC_Activefalg === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmtC_Activefalg === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
                // $scope.gridApi.core.refresh($scope.gridOptions.data);
            }
        };

        $scope.onchangeyear = function () {
            $scope.topicdetailsnew = [];
            $scope.masteruinotdetails = [];
            $scope.LPMU_Id = "";
            $scope.LPMMTC_Id = "";
            $scope.LPMTC_TopicName = "";
            $scope.LPMTC_LessonPlan = "";
            $scope.LPMTC_TotalHrs = "";
            $scope.LPMTC_TotalPeriods = "";
            $scope.LPMTC_TeacherGuide = "";
            $scope.LPMTC_StudentGuide = "";
            $scope.LPMTC_MaterialNeeded = "";
            $scope.LPMTC_Homework = "";
            $scope.LPMTC_References = "";
            $scope.subjectlist = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercourse = promise.getcourse;
                }
            });
        };

        $scope.onchangecourse = function () {
            $scope.topicdetailsnew = [];
            $scope.masteruinotdetails = [];
            $scope.LPMU_Id = "";
            $scope.LPMMTC_Id = "";
            $scope.LPMTC_TopicName = "";
            $scope.LPMTC_LessonPlan = "";
            $scope.LPMTC_TotalHrs = "";
            $scope.LPMTC_TotalPeriods = "";
            $scope.LPMTC_TeacherGuide = "";
            $scope.LPMTC_StudentGuide = "";
            $scope.LPMTC_MaterialNeeded = "";
            $scope.LPMTC_Homework = "";
            $scope.LPMTC_References = "";
            $scope.subjectlist = [];
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangecourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterbranch = promise.getbranch;
                }
            });
        };

        $scope.onchangebranch = function () {
            $scope.topicdetailsnew = [];
            $scope.masteruinotdetails = [];
            $scope.LPMU_Id = "";
            $scope.LPMMTC_Id = "";
            $scope.LPMTC_TopicName = "";
            $scope.LPMTC_LessonPlan = "";
            $scope.LPMTC_TotalHrs = "";
            $scope.LPMTC_TotalPeriods = "";
            $scope.LPMTC_TeacherGuide = "";
            $scope.LPMTC_StudentGuide = "";
            $scope.LPMTC_MaterialNeeded = "";
            $scope.LPMTC_Homework = "";
            $scope.LPMTC_References = "";
            $scope.subjectlist = [];
            $scope.AMSE_Id = "";
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangebranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersemester = promise.getsemester;
                }
            });
        };

        $scope.onchangesemester = function () {
            $scope.topicdetailsnew = [];
            $scope.masteruinotdetails = [];
            $scope.LPMU_Id = "";
            $scope.LPMMTC_Id = "";
            $scope.LPMTC_TopicName = "";
            $scope.LPMTC_LessonPlan = "";
            $scope.LPMTC_TotalHrs = "";
            $scope.LPMTC_TotalPeriods = "";
            $scope.LPMTC_TeacherGuide = "";
            $scope.LPMTC_StudentGuide = "";
            $scope.LPMTC_MaterialNeeded = "";
            $scope.LPMTC_Homework = "";
            $scope.LPMTC_References = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangesemester", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlist = promise.getsubjectlist;
                    angular.forEach($scope.subjectlist, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + ' : ' + dd.ismS_SubjectCode;
                    });
                }
            });
        };

        $scope.onchangesubject = function () {
            $scope.topicdetailsnew = [];
            $scope.masteruinotdetails = [];
            $scope.LPMU_Id = "";
            $scope.LPMMT_Id = "";
            $scope.LPMT_TopicName = "";
            $scope.LPMT_LessonPlan = "";
            $scope.LPMT_TotalHrs = "";
            $scope.LPMT_TotalPeriods = "";
            $scope.LPMT_TeacherGuide = "";
            $scope.LPMT_StudentGuide = "";
            $scope.LPMT_MaterialNeeded = "";
            $scope.LPMT_Homework = "";
            $scope.LPMT_References = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangecollegesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.masterinotdetailsnew = promise.unitdetails;
                    if ($scope.masterinotdetailsnew.length > 0) {
                        $scope.masteruinotdetails = $scope.masterinotdetailsnew;

                    } else {
                        swal("No Unit Mapped To This Subjects");
                    }
                } else {
                    swal("Something Went Wrong , Kindly Contact Administrator");
                }
            });
        };

        $scope.onchangeunit = function () {
            $scope.topicdetailsnew = [];
            $scope.LPMMT_Id = "";
            $scope.LPMT_TopicName = "";
            $scope.LPMT_LessonPlan = "";
            $scope.LPMT_TotalHrs = "";
            $scope.LPMT_TotalPeriods = "";
            $scope.LPMT_TeacherGuide = "";
            $scope.LPMT_StudentGuide = "";
            $scope.LPMT_MaterialNeeded = "";
            $scope.LPMT_Homework = "";
            $scope.LPMT_References = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "LPMU_Id": $scope.LPMU_Id
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangecollegeunit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.topicdetails = promise.topicdetails;
                    if ($scope.topicdetails !== null && $scope.topicdetails, length > 0) {
                        $scope.topicdetailsnew = $scope.topicdetails;
                    } else {
                        swal("No Topic Found");
                    }
                }
            });
        };

        $scope.isOptionsRequired = function () {
            return !$scope.topicdetails.some(function (options) {
                return options.Selected;
            });
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.arraydetails = [];
                var LPMTC_TeacherGuide = "";
                if ($scope.LPMTC_TeacherGuide !== undefined && $scope.LPMTC_TeacherGuide !== null) {
                    LPMTC_TeacherGuide = $scope.LPMTC_TeacherGuide;
                } else {
                    LPMTC_TeacherGuide = "";
                }
                var LPMTC_MaterialNeeded = "";
                if ($scope.LPMTC_MaterialNeeded !== undefined && $scope.LPMTC_MaterialNeeded !== null) {
                    LPMTC_MaterialNeeded = $scope.LPMTC_MaterialNeeded;
                } else {
                    LPMTC_MaterialNeeded = "";
                }
                var LPMTC_References = "";
                if ($scope.LPMTC_References !== undefined && $scope.LPMTC_References !== null) {
                    LPMTC_References = $scope.LPMTC_References;
                } else {
                    LPMTC_References = "";
                }
                var LPMTC_Homework = "";
                if ($scope.LPMTC_Homework !== undefined && $scope.LPMTC_Homework !== null) {
                    LPMTC_Homework = $scope.LPMTC_Homework;
                } else {
                    LPMTC_Homework = "";
                }
                var LPMTC_StudentGuide = "";
                if ($scope.LPMTC_StudentGuide !== undefined && $scope.LPMTC_StudentGuide !== null) {
                    LPMTC_StudentGuide = $scope.LPMTC_StudentGuide;
                } else {
                    LPMTC_StudentGuide = "";
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                    "LPMMTC_Id": $scope.LPMMTC_Id,
                    "LPMU_Id": $scope.LPMU_Id,
                    "LPMTC_TopicName": $scope.LPMTC_TopicName,
                    "LPMTC_LessonPlan": $scope.LPMTC_LessonPlan,
                    "LPMTC_TotalHrs": $scope.LPMTC_TotalHrs,
                    "LPMTC_TotalPeriods": $scope.LPMTC_TotalPeriods,
                    "LPMTC_TeacherGuide": LPMTC_TeacherGuide,
                    "LPMTC_StudentGuide": LPMTC_StudentGuide,
                    "LPMTC_MaterialNeeded": LPMTC_MaterialNeeded,
                    "LPMTC_References": LPMTC_References,
                    "LPMTC_Homework": LPMTC_Homework,
                    "LPMTC_Id": $scope.LPMTC_Id,
                    "ReferenceGuideUploadDTO": $scope.referencedocuupload,
                    "MateralGuideUploadDTO": $scope.materaldocuupload,
                    "StudnetGuideUploadDTO": $scope.studentdocuupload,
                    "TeacherGuideUploadDTO": $scope.teacherdocuupload
                };

                apiService.create("SchoolSubjectWithMasterTopicMapping/savecollegedetails", data).then(function (promise) {
                    if (promise.message === "Add") {
                        if (promise.returnval === true) {
                            swal('Record saved successfully');
                        } else {
                            swal('Failed To save Record');
                        }
                    } else if (promise.message === "Update") {
                        if (promise.returnval === true) {
                            swal('Record updated successfully');
                        } else {
                            swal('Failed To Update Record');
                        }
                    }
                    else if (promise.message === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        swal("Failed To Save / Update Record");
                    }
                    $scope.cancel();
                });
            } else {
                $scope.submitted = true;
            }

        };

        // to Edit Data
        $scope.getorgvalue = function (EditRecord) {
            var data = EditRecord;
            apiService.create("SchoolSubjectWithMasterTopicMapping/editcollegedeatils", data).then(function (promise) {
                $scope.editdata = true;
                $scope.topicdetailsnew = promise.topicdetails;
                $scope.masteruinotdetails = promise.unitdetails;
                $scope.mastercourse = promise.getcourse;
                $scope.masterbranch = promise.getbranch;
                $scope.mastersemester = promise.getsemester;
                $scope.ISMS_Id = promise.geteditdetials[0];
                $scope.LPMMTC_Id = promise.geteditdetials[0].lpmmtC_Id;
                $scope.LPMTC_TopicName = promise.geteditdetials[0].lpmtC_TopicName;
                $scope.LPMTC_LessonPlan = promise.geteditdetials[0].lpmtC_LessonPlan;
                $scope.LPMTC_TotalHrs = promise.geteditdetials[0].lpmtC_TotalHrs;
                $scope.LPMTC_TotalPeriods = promise.geteditdetials[0].lpmtC_TotalPeriods;
                $scope.LPMTC_TeacherGuide = promise.geteditdetials[0].lpmtC_TeacherGuide;
                $scope.LPMTC_StudentGuide = promise.geteditdetials[0].lpmtC_StudentGuide;
                $scope.LPMTC_MaterialNeeded = promise.geteditdetials[0].lpmtC_MaterialNeeded;
                $scope.LPMTC_References = promise.geteditdetials[0].lpmtC_References;
                $scope.LPMTC_Homework = promise.geteditdetials[0].lpmtC_Homework;
                $scope.LPMTC_Id = promise.geteditdetials[0].lpmtC_Id;
                $scope.LPMU_Id = promise.geteditdetials[0].lpmU_Id;
                $scope.ASMAY_Id = promise.geteditdetials[0].asmaY_Id;
                $scope.AMCO_Id = promise.geteditdetials[0].amcO_Id;
                $scope.AMB_Id = promise.geteditdetials[0].amB_Id;
                $scope.AMSE_Id = promise.geteditdetials[0].amsE_Id;
            });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmtC_Activefalg === true) {
                mgs = "Deactivate";
                confirmmgs = "De-activated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do You Want To " + mgs + " Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("SchoolSubjectWithMasterTopicMapping/collegedeactivate", deactiveRecord).
                            then(function (promise) {
                                if (promise.already_cnt === true) {
                                    swal("You Can Not Deactivate This Record,It Has Dependency");
                                }
                                else {
                                    if (promise.returnval === true) {
                                        swal("Record " + confirmmgs + " " + "successfully");
                                    }
                                    else {
                                        swal("Record " + mgs + " Failed");
                                    }
                                }
                                $scope.cancel();
                                // $scope.BindData();

                            });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        // Order 

        $scope.onchangeyearnew = function () {
            $scope.AMB_IdNew = "";
            $scope.AMCO_IdNew = "";
            $scope.AMSE_IdNew = "";
            $scope.LPMMTC_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.ISMS_IdNew = "";
            $scope.subjectlistnew = [];
            $scope.mastersemesternew = [];
            $scope.mastercoursenew = [];
            $scope.masterbranchnew = [];
            $scope.masterunitnew = [];
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastercoursenew = promise.getcourse;
                }
            });
        };


        $scope.onchangecoursenew = function () {
            $scope.AMB_IdNew = "";
            $scope.AMSE_IdNew = "";
            $scope.LPMMTC_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.ISMS_IdNew = "";
            $scope.subjectlistnew = [];
            $scope.mastersemesternew = [];
            $scope.masterbranchnew = [];
            $scope.masterunitnew = [];
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangecourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterbranchnew = promise.getbranch;
                }
            });
        };

        $scope.onchangebranchnew = function () {
            $scope.AMSE_IdNew = "";
            $scope.LPMMTC_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.ISMS_IdNew = "";
            $scope.subjectlistnew = [];
            $scope.mastersemesternew = [];
            $scope.masterunitnew = [];
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "AMB_Id": $scope.AMB_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangebranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.mastersemesternew = promise.getsemester;
                }
            });
        };

        $scope.onchangesemesternew = function () {
            $scope.LPMMTC_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.ISMS_IdNew = "";
            $scope.subjectlistnew = [];
            $scope.masterunitnew = [];
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "AMB_Id": $scope.AMB_IdNew,
                "AMSE_Id": $scope.AMSE_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/collegeonchangesemester", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlistnew = promise.getsubjectlist;
                    angular.forEach($scope.subjectlistnew, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + ' : ' + dd.ismS_SubjectCode;
                    });
                }
            });
        };

        $scope.onchangesubjectnew = function () {
            $scope.LPMMTC_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];
            $scope.masterunitnew = [];
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "AMB_Id": $scope.AMB_IdNew,
                "AMSE_Id": $scope.AMSE_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangecollegesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.masterinotdetailsnew = promise.unitdetails;
                    if ($scope.masterinotdetailsnew.length > 0) {
                        $scope.masterunitnew = $scope.masterinotdetailsnew;

                    } else {
                        swal("No Unit Mapped To This Subjects");
                    }
                } else {
                    swal("Something Went Wrong , Kindly Contact Administrator");
                }
            });
        };

        $scope.onchnageunitnew = function () {
            $scope.LPMMTC_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.grouptypeListOrder = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "AMB_Id": $scope.AMB_IdNew,
                "AMSE_Id": $scope.AMSE_IdNew,
                "LPMU_Id": $scope.LPMU_IdNew,
                "Flag": "1"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangecollegeunit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.topicdetails = promise.topicdetails;
                    if ($scope.topicdetails !== null && $scope.topicdetails, length > 0) {
                        $scope.topicdetailsnews = $scope.topicdetails;
                    } else {
                        swal("No Topic Found");
                    }
                }
            });
        };

        $scope.onselecttopic = function () {
            $scope.grouptypeListOrder = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "AMCO_Id": $scope.AMCO_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "AMB_Id": $scope.AMB_IdNew,
                "AMSE_Id": $scope.AMSE_IdNew,
                "LPMU_Id": $scope.LPMU_IdNew,
                "LPMMTC_Id": $scope.LPMMTC_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/oncollegeselecttopic", data).then(function (promise) {

                $scope.grouptypeListOrder = promise.gettopicdetailsorder;
                if ($scope.grouptypeListOrder !== null) {
                    if ($scope.grouptypeListOrder.length > 0) {
                        $scope.btn = true;
                    } else {
                        $scope.btn = false;
                        swal("No Records Found");
                    }
                } else {
                    $scope.btn = false;
                    swal("No Records Found");
                }

            });
        };


        //fix the order drag
        //ConfigA is an Items
        $scope.resetLists = function () {
            $scope.configA = {
                // Changed sorting within list
                onUpdate: function (evt) {
                    var itemEl = evt.item;  // dragged HTMLElement
                    // + indexes from onEnd
                }
            };
        };
        $scope.init = function () {
            $scope.resetLists();
        };
        $scope.init();

        $scope.getOrder = function (orderarray) {
            var data = {
                CollegeSubjectTopicMappingTemporderDTO: orderarray
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/validatecollegeordernumber", data).then(function (promise) {
                if (promise.returnval === true) {
                    swal("Order Updated Successfully");
                } else {
                    swal("Failed To Update order");
                }
                $scope.cancel();
                // $scope.BindData();
            });
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].lpmtC_TopicOrder = Number(index) + 1;

                }
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.viewteacherguides = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "LPMTC_Id": obj.lpmtC_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "AMCO_Id": obj.amcO_Id,
                "AMB_Id": obj.amB_Id,
                "AMSE_Id": obj.amsE_Id,
                "ASMAY_Id": obj.asmaY_Id,
                "LPMTRC_ResourceType": "Teacher Guide"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewcollegeuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtrC_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtrC_Resources;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewstudentguides = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "LPMTC_Id": obj.lpmtC_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "AMCO_Id": obj.amcO_Id,
                "AMB_Id": obj.amB_Id,
                "AMSE_Id": obj.amsE_Id,
                "ASMAY_Id": obj.asmaY_Id,
                "LPMTRC_ResourceType": "Student Guide"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewcollegeuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtrC_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtrC_Resources;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewmaterials = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "LPMTC_Id": obj.lpmtC_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "AMCO_Id": obj.amcO_Id,
                "AMB_Id": obj.amB_Id,
                "AMSE_Id": obj.amsE_Id,
                "ASMAY_Id": obj.asmaY_Id,
                "LPMTRC_ResourceType": "Materials Needed"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewcollegeuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtrC_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtrC_Resources;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.viewrefernce = function (obj) {
            $scope.uploaddocfiles = [];
            $scope.uploadfilesdetails = [];
            var data = {
                "LPMTC_Id": obj.lpmtC_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "AMCO_Id": obj.amcO_Id,
                "AMB_Id": obj.amB_Id,
                "AMSE_Id": obj.amsE_Id,
                "ASMAY_Id": obj.asmaY_Id,
                "LPMTRC_ResourceType": "References"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewcollegeuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtrC_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtrC_Resources;
                            }
                        });
                    } else {
                        $('#popup11').modal('hide');
                        swal("No Documents Found");
                    }
                }
            });
        };

        $scope.deleteuploadfile = function (obj) {

            var data = obj;

            swal({
                title: "Are You Sure",
                text: "Do You Want To Delete The Record ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SchoolSubjectWithMasterTopicMapping/deletecollegeuploadfile", data).then(function (promise) {
                            if (promise.already_cnt === true) {
                                swal("You Can Not Deactivate This Record,It Has Dependency");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record Deleted successfully");
                                }
                                else {
                                    swal("Record Deletion Failed");
                                }
                            }
                            $('#popup11').modal('hide');
                        });
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        };

        var imagedownload = "";
        var docname = "";
        var studentreg = "";

        $scope.downloaddirectimage = function (data, idd) {
            studentreg = idd;
            $scope.imagedownload = data;
            imagedownload = data;
            $http.get(imagedownload, { responseType: "arraybuffer" }).success(function (data) {
                var anchor = angular.element('<a/>');
                var blob = new Blob([data]);
                anchor.attr({
                    href: window.URL.createObjectURL(blob),
                    target: '_blank',
                    download: studentreg
                })[0].click();
            });
        };

        $scope.showGuardianPhoto = function (data) {
            imagedownload = data.lpmtrC_Resources;
            studentreg = data.ismS_SubjectName;
            docname = data.lpmtrC_FileName;

            $('#preview').attr('src', data.lpmtrC_Resources);
        };


        $scope.download = function () {
            $http.get(imagedownload, {
                responseType: "arraybuffer"
            })
                .success(function (data) {
                    var anchor = angular.element('<a/>');
                    var blob = new Blob([data]);
                    anchor.attr({
                        href: window.URL.createObjectURL(blob),
                        target: '_blank',
                        download: studentreg + '-' + docname + '.jpg'
                    })[0].click();
                });
        };


        $scope.showGuardianPhotonew = function (data) {
            $scope.view_videos = [];
            $scope.videoSources = [];
            $scope.preview1 = data.lpmtrC_Resources;
            $scope.videdfd = data.lpmtrC_Resources;
            $scope.movie = { src: data.lpmtrC_Resources };
            $scope.movie1 = { src: data.lpmtrC_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtrC_Resources });
            console.log($scope.view_videos);
        };


        $scope.showpdf = false;
        $scope.downloadview = function (pdfview) {
            $scope.pdfurl = pdfview;
            $scope.showpdf = true;
            $('#showpdf').modal('show');
        };

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };

        $scope.showmothersign = function (path) {
            $('#preview').attr('src', path);
            $('img').bind('contextmenu', function (e) {
                return false;
            });
            $('#myModalimg').modal('show');
        };


        $scope.pauseOrPlay = function (ele) {
            $('#popup15').modal({
                show: false
            }).on('hidden.bs.modal', function () {
                $(this).find('video')[0].pause();
            });
        };

        // Upload Functions For Teacher Guides


        $scope.teacherdocuupload = [{ id: 'Teacher1' }];

        $scope.addNewsiblingguard = function () {
            var newItemNo = $scope.teacherdocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.teacherdocuupload.push({ 'id': 'Teacher' + newItemNo });
            }
            console.log($scope.teacherdocuupload);
        };

        $scope.removeNewsiblingguard = function (index) {
            var newItemNo = $scope.teacherdocuupload.length - 1;
            $scope.teacherdocuupload.splice(index, 1);

            if ($scope.teacherdocuupload.length === 0) {
                //data
            }
        };


        // Upload Functions For Student Guides

        $scope.studentdocuupload = [{ id: 'Student1' }];

        $scope.addstudent = function () {
            var newItemNo = $scope.studentdocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.studentdocuupload.push({ 'id': 'Student' + newItemNo });
            }
        };

        $scope.removestudent = function (index) {
            var newItemNo = $scope.studentdocuupload.length - 1;
            $scope.studentdocuupload.splice(index, 1);

            if ($scope.studentdocuupload.length === 0) {
                //data
            }
        };

        // Upload Functions For Materal Guides

        $scope.materaldocuupload = [{ id: 'Materal1' }];

        $scope.addmateral = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };

        $scope.removemateral = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);

            if ($scope.materaldocuupload.length === 0) {
                //data
            }
        };



        // Upload Functions For Refernce Guides

        $scope.referencedocuupload = [{ id: 'Refernce1' }];

        $scope.addreference = function () {
            var newItemNo = $scope.referencedocuupload.length + 1;

            if (newItemNo <= 10) {
                $scope.referencedocuupload.push({ 'id': 'Refernce' + newItemNo });
            }
        };

        $scope.removerefernce = function (index) {
            var newItemNo = $scope.referencedocuupload.length - 1;
            $scope.referencedocuupload.splice(index, 1);

            if ($scope.referencedocuupload.length === 0) {
                //data
            }
        };


        // Save Function For Teacher Guide Upload


        $scope.uploadtecherdocuments1 = [];

        $scope.uploadtecherdocuments = function (input, document) {

            $scope.uploadtecherdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianPhoto(data) {
            console.log("Teacher Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadtecherdocuments1.length; i++) {
                formData.append("File", $scope.uploadtecherdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.lpmtrC_Resources = d;
                    data.lpmtrC_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtrC_Resources);
                    var img = data.lpmtrC_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtrC_Resources;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }


        // Save Function For Student Guide Upload

        $scope.uploadstudentdocuments1 = [];

        $scope.uploadstudentdocuments = function (input, document) {

            $scope.uploadstudentdocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Doc, Image Files Only");
                }
            }
        };

        function UploaddianstudentPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadstudentdocuments1.length; i++) {
                formData.append("File", $scope.uploadstudentdocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.lpmtrC_Resources = d;
                    data.lpmtrC_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtrC_Resources);
                    var img = data.lpmtrC_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtrC_Resources;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        // Save Function For Materal Guide Upload

        $scope.uploadmateraldocuments1 = [];

        $scope.uploadmateraldocuments = function (input, document) {

            $scope.uploadmateraldocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddianmateralPhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddianmateralPhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadmateraldocuments1.length; i++) {
                formData.append("File", $scope.uploadmateraldocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.lpmtrC_Resources = d;
                    data.lpmtrC_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtrC_Resources);
                    var img = data.lpmtrC_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtrC_Resources;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }

        $scope.trustSrc = function (src) {
            return $sce.trustAsResourceUrl(src);
        };


        // Save Function For Reference Guide Upload

        $scope.uploadreferncedocuments1 = [];

        $scope.uploadreferncedocuments = function (input, document) {

            $scope.uploadreferncedocuments1 = input.files;

            $scope.filename = input.files[0].name;

            if (input.files && input.files[0]) {

                if (input.files[0].type === "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].type === "video/mp4") {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].type === "application/pdf") {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].type === "application/msword") {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-excel") {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document,") {
                    UploaddiareferncePhoto(document);
                }
                else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                } else {
                    swal("Upload MP4, Pdf, Image Files Only");
                }
            }
        };

        function UploaddiareferncePhoto(data) {
            console.log("Student Upload  :" + data);
            var formData = new FormData();
            for (var i = 0; i <= $scope.uploadreferncedocuments1.length; i++) {
                formData.append("File", $scope.uploadreferncedocuments1[i]);
            }
            var defer = $q.defer();
            $http.post("/api/ImageUpload/lessonplannerdoc", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    data.lpmtrC_Resources = d;
                    data.lpmtrC_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtrC_Resources);
                    var img = data.lpmtrC_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtrC_Resources;
                    }
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }
    }


    angular.module('app').filter("trustUrl", function ($sce) {
        return function (Url) { return $sce.trustAsResourceUrl(Url); };
    });



    angular.module('app').directive('txtArea', function () {
        return {
            restrict: 'AE',
            replace: 'true',
            scope: { data: '=', model: '=ngModel' },
            template: "<textarea></textarea>",
            link: function (scope, elem) {
                scope.$watch('data', function (newVal) {
                    if (newVal) {
                        scope.model += newVal[0];
                    }
                });
            }
        };
    });

})();