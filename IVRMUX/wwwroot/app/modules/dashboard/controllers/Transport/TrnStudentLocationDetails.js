(function () {
    'use strict';
    angular
.module('app')
        .controller('TrnStudentLocationDetailsController', TrnStudentLocationDetailsController)
    TrnStudentLocationDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function TrnStudentLocationDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;
        $scope.datecheck = false;
        $scope.usrname = localStorage.getItem('username');


        //var id = 1;
        //$scope.itemsPerPage = 10;
        //$scope.currentPage = 1;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.ddate = new Date();
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
       
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("TrnStudentLocationDetails/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.yearList = promise.yearList;
                      
                }
                else {
                    swal("No Records Found")
                }
            })
        }

        $scope.cancel = function () {
            //$scope.searchValue = '';
            //$scope.frmdate = '';
            //$scope.todate = '';
            //$scope.griddata = [];
            //$scope.griddeatails = false;
            //$scope.griddata.length = 0;
            $state.reload();
            
        }

        $scope.searchValue = '';
        $scope.griddeatails = false;


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
       
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.printstudents = [];
        $scope.userselect = "";
        $scope.toggleAll = function () {
            debugger;
            var toggleStatus = $scope.userselect;
            $scope.printstudents = [];
            angular.forEach($scope.griddata, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected == true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
            });

        }
       
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.userselect = $scope.griddata.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        }
        $scope.gpname = '';
        $scope.type = 'APL'
        $scope.cdeposit = [];
        $scope.getreport = function () {
            $scope.griddata = [];
            $scope.gpname = '';
          
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
              
                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                    }
              
                
                apiService.create("TrnStudentLocationDetails/Getreportdetails", data).
                 then(function (promise) {
                     debugger;
                     if (promise.griddata.length>0) {
                         $scope.griddata = promise.griddata;

                         $scope.colarrayall = [{

                             title: "SLNO",
                             template: "<span class='row-number'></span>", width: "80px"

                         },
                         {
                             name: 'studentname', field: 'studentname', title: 'Student Name', width: "130px"
                         }
                             ,
                         {
                             name: 'AMST_AdmNo', field: 'AMST_AdmNo', title: 'Admission No', width: "150px"
                         },
                         {
                             name: 'AMST_FatherName', field: 'AMST_FatherName', title: 'Father Name', width: "130px"
                         },
                        
                         {
                             name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: "130px"
                         },
                         {
                             name: 'ASMC_SectionName', field: 'ASMC_SectionName', title: 'Section', width: "130px"
                         },
                         {
                             name: 'AMSTSMS_MobileNo', field: 'AMSTSMS_MobileNo', title: 'Mobile No', width: "130px"
                         },
                             {
                                 name: 'TRMR_RouteName', field: 'TRMR_RouteName', title: 'Route Name', width: "130px"
                             },
                             {
                                 name: 'TRML_LocationName', field: 'TRML_LocationName', title: 'Location Name', width: "130px"
                             },
                        
                         ];

                         $(document).ready(function () {
                             initGridall();
                         });
                         function initGridall() {
                             $('#gridall').empty();
                             // gridall = $("#gridall").kendoGrid({
                             $("#gridall").kendoGrid({
                                 toolbar: ["excel"],
                                 excel: {
                                     fileName: "StudentRouteDetails.xlsx",
                                     proxyURL: "",
                                     filterable: true,
                                     allPages: true
                                 },
                                 pdf: {
                                    
                                     avoidLinks: true,
                                   
                                     landscape: true,
                                     repeatHeaders: true,
                                   
                                     fileName: "AvailableBooksReport.pdf",
                                     allPages: true
                                 },
                                 dataSource: {
                                     
                                     data: $scope.griddata,
                                     pageSize: 10,
                                  
                                 },
                                 sortable: true,
                                 //pageable: false,
                                 pageable: true,
                                 groupable: false,
                                 filterable: true,
                                 columnMenu: true,
                                 reorderable: true,
                                 resizable: true,
                                 columns: $scope.colarrayall,
                                 dataBound: function () {
                                     var pagenum = this.dataSource.page();
                                     var pageitms = this.dataSource.pageSize()
                                     var rows = this.items();
                                     $(rows).each(function () {
                                         var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                         var rowLabel = $(this).find(".row-number");
                                         $(rowLabel).html(index);
                                     });
                                 }

                             }).data("kendoGrid");
                         }




                         $scope.griddeatails = true;
                       

                     }
                     else {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                         $state.reload();
                     }
                    
                 })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.transtypechange = function () {
            $scope.griddeatails = false;
            $scope.griddata = [];
        }
        $scope.msg = '';
      


        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
       '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>'
       );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();