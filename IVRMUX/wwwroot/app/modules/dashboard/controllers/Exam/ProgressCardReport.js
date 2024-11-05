
(function () { 
    'use strict';
    angular
.module('app')
.controller('ProgressCardReportController', ProgressCardReportController)

    ProgressCardReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ProgressCardReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
       $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

      //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("ProgressCardReport/Getdetails").
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
                    apiService.create("ProgressCardReport/savedetails", data).
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

                                             //angular.forEach(promise.subjlist, function (abc) {
                                             //    var cnt = 0;
                                             //    angular.forEach(stu_subj_list, function (abc1) {
                                             //        if(abc1.ismS_Id==abc.ismS_Id)
                                             //        {
                                             //            cnt += 1;

                                             //        }
                                             //    })
                                             //    if(cnt==0)
                                             //    {
                                             //        stu_subj_list.push({ amsT_Id: stu_id, ismS_Id: abc.ismS_Id, ismS_SubjectName: abc.ismS_SubjectName, estmpS_MaxMarks: opq.eyceS_MaxMarks, estmpS_ObtainedMarks: null, estmpS_ObtainedGrade: null, estmpS_PassFailFlg: null });
                                             //    }

                                             //})


                                             temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                         }
                                         else if (temp_list.length > 0) {
                                             
                                             var already_cnt = 0;
                                             angular.forEach(temp_list, function (opq1) {
                                                 //if (opq1.student_id != stu_id) {
                                                 //    temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                                 //}
                                                 if (opq1.student_id == stu_id) {
                                                     already_cnt += 1;
                                                     
                                                 }
                                             })
                                             if(already_cnt==0)
                                             {
                                                 //angular.forEach(promise.subjlist, function (abc) {
                                                 //    var cnt = 0;
                                                 //    angular.forEach(stu_subj_list, function (abc1) {
                                                 //        if (abc1.ismS_Id == abc.ismS_Id) {
                                                 //            cnt += 1;
                                                 //        }
                                                 //    })
                                                 //    if (cnt == 0) {
                                                 //        stu_subj_list.push({ amsT_Id: stu_id, ismS_Id: abc.ismS_Id, ismS_SubjectName: abc.ismS_SubjectName, estmpS_MaxMarks: opq.eyceS_MaxMarks, estmpS_ObtainedMarks: null, estmpS_ObtainedGrade: null, estmpS_PassFailFlg: null});
                                                 //    }

                                                 //})


                                                 temp_list.push({ student_id: promise.savelist[x].amsT_Id, amsT_FirstName: promise.savelist[x].amsT_FirstName, amsT_DOB: promise.savelist[x].amsT_DOB, amsT_AdmNo: promise.savelist[x].amsT_AdmNo, amaY_RollNo: promise.savelist[x].amaY_RollNo, asmcL_ClassName: promise.savelist[x].asmcL_ClassName, asmC_SectionName: promise.savelist[x].asmC_SectionName, estmP_TotalMaxMarks: promise.savelist[x].estmP_TotalMaxMarks, estmP_TotalObtMarks: promise.savelist[x].estmP_TotalObtMarks, estmP_Percentage: promise.savelist[x].estmP_Percentage, estmP_TotalGrade: promise.savelist[x].estmP_TotalGrade, sub_list: stu_subj_list });
                                             }
                                         }

                                     }
                                     $scope.exam = promise.savelist[0].emE_ExamName;
                                     $scope.exm_sublist = promise.subjlist;
                                     $scope.processtot = promise.savelisttot;
                                     $scope.subj_grade_remarks = promise.grade_details;
                                     $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                                     $scope.report_list = temp_list;
                                    // angular.forEach($scope.clslist, function (itm) {
                                    //     if (itm.asmcL_Id == $scope.asmcL_Id) {
                                    //         $scope.cla = itm.asmcL_ClassName;
                                    //     }
                                    // })
                                    // angular.forEach($scope.yearlt, function (itm) {
                                    //     if (itm.asmaY_Id == $scope.asmaY_Id) {
                                    //         $scope.yr = itm.asmaY_Year;
                                    //     }
                                    // })
                                    // angular.forEach($scope.seclist, function (itm) {
                                    //     if (itm.asmS_Id == $scope.asmS_Id) {
                                    //         $scope.sec = itm.asmC_SectionName;
                                    //     }
                                    // })
                                    // angular.forEach($scope.exsplt, function (itm) {
                                    //     if (itm.emE_Id == $scope.emE_Id) {
                                    //         $scope.exmmid = itm.emE_ExamName;
                                    //     }
                                    // })

                                    //$scope.studentslt = promise.savelist;
                                    //$scope.studentslt1 = promise.subjlist;
                                    //$scope.table_list_sub_wise = [];
                                   // for (var a = 0; a < promise.savelist.length; a++) {
                                   //     $scope.cla = promise.savelist[0].asmcL_ClassName
                                   //     $scope.sec = promise.savelist[0].asmC_SectionName
                                   //     $scope.exmmid = promise.savelist[0].emE_ExamName
                                   //     $scope.stdnam = promise.savelist[0].amsT_FirstName
                                   //     $scope.table_list_sub_wise.push({ clases: $scope.cla, sec: $scope.sec, examname: $scope.exmmid, studentname: $scope.stdnam })
                                   ////$scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.albumNameArray1[a].staffName, id: $scope.albumNameArray1[a].hrmE_Id });
                                   // }


                                 } else {
                                     swal('No record Found');
                                 }
                             })
                }
            };

            $scope.cancel = function () {
                $scope.asmcL_Id = ""
                $scope.emcA_Id =""
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
            $scope.printToCart = function () {
                var innerContents = document.getElementById("Baldwin").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                      '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BProgressReportPdf.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }

            $scope.get_totalmin = function (exm_subjs, stu_subjs) {
                

                $scope.stu_grandmin_marks = 0;
                angular.forEach(exm_subjs, function (itm) {
                    if (itm.eyceS_AplResultFlg)
                    {
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