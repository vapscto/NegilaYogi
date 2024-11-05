
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamPromotionReportController', ExamPromotionReportController)

    ExamPromotionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$filter', 'Excel', '$timeout']
    function ExamPromotionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $filter, Excel,$timeout) {

       
        $scope.ddate = new Date();
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.coptyright = copty;
        $scope.sortKey = "regno";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;

        $scope.searchValue = "";


        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("ExamPromotionReport/Getdetails", pageid).then(function (promise) {
                $scope.year_list = promise.getyear;
            })
        };

        $scope.get_class = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("ExamPromotionReport/get_class", data).then(function (promise) {
                if (promise != null) {
                    $scope.classlist = promise.getclass;
                    if ($scope.classlist.length > 0) {
                        $scope.classlist = promise.getclass;
                    } else {
                        swal("Class Teacher Is Not Mapped For This Staff");
                    }

                } else {
                    swal("No Records Found");
                }
            })
        }

        $scope.get_section = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("ExamPromotionReport/get_section", data).then(function (promise) {
                if (promise != null) {
                    $scope.getsection = promise.getsection;
                    if ($scope.getsection.length > 0) {
                        $scope.sectionlist = promise.getsection;
                    } else {
                        swal("No Records Found");
                    }
                }
                else {
                    swal("No Records Found");
                }
            })
        }

        $scope.get_exam = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            }
            apiService.create("ExamPromotionReport/get_exam", data).then(function (promise) {
                if (promise != null) {
                    $scope.getexam = promise.getexam;
                    if ($scope.getexam.length > 0) {
                        $scope.exam_list = promise.getexam;
                    } else {
                        swal("No Exam Is Mapped For this Combination");
                    }
                } else {
                    swal("No Records Found");
                }

            })
        }

        $scope.result_data = true;
        $scope.submitted = false;
        $scope.search_student = function () {
            debugger;
            $scope.clsp = 0;
            if ($scope.myForm.$valid) {

                if ($scope.examtype == 1) {
                    $scope.EME_Id = 0;
                } else {
                    $scope.EME_Id = $scope.EME_Id;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "EME_Id": $scope.EME_Id,
                    "examtype": $scope.examtype,
                }
                apiService.create("ExamPromotionReport/get_reports", data).then(function (promise) {
                    if (promise != null) {

                        $scope.student = promise.get_result;
                        if ($scope.student.length > 0) {
                            if ($scope.examtype == '1') {
                                $scope.clsp = 6;
                            }
                            else {
                                $scope.clsp = 5;
                            }
                            if ($scope.student.length > 0) {
                                $scope.result_data = false;

                                angular.forEach($scope.year_list, function (fff) {
                                    if (fff.asmaY_Id == $scope.ASMAY_Id) {
                                        $scope.yearname = fff.asmaY_Year;
                                    }
                                })

                                angular.forEach($scope.classlist, function (fff) {
                                    if (fff.asmcL_Id == $scope.ASMCL_Id) {
                                        $scope.asmcL_ClassName = fff.asmcL_ClassName;
                                    }
                                })

                                angular.forEach($scope.getsection, function (fff) {
                                    if (fff.asmS_Id == $scope.ASMS_Id) {
                                        $scope.asmC_SectionName = fff.asmC_SectionName;
                                    }
                                })

                                angular.forEach($scope.exam_list, function (fff) {
                                    if (fff.emE_Id == $scope.EME_Id) {
                                        $scope.emE_ExamName = fff.emE_ExamName;
                                    }
                                })

                            }
                            else {
                                $scope.result_data = true;
                            }
                        }
                            else {
                                swal("No Records Found");
                            }
                        

                       

                    }
                    else {
                        swal("No Records Found");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.sortKey = 'studentname';
        $scope.sortReverse = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.submitted1 = false;
      

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        }

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        }

        $scope.getlist = function () {
            $scope.select_cat = false;
            if ($scope.examtype == 0) {
                $scope.student = [];
                $scope.getsavedetails = [];
            } else {
                $scope.EME_Id = "";
                $scope.student = [];
                $scope.getsavedetails = [];
            }
        }

        $scope.Print = function () {           
            var innerContents = document.getElementById("printIndExId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/Sports/HouseReportPdf.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
           
        }

        $scope.ExportToExcel = function (tableId) {           
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            
        }

    }

})();