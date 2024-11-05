
(function () {
    'use strict';
    angular
        .module('app')
        .controller('EmployeeStudentReportCardController', EmployeeStudentReportCardController)

    EmployeeStudentReportCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function EmployeeStudentReportCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("EmployeeStudentReportCard/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    //$scope.clslist = promise.classlist;
                    //$scope.seclist = promise.SectionList;
                    //$scope.amstlt = promise.amstlist;
                    //$scope.exsplt = promise.exmstdlist;
                    //$scope.studentDropdown = promise.studentList;
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    //  $scope.amsT_Id = "";
                    $scope.emE_Id = "";
                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";
                    $scope.studentDropdown = "";
                    $scope.exsplt = "";

                })
        };

        $scope.get_class = function () {

            // $scope.sectionDropdown = "";
            // $scope.studentDropdown = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            // $scope.amsT_Id = "";
            $scope.emE_Id = "";
            // $scope.sectionDropdown = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("EmployeeStudentReportCard/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                })
        }
        $scope.get_section = function () {

            //$scope.amsT_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.studentDropdown = "";
            $scope.exsplt = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("EmployeeStudentReportCard/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_student = function (asmS_Id) {


            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("EmployeeStudentReportCard/get_student", data)
                .then(function (promise) {
                    $scope.studentDropdown = promise.studentList;


                })

        }

        $scope.get_exam1 = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMST_Id": $scope.amst_Id
            }
            apiService.create("EmployeeStudentReportCard/get_exam", data)
                .then(function (promise) {
                    $scope.exsplt = promise.exmstdlist;

                })

        }

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

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "AMST_Id": $scope.amst_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("EmployeeStudentReportCard/savedetails", data).
                    then(function (promise) {


                        if (promise.savelist.length > 0) {
                            var temp_list = [];
                            for (var x = 0; x < promise.savelist.length; x++) {
                                var stu_id = promise.savelist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savelist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg });
                                    }
                                })
                                if (temp_list.length == 0) {


                                    temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                }
                                else if (temp_list.length > 0) {

                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {

                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;

                                        }
                                    })
                                    if (already_cnt == 0) {

                                        temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                    }
                                }

                            }
                            $scope.savelist = promise.savelist;

                            $scope.exam = promise.savelist[0].emE_ExamName;
                            $scope.exm_sublist = promise.subjlist;
                            $scope.processtot = promise.savelisttot;
                            $scope.subj_grade_remarks = promise.grade_details;
                            $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                            $scope.report_list = temp_list;

                            $scope.temparray = [];
                            angular.forEach($scope.savelist, function (t1) {

                                angular.forEach($scope.exm_sublist, function (t2) {
                                    if (t1.ismS_Id == t2.ismS_Id) {

                                        $scope.temparray.push({ ismS_Id: t1.ismS_Id, ismS_SubjectName: t1.ismS_SubjectName, estmpS_MaxMarks: t2.eyceS_MaxMarks, estmpS_MinMarks: t2.eyceS_MinMarks, estmpS_ObtainedMarks: t1.estmpS_ObtainedMarks, estmpS_ObtainedGrade: t1.estmpS_ObtainedGrade, estmpS_PassFailFlg: t1.estmpS_PassFailFlg });
                                    }

                                })
                            })
                            console.log($scope.temparray)

                            //var exmlist = [];
                            //if ($scope.exm_sublist != null && $scope.savelist != null) {
                            //    for (var i = 0; i < $scope.exm_sublist.length; i++) {
                            //        for (var j = 0; j < $scope.savelist.length; j++) {
                            //            if ($scope.exm_sublist[i].ismS_SubjectName == $scope.savelist[j].ismS_SubjectName) {
                            //                exmlist.push({ label: $scope.exm_sublist[i].ismS_SubjectName, "y": parseInt($scope.savelist[j].estmpS_ObtainedMarks) })
                            //            }
                            //            else {
                            //                exmlist.push({ label: $scope.exm_sublist[i].ismS_SubjectName, "y": 0 })
                            //            }

                            //        }
                            //    }
                            //}

                            var exmlist = [];
                            var exmlist1 = [];
                            var exmlist2 = [];
                            if ($scope.temparray.length > 0) {
                                angular.forEach($scope.temparray, function (sss) {
                                    exmlist.push({ label: sss.ismS_SubjectName, "y": parseInt(sss.estmpS_MaxMarks) })
                                })
                                angular.forEach($scope.temparray, function (sss) {
                                    exmlist1.push({ label: sss.ismS_SubjectName, "y": parseInt(sss.estmpS_ObtainedMarks) })
                                })
                                angular.forEach($scope.temparray, function (sss) {
                                    exmlist2.push({ label: sss.ismS_SubjectName, "y": parseInt(sss.estmpS_MinMarks) })
                                })
                            }



                            //var chart = new CanvasJS.Chart("columnchart", {

                            //    axisX: {
                            //        labelFontSize: 12,
                            //    },
                            //    axisY: {
                            //        labelFontSize: 12,
                            //    },

                            //    data: [
                            //    {
                            //        type: "column",
                            //        showInLegend: true,
                            //        dataPoints: exmlist
                            //    }
                            //    ]
                            //});
                            //chart.render();
                            var chart = new CanvasJS.Chart("chartContainer", {
                                animationEnabled: true,
                                animationDuration: 3000,
                                height: 350,
                                width: 1083,
                                axisX: {
                                    labelFontSize: 8, interval: 1,
                                },
                                axisY: {

                                    labelFontSize: 12,
                                },
                                data: [{
                                    name: "Maximum Marks",
                                    type: "column",
                                    showInLegend: true,
                                    dataPoints: exmlist

                                },
                                {
                                    name: "Obtained Marks",
                                    type: "column",
                                    showInLegend: true,
                                    dataPoints: exmlist1

                                },
                                {
                                    name: "Minimum Marks",
                                    type: "column",
                                    showInLegend: true,
                                    dataPoints: exmlist2

                                }
                                ]
                            });
                            chart.render();

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
        }


    }

})();