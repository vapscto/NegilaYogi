(function () {
    'use strict';
    angular.module('app').controller('OnlineExamReport_NewController', OnlineExamReport_NewController)

    OnlineExamReport_NewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function OnlineExamReport_NewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');

        var copty;

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        //=============
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var id = 1;
            apiService.getURI("OnlineExamConfig/getloaddata", id).then(function (promise) {
                $scope.getclass = promise.getclass;
            });
        };

        $scope.getsectionchange = function (ASMCL_Id) {
            var data = {
                "ASMCL_Id": ASMCL_Id
            };
            apiService.create("OnlineExamConfig/getsection", data).then(function (promise) {
                $scope.getsection = promise.getsection;
            });
        };
        //------------------1st Tab 
        $scope.savedata = function () {
            $scope.submitted1 = true;
            if ($scope.myForm.$valid) {
                if ($scope.FMCB_fromDATE === undefined || $scope.FMCB_fromDATE === null || $scope.FMCB_fromDATE === '') {
                    $scope.fromdate1 = "";
                }
                else {
                    $scope.fromdate1 = $filter('date')($scope.FMCB_fromDATE, "yyyy-MM-dd");
                }
                if ($scope.FMCB_toDATE === undefined || $scope.FMCB_toDATE === null || $scope.FMCB_toDATE === '') {
                    $scope.todate1 = "";
                }
                else {
                    $scope.todate1 = $filter('date')($scope.FMCB_toDATE, "yyyy-MM-dd");
                }

                var data = {
                    "fromdate1": $scope.fromdate1,
                    "todate1": $scope.todate1,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("OnlineExamConfig/getonlinereport", data).then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.presentCountgrid = promise.onlinereport.length;
                    $scope.result1 = [];

                    if (promise.onlinereport.length > 0) {
                        $scope.tablediv = true;
                        $scope.result1 = promise.onlinereport;
                        $scope.colarrayall = [{
                            title: "SLNO",
                            template: "<span class='row-number'></span>", width: "80px"
                        },
                        {
                            name: 'StudentName', field: 'StudentName', title: 'Student Name', width: "160px"
                        },
                        {
                            name: 'PASR_MobileNo', field: 'PASR_MobileNo', title: 'Mobile No', width: "160px"
                        },
                        {
                            name: 'PASR_emailId', field: 'PASR_emailId', title: 'Email ID', width: "160px"
                        },
                        {
                            name: 'ASMCL_ClassName', field: 'ASMCL_ClassName', title: 'Class', width: "160px"
                        },
                        {
                            name: 'TotalNofQuestions', field: 'TotalNofQuestions', title: 'Total No Of Questions', width: "160px"
                        },
                        {
                            name: 'PASTE_TotalQnsAnswered', field: 'PASTE_TotalQnsAnswered', title: 'No Of Questions Answered', width: "160px"
                        },
                        {
                            name: 'PASTE_TotalMaxMarks', field: 'PASTE_TotalMaxMarks', title: 'Total Marks', width: "160px"
                        },
                        {
                            name: 'PASTE_TotalMarks', field: 'PASTE_TotalMarks', title: 'Obtained Marks', width: "160px"
                        },
                        {
                            name: 'PASTE_Percentage', field: 'PASTE_Percentage', title: 'Percentage', width: "160px"
                        }];

                        $(document).ready(function () {
                            initGridall();
                        });

                        function initGridall() {
                            $('#gridall').empty();

                            $("#gridall").kendoGrid({
                                toolbar: ["excel", "pdf"],
                                excel: {
                                    fileName: "OnlineExam_Report.xlsx",
                                    proxyURL: "",
                                    filterable: true,
                                    allPages: true
                                },
                                pdf: {
                                    fileName: "OnlineExam_Report.pdf",
                                    allPages: true
                                },
                                dataSource: {
                                    data: $scope.result1,
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
                    }
                    else {
                        swal('No Records Found');
                        $scope.result1 = [];
                        $scope.tablediv = false;
                    }
                });
            }
            else {
                $scope.submitted1 = true;
            }
        };

        $scope.cancel = function () {
            $scope.tablediv = false;
            $scope.ASMS_Id = '';
            $scope.ASMCL_Id = '';
            $scope.FMCB_fromDATE = '';
            $scope.FMCB_toDATE = '';
        };

        $scope.sort = function (key) {
            $scope.sortKey = key;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted1;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };
    }
})();