
(function () {
    'use strict';
    angular
.module('app')
.controller('VikasaProgressReportExamController', VikasaProgressReportExamController)

    VikasaProgressReportExamController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function VikasaProgressReportExamController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
            apiService.getDATA("VikasaAssessment2Report/Getdetails").
       then(function (promise) {

           $scope.yearlt = promise.yearlist;
           //$scope.clslist = promise.classlist;
           //$scope.seclist = promise.SectionList;
           //$scope.amstlt = promise.amstlist;
           //$scope.exsplt = promise.exmstdlist;
           //$scope.studentDropdown = promise.studentList;
           $scope.asmcL_Id = "";
           $scope.asmS_Id = "";
           $scope.amsT_Id = "";
           $scope.emE_Id = "";
           $scope.classDropdown = "";
           $scope.sectionDropdown = "";
           $scope.exsplt = "";

       })
        };

        $scope.get_class = function () {
            
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.sectionDropdown = "";
            $scope.exsplt = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("VikasaAssessment2Report/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                })
        }
        $scope.get_section = function () {
            
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("VikasaAssessment2Report/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        
        $scope.get_Exam = function () {
            

            $scope.emE_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id

            }
            apiService.create("VikasaAssessment2Report/get_exam", data)
                .then(function (promise) {
                    $scope.exsplt = promise.examList;

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
                    "ASMS_Id": $scope.asmS_Id

                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                var temp_list = [];
                apiService.create("VikasaProgressReportExam/savedetails", data).
                         then(function (promise) {
                             
                             if (promise.studentlist.length > 0) {
                                 $scope.screport = true;
                                 $scope.studentlist = promise.studentlist;
                                 if (promise.subjlist.length > 0) {
                                     $scope.subjectlist = promise.subjlist;
                                     if (promise.savelist.length > 0) {
                                         $scope.datalist = promise.savelist;
                                         if (promise.clstchname.length > 0)
                                         {
                                             $scope.clastechname = promise.clstchname[0].hrmE_EmployeeFirstName;
                                         } else {
                                             $scope.clastechname = "";
                                         }
                                         $scope.exam = promise.savelist[0].emE_ExamName;
                                         if (promise.empAttendence.length > 0)
                                         {
                                             $scope.empAttendence = promise.empAttendence;
                                         } else {
                                             $scope.empAttendence = "";
                                         }
                                         if (promise.basiListclass.length > 0) {
                                             $scope.asmcL_ClassName = promise.basiListclass[0].className;
                                             if (promise.basiListsectiont.length > 0) {
                                                 $scope.asmC_SectionName = promise.basiListsectiont[0].sectionname;
                                             }
                                         }
                                         $scope.issuedate = new Date();
                                         $scope.yearname = promise.basicListYear[0].yearname;
                                        
                                        
                                     }
                                 }
                             }
                             else {
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
    
        // end for print

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

        $scope.VIKASAProgressCardReport = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                  '<link type="text/css" media="print" rel="stylesheet" href="css/print/Vikasa/ProgressCardReport/ProgressCardReportPdf.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
            '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

    }

})();