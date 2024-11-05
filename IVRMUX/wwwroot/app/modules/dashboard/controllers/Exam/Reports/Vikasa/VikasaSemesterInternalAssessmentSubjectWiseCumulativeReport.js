
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportController', VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportController)

    VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {



        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        //TO  GEt The Values iN Grid

        $scope.BindData = function () {


            apiService.getDATA("VikasaSemesterInternalAssessmentSubjectWiseCumulativeReport/Getdetails").
                then(function (promise) {

                    $scope.yearlt = promise.yearlist;

                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";


                    $scope.classDropdown = "";
                    $scope.sectionDropdown = "";


                })
        };

        $scope.get_class = function () {


            $scope.asmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            }
            apiService.create("VikasaSemesterInternalAssessmentSubjectWiseCumulativeReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classlist;

                })
        }
        $scope.get_section = function () {

            $scope.amsT_Id = "";
            $scope.asmS_Id = "";
            $scope.emE_Id = "";
            $scope.studentDropdown = "";
            $scope.exsplt = "";
            var data = {
                "ASMCL_Id": $scope.asmcL_Id
            }
            apiService.create("VikasaSemesterInternalAssessmentSubjectWiseCumulativeReport/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_category = function () {

            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            var data = {

                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id

            }
            apiService.create("VikasaSemesterInternalAssessmentSubjectWiseCumulativeReport/get_category", data)
                .then(function (promise) {
                    $scope.categoryDropdown = promise.categoryList;


                })

        }

        $scope.get_subject = function (asmS_Id) {

            $scope.amsT_Id = "";
            $scope.emE_Id = "";
            $scope.exsplt = "";
            var data = {
                "ASMS_Id": $scope.asmS_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "EMCA_Id": $scope.emcA_Id
            }
            apiService.create("VikasaSemesterInternalAssessmentSubjectWiseCumulativeReport/get_subject", data)
                .then(function (promise) {
                    $scope.subjectDropdown = promise.subjectList;


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
                    "ISMS_Id": $scope.ismS_Id,
                    "EMCA_Id": $scope.emcA_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }


                apiService.create("VikasaSemesterInternalAssessmentSubjectWiseCumulativeReport/showdetails", data).
                    then(function (promise) {


                        if (promise.studentList.length > 0) {
                            $scope.Cumureport = true;
                            $scope.screport = true;
                            $scope.Datalist = promise.studentList;
                            $scope.asmcL_ClassName = promise.studentList[0].asmcL_ClassName;
                            $scope.asmC_SectionName = promise.studentList[0].asmC_SectionName;
                            $scope.gradel = promise.gradelist;

                            //for group head
                            if (promise.examGroupname.length > 0) {
                                $scope.GroupHead = promise.examGroupname;
                                $scope.temp_headers = [];
                                angular.forEach($scope.GroupHead, function (grp) {
                                    $scope.temp_headers.push({ grp_name: grp.empsG_GroupName, grp_id: grp.empsG_Id, flag: 0 });
                                    //  $scope.temp_headers.push({ grp_name: grp.empsG_GroupName + '(' + grp.empsG_PercentValue + '%)', grp_id: grp.empsG_Id, flag: 1 });
                                })
                            }
                            //for group head marks

                            if (promise.examgroupmarks.length > 0) {

                                $scope.Dataexamgroup = promise.examgroupmarks;

                                $scope.temparrayData = [];
                                $scope.temparrayData1 = [];
                                angular.forEach($scope.Datalist, function (t1) {

                                    angular.forEach($scope.GroupHead, function (t2) {
                                        angular.forEach($scope.Dataexamgroup, function (t3) {
                                            if (t3.amsT_Id == t1.amsT_Id && t3.empsG_Id == t2.empsG_Id) {
                                                $scope.temparrayData.push({ estmpS_ObtainedMarks: t3.estmpS_ObtainedMarks, estmpS_ObtainedGrade: t3.estmpS_ObtainedGrade, estmpS_MaxMarks: t3.estmpS_MaxMarks, estmpS_SectionAverage: t3.estmpS_SectionAverage, estmpS_PassFailFlg: t3.estmpS_PassFailFlg, group_per_marks: (t3.estmpS_ObtainedMarks * t2.empsG_PercentValue) / 100, amsT_Id: t1.amsT_Id, ismS_Id: t2.ismS_Id, grp_id: t2.empsG_Id });
                                            }
                                        });
                                    });
                                });

                                //$scope.temp_Totalp = [];
                                //angular.forEach($scope.temparrayData, function (Totalp) {

                                //})

                                $scope.temp_GroupD = [];
                                angular.forEach($scope.temparrayData, function (grpD) {
                                    $scope.temp_GroupD.push({ grpD_name: grpD.estmpS_ObtainedMarks, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, flag: 0 });
                                    // $scope.temp_GroupD.push({ grpD_name: grpD.group_per_marks, grpD_id: grpD.empsG_Id, amsT_Id: grpD.amsT_Id, flag: 1 });
                                });
                            }

                            $scope.yearname = promise.basicListYear[0].yearname;
                            if (promise.basiListclass.length > 0) {
                                $scope.asmcL_ClassName = promise.basiListclass[0].className;
                            }
                            if (promise.basiListsectiont.length > 0) {
                                $scope.asmC_SectionName = promise.basiListsectiont[0].sectionname;
                            }
                            if (promise.basiListsubject.length > 0) {
                                $scope.ismS_SubjectName = promise.basiListsubject[0].subjectName;
                            }
                            if (promise.basicListYear.length > 0) {
                                $scope.asmaY_Year = promise.basicListYear[0].yearname;
                            }
                            if (promise.basiListcategory.length > 0) {
                                $scope.EMCA_CategoryName = promise.basiListcategory[0].emcA_CategoryName;
                            }

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
        // end for print
    }

})();