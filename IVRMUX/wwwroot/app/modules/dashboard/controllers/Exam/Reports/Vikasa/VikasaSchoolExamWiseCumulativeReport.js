﻿
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VikasaSchoolExamWiseCumulativeReportController', VikasaSchoolExamWiseCumulativeReportController)

    VikasaSchoolExamWiseCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function VikasaSchoolExamWiseCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid
        $scope.displayprincipal = true;
        $scope.displayviceprincipal = true;
        $scope.BindData = function () {
            var pageid = 2;

            apiService.getURI("VikasaSchoolExamWiseCumulativeReport/Getdetails", pageid).
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;

                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";


                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";


                })
        };

        $scope.get_class = function () {

            // $scope.sectionDropdown = "";
            // $scope.studentDropdown = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.emgR_Id = "";
            // $scope.sectionDropdown = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("VikasaSchoolExamWiseCumulativeReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                })
        }
        $scope.get_section = function () {

            $scope.emE_Id = "";
            $scope.emgR_Id = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("VikasaSchoolExamWiseCumulativeReport/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_Exam = function () {

            $scope.emgR_Id = "";
            var data = {

                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id

            }
            apiService.create("VikasaSchoolExamWiseCumulativeReport/get_Exam", data)
                .then(function (promise) {
                    $scope.exsplt = promise.examList;


                })

        }

        $scope.get_Grade = function (asmS_Id) {


            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("VikasaSchoolExamWiseCumulativeReport/get_subject", data)
                .then(function (promise) {
                    $scope.gradeDropdown = promise.gradeList;


                })

        }


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        // TO Show The Data
        $scope.submitted = false;
        $scope.showdetails = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "EMGR_Id": $scope.emgR_Id,
                    "EME_Id": $scope.emE_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("VikasaSchoolExamWiseCumulativeReport/showdetails", data).
                    then(function (promise) {
                        if (promise.studentList.length > 0) {

                            if ($scope.checkoruncheck == true) {
                                $scope.checkbox1 = true;
                                $scope.checkbox = false;
                            } else {
                                $scope.checkbox1 = false;
                                $scope.checkbox = true;
                            }

                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.Datalist = promise.studentList;
                            $scope.asmcL_ClassName = promise.studentList[0].asmcL_ClassName;
                            $scope.asmC_SectionName = promise.studentList[0].asmC_SectionName;
                            $scope.gradel = promise.gradelist;

                            //for group head
                            if (promise.examGroupname.length > 0) {
                                $scope.GroupHead = promise.examGroupname;

                                //for group head marks

                                if (promise.examgroupmarks.length > 0) {

                                    $scope.Dataexamgroup = promise.examgroupmarks;

                                    $scope.temparrayData = [];
                                    angular.forEach($scope.Datalist, function (t1) {

                                        angular.forEach($scope.GroupHead, function (t2) {
                                            angular.forEach($scope.Dataexamgroup, function (t3) {
                                                if (t3.amsT_Id == t1.amsT_Id && t3.ismS_Id == t2.ismS_Id) {

                                                    $scope.temparrayData.push({ estmpS_ObtainedMarks: t3.estmpS_ObtainedMarks, estmpS_ObtainedGrade: t3.estmpS_ObtainedGrade, estmpS_MaxMarks: t3.estmpS_MaxMarks, estmpS_SectionAverage: t3.estmpS_SectionAverage, estmpS_PassFailFlg: t3.estmpS_PassFailFlg, group_per_marks: (t3.estmpS_ObtainedMarks * t2.empsG_PercentValue) / 100, amsT_Id: t1.amsT_Id, ismS_Id: t2.ismS_Id });

                                                }
                                            })


                                        })
                                    })

                                    //$scope.temp_Totalp = [];
                                    //angular.forEach($scope.temparrayData, function (Totalp) {

                                    //})

                                    $scope.temp_GroupD = [];
                                    angular.forEach($scope.temparrayData, function (grpD) {
                                        $scope.temp_GroupD.push({ grpD_name: grpD.estmpS_ObtainedMarks, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, ismS_Id: grpD.ismS_Id, flag: 0 });
                                        $scope.temp_GroupD.push({ grpD_name: grpD.estmpS_MaxMarks, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, ismS_Id: grpD.ismS_Id, flag: 1 });
                                        $scope.temp_GroupD.push({ grpD_name: grpD.estmpS_SectionAverage, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, ismS_Id: grpD.ismS_Id, flag: 1 });
                                        $scope.temp_GroupD.push({ grpD_name: grpD.estmpS_ObtainedGrade, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, ismS_Id: grpD.ismS_Id, flag: 1 });
                                        $scope.temp_GroupD.push({ grpD_name: grpD.estmpS_PassFailFlg, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, ismS_Id: grpD.ismS_Id, flag: 1 });
                                    })
                                }
                                console.log($scope.temp_GroupD);
                            }
                            if (promise.basiListclass.length > 0) {
                                $scope.asmcL_ClassName = promise.basiListclass[0].className;
                            }
                            if (promise.basiListsectiont.length > 0) {
                                $scope.asmC_SectionName = promise.basiListsectiont[0].sectionname;
                            }
                            if (promise.basiListsubject.length > 0) {
                                $scope.ismS_SubjectName = promise.basiListsubject[0].emE_ExamName;
                            }
                            if (promise.basicListYear.length > 0) {
                                $scope.asmaY_Year = promise.basicListYear[0].yearname;
                            }


                            $scope.classteacher = promise.classteacher;
                            $scope.classteachername = $scope.classteacher[0].empname;

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

        //for print
        $scope.printData = function () {

            if ($scope.checkoruncheck == true) {
                var innerContents = document.getElementById("Baldwin1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            } else {
                var innerContents = document.getElementById("Baldwin").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }



        }
        // end for print


        $scope.showdetails = function () {

            $scope.repoershow = false;
            $scope.electivestd = [];
            $scope.electivesub = [];
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("VikasaSchoolExamWiseCumulativeReport/savedetails", data).
                    then(function (promise) {

                        if (promise.savelist !== null) {

                            if ($scope.checkoruncheck === true) {
                                $scope.checkbox1 = true;
                                $scope.checkbox = false;
                            } else {
                                $scope.checkbox1 = false;
                                $scope.checkbox = true;
                            }


                            if (promise.classteacher !== null && promise.classteacher.length>0) {
                                $scope.classteacher = promise.classteacher;
                                $scope.classteachername = $scope.classteacher[0].empname;
                            }

                            $scope.repoershow = true;
                            angular.forEach($scope.classDropdown, function (itm) {
                                if (itm.asmcL_Id == $scope.asmcL_Id) {
                                    $scope.cla = itm.asmcL_ClassName;
                                }
                            });
                            angular.forEach($scope.yearlt, function (itm) {
                                if (itm.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yr = itm.asmaY_Year;
                                }
                            });
                            angular.forEach($scope.sectionDropdown, function (itm) {
                                if (itm.asmS_Id == $scope.asmS_Id) {
                                    $scope.sec = itm.asmC_SectionName;
                                }
                            });
                            angular.forEach($scope.exsplt, function (itm) {
                                if (itm.emE_Id == $scope.emE_Id) {
                                    $scope.exmmid = itm.emE_ExamName;
                                }
                            });


                            //inst name binding
                            $scope.instname = promise.instname;
                            $scope.inst_name = $scope.instname[0].mI_Name;


                            //MB
                            $scope.examsubjectwise_details = promise.examsubjectwise_details;
                            //MB

                            $scope.studentslt = promise.savelist;
                            $scope.studentslt1 = promise.subjlist;

                            var temp_list = [];
                            for (var x = 0; x < promise.savelist.length; x++) {
                                var stu_id = promise.savelist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savelist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg, estmpS_SectionAverage: opq.estmpS_SectionAverage });
                                    }
                                })
                                if (temp_list.length == 0) {
                                    temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, estmP_SectionRank: promise.savelist[x].estmP_SectionRank, sub_list: stu_subj_list });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {
                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;
                                        }
                                    })
                                    if (already_cnt == 0) {
                                        temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_LastName: promise.savelist[x].amsT_LastName, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, classheld: promise.savelist[x].classheld, classatt: promise.savelist[x].classatt, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, estmP_SectionRank: promise.savelist[x].estmP_SectionRank, sub_list: stu_subj_list });
                                    }
                                }

                            }
                            $scope.studentslt = temp_list;

                            angular.forEach($scope.studentslt, function (oobj) {

                                $scope.tmparrry = [];
                                angular.forEach($scope.studentslt1, function (oobj1) {
                                    var ccount = 0;
                                    angular.forEach(oobj.sub_list, function (oobj2) {
                                        if (oobj1.ismS_Id == oobj2.ismS_Id) {
                                            angular.forEach($scope.examsubjectwise_details, function (sub_det) {
                                                if (sub_det.ismS_Id == oobj2.ismS_Id) {
                                                    oobj2.eyceS_MarksDisplayFlg = sub_det.eyceS_MarksDisplayFlg;
                                                    oobj2.eyceS_GradeDisplayFlg = sub_det.eyceS_GradeDisplayFlg;
                                                }
                                            })
                                            ccount += 1;
                                            if (oobj2.estmpS_PassFailFlg != 'AB') {
                                                oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_ObtainedMarks;
                                                oobj2.hema_estmpS_ObtainedGrade = oobj2.estmpS_ObtainedGrade;
                                            }
                                            else if (oobj2.estmpS_PassFailFlg == 'AB') {
                                                oobj2.hema_estmpS_ObtainedMarks = oobj2.estmpS_PassFailFlg;
                                            }
                                            $scope.tmparrry.push(oobj2);
                                        }
                                        //else {
                                        //    ccount += 1;
                                        //    //oobj2.hema_estmpS_ObtainedMarks = "";
                                        //}
                                    })
                                    if (ccount == 0) {
                                        var obj = {};
                                        obj.hema_estmpS_ObtainedMarks = "";
                                        obj.hema_estmpS_ObtainedGrade = "";
                                        $scope.tmparrry.push(obj);
                                    }
                                })
                                oobj.sub_list = $scope.tmparrry;
                            })



                            if (promise.savenonsubjlist != null && promise.savenonsubjlist.length > 0) {
                                angular.forEach($scope.clslist, function (itm) {
                                    if (itm.asmcL_Id == $scope.asmcL_Id) {
                                        $scope.cla = itm.asmcL_ClassName;
                                    }
                                })
                                angular.forEach($scope.yearlt, function (itm) {
                                    if (itm.asmaY_Id == $scope.asmaY_Id) {
                                        $scope.yr = itm.asmaY_Year;
                                    }
                                })
                                angular.forEach($scope.seclist, function (itm) {
                                    if (itm.asmS_Id == $scope.asmS_Id) {
                                        $scope.sec = itm.asmC_SectionName;
                                    }
                                })
                                angular.forEach($scope.exsplt, function (itm) {
                                    if (itm.emE_Id == $scope.emE_Id) {
                                        $scope.exmmid = itm.emE_ExamName;
                                    }
                                })


                                $scope.electivestd = promise.savenonsubjlist;
                                $scope.electivesub = promise.nonsubjlist;

                                var temp_list = [];
                                for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                    var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                    var stu_subj_list = [];
                                    angular.forEach(promise.savenonsubjlist, function (opq) {
                                        if (opq.amsT_Id == stu_id) {
                                            stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg });
                                        }
                                    })
                                    if (temp_list.length == 0) {
                                        temp_list.push({
                                            student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks, estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage, estmP_TotalGrade: promise.savenonsubjlist[x].estmP_TotalGrade, estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, sub_list: stu_subj_list
                                        });
                                    }
                                    else if (temp_list.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(temp_list, function (opq1) {
                                            if (opq1.student_id == stu_id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            temp_list.push({ student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks, estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage, estmP_TotalGrade: promise.savenonsubjlist[x].estmP_TotalGrade, estmP_SectionRank: promise.savenonsubjlist[x].estmP_SectionRank, sub_list: stu_subj_list });
                                        }
                                    }

                                }
                                $scope.nonstudentslt = temp_list;
                                $scope.newarray1 = [];
                                angular.forEach($scope.studentslt, function (testobj) {

                                    angular.forEach($scope.electivesub, function (testobjele) {

                                        angular.forEach($scope.nonstudentslt, function (testobj1) {
                                            if (testobj.student_id == testobj1.student_id) {
                                                angular.forEach(testobj1.sub_list, function (testobj2) {
                                                    if (testobjele.ismS_Id == testobj2.ismS_Id) {
                                                        $scope.newarray1.push({ stuid: testobj.student_id, subid: testobjele.ismS_Id, abc: testobj2.estmpS_ObtainedMarks + ' ' + testobj2.estmpS_ObtainedGrade });
                                                    }

                                                });
                                            }

                                        });
                                    });
                                });

                                //$scope.studentslt = [];
                                //angular.forEach(temp_list, function (xyz) {
                                //    $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName });
                                //})
                            }

                        }
                        else if (promise.savenonsubjlist != null) {
                            $scope.repoershow = true;
                            angular.forEach($scope.clslist, function (itm) {
                                if (itm.asmcL_Id == $scope.asmcL_Id) {
                                    $scope.cla = itm.asmcL_ClassName;
                                }
                            })
                            angular.forEach($scope.yearlt, function (itm) {
                                if (itm.asmaY_Id == $scope.asmaY_Id) {
                                    $scope.yr = itm.asmaY_Year;
                                }
                            })
                            angular.forEach($scope.seclist, function (itm) {
                                if (itm.asmS_Id == $scope.asmS_Id) {
                                    $scope.sec = itm.asmC_SectionName;
                                }
                            })
                            angular.forEach($scope.exsplt, function (itm) {
                                if (itm.emE_Id == $scope.emE_Id) {
                                    $scope.exmmid = itm.emE_ExamName;
                                }
                            })


                            $scope.electivestd = promise.savenonsubjlist;
                            $scope.electivesub = promise.nonsubjlist;

                            var temp_list = [];
                            for (var x = 0; x < promise.savenonsubjlist.length; x++) {
                                var stu_id = promise.savenonsubjlist[x].amsT_Id;
                                var stu_subj_list = [];
                                angular.forEach(promise.savenonsubjlist, function (opq) {
                                    if (opq.amsT_Id == stu_id) {
                                        stu_subj_list.push({ amsT_Id: opq.amsT_Id, ismS_Id: opq.ismS_Id, ismS_SubjectName: opq.amsT_Id.ismS_SubjectName, estmpS_MaxMarks: opq.estmpS_MaxMarks, estmpS_ObtainedMarks: opq.estmpS_ObtainedMarks, estmpS_ObtainedGrade: opq.estmpS_ObtainedGrade, estmpS_PassFailFlg: opq.estmpS_PassFailFlg });
                                    }
                                })
                                if (temp_list.length == 0) {
                                    temp_list.push({
                                        student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks, estmP_TotalGrade: promise.savenonsubjlist[x].estmP_TotalGrade, estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage, sub_list: stu_subj_list
                                    });
                                }
                                else if (temp_list.length > 0) {
                                    var already_cnt = 0;
                                    angular.forEach(temp_list, function (opq1) {
                                        if (opq1.student_id == stu_id) {
                                            already_cnt += 1;
                                        }
                                    })
                                    if (already_cnt == 0) {
                                        temp_list.push({ student_id: promise.savenonsubjlist[x].amsT_Id, amsT_FirstName: promise.savenonsubjlist[x].amsT_FirstName, amsT_LastName: promise.savenonsubjlist[x].amsT_LastName, amsT_AdmNo: promise.savenonsubjlist[x].amsT_AdmNo, classheld: promise.savenonsubjlist[x].classheld, classatt: promise.savenonsubjlist[x].classatt, estmP_TotalObtMarks: promise.savenonsubjlist[x].estmP_TotalObtMarks, estmP_TotalMaxMarks: promise.savenonsubjlist[x].estmP_TotalMaxMarks, estmP_TotalGrade: promise.savenonsubjlist[x].estmP_TotalGrade, estmP_Percentage: promise.savenonsubjlist[x].estmP_Percentage, sub_list: stu_subj_list });
                                    }
                                }

                            }
                            $scope.nonstudentslt = temp_list;
                            $scope.studentslt = [];
                            angular.forEach(temp_list, function (xyz) {
                                $scope.studentslt.push({ student_id: xyz.student_id, amsT_FirstName: xyz.amsT_FirstName, amsT_LastName: xyz.amsT_LastName, classheld: xyz.classheld, classatt: xyz.classatt });
                            })


                        }
                        else if (promise.savenonsubjlist == null && promise.savelist == null) {
                            swal('No record Found');
                        }


                        console.log($scope.studentslt)


                        $scope.sectionaverage = promise.sectionaverage;


                        angular.forEach($scope.studentslt, function (oobj) {
                            angular.forEach($scope.nonstudentslt, function (oobj2) {
                                if (oobj2.student_id == oobj.student_id) {
                                    $scope.tmparrry1 = [];
                                    angular.forEach($scope.electivesub, function (oobj1) {
                                        var ccount1 = 0;
                                        angular.forEach(oobj2.sub_list, function (oobj3) {
                                            //ccount1 += 1;
                                            if (oobj1.ismS_Id == oobj3.ismS_Id) {
                                                angular.forEach($scope.examsubjectwise_details, function (sub_det) {
                                                    if (sub_det.ismS_Id == oobj3.ismS_Id) {
                                                        oobj3.eyceS_MarksDisplayFlg = sub_det.eyceS_MarksDisplayFlg;
                                                        oobj3.eyceS_GradeDisplayFlg = sub_det.eyceS_GradeDisplayFlg;
                                                    }
                                                })

                                                ccount1 += 1;
                                                if (oobj3.estmpS_PassFailFlg != 'AB' && oobj3.estmpS_PassFailFlg != 'L' && oobj3.estmpS_PassFailFlg != 'OD' && oobj3.estmpS_PassFailFlg != 'M') {
                                                    oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_ObtainedMarks;
                                                    oobj3.hema_estmpS_ObtainedGrade = oobj3.estmpS_ObtainedGrade;
                                                }
                                                else if (oobj3.estmpS_PassFailFlg == 'AB' || oobj3.estmpS_PassFailFlg == 'L' || oobj3.estmpS_PassFailFlg == 'OD' || oobj3.estmpS_PassFailFlg == 'M') {
                                                    oobj3.hema_estmpS_ObtainedMarks = oobj3.estmpS_PassFailFlg;
                                                }
                                                $scope.tmparrry1.push(oobj3);
                                            }

                                        })
                                        if (ccount1 == 0) {
                                            var obj1 = {};
                                            obj1.hema_estmpS_ObtainedMarks = "";
                                            obj1.hema_estmpS_ObtainedGrade = "";
                                            $scope.tmparrry1.push(obj1);
                                        }
                                    })
                                    oobj.sub_list_e = $scope.tmparrry1;
                                }
                            })

                        })
                    })
            }
        };


        $scope.exportToExcel = function (tableIds) {

            if ($scope.checkoruncheck == true) {
                var excelname = "Cumulative Report Exam " + $scope.exmmid + "Year : " + $scope.yr + " Class-Section : " + $scope.cla + "-" + $scope.sec + ".xls";

                var exportHref = Excel.tableToExcel('#table2', 'sheet name');

                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);

            } else {
                var excelname = "Cumulative Report Exam " + $scope.exmmid + "Year : " + $scope.yr + " Class-Section : " + $scope.cla + "-" + $scope.sec + ".xls";

                var exportHref = Excel.tableToExcel('#table1', 'sheet name');

                $timeout(function () {
                    var a = document.createElement('a');
                    a.href = exportHref;
                    a.download = excelname;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                }, 100);

            }



            //var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
            //$timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.fonts = [
            { name: '10px', size: '10px ', class: 'font10' },
            { name: '11px', size: '11px ', class: 'font11' },
            { name: '12px', size: '12px ', class: 'font12' },
            { name: '13px', size: '13px ', class: 'font13' },
            { name: '14px', size: '14px ', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' }, { name: '18px', size: '18px', class: 'font18' }, { name: '25px', size: '25px', class: 'font25' }
        ];
        $scope.font_size = 'font25';

    }

})();