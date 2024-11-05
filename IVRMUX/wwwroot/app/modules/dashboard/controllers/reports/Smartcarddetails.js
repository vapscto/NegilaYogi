(function () {
    'use strict';
    angular
.module('app')
.controller('SmartcarddetailsController', SmartcarddetailsController)

    SmartcarddetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function SmartcarddetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.searchValue = '';

        $scope.AttRptDropdownList = function () {
            $scope.currentPage = 1;
            // $scope.itemsPerPage = 10;
            $scope.printstudents = [];
            apiService.get("Smartcarddetails/getdetails/").then(function (promise) {

                $scope.yearDropdown = promise.academicList;
                $scope.classDropdown = promise.classlist;
                $scope.sectionDropdown = promise.sectionList;
               

            });
        }
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;


        
        $scope.onclickloaddata = function () {          
        };

        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.catreport = true;
        $scope.submitted = false;

        $scope.columnTotal = [];
        $scope.angularData =
          {
              'nameList': []
          };

        $scope.vals = [];
        $scope.rptyearwisedata = function () {
            $scope.printstudents = [];
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMC_Id": $scope.asmC_Id,
                    "stdmobilenumber": $scope.Admnoallind
                }
                apiService.create("Smartcarddetails/getAttendetails", data)
                    .then(function (promise) {
                                              
                        $scope.students = promise.getcarddetails;
                        $scope.presentCountgrid = $scope.students.length;
                        for (var i = 0; i < promise.getcarddetails.length; i++) {
                            var name_list = promise.getcarddetails[i].name;
                            $scope.vals.push(name_list);
                        }
                        //angular.forEach($scope.vals, function (v, k) {
                        //    $scope.angularData.nameList.push({
                        //        'fullname': v
                        //    });
                        //});
                        //var j = 0;
                        //angular.forEach($scope.students, function (obj) {
                        //    //Using bracket notation
                        //    obj["fullname"] = $scope.angularData.nameList[j].fullname;
                        //    j++;
                        //});
                        //angular.forEach($scope.students, function (objectt) {
                        //    if (objectt.fullname.length > 0) {
                        //        var string = objectt.fullname
                        //        objectt.namme = string.replace(/  +/g, ' ');
                        //    }
                        //})
                        //angular.forEach($scope.columnsTest, function (value1, i) {
                        //    var clmttl = $scope.columnsTest[i].TOTAL_classheld;
                        //    $scope.columnTotal.push(clmttl);
                        //});
                        //angular.forEach($scope.students, function (value1, i) {
                        //    $scope.students[i].totalval = 0;
                        //});

                        //$scope.totalList = [];
                        
                                             
                        if (promise.getcarddetails == null || promise.getcarddetails.length == 0) {
                            swal('No Records Found!');
                            $scope.catreport = true;
                        }
                        else {
                            $scope.catreport = false;
                            angular.forEach($scope.totalList, function (value1, i) {
                                $scope.students[i].totalval = $scope.totalList[i];
                            });                                                       
                            console.log($scope.students);
                        }
                    })
            } else {
                $scope.submitted = true;
            }
        }

        $scope.toggleAll = function () {
            
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.Clearid = function () {
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.submitted = false;
            $scope.catreport = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printData = function (printSectionId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
            '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
             '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
            '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }

        $scope.exportToExcel = function (export_id) {
            
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(export_id, 'WireWorkbenchDataExport');

                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
    }
})();
