(function () {
    'use strict';
    angular.module('app').controller('StudentIdCardFormatController', StudentIdCardFormatController)

    StudentIdCardFormatController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$filter', 'Excel', '$timeout', '$compile']
    function StudentIdCardFormatController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $filter, Excel, $timeout, $compile) {

        $scope.edit = false;
        $scope.excel_flag = true;
        $scope.excel_flag = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }

        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.maxdate = new Date();

        $scope.OnLoadStudentIdCardDetails = function () {
            var pageid = 2;
            apiService.getURI("StudentIdCardFormat/OnLoadStudentIdCardDetails", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.GetAcademicYearList = promise.getAcademicYearList;
                    $scope.GetClassList = promise.getClassList;                    
                    $scope.ASMAY_Id = promise.asmaY_Id;
                }
            });
        };

        $scope.OnChangeYear = function (objj) {
            var data = "";
            $scope.GetClassList = [];
            $scope.GetSectionList = [];
            $scope.GetStudentList = [];
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentIdCardFormat/OnChangeYear", data).then(function (promise) {
                if (promise.getClassList !== null && promise.getClassList.length > 0) {
                    $scope.GetClassList = promise.getClassList;
                } else {                     
                    swal("Class List Not Found");
                }
            });
        };

        $scope.OnChangeClass = function (objj) {
            var data = "";
            $scope.GetSectionList = [];
            $scope.GetStudentList = [];
            $scope.ASMS_Id = "";
            data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("StudentIdCardFormat/OnChangeClass", data).then(function (promise) {
                if (promise.getSectionList !== null && promise.getSectionList.length > 0) {
                    $scope.GetSectionList = promise.getSectionList;
                } else {
                    $scope.AMST_Id = "";
                    swal("Section List Not Found");
                }
            });
        };

        $scope.OnChangeSection = function (objj) {
            var data = "";
            $scope.GetStudentList = [];
            data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("StudentIdCardFormat/OnChangeSection", data).then(function (promise) {
                if (promise.getStudentList !== null && promise.getStudentList.length > 0) {
                    $scope.GetStudentList = promise.getStudentList;
                    angular.forEach($scope.GetStudentList, function (dd) {
                        dd.checkedsub = true;                            
                    });
                } else {
                    $scope.AMST_Id = "";
                    swal("No students are found for your search");
                }
            });
        };
        
        $scope.GetReport = function () {
            $scope.StudentTempList = [];
            $scope.submitted = false;
            if ($scope.myform.$valid) {

                angular.forEach($scope.GetStudentList, function (dd) {
                    if (dd.checkedsub) {
                        $scope.StudentTempList.push({ AMST_Id: dd.amsT_Id, StudentName: dd.studentName });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "StudentTempList": $scope.StudentTempList                  
                };

                apiService.create("StudentIdCardFormat/GetReportDetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.cardData = promise.cardData;


                        var e1 = angular.element(document.getElementById("idcard"));
                        $compile(e1.html(promise.retrunMsg))(($scope));

                    }

                    //idcard-dynamic-design-from-azure added by adarsh
                  
                });
            }
            else {
                $scope.submitted = true;
            }


           
        };

        $scope.isOptionsRequired3 = function () {
            return !$scope.GetStudentList.some(function (options) {
                return options.checkedsub;
            });
        };  

        $scope.cleardata = function () {
            $scope.submitted = false;

            $scope.GetClassList = [];
            $scope.GetSectionList = [];
            $scope.GetStudentList = [];
            $scope.GetAcademicYearList = [];
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.ASMAY_Id = "";
            $scope.OnLoadStudentIdCardDetails();
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };       

        $scope.smartcardstudent = function () {
            $scope.print = true;

            var innerContents = document.getElementById("smartcardidstudent").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +

                //'<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/idcardprint.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
    }
})();