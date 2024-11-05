(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeGroupWiseStudentReportController', FeeGroupWiseStudentReportController)
    FeeGroupWiseStudentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function FeeGroupWiseStudentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.searchString = "";
        $scope.print = false;
        $scope.obj = {};
        $scope.bulkdownload = false;

        $scope.colarrayall = [];
        $scope.tablediv = true;
        $scope.printgrn = true;
        $scope.tempaggary = [];

        $scope.loadbasicdata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("FeeGroupWiseStudentReport/getinitialfeedata/", pageid).then(function (promise) {

                $scope.yearlist = promise.yearList;
                // $scope.fee_group_list = promise.fee_Group_List;
                //  $scope.class_list = promise.class_List;
                $scope.section_list = promise.section_List;
                $scope.student_name_list = promise.student_Name_List;
                $scope.class_category_list = promise.class_Category_List;
                $scope.radio_val = "all";
                //   $scope.getVal();
            });
        }

        $scope.all_checktype = function () {

            var checkStatus = $scope.obj.feegroupselected;
            angular.forEach($scope.fee_group_list, function (itmtype) {
                itmtype.selected = checkStatus;
            });

        };
        $scope.togchkbxtype = function () {

            $scope.feegroupselected = $scope.fee_group_list.every(function (optionstype) {
                return optionstype.selected;
            });
        };
        $scope.isOptionsRequiredtype = function () {
            return !$scope.fee_group_list.some(function (options) {
                return options.selected;
            });
        };

        $scope.getVal = function () {

            $scope.asmaY_Id = "";
            $scope.fmG_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.stuD_Id = "";
            $scope.fmcC_Id = "";
            $scope.showgrid = false;

            if ($scope.radio_val == "student_wise" || $scope.radio_val == "admCat" || $scope.radio_val == "unmapped") {
                $scope.amntchecked = true;
            }
            else {
                $scope.amntchecked = false;
            }
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.printdatatable = [];
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sortBy = function (keyname) {

            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




        $scope.submitted = false;
        $scope.showreport = function () {
            $scope.printdatatable = [];
            $scope.all = false;

            if ($scope.myForm.$valid) {

                $scope.selectfeegroup = [];
                angular.forEach($scope.fee_group_list, function (aca) {
                    if (aca.selected === true) {
                        $scope.selectfeegroup.push({ fmG_Id: aca.fmG_Id });
                    }
                });


                var selectedVal = $scope.radio_val;
                if (selectedVal == "all") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.fee_group_list != "") {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        //"FMG_Id": $scope.fmG_Id,
                        selectfeegroup: $scope.selectfeegroup,
                    }
                }
                else if (selectedVal == "individual") {

                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined
                        //&& $scope.fmG_Id != "" && $scope.fmG_Id != undefined
                        && $scope.asmcL_Id != "" && $scope.asmcL_Id != undefined
                        && $scope.asmS_Id != "" && $scope.asmS_Id != undefined
                    ) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        selectfeegroup: $scope.selectfeegroup,
                        //"FMG_Id": $scope.fmG_Id,
                        "ASMCL_Id": $scope.asmcL_Id, //class
                        "ASMS_Id": $scope.asmS_Id,  //section
                        "Stud_Sel": $scope.stud_sel,
                    }
                }
                else if (selectedVal == "student_wise" || selectedVal == "Termwise") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined
                        && $scope.asmcL_Id != "" && $scope.asmcL_Id != undefined
                        && $scope.asmS_Id != "" && $scope.asmS_Id != undefined
                        && $scope.stuD_Id != "" && $scope.stuD_Id != undefined
                    ) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id, //class
                        "ASMS_Id": $scope.asmS_Id,  //section
                        "Stud_Id": $scope.stuD_Id,

                    }
                }
                else if (selectedVal == "admCat") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined
                        && $scope.asmcL_Id != "" && $scope.asmcL_Id != undefined
                        && $scope.asmS_Id != "" && $scope.asmS_Id != undefined
                        && $scope.fmcC_Id != "" && $scope.fmcC_Id != undefined
                    ) {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "ASMCL_Id": $scope.asmcL_Id, //class
                        "ASMS_Id": $scope.asmS_Id,  //section
                        "FMCC_Id": $scope.fmcC_Id, //category    

                    }
                }

                else if (selectedVal == "unmapped") {
                    if ($scope.asmaY_Id != "" && $scope.asmaY_Id != undefined && $scope.fee_group_list != "") {
                        $scope.validate = true;
                    }
                    var data = {
                        "radio_selected": $scope.radio_val,
                        "ASMAY_Id": $scope.asmaY_Id,
                        selectfeegroup: $scope.selectfeegroup,

                    }
                }

                if ($scope.validate) {
                    apiService.create("FeeGroupWiseStudentReport/", data).then(function (promise) {

                        $scope.validate = false;
                        if (promise.fhwR_searchdatalist != null && promise.fhwR_searchdatalist.length > 0) {

                            if (selectedVal == "Termwise") {

                                $scope.Studename = promise.fhwR_searchdatalist[0].StudentName;
                                $scope.classname = promise.fhwR_searchdatalist[0].classname;
                                $scope.sectionname = promise.fhwR_searchdatalist[0].sectionname;
                                $scope.termlist = promise.class_Category_List;
                                $scope.paiddatelist = promise.paiddatelist;
                                $scope.termwisedetails = promise.fhwR_searchdatalist;                           }
                            else {
                                $scope.searchdatalist = promise.fhwR_searchdatalist;
                                $scope.totcountfirst = promise.fhwR_searchdatalist.length;
                                $scope.showgrid = true;
                                $scope.classFlag = false;
                                $scope.sectionFlag = false;
                                $scope.rollNo = false;
                                $scope.feeGrp = false;
                                $scope.amtFlag = false;
                                $scope.print = true;
                                if ($scope.checked == true) {
                                    $scope.feeGrp = true;
                                }
                                if ($scope.amntchecked == true) {
                                    $scope.checked = true; $scope.feeGrp = true; $scope.amtFlag = true;
                                }

                                if ($scope.searchdatalist != null && $scope.searchdatalist.length > 0) {
                                    angular.forEach($scope.searchdatalist, function () {
                                        $scope.colarrayall = [{
                                            title: "SLNO",
                                            template: "<span class='row-number'></span>", width: "80px"

                                        },
                                        {
                                            name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Adm No', width: "130px"


                                        },
                                        {
                                            name: 'AMST_FirstName', field: 'AMST_FirstName', title: 'Student Name', width: "130px"
                                        },
                                        {
                                            name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: "150px"
                                        },
                                        {
                                            name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section', width: "100px"
                                        },
                                        {
                                            name: 'AMAY_RollNo', field: 'AMAY_RollNo', title: 'Roll No.', width: "100px"
                                        },
                                        {
                                            name: 'FMG_GroupName', field: 'FMG_GroupName', title: 'Fee Group', width: "100px"
                                        },
                                        {
                                            name: 'Amount', field: 'Amount', title: 'Amount', width: "100px"
                                        }


                                        ];
                                        $(document).ready(function () {
                                            initGridall();
                                        });

                                        function initGridall() {
                                            $('#gridall').empty();

                                            $("#gridall").kendoGrid({
                                                toolbar: ["excel"],
                                                excel: {
                                                    fileName: "Feegroup.xlsx",
                                                    proxyURL: "",
                                                    filterable: true,
                                                    allPages: true
                                                },
                                                pdf: {
                                                    avoidLinks: true,
                                                    landscape: true,
                                                    repeatHeaders: true,
                                                    fileName: "feegroup.pdf",
                                                    allPages: true
                                                },
                                                dataSource: {
                                                    data: $scope.searchdatalist,
                                                    pageSize: 10
                                                    //aggregate: $scope.tempaggary

                                                },

                                                sortable: true,
                                                pageable: true,
                                                groupable: true,
                                                filterable: true,
                                                columnMenu: true,
                                                reorderable: true,
                                                resizable: false,

                                                columns: $scope.colarrayall,
                                                dataBound: function () {
                                                    var pagenum = this.dataSource.page();
                                                    var pageitms = this.dataSource.pageSize();
                                                    var rows = this.items();
                                                    $(rows).each(function () {
                                                        var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                                        var rowLabel = $(this).find(".row-number");
                                                        $(rowLabel).html(index);
                                                    });
                                                }

                                            }).data("kendoGrid");
                                        }
                                    });

                                }
                            }
                        }
                        else {
                            $scope.showgrid = false;
                            $scope.print = false;
                            swal("No Records Found !!!");
                        }
                    })
                }
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        $scope.onselectyrdrpdwn = function (asmaY_Id) {
            //if (asmaY_Id != undefined && asmaY_Id != "" && asmaY_Id != null)
            // {
            $scope.yearid = asmaY_Id;
            var data = {
                "ASMAY_Id": asmaY_Id
            }

            apiService.create("FeeGroupWiseStudentReport/Getclass/", data).then(function (promise) {


                if (promise.class_List != null && promise.class_List.length > 0) {
                    $scope.class_list = promise.class_List;
                    $scope.fee_group_list = promise.fee_Group_List;

                }
                //else {
                //    swal("No Class Found");
                //}
                if (promise.fee_Group_List.length > 0) {
                    $scope.fee_group_list = promise.fee_Group_List;
                    $scope.class_list = promise.class_List;
                }
                //else {
                //    swal("No Group Found");
                //}
                //$scope.yearlist = promise.yearList;
                //$scope.fee_group_list = promise.fee_Group_List;
                //$scope.class_list = promise.class_List;
                //$scope.section_list = promise.section_List;
                //$scope.student_name_list = promise.student_Name_List;
                //$scope.class_category_list = promise.class_Category_List;
                //$scope.radio_val = "all";
                //$scope.getVal();
            });
            // }


        }






        $scope.onselectclsdrpdwn = function (asmcL_Id) {
            //if (asmaY_Id != undefined && asmaY_Id != "" && asmaY_Id != null)
            // {
            $scope.class = asmcL_Id;
            var data = {
                "ASMAY_Id": $scope.yearid,
                "ASMCL_Id": asmcL_Id
            }

            apiService.create("FeeGroupWiseStudentReport/GetSection/", data).then(function (promise) {

                //$scope.section_list = promise.Section_List;
                // if (promise.section_List.length > 0) {
                $scope.section_list = promise.section_List;

                //  }

                //$scope.yearlist = promise.yearList;
                //$scope.fee_group_list = promise.fee_Group_List;
                //$scope.class_list = promise.class_List;
                //$scope.section_list = promise.section_List;
                //$scope.student_name_list = promise.student_Name_List;
                //$scope.class_category_list = promise.class_Category_List;
                //$scope.radio_val = "all";
                //$scope.getVal();
            });
            // }


        }
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }

        }

        $scope.printData = function () {
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var innerContents = document.getElementById("printSectionId").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }
        }
        
        $scope.printDataterm = function () {
         
            var innerContents = document.getElementById("printdatatable1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                    '<link type="text/css" rel="stylesheet" href="css/style.css" />' +
                    '<link type="text/css" href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />' +
                    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
                popupWinindow.document.close();
           
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.filterValue, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all = $scope.searchdatalist.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }

        }


        $scope.onselectsectiondrpdwn = function (asmS_Id) {
            //if (asmaY_Id != undefined && asmaY_Id != "" && asmaY_Id != null)
            // {

            var data = {
                "ASMAY_Id": $scope.yearid,
                "ASMS_Id": asmS_Id,
                "ASMCL_Id": $scope.class
            }

            apiService.create("FeeGroupWiseStudentReport/GetStudent/", data).then(function (promise) {


                if (promise.student_Name_List.length > 0) {
                    $scope.student_name_list = promise.student_Name_List;

                }
                //else {
                //    swal("No Record Found");
                //}
                //$scope.yearlist = promise.yearList;
                //$scope.fee_group_list = promise.fee_Group_List;
                //$scope.class_list = promise.class_List;
                //$scope.section_list = promise.section_List;
                //$scope.student_name_list = promise.student_Name_List;
                //$scope.class_category_list = promise.class_Category_List;
                //$scope.radio_val = "all";
                //$scope.getVal();
            });
            // }


        }
    }
})();







