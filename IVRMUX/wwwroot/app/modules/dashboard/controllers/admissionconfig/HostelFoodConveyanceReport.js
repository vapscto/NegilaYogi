(function () {
    'use strict';
    angular
.module('app')
        .controller('HostelFoodConveyanceReportController', HostelFoodConveyanceReportController)

    HostelFoodConveyanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function HostelFoodConveyanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.pagesrecord = {};
        $scope.excel_flag = true;
        $scope.catreport = true;
        $scope.currentPage = 1;
        // $scope.itemsPerPage = 10;
        $scope.table_flag = false;

        $scope.printdatatable = [];

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
        //$scope.itemsPerPage = 10;



        $scope.exportToExcel = function (tableId) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }



        $scope.printData = function (printSectionId) {
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
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


        $scope.toggleAll = function () {
            $scope.printdatatable = [];

            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (obj.regno).indexOf($scope.searchValue) >= 0 || (obj.admno).indexOf($scope.searchValue) >= 0 || (obj.stuFN).indexOf($scope.searchValue) >= 0 || (obj.class).indexOf($scope.searchValue) >= 0 || (obj.section).indexOf($scope.searchValue) >= 0;
        }



        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
            $scope.all2 = $scope.students.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }



        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("HostelFoodConveyanceReport/getdata", pageid).
        then(function (promise) {
            $scope.yearlist = promise.allAcademicYear;
            $scope.classlist = promise.allClass;
            $scope.sectionlist = promise.allSection;
        })
        }
        $scope.report123 = false;
        $scope.submitted = false;


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };




        $scope.angularData =
                   {
                       'nameList': []
                   };

        $scope.vals = [];


        $scope.Report = function (hfc) {

            $scope.printdatatable = [];
            $scope.searchValue = "";
            
            if ($scope.myForm.$valid) {
                var acedamicId = $scope.hfc.asmaY_Id;
                var sectionId = $scope.hfc.asmC_Id;
                var classId = $scope.hfc.asmcL_Id;
                var Flag = $scope.hfc.optradio;

                if (acedamicId == undefined || acedamicId == "") {
                    acedamicId = 0;
                }
                if (classId == undefined || classId == "") {
                    classId = 0;
                }
                if (sectionId == undefined || sectionId == "") {
                    sectionId = 0
                }
                var data = {
                    "ASMAY_Id": acedamicId,
                    "ASMCL_Id": classId,
                    "ASMC_Id": sectionId,
                    "HFC_Flag": Flag
                }
                

                if ($scope.hfc.asmaY_Id == undefined || $scope.hfc.asmaY_Id == "" || $scope.hfc.asmC_Id == undefined || $scope.hfc.asmC_Id == "" || $scope.hfc.asmcL_Id == undefined || $scope.hfc.asmcL_Id == "") {
                    swal("Select Required Fields")
                }
                else {
                    apiService.create("HostelFoodConveyanceReport/Studdetails", data)
                                   .then(function (promise) {
                                       
                                       if (promise.studentlist == null || promise.studentlist.length == 0) {
                                           swal(" Record Not Found. !!")
                                           $scope.report123 = false;
                                           $scope.excel_flag = true;
                                           $scope.catreport = true;
                                       }
                                       else if (promise.studentlist.length > 0 && promise.studentlist != null) {
                                           $scope.students = promise.studentlist;

                                           for (var i = 0; i < promise.studentlist.length; i++) {

                                               var name = promise.studentlist[i].stuFN;

                                               if (promise.studentlist[i].stuMN !== null) {
                                                   name += " " + promise.studentlist[i].stuMN;
                                               }
                                               if (promise.studentlist[i].stuLN != null) {
                                                   name += " " + promise.studentlist[i].stuLN;
                                               }
                                               $scope.vals.push(name);
                                           }
                                           angular.forEach($scope.vals, function (v, k) {
                                               $scope.angularData.nameList.push({
                                                   'fullname': v
                                               });
                                           });

                                           var j = 0;
                                           angular.forEach($scope.students, function (obj) {
                                               //Using bracket notation
                                               obj["fullname"] = $scope.angularData.nameList[j].fullname;
                                               j++;
                                           });

                                           angular.forEach($scope.students, function (objectt) {
                                               if (objectt.fullname.length > 0) {
                                                   var string = objectt.fullname
                                                   objectt.namme = string.replace(/  +/g, ' ');
                                               }
                                           })
                                           $scope.report123 = true;
                                           $scope.excel_flag = false;
                                           $scope.catreport = false;

                                           $scope.students = promise.studentlist;
                                           $scope.presentCountgrid = $scope.students.length;
                                           console.log(promise.studentlist);

                                       }
                                   });
                }
            } else {
                $scope.submitted = true;
            }
        }



        var studclear = [];
        $scope.Clearid = function () {           
            $state.reload();
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                //"FMG_GroupName": $scope.search,
                //"FMG_Remarks": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StudentYearLoss/1", data).
        then(function (promise) {
            $scope.students = promise.alldatagridreport;
            swal("Searched Successfully");
        })
        }
    }
})();

