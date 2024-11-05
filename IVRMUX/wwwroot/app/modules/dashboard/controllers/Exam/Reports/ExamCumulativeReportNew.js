(function () {
    'use strict';
    angular.module('app').controller('ExamCumulativeReportNew', ExamCumulativeReportNew)
    ExamCumulativeReportNew.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function ExamCumulativeReportNew($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.printbtn = false;
        $scope.exclbtn = false;
        $scope.colarraytmp = [];
        $scope.repoershow = false;        
        $scope.font_size = 'font25';
        $scope.size = '10px';
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.submitted = false;
        $scope.searchchkbx = "";
        $scope.applicable = true;
        $scope.nonapplicable = true;
        $scope.Left_Flag = true;
        $scope.Deactive_Flag = true;

        $scope.fonts = [
            { name: '10px', size: '10px', class: 'font10' },
            { name: '11px', size: '11px', class: 'font11' },
            { name: '12px', size: '12px', class: 'font12' },
            { name: '13px', size: '13px', class: 'font13' },
            { name: '14px', size: '14px', class: 'font14' },
            { name: '15px', size: '15px', class: 'font15' },
            { name: '16px', size: '16px', class: 'font16' },
            { name: '17px', size: '17px', class: 'font17' },
            { name: '18px', size: '18px', class: 'font18' },
            { name: '25px', size: '25px', class: 'font25' }
        ];
        
        $scope.BindData = function () {
            apiService.getDATA("HHSMIDFINALCumReport/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;                
            });
        };

        $scope.get_classes = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
          
            apiService.create("ExamCalculation_SSSE/get_classes", data).then(function (promise) {
                $scope.clslist = promise.classlist;
                $scope.asmcL_Id = "";
                $scope.asmS_Id = "";
                $scope.emE_Id = "";
                $scope.seclist = [];
                $scope.exsplt = [];
                $scope.studentlistdetails = [];
                $scope.searchchkbx = "";
                if (promise.classlist === null || promise.classlist === "") {
                    swal("Classes are Not Mapped To Selected Academic Year!!!");
                }
            });
        };

        $scope.get_cls_sections = function (cls_id) {
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
                    $scope.studentlistdetails = [];
                    $scope.searchchkbx = "";
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

        $scope.get_exams = function (ASMS_Id, ASMCL_Id, ASMAY_Id) {
            $scope.studentlistdetails = [];
            $scope.searchchkbx = "";
            if (ASMAY_Id !== null && ASMAY_Id !== undefined && ASMCL_Id !== null && ASMCL_Id !== undefined) {
                var data = {
                    "ASMS_Id": ASMS_Id,
                    "ASMCL_Id": ASMCL_Id,
                    "ASMAY_Id": ASMAY_Id,
                    "Left_Flag": $scope.Left_Flag,
                    "Deactive_Flag": $scope.Deactive_Flag
                };                
                apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {
                    $scope.emE_Id = "";
                    $scope.exsplt = promise.exmstdlist;
                    if (promise.exmstdlist === null || promise.exmstdlist === "") {
                        swal("Exams are Not Mapped To Selected Class And Section!!!");
                    }

                    $scope.studentlistdetails = promise.studentlist;

                    $scope.all = true;
                    angular.forEach($scope.studentlistdetails, function (dd) {
                        dd.checkedsub = true;
                    });
                });
            }
            else {
                swal("Please Select Academic Year , Class , Section First !!!");
                $scope.asmS_Id = "";
            }
        };

        $scope.OnChangeLeftFlag = function () {
            $scope.report = false;
            $scope.print = true;
            $scope.searchchkbx = "";
            $scope.studentlistdetails = [];
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "Left_Flag": $scope.Left_Flag,
                "Deactive_Flag": $scope.Deactive_Flag
            };

            apiService.create("ExamCalculation_SSSE/get_exams", data).then(function (promise) {
                $scope.studentlistdetails = promise.studentlist;
                $scope.all = true;
                angular.forEach($scope.studentlistdetails, function (dd) {
                    dd.checkedsub = true;
                });
            });
        };

        $scope.onselectcategory = function () {
            $scope.report = false;
            $scope.print = true;
            $scope.repoershow = false;
        };

        $scope.saveddata = function () {
            $scope.exmrank = [];
            $scope.printbtn = false;
            $scope.exclbtn = false;
            $scope.repoershow = false;
            $scope.electivestd = [];
            $scope.electivesub = [];
            $scope.submitted = true;
            $scope.colarrayall = [];
            $scope.selectedamstids = [];

            if ($scope.myForm.$valid) {

                angular.forEach($scope.studentlistdetails, function (dd) {
                    if (dd.checkedsub === true) {
                        $scope.selectedamstids.push(dd.amsT_Id);
                    }
                });
                var data = {
                    "EME_ID": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "AMST_Ids": $scope.selectedamstids,
                };               
                apiService.create("HHSMIDFINALCumReport/cumulativereport", data).then(function (promise) {

                    $scope.masterinst = promise.configuration;
                    var count = 0;
                    if ($scope.masterinst !== null && $scope.masterinst.length > 0) {
                        $scope.colarrayall = [{ title: "SLNO", template: "<span class='row-number'></span>", width: 100 },
                        { name: 'amsT_FirstName', field: 'amsT_FirstName', title: 'Student Name', width: 200 }];

                        if ($scope.masterinst[0].exmConfig_RegnoColumnDisplay === true) {

                            $scope.colarrayall.push({ name: 'amsT_RegistrationNo', field: 'amsT_RegistrationNo', title: 'Reg.NO', width: 100 });

                            $scope.regno = true;
                            count = count + 1;

                        } else {
                            $scope.regno = false;
                        }

                        if ($scope.masterinst[0].exmConfig_AdmnoColumnDisplay === true) {

                            $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });

                            $scope.admno = true;
                            count = count + 1;

                        } else {
                            $scope.admno = false;
                        }

                        if ($scope.masterinst[0].exmConfig_RollnoColumnDisplay === true) {

                            $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });

                            $scope.rollno = true;
                            count = count + 1;

                        } else {
                            $scope.rollno = false;
                        }

                        if (count === 0) {

                            $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                            $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });

                            $scope.admno = true;
                            $scope.rollno = true;
                        }

                    } else {

                        $scope.colarrayall.push({ name: 'amsT_AdmNo', field: 'amsT_AdmNo', title: 'Adm.NO', width: 100 });
                        $scope.colarrayall.push({ name: 'amaY_RollNo', field: 'amaY_RollNo', title: 'Roll.NO', width: 100 });
                        $scope.admno = true;
                        $scope.rollno = true;
                    }

                    angular.forEach($scope.clslist, function (itm) {
                        if (parseInt(itm.asmcL_Id) === parseInt($scope.asmcL_Id)) {
                            $scope.cla = itm.asmcL_ClassName;
                        }
                    });

                    angular.forEach($scope.yearlt, function (itm) {
                        if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                            $scope.yr = itm.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.seclist, function (itm) {
                        if (parseInt(itm.asmS_Id) === parseInt($scope.asmS_Id)) {
                            $scope.sec = itm.asmC_SectionName;
                        }
                    });

                    angular.forEach($scope.exsplt, function (itm) {
                        if (parseInt(itm.emE_Id) === parseInt($scope.emE_Id)) {
                            $scope.exmmid = itm.emE_ExamName;
                        }
                    });

                    $scope.printbtn = true;
                    $scope.exclbtn = true;

                    $scope.instname = promise.instname;

                    if ($scope.instname !== null && $scope.instname.length > 0) {
                        $scope.inst_name = $scope.instname[0].mI_Name;
                        $scope.add = $scope.instname[0].mI_Address1;
                        $scope.city = $scope.instname[0].ivrmmcT_Name;
                        $scope.pin = $scope.instname[0].mI_Pincode;
                    }

                    $scope.studlist = promise.studlist;
                    $scope.exmrank = promise.exmrank;

                    $scope.savelistnew = promise.savelistnew;

                    $scope.subjectlistwithdetails = promise.subjectlistwithdetails;

                    if ($scope.savelistnew !== null && $scope.savelistnew.length > 0) {

                        $scope.repoershow = true;

                        $scope.subjectlist = [];

                        angular.forEach($scope.savelistnew, function (dd) {
                            if ($scope.subjectlist.length === 0) {
                                $scope.subjectlist.push({ ISMS_Id: dd.ISMS_Id, ISMS_SubjectName: dd.ISMS_SubjectName, gradedisplay: dd.gradedisplay, marksdisplay: dd.marksdisplay, apptoresult: dd.apptoresult, EYCES_SubExamFlg: dd.EYCES_SubExamFlg, EYCES_SubSubjectFlg: dd.EYCES_SubSubjectFlg });
                            } else if ($scope.subjectlist.length > 0) {
                                var count = 0;
                                angular.forEach($scope.subjectlist, function (d) {
                                    if (d.ISMS_Id === dd.ISMS_Id) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.subjectlist.push({
                                        ISMS_Id: dd.ISMS_Id, ISMS_SubjectName: dd.ISMS_SubjectName, gradedisplay: dd.gradedisplay, marksdisplay: dd.marksdisplay, apptoresult: dd.apptoresult, EYCES_SubExamFlg: dd.EYCES_SubExamFlg, EYCES_SubSubjectFlg: dd.EYCES_SubSubjectFlg
                                    });
                                }
                            }
                        });


                        /***************** GET SUBSUBJECT LIST *************/
                        $scope.subsubject = [];
                        angular.forEach($scope.subjectlist, function (dd) {
                            $scope.subsubject = [];
                            if (dd.EYCES_SubSubjectFlg === true && dd.EYCES_SubExamFlg === false) {
                                angular.forEach($scope.subjectlistwithdetails, function (d) {
                                    if (d.ISMS_Id === dd.ISMS_Id) {
                                        if ($scope.EMSS_SubSubjectName !== "") {
                                            $scope.subsubject.push({
                                                subjectid: dd.ISMS_Id, subjectname: dd.ISMS_SubjectName,
                                                subsubjectname: d.EMSS_SubSubjectName, gradedisplay: d.EYCES_GradeDisplayFlg, marksdisplay: d.EYCES_MarksDisplayFlg
                                            });
                                        }
                                    }
                                });
                                dd.subsubjectarray = $scope.subsubject;
                            }
                        });


                        /***************** GET SUBEXAM LIST *************/
                        $scope.subexam = [];
                        angular.forEach($scope.subjectlist, function (dd) {
                            $scope.subexam = [];
                            if (dd.EYCES_SubSubjectFlg === false && dd.EYCES_SubExamFlg === true) {
                                angular.forEach($scope.subjectlistwithdetails, function (d) {
                                    if (d.ISMS_Id === dd.ISMS_Id) {
                                        if ($scope.EMSE_SubExamName !== "") {
                                            $scope.subexam.push({
                                                subjectid: dd.ISMS_Id, subjectname: dd.ISMS_SubjectName,
                                                subexamname: d.EMSE_SubExamName, gradedisplay: d.EYCES_GradeDisplayFlg, marksdisplay: d.EYCES_MarksDisplayFlg
                                            });
                                        }
                                    }
                                });
                            }

                            dd.subexamarray = $scope.subexam;
                        });


                        /***************** GET SUBSUBJECT SUBEXAM LIST *************/
                        $scope.subsubject_subexam = [];
                        $scope.subsubject = [];
                        angular.forEach($scope.subjectlist, function (dd) {
                            $scope.subsubject = [];
                            if (dd.EYCES_SubSubjectFlg === true && dd.EYCES_SubExamFlg === true) {
                                angular.forEach($scope.subjectlistwithdetails, function (d) {
                                    if (d.ISMS_Id === dd.ISMS_Id) {
                                        if ($scope.EMSS_SubSubjectName !== "") {
                                            if ($scope.subsubject.length === 0) {
                                                $scope.subsubject.push({
                                                    subjectid: dd.ISMS_Id, subjectname: dd.ISMS_SubjectName, subsubjectname: d.EMSS_SubSubjectName
                                                });
                                            } else if ($scope.subsubject.length > 0) {
                                                var count1 = 0;
                                                angular.forEach($scope.subsubject, function (s) {
                                                    if (s.subjectid === d.ISMS_Id && s.subsubjectname === d.EMSS_SubSubjectName) {
                                                        count1 += 1;
                                                    }
                                                });

                                                if (count1 === 0) {
                                                    $scope.subsubject.push({
                                                        subjectid: dd.ISMS_Id, subjectname: dd.ISMS_SubjectName, subsubjectname: d.EMSS_SubSubjectName
                                                    });
                                                }
                                            }
                                        }
                                    }
                                });
                                dd.subsubjectarray = $scope.subsubject;

                                $scope.subsubject_subexam = [];
                                angular.forEach(dd.subsubjectarray, function (d) {
                                    $scope.subsubject_subexam = [];
                                    angular.forEach($scope.subjectlistwithdetails, function (ddd) {
                                        if (dd.ISMS_Id === ddd.ISMS_Id && d.subsubjectname === ddd.EMSS_SubSubjectName) {
                                            $scope.subsubject_subexam.push({
                                                subsubjectnameid: d.subsubjectname, subexamname: ddd.EMSE_SubExamName
                                                , gradedisplay: ddd.EYCES_GradeDisplayFlg, marksdisplay: ddd.EYCES_MarksDisplayFlg
                                            });
                                        }
                                    });
                                    d.subsubject_subexamarray = $scope.subsubject_subexam;
                                });
                            }
                        });
                        console.log($scope.subjectlist);

                        $scope.details = [];
                        angular.forEach($scope.studlist, function (dd) {
                            $scope.details.push({
                                amsT_Id: dd.amsT_Id, amsT_FirstName: dd.amsT_FirstName, amsT_RegistrationNo: dd.amsT_RegistrationNo, amsT_AdmNo: dd.amsT_AdmNo,
                                amaY_RollNo: dd.amaY_RollNo
                            });
                        });

                        console.log($scope.details);
                        console.log($scope.colarrayall);

                        $scope.subexamlist = [];
                        $scope.subsubsubjectlist = [];
                        $scope.subsubsubject_subexamlist = [];

                        $scope.subjectexamlist = [];
                        $scope.marksgrade = [];

                        $scope.subjectsubsubjectexamlist = [];
                        $scope.subsubjectksgrade = [];

                        $scope.subjectmainexamlist = [];
                        $scope.subjectksgrade = [];

                        $scope.subexamcolumns = [];

                        //**** CREATING UNIQUE COLUMNS NAMES ****//
                        angular.forEach($scope.subjectlist, function (dd) {

                            var subjectnamenew = dd.ISMS_SubjectName;
                            subjectnamenew = subjectnamenew.replace(/[^a-zA-Z0-9]/g, "");

                            $scope.subjectexamlist = [];
                            $scope.marksgrade = [];

                            $scope.subjectsubsubjectexamlist = [];
                            $scope.subsubjectkmarksgrade = [];

                            $scope.subjectmainexamlist = [];
                            $scope.subjectksgrade = [];

                            $scope.subjectsubsubjsubexamlist = [];
                            $scope.subjectsubsubjsubexamksgrade = [];

                            $scope.subexamcolumns = [];
                            $scope.subjectsubsubjectexamlistnew = [];

                            if (dd.EYCES_SubSubjectFlg === false && dd.EYCES_SubExamFlg === true) {
                                $scope.subexamlist = [];
                                $scope.marksgrade = [];

                                angular.forEach(dd.subexamarray, function (obj2) {
                                    $scope.marksgrade = [];
                                    var subexamname = obj2.subexamname;
                                    subexamname = subexamname.replace(/[^a-zA-Z0-9]/g, "");

                                    if (obj2.marksdisplay === true && obj2.gradedisplay === true) {
                                        $scope.marksgrade.push({
                                            title: 'M', field: "unique" + obj2.subjectid + subexamname + 'M', width: 100
                                        });
                                        $scope.marksgrade.push({
                                            title: 'G', field: "unique" + obj2.subjectid + subexamname + 'G', width: 100
                                        });
                                    }

                                    else if (obj2.marksdisplay === true && obj2.gradedisplay === false) {
                                        $scope.marksgrade.push({
                                            title: 'M', field: "unique" + obj2.subjectid + subexamname + 'M', width: 100
                                        });
                                    }

                                    else if (obj2.marksdisplay === false && obj2.gradedisplay === true) {
                                        $scope.marksgrade.push({
                                            title: 'G', field: "unique" + obj2.subjectid + subexamname + 'G', width: 100
                                        });
                                    }

                                    else {
                                        $scope.marksgrade.push({
                                            title: 'M', field: "unique" + obj2.subjectid + subexamname + 'M', width: 100
                                        });
                                    }

                                    $scope.subexamlist.push({
                                        title: obj2.subexamname, field: "unique" + obj2.subjectid + subexamname, width: 200,
                                        submarksflag: obj2.marksdisplay, subgradeflag: obj2.gradedisplay, columns: $scope.marksgrade
                                    });
                                });

                                $scope.colarrayall.push({
                                    name: dd.ISMS_Id, field: dd.ISMS_Id, title: dd.ISMS_SubjectName, width: 100, columns: $scope.subexamlist
                                });
                            }

                            if (dd.EYCES_SubSubjectFlg === true && dd.EYCES_SubExamFlg === false) {
                                $scope.subjectsubsubjectexamlist = [];
                                $scope.subsubjectkmarksgrade = [];

                                angular.forEach(dd.subsubjectarray, function (obj2) {
                                    $scope.subsubjectkmarksgrade = [];
                                    var subsubjectname = obj2.subsubjectname;
                                    subsubjectname = subsubjectname.replace(/[^a-zA-Z0-9]/g, "");

                                    if (obj2.marksdisplay === true && obj2.gradedisplay === true) {
                                        $scope.subsubjectkmarksgrade.push({
                                            title: 'M', field: "unique" + obj2.subjectid + subsubjectname + 'M', width: 100
                                        });
                                        $scope.subsubjectkmarksgrade.push({
                                            title: 'G', field: "unique" + obj2.subjectid + subsubjectname + 'G', width: 100
                                        });
                                    }

                                    else if (obj2.marksdisplay === true && obj2.gradedisplay === false) {
                                        $scope.subsubjectkmarksgrade.push({
                                            title: 'M', field: "unique" + obj2.subjectid + subsubjectname + 'M', width: 100
                                        });
                                    }

                                    else if (obj2.marksdisplay === false && obj2.gradedisplay === true) {
                                        $scope.subsubjectkmarksgrade.push({
                                            title: 'G', field: "unique" + obj2.subjectid + subsubjectname + 'G', width: 100
                                        });
                                    }

                                    else {
                                        $scope.subsubjectkmarksgrade.push({
                                            title: 'M', field: "unique" + obj2.subjectid + subsubjectname + 'M', width: 100
                                        });
                                    }

                                    $scope.subjectsubsubjectexamlist.push({
                                        title: obj2.subsubjectname, field: "unique" + obj2.subjectid + subsubjectname, width: 200,
                                        submarksflag: obj2.marksdisplay, subgradeflag: obj2.gradedisplay, columns: $scope.subsubjectkmarksgrade
                                    });
                                });

                                $scope.colarrayall.push({
                                    name: dd.ISMS_Id, field: dd.ISMS_Id, title: dd.ISMS_SubjectName, width: 100,
                                    columns: $scope.subjectsubsubjectexamlist
                                });
                            }

                            if (dd.EYCES_SubSubjectFlg === false && dd.EYCES_SubExamFlg === false) {
                                $scope.subjectmainexamlist = [];
                                $scope.subjectksgrade = [];

                                if (dd.marksdisplay === true && dd.gradedisplay === true) {
                                    $scope.subjectksgrade.push({
                                        title: 'M', field: "unique" + dd.ISMS_Id + subjectnamenew + 'M', width: 100
                                    });
                                    $scope.subjectksgrade.push({
                                        title: 'G', field: "unique" + dd.ISMS_Id + subjectnamenew + 'G', width: 100
                                    });
                                }

                                else if (dd.marksdisplay === true && dd.gradedisplay === false) {
                                    $scope.subjectksgrade.push({
                                        title: 'M', field: "unique" + dd.ISMS_Id + subjectnamenew + 'M', width: 100
                                    });
                                }

                                else if (dd.marksdisplay === false && dd.gradedisplay === true) {
                                    $scope.subjectksgrade.push({
                                        title: 'G', field: "unique" + dd.ISMS_Id + subjectnamenew + 'G', width: 100
                                    });
                                }

                                else {
                                    $scope.subjectksgrade.push({
                                        title: 'M', field: "unique" + dd.ISMS_Id + subsubjectname + 'M', width: 100
                                    });
                                }

                                $scope.colarrayall.push({
                                    name: dd.ISMS_Id, field: dd.ISMS_Id, title: dd.ISMS_SubjectName, width: 100,
                                    columns: $scope.subjectksgrade
                                });
                            }

                            if (dd.EYCES_SubSubjectFlg === true && dd.EYCES_SubExamFlg === true) {

                                angular.forEach(dd.subsubjectarray, function (ss) {
                                    $scope.subexamcolumns = [];

                                    var subsubjectnamenew = ss.subsubjectname;
                                    subsubjectnamenew = subsubjectnamenew.replace(/[^a-zA-Z0-9]/g, "");

                                    angular.forEach(ss.subsubject_subexamarray, function (se) {
                                        $scope.subjectsubsubjsubexamksgrade = [];
                                        var subexamnamenew = se.subexamname;
                                        subexamnamenew = subexamnamenew.replace(/[^a-zA-Z0-9]/g, "");

                                        if (se.marksdisplay === true && se.gradedisplay === true) {
                                            $scope.subjectsubsubjsubexamksgrade.push({
                                                title: 'M', field: "unique" + dd.ISMS_Id + subsubjectnamenew + subexamnamenew + 'M', width: 100
                                            });
                                            $scope.subjectsubsubjsubexamksgrade.push({
                                                title: 'G', field: "unique" + dd.ISMS_Id + subsubjectnamenew + subexamnamenew + 'G', width: 100
                                            });
                                        }

                                        else if (se.marksdisplay === true && se.gradedisplay === false) {
                                            $scope.subjectsubsubjsubexamksgrade.push({
                                                title: 'M', field: "unique" + dd.ISMS_Id + subsubjectnamenew + subexamnamenew + 'M', width: 100
                                            });
                                        }

                                        else if (se.marksdisplay === false && se.gradedisplay === true) {
                                            $scope.subjectsubsubjsubexamksgrade.push({
                                                title: 'G', field: "unique" + dd.ISMS_Id + subsubjectnamenew + subexamnamenew + 'G', width: 100
                                            });
                                        }

                                        else {
                                            $scope.subjectsubsubjsubexamksgrade.push({
                                                title: 'M', field: "unique" + dd.ISMS_Id + subsubjectnamenew + subexamnamenew + 'M', width: 100
                                            });
                                        }

                                        $scope.subexamcolumns.push({
                                            title: se.subexamname, field: "unique" + dd.ISMS_Id + subsubjectnamenew + subexamnamenew, width: 100,
                                            columns: $scope.subjectsubsubjsubexamksgrade
                                        });

                                    });

                                    $scope.subjectsubsubjectexamlistnew.push({
                                        title: ss.subsubjectname, field: "unique" + dd.ISMS_Id + subsubjectnamenew,
                                        columns: $scope.subexamcolumns
                                    });

                                });

                                $scope.colarrayall.push({ name: dd.ISMS_Id, field: dd.ISMS_Id, title: dd.ISMS_SubjectName, width: 100, columns: $scope.subjectsubsubjectexamlistnew });
                            }
                        });

                        $scope.colarrayall.push(
                            { title: 'Over All Total', name: 'estmP_TotalObtMarks', field: 'estmP_TotalObtMarks', width: 100 },
                            { title: 'Over All Grade', name: 'estmP_TotalGrade', field: 'estmP_TotalGrade', width: 100 },
                            { title: 'Max.Total', name: 'estmP_TotalMaxMarks', field: 'estmP_TotalMaxMarks', width: 100 },
                            { title: 'PER.(%)', name: 'estmP_Percentage', field: 'estmP_Percentage', width: 100 },
                            { title: 'Rank', name: 'estmP_SectionRank', field: 'estmP_SectionRank', width: 100 },
                            { title: 'PASS/FAIL', name: 'estmP_Result', field: 'estmP_Result', width: 100 }
                        );

                        console.log($scope.colarrayall);

                        //**** CREATING STUDENT FILEDS WITH MARKS ****//
                        angular.forEach($scope.details, function (stu) {
                            angular.forEach($scope.subjectlist, function (stusubj) {
                                var subjectnamenew1 = stusubj.ISMS_SubjectName;

                                subjectnamenew1 = subjectnamenew1.replace(/[^a-zA-Z0-9]/g, "");

                                if (stusubj.EYCES_SubExamFlg === true && stusubj.EYCES_SubSubjectFlg === false) {
                                    angular.forEach(stusubj.subexamarray, function (subexam) {
                                        var subjsubexam1 = subexam.subexamname;
                                        subjsubexam1 = subjsubexam1.replace(".", "");
                                        subjsubexam1 = subjsubexam1.replace("&", "");
                                        subjsubexam1 = subjsubexam1.replace(" ", "");
                                        subjsubexam1 = subjsubexam1.replace("/", "");
                                        subjsubexam1 = subjsubexam1.replace("-", "");
                                        subjsubexam1 = subjsubexam1.replace(" ", "");

                                        angular.forEach($scope.savelistnew, function (stumarks) {
                                            if (stu.amsT_Id === stumarks.AMST_Id && stusubj.ISMS_Id === stumarks.ISMS_Id
                                                && stumarks.EMSE_SubExamName === subexam.subexamname) {
                                                if (subexam.marksdisplay === true && subexam.gradedisplay === true) {
                                                    stu["unique" + stusubj.ISMS_Id + subjsubexam1 + 'M'] = stumarks.subsubjectmarks;
                                                    stu["unique" + stusubj.ISMS_Id + subjsubexam1 + 'G'] = stumarks.subsubjectgrade;
                                                }
                                                else if (subexam.marksdisplay === true && subexam.gradedisplay === false) {
                                                    stu["unique" + stusubj.ISMS_Id + subjsubexam1 + 'M'] = stumarks.subsubjectmarks;
                                                }
                                                else if (subexam.marksdisplay === false && subexam.gradedisplay === true) {
                                                    stu["unique" + stusubj.ISMS_Id + subjsubexam1 + 'G'] = stumarks.subsubjectgrade;
                                                }
                                                else {
                                                    stu["unique" + stusubj.ISMS_Id + subjsubexam1 + 'M'] = stumarks.subsubjectmarks;
                                                }
                                            }
                                        });
                                    });
                                }

                                if (stusubj.EYCES_SubExamFlg === false && stusubj.EYCES_SubSubjectFlg === true) {
                                    angular.forEach(stusubj.subsubjectarray, function (subsubj) {
                                        var subsubjectname1 = subsubj.subsubjectname;
                                        subsubjectname1 = subsubjectname1.replace(".", "");
                                        subsubjectname1 = subsubjectname1.replace("&", "");
                                        subsubjectname1 = subsubjectname1.replace(" ", "");
                                        subsubjectname1 = subsubjectname1.replace("/", "");
                                        subsubjectname1 = subsubjectname1.replace("-", "");
                                        subsubjectname1 = subsubjectname1.replace(" ", "");

                                        angular.forEach($scope.savelistnew, function (stumarks) {
                                            if (stu.amsT_Id === stumarks.AMST_Id && stusubj.ISMS_Id === stumarks.ISMS_Id
                                                && stumarks.EMSS_SubSubjectName === subsubj.subsubjectname) {
                                                if (subsubj.marksdisplay === true && subsubj.gradedisplay === true) {
                                                    stu["unique" + stusubj.ISMS_Id + subsubjectname1 + 'M'] = stumarks.subsubjectmarks;
                                                    stu["unique" + stusubj.ISMS_Id + subsubjectname1 + 'G'] = stumarks.subsubjectgrade;
                                                }
                                                else if (subsubj.marksdisplay === true && subsubj.gradedisplay === false) {
                                                    stu["unique" + stusubj.ISMS_Id + subsubjectname1 + 'M'] = stumarks.subsubjectmarks;
                                                }
                                                else if (subsubj.marksdisplay === false && subsubj.gradedisplay === true) {
                                                    stu["unique" + stusubj.ISMS_Id + subsubjectname1 + 'G'] = stumarks.subsubjectgrade;
                                                }
                                                else {
                                                    stu["unique" + stusubj.ISMS_Id + subsubjectname1 + 'M'] = stumarks.subsubjectmarks;
                                                }
                                            }
                                        });
                                    });
                                }

                                if (stusubj.EYCES_SubExamFlg === false && stusubj.EYCES_SubSubjectFlg === false) {
                                    angular.forEach($scope.savelistnew, function (stumarks) {
                                        if (stu.amsT_Id === stumarks.AMST_Id && stusubj.ISMS_Id === stumarks.ISMS_Id) {
                                            if (stusubj.marksdisplay === true && stusubj.gradedisplay === true) {
                                                stu["unique" + stusubj.ISMS_Id + subjectnamenew1 + 'M'] = stumarks.subsubjectmarks;
                                                stu["unique" + stusubj.ISMS_Id + subjectnamenew1 + 'G'] = stumarks.subsubjectgrade;
                                            }

                                            else if (stusubj.marksdisplay === true && stusubj.gradedisplay === false) {
                                                stu["unique" + stusubj.ISMS_Id + subjectnamenew1 + 'M'] = stumarks.subsubjectmarks;
                                            }

                                            else if (stusubj.marksdisplay === false && stusubj.gradedisplay === true) {
                                                stu["unique" + stusubj.ISMS_Id + subjectnamenew1 + 'G'] = stumarks.subsubjectgrade;
                                            }

                                            else {
                                                stu["unique" + stusubj.ISMS_Id + subjectnamenew1 + 'M'] = stumarks.subsubjectmarks;
                                            }
                                        }
                                    });
                                }

                                if (stusubj.EYCES_SubExamFlg === true && stusubj.EYCES_SubSubjectFlg === true) {

                                    angular.forEach(stusubj.subsubjectarray, function (ss) {
                                        $scope.subexamcolumns = [];

                                        var subsubjectnamenew1 = ss.subsubjectname;
                                        subsubjectnamenew1 = subsubjectnamenew1.replace(/[^a-zA-Z0-9]/g, "");

                                        angular.forEach(ss.subsubject_subexamarray, function (se) {
                                            $scope.subjectsubsubjsubexamksgrade = [];
                                            var subexamnamenew1 = se.subexamname;
                                            subexamnamenew1 = subexamnamenew1.replace(/[^a-zA-Z0-9]/g, "");

                                            angular.forEach($scope.savelistnew, function (stumarks) {
                                                if (stu.amsT_Id === stumarks.AMST_Id && stusubj.ISMS_Id === stumarks.ISMS_Id
                                                    && stumarks.EMSE_SubExamName === se.subexamname && stumarks.EMSS_SubSubjectName === ss.subsubjectname) {
                                                    if (se.marksdisplay === true && se.gradedisplay === true) {
                                                        stu["unique" + stusubj.ISMS_Id + subsubjectnamenew1 + subexamnamenew1 + 'M'] = stumarks.subsubjectmarks;
                                                        stu["unique" + stusubj.ISMS_Id + subsubjectnamenew1 + subexamnamenew1 + 'G'] = stumarks.subsubjectgrade;
                                                    }

                                                    else if (se.marksdisplay === true && se.gradedisplay === false) {
                                                        stu["unique" + stusubj.ISMS_Id + subsubjectnamenew1 + subexamnamenew1 + 'M'] = stumarks.subsubjectmarks;
                                                    }

                                                    else if (se.marksdisplay === false && se.gradedisplay === true) {
                                                        stu["unique" + stusubj.ISMS_Id + subsubjectnamenew1 + subexamnamenew1 + 'G'] = stumarks.subsubjectgrade;
                                                    }

                                                    else {
                                                        stu["unique" + stusubj.ISMS_Id + subsubjectnamenew1 + subexamnamenew1 + 'M'] = stumarks.subsubjectmarks;
                                                    }
                                                }
                                            });
                                        });
                                    });
                                }
                            });
                        });

                        //***********CREATING TOTAL MARKS ***************//
                        angular.forEach($scope.details, function (stud) {
                            angular.forEach($scope.exmrank, function (studrank) {
                                if (stud.amsT_Id === studrank.amsT_Id) {
                                    stud.estmP_TotalObtMarks = studrank.estmP_TotalObtMarks;
                                    stud.estmP_TotalGrade = studrank.estmP_TotalGrade;
                                    stud.estmP_TotalMaxMarks = studrank.estmP_TotalMaxMarks;
                                    stud.estmP_Percentage = studrank.estmP_Percentage;
                                    stud.estmP_SectionRank = studrank.estmP_SectionRank;
                                    stud.estmP_Result = studrank.estmP_Result;
                                }
                            });
                        });


                        console.log($scope.details);

                        $(document).ready(function () {
                            $('#gridhhs').empty();
                            $("#gridhhs").kendoGrid({
                                toolbar: ["excel"],
                                excel: {
                                    fileName: "Exam_Cumulative_Report.xlsx",
                                    //allPages: true,
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                excelExport: function (e) {
                                    var sheet = e.workbook.sheets[0];
                                    //sheet.frozenRows = 2;
                                    //sheet.mergedCells = ["A1:L1"];
                                    sheet.name = "Exam Cumulative Report";
                                },
                                dataSource: {
                                    data: $scope.details,
                                    pageSize: 100
                                },

                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,

                                columns: $scope.colarrayall,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize();
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-number");
                                        $(rowLabel).html(index);
                                    });
                                }
                            });
                        });

                    } else {
                        swal('No record Found');
                    }
                });
            }
        };

        $scope.cancel = function () {         
            $state.reload();
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.OnClickAll = function () {
            angular.forEach($scope.studentlistdetails, function (dd) {
                dd.checkedsub = $scope.all;
            });
        };

        $scope.individual = function () {
            $scope.all = $scope.studentlistdetails.every(function (itm) { return itm.checkedsub; });
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.studentlistdetails.some(function (options) {
                return options.checkedsub;
            });
        };
         
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };
    }
})();