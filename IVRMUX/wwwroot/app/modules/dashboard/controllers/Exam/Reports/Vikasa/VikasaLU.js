(function () {
    'use strict';
    angular.module('app').controller('VikasaLUController', VikasaLUController)

    VikasaLUController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function VikasaLUController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {
        $scope.reporttype = "indi";
        //$scope.report_hide = true;

        $scope.readmit = false;
        $scope.amsT_Date = new Date();
        $scope.BindData = function () {
            apiService.getDATA("VikasaLU/getdetails").then(function (promise) {

                $scope.acdlist = promise.acdlist;
                $scope.ctlist = promise.ctlist;
                $scope.seclist = promise.seclist;
                $scope.examlist = promise.examlist;
            });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("VikasaLU/onselectAcdYear", data).then(function (promise) {
                $scope.ctlist = promise.ctlist;
            });
        };

        $scope.onselectclass = function (ASMCL_Id, ASMAY_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("VikasaLU/onselectclass", data).then(function (promise) {
                $scope.seclist = promise.seclist;
            });
        };

        $scope.onselectSection = function () {
            var data = {
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("VikasaLU/onselectSection", data).then(function (promise) {
                $scope.examlist = promise.examlist;
                $scope.studentlist = promise.studentlist;
            });
        };

        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id
                };
                apiService.create("VikasaLU/onreport", data).then(function (promise) {
                    angular.forEach($scope.acdlist, function (y) {
                        if (y.asmaY_Id == $scope.ASMAY_Id) {
                            $scope.Year_Name = y.asmaY_Year;
                        }
                    });
                    angular.forEach($scope.ctlist, function (c) {
                        if (c.asmcL_Id == $scope.ASMCL_Id) {
                            $scope.Class_Name = c.asmcL_ClassName;
                        }
                    });
                    angular.forEach($scope.seclist, function (s) {
                        if (s.asmS_Id == $scope.ASMS_Id) {
                            $scope.Section_Name = s.asmC_SectionName;
                        }
                    });

                    angular.forEach($scope.examlist, function (s) {
                        if (s.emE_Id == $scope.EME_Id) {
                            $scope.exam = s.emE_ExamName;
                        }
                    });

                    if ($scope.reporttype === "indi") {
                        $scope.imagename = "/images/clients/Vikasa/HeaderMillerpuram.jpg";
                    } else {
                        $scope.imagename = "/images/clients/Vikasa/HeaderSawyerpuram.jpg";
                    }

                    $scope.classteacherarray = promise.classteacher;

                    $scope.classteacher = $scope.classteacherarray[0].hrmE_EmployeeFirstName;

                    $scope.attendance = promise.attendance;

                    $scope.studentlist22 = promise.studentlist;

                    $scope.sports = promise.sports;

                    $scope.date_m = new Date($scope.amsT_Date);
                    $scope.datareport = promise.datareport;
                    var student_list = [];
                    angular.forEach(promise.datareport, function (ore) {
                        var stu_id = ore.amsT_Id;
                        if (student_list.length == 0) {
                            var subs_list = [];
                            angular.forEach(promise.datareport, function (ore1) {
                                if (ore1.amsT_Id == stu_id) {

                                    if (ore1.overallgrade === "A") {
                                        ore1.color = 'Red';
                                    } else if (ore1.overallgrade === "E") {
                                        ore1.color = 'Darkgreen';
                                    } else if (ore1.overallgrade === "JE") {
                                        ore1.color = '#61ab58';
                                    } else if (ore1.overallgrade === "JM") {
                                        ore1.color = 'LIGHTBLUE';
                                    } else if (ore1.overallgrade === "M") {
                                        ore1.color = 'NAVY';
                                    } else if (ore1.overallgrade === "MM") {
                                        ore1.color = 'NAVY';
                                    }
                                    else {
                                        ore1.color = 'Black';
                                    }

                                    if (subs_list.length === 0) {
                                        var sub_subj_list = [];
                                        angular.forEach(promise.datareport, function (ore2) {

                                            if (ore2.estmpssS_ObtainedGrade === "A") {
                                                ore2.color = 'Red';
                                            } else if (ore2.estmpssS_ObtainedGrade === "E") {
                                                ore2.color = 'Darkgreen';
                                            } else if (ore2.estmpssS_ObtainedGrade === "JE") {
                                                ore2.color = '#61ab58';
                                            } else if (ore2.estmpssS_ObtainedGrade === "JM") {
                                                ore2.color = 'LIGHTBLUE';
                                            } else if (ore2.estmpssS_ObtainedGrade === "M") {
                                                ore2.color = 'NAVY';
                                            } else if (ore2.estmpssS_ObtainedGrade === "MM") {
                                                ore2.color = 'NAVY';
                                            }
                                            else {
                                                ore2.color = 'Black';
                                            }

                                            if (ore2.amsT_Id == stu_id && ore2.ismS_Id == ore1.ismS_Id) {
                                                if (sub_subj_list.length === 0) {
                                                    sub_subj_list.push(ore2);
                                                }
                                                else if (sub_subj_list.length > 0) {
                                                    var al_cnt1 = 0;
                                                    angular.forEach(sub_subj_list, function (ssub) {
                                                        if (ssub.ismS_Id == ore1.ismS_Id && ssub.amsT_Id == ore1.amsT_Id && ssub.emsS_Id == ore2.emsS_Id) {
                                                            al_cnt1 += 1;
                                                        }
                                                    });
                                                    if (al_cnt1 == 0) {
                                                        sub_subj_list.push(ore2);
                                                    }
                                                }
                                            }
                                        });

                                        subs_list.push({
                                            amsT_Id: ore1.amsT_Id, ismS_Id: ore1.ismS_Id, ismS_SubjectName: ore1.ismS_SubjectName, overallgrade: ore1.overallgrade, color: ore1.color, ssubjs: sub_subj_list
                                        });
                                    }
                                    else if (subs_list.length > 0) {
                                        var al_cnt = 0;
                                        angular.forEach(subs_list, function (sub) {
                                            if (sub.ismS_Id == ore1.ismS_Id && sub.amsT_Id == ore1.amsT_Id) {
                                                al_cnt += 1;
                                            }
                                        });
                                        if (al_cnt == 0) {
                                            var sub_subj_list = [];
                                            angular.forEach(promise.datareport, function (ore2) {
                                                if (ore2.amsT_Id == stu_id && ore2.ismS_Id == ore1.ismS_Id) {
                                                    if (sub_subj_list.length == 0) {
                                                        sub_subj_list.push(ore2);
                                                    }
                                                    else if (sub_subj_list.length > 0) {
                                                        var al_cnt1 = 0;
                                                        angular.forEach(sub_subj_list, function (ssub) {
                                                            if (ssub.ismS_Id == ore1.ismS_Id && ssub.amsT_Id == ore1.amsT_Id && ssub.emsS_Id == ore2.emsS_Id) {
                                                                al_cnt1 += 1;
                                                            }
                                                        });
                                                        if (al_cnt1 == 0) {
                                                            sub_subj_list.push(ore2);
                                                        }
                                                    }
                                                }
                                            });

                                            subs_list.push({
                                                amsT_Id: ore1.amsT_Id, ismS_Id: ore1.ismS_Id, ismS_SubjectName: ore1.ismS_SubjectName, overallgrade: ore1.overallgrade, color: ore1.color, ssubjs: sub_subj_list
                                            });
                                        }
                                    }

                                }
                            });
                            angular.forEach($scope.studentlist, function (s) {
                                if (s.amsT_Id == stu_id) {
                                    $scope.Student_Name = s.amsT_FirstName;
                                    $scope.Student_AdmNo = s.amsT_AdmNo;
                                    $scope.Student_RollNo = s.amaY_RollNo;
                                    $scope.Student_dob = s.amsT_DOB;
                                }
                            });

                            student_list.push({ amsT_Id: stu_id, Student_Name: $scope.Student_Name, Student_AdmNo: $scope.Student_AdmNo, Student_RollNo: $scope.Student_RollNo, Student_dob: $scope.Student_dob, subs: subs_list });
                        }
                        else if (student_list.length > 0) {
                            var alrdy_cnt = 0;
                            angular.forEach(student_list, function (st) {
                                if (st.amsT_Id == stu_id) {
                                    alrdy_cnt += 1;
                                }
                            })
                            if (alrdy_cnt === 0) {

                                var subs_list = [];
                                angular.forEach(promise.datareport, function (ore1) {
                                    if (ore1.amsT_Id == stu_id) {
                                        if (subs_list.length === 0) {
                                            var sub_subj_list = [];
                                            angular.forEach(promise.datareport, function (ore2) {
                                                if (ore2.amsT_Id == stu_id && ore2.ismS_Id == ore1.ismS_Id) {
                                                    if (sub_subj_list.length === 0) {
                                                        sub_subj_list.push(ore2);
                                                    }
                                                    else if (sub_subj_list.length > 0) {
                                                        var al_cnt1 = 0;
                                                        angular.forEach(sub_subj_list, function (ssub) {
                                                            if (ssub.ismS_Id == ore1.ismS_Id && ssub.amsT_Id == ore1.amsT_Id && ssub.emsS_Id == ore2.emsS_Id) {
                                                                al_cnt1 += 1;
                                                            }
                                                        })
                                                        if (al_cnt1 === 0) {
                                                            sub_subj_list.push(ore2);
                                                        }
                                                    }
                                                }
                                            });

                                            subs_list.push({
                                                amsT_Id: ore1.amsT_Id, ismS_Id: ore1.ismS_Id, ismS_SubjectName: ore1.ismS_SubjectName, overallgrade: ore1.overallgrade, color: ore1.color, ssubjs: sub_subj_list
                                            });
                                        }
                                        else if (subs_list.length > 0) {
                                            var al_cnt = 0;
                                            angular.forEach(subs_list, function (sub) {
                                                if (sub.ismS_Id == ore1.ismS_Id && sub.amsT_Id == ore1.amsT_Id) {
                                                    al_cnt += 1;
                                                }
                                            });
                                            if (al_cnt == 0) {
                                                var sub_subj_list = [];
                                                angular.forEach(promise.datareport, function (ore2) {
                                                    if (ore2.amsT_Id == stu_id && ore2.ismS_Id == ore1.ismS_Id) {
                                                        if (sub_subj_list.length === 0) {
                                                            sub_subj_list.push(ore2);
                                                        }
                                                        else if (sub_subj_list.length > 0) {
                                                            var al_cnt1 = 0;
                                                            angular.forEach(sub_subj_list, function (ssub) {
                                                                if (ssub.ismS_Id == ore1.ismS_Id && ssub.amsT_Id == ore1.amsT_Id && ssub.emsS_Id == ore2.emsS_Id) {
                                                                    al_cnt1 += 1;
                                                                }
                                                            });
                                                            if (al_cnt1 == 0) {
                                                                sub_subj_list.push(ore2);
                                                            }
                                                        }
                                                    }
                                                });

                                                subs_list.push({
                                                    amsT_Id: ore1.amsT_Id, ismS_Id: ore1.ismS_Id, ismS_SubjectName: ore1.ismS_SubjectName, overallgrade: ore1.overallgrade, color: ore1.color, ssubjs: sub_subj_list
                                                });
                                            }
                                        }

                                    }
                                });
                                angular.forEach($scope.studentlist, function (s) {
                                    if (s.amsT_Id == stu_id) {
                                        $scope.Student_Name = s.amsT_FirstName;
                                        $scope.Student_AdmNo = s.amsT_AdmNo;
                                        $scope.Student_RollNo = s.amaY_RollNo;
                                        $scope.Student_dob = s.amsT_DOB;
                                    }
                                });

                                student_list.push({ amsT_Id: stu_id, Student_Name: $scope.Student_Name, Student_AdmNo: $scope.Student_AdmNo, Student_RollNo: $scope.Student_RollNo, Student_dob: $scope.Student_dob, subs: subs_list });

                                //student_list.push({ amsT_Id: stu_id, subs: subs_list });
                            }
                        }

                    });

                    $scope.Main_list = student_list;
                });
            }
        };


        $scope.printData = function () {

            //var innerContents = document.getElementById("table").innerHTML;
            //var popupWinindow = window.open('');
            //popupWinindow.document.open();
            //popupWinindow.document.write('<html><head>' +
            //    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            //      '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
            //  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            //'</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            //popupWinindow.document.close();
            var innerContents = document.getElementById("VIKASAProgressCard").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/ProgressCardReport/LKGUKGProgressCardReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EME_Id = '';
            $scope.Main_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

    }

})();