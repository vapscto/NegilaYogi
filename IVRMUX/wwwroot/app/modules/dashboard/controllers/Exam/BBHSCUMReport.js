

(function () {
    'use strict';
    angular
        .module('app')
        .controller('BBHSCUMReportController', BBHSCUMReportController)

    BBHSCUMReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function BBHSCUMReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.exm_sub_mrks_list = [];
        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.HHS_I_IV_grid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.font_size = 'font25';
        $scope.size = '10px';
        $scope.printbtn = false;
        $scope.exclbtn = false;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("BBHSCUMReport/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;
                    $scope.clslist = promise.classlist;
                    $scope.seclist = promise.seclist;
                    $scope.amstlt = promise.amstlist;
                    $scope.studlist = promise.hhstudlist;
                    $scope.classarray = promise.classlist;
                    $scope.section = promise.seclist;
                    $scope.fillstudents = promise.amstlist;
                    $scope.fillstudents = promise.hhstudlist;
                    $scope.grade_list = promise.grade_list;
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

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "EMGR_Id": $scope.emgR_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("BBHSCUMReport/savedetails", data).
                    then(function (promise) {
                        $scope.HHS_I_IV_grid = true;
                        $scope.eam_sub_mrks_list = promise.eam_sub_mrks_list;
                        $scope.subjectslist = promise.subjectslist;
                        $scope.studlist = promise.studlist;

                        $scope.instname = promise.instname;
                        $scope.inst_name = $scope.instname[0].mI_Name;
                        $scope.add = $scope.instname[0].mI_Address1;
                        $scope.city = $scope.instname[0].ivrmmcT_Name;
                        $scope.pin = $scope.instname[0].mI_Pincode;
                        $scope.termlist = promise.termlist;

                        $scope.grade_detailslist = promise.grade_detailslist;
                        console.log($scope.grade_detailslist);
                        console.log($scope.eam_sub_mrks_list);
                        console.log($scope.studlist);

                        angular.forEach($scope.clslist, function (itm) {
                            if (itm.asmcL_Id == $scope.asmcL_Id) {
                                $scope.cla = itm.asmcL_ClassName;
                            }
                        });
                        angular.forEach($scope.yearlt, function (itm) {
                            if (itm.asmaY_Id == $scope.asmaY_Id) {
                                $scope.yr = itm.asmaY_Year;
                            }
                        });
                        angular.forEach($scope.seclist, function (itm) {
                            if (itm.asmS_Id == $scope.asmS_Id) {
                                $scope.sec = itm.asmC_SectionName;
                            }
                        });




                        if ($scope.eam_sub_mrks_list.length != 0) {
                            $scope.printbtn = true;
                            $scope.exclbtn = true;
                            $scope.attendencelist = promise.attendencelist;

                            $scope.selectedstudent = [];
                            $scope.selectedterm = [];
                            //test
                            //var a = 9;
                            //var b = 6;
                            //angular.forEach($scope.attendencelist, function (attt) {
                            //    attt.ASA_ClassHeld = a;
                            //    attt.ASA_Classpresent = b;
                            //    a += 2;
                            //    b += 1;
                            //})
                            //console.log($scope.attendencelist)


                            angular.forEach($scope.eam_sub_mrks_list, function (stu2) {
                                //student
                                if ($scope.selectedstudent.length == 0) {
                                    $scope.selectedstudent.push({ AMST_Id: stu2.AMST_Id, StudentName: stu2.StudentName, AMST_AdmNo: stu2.AMST_AdmNo, IMC_CasteName: stu2.IMC_CasteName });
                                }
                                else if ($scope.selectedstudent.length > 0) {
                                    var al_ct = 0;
                                    angular.forEach($scope.selectedstudent, function (uf) {
                                        if (uf.AMST_Id == stu2.AMST_Id) {
                                            al_ct += 1;
                                        }
                                    })
                                    if (al_ct == 0) {
                                        $scope.selectedstudent.push({ AMST_Id: stu2.AMST_Id, StudentName: stu2.StudentName, AMST_AdmNo: stu2.AMST_AdmNo, IMC_CasteName: stu2.IMC_CasteName });
                                    }
                                }
                                ///term
                                if ($scope.selectedterm.length == 0) {
                                    $scope.selectedterm.push({ ECT_Id: stu2.ECT_Id, ECT_TermName: stu2.ECT_TermName });
                                }
                                else if ($scope.selectedterm.length > 0) {
                                    var trc = 0;
                                    angular.forEach($scope.selectedterm, function (tf) {
                                        if (tf.ECT_Id == stu2.ECT_Id) {
                                            trc += 1;
                                        }
                                    });
                                    if (trc == 0) {
                                        $scope.selectedterm.push({ ECT_Id: stu2.ECT_Id, ECT_TermName: stu2.ECT_TermName });
                                    }
                                }
                            });

                            $scope.selectedgroup = [];
                            angular.forEach($scope.eam_sub_mrks_list, function (stu2) {

                                angular.forEach($scope.selectedterm, function (dd) {
                                    if (dd.ECT_Id == stu2.ECT_Id) {

                                        if ($scope.selectedgroup.length == 0) {
                                            $scope.selectedgroup.push({ ECT_Id: stu2.ECT_Id, EMPSG_Id: stu2.EMPSG_Id, EMPSG_GroupName: stu2.EMPSG_GroupName, EMPSG_PercentValue: stu2.EMPSG_PercentValue, displayname: stu2.ECT_Id });
                                        }
                                        else if ($scope.selectedgroup.length > 0) {
                                            var trc1 = 0;
                                            angular.forEach($scope.selectedgroup, function (tf) {
                                                if (tf.EMPSG_GroupName == stu2.EMPSG_GroupName) {
                                                    trc1 += 1;
                                                }
                                            })
                                            if (trc1 == 0) {
                                                $scope.selectedgroup.push({ ECT_Id: stu2.ECT_Id, EMPSG_Id: stu2.EMPSG_Id, EMPSG_GroupName: stu2.EMPSG_GroupName, EMPSG_PercentValue: stu2.EMPSG_PercentValue, displayname: stu2.ECT_Id });
                                            }
                                        }
                                    }
                                })
                            })






                            $scope.sublist = [];
                            angular.forEach($scope.eam_sub_mrks_list, function (stu2) {


                                if ($scope.sublist.length == 0) {
                                    $scope.sublist.push({ ISMS_Id: stu2.ISMS_Id, ISMS_SubjectName: stu2.ISMS_SubjectName, EMPS_AppToResultFlg: stu2.EMPS_AppToResultFlg });
                                }
                                else if ($scope.sublist.length > 0) {
                                    var trc11 = 0;
                                    angular.forEach($scope.sublist, function (ss) {
                                        if (ss.ISMS_Id == stu2.ISMS_Id) {
                                            trc11 += 1;
                                        }
                                    })
                                    if (trc11 == 0) {
                                        $scope.sublist.push({ ISMS_Id: stu2.ISMS_Id, ISMS_SubjectName: stu2.ISMS_SubjectName, EMPS_AppToResultFlg: stu2.EMPS_AppToResultFlg });
                                    }
                                }
                            }

                            )




                            $scope.selectedgroupwithtotal = [];
                            var ttper = 0;
                            angular.forEach($scope.selectedterm, function (tt) {
                                var per = 0;
                                angular.forEach($scope.selectedgroup, function (mm) {
                                    if (tt.ECT_Id == mm.ECT_Id) {
                                        per += mm.EMPSG_PercentValue;
                                        ttper += mm.EMPSG_PercentValue;
                                        $scope.selectedgroupwithtotal.push({ ECT_Id: mm.ECT_Id, EMPSG_Id: mm.EMPSG_Id, EMPSG_GroupName: mm.EMPSG_GroupName, EMPSG_PercentValue: mm.EMPSG_PercentValue, ECT_TermName: tt.ECT_TermName, displayname: mm.displayname })
                                    }

                                });

                                $scope.selectedgroupwithtotal.push({ ECT_Id: tt.ECT_Id, EMPSG_GroupName: 'Total', EMPSG_PercentValue: per, ECT_TermName: "", displayname: tt.ECT_Id });

                            });
                            $scope.selectedgroupwithtotal.push({ EMPSG_GroupName: 'FTotal', EMPSG_PercentValue: ttper, ECT_TermName: '', EMPSG_GroupName1: 'Total' });


                            $scope.selectedgroupwithattendance = [];
                            angular.forEach($scope.selectedstudent, function (ss) {
                                var grpwithatt = [];
                                var grpmatpertotal = 0;
                                var grpmatworktotal = 0;
                                var ttper = 0;
                                angular.forEach($scope.selectedterm, function (tr) {
                                    var termatpertotal = 0;
                                    var termatworktotal = 0;
                                    var per = 0;
                                    angular.forEach($scope.selectedgroupwithtotal, function (sg) {
                                        angular.forEach($scope.attendencelist, function (at) {
                                            if (ss.AMST_Id == at.AMST_Id && tr.ECT_Id == sg.ECT_Id && tr.ECT_Id == at.ECT_Id && sg.EMPSG_GroupName == at.EMPSG_GroupName) {
                                                per += sg.EMPSG_PercentValue;
                                                ttper += sg.EMPSG_PercentValue;
                                                termatpertotal += at.ASA_Classpresent;
                                                termatworktotal += at.ASA_ClassHeld;
                                                grpmatpertotal += at.ASA_Classpresent;
                                                grpmatworktotal += at.ASA_ClassHeld;
                                                grpwithatt.push({ AMST_Id: ss.AMST_Id, ECT_Id: at.ECT_Id, EMPSG_GroupName: at.EMPSG_GroupName, EMPSG_PercentValue: sg.EMPSG_PercentValue, ECT_TermName: sg.ECT_TermName, displayname: sg.displayname, ASA_ClassHeld: at.ASA_ClassHeld, ASA_Classpresent: at.ASA_Classpresent });
                                            }
                                        });
                                        //$scope.selectedgroupwithtotal.push({ ECT_Id: tt.ECT_Id, EMPSG_GroupName: 'Total', EMPSG_PercentValue: per, ECT_TermName: "", displayname: tt.ECT_Id }) 
                                    });

                                    grpwithatt.push({ AMST_Id: ss.AMST_Id, ECT_Id: tr.ECT_Id, EMPSG_GroupName: 'Total', EMPSG_PercentValue: per, ECT_TermName: '', displayname: tr.ECT_Id, ASA_ClassHeld: termatworktotal, ASA_Classpresent: termatpertotal });
                                });

                                grpwithatt.push({ AMST_Id: ss.AMST_Id, EMPSG_GroupName: 'FTotal', EMPSG_PercentValue: ttper, ECT_TermName: '', ASA_ClassHeld: grpmatworktotal, ASA_Classpresent: grpmatpertotal, EMPSG_GroupName1: 'Total' });

                                $scope.selectedgroupwithattendance.push({ AMST_Id: ss.AMST_Id, grpwithatt: grpwithatt });
                            });



                            console.log($scope.selectedgroupwithattendance);
                            console.log($scope.selectedgroupwithtotal);



                            $scope.stdmarkslist = [];
                            angular.forEach($scope.selectedstudent, function (oo) {

                                $scope.markslist = [];
                                angular.forEach($scope.sublist, function (zz) {
                                    var finaltotalmrks = 0;
                                    var finalmaxtotalmrks = 0;
                                    angular.forEach($scope.selectedterm, function (xx) {
                                        var totalmrks = 0;
                                        var maxtotalmrks = 0;
                                        angular.forEach($scope.eam_sub_mrks_list, function (pp) {

                                            if (oo.AMST_Id == pp.AMST_Id) {



                                                if (xx.ECT_Id == pp.ECT_Id) {



                                                    if (zz.ISMS_Id == pp.ISMS_Id) {

                                                        finaltotalmrks += pp.ESTMPPSG_GroupObtMarks;
                                                        finalmaxtotalmrks += pp.ESTMPPSG_GroupMaxMarks;
                                                        totalmrks += pp.ESTMPPSG_GroupObtMarks;
                                                        maxtotalmrks += pp.ESTMPPSG_GroupMaxMarks;
                                                        if (pp.ESTMPPSG_GroupObtGrade == 0) {
                                                            pp.ESTMPPSG_GroupObtGrade = '';
                                                        }


                                                        $scope.markslist.push({ AMST_Id: pp.AMST_Id, ECT_Id: pp.ECT_Id, EMPSG_GroupName: pp.EMPSG_GroupName, ISMS_Id: pp.ISMS_Id, ISMS_SubjectName: pp.ISMS_SubjectName, ESTMPPSG_GroupMaxMarks: pp.ESTMPPSG_GroupMaxMarks, ESTMPPSG_GroupObtMarks: pp.ESTMPPSG_GroupObtMarks, ESTMPPSG_GroupObtGrade: pp.ESTMPPSG_GroupObtGrade, EMPS_AppToResultFlg: pp.EMPS_AppToResultFlg, displayname: pp.displayname })

                                                    }





                                                }







                                            }



                                        })


                                        var grade1 = '';
                                        angular.forEach($scope.grade_detailslist, function (grd1) {
                                            if (totalmrks >= grd1.emgD_From && totalmrks <= grd1.emgD_To) {
                                                grade1 = grd1.emgD_Name;
                                            }
                                        })


                                        $scope.markslist.push({ AMST_Id: oo.AMST_Id, ECT_Id: xx.ECT_Id, EMPSG_GroupName: 'Total', ISMS_Id: zz.ISMS_Id, ISMS_SubjectName: zz.ISMS_SubjectName, ESTMPPSG_GroupMaxMarks: maxtotalmrks, ESTMPPSG_GroupObtMarks: totalmrks, ESTMPPSG_GroupObtGrade: grade1, EMPS_AppToResultFlg: zz.EMPS_AppToResultFlg, displayname: xx.ECT_Id })

                                    })

                                    var grade = '';
                                    angular.forEach($scope.grade_detailslist, function (grd) {
                                        if (finaltotalmrks >= grd.emgD_From && finaltotalmrks <= grd.emgD_To) {
                                            grade = grd.emgD_Name;
                                        }
                                    })



                                    $scope.markslist.push({ AMST_Id: oo.AMST_Id, ECT_Id: '', EMPSG_GroupName: 'FTotal', ISMS_Id: zz.ISMS_Id, ISMS_SubjectName: zz.ISMS_SubjectName, ESTMPPSG_GroupMaxMarks: finalmaxtotalmrks, ESTMPPSG_GroupObtMarks: finaltotalmrks, ESTMPPSG_GroupObtGrade: grade, EMPSG_GroupName1: 'Total', EMPS_AppToResultFlg: zz.EMPS_AppToResultFlg })



                                })

                                angular.forEach($scope.selectedgroupwithtotal, function (gp) {
                                    var exmtottalmarks = 0;
                                    var exmtottalmarksmax = 0;
                                    angular.forEach($scope.markslist, function (mgp) {
                                        if (gp.EMPSG_GroupName == mgp.EMPSG_GroupName && oo.AMST_Id == mgp.AMST_Id && mgp.EMPS_AppToResultFlg == true && mgp.EMPSG_GroupName != 'Total') {
                                            exmtottalmarks += mgp.ESTMPPSG_GroupObtMarks;
                                            exmtottalmarksmax += mgp.ESTMPPSG_GroupMaxMarks;
                                        }
                                        if (gp.EMPSG_GroupName == mgp.EMPSG_GroupName && oo.AMST_Id == mgp.AMST_Id && mgp.EMPS_AppToResultFlg == true && mgp.EMPSG_GroupName == 'Total') {
                                            if (gp.displayname == mgp.displayname) {
                                                exmtottalmarks += mgp.ESTMPPSG_GroupObtMarks;
                                                exmtottalmarksmax += mgp.ESTMPPSG_GroupMaxMarks;
                                            }


                                        }
                                    })
                                    $scope.markslist.push({ AMST_Id: oo.AMST_Id, ECT_Id: '', EMPSG_GroupName: gp.EMPSG_GroupName, ISMS_Id: 1234, ISMS_SubjectName: 'subttl', ESTMPPSG_GroupMaxMarks: exmtottalmarksmax, ESTMPPSG_GroupObtMarks: exmtottalmarks, displayname: gp.displayname })

                                })
                                $scope.stdmarkslist.push({ AMST_Id: oo.AMST_Id, studentmarklist: $scope.markslist })
                            })


                            $scope.mainlist = [];

                            angular.forEach($scope.selectedstudent, function (ff) {
                                angular.forEach($scope.selectedgroupwithattendance, function (vv) {
                                    if (ff.AMST_Id == vv.AMST_Id) {


                                        $scope.std = [];
                                        angular.forEach($scope.stdmarkslist, function (bb) {

                                            if (ff.AMST_Id == bb.AMST_Id) {
                                                $scope.std.push({ AMST_Id: ff.AMST_Id, StudentName: ff.StudentName, AMST_AdmNo: ff.AMST_AdmNo, IMC_CasteName: ff.IMC_CasteName });
                                                $scope.mainlist.push({ AMST_Id: bb.AMST_Id, studdetails: $scope.std, marks: bb.studentmarklist, terms: $scope.selectedterm, group: vv.grpwithatt })
                                            }

                                        })

                                    }
                                })

                            })

                            console.log($scope.mainlist)

                            console.log($scope.markslist)
                            console.log($scope.sublist)
                            console.log($scope.selectedgroup)

                            console.log($scope.selectedgroupwithtotal)
                            console.log($scope.selectedstudent)

                            console.log($scope.selectedterm)
                        }
                        else {
                            $scope.HHS_I_IV_grid = false;
                            swal("No Record found....")
                        }


                    })
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

            $scope.exportToExcel = function (tableIds) {

                var exportHref = Excel.tableToExcel(tableIds, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);

                //var blob = new Blob([document.getElementById(tableIds).outerHTML], {
                //    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
                //});
                //saveAs(blob, "Cumulative Report of " + $scope.cla - $scope.sec + ".xls");


            }


            $scope.printToCart = function () {
                var innerContents = document.getElementById("Baldwin").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/balabarathi/CumulativeReportbalPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            //to print


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

        $scope.OnAcdyear = function (asmaY_Id) {

            $scope.asmcL_Id = '';
            $scope.asmS_Id = '';

            $scope.amstid = '';
            $scope.fillstudents = [];
            $scope.section = [];
            $scope.classarray = [];
            apiService.getURI("BBHSCUMReport/getclass", asmaY_Id).
                then(function (promise) {
                    $scope.classarray = promise.fillclass;
                    // console.log($scope.classarray);
                })


        }



        $scope.OnClass = function (asmcL_Id) {

            $scope.asmS_Id = '';

            $scope.amstid = '';
            $scope.asmcL_Id = asmcL_Id;
            // alert(asmaY_Id)
            $scope.section = [];
            $scope.fillstudents = [];
            var data = {
                "ASMCL_Id": asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("BBHSCUMReport/Getsection", data).
                then(function (promise) {

                    //  
                    $scope.section = promise.fillsection;



                })


        }
        $scope.OnSection = function (asmS_Id) {


            $scope.amstid = '';
            $scope.asmS_Id = asmS_Id;

            $scope.fillstudents = [];
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": asmS_Id,

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("BBHSCUMReport/GetAttendence", data).
                then(function (promise) {

                    //

                    // $scope.indattendance = true;
                    $scope.fillstudents = promise.fillstudents;





                })


        }
    }
})();