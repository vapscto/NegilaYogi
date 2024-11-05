(function () {
    'use strict';
    angular
        .module('app')
        .controller('MarksEntry_Ent_ReportController', MarksEntry_Ent_ReportController)

    MarksEntry_Ent_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel','$timeout']
    function MarksEntry_Ent_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.obj = {};
        $scope.report = false;
        $scope.subjetClick = false;
        $scope.subjectListClick = [];
        $scope.imgname = "";
        $scope.BindData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";
            var pageid = 2;

            apiService.getURI("MarksEntry_Ent_Report/Getdetails", pageid).
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;           
                    $scope.exsplt = promise.exmstdlist;
                    $scope.stafflist = promise.stafflist;
                })
        };

        $scope.get_report = function () {
            $scope.Student_Count = [];
            $scope.exm_sublist = [];
            $scope.exam = [];
            $scope.exm_sublisttemp=[]
            if ($scope.myForm.$valid) {
                if ($scope.subjectlist != null && $scope.subjectlist.length > 0) {
                    angular.forEach($scope.subjectlist, function (ty) {
                        if (ty.selected == true) {
                            $scope.exm_sublisttemp.push({
                                ISMS_SubjectName: ty.ISMS_SubjectName
                            })
                        }
                    })
                }
                
                var HRMAMIDS = [];
                if ($scope.stafflist != null && $scope.stafflist.length > 0) {
                    angular.forEach($scope.stafflist, function (ty) {
                        if (ty.selected) {
                            HRMAMIDS.push(ty.applId);
                        }
                    })
                }
               
                var data = {
                    "EME_Id": $scope.emE_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "staffarray": HRMAMIDS,
                    
                }
                if ($scope.exm_sublisttemp != null && HRMAMIDS != null && $scope.exm_sublisttemp.length > 0 && HRMAMIDS.length > 0) {
                    
                    apiService.create("MarksEntry_Ent_Report/get_report", data).then(function (promise) {
                        if (promise.get_report != null && promise.get_report.length > 0) {
                            $scope.exam = promise.get_report;
                            $scope.exm_sublist = $scope.exm_sublisttemp;
                            $scope.Student_Count = promise.studentcount;
                            $scope.imgname = promise.imgname;
                        }
                        else {
                            swal("Record Not Found  !");
                        }
                    });
                }
                else {
                    swal("Please Select At Least One Subject / Staff !");
                }
               
            }
            else {
                $scope.submitted = true;
            }
            
        };

        $scope.clear = function () {
            $scope.yearlt = "";
            $scope.exsplt = "";
            $scope.stafflist = "";
            $state.reload();
        }


            $scope.sectionAll = function () {
           var checkStatus = $scope.login_Id;
            var count = 0;
            angular.forEach($scope.stafflist, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
                });
                $scope.subjectListClick();
        }
        //
        $scope.Subjectall = function () {
            var checkStatus = $scope.Subject_Id;
            var count = 0;
            angular.forEach($scope.subjectlist, function (itm) {
                itm.selected = checkStatus;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
            $scope.Student_Count = [];
            $scope.exm_sublist = [];
            $scope.exam = [];
            $scope.exm_sublisttemp = []
        }
        //
        $scope.StaffFliter = "";
        $scope.SubjectFliter = "";
        $scope.togchkSubject = function () {

            $scope.usercheckCCSS = $scope.subjectlist.every(function (options) {
                return options.selected;
            });
            $scope.Student_Count = [];
            $scope.exm_sublist = [];
            $scope.exam = [];
            $scope.exm_sublisttemp = []
        }
        $scope.isOptionsRequiredSubject = function () {
            return !$scope.subjectlist.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.stafflist.some(function (options) {
                return options.selected;
            });

        }

        $scope.togchkbxC = function () {

            $scope.usercheckCCSS = $scope.stafflist.every(function (options) {
                return options.selected;
            });
            $scope.subjectListClick();
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.subjectListClick = function () {
            $scope.LoginTemp = [];
            $scope.subjectlist = [];
            $scope.Student_Count = [];
            $scope.exm_sublist = [];
            $scope.exam = [];
            $scope.subjectlist = [];
            $scope.exm_sublisttemp = []
            if ($scope.stafflist != null && $scope.stafflist.length > 0) {
                angular.forEach($scope.stafflist, function (ty) {
                    if (ty.selected == true) {
                        $scope.LoginTemp.push({
                            LoginId: ty.login_Id
                        })
                    }
                })
            }
           
            
            if ($scope.LoginTemp != null && $scope.LoginTemp.length > 0 && $scope.asmaY_Id > 0) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "LoginIdLists": $scope.LoginTemp,

                }

                apiService.create("MarksEntry_Ent_Report/SubjectList", data).then(function (promise) {
                    if (promise.subjecList != null && promise.subjecList.length > 0) {
                        $scope.subjectlist = promise.subjecList;
                    }
                    else {
                        swal("Subject Not Found  !");
                    }
                });
            }
            else {
                swal("Select At Least One Staff / Academic Year ")
                return;
            }
            
           
        }
        $scope.printToCart = function () {
            var innerContents = document.getElementById("Baldwin").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ProgressReport/BGIProgressReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.exportToExcel = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
           
        }
    }

})();