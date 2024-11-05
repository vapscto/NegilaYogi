(function () {
    'use strict';
    angular
        .module('app')
        .controller('OpacSearchController', OpacSearchController);

    OpacSearchController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'uiGridGroupingConstants'];
    function OpacSearchController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, uiGridGroupingConstants) {
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.C1 = 'OR';
        $scope.C2 = 'OR';
        $scope.C3 = 'OR';
        $scope.search = '';
        $scope.TYPE = 'Author';
        $scope.filtervalue1 = function (user) {
        };
        $scope.loaddata = function () {
            var data = {};           
            apiService.create("OpacSearch/getalldetails", data).then(function (promise) {
                $scope.subjectlist = promise.subjectlist;
                $scope.publisherlist = promise.publisherlist;
                $scope.authorlist = promise.authorlist;
            });
        };
        $scope.Exactmatch = "Nearest";

        $scope.rpt = false;
        $scope.reportlist = [];
        $scope.report = function () {
            $scope.reportlist = [];
            debugger;
            //if ($scope.Exactmatch === "Exact") {
            //    $scope.Exactmatch = 1;
            //}
            //else {
            //    $scope.Exactmatch = 0;
            //}
            var data = {
                "TYPE": $scope.TYPE,
                "Title": $scope.Title,
                "AuthorId": $scope.AuthorId,
                "SubjectId": $scope.SubjectId,
                "PublisherId": $scope.PublisherId,
                "AccessionNo": $scope.AccessionNo,
                "CallNo": $scope.CallNo,
                "ClassNo": $scope.ClassNo,
                "C1": $scope.C1,
                "C2": $scope.C2,
                "C3": $scope.C3,
            };
            apiService.create("OpacSearch/report", data).then(function (promise) {
                $scope.reportlist = promise.reportlist;




                if ($scope.reportlist.length>0) {

                    $scope.rpt = true;

                angular.forEach($scope.reportlist, function (ff) {
                    ff.LMB_EntryDate = ff.LMB_EntryDate == null ? "" : $filter('date')(ff.LMB_EntryDate, "dd/MM/yyyy");

                })


                $scope.colarrayall = [{

                    title: "SLNO",
                    template: "<span class='row-number'></span>", width: "80px"

                },
                {
                    name: 'Title', field: 'Title', title: 'Book Title', width: "150px"
                },
                {
                    name: 'Accno', field: 'Accno', title: 'Accession No', width: "100px"
                },
                {
                    name: 'LMRA_RackName', field: 'LMRA_RackName', title: 'Rack Name', width: "120px"
                },

                {
                    name: 'LMB_EntryDate', field: 'LMB_EntryDate', title: 'Entry Date', width: "130px", template: "#= kendo.toString(LMB_EntryDate, 'dd/MM/yyyy') #"
                }
                    ,


                {
                    name: 'LMB_ClassNo', field: 'LMB_ClassNo', title: 'Class No', width: "130px"
                },
                {
                    name: 'LMB_NetPrice', field: 'LMB_NetPrice', title: 'Price', width: "130px"
                },
                {
                    name: 'LMB_BookType', field: 'LMB_BookType', title: 'Book Type', width: "130px"
                },
                {
                    name: 'Department', field: 'Department', title: 'Department', width: "130px"
                },
                {
                    name: 'Author', field: 'Author', title: 'Author', width: "130px"
                },
                {
                    name: 'LMP_PublisherName', field: 'LMP_PublisherName', title: 'Publisher', width: "150px"
                },

                {
                    name: 'LMS_SubjectName', field: 'LMS_SubjectName', title: 'Subject', width: "130px"
                },
                {
                    name: 'LMB_PurOrDonated', field: 'LMB_PurOrDonated', title: 'Purchased/Donated', width: "170px"
                }
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
                            fileName: "BookDetails.xlsx",
                            proxyURL: "",
                            filterable: true,
                            allPages: true
                        },
                        pdf: {

                            avoidLinks: true,

                            landscape: true,
                            repeatHeaders: true,

                            fileName: "BookDetails.pdf",
                            allPages: true
                        },


                     
                        dataSource: {

                            data: $scope.reportlist,
                            pageSize: 20,

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
            }else
                {
                    $scope.rpt = false;
                    swal('No Record Found')
                }
            });
        };
        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();