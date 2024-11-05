
(function () {
    'use strict';
    angular
.module('app')
.controller('SportStudentEligbleReportController', SportStudentEligbleReportController)

    SportStudentEligbleReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function SportStudentEligbleReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {


        $scope.DeleteRecord = {};
        $scope.EditRecord = {};
        $scope.obj = {};
        $scope.studentlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 5;

        $scope.ddate = {};
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


        //TO  GEt The Values iN Grid

        $scope.BindData = function () {
             

            apiService.getDATA("SportStudentEligbleReport/Getdetails").
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
            apiService.create("SportStudentEligbleReport/get_class", data)
                .then(function (promise) {
                    $scope.classDropdown = promise.classList;

                })
        }
        $scope.get_section = function () {
             

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportStudentEligbleReport/get_section", data)
                .then(function (promise) {
                    $scope.sectionDropdown = promise.sectionList;


                })
        }
        $scope.get_student = function (asmS_Id) {
             

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMS_Id": $scope.ASMS_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            }
            apiService.create("SportStudentEligbleReport/get_student", data)
                .then(function (promise) {
                    if (promise.studentList1.length > 0) {
                        $scope.stdrecord = true;
                        $scope.stuDropdown = promise.studentList1;
                    }



                })

        }
        //$scope.clscatId = 0;
        $scope.columnSort = false;
        $scope.isOptionsRequired = function () {
            return !$scope.stuDropdown.some(function (options) {
                return options.Selected;
            });
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
        $scope.selectedStdList = [];
        $scope.showdetails = function () {
             
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.stuDropdown != "" && $scope.stuDropdown != null) {
                    if ($scope.stuDropdown.length > 0) {
                        $scope.selectedStdList = [];
                        for (var i = 0; i < $scope.stuDropdown.length; i++) {
                            if ($scope.stuDropdown[i].Selected == true) {
                                $scope.selectedStdList.push($scope.stuDropdown[i]);
                            }
                        }
                    }
                }
                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "StudentList": $scope.selectedStdList
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("SportStudentEligbleReport/showdetails", data).
                         then(function (promise) {
                              
                             if (promise.viewlist.length > 0) {
                                 $scope.newuser = promise.viewlist;
                                 $scope.presentCountgrid = $scope.newuser.length;
                                 $scope.Cumureport = true;
                                 $scope.screport = true;
                                 $scope.export = true;
                             }
                             else {
                                 swal("No Records Found");
                             }

                         })
            }
        };

        $scope.cancel = function () {
            $scope.asmaY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.stdrecord = "";
            $scope.Cumureport = false;
            $scope.screport = false;
            $scope.export = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
           
        }

        //for print
        $scope.Print = function () {

            if ($scope.filterValue1 !== null && $scope.filterValue1.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
      '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
       '<link type="text/css" media="print" href="css/print/preadmission/HouseReportPdf.css" rel="stylesheet" />' +
      '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
          );
                popupWinindow.document.close();
            }
        }

        // end for print

        $scope.exportToExcel = function (table) {
             
            //if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
            // }
            // else {
            //   swal("Please Select Records to be Exported");
            // }
        }
    }

})();