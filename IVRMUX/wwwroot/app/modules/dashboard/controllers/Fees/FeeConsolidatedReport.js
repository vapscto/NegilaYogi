(function () {
    'use strict';
    angular
        .module('app')
        .controller('FeeConsolidatedReportController', FeeConsolidatedReportController123)

    FeeConsolidatedReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function FeeConsolidatedReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {

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

        $scope.show_btn = false;
        $scope.show_cancel = false;
        $scope.show_grid = false;
        $scope.searc_button = true;
        $scope.searchString = "";
      
        $scope.loaddata = function () {
            $scope.report = false;
            var pageid = 2;
            $scope.gridview = false;
            $scope.stddetails = false;
            $scope.export_flag = false;
            apiService.getURI("FeeConsolidatedReport/getalldetails123", pageid).
                then(function (promise) {
                    $scope.acayyearbind = promise.acayear;
                    //$scope.sectiondrpre = promise.sectionlist;
                    $scope.clsdrpdown = promise.classlist;
                    $scope.arrlstinst1 = promise.fillinstallment;

                })
        }

        $scope.onselectclasssection = function () {
            $scope.result = false;
        }

        //adding section 
        $scope.onselectclass = function () {

          
            var data = {
                "ASMAY_Id": $scope.acayyearbindM,
                "ASMCL_Id": $scope.clsdrpdownM
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeInstallmentReport/classes", data).
                then(function (promise) {
                    $scope.result = false;
                    $scope.sectiondrpre = promise.fillsection;
                })
        }


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.students1, function (itm) {
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
            debugger;
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });

            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.showbutton = true;
            }
            else {
                $scope.showbutton = false;
            }
        }

        $scope.exportToExcel = function (table1) {
            debugger;
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {

                var exportHref = Excel.tableToExcel(table1, 'WireWorkbenchDataExport');
                
                $timeout(function () { location.href = exportHref; }, 100);
                $timeout(function () {
                    location.href = exportHref;
                }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
        }



        $scope.printData = function (printSectionId) {
            debugger;
            var pdss ='Table'
             
            
            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {

                var innerContents = document.getElementById(pdss).innerHTML;
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



      
        $scope.submitted = false;
        $scope.showreport = function () {
       
            if ($scope.myForm.$valid) {



                $scope.header_list = [];
                angular.forEach($scope.arrlstinst1, function (role) {
                    $scope.header_list.push(role);
                })


                var data = {
                    "acyrid": $scope.acayyearbindM,
                    "secidpass": $scope.sectiondrpreM,
                    "claspass": $scope.clsdrpdownM,
                    "flagrpt": $scope.Selectionrd,
                }
                apiService.create("FeeConsolidatedReport/getreport", data).
                    then(function (promise) {
                        $scope.result = true;

                        var installmentcount = $scope.arrlstinst1.length;

                        $scope.insarray = [{ name: "ReceiptNo" }, { name: "Date" }, { name: "ChequeNo" }, { name: "Paid" }];
                        $scope.columns1 = [{ field: "ReceiptNo", name: "ReceiptNo", title: "ReceiptNo" }, { field: "Date", name: "Date", title: "Date" }, { field: "ChequeNo", name: "ChequeNo", title: "ChequeNo" }, { field: "Paid", name: "Paid", title: "Amount" }];
                        $scope.newarray = [];
                        var finalarray = 0;
                        finalarray = Number(installmentcount)
                        for (var i = 0; i < $scope.header_list.length; i++) {
                            for (var j = 0; j < 4; j++) {
                                $scope.newarray.push({ name: $scope.insarray[j].name, id: $scope.header_list[i].ftI_Id, name1: "hema" + $scope.header_list[i].ftI_Id + $scope.insarray[j].name });

                            }

                        }
                        $scope.newarray1 = $scope.newarray;


                        $scope.students = promise.reportdatelist;
                        
                        angular.forEach($scope.clsdrpdown, function (ty) {
                            if (ty.asmcL_Id == $scope.clsdrpdownM) {
                                $scope.classsec = ty.asmcL_ClassName;
                            }
                        })

                        angular.forEach($scope.sectiondrpre, function (ty) {
                            if (ty.amsC_Id == $scope.sectiondrpreM) {
                                $scope.sec = ty.asmC_SectionName;
                            }
                        })


                        if (promise.reportdatelist != null && promise.reportdatelist != "" && promise.reportdatelist.length > 0) {
                            $scope.totcountfirst = promise.reportdatelist.length;

                            if ($scope.Selectionrd != "allr") {
                                $scope.reportdatelist = promise.reportdatelist;
                                $scope.gridview = true;
                                $scope.stddetails = false;
                                $scope.export_flag = true;
                            }
                            else {
                                $scope.gridview = false;
                                $scope.stddetails = true;
                                $scope.export_flag = false;
                                
                                var stu_list_new = [];
                                angular.forEach(promise.reportdatelist, function (op1) {
                                    var stu_id = op1.AMST_Id;
                                    var list_stu = [];
                                    angular.forEach(promise.reportdatelist, function (op2) {
                                        if (op2.AMST_Id == stu_id) {
                                            var coun = 0;
                                            angular.forEach($scope.header_list, function (op) {
                                                if (op2.FTI_Id == op.ftI_Id) {
                                                    list_stu.push({ FTI_Id: op2.FTI_Id, ReceiptNo: op2.ReceiptNo, Date: op2.Date, ChequeNo: op2.ChequeNo, Paid: op2.Paid })
                                                    coun += 1;
                                                }
                                                //else {
                                                //    list_stu.push({ FTI_Id: op2.FTI_Id, ReceiptNo: "-", Date: "-", ChequeNo: "-", Paid: "-"})
                                                //}
                                            })
                                            if (coun == 0) {
                                                list_stu.push({ FTI_Id: op2.FTI_Id, ReceiptNo: op2.ReceiptNo, Date: op2.Date, ChequeNo: op2.ChequeNo, Paid: op2.Paid })
                                            }

                                        }


                                    })
                                    if (stu_list_new.length == 0) {
                                        stu_list_new.push({ AMST_Id: stu_id, AMST_FirstName: op1.StudentName, ASMCL_ClassName: op1.ClassName, ASMC_SectionName: op1.SectionName, AMAY_RollNo: op1.AMAY_RollNo, Installment_Reports: list_stu });
                                    }
                                    else if (stu_list_new.length > 0) {
                                        var already_cnt = 0;
                                        angular.forEach(stu_list_new, function (td) {
                                            if (td.AMST_Id == stu_id) {
                                                already_cnt += 1;
                                            }
                                        })
                                        if (already_cnt == 0) {
                                            stu_list_new.push({ AMST_Id: stu_id, AMST_FirstName: op1.StudentName, ASMCL_ClassName: op1.ClassName, ASMC_SectionName: op1.SectionName, AMAY_RollNo: op1.AMAY_RollNo, Installment_Reports: list_stu });
                                        }
                                    }

                                })


                                 angular.forEach(stu_list_new, function (obj) {
                                    angular.forEach(obj.Installment_Reports, function (obj1) {
                                        angular.forEach($scope.newarray1, function (x) {
                                            if (x.id == obj1.FTI_Id) {
                                                obj[x.name1] = obj1[x.name];

                                            }
                                        })

                                    })
                                })

                                $scope.totcountfirstnew = stu_list_new.length;
                                $scope.data = stu_list_new;


                                //hema



                                //   $scope.mainsubhdr = [];
                                angular.forEach($scope.header_list, function (op) {
                                    op.columns = [];
                                    angular.forEach($scope.columns1, function (op123) {

                                        op.columns.push({ field: "hema" + op.ftI_Id + op123.field, name: "hema" + op.ftI_Id + op123.name, title: op123.title, width: 100 });

                                    })
                                    op.title = op.ftI_Name;
                                })

                                $scope.colarrayall = [{

                                    title: "SLNO",
                                    template: "<span class='row-number'></span>", width: 75

                                },
                                {
                                    name: 'AMAY_RollNo', field: 'AMAY_RollNo', title: 'Roll No', width: 200
                                },
                                {
                                    name: 'AMST_FirstName', field: 'AMST_FirstName', title: 'Student Name', width: 200
                                },
                                {
                                    name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: 100
                                },
                                {
                                    name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section', width: 100
                                }
                                ];


                                angular.forEach($scope.header_list, function (obj) {
                                    $scope.colarrayall.push(obj);
                                })
                                console.log($scope.header_list);

                                console.log($scope.colarrayall);

                                $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "," + " " + $scope.coptyright;

                                console.log($scope.txtdata);
                                $scope.aaaa = [{
                                    title: $scope.txtdata,
                                    columns: $scope.colarrayall
                                }]


                                console.log($scope.data);
                                var gridall;

                                $(document).ready(function () {
                                    initGridall();
                                    $(".k-grid-toolbar").prepend('<div class="gridTitle"><h4  style="color:white;" class="titlecolor">' + "Report For" + "   " + $scope.classsec + "-" + $scope.sec + '</h4></div>');
                                });

                                function initGridall() {
                                    gridall = $("#grid123").kendoGrid({
                                        toolbar: ["excel", "pdf"],
                                        excel: {
                                            fileName: "Kendo UI Grid Export.xlsx",
                                            proxyURL: "",
                                            filterable: true
                                        },
                                        pdf: {
                                            fileName: "Kendo UI Grid Export.pdf"
                                        },
                                        dataSource: {
                                            //type: "odata",
                                            //transport: {
                                            //    read: "https://demos.telerik.com/kendo-ui/service/Northwind.svc/Products"
                                            //},
                                            data: $scope.data
                                        },
                                        sortable: true,
                                        pageable: false,
                                        groupable: false,
                                        filterable: true,
                                        columnMenu: true,
                                        reorderable: true,
                                        resizable: true,
                                        columns: $scope.aaaa,
                                        dataBound: function () {
                                            var rows = this.items();
                                            $(rows).each(function () {
                                                var index = $(this).index() + 1;
                                                var rowLabel = $(this).find(".row-number");
                                                $(rowLabel).html(index);
                                            });
                                        }
                                    }).data("kendoGrid");
                                    gridall.setOptions({
                                        sortable: true
                                    });
                                }

                            }

                        }
                        
                     
                    })
            }
            else {
                $scope.submitted = true;

            }
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {

          //  $state.reload();
            $scope.loaddata();
        }

    }

})();
