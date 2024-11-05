
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdmissionRegisterController', AdmissionRegisterController)

    AdmissionRegisterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function AdmissionRegisterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {


        $scope.tadprint = false;
        $scope.albumNameArraycolumn = [];
        //TO  GEt The Values iN Grid
        $scope.tablev = false;
        $scope.exp = false;

        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];

        //$scope.userPrivileges = "";
        //var pageid = $stateParams.pageId;

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.objj = {};
        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !==null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

      //  $scope.imgname = logopath;


        $scope.order = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa

        }




        $scope.Toggle_header1 = function () {
            var toggleStatus1 = $scope.all1;
            angular.forEach($scope.newuser2, function (itm) {
                itm.selected = toggleStatus1;

            });
        }



        $scope.Toggle_header2 = function () {
            var toggleStatus2 = $scope.all2;
            angular.forEach($scope.newuser3, function (itm) {
                itm.selected = toggleStatus2;

            });
        }



        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
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
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }

        }

        $scope.BindData = function () {
            apiService.getDATA("AdmissionRegister/Getdetails").

                then(function (promise) {
                    $scope.newuser1 = promise.yearList;
                    $scope.newuser2 = promise.classList;
                    $scope.newuser3 = promise.sectionList;
                    $scope.categoryDropdown = promise.category_list;

                    $scope.categoryflag = promise.categoryflag;
                   
                    $scope.currentPage = 1;
                    //$scope.itemsPerPage = 10;

                })

        };
        $scope.isOptionsRequired = function () {
            return !$scope.newuser2.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequired1 = function () {
            return !$scope.newuser3.some(function (options1) {
                return options1.selected;
            });
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cleardata = function () {
            // $state.reload();
            $scope.BindData();
            $scope.asmaY_Id = "";

            $scope.exp_excel_flag = true;
            $scope.print_flag = true;
            $scope.catreport = true;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.all1 = "";
            $scope.all2 = "";
        }


        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];
        $scope.addColumn1 = function (role) {
            $scope.all1 = $scope.newuser2.every(function (itm) { return itm.selected; });

            if (role.selected === true) {

                $scope.albumNameArraycolumn.push(role);

                var newCol = { id: role.ivrM_CLM_NAME, checked: true, value: role.ivrM_CLM_PAR }

                $scope.columnsTest.push(newCol);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role), 1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }



        };


        $scope.addColumn2 = function (role1) {
            $scope.all2 = $scope.newuser3.every(function (itm) { return itm.selected; });

            if (role1.selected == true) {

                $scope.albumNameArraycolumn.push(role1);

                var newCol = { id: role1.ivrM_CLM_NAME, checked: true, value: role1.ivrM_CLM_PAR }

                $scope.columnsTest.push(newCol);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
        };

        $scope.students = [];
        $scope.catreport = true;
        $scope.submitted = false;

        $scope.ShowReport = function () {
            $scope.printstudents = [];
            $scope.searchValue = "";
            $scope.columnsTest1 = [];
            $scope.columnsTest2 = [];
            $scope.columnsTest = [];
            if ($scope.myForm.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.newuser2, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })

                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.newuser3, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push(role);
                })
                //var AMC_Id = 0
                //if ($scope.amC_Id != 'All') {
                //    AMC_Id = $scope.amC_Id
                //}
                var AMC_Id = 0
                if ($scope.objj.amC_Id != 'All') {
                    AMC_Id = $scope.objj.amC_Id
                }

                var data = {
                    "MI_Id": 2,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TempararyArrayListcoloumn": $scope.albumNameArraycolumn,
                    "TempararyArrayListclass": $scope.albumNameArray,
                    "AMST_SOL": $scope.AttendanceType,
                    "AMC_Id": AMC_Id

                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("AdmissionRegister/Getdetailsreport", data).
                    then(function (promise) {

                        for (var i = 0; i < promise.searchstudentDetails.length; i++) {
                            if ((promise.searchstudentDetails[i].AMST_FatherName === "") && (promise.searchstudentDetails[i].AMST_MotherName == "=")) {

                                promise.searchstudentDetails[i].AMST_FatherName = "Not Available";
                                promise.searchstudentDetails[i].AMST_MotherName = "Not Available";
                            }

                            if (promise.searchstudentDetails[i].AMST_MotherName === " ") {
                                promise.searchstudentDetails[i].AMST_MotherName = "Not Available";
                            }
                            if (promise.searchstudentDetails[i].AMST_FatherName === " ") {
                                promise.searchstudentDetails[i].AMST_FatherName = "Not Available";
                            }
                        }


                        $scope.students = promise.searchstudentDetails;
                        $scope.presentCountgrid = $scope.students.length;
                        console.log($scope.students);


                        if (promise.AMC_logo != null) {
                            $scope.imgname = promise.AMC_logo[0].amC_FilePath;
                        }
                        else {
                            $scope.imgname = logopath;
                        }

                        angular.forEach($scope.students, function (objectt) {
                            var string = "";
                            if (objectt.StudentsName !== undefined) {
                                string = objectt.StudentsName;
                                objectt.StudentsName = string.replace(/  +/g, ' ');
                            }
                            if (objectt.AMST_PerAdd3 !== undefined) {
                                string = objectt.AMST_PerAdd3;
                                objectt.AMST_PerAdd3 = string.replace(/  +/g, ' ');

                            }
                            if (objectt.AMST_PerArea !== undefined) {
                                string = objectt.AMST_PerArea;
                                objectt.AMST_PerArea = string.replace(/  +/g, ' ');
                            }
                        });



                        $scope.columnsTest1 = promise.tempararyArrayListcoloumn;

                        if ($scope.students.length > 0 && $scope.students !== null && $scope.students !== undefined) {

                            var id = 0;
                            angular.forEach($scope.students, function (dd) {
                                id = id + 1;
                                dd.SNo = id;
                            });

                            console.log($scope.students);

                            $scope.columnsTest2.push({ ivrM_CLM_NAME: "SNo", ivrM_CLM_PAR: "SNo" });

                            angular.forEach($scope.columnsTest1, function (ddd) {
                                $scope.columnsTest2.push(ddd);
                            });

                            angular.forEach($scope.columnsTest2, function (qwe) {
                                if (qwe.ivrM_CLM_PAR === "AMST_PhotoName") {
                                    qwe.field = qwe.ivrM_CLM_PAR;
                                    qwe.title = qwe.ivrM_CLM_NAME;
                                    qwe.width = 120;
                                } else {
                                    qwe.field = qwe.ivrM_CLM_PAR;
                                    qwe.title = qwe.ivrM_CLM_NAME;
                                    qwe.width = 120;
                                }
                            });

                            angular.forEach($scope.columnsTest2, function (wer) {
                                $scope.columnsTest.push(wer);
                            });

                            var gridall;

                            $(document).ready(function () {
                                initGridall();
                            });

                            function initGridall() {
                                $('#gridlst').empty();
                                gridall = $("#gridlst").kendoGrid({
                                    toolbar: ["excel"],
                                    excel: {
                                        fileName: "Student Admission Register Report.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        fileName: "Student Admission Register Report.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.students,
                                        pageSize: 10
                                    },
                                    sortable: true,
                                    //pageable: false,
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
                            //kendo end

                            $scope.catreport = false;
                            $scope.exp_excel_flag = false;
                            $scope.print_flag = false;
                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport = true;
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.getclass = function () {
            var acedamicId = $scope.asmaY_Id;
            var amcId = 0;
            if ($scope.objj.amC_Id != null && $scope.objj.amC_Id != 0 && $scope.objj.amC_Id != undefined) {
                amcId = $scope.objj.amC_Id;
            }


            var data = {
                "ASMAY_Id": acedamicId,
                "AMC_Id": amcId
            };
            apiService.create("AdmissionRegister/getclass", data).then(function (promise) {
                $scope.newuser2 = promise.classList;
            });
        };

        $scope.exportToExcel = function (tableId) {
            if ($scope.printstudents !== null && $scope.printstudents.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
                //$state.reload();
            }
            else {
                swal("Please Select Records to be Exported");
            }

        }

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

        $scope.cler = function () {
            $scope.asmaY_Id = "";
            $scope.FromDate = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
        }

        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "AMST_FirstName": $scope.search,
                //"AMST_FirstName": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("AdmissionRegister/1", data).
                then(function (promise) {

                    swal("Searched Successfully");
                })
        }

        $scope.filterValue = function () {

        }
    }

})();