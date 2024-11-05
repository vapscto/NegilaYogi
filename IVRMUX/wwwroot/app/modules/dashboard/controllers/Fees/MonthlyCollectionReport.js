(function () {
    'use strict';
    angular
.module('app')
.controller('MonthlyCollectionReportController', MonthlyCollectionReportController)
    MonthlyCollectionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout', '$filter']
    function MonthlyCollectionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, $filter) {
        $scope.colarrayaggre = [];
        $scope.colarrayall = [
            {
            title: "SLNO",
            template: "<span class='row-numberind'></span>"

            }, { name: 'AdmNo', field: 'AdmNo', title: 'AdmNo' },
        { name: 'StudentName', field: 'StudentName', title: 'StudentName' },
        { name: 'ReceiptDetails', field: 'ReceiptDetails', title: 'ReceiptDetails' }
        ];

        $scope.search = "";
        $scope.IsHiddenup = true;
        $scope.columnsTest = [];
        $scope.checkboxchcked = [];
        $scope.sectioncheckboxchcked = [];
        $scope.checkallhrd1 = "";
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
       




        var grouporterm, autoreceipt, receiptformat, mergeinstallment;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        // $scope.Mi_Id = configsettings[0].mI_Id;
        var institutionid = configsettings[0].mI_Id;
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        if (configsettings.length > 0) {
            grouporterm = configsettings[0].fmC_GroupOrTermFlg;
            autoreceipt = configsettings[0].fmC_AutoReceiptFeeGroupFlag;
            receiptformat = configsettings[0].fmC_Receipt_Format;
            mergeinstallment = configsettings[0].fmC_RInstallmentsMergeFlag;//added by kiran
        }
      
        if (grouporterm == "T") {
            $scope.grouportername = "Term Name"
            $scope.term = true;
            $scope.groupterm = false;
        }
        else if (grouporterm == "G") {
            $scope.grouportername = "Group Name"
            $scope.groupterm = true;
            $scope.term = false;
        }

        //Added by Praveen Gouda
        $scope.binddatagrp2 = function () {
            $scope.checkallhrd1 = $scope.groupcount.every(function (role) {
                return role.fmT_Id_chk;
            });
        };

        $scope.hrdallcheck1 = function () {
            var toggleStatus1 = $scope.checkallhrd1;
            angular.forEach($scope.groupcount, function (itm) {
                itm.fmT_Id_chk = toggleStatus1;
            });
        }
        ///Ended

        var temp_grp_list = [];
        //binding the default values 

        $scope.loaddata = function () {          
   
            $scope.paginate1 = "paginate1";
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.custom_check_flag = true;
            $scope.group_check_flag = true;
            $scope.custom_check = "0";
            $scope.group_check = "0";
            $scope.export_flag = false;
            $scope.load_group_check();
            $scope.load_custom_check();
            $scope.chequedate = 0;
            var data = {
                "reporttype": grouporterm,
            }
            var pageid = 1;
            apiService.create("MonthlyCollectionReport/getalldetails", data).
        then(function (promise) {
            //$scope.arrlistchkgroup = promise.fillfeegroup;
            $scope.studentlst = promise.studentlist;
            $scope.columnsTest = [];
            $scope.onclickloaddata();

            $scope.groupcount = promise.fillmastergroup;
            if (grouporterm == "T") {
                angular.forEach(promise.customlist, function (tr) {
                    tr.fmgG_Id_chk = true;
                })
            }
            else if (grouporterm == "G") {
                angular.forEach(promise.customlist, function (tr) {
                    tr.fmgG_Id_chk1 = true;
                })
            }

            $scope.custom = promise.customlist;

            if (grouporterm == "T") {
                angular.forEach(promise.grouplist, function (tr) {
                    tr.fmG_Id_chk = true;
                })
            }
            else if (grouporterm == "G") {
                angular.forEach(promise.grouplist, function (tr) {
                    tr.fmG_Id_chk1 = true;
                })
            }


            $scope.group = promise.grouplist;
            temp_grp_list = promise.grouplist;

        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
           
        }




        $scope.load_custom_check = function () {

            if ($scope.custom_check == "1") {
                $scope.custom_check_flag = false;

            }
            else if ($scope.custom_check == "0") {
                $scope.custom_check_flag = true;

            }
        }
        $scope.load_group_check = function () {

            if ($scope.group_check == "1") {
                $scope.group_check_flag = false;
            }
            else if ($scope.group_check == "0") {
                $scope.group_check_flag = true;

            }
        }

        $scope.cheque_date = function () {
            if ($scope.chequedate == "1") {
                $scope.chequedate = 1;
            }
            else {
                $scope.chequedate = 0;
            }
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
            $scope.loaddata();
        }

        $scope.ShowHideup = function () {

            $scope.IsHiddenup = $scope.IsHiddenup ? false : true;
        }
        $scope.isOptionsRequired = function () {
            return !$scope.arrlistchkgroup.some(function (options) {
                return options.selected;
            });
        }
      
        $scope.onclickloaddata = function () {
            
           
            if ($scope.allindi === "all") {

                $scope.regnonamestd = true;              
            }
            else if ($scope.allindi === "indi") {

                $scope.regnonamestd = false;
             

            }

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        };

        $scope.onclickregnoname = function () {

            var data = {

                "regornamedetails": $scope.admnoname
            }

            apiService.create("MonthlyCollectionReport/getnameregno", data).
       then(function (promise) {

           $scope.studentlst = promise.studentlist;

       })
        };

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //by deepak

        $scope.exportToExcel = function (tableId) {
            
            if ($scope.students !== null && $scope.students.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please select records to be Exported");
            }
        }

        $scope.printdatatable = [];
       
        $scope.printData = function (printSectionId) {
            
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

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printdatatable = [];
            angular.forEach($scope.filterValue, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
            if ($scope.printdatatable.length != null) {
                $scope.export_flag = true;
            }
            else {
                $scope.export_flag = false;
            }
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            
           
            $scope.all = $scope.filterValue.every(function (itm)
            { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
            if ($scope.printdatatable.length > 0) {
                $scope.export_flag = true;
            }
            else {
                $scope.export_flag = false;
            }

        }

      

        // end by

        $scope.submitted = false;
        $scope.getreport = function () {
            
            $scope.colarrayall = [];
            $scope.colarrayaggre = [];
            $scope.kengrdtotary = [];
            $scope.colarrayall = [
            {
                title: "SLNO",
                template: "<span class='row-numberind'></span>"

            }, { name: 'AdmNo', field: 'AdmNo', title: 'AdmNo' },
        { name: 'StudentName', field: 'StudentName', title: 'StudentName' },
        { name: 'ReceiptDetails', field: 'ReceiptDetails', title: 'ReceiptDetails' }
            ];

            //$scope.colarrayall = "";
            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.custom, function (custom1) {
                    if (!!custom1.selected) $scope.albumNameArraycolumn1.push({
                        columnID1: custom1.fmgG_Id

                    });
                })

                $scope.albumNameArraycolumn2 = [];
                angular.forEach($scope.group, function (group) {
                    if (!!group.selected) $scope.albumNameArraycolumn2.push({
                        columnID2: group.fmG_Id

                    });
                })

                $scope.albumNameArraycolumn3 = [];
                angular.forEach($scope.groupcount, function (groupcount) {
                    if (!!groupcount.selected) $scope.albumNameArraycolumn3.push({
                        columnID3: groupcount.fmT_Id

                    });
                })



                var FMG_Ids = [];
                var FMT_Ids = [];
                if (grouporterm == "T") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                    angular.forEach($scope.groupcount, function (ty) {
                        if (ty.fmT_Id_chk) {
                            FMT_Ids.push(ty.fmT_Id);
                        }
                    })
                }
                else if (grouporterm == "G") {
                    angular.forEach($scope.group, function (ty) {
                        if (ty.fmG_Id_chk1) {
                            FMG_Ids.push(ty.fmG_Id);
                        }
                    })
                }


            if ($scope.allindi === 'all') {

                $scope.amsT_Id = '';

            }
            else {

                $scope.amsT_Id = $scope.amsT_Id;

            }
            var activeflag = $scope.leftstd123
            if (activeflag == true) {
                activeflag = 1;
            }
            else {
                activeflag = 0;
            }
            $scope.from_date = new Date($scope.fromdate).toDateString();
            $scope.to_date = new Date($scope.todate).toDateString();
            var data = {
                "Fromdate": $scope.from_date,
                "Todate": $scope.to_date,
                "allorindivflag": $scope.allindi,
                "idamstid": $scope.amsT_Id,
                "flagleft": activeflag,
                "customflag": $scope.custom_check,
                "groupflag": $scope.group_check,
                "studenttype":$scope.status,
                FMG_Ids: FMG_Ids,
                FMT_Ids: FMT_Ids,
                "term_group": grouporterm,
                "chequedate": $scope.chequedate
            }

            $scope.getTotalgrp = function (int) {
                var total = 0;
                angular.forEach($scope.students, function (e) {
                    total += e.Total;
                });
                return total;
            };

            apiService.create("MonthlyCollectionReport/getreport", data).
      then(function (promise) {
          
          if (promise.alldatagridreport.length > 0 ) {
              $scope.students = promise.alldatagridreport;
              //$scope.colarrayall = "";
              $scope.kengrdtotary = $scope.students;
              console.log($scope.students);
              console.log($scope.kengrdtotary);
              $scope.totcountfirst = promise.alldatagridreport.length;
              $scope.columnsTest = promise.alldatagridreportheads;
              console.log($scope.columnsTest);


              //$scope.colarrayaggre = [];
              //$scope.colarrayall = [

              //    {

              //        title: "SLNO",
              //        template: "<span class='row-numberind'></span>"

              //    }, { name: 'admno', field: 'admno', title: 'admno' },
              //{ name: 'Name', field: 'Name', title: 'Name' },
              //{ name: 'redate', field: 'redate', title: 'redate' }
              //];




              angular.forEach($scope.columnsTest, function (objj) {
                  var strstr = '["' + objj.monthyear + '"]';
                  $scope.colarrayaggre.push({ field: strstr, name: objj.monthyear, title: objj.monthyear, aggregate: "sum" });
                  $scope.colarrayall.push({
                      field: strstr, name: objj.monthyear, title: objj.monthyear, aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                      groupFooterTemplate: "Sum: #=sum#"
                  });

              })
              $scope.colarrayall.push({
                  name: 'Total', field: 'Total', title: 'Total', aggregates: ["sum"], footerTemplate: "Sum: #=sum#",
                  groupFooterTemplate: "Sum: #=sum#"
              });

              $scope.colarrayaggre.push({ name: 'Total', field: 'Total', title: 'Total', aggregate: "sum" });
              angular.forEach($scope.colarrayall, function (widobj) {
                  widobj.width = 100;
              })
              
              console.log($scope.colarrayall);           
              $scope.tot = $scope.getTotalgrp(promise.alldatagridreport);
              $scope.lower_grid = true;


              $scope.txtdata = "DATE :" + " " + $filter('date')(new Date(), 'dd-MM-yyyy') + "  " + "," + " " + "USERNAME :" + " " + $scope.usrname + "," + " " + $scope.coptyright;
              console.log($scope.txtdata);
              $scope.aaaa = [{
                  title: $scope.txtdata,
                  columns: $scope.colarrayall
              }]


              var gridind;

              $(document).ready(function () {
                  initGridind();
              });
              function initGridind() {
                  gridind = $("#gridind").kendoGrid({
                      toolbar: ["excel", "pdf"],
                     
                      excel: {
                          fileName: "inddExport.xlsx",
                          //allPages: true,
                          filterable: true
                      },
                      pdf: {
                          fileName: "inddExport.pdf",
                          //allPages: true,
                          filterable: true
                      },
                      dataSource: {

                          data: $scope.kengrdtotary,
                          aggregate: $scope.colarrayaggre
                          //pageSize: 10
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
                              var rowLabel = $(this).find(".row-numberind");
                              $(rowLabel).html(index);
                          });
                      }
                  }).data("kendoGrid");
              }
          }
          else {
              swal("Record Not Found !!");
              $scope.lower_grid = false;
          }

      })
         
            }
            else {
                $scope.submitted = true;
            }

        };


        $scope.get_groups = function () {
            var FMGG_Ids = [];
            if (grouporterm == "T") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }
            else if (grouporterm == "G") {
                angular.forEach($scope.custom, function (ty) {
                    if (ty.fmgG_Id_chk1) {
                        FMGG_Ids.push(ty.fmgG_Id);
                    }
                })
            }

            if (FMGG_Ids.length > 0) {
                var data = {
                    "reporttype": grouporterm,
                    FMGG_Ids: FMGG_Ids
                }



                apiService.create("FeeCollectionReport/get_groups", data).
                 then(function (promise) {


                     if (grouporterm == "T") {
                         angular.forEach(promise.grouplist, function (tr) {
                             tr.fmG_Id_chk = true;
                         })
                     }
                     else if (grouporterm == "G") {
                         angular.forEach(promise.grouplist, function (tr) {
                             tr.fmG_Id_chk1 = true;
                         })
                     }
                     $scope.group = promise.grouplist;
                 });
            }
            else if (FMGG_Ids.length == 0) {
                //$scope.group = temp_grp_list;
                $scope.group = [];
            }


        }

    }
}());