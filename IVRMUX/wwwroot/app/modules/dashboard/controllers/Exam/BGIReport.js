

(function () {
    'use strict';
    angular
.module('app')
.controller('BGIReportController', BGIReportController)

    BGIReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function BGIReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.report = false;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("BaldwinAllReport/Getdetails").
       then(function (promise) {
           
           $scope.yearlt = promise.yearlist;
           $scope.clslist = promise.classlist;
           $scope.seclist = promise.seclist;
           $scope.amstlt = promise.amstlist;
           $scope.exsplt = promise.exmstdlist;
           //  $scope.gridOptions.data = promise.studmaplist;
       })
        };



        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {

            angular.forEach($scope.yearlt, function (yr) {
                if (yr.asmaY_Id == $scope.asmaY_Id) {
                    $scope.year = yr.asmaY_Year;
                }
            })


            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BaldwinAllReport/savedetails", data).
                         then(function (promise) {
                             
                             if (promise.savelist.length > 0) {

                                 $scope.report = true;
                                 //inst name binding
                                 $scope.instname = promise.instname;
                                 $scope.inst_name = $scope.instname[0].mI_Name;

                                 var temp_list = [];
                                 for (var x = 0; x < promise.savelist.length; x++) {
                                     var stu_id = promise.savelist[x].amsT_Id;
                                     var stu_subj_list = [];
                                     angular.forEach(promise.savelist, function (opq) {
                                         if (opq.amsT_Id == stu_id) {
                                             stu_subj_list.push({
                                                 amsT_Id: opq.amsT_Id,
                                                 ismS_Id: opq.ismS_Id,
                                                 emgR_Id: opq.emgR_Id,
                                                 ismS_SubjectName: opq.ismS_SubjectName,
                                                 estmpS_MaxMarks: opq.estmpS_MaxMarks,
                                                 estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks,
                                                 estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade,
                                                 estmpS_PassFailFlg: opq.estmpS_PassFailFlg,
                                                 ESTMPS_ClassAverage: opq.estmpS_ClassAverage,
                                                 ESTMPS_SectionAverage: opq.estmpS_SectionAverage,
                                                 ESTMPS_ClassHighest: opq.estmpS_ClassHighest,
                                                 ESTMPS_SectionHighest: opq.estmpS_SectionHighest,
                                                 eyceS_MarksDisplayFlg: opq.eyceS_MarksDisplayFlg,
                                                 eyceS_GradeDisplayFlg: opq.eyceS_GradeDisplayFlg
                                             });
                                         }
                                     })
                                     if (temp_list.length == 0) {

                                         temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                     }
                                     else if (temp_list.length > 0) {
                                         var already_cnt = 0;
                                         angular.forEach(temp_list, function (opq1) {

                                             if (opq1.student_id == stu_id) {
                                                 already_cnt += 1;

                                             }
                                         })
                                         if (already_cnt == 0) {
                                             temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                         }
                                     }

                                 }
                                 $scope.exam = promise.savelist[0].emE_ExamName;
                                 $scope.exm_sublist = promise.subjlist;
                                 $scope.subj_grade_remarks = promise.grade_details;
                                 if (promise.clstchname.length > 0) {
                                     $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                                 } else {
                                     $scope.clastechname = "";
                                 }
                                // $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                                 $scope.report_list = temp_list;

                             } else {
                                 swal('No record Found');
                             }
                         })
            }
        };

        $scope.cancel = function () {
            $scope.asmcL_Id = ""
            $scope.emcA_Id = ""
            $scope.asmaY_Id = ""
            $scope.emG_Id = ""
            $scope.asmS_Id = ""
            $scope.subjectlt = ""
            $scope.subjectlt1 = ""
            $scope.studentlist = false;
            $state.reload();
        }

        $scope.toggleAll = function () {
            
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;

            });
        }

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; })
        }


        //to print
        $scope.BGIREPORT = function () {
            var innerContents = document.getElementById("BGIREPORT").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.get_totalmin = function (exm_subjs, stu_subjs) {


            $scope.stu_grandmin_marks = 0;
            angular.forEach(exm_subjs, function (itm) {
                if (itm.eyceS_AplResultFlg) {
                    angular.forEach(stu_subjs, function (itm1) {
                        if (itm1.ismS_Id == itm.ismS_Id) {
                            $scope.stu_grandmin_marks += itm.eyceS_MinMarks;
                        }
                    })
                }

            })
        };

        $scope.OnAcdyear = function () {

            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("CumulativeReport/onchangeyear", data).then(function (promise) {

                if (promise != null) {
                    $scope.clslist = promise.classlist;
                    if ($scope.clslist.length == 0) {
                        swal("No Class Is Mapped For Selected Academic Year");
                    }
                } else {
                    swal("No Class Is Mapped For Selected Academic Year");
                }

            });
        };

        $scope.onchangeclass = function () {

            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };

            apiService.create("CumulativeReport/onchangeclass", data).then(function (promise) {

                if (promise != null) {
                    $scope.seclist = promise.seclist;
                    if ($scope.seclist.length == 0) {
                        swal("No Section Is Mapped For Selected Class");
                    }

                } else {
                    swal("No Section Is Mapped For Selected Class");
                }

            });
        };

        $scope.onchangesection = function () {

            $scope.emE_Id = "";
            $scope.report = false;

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };

            apiService.create("CumulativeReport/onchangesection", data).then(function (promise) {

                if (promise != null) {
                    $scope.exsplt = promise.exmstdlist;
                    if ($scope.exsplt.length == 0) {
                        swal("No Exam Is Mapped For Selected Details ");
                    }
                } else {
                    swal("No Exam Is Mapped For Selected Details");
                }

            });
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
        };


    }

})();