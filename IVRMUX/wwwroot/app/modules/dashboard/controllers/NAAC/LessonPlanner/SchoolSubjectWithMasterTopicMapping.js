﻿(function () {
    'use strict';
    angular.module('app').controller('SchoolSubjecWithtMasterTopicMappingController', SchoolSubjecWithtMasterTopicMappingController)
    SchoolSubjecWithtMasterTopicMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$sce', '$window']
    function SchoolSubjecWithtMasterTopicMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $sce, $window) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.showbtn = false;
        $scope.teacherdocuupload = {};
        $scope.btn = false;
        //TO  GEt The Values iN Grid

        $scope.onview = function (filepath, filename) {
            //var myPdfUrl = 'https://bdcampusstrg.blob.core.windows.net/files/6/LessonPlanner/1e55cab1-2e21-4878-802d-ad87b8507e6b.pdf';
            var imagedownload = filepath;
            $scope.content = "";
            var fileURL = "";
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

            $scope.ASMCL_IdNew = "";
            $scope.ISMS_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.LPMMT_IdNew = "";
            $scope.ASMAY_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.masterclassnew = [];
            $scope.subjectlistnew = [];
            $scope.masterunitnew = [];
            $scope.grouptypeListOrder = [];

            $scope.masterinotdetailsnews = [];
            $scope.masterinotdetailsnew = [];
            $scope.grouptypeListOrder = [];
            $scope.topicdetailsnew = [];
            $scope.topicdetailsdsdd = [];

            apiService.getURI("SchoolSubjectWithMasterTopicMapping/Getdetails", id).then(function (promise) {
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
                { name: 'asmcL_ClassName', displayName: 'Class', width: 200 },
                { name: 'ismS_SubjectName', displayName: 'Subject Name', width: 200 },
                { name: 'lpmU_UnitName', displayName: 'Unit', width: 200 },
                { name: 'lpmmT_TopicName', displayName: 'Main Topic', width: 100 },
                { name: 'lpmT_TopicName', displayName: 'Topic Name', width: 100 },
                { name: 'lpmT_LessonPlan', displayName: 'Description', width: 100 },
                { name: 'lpmT_TotalHrs', displayName: 'Total Hours', width: 100 },
                { name: 'lpmT_TotalPeriods', displayName: 'Total Periods', width: 100 },
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
                        '<a ng-if="row.entity.lpmT_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.lpmT_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
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
            $scope.subjectlist = [];
            $scope.ASMCL_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterclass = promise.getclass;
                }
            });
        };

        $scope.onchangeclass = function () {
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
            $scope.subjectlist = [];

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlist = promise.getsubjectlist;
                    angular.forEach($scope.subjectlist, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
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
                "ASMCL_Id": $scope.ASMCL_Id,
                "ISMS_Id": $scope.ISMS_Id.ismS_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.masterinotdetailsnew = promise.unitdetails;
                    //$scope.grouptypeListOrder = promise.getorderdetails;
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
                "ASMCL_Id": $scope.ASMCL_Id,
                "LPMU_Id": $scope.LPMU_Id,
                "ISMS_Id": $scope.ISMS_Id.ismS_Id
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangeunit", data).then(function (promise) {
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
                var LPMT_TeacherGuide = "";
                if ($scope.LPMT_TeacherGuide !== undefined && $scope.LPMT_TeacherGuide !== null) {
                    LPMT_TeacherGuide = $scope.LPMT_TeacherGuide;
                } else {
                    LPMT_TeacherGuide = "";
                }
                var LPMT_MaterialNeeded = "";
                if ($scope.LPMT_MaterialNeeded !== undefined && $scope.LPMT_MaterialNeeded !== null) {
                    LPMT_MaterialNeeded = $scope.LPMT_MaterialNeeded;
                } else {
                    LPMT_MaterialNeeded = "";
                }
                var LPMT_References = "";
                if ($scope.LPMT_References !== undefined && $scope.LPMT_References !== null) {
                    LPMT_References = $scope.LPMT_References;
                } else {
                    LPMT_References = "";
                }
                var LPMT_Homework = "";
                if ($scope.LPMT_Homework !== undefined && $scope.LPMT_Homework !== null) {
                    LPMT_Homework = $scope.LPMT_Homework;
                } else {
                    LPMT_Homework = "";
                }
                var LPMT_StudentGuide = "";
                if ($scope.LPMT_StudentGuide !== undefined && $scope.LPMT_StudentGuide !== null) {
                    LPMT_StudentGuide = $scope.LPMT_StudentGuide;
                } else {
                    LPMT_StudentGuide = "";
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ISMS_Id": $scope.ISMS_Id.ismS_Id,
                    "LPMMT_Id": $scope.LPMMT_Id,
                    "LPMU_Id": $scope.LPMU_Id,
                    "LPMT_TopicName": $scope.LPMT_TopicName,
                    "LPMT_LessonPlan": $scope.LPMT_LessonPlan,
                    "LPMT_TotalHrs": $scope.LPMT_TotalHrs === undefined || $scope.LPMT_TotalHrs === null || $scope.LPMT_TotalHrs === "" ? null : $scope.LPMT_TotalHrs,
                    "LPMT_TotalPeriods": $scope.LPMT_TotalPeriods === undefined || $scope.LPMT_TotalPeriods === null || $scope.LPMT_TotalPeriods === "" ? null : $scope.LPMT_TotalPeriods,
                    "LPMT_TeacherGuide": LPMT_TeacherGuide,
                    "LPMT_StudentGuide": LPMT_StudentGuide,
                    "LPMT_MaterialNeeded": LPMT_MaterialNeeded,
                    "LPMT_References": LPMT_References,
                    "LPMT_Homework": LPMT_Homework,
                    "LPMT_Id": $scope.LPMT_Id,
                    "ReferenceGuideUploadDTO": $scope.referencedocuupload,
                    "MateralGuideUploadDTO": $scope.materaldocuupload,
                    "StudnetGuideUploadDTO": $scope.studentdocuupload,
                    "TeacherGuideUploadDTO": $scope.teacherdocuupload
                };

                apiService.create("SchoolSubjectWithMasterTopicMapping/savedetails", data).then(function (promise) {
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
            apiService.create("SchoolSubjectWithMasterTopicMapping/editdeatils", data).then(function (promise) {
                $scope.editdata = true;
                $scope.topicdetailsnew = promise.topicdetails;
                $scope.masteruinotdetails = promise.unitdetails;
                $scope.masterclass = promise.getclass;
                $scope.ISMS_Id = promise.geteditdetials[0];
                $scope.LPMMT_Id = promise.geteditdetials[0].lpmmT_Id;
                $scope.LPMT_TopicName = promise.geteditdetials[0].lpmT_TopicName;
                $scope.LPMT_LessonPlan = promise.geteditdetials[0].lpmT_LessonPlan;
                $scope.LPMT_TotalHrs = promise.geteditdetials[0].lpmT_TotalHrs;
                $scope.LPMT_TotalPeriods = promise.geteditdetials[0].lpmT_TotalPeriods;
                $scope.LPMT_TeacherGuide = promise.geteditdetials[0].lpmT_TeacherGuide;
                $scope.LPMT_StudentGuide = promise.geteditdetials[0].lpmT_StudentGuide;
                $scope.LPMT_MaterialNeeded = promise.geteditdetials[0].lpmT_MaterialNeeded;
                $scope.LPMT_References = promise.geteditdetials[0].lpmT_References;
                $scope.LPMT_Homework = promise.geteditdetials[0].lpmT_Homework;
                $scope.LPMT_Id = promise.geteditdetials[0].lpmT_Id;
                $scope.LPMU_Id = promise.geteditdetials[0].lpmU_Id;
                $scope.ASMAY_Id = promise.geteditdetials[0].asmaY_Id;
                $scope.ASMCL_Id = promise.geteditdetials[0].asmcL_Id;
            });
        };

        //to active or deactive 
        $scope.deactive = function (deactiveRecord) {
            var mgs = "";
            var confirmmgs = "";
            if (deactiveRecord.lpmT_ActiveFlag === true) {
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

                        apiService.create("SchoolSubjectWithMasterTopicMapping/deactivate", deactiveRecord).
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
            $scope.ASMCL_IdNew = "";
            $scope.ISMS_IdNew = "";
            $scope.LPMU_IdNew = "";
            $scope.LPMMT_IdNew = "";
            $scope.topicdetailsnews = [];
            $scope.masterclassnew = [];
            $scope.subjectlistnew = [];
            $scope.masterunitnew = [];
            $scope.grouptypeListOrder = [];


            $scope.grouptypeListOrder = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.masterclassnew = promise.getclass;
                }
            });
        };

        $scope.onchangeclassnew = function () {
            $scope.subjectlist = [];
            $scope.topicdetailsnews = [];
            $scope.LPMMT_IdNew = "";
            $scope.grouptypeListOrder = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "ASMCL_Id": $scope.ASMCL_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlistnew = promise.getsubjectlist;
                    angular.forEach($scope.subjectlistnew, function (dd) {
                        dd.ismS_SubjectName = dd.ismS_SubjectName + " : " + dd.ismS_SubjectCode;
                    });
                }
            });
        };

        $scope.onchangesubjectnew = function () {
            $scope.topicdetailsnews = [];
            $scope.LPMMT_IdNew = "";
            $scope.grouptypeListOrder = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "ASMCL_Id": $scope.ASMCL_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangesubject", data).then(function (promise) {
                if (promise !== null) {
                    $scope.showbtn = true;
                    $scope.masterinotdetailsnews = promise.unitdetails;
                    if ($scope.masterinotdetailsnews.length > 0) {
                        $scope.masterunitnew = $scope.masterinotdetailsnews;
                    } else {
                        swal("No Unit Mapped To This Subjects");
                    }
                } else {
                    swal("Something Went Wrong , Kindly Contact Administrator");
                }
            });
        };

        $scope.onchnageunit = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_IdNew,
                "ASMCL_Id": $scope.ASMCL_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "LPMU_Id": $scope.LPMU_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onchangeunit", data).then(function (promise) {
                if (promise !== null) {
                    $scope.topicdetailsdsdd = promise.topicdetails;
                    if ($scope.topicdetailsdsdd !== null && $scope.topicdetailsdsdd, length > 0) {
                        $scope.topicdetailsnews = $scope.topicdetailsdsdd;
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
                "ASMCL_Id": $scope.ASMCL_IdNew,
                "ISMS_Id": $scope.ISMS_IdNew.ismS_Id,
                "LPMMT_Id": $scope.LPMMT_IdNew,
                "Flag": "1"
            };
            apiService.create("SchoolSubjectWithMasterTopicMapping/onselecttopic", data).then(function (promise) {

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
                SchoolSubjectWithMasterTopicMappingTemporderDTO: orderarray
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/validateordernumber", data).then(function (promise) {
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
                    $scope.grouptypeListOrder[index].lpmT_TopicOrder = Number(index) + 1;

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
                "LPMT_Id": obj.lpmT_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "LPMTR_ResourceType": "Teacher Guide"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_Resources;
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
                "LPMT_Id": obj.lpmT_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "LPMTR_ResourceType": "Student Guide"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_Resources;
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
                "LPMT_Id": obj.lpmT_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "LPMTR_ResourceType": "Materials Needed"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_Resources;
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
                "LPMT_Id": obj.lpmT_Id,
                "ISMS_Id": obj.ismS_Id,
                "LPMU_Id": obj.lpmU_Id,
                "LPMTR_ResourceType": "References"
            };

            apiService.create("SchoolSubjectWithMasterTopicMapping/viewuploadflies", data).then(function (promise) {
                if (promise !== null) {
                    $scope.uploadfilesdetails = promise.uploadfiles;
                    if ($scope.uploadfilesdetails !== null && $scope.uploadfilesdetails.length > 0) {
                        $scope.uploaddocfiles = promise.uploadfiles;

                        angular.forEach($scope.uploaddocfiles, function (dd) {
                            var img = dd.lpmtR_Resources;
                            var imagarr = img.split('.');
                            var lastelement = imagarr[imagarr.length - 1];
                            dd.filetype = lastelement;
                            if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                                dd.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + dd.lpmtR_Resources;
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
                        apiService.create("SchoolSubjectWithMasterTopicMapping/deleteuploadfile", data).then(function (promise) {
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
            imagedownload = data.lpmtR_Resources;
            studentreg = data.ismS_SubjectName;
            docname = data.lpmtR_FileName;

            $('#preview').attr('src', data.lpmtR_Resources);
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
            $scope.preview1 = data.lpmtR_Resources;
            $scope.videdfd = data.lpmtR_Resources;
            $scope.movie = { src: data.lpmtR_Resources };
            $scope.movie1 = { src: data.lpmtR_Resources };
            $scope.view_videos.push({ id: 1, coeeV_Videos: data.lpmtR_Resources });
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
                //$scope.size = input.files[0].size;
                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
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
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianPhoto(document);
                }
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //} 
                else {
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
                    headers: {
                        'Content-Type': undefined                       
                    },
                    transformRequest: angular.identity,

                })
                .success(function (d) {
                    defer.resolve(d);
                    data.lpmtR_Resources = d;
                    data.lpmtR_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    console.log("data.filetype : " + data.filetype);
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
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

                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
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
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.wordprocessingml.document") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.openxmlformats-officedocument.presentationml.presentation") {
                    UploaddianstudentPhoto(document);
                }
                else if (input.files[0].type === "application/vnd.ms-powerpoint") {
                    UploaddianstudentPhoto(document);
                }
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //}
                else {
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
                    data.lpmtR_Resources = d;
                    data.lpmtR_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
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

                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
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
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //}
                else {
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
                    data.lpmtR_Resources = d;
                    data.lpmtR_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
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

                if (input.files[0].type === "image/jpeg")  // 2097152 bytes = 2MB 
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
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //}
                else {
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
                    data.lpmtR_Resources = d;
                    data.lpmtR_FileName = $scope.filename;
                    $('#').attr('src', data.lpmtR_Resources);
                    var img = data.lpmtR_Resources;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];
                    data.filetype = lastelement;
                    if (lastelement === 'doc' || lastelement === 'docx' || lastelement === 'ppt' || lastelement === 'pptx' || lastelement === 'xlsx' || lastelement === 'xls') {
                        data.document_Pathnew = "https://view.officeapps.live.com/op/view.aspx?src=" + data.lpmtR_Resources;
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