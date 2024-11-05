(function () {
    'use strict';
    angular
.module('app')
.controller('CategoryWiseAttendanceController', CategoryWiseAttendanceController)

    CategoryWiseAttendanceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache']
    function CategoryWiseAttendanceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache) {

        $scope.ddate = {};
        $scope.ddate = new Date(); 
        $scope.objj = {};
        

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

        $scope.propertyName = 'AMC_Name';
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.setfromdate = function (iddata) {
            for (var k = 0; k < $scope.yearDropdown.length; k++) {
                if ($scope.yearDropdown[k].asmaY_Id == iddata) {
                    var data = $scope.yearDropdown[k].asmaY_Year;
                }
            }

            if (data != null) {
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date
                $scope.minDatef = new Date(
                      $scope.fromDate,
                       $scope.frommon,
                        $scope.fromDay + 1);

                $scope.maxDatef = new Date(
                      $scope.toDate,
                       $scope.tomon,
                        $scope.toDay + 365);

            }

        }
        $scope.printdatatable = [];
        $scope.currentPage = 1;
      //  $scope.itemsPerPage = 10;
        $scope.catreport_grid = false;


        $scope.submitted = false;
        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
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
            
            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (obj.AMST_FirstName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_AdmNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMAY_RollNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_DOB_Words).indexOf($scope.searchValue) >= 0 || (obj.AMST_FatherName).indexOf($scope.searchValue) >= 0 || (obj.AMST_MotherName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_FatherMobleNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0 || (obj.AMST_emailId).indexOf($scope.searchValue) >= 0 || (obj.AMST_BloodGroup).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0;
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

        //$scope.printData = function ()
        //{

        //    
        //    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0)
        //    {
        //        var divToPrint = document.getElementById("table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //    }
        //    else {
        //        swal("Please Select Records to be Printed");
        //    }
        //}

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("print_id").innerHTML;
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
        
       
        $scope.StuAttRptDropdownList = function () {
            apiService.get("CategoryWiseAttendance/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.categoryDropdown = promise.category_list;

                $scope.categoryflag = promise.categoryflag;

            });
        }
        //category
        $scope.isOptionsRequiredclass = function () {
            return !$scope.categoryDropdown.some(function (item) {
                return item.selected;
            });
        };

        $scope.al_checkcategory = function (all, ASMCL_Id) {


            $scope.categorylistarray = [];
            $scope.obj.usercheckCC = all;

            var toggleStatus = $scope.obj.usercheckCC;
            angular.forEach($scope.categoryDropdown, function (role) {
                role.selected = toggleStatus;
            });


            $scope.categorylistarray = [];
            angular.forEach($scope.categoryDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.categorylistarray.push({ AMC_Id: qq.amC_Id })
                }
            });


           

        }


        $scope.togchkbxC = function () {
            $scope.categorylistarray = [];
            angular.forEach($scope.categoryDropdown, function (qq) {
                if (qq.selected == true) {
                    $scope.categorylistarray.push({ AMC_Id: qq.amC_Id })
                }
            })
        }
        //
        $scope.catreport = true;
        $scope.submitted = false;
        $scope.showReport = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            if ($scope.myForm.$valid) {
                var fromdate = new Date($scope.fromdate).toDateString();
                //var AMC_Id = 0
                //if ($scope.objj.amC_Id != 'All') {
                //    AMC_Id = $scope.objj.amC_Id
                //}


                $scope.categorylistarraynew = [];
                if ($scope.categorylistarray != null || $scope.categorylistarray > 0) {
                    $scope.categorylistarraynew = $scope.categorylistarray;
                }
                else {
                    angular.forEach($scope.categoryDropdown, function (qq) {
                        $scope.classlistarraynew.push({ AMC_Id: qq.amC_Id });
                    })
                }
                var AMC_Id = 0
                if ($scope.objj.amC_Id != 'All') {
                    AMC_Id = $scope.objj.amC_Id
                }
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "fromdate": fromdate,
                    "AMC_Id": AMC_Id,
                    "categorylistarray": $scope.categorylistarraynew
                }
                apiService.create("CategoryWiseAttendance/getAttendetails", data)
                   .then(function (promise) {

                       
                       $scope.students = promise.studentAttendanceList;
                       $scope.presentCountgrid = $scope.students.length;
                       console.log($scope.students);

                       if (promise.AMC_logo != null) {
                           $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                       }
                       else {
                           $scope.imgname = logopath;
                       }

                       if ($scope.students.length > 0 && $scope.students != null) {
                           $scope.catreport = false;
                           $scope.catreport_grid = true;
                       }
                       else {
                           swal("No Records Found!");
                           $scope.catreport = true;
                           $scope.catreport_grid = false;
                       }
                   });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            //$state.reload();
            $scope.asmaY_Id = "";
            $scope.fromdate = "";
            $scope.submitted = false;
            $scope.catreport = true;
            $scope.catreport_grid = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });
    }
})();
