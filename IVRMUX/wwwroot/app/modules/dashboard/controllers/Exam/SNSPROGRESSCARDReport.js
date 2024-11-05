

(function () {
    'use strict';
    angular
.module('app')
.controller('SNSPROGRESSCARDReportController', SNSPROGRESSCARDReportController)

    SNSPROGRESSCARDReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter']
    function SNSPROGRESSCARDReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.HHS_I_IV_grid = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;
        $scope.halfyearatt = [];
        $scope.fullyearatt = [];
        $scope.exm_sub_mrks_list = [];
        //TO  GEt The Values iN Grid
        $scope.finalexam = [];
        $scope.BindData = function () {
            apiService.getDATA("SNSPROGRESSCARDReport/Getdetails").
       then(function (promise) {
           
           $scope.yearlt = promise.yearlist;
           $scope.clslist = promise.classlist;
           $scope.seclist = promise.seclist;
           $scope.amstlt = promise.amstlist;
           $scope.studlist = promise.hhstudlist;
           $scope.grade_list = promise.grade_list;
       



       //    console.log($scope.grade_list);
       })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //-----------academicyear Selection
        $scope.onyearchange = function () {
            
            $scope.asmcL_Id = '';
            $scope.asmS_Id = '';
            $scope.clslist = [];
            $scope.seclist = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("SNSPROGRESSCARDReport/yearchange", data).
               then(function (promise) {
                   $scope.clslist = promise.classlist;
               })
        }

        //-----------class Selection
        $scope.onclasschange = function () {
            
           
            $scope.asmS_Id = '';
            $scope.seclist = [];
            var data = {
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMAY_Id": $scope.asmaY_Id,
            }
            apiService.create("SNSPROGRESSCARDReport/classchange", data).
               then(function (promise) {
                   $scope.seclist = promise.seclist;
               })
        }

        //-----------section Selection
        $scope.onsectionchange = function () {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
            }
            apiService.create("SNSPROGRESSCARDReport/sectionchange", data).
               then(function (promise) {
                   $scope.studentlist = promise.studentlist;
               })
        }


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id
                   
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SNSPROGRESSCARDReport/savedetails", data).

                   then(function (promise) {
                       
                      
                       if (promise.savelist != null) {
                           $scope.exmstdlist = promise.exmstdlist;
                           $scope.HHS_I_IV_grid = true;
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
                           //angular.forEach($scope.exsplt, function (itm) {
                           //    if (itm.emE_Id == $scope.emE_Id) {
                           //        $scope.exmmid = itm.emE_ExamName;
                           //    }
                           //})

                           $scope.printbtn = true;
                           $scope.exclbtn = true;
                           //inst name binding
                           $scope.instname = promise.instname;
                           $scope.inst_name = $scope.instname[0].mI_Name;
                           $scope.add = $scope.instname[0].mI_Address1;
                           $scope.city = $scope.instname[0].ivrmmcT_Name;
                           $scope.pin = $scope.instname[0].mI_Pincode;
                           $scope.finalexam = promise.finalexam;
                           if ($scope.finalexam.length > 0) {
                               $scope.finalexid = $scope.finalexam[0].emE_Id;
                           }

                           $scope.studlist = promise.studlist;
                           $scope.savelist = promise.savelist;
                           $scope.subwithsubexm = promise.subwithsubexm;
                           $scope.electivemarks = promise.electivemarks;
                           $scope.electivestd = promise.savenonsubjlist;
                           $scope.electivesub = promise.nonsubjlist;
                           $scope.exmrank = promise.exmrank;
                           $scope.attdetails = promise.attdetails;

                           $scope.details = [];
                           angular.forEach($scope.studlist, function (std) {
                               var mrklist = [];
                               var ellist = [];

                               angular.forEach($scope.attdetails, function (at) {
                                   if (at.amsT_Id == std.amsT_Id) {
                                       std.attendance = at.attper;
                                   }
                               })


                               angular.forEach($scope.exmrank, function (ww) {


                                   if (ww.amsT_Id == std.amsT_Id && ww.emE_Id == $scope.finalexid) {
                                       
                                     std.annualresult=ww.estmP_Result

                                   }
                               })
                                

                               angular.forEach($scope.savelist, function (mrk) {

                                   if (mrk.amsT_Id==std.amsT_Id) {
                                       mrklist.push({ emE_Id: mrk.emE_Id, ismS_Id: mrk.ismS_Id, emsE_Id: mrk.emsE_Id, emsE_SubExamName: mrk.emsE_SubExamName, estmpS_ObtainedMarks: mrk.estmpS_ObtainedMarks, estmpS_MaxMarks: mrk.estmpS_MaxMarks, estmpS_PassFailFlg: mrk.estmpS_PassFailFlg, flag: true })
                                   }

                               })
                               angular.forEach($scope.electivemarks, function (el) {

                                   if (el.amsT_Id == std.amsT_Id) {
                                       ellist.push({ emE_Id: el.emE_Id, ismS_Id: el.ismS_Id, emsE_Id: el.emsE_Id, estmpS_ObtainedMarks: el.estmpS_ObtainedMarks, estmpS_MaxMarks: el.estmpS_MaxMarks, estmpS_PassFailFlg: el.estmpS_PassFailFlg, flag: false, estmpS_ObtainedGrade: el.estmpS_ObtainedGrade })
                                   }

                               })


                               var totalmrklist = [];
                               angular.forEach($scope.exmrank, function (rr) {
                               
                                 
                                   if (rr.amsT_Id == std.amsT_Id) {
                                       totalmrklist.push({ emE_Id: rr.emE_Id, amsT_Id: rr.amsT_Id, estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, estmP_Percentage: rr.estmP_Percentage, estmP_SectionRank: rr.estmP_SectionRank, estmP_Result: rr.estmP_Result, estmP_TotalGrade: rr.estmP_TotalGrade })

                                   }
                               })


                               $scope.details.push({ amsT_Id: std.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_FatherName: std.amsT_FatherName, amsT_AdmNo: std.amsT_AdmNo, amaY_RollNo: std.amaY_RollNo, stmrklist: mrklist, elmrklist: ellist, totalmrklist: totalmrklist, attt: std.attendance, annualresult: std.annualresult })

                           })








                           //$scope.details = [];
                           //angular.forEach($scope.studlist, function (std) {
                           //    var mrklist = [];
                           //    var ellist = [];

                           //    angular.forEach($scope.subwithsubexm, function (exm) {
                           //        if (exm.eyceS_AplResultFlg == true) {
                           //            var obtn = '';
                           //            var max = '';
                           //            var fff = '';
                           //            var cnttt = 0;
                           //            angular.forEach($scope.savelist, function (mrk) {




                           //                if (mrk.amsT_Id == std.amsT_Id && exm.ismS_Id == mrk.ismS_Id && exm.emsE_Id == mrk.emsE_Id) {
                           //                    cnttt += 1;
                           //                    obtn = mrk.estmpS_ObtainedMarks;
                           //                    max = mrk.estmpS_MaxMarks;
                           //                    fff = mrk.estmpS_PassFailFlg;
                           //                }

                           //            })
                           //            if (cnttt > 0) {
                           //                mrklist.push({ ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName, estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true })
                           //            }
                           //            if (cnttt == 0) {
                           //                mrklist.push({ ismS_Id: exm.ismS_Id, emsE_Id: exm.emsE_Id, emsE_SubExamName: exm.emsE_SubExamName, estmpS_ObtainedMarks: obtn, estmpS_MaxMarks: max, estmpS_PassFailFlg: fff, flag: true })
                           //            }
                           //        }

                           //    })

                           //    angular.forEach($scope.electivemarks, function (el) {

                           //        if (el.amsT_Id == std.amsT_Id) {
                           //            ellist.push({ ismS_Id: el.ismS_Id, emsE_Id: el.emsE_Id, estmpS_ObtainedMarks: el.estmpS_ObtainedMarks, estmpS_MaxMarks: el.estmpS_MaxMarks, estmpS_PassFailFlg: el.estmpS_PassFailFlg, flag: false })
                           //        }

                           //    })


                           //    //$scope.details.push({ amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo, amaY_RollNo: std.amaY_RollNo, stmrklist: mrklist, elmrklist: ellist })

                           //    angular.forEach($scope.exmrank, function (rr) {

                           //        if (rr.amsT_Id == std.amsT_Id) {
                           //            $scope.details.push({ amsT_Id: rr.amsT_Id, amsT_FirstName: std.amsT_FirstName, amsT_AdmNo: std.amsT_AdmNo, amaY_RollNo: std.amaY_RollNo, stmrklist: mrklist, elmrklist: ellist, estmP_TotalObtMarks: rr.estmP_TotalObtMarks, estmP_TotalMaxMarks: rr.estmP_TotalMaxMarks, estmP_Percentage: rr.estmP_Percentage, estmP_SectionRank: rr.estmP_SectionRank, estmP_Result: rr.estmP_Result })

                           //        }
                           //    })

                           //})









                           console.log($scope.details)

                           //getting all subjects
                           $scope.mainsubjects = [];

                           angular.forEach($scope.subwithsubexm, function (stu2) {

                               if ($scope.mainsubjects.length == 0) {
                                   $scope.mainsubjects.push({ ismS_Id: stu2.ismS_Id, ismS_SubjectName: stu2.ismS_SubjectName, eyceS_AplResultFlg: stu2.eyceS_AplResultFlg });
                               }
                               else if ($scope.mainsubjects.length > 0) {
                                   var al_ct = 0;
                                   angular.forEach($scope.mainsubjects, function (uf) {
                                       if (uf.ismS_Id == stu2.ismS_Id) {
                                           al_ct += 1;
                                       }
                                   })
                                   if (al_ct == 0) {
                                       $scope.mainsubjects.push({ ismS_Id: stu2.ismS_Id, ismS_SubjectName: stu2.ismS_SubjectName, eyceS_AplResultFlg: stu2.eyceS_AplResultFlg });
                                   }
                               }


                           })
                           console.log($scope.mainsubjects)
                           //get all sub exam
                           $scope.subexamlist = [];

                           angular.forEach($scope.subwithsubexm, function (stu2) {

                               if ($scope.subexamlist.length == 0) {
                                   $scope.subexamlist.push({ emsE_Id: stu2.emsE_Id, emsE_SubExamName: stu2.emsE_SubExamName });
                               }
                               else if ($scope.subexamlist.length > 0) {
                                   var al_ct = 0;
                                   angular.forEach($scope.subexamlist, function (uf) {
                                       if (uf.emsE_Id == stu2.emsE_Id) {
                                           al_ct += 1;
                                       }
                                   })
                                   if (al_ct == 0) {
                                       $scope.subexamlist.push({ emsE_Id: stu2.emsE_Id, emsE_SubExamName: stu2.emsE_SubExamName});
                                   }
                               }


                           })
                           console.log($scope.subexamlist)
                           console.log($scope.mainsubjects)
                           $scope.tempsubexmlist = [];
                           angular.forEach($scope.mainsubjects, function (s1) {

                            if (s1.eyceS_AplResultFlg == true) {
                                  
                              
                               angular.forEach($scope.subexamlist, function (ss) {
                                  
                                   $scope.tempsubexmlist.push({ ismS_Id: s1.ismS_Id, emsE_Id: ss.emsE_Id, emsE_SubExamName: ss.emsE_SubExamName });

                               })
                            }
                           })


                           $scope.headersub = [];
                           angular.forEach($scope.mainsubjects, function (s1) {
                               var temp1 = [];
                               angular.forEach($scope.subwithsubexm, function (e1) {

                                   if (s1.ismS_Id == e1.ismS_Id) {
                                       temp1.push({ emsE_Id: e1.emsE_Id, emsE_SubExamName: e1.emsE_SubExamName, ismS_Id: e1.ismS_Id, ismS_SubjectName: e1.ismS_SubjectName })
                                   }

                               })
                               $scope.headersub.push({ ismS_Id: s1.ismS_Id, ismS_SubjectName: s1.ismS_SubjectName, eyceS_AplResultFlg: s1.eyceS_AplResultFlg, subexmlist: temp1 })

                           })

                           //  console.log($scope.savelist)


                           //console.log($scope.exmrank)







                       }
                       else if (promise.savenonsubjlist == null && promise.savelist == null) {
                           swal('No record Found');
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
            //to print
            $scope.print_HHS02 = function () {
                var innerContents = document.getElementById("HHS02").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                      '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/SnehasagarprogressreportPdf.css" />' +
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
            }
        }
    }
})();