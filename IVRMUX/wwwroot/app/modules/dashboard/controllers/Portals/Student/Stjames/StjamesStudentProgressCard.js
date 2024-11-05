(function () {
    'use strict';
    angular.module('app').controller('StjamesStudentProgressCardController', StjamesStudentProgressCardController)

    StjamesStudentProgressCardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$compile']
    function StjamesStudentProgressCardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $compile) {

        $scope.percounttotal = 0;
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.HHS_I_IV_grid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.grandfinaltotal = 0;
        $scope.grandfinalmaxtotal = 0;
        $scope.grandtotalperc = 0;
        //TO  GEt The Values iN Grid
        $scope.grandavgtotal = 0;
        $scope.exm_sub_mrks_list = [];

        $scope.halfyearatt = [];
        $scope.fullyearatt = [];


        $scope.BindData = function () {
            var pageid = 2;

            apiService.getURI("StudentProgressCardReport/stmarygetdetails", pageid).then(function (promise) {
                $scope.btn = false;
                $scope.getstudentdetails = promise.getstudentdetails;
                $scope.year_list = promise.getyear;
                $scope.classlist = promise.getclass;
                $scope.examorterm = promise.examorterm;

                if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                    $scope.ASMCL_Id = $scope.getstudentdetails[0].asmcL_Id;
                    $scope.asmS_Id = $scope.getstudentdetails[0].asmS_Id;
                    $scope.asmaY_Id = $scope.getstudentdetails[0].asmaY_Id;
                    $scope.getexamtermlist = [];

                    if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                        $scope.getexamtermlist = promise.getexamtermlist;
                        if ($scope.getexamtermlist === null || $scope.getexamtermlist.length === 0) {
                            swal("Marks Not Published");
                        }
                        $scope.examflag = false;
                        $scope.termflag = false;
                    }
                    else {
                        $scope.btn = true;
                        swal("Report Format Not Mapped Contact Administrator");
                    }
                } else {
                    $scope.btn = true;
                    swal("No Studnet Data Found");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.getexamtermlist = [];
            $scope.obj.ECT_Id = "";
            $scope.obj.EME_Id = "";
            $scope.EME_Id = "";
            $scope.ECT_Id = "";
            $scope.examflag = false;
            $scope.HHS_I_IV_grid = false;
            $scope.termflag = false;
            $scope.reportshowcount = 0;
            $scope.btn = false;
            if ($scope.ASMCL_Id !== undefined && $scope.ASMCL_Id !== null && $scope.ASMCL_Id !== "") {
                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id
                };

                apiService.create("StudentProgressCardReport/stmaryonchangeclass", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getstudentdetails = promise.getstudentdetails;

                        if ($scope.getstudentdetails !== null && $scope.getstudentdetails.length > 0) {

                            $scope.examorterm = promise.examorterm;

                            if ($scope.examorterm === 'Exam' || $scope.examorterm === 'Promotion') {
                                if ($scope.getexamtermlist === null && $scope.getexamtermlist.length === 0) {
                                    swal("Marks Not Published");
                                }
                                $scope.getexamtermlist = promise.getexamtermlist;
                                $scope.examflag = false;
                                $scope.termflag = false;
                            }
                            else {
                                $scope.btn = true;
                                swal("Report Format Not Mapped Contact Administrator");
                            }
                        } else {
                            $scope.btn = true;
                            swal("No Studnet Data Found");
                        }
                    }
                });
            }
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;

        $scope.Get_Stjames_Progresscard_Report = function () {
            $scope.HHS_I_IV_grid = false;
            $scope.submitted = true;
            $scope.getstudentmarksdetails_temp = [];
            $scope.getstudentmarksdetails = [];
            $scope.getstudentwisesubjectlist = [];
            $scope.getstudentdetails = [];
            $scope.getexamsubjectwisemarksdetails = [];
            $scope.getexamwisetotaldetails = [];


            if ($scope.myForm.$valid) {
                $scope.grandavgtotal = 0;

                var data = {
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "EME_Id": $scope.EME_Id
                };
                apiService.create("StudentProgressCardReport/Get_Stjames_Progresscard_Report", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.spflag = promise.spflag;
                        $scope.examflag = promise.examflag;

                        if ($scope.spflag === "1") {
                            if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {

                                $scope.HHS_I_IV_grid = true;

                                $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                                $scope.getstudentdetails = promise.getstudentdetails;
                                $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlist;
                                $scope.getexamwisesubsubjectlist = promise.getexamwisesubsubjectlist;
                                $scope.getstudentwiseattendancedetails = promise.getstudentwiseattendancedetails;

                                $scope.examwiseremarks = promise.examwiseremarks;
                                $scope.studentwisemarks = promise.studentwisemarks;

                                // STUDENT WISE SUBJECT
                                angular.forEach($scope.getstudentdetails, function (dd) {
                                    $scope.subjecttemplist = [];
                                    angular.forEach($scope.getstudentwisesubjectlist, function (d) {
                                        if (dd.AMST_Id === d.AMST_Id) {
                                            $scope.subjecttemplist.push({
                                                ISMS_Id: d.ISMS_Id, ISMS_SubjectName: d.ISMS_SubjectName, EYCES_SubjectOrder: d.EYCES_SubjectOrder,
                                                EYCES_AplResultFlg: d.EYCES_AplResultFlg, EYCES_SubSubjectFlg: d.EYCES_SubSubjectFlg, EYCES_SubExamFlg: d.EYCES_SubExamFlg
                                            });
                                        }
                                    });
                                    dd.subjectlist = $scope.subjecttemplist;
                                });

                                // STUDENT WISE SUBJECT AND SUB SUBJECT
                                if ($scope.getexamwisesubsubjectlist !== null && $scope.getexamwisesubsubjectlist.length > 0) {
                                    angular.forEach($scope.getstudentdetails, function (dd) {
                                        $scope.subsubjecttemp = [];
                                        angular.forEach(dd.subjectlist, function (d) {
                                            $scope.subsubjecttemp = [];
                                            angular.forEach($scope.getexamwisesubsubjectlist, function (ddd) {
                                                if (d.ISMS_Id === ddd.ISMS_Id) {
                                                    $scope.subsubjecttemp.push({
                                                        ISMS_Id: ddd.ISMS_Id, EMSS_Id: ddd.ssubj, EMSS_SubSubjectName: ddd.EMSS_SubSubjectName,
                                                        EMSS_Order: ddd.EMSS_Order
                                                    });
                                                }
                                            });
                                            d.subsubjectlist = $scope.subsubjecttemp;
                                        });
                                    });
                                }                               

                                // STUDENT WISE MARKS
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.studentmarks = [];
                                    angular.forEach($scope.getstudentmarksdetails, function (stumrks) {
                                        if (stu.AMST_Id === stumrks.AMST_Id) {
                                            $scope.studentmarks.push(stumrks);
                                        }
                                    });
                                    stu.stdmarks = $scope.studentmarks;
                                });


                                // STUDENT WISE MARKS
                                if ($scope.getexamwisesubsubjectlist !== null && $scope.getexamwisesubsubjectlist.length > 0) {
                                    angular.forEach($scope.getstudentdetails, function (d) {
                                        $scope.tempmarks = [];
                                        angular.forEach(d.subjectlist, function (dd) {
                                            $scope.tempmarks = [];
                                            angular.forEach(dd.subsubjectlist, function (ddd) {
                                                $scope.tempmarks = [];
                                                angular.forEach($scope.getstudentmarksdetails, function (dddd) {
                                                    if (d.AMST_Id === dddd.AMST_Id && dd.ISMS_Id === parseInt(dddd.ISMS_Id) && ddd.EMSS_Id === parseInt(dddd.EMSS_Id)) {
                                                        $scope.tempmarks.push({
                                                            obtainmarks: dddd.obtainmarks, maxmarks: dddd.maxmarks, ObtainedGrade: dddd.Grade,
                                                            PassFailFlg: dddd.PassFailFlg, Marksdispaly: dddd.Marksdispaly, Gradedisplay: dddd.Gradedisplay,
                                                            Sectionaverage: dddd.Sectionaverage, flag: dddd.flag, GRADEREMAKS: dddd.GRADEREMAKS
                                                        });
                                                    }
                                                });
                                                ddd.studentmarks = $scope.tempmarks;
                                            });
                                        });
                                    });
                                }

                                //STUDNET WISE REMARKS
                                if ($scope.examwiseremarks !== null && $scope.examwiseremarks.length > 0) {
                                    angular.forEach($scope.getstudentdetails, function (d) {
                                        angular.forEach($scope.examwiseremarks, function (dd) {
                                            if (d.AMST_Id === dd.amsT_Id) {
                                                d.remaks = dd.emeR_Remarks;
                                            }
                                        });
                                    });
                                }

                                angular.forEach($scope.getstudentdetails, function (dd) {
                                    $scope.subjecttemplist = [];
                                    angular.forEach($scope.studentwisemarks, function (d) {
                                        if (dd.AMST_Id === d.AMST_Id) {
                                            dd.ESTMP_TotalMaxMarks = d.estmP_TotalMaxMarks;
                                            dd.ESTMP_TotalConverionMaxMarks = d.estmP_TotalConverionMaxMarks;

                                            dd.ESTMP_TotalObtMarks = d.estmP_TotalObtMarks;
                                            dd.ESTMP_TotalConvertedMarks = d.estmP_TotalConvertedMarks;

                                            dd.ESTMP_TotalGrade = d.estmP_TotalGrade;
                                            dd.ESTMP_GradePoints = d.estmP_GradePoints;
                                        }
                                    });
                                });

                                console.log($scope.getstudentdetails);
                            } else {
                                swal("No Records Found");
                            }
                        }

                        else if ($scope.spflag === "2") {
                            if (promise.getstudentmarksdetails !== null && promise.getstudentmarksdetails.length > 0) {
                                $scope.HHS_I_IV_grid = true;
                                $scope.getstudentmarksdetails = promise.getstudentmarksdetails;
                                $scope.getstudentwisesubjectlist = promise.getstudentwisesubjectlistnew;                               
                                $scope.getstudentdetails = promise.getstudentdetails;
                                $scope.getexamdetails = promise.getexamdetails;                                 
                                $scope.getexamwisetotaldetails = promise.studentwisemarks;

                                $scope.tempexamdetails = [];
                                if ($scope.examflag === 1) {
                                    angular.forEach($scope.getexamdetails, function (dd) {
                                        $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Paper" });
                                        $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Subject" });
                                    });
                                } else if ($scope.examflag === 2) {
                                    angular.forEach($scope.getexamdetails, function (dd) {
                                        $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Marks", emE_ExamCode: dd.emE_ExamCode });
                                        $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Point", emE_ExamCode: dd.emE_ExamCode });
                                    });
                                }
                                else if ($scope.examflag === 0) {
                                    angular.forEach($scope.getexamdetails, function (dd) {
                                        $scope.tempexamdetails.push({ EME_Id: dd.emE_Id, columnid: dd.emE_Id, columname: "Marks", emE_ExamCode: dd.emE_ExamCode });
                                    });
                                }

                                $scope.subjectdetails = [];
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.subjectdetails = [];
                                    angular.forEach($scope.getstudentwisesubjectlist, function (sub) {
                                        if (stu.AMST_Id === parseInt(sub.AMST_Id)) {
                                            $scope.subjectdetails.push({
                                                ISMS_SubjectName: sub.subjectname, ISMS_Id: sub.subid,
                                                EYCES_AplResultFlg: sub.EYCES_AplResultFlg, subjectorder: sub.subjectorder
                                            });
                                        }
                                    });
                                    stu.subjects = $scope.subjectdetails;
                                });

                                $scope.marksdetails = [];
                                angular.forEach($scope.getstudentdetails, function (stu) {
                                    $scope.marksdetails = [];
                                    angular.forEach($scope.getstudentmarksdetails, function (subd) {
                                        if (stu.AMST_Id === parseInt(subd.AMST_Id)) {
                                            $scope.marksdetails.push(subd);
                                        }
                                    });
                                    stu.marks = $scope.marksdetails;
                                });

                                // Over All Total                           
                                $scope.totalgrade = [];
                                angular.forEach($scope.getstudentdetails, function (st) {
                                    $scope.totalgrade = [];
                                    angular.forEach($scope.getexamwisetotaldetails, function (su) {
                                        if (parseInt(st.AMST_Id) === parseInt(su.amsT_Id)) {
                                            $scope.totalgrade.push(su);
                                        }
                                    });
                                    st.markstotal = $scope.totalgrade;
                                });


                                console.log($scope.getstudentdetails);
                            } else {
                                swal("No Records Found");
                            }
                        }

                        $scope.examwiseremarks = [];
                        if (promise.examwiseremarks !== null && promise.examwiseremarks.length > 0) {
                            $scope.examwiseremarks = promise.examwiseremarks;
                            $scope.promotionremarks = $scope.examwiseremarks[0].emeR_Remarks;
                        }


                        var e1 = angular.element(document.getElementById("report"));
                        $compile(e1.html(promise.htmlstring))(($scope));

                        angular.forEach($scope.year_list, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.year = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.class_list, function (dd) {
                            if (dd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                $scope.cla = dd.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.section_list, function (dd) {
                            if (dd.asmS_Id === parseInt($scope.ASMS_Id)) {
                                $scope.sec = dd.asmC_SectionName;
                            }
                        });

                        angular.forEach($scope.getexamlist, function (dd) {
                            if (dd.emE_Id === parseInt($scope.EME_Id)) {
                                $scope.exam = dd.emE_ExamName;
                            }
                        });
                    }
                });
            }
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.subjectlt1, function (itm) {
                itm.xyz = toggleStatus;
            });
        };

        $scope.optionToggled = function (chk_box) {
            $scope.all = $scope.subjectlt1.every(function (itm) { return itm.xyz; });
        };

        //to print
        $scope.printToCart = function () {
            var innerContents = "";
            var popupWinindow = "";

            if ($scope.spflag === "1") {
                innerContents = document.getElementById("Baldwin").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/Stjames/Stjamesprogresscardpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            } else if ($scope.spflag === "2") {
                innerContents = document.getElementById("Baldwin4").innerHTML;
                popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="/css/print/StJames/progresscardpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
        };

        $scope.export = function () {
            html2canvas(document.getElementById('Baldwin'), {
                onrendered: function (canvas) {
                    var data = canvas.toDataURL();
                    var docDefinition = {
                        content: [{
                            image: data,
                            width: 500,
                        }]
                    };
                    pdfMake.createPdf(docDefinition).download("test.pdf");
                }
            });
        }
    }
})();
