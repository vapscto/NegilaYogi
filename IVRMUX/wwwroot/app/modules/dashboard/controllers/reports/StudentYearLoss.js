(function () {
    'use strict';
    angular.module('app').controller('StudentYearLossController', StudentYearLossController)
    StudentYearLossController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function StudentYearLossController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.printdatatable = [];
        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            var pageid = 1;

            apiService.getURI("StudentYearLoss/getalldetails", pageid).then(function (promise) {
                $scope.yearlst = promise.fillyear;
                $scope.arrclasslist = promise.fillclass;
                $scope.arrseclist = promise.fillsection;
                $scope.columnsTest = [];

                $scope.headertest = [{ id: 'AMST_FirstName', checked: true, value: 'Student Name' },
                { id: 'AMST_AdmNo', checked: true, value: 'Adm. No.' },
                { id: 'ASMCL_ClassName', checked: true, value: 'Class' },
                { id: 'ASMC_SectionName', checked: true, value: 'Section' },
                { id: 'AMAY_RollNo', checked: true, value: 'Roll No.' },
                { id: 'AMST_RegistrationNo', checked: true, value: 'Reg. No.' },
                { id: 'AMST_Date', checked: true, value: 'Date Of Admission' },
                { id: 'AMST_DOB', checked: true, value: 'Date Of Birth' },
                { id: 'AMST_DOB_Words', checked: true, value: 'DOB(Words)' },
                { id: 'AMST_FatherName', checked: true, value: 'Father Name' },
                { id: 'AMST_MotherName', checked: true, value: 'Mother Name' },
                { id: 'AMST_Sex', checked: true, value: 'Sex' },
                { id: 'AMST_FatherMobleNo', checked: true, value: 'Phone Number' },
                { id: 'AMST_PerAdd3', checked: true, value: 'Permanent Address' },
                { id: 'AMST_MobileNo', checked: true, value: 'Mobile Number' },
                { id: 'AMST_emailId', checked: true, value: 'Email ID' },
                { id: 'AMST_BloodGroup', checked: true, value: 'Blood Group' },
                { id: 'IMC_CasteName', checked: true, value: 'Caste' },
                { id: 'AMC_Name', checked: true, value: 'Category' },
                { id: 'AMST_ConCity', checked: true, value: 'Present Address' }
                ]
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (obj.AMST_FirstName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_AdmNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMAY_RollNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_DOB_Words).indexOf($scope.searchValue) >= 0 || (obj.AMST_FatherName).indexOf($scope.searchValue) >= 0 || (obj.AMST_MotherName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_FatherMobleNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0 || (obj.AMST_emailId).indexOf($scope.searchValue) >= 0 || (obj.AMST_BloodGroup).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.exportToExcel = function (tableId) {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };

        $scope.toggleAll = function () {
            $scope.printdatatable = [];
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
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.printdatatable = [];
            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.clssec = true;
        $scope.clssec = true;
        $scope.albumNameArraycolumn = [];

        $scope.addColumn = function (role) {
            $scope.all = $scope.headertest.every(function (itm) { return itm.selected; });
            if (role.selected === true) {
                $scope.albumNameArraycolumn.push(role);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
            }
        };


        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.onclickloaddata = function () {
            if ($scope.TC_allorind == "all") {
                $scope.clssec = true;
                $scope.clssec = true;
                $scope.cls_sec_flag = false;
                $scope.gridflag = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
            else if ($scope.TC_allorind === "indi") {
                $scope.clssec = false;
                $scope.clssec = false;
                $scope.cls_sec_flag = true;
                $scope.gridflag = false;
                $scope.submitted = false;
                $scope.myForm.$setPristine();
                $scope.myForm.$setUntouched();
            }
        };

        $scope.isOptionsRequired = function () {
            return !$scope.headertest.some(function (options) {
                return options.selected;
            });
        };

        $scope.submitted = false;
        $scope.ShowReport = function (TC_allorind, Tc_teporper, headertest) {
            $scope.printdatatable = [];
            $scope.searchValue = "";

            if ($scope.myForm.$valid) {

                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.headertest, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push({
                        columnID: role.id,
                        columnName: role.value
                    });
                });


                $scope.columnsTest = $scope.albumNameArraycolumn;
                $scope.columnsTest2 = [];
                angular.forEach($scope.columnsTest, function (ddd) {
                    $scope.columnsTest2.push(ddd);
                });

                angular.forEach($scope.columnsTest2, function (qwe) {
                    qwe.field = qwe.columnID;
                    qwe.title = qwe.columnName;
                    qwe.width = 120;
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY,
                    "tcperortemp": Tc_teporper,
                    "ASMCL_Id": $scope.ASMCL,
                    "ASMC_Id": $scope.ASMC,
                    "tcallorindi": TC_allorind,
                    "TempararyArrayheadList": $scope.albumNameArraycolumn
                };
                apiService.create("StudentYearLoss/Getreportdetails", data).then(function (promise) {
                    $scope.students = promise.alldatagridreport;
                    $scope.presentCountgrid = $scope.students.length;

                    if ($scope.format === "1") {
                        var gridall;
                        $(document).ready(function () {
                            initGridall();
                        });

                        function initGridall() {
                            $('#gridlst').empty();
                            gridall = $("#gridlst").kendoGrid({
                                toolbar: ["excel", "pdf"],
                                excel: {
                                    fileName: "Student Year Loss Report.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                pdf: {
                                    fileName: "Student Year Loss Report.pdf",
                                    allPages: true
                                },
                                dataSource: {
                                    data: $scope.students,
                                    pageSize: 10
                                },
                                sortable: true,
                                pageable: true,
                                groupable: false,
                                filterable: true,
                                columnMenu: true,
                                reorderable: true,
                                resizable: true,
                                columns: $scope.columnsTest2,
                                dataBound: function () {
                                    var pagenum = this.dataSource.page();
                                    var pageitms = this.dataSource.pageSize()
                                    var rows = this.items();
                                    $(rows).each(function () {
                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                        var rowLabel = $(this).find(".row-numberind");
                                        $(rowLabel).html(index);
                                    });
                                }

                            }).data("kendoGrid");
                        }
                        $scope.showbutton = false;
                    }
                    else {
                        angular.forEach($scope.students, function (objectt) {
                            if (objectt.AMST_FirstName.length > 0) {
                                var string = objectt.AMST_FirstName
                                objectt.AMST_FirstName = string.replace(/  +/g, ' ');
                            }
                        })
                        $scope.columnsTest = promise.tempararyArrayheadList;
                        $scope.print_flag = false;
                        $scope.excel_flag = false;
                    }
                    $scope.gridflag = true;

                    if (promise.alldatagridreport === null || promise.alldatagridreport.length == 0) {
                        swal(" Record Not Found");
                        $scope.gridflag = false;
                        $scope.print_flag = true;
                        $scope.excel_flag = true;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Toggle_header = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.headertest, function (itm) {
                itm.selected = toggleStatus;
            });
        };

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
        };

        $("#btnExport").click(function (e) {
            window.open('data:application/vnd.ms-excel,' + $('#Table').html());
            e.preventDefault();
        });

        $scope.Clear_Details = function () {
            $state.reload();
        };

        $scope.Onchagneformat = function () {
            $scope.gridflag = false;
            $scope.excel_flag = true;
            $scope.print_flag = true;
            $scope.students = [];
        };
    }
})();

