(function () {
    'use strict';
    angular
.module('app')
.controller('CollegeHeadWiseCollectionReportController', CollegeHeadWiseCollectionReportController)
    CollegeHeadWiseCollectionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter', 'superCache', 'uiGridConstants']
    function CollegeHeadWiseCollectionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter, superCache, uiGridConstants) {

        $scope.colarrayaggre = [];
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        
        $scope.imgname = logopath;
        $scope.checkallhrd = true;
        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.arrlistchkgroup, function (itm) {
                itm.selected = toggleStatus1;
            });
        }
        $scope.colarray = [{

            title: "SLNO",
            template: "<span class='row-numberind'></span>"

        },
        { name: 'StudentName', field: 'StudentName', width: '100px', title: 'Student Name' }, { name: 'AMCO_CourseName', field: 'AMCO_CourseName', title: 'Course' },
                 { name: 'AMB_BranchName', field: 'AMB_BranchName', width: '100px', title: 'Branch' },
                 { name: 'AMSE_SEMName', field: 'AMSE_SEMName', width: '100px', title: 'Semester' },
                 { name: 'ACMS_SectionName', field: 'ACMS_SectionName', width: '100px', title: 'Semester' },
                   { name: 'FYP_ReceiptNo', field: 'FYP_ReceiptNo', width: '100px', title: 'Receiptno' }

        ];
      

        $scope.gridOptions = {
            enableRowSelection: true,
            enableSelectAll: true,
            showGridFooter: false,
            showColumnFooter: false,
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [10, 20, 30],
            paginationPageSize: 20,
            enableGridMenu: false,
            columnDefs: $scope.colarray,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
                }
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[2].getAggregationValue();
            }

        };

        $scope.gridOptionsall = {
            enableRowSelection: true,
            enableSelectAll: true,
            showGridFooter: false,
            showColumnFooter: true,
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [10, 20, 30],
            paginationPageSize: 20,
            enableGridMenu: false,
          //  columnDefs: $scope.colarrayall,
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            },
            gridMenuCustomItems: [{
                title: 'Export all data as EXCEL',
                action: function ($event) {
                    exportUiGridService.exportToExcel('sheet 1', $scope.gridApi, 'selected', 'selected');
                }
            }
            ],
            getFooterValue: function () {
                return $scope.gridApi.grid.columns[2].getAggregationValue();
            }

        };




        
       
        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        if (configsettings.length > 0) {
            var grouporterm = configsettings[0].fmC_GroupOrTermFlg;
        }

        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.tabel12 = false;
            $scope.tabel123 = false;

            var data = {
                "configset": grouporterm,
            }

            apiService.create("CollegeHeadWiseCollectionReport/getalldetails", data).
        then(function (promise) {
            $scope.yearlst = promise.yearlst;
            $scope.arrlistchkgroup = promise.grouplist;
            $scope.sectioncount = promise.fillsection;
            angular.forEach($scope.arrlistchkgroup, function (tr) {
                tr.selected = true;
            })
            $scope.seclist = promise.fillfeehead;
            //$scope.onclickloaddata();
            //$scope.binddatagrp();


            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        })
        }


        $scope.onselectyear = function (obj) {
            
            var data = {
                "ASMAY_Id": obj.ASMAY,

            }


            apiService.create("CollegeHeadWiseCollectionReport/get_courses", data).
                then(function (promise) {

                    $scope.coursecount = promise.courselist;
                    

                })

        };

        $scope.get_branches = function (obj) {
            
            var AMCO_Ids = [];
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })
            var data = {
                "ASMAY_Id": $scope.obj.ASMAY,
                AMCO_Ids: AMCO_Ids,

            }

            apiService.create("CollegeHeadWiseCollectionReport/get_branches", data).
                then(function (promise) {
                    $scope.branchcount = promise.branchlist;
                    angular.forEach($scope.branchcount, function (fy) {
                        fy.selectedbranch = true;
                    })
                    $scope.binddatagrp();
                })

        };

        $scope.get_semisters = function () {
            
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
            }


            apiService.create("CollegeHeadWiseCollectionReport/get_semisters", data).
                then(function (promise) {

                    $scope.semestercount = promise.semisterlist;


                })

        };



        $scope.binddatagrp = function (arrlistchkgroup) {
            
            $scope.albumNameArray = [];
            angular.forEach($scope.arrlistchkgroup, function (role) {
                if (!!role.selected) $scope.albumNameArray.push({ FMG_Id: role.fmG_Id });

            })
            var data = {

                TempararyArrayList: $scope.albumNameArray,
              //  Group_All: $scope.group_check,
                "ASMAY_Id": $scope.obj.ASMAY,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CollegeHeadWiseCollectionReport/getgroupmappedheads", data).
        then(function (promise) {
            if (promise.alldata != null) {
                
                // $scope.grigview1 = true;
                $scope.arrlistchkhead = promise.alldata;
               
                console.log($scope.arrlistchkhead);
                $scope.grand_total();
                if ($scope.temp_array_total.length > 0) {
                    $scope.total_row = true;
                }
                else {
                    $scope.total_row = false;
                }
                // $scope.temp_headlist = promise.alldata;
            }
            if ($scope.group_check == "1" && (promise.alldata == null || promise.alldata == "") && $scope.albumNameArray != null && $scope.albumNameArray != "") {
                // swal("No Head Is Mapped to This Group");
            }
        })
        }

        $scope.grand_total = function () {
            $scope.temp_array_total = [];

            //$scope.iddd = $scope.arrlistchk[i].asmcL_ID;
            for (var j = 0; j < $scope.arrlistchkhead.length; j++) {

                var total_X = 0;
                angular.forEach($scope.student_list, function (e) {
                    // if(e[$scope.arrlistchkhead[j].fmH_FeeName]=$scope.arrlistchkhead[j].fmH_FeeName)
                    total_X += e[$scope.arrlistchkhead[j].fmH_FeeName];

                });
                var newcol_tot = { id: $scope.arrlistchkhead[j].fmH_Id, total: total_X, value: $scope.arrlistchkhead[j].fmH_FeeName }
                $scope.temp_array_total.push(newcol_tot);
            }

            var tottemprow = 0;
            for (var i = 0; i < $scope.temp_array_total.length; i++) {
                tottemprow = $scope.temp_array_total[i].total + tottemprow;
            }
            $scope.totrow = tottemprow;
        }


        $scope.ShowReport = function () {
            $('#gridind').empty();
            $('.k-grid-toolbar').empty();
            var AMCO_Ids = [];
            var AMB_Ids = [];
            var FMG_Ids = [];
            // var AMSE_Ids = [];



            $scope.colarrayaggre = [];
            $scope.kengrdtotary = [];
            $scope.colarray = [];
            $scope.colarray = [{

                title: "SLNO",
                template: "<span class='row-numberind'></span>"

            },    { name: 'StudentName', field: 'StudentName', width: '100px',title: 'Student Name' }, { name: 'AMCO_CourseName', field: 'AMCO_CourseName', title: 'Course' },
                 { name: 'AMB_BranchName', field: 'AMB_BranchName', width: '100px', title: 'Branch' },
                 { name: 'AMSE_SEMName', field: 'AMSE_SEMName', width: '100px', title: 'Semester' },
                 { name: 'ACMS_SectionName', field: 'ACMS_SectionName', width: '100px', title: 'Semester' },
                   { name: 'FYP_ReceiptNo', field: 'FYP_ReceiptNo', width: '100px', title: 'Receiptno' }
            ];
            $scope.gridOptions.data = [];
            $scope.gridOptionsall.data = [];


            angular.forEach($scope.arrlistchkgroup, function (ty) {
                if (ty.selected) {
                    FMG_Ids.push(ty.fmG_Id);
                }
            })
            angular.forEach($scope.coursecount, function (ty) {
                if (ty.selectedcourse) {
                    AMCO_Ids.push(ty.amcO_Id);
                }
            })

            angular.forEach($scope.branchcount, function (ty) {
                if (ty.selectedbranch) {
                    AMB_Ids.push(ty.amB_Id);
                }
            })

            if ($scope.myForm.$valid) {

                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();


             

                var data = {

                    "ASMAY_Id": $scope.obj.ASMAY,
                        "radioval": $scope.result,
                        "reporttype": $scope.rpttyp,
                        "studenttype": $scope.status,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                        "AMSC_Id": $scope.asmC_Id,
                        AMCO_Ids: AMCO_Ids,
                        AMB_Ids: AMB_Ids,
                        FMG_Ids: FMG_Ids,
                        "active": $scope.active,
                        "deactive": $scope.deactive,
                        "left": $scope.left,
                    }



                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    $scope.getTotalstd = function (int) {
                        var total = 0;
                        angular.forEach($scope.students, function (e) {
                            total += e.totalbalance;
                        });
                        return total;
                    };

                    $scope.getTotalgrp = function (int) {
                        var total = 0;
                        angular.forEach($scope.groups, function (e) {
                            total += e.totalbalance;
                        });
                        return total;
                    };
                    $scope.getTotalhd = function (int) {
                        var total = 0;
                        angular.forEach($scope.heads, function (e) {
                            total += e.totalbalance;
                        });
                        return total;
                    };
                    $scope.getTotalcls = function (int) {
                        var total = 0;
                        angular.forEach($scope.classes, function (e) {
                            total += e.totalbalance;
                        });
                        return total;
                    };


                    apiService.create("CollegeHeadWiseCollectionReport/radiobtndata", data).
                   then(function (promise) {
                       
                       $scope.Grid_View = true;
                       $scope.mI_ID = promise.mI_ID;
                       $scope.reportdetails = promise.searchstudentDetails;
                       $scope.FromDate = new Date();
                       $scope.dueduration = promise.month;



                       $scope.temporary_list = promise.alldata;
                       $scope.student_list = promise.alldata;


                       $scope.table_all = false;
                       $scope.table = true;
                       $scope.testary = [];
                       $scope.temp_array_total = [];


                       $scope.temp_array = [];
                       
                       angular.forEach($scope.arrlistchkhead, function (objj) {

                           var strstr = '["' + objj.fmH_FeeName + '"]';
                           $scope.colarrayaggre.push({ field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregate: "sum" });
                           $scope.colarray.push({
                               field: strstr, name: objj.fmH_FeeName, title: objj.fmH_FeeName, aggregates: ["sum"], footerTemplate: "#=sum#",
                               groupFooterTemplate: "#=sum#"
                           });

                       })
                       $scope.all_grand_total = 0;
                       for (var z = 0; z < $scope.temp_array_total.length; z++) {
                           $scope.all_grand_total += $scope.temp_array_total[z].total;
                       }
                       $scope.colarray.push({
                           name: 'total_side', field: 'total_side', title: 'Total', aggregates: ["sum"], footerTemplate: "Total: #=sum#",
                           groupFooterTemplate: "Total: #=sum#"
                       });

                       $scope.colarrayaggre.push({ name: 'total_side', field: 'total_side', title: 'Total', aggregate: "sum" });
                       angular.forEach($scope.colarray, function (widobj) {
                           widobj.width = 250;
                       })
                       
                       for (var x = 0; x < $scope.student_list.length; x++) {
                           var total_y = 0;
                           for (var y = 0; y < $scope.arrlistchkhead.length; y++) {
                               var column = $scope.arrlistchkhead[y].fmH_FeeName;
                               total_y += $scope.student_list[x][column];
                               $scope.student_list[x].total_side = total_y;
                           }
                           $scope.temp_array.push({ total_side: total_y, date: $scope.student_list[x].Date, array: $scope.student_list[x] });
                           $scope.gridOptions.data.push($scope.student_list[x]);
                          
                       }
                       $scope.kengrdtotary = $scope.gridOptions.data;
                       
                       var obj = [];
                       var indi_totals = [];
                       for (var j = 0; j < $scope.arrlistchkhead.length; j++) {

                           var total_X = 0;
                           angular.forEach($scope.student_list, function (e) {
                               if (e[$scope.arrlistchkhead[j].fmH_FeeName] == null) {//added for checking null values
                                   e[$scope.arrlistchkhead[j].fmH_FeeName] = 0;
                               }
                               total_X += e[$scope.arrlistchkhead[j].fmH_FeeName];

                           });
                           
                           var key = $scope.arrlistchkhead[j].fmH_FeeName;

                           obj = {
                               [key]: total_X
                           };
                           indi_totals.push(obj);
                           console.log(obj);

                       }
                       debugger;
                       console.log($scope.colarray);
                       console.log($scope.kengrdtotary);
                       var gridind;
                       var filteredDataa = $filter('date')($scope.FMCB_fromDATE, 'dd/MM/yy');
                       var filteredDataa1 = $filter('date')($scope.FMCB_toDATE, 'dd/MM/yy')
                     //  var photoimg = "/images/BaldwinGirls.png";
                      // var header = $("#gridind .k-grid-header thead");
                       $(document).ready(function () {
                           initGridind();
                          // $(".k-grid-toolbar").prepend('<img src="{{photoimg}}" alt="img"/>');
                           $(".k-grid-toolbar").prepend('<div class="gridTitle"><h4  style="color:white;" class="titlecolor">' + "Report For" + "   " + filteredDataa + "  -  " + filteredDataa1 + '</h4></div>');
                       });
                       function initGridind() {
                           gridind = $("#gridind").kendoGrid({
                               toolbar: ["excel", "pdf"],
                              
                               excel: {
                                   fileName: "inddExport.xlsx",
                                  
                                   filterable: true,
                                   allPages: true
                               },
                               pdf: {
                                   fileName: "inddExport.pdf",
                                   
                                   filterable: true
                               },
                               dataSource: {

                                   data: $scope.kengrdtotary,
                                   aggregate: $scope.colarrayaggre,
                                   pageSize: 100,
                                 
                               },

                               sortable: true,
                               pageable: true,
                               groupable: false,
                               filterable: true,
                               columnMenu: true,
                               reorderable: true,
                               resizable: true,
                               columns: $scope.colarray,
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
                   })
                }
                else {
                    $scope.submitted = true;

                }
            };

        
        }
    
})();
