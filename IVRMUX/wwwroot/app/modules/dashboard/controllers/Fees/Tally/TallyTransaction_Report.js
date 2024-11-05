(function () {
    'use strict';
    angular
        .module('app')
        .controller('TallyTransaction_ReportController', TallyTransaction_ReportController)
    TallyTransaction_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function TallyTransaction_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.tablediv = false;



        $scope.changevouchertype = function (vouchertype) {
            $scope.tablediv = false;
            $scope.Grid_view = false;

            var data = {
                "TMT_VoucherType": vouchertype + 'VOUCHER'
            }

            apiService.create("FeeTallyTransaction/getvouchertypedetails", data).
                then(function (promise) {

                    $scope.students = promise.tempararyArrayList;

                })

        }


        $scope.formload = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters

            var data = {
                "TMT_VoucherTypeFlg": $scope.Vchtype
            }

            apiService.create("FeeTallyTransaction/getalldetails", data).
                then(function (promise) {
                    $scope.tablediv = false;
                    $scope.inscount = promise.allinsdata;
                    $scope.yearlst = promise.f_year;
                    $scope.IMFY_Id = promise.f_year_Current_financial_year[0].imfY_Id;
                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.SHOWSTU = function () {

            $scope.termsarray = [];

            angular.forEach($scope.inscount, function (qq) {
                if (qq.selected === true) {
                    $scope.termsarray.push({ FMT_Id: qq.fmT_Id });
                }
            })

            $scope.submitted = true;
            if ($scope.myform.$valid) {
                if ($scope.Vchtype == 'Journal') {
                    if ($scope.termsarray.length > 0) {

                        var data = {
                            termsarray: $scope.termsarray,
                            "TMT_VoucherType": $scope.Vchtype,
                            "From_Date_new": new Date(),
                            "To_Date_new": new Date(),
                            "IMFY_Id": $scope.IMFY_Id,
                        }
                    }
                    else {
                        swal("Atleast Select any one Term")
                    }
                }
                else {
                    $scope.termsarray.push({ FMT_Id: 1 });
                    $scope.start_Date = $filter('date')($scope.FMCB_fromDATE, "yyyy/MM/dd");
                    $scope.end_Date = $filter('date')($scope.FMCB_toDATE, "yyyy/MM/dd");
                    var data = {
                        "From_Date_new": $scope.start_Date,
                        "To_Date_new": $scope.end_Date,
                        "TMT_VoucherType": $scope.Vchtype,
                        "IMFY_Id": $scope.IMFY_Id,
                        termsarray: $scope.termsarray,
                       
                    }

                }

                apiService.create("FeeTallyTransaction/get_tally_data", data).
                    then(function (promise) {
                      
                        if (promise.tally_report_list.length > 0) {
                              $scope.tablediv = true;
                            $scope.tally_report_list = promise.tally_report_list;
                            $scope.presentCountgrid = promise.tally_report_list.length;

                            angular.forEach($scope.tally_report_list, function (ff) {
                                ff.Date = ff.Date == null ? "" : $filter('date')(ff.Date, "dd/MM/yyyy");

                            })
                            
                            angular.forEach($scope.tally_report_list, function (ff) {
                                ff.CHEQUEDATE = ff.CHEQUEDATE == null ? "" : $filter('date')(ff.CHEQUEDATE, "dd/MM/yyyy");

                            })

                            $scope.tempaggary = [];
                            $scope.colarrayall = [{
                                title: "SLNO",
                                template: "<span class='row-number'></span>", width: "80px"
                            },
                              
                                {
                                    name: 'Date', field: 'Date', title: ' Date', width: "130px", template: "#= kendo.toString(Date,  'MM/dd/yyyy') #"
                                },

                            {
                                name: 'VOUCHERTYPE', field: 'VOUCHERTYPE', title: 'VOUCHER TYPE', width: "140px"
                            },
                            {
                                name: 'VOUCHERNO', field: 'VOUCHERNO', title: 'VOUCHER NO', width: "130px"
                            },
                            {
                                name: 'L_Name', field: 'L_Name', title: 'L Name', width: "100px"
                            },
                            {
                                name: 'DR_CR', field: 'DR_CR', title: 'DR/CR', width: "100px"
                            },
                             {
                                name: 'AMOUNT', field: 'AMOUNT', title: 'AMOUNT', width: "100px"
                            },
                            {
                                name: 'TRANSACTION_TYPE', field: 'TRANSACTION_TYPE', title: 'TRANSACTION TYPE', width: "100px"
                            },
                            {
                                name: 'CHEQUENO', field: 'CHEQUENO', title: 'CHEQUE NO', width: "100px"
                            },
                            {
                                name: 'CHEQUEDATE', field: 'CHEQUEDATE', title: 'CHEQUE DATE', width: "100px",
                                template: "#= kendo.toString(CHEQUEDATE, 'MM/dd/yyyy') #"
                                
                            },
                            {
                                name: 'VAPSID', field: 'VAPSID', title: 'VAPS ID', width: "100px"
                            },
                            {
                                name: 'STATUS', field: 'STATUS', title: 'STATUS', width: "100px"
                            },
                            {
                                name: 'NARRATION', field: 'NARRATION', title: 'NARRATION', width: "100px"
                            },
                            {
                                name: 'UNDER', field: 'UNDER', title: 'UNDER', width: "100px"
                            }
                           

                            ];

                            $(document).ready(function () {
                                initGridall();
                            });
                            function initGridall() {
                                $('#gridall').empty();
                                //gridall =
                                $("#gridall").kendoGrid({
                                    toolbar: ["excel"],

                                   

                                    excel: {
                                        fileName: "TallyTransaction_Report.xlsx",
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },
                                    pdf: {
                                        avoidLinks: true,
                                        landscape: true,
                                        repeatHeaders: true,
                                        fileName: "TallyTransaction_Report.pdf",
                                        allPages: true
                                    },
                                    dataSource: {
                                        data: $scope.tally_report_list,
                                        pageSize: 10,


                                    },
                                    sortable: true,
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
                        }
                    
                    else {
                            swal("No Data Imported!!!")
                        }
                    
                    })

            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.cleardata = function () {
            $state.reload();
        }


        $scope.toggleAll = function () {
            debugger;
            $scope.printdatatable = [];
            var toggleStatus = $scope.all;
            angular.forEach($scope.totalgrid, function (itm) {
                itm.isSelected = toggleStatus;
                if ($scope.all == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });

        }


    }

})();



