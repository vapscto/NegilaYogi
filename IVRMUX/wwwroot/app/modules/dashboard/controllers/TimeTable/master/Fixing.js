
(function () {
    'use strict';
    angular
.module('app')
.controller('FixingController', FixingController)

    FixingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function FixingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};



        //Day Period Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', displayName: 'Academic Year' },
            { name: 'ttmC_CategoryName', displayName: 'Category' },
            { name: 'asmcL_ClassName', displayName: 'Class' },
             { name: 'asmC_SectionName', displayName: 'Section' },
               { name: 'staffName', displayName: 'Staff Name' },
            { name: 'ismS_SubjectName', displayName: 'Subject Name' },
             { name: 'ttmD_DayName', displayName: 'Day' },
             { name: 'ttmP_PeriodName', displayName: 'Period' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <md-tooltip md-direction="down">Edit</md-tooltip><i class="fa fa-pencil-square-o" aria-hidden="true"></i></a>' +
                 //'<a ng-if="row.entity.ttfdP_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch1(row.entity);"><md-tooltip md-direction="down">Active Now</md-tooltip> <i class="fa fa-toggle-on text-green" aria-hidden="true"></i></a>' +
                 // '<span ng-if="row.entity.ttfdP_ActiveFlag === true ""><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch1(row.entity);"> <md-tooltip md-direction="down">Deactive Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a><span>' +
                 //'</div>'
                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttfdP_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttfdP_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
                   //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                   //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata1(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                   //'</div>'
               }
            ]

        };
        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', displayName: 'Academic Year' },

               { name: 'staffName', displayName: 'Staff Name' },

             { name: 'ttmD_DayName', displayName: 'Day Name' },
             { name: 'ttfdS_SUbSelcFlag', displayName: 'Class-Section Wise' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                    '<div class="grid-action-cell">' +
                     '<a ng-if="row.entity.ttfdS_SUbSelcFlag === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup2" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup2(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue2(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttfdS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch2(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttfdS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch2(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };

        $scope.gridOptions3 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', displayName: 'Academic Year' },

               { name: 'ismS_SubjectName', displayName: 'Subject Name' },

             { name: 'ttmD_DayName', displayName: 'Day Name' },
             { name: 'ttfdsU_SUbSelcFlag', displayName: 'Class-Section Wise' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                    '<div class="grid-action-cell">' +
                     '<a ng-if="row.entity.ttfdsU_SUbSelcFlag === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup3" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup3(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue3(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttfdsU_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch3(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttfdsU_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch3(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };

        $scope.gridOptions4 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', displayName: 'Academic Year' },

               { name: 'staffName', displayName: 'Staff Name' },

             { name: 'ttmP_PeriodName', displayName: 'Period Name' },
             { name: 'ttfpS_SUbSelcFlag', displayName: 'Class-Section Wise' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                    '<div class="grid-action-cell">' +
                     '<a ng-if="row.entity.ttfpS_SUbSelcFlag === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup4" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup4(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue4(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttfpS_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch4(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttfpS_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch4(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };

        $scope.gridOptions5 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', displayName: 'Academic Year' },

               { name: 'ismS_SubjectName', displayName: 'Subject Name' },

             { name: 'ttmP_PeriodName', displayName: 'Period Name' },
             { name: 'ttfpsU_SUbSelcFlag', displayName: 'Class-Section Wise' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                    '<div class="grid-action-cell">' +
                     '<a ng-if="row.entity.ttfpsU_SUbSelcFlag === true" href="javascript:void(0)" data-toggle="modal" data-target="#popup5" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup5(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue5(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttfpsU_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch5(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttfpsU_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch5(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };

        //Day Staff Grid view rendering data from data base
        $scope.gridOptions2_sub = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [


              { name: 'clsDisplay', displayName: 'Class' },
            { name: 'sectDisplay', displayName: 'Section' },
             { name: 'subDisplay', displayName: 'Subject' },
              { name: 'pedDisplay', displayName: 'No Of Periods' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedatarightgrid2(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]


        };

        //Day Staff Grid view rendering data from data base
        $scope.gridOptions3_sub = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [


              { name: 'clsDisplay', displayName: 'Class' },
            { name: 'sectDisplay', displayName: 'Section' },
             { name: 'staffDisplay', displayName: 'Staff' },
              { name: 'pedDisplay', displayName: 'No Of Periods' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedatarightgrid3(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]


        };

        //Day Staff Grid view rendering data from data base
        $scope.gridOptions4_sub = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [


              { name: 'clsDisplay', displayName: 'Class' },
            { name: 'sectDisplay', displayName: 'Section' },
             { name: 'subDisplay', displayName: 'Subject' },
              { name: 'dayDisplay', displayName: 'No Of Days' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedatarightgrid4(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]


        };

        //Day Staff Grid view rendering data from data base
        $scope.gridOptions5_sub = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [


              { name: 'clsDisplay', displayName: 'Class' },
            { name: 'sectDisplay', displayName: 'Section' },
             { name: 'staffDisplay', displayName: 'Staff' },
               { name: 'dayDisplay', displayName: 'No Of Days' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedatarightgrid5(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]


        };

        $scope.griddperiod = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttc_AcademicYear', displayName: 'Academic Year' },
            { name: 'ttc_Category', displayName: 'Category' },
            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
               { name: 'ttc_Staff', displayName: 'Staff' },
            { name: 'ttc_Subject', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };

        $scope.griddstaff = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttc_AcademicYear', displayName: 'Academic Year' },
            { name: 'ttc_day', displayName: 'Day' },
                { name: 'ttc_Staff', displayName: 'Staff' },
            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
            { name: 'ttc_Subject', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        //Day Subject Grid view rendering data from data base
        $scope.griddsubject1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
          { name: 'ttc_Staff', displayName: 'Staff' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        $scope.griddsubject = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttc_AcademicYear', displayName: 'Academic Year' },
            { name: 'ttc_day', displayName: 'Day' },
                { name: 'ttc_Staff', displayName: 'Staff' },
            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
            { name: 'ttc_Subject', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        //Period Staff Grid view rendering data from data base
        $scope.gridpstaff1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },

            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
            { name: 'ttc_Subject', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        $scope.gridpstaff = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                   { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttc_AcademicYear', displayName: 'Academic Year' },
            { name: 'ttc_Period', displayName: 'Period' },
                { name: 'ttc_Staff', displayName: 'Staff' },
            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
            { name: 'ttc_Subject', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        //Period Subject Grid view rendering data from data base
        $scope.gridpsubject1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
               { name: 'ttc_Class', displayName: 'Class' },
                { name: 'ttc_Staff', displayName: 'Staff' },
             { name: 'ttc_Section', displayName: 'Section' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        $scope.gridpsubject = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                 { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'ttc_AcademicYear', displayName: 'Academic Year' },
            { name: 'ttc_Period', displayName: 'Period' },
                { name: 'ttc_Staff', displayName: 'Staff' },
            { name: 'ttc_Class', displayName: 'Class' },
             { name: 'ttc_Section', displayName: 'Section' },
            { name: 'ttc_Subject', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };

        // TO Save The Data
        $scope.submitted1 = false;
        $scope.saveddata1 = function () {
            
            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {


                var data = {

                    "TTFDP_Id": $scope.TTFDP_Id,
                    "ASMAY_Id": $scope.asmaY_Id1,
                    "TTMC_Id": $scope.ttmC_Id1,
                    "ASMCL_Id": $scope.asmcL_Id1,
                    "ASMS_Id": $scope.asmS_Id1,
                    "HRME_Id": $scope.hrmE_Id1,
                    "ISMS_Id": $scope.ismS_Id1,
                    "TTMD_Id": $scope.ttmD_Id1,
                    "TTMP_Id": $scope.ttmP_Id1,
                    // "TTMC_Id": $scope.TTMC_Id,
                    // "TTMC_CategoryName": $scope.TTMC_CategoryName,
                }
                apiService.create("Fixing/savedetail1", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected Staff And Subject Is Restricted For Selected Details !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                            //else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            //    swal('Recards AlReady Exist !');
                            //}

                        else {
                            swal('Data Not Saved !');
                        }
                        $scope.BindData();
                    })
                $scope.BindData();
                $scope.clear1();
            }

        };

        $scope.submitted2 = false;
        $scope.saveddata2 = function () {
            $scope.submitted2 = true;
            
            if ($scope.myForm2.$valid) {

                if ($scope.cs_flag2 == 0) {
                    var data = {
                        "TTFDS_Id": $scope.TTFDS_Id,
                        "ASMAY_Id": $scope.asmaY_Id2,
                        "TTMD_Id": $scope.ttmD_Id2,
                        "HRME_Id": $scope.hrmE_Id2,
                        "TTFDS_SUbSelcFlag": $scope.cs_flag2,
                    }
                }
                else if ($scope.cs_flag2 == 1) {
                    var data = {
                        "TTFDS_Id": $scope.TTFDS_Id,
                        "TTFDSCC_Id": $scope.TTFDSCC_Id,
                        "ASMAY_Id": $scope.asmaY_Id2,
                        "HRME_Id": $scope.hrmE_Id2,
                        "TTFDS_SUbSelcFlag": $scope.cs_flag2,
                        "TTMD_Id": $scope.ttmD_Id2,
                        "TempararyArrayList": $scope.albumNameArraysaveDB2
                    }
                }
                apiService.create("Fixing/savedetail2", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Staff Is Restricted For Selected Day !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB2 = "";
                            $scope.gridOptions2_sub.data = "";
                            $scope.albumNameArray2 = "";
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        // $scope.BindData();
                        $scope.gridOptions2.data = promise.all_fix_day_staff_list;
                    })
                //$scope.BindData();
                $scope.clear2();
            }
            else {
                $scope.submitted2 = true;

            }
        };

        $scope.submitted3 = false;
        $scope.saveddata3 = function () {
            $scope.submitted3 = true;
            
            if ($scope.myForm3.$valid) {

                if ($scope.cs_flag3 == 0) {
                    var data = {
                        "TTFDSU_Id": $scope.TTFDSU_Id,
                        "ASMAY_Id": $scope.asmaY_Id3,
                        "TTMD_Id": $scope.ttmD_Id3,
                        //"HRME_Id": $scope.hrmE_Id2,
                        "ISMS_Id": $scope.ismS_Id3,
                        "TTFDSU_SUbSelcFlag": $scope.cs_flag3,
                    }
                }
                else if ($scope.cs_flag3 == 1) {
                    var data = {
                        "TTFDSU_Id": $scope.TTFDSU_Id,
                        "TTFDSUCC_Id": $scope.TTFDSUCC_Id,
                        "ASMAY_Id": $scope.asmaY_Id3,
                        //"HRME_Id": $scope.hrmE_Id2,
                        "ISMS_Id": $scope.ismS_Id3,
                        "TTFDSU_SUbSelcFlag": $scope.cs_flag3,
                        "TTMD_Id": $scope.ttmD_Id3,
                        "TempararyArrayList": $scope.albumNameArraysaveDB3
                    }
                }
                apiService.create("Fixing/savedetail3", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Subject Is Restricted For Selected Day !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB3 = "";
                            $scope.gridOptions3_sub.data = "";
                            $scope.albumNameArray3 = "";
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        // $scope.BindData();
                        $scope.gridOptions3.data = promise.all_fix_day_subject_list;
                    })
                //$scope.BindData();
                $scope.clear3();
            }
            else {
                $scope.submitted3 = true;

            }
        };

        $scope.submitted4 = false;
        $scope.saveddata4 = function () {
            $scope.submitted4 = true;
            
            if ($scope.myForm4.$valid) {

                if ($scope.cs_flag4 == 0) {
                    var data = {
                        "TTFPS_Id": $scope.TTFPS_Id,
                        "ASMAY_Id": $scope.asmaY_Id4,
                        "TTMP_Id": $scope.ttmP_Id4,
                        "HRME_Id": $scope.hrmE_Id4,
                        "TTFPS_SUbSelcFlag": $scope.cs_flag4,
                    }
                }
                else if ($scope.cs_flag4 == 1) {
                    var data = {
                        "TTFPS_Id": $scope.TTFPS_Id,
                        "TTFPSCC_Id": $scope.TTFPSCC_Id,
                        "ASMAY_Id": $scope.asmaY_Id4,
                        "HRME_Id": $scope.hrmE_Id4,
                        "TTFPS_SUbSelcFlag": $scope.cs_flag4,
                        "TTMP_Id": $scope.ttmP_Id4,
                        "TempararyArrayList": $scope.albumNameArraysaveDB4
                    }
                }
                apiService.create("Fixing/savedetail4", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Staff Is Restricted For Selected Period !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB4 =[];
                            $scope.gridOptions4_sub.data = "";
                            $scope.albumNameArray4 = [ ];
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        // $scope.BindData();
                        $scope.gridOptions4.data = promise.all_fix_period_staff_list;
                    })
                //$scope.BindData();
                $scope.clear4();
            }
            else {
                $scope.submitted4 = true;

            }
        };

        $scope.submitted5 = false;
        $scope.saveddata5 = function () {
            $scope.submitted5 = true;
            
            if ($scope.myForm5.$valid) {

                if ($scope.cs_flag5 == 0) {
                    var data = {
                        "TTFPSU_Id": $scope.TTFPSU_Id,
                        "ASMAY_Id": $scope.asmaY_Id5,
                        "TTMP_Id": $scope.ttmP_Id5,
                        //"HRME_Id": $scope.hrmE_Id2,
                        "ISMS_Id": $scope.ismS_Id5,
                        "TTFPSU_SUbSelcFlag": $scope.cs_flag5,
                    }
                }
                else if ($scope.cs_flag5 == 1) {
                    var data = {
                        "TTFPSU_Id": $scope.TTFPSU_Id,
                        "TTFPSUCC_Id": $scope.TTFPSUCC_Id,
                        "ASMAY_Id": $scope.asmaY_Id5,
                        //"HRME_Id": $scope.hrmE_Id2,
                        "ISMS_Id": $scope.ismS_Id5,
                        "TTFPSU_SUbSelcFlag": $scope.cs_flag5,
                        "TTMP_Id": $scope.ttmP_Id5,
                        "TempararyArrayList": $scope.albumNameArraysaveDB5
                    }
                }
                apiService.create("Fixing/savedetail5", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Subject Is Restricted For Selected Day !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB5 = "";
                            $scope.gridOptions5_sub.data = "";
                            $scope.albumNameArray5 = "";
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        // $scope.BindData();
                        $scope.gridOptions5.data = promise.all_fix_period_subject_list;
                    })
                //$scope.BindData();
                $scope.clear5();
            }
            else {
                $scope.submitted5 = true;

            }
        };

        //to edit day period
        $scope.getorgvalue1 = function (employee) {
            
            $scope.editperiod = employee.ttfdP_Id;

            var editid = $scope.editperiod;
            apiService.getURI("Fixing/getpagedetails1", editid).
            then(function (promise) {
                $scope.TTFDP_Id = promise.fix_day_period_edit[0].ttfdP_Id;
                $scope.asmaY_Id1 = promise.fix_day_period_edit[0].asmaY_Id;
                $scope.ttmD_Id1 = promise.fix_day_period_edit[0].ttmD_Id;
                $scope.temp_category1 = promise.fix_day_period_edit[0].ttmC_Id;
                $scope.ttmC_Id1 = $scope.temp_category1;
                $scope.temp_class1 = promise.fix_day_period_edit[0].asmcL_Id;
                $scope.temp_period1 = promise.fix_day_period_edit[0].ttmP_Id;
                $scope.temp_section1 = promise.fix_day_period_edit[0].asmS_Id;
                $scope.temp_staff1 = promise.fix_day_period_edit[0].hrmE_Id;
                $scope.temp_subject1 = promise.fix_day_period_edit[0].ismS_Id;
                if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "") {
                    
                    $scope.get_class1();
                    $scope.asmcL_Id1 = $scope.temp_class1;
                }

                if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "" && $scope.asmcL_Id1 != "") {
                    
                    $scope.get_period_section1();
                    $scope.ttmP_Id1 = $scope.temp_period1;
                    $scope.asmS_Id1 = $scope.temp_section1;
                }
                if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "" && $scope.asmcL_Id1 != "" && $scope.asmS_Id1 != "") {
                    
                    $scope.get_staff1();
                    $scope.hrmE_Id1 = $scope.temp_staff1;
                }
                if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "" && $scope.asmcL_Id1 != "" && $scope.asmS_Id1 != "" && $scope.hrmE_Id1 != "") {
                    
                    $scope.get_subject1();
                    $scope.ismS_Id1 = $scope.temp_subject1;
                }

            })

        }


        $scope.getorgvalue2 = function (employee) {
            
            $scope.editEmployee = employee.ttfdS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Fixing/getpagedetails2", pageid).
            then(function (promise) {
                
                $scope.clear2();
                // pedcount = promise.edit_count;

                $scope.TTFDS_Id = promise.fix_day_staff_edit[0].ttfdS_Id;
                $scope.asmaY_Id2 = promise.fix_day_staff_edit[0].asmaY_Id;
                $scope.hrmE_Id2 = promise.fix_day_staff_edit[0].hrmE_Id;
                $scope.ttmD_Id2 = promise.fix_day_staff_edit[0].ttmD_Id;
                $scope.temp_check2 = promise.fix_day_staff_edit[0].ttfdS_SUbSelcFlag;
                if ($scope.temp_check2 == true) {
                    $scope.cs_flag2 = 1;
                }
                else if ($scope.temp_check2 == false) {
                    $scope.cs_flag2 = 0;
                }
                $scope.CSwise2();
                // $scope.NOP_1 = promise.period_distri_edit[0].ttfpD_TotWeekPeriods;

                
                if ($scope.asmaY_Id2 != "" && $scope.hrmE_Id2 != "") {
                    $scope.get_cls_sec_sub2();
                }
                if (promise.fix_day_staff__classecedit != "" && promise.fix_day_staff__classecedit != null) {

                    $scope.TTFDSCC_Id = promise.fix_day_staff__classecedit[0].ttfdscC_Id;
                    $scope.temp_class2 = promise.fix_day_staff__classecedit[0].asmcL_Id;

                    for (var i = 0; i < promise.fix_day_staff__classecedit.length; i++) {

                        $scope.albumNameArraysaveDB2.push({ ASMCL_Id: promise.fix_day_staff__classecedit[i].asmcL_Id, ASMS_Id: promise.fix_day_staff__classecedit[i].asmS_Id, ISMS_Id: promise.fix_day_staff__classecedit[i].ismS_Id, NOP: promise.fix_day_staff__classecedit[i].ttfdscC_Periods });
                        for (var k = 0; k < $scope.temp_classlist.length; k++) {
                            if ($scope.temp_classlist[k].asmcL_Id == promise.fix_day_staff__classecedit[i].asmcL_Id) {
                                cls2 = $scope.temp_classlist[k].asmcL_ClassName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_sectionlist.length; k++) {
                            if ($scope.temp_sectionlist[k].asmS_Id == promise.fix_day_staff__classecedit[i].asmS_Id) {
                                sect2 = $scope.temp_sectionlist[k].asmC_SectionName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_subjectlist.length; k++) {
                            if ($scope.temp_subjectlist[k].ismS_Id == promise.fix_day_staff__classecedit[i].ismS_Id) {
                                subj2 = $scope.temp_subjectlist[k].ismS_SubjectName;
                            }
                        }
                        period2 = promise.fix_day_staff__classecedit[i].ttfdscC_Periods;
                        $scope.albumNameArray2.push({
                            clsDisplay: cls2,
                            sectDisplay: sect2,
                            subDisplay: subj2,
                            pedDisplay: period2,
                        });
                    }
                }

                $scope.gridOptions2_sub.data = $scope.albumNameArray2;

            })
        }

        $scope.getorgvalue3 = function (employee) {
            $scope.editEmployee = employee.ttfdsU_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Fixing/getpagedetails3", pageid).
            then(function (promise) {
                $scope.clear3();
                $scope.TTFDSU_Id = promise.fix_day_subject_edit[0].ttfdsU_Id;
                $scope.asmaY_Id3 = promise.fix_day_subject_edit[0].asmaY_Id;
                $scope.ismS_Id3 = promise.fix_day_subject_edit[0].ismS_Id;
                $scope.ttmD_Id3 = promise.fix_day_subject_edit[0].ttmD_Id;
                $scope.temp_check3 = promise.fix_day_subject_edit[0].ttfdsU_SUbSelcFlag;
                if ($scope.temp_check3 == true) {
                    $scope.cs_flag3 = 1;
                }
                else if ($scope.temp_check3 == false) {
                    $scope.cs_flag3 = 0;
                }
                $scope.CSwise3();
                if ($scope.asmaY_Id3 != "" && $scope.ismS_Id3 != "") {
                    $scope.get_cls_sec_staff3();
                }
                if (promise.fix_day_subject__classecedit != "" && promise.fix_day_subject__classecedit != null) {
                    $scope.TTFDSUCC_Id = promise.fix_day_subject__classecedit[0].ttfdsucC_Id;
                    $scope.temp_class3 = promise.fix_day_subject__classecedit[0].asmcL_Id;
                    for (var i = 0; i < promise.fix_day_subject__classecedit.length; i++) {
                        $scope.albumNameArraysaveDB3.push({ ASMCL_Id: promise.fix_day_subject__classecedit[i].asmcL_Id, ASMS_Id: promise.fix_day_subject__classecedit[i].asmS_Id, HRME_Id: promise.fix_day_subject__classecedit[i].hrmE_Id, NOP: promise.fix_day_subject__classecedit[i].ttfdsucC_Periods });
                        for (var k = 0; k < $scope.temp_classlist.length; k++) {
                            if ($scope.temp_classlist[k].asmcL_Id == promise.fix_day_subject__classecedit[i].asmcL_Id) {
                                cls3 = $scope.temp_classlist[k].asmcL_ClassName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_sectionlist.length; k++) {
                            if ($scope.temp_sectionlist[k].asmS_Id == promise.fix_day_subject__classecedit[i].asmS_Id) {
                                sect3 = $scope.temp_sectionlist[k].asmC_SectionName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_stafflist.length; k++) {
                            if ($scope.temp_stafflist[k].hrmE_Id == promise.fix_day_subject__classecedit[i].hrmE_Id) {
                                staff3 = $scope.temp_stafflist[k].staffName;
                            }
                        }
                        period3 = promise.fix_day_subject__classecedit[i].ttfdsucC_Periods;
                        $scope.albumNameArray3.push({
                            clsDisplay: cls3,
                            sectDisplay: sect3,
                            staffDisplay: staff3,
                            pedDisplay: period3,
                        });
                    }
                }
                $scope.gridOptions3_sub.data = $scope.albumNameArray3;

            })
        }

        $scope.getorgvalue4 = function (employee) {
            $scope.editEmployee = employee.ttfpS_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Fixing/getpagedetails4", pageid).
            then(function (promise) {
                $scope.clear4();
                $scope.TTFPS_Id = promise.fix_period_staff_edit[0].ttfpS_Id;
                $scope.asmaY_Id4 = promise.fix_period_staff_edit[0].asmaY_Id;
                $scope.hrmE_Id4 = promise.fix_period_staff_edit[0].hrmE_Id;
                $scope.ttmP_Id4 = promise.fix_period_staff_edit[0].ttmP_Id;
                $scope.temp_check4 = promise.fix_period_staff_edit[0].ttfpS_SUbSelcFlag;
                if ($scope.temp_check4 == true) {
                    $scope.cs_flag4 = 1;
                }
                else if ($scope.temp_check4 == false) {
                    $scope.cs_flag4 = 0;
                }
                $scope.CSwise4();
                if ($scope.asmaY_Id4 != "" && $scope.hrmE_Id4 != "") {
                    $scope.get_cls_sec_sub4();
                }
                if (promise.fix_period_staff__classecedit != "" && promise.fix_period_staff__classecedit != null) {
                    $scope.TTFPSCC_Id = promise.fix_period_staff__classecedit[0].ttfpscC_Id;
                    $scope.temp_class4 = promise.fix_period_staff__classecedit[0].asmcL_Id;

                    for (var i = 0; i < promise.fix_period_staff__classecedit.length; i++) {
                        $scope.albumNameArraysaveDB4.push({ ASMCL_Id: promise.fix_period_staff__classecedit[i].asmcL_Id, ASMS_Id: promise.fix_period_staff__classecedit[i].asmS_Id, ISMS_Id: promise.fix_period_staff__classecedit[i].ismS_Id, NOD: promise.fix_period_staff__classecedit[i].ttfpscC_Days });
                        for (var k = 0; k < $scope.temp_classlist.length; k++) {
                            if ($scope.temp_classlist[k].asmcL_Id == promise.fix_period_staff__classecedit[i].asmcL_Id) {
                                cls4 = $scope.temp_classlist[k].asmcL_ClassName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_sectionlist.length; k++) {
                            if ($scope.temp_sectionlist[k].asmS_Id == promise.fix_period_staff__classecedit[i].asmS_Id) {
                                sect4 = $scope.temp_sectionlist[k].asmC_SectionName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_subjectlist.length; k++) {
                            if ($scope.temp_subjectlist[k].ismS_Id == promise.fix_period_staff__classecedit[i].ismS_Id) {
                                subj4 = $scope.temp_subjectlist[k].ismS_SubjectName;
                            }
                        }
                        day4 = promise.fix_period_staff__classecedit[i].ttfpscC_Days;
                        $scope.albumNameArray4.push({
                            clsDisplay: cls4,
                            sectDisplay: sect4,
                            subDisplay: subj4,
                            dayDisplay: day4,
                        });
                    }
                }
                $scope.gridOptions4_sub.data = $scope.albumNameArray4;
            })
        }

        $scope.getorgvalue5 = function (employee) {
            $scope.editEmployee = employee.ttfpsU_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Fixing/getpagedetails5", pageid).
            then(function (promise) {
                $scope.clear5();
                $scope.TTFPSU_Id = promise.fix_period_subject_edit[0].ttfpsU_Id;
                $scope.asmaY_Id5 = promise.fix_period_subject_edit[0].asmaY_Id;
                $scope.ismS_Id5 = promise.fix_period_subject_edit[0].ismS_Id;
                $scope.ttmP_Id5 = promise.fix_period_subject_edit[0].ttmP_Id;
                $scope.temp_check5 = promise.fix_period_subject_edit[0].ttfpsU_SUbSelcFlag;
                if ($scope.temp_check5 == true) {
                    $scope.cs_flag5 = 1;
                }
                else if ($scope.temp_check5 == false) {
                    $scope.cs_flag5 = 0;
                }
                $scope.CSwise5();
                if ($scope.asmaY_Id5 != "" && $scope.ismS_Id5 != "") {
                    $scope.get_cls_sec_staff5();
                }
                if (promise.fix_period_subject__classecedit != "" && promise.fix_period_subject__classecedit != null) {
                    $scope.TTFPSUCC_Id = promise.fix_period_subject__classecedit[0].ttfpsucC_Id;
                    $scope.temp_class5 = promise.fix_period_subject__classecedit[0].asmcL_Id;

                    for (var i = 0; i < promise.fix_period_subject__classecedit.length; i++) {
                        $scope.albumNameArraysaveDB5.push({ ASMCL_Id: promise.fix_period_subject__classecedit[i].asmcL_Id, ASMS_Id: promise.fix_period_subject__classecedit[i].asmS_Id, HRME_Id: promise.fix_period_subject__classecedit[i].hrmE_Id, NOD: promise.fix_period_subject__classecedit[i].ttfpsucC_Days });
                        for (var k = 0; k < $scope.temp_classlist.length; k++) {
                            if ($scope.temp_classlist[k].asmcL_Id == promise.fix_period_subject__classecedit[i].asmcL_Id) {
                                cls5 = $scope.temp_classlist[k].asmcL_ClassName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_sectionlist.length; k++) {
                            if ($scope.temp_sectionlist[k].asmS_Id == promise.fix_period_subject__classecedit[i].asmS_Id) {
                                sect5 = $scope.temp_sectionlist[k].asmC_SectionName;
                            }
                        }
                        for (var k = 0; k < $scope.temp_stafflist.length; k++) {
                            if ($scope.temp_stafflist[k].hrmE_Id == promise.fix_period_subject__classecedit[i].hrmE_Id) {
                                staff5 = $scope.temp_stafflist[k].staffName;
                            }
                        }
                        day5 = promise.fix_period_subject__classecedit[i].ttfpsucC_Days;
                        $scope.albumNameArray5.push({
                            clsDisplay: cls5,
                            sectDisplay: sect5,
                            staffDisplay: staff5,
                            dayDisplay: day5,
                        });
                    }
                }
                $scope.gridOptions5_sub.data = $scope.albumNameArray5;

            })
        }

        //TO  View Record
        $scope.viewrecordspopup2 = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.ttfdS_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("Fixing/getalldetailsviewrecords2", pageid).
                    then(function (promise) {
                        
                        $scope.staff_Name = promise.detailspopuparray2[0].staffName;
                        $scope.day_Name = promise.detailspopuparray2[0].ttmD_DayName;
                        $scope.viewrecordspopupdisplay2 = promise.detailspopuparray2;
                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid2 = function () {
            $scope.viewrecordspopupdisplay2 = "";
        };

        //TO  View Record
        $scope.viewrecordspopup3 = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.ttfdsU_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("Fixing/getalldetailsviewrecords3", pageid).
                    then(function (promise) {
                        
                        $scope.subject_Name = promise.detailspopuparray3[0].ismS_SubjectName;
                        $scope.day_Name = promise.detailspopuparray3[0].ttmD_DayName;
                        $scope.viewrecordspopupdisplay3 = promise.detailspopuparray3;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid3 = function () {
            $scope.viewrecordspopupdisplay3 = "";
        };

        //TO  View Record
        $scope.viewrecordspopup4 = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.ttfpS_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("Fixing/getalldetailsviewrecords4", pageid).
                    then(function (promise) {
                        
                        $scope.staff_Name = promise.detailspopuparray4[0].staffName;
                        $scope.period_Name = promise.detailspopuparray4[0].ttmP_PeriodName;
                        $scope.viewrecordspopupdisplay4 = promise.detailspopuparray4;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid4 = function () {
            $scope.viewrecordspopupdisplay4 = "";
        };

        //TO  View Record
        $scope.viewrecordspopup5 = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttfpsU_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("Fixing/getalldetailsviewrecords5", pageid).
                    then(function (promise) {
                        $scope.subject_Name = promise.detailspopuparray5[0].ismS_SubjectName;
                        $scope.period_Name = promise.detailspopuparray5[0].ttmP_PeriodName;
                        $scope.viewrecordspopupdisplay5 = promise.detailspopuparray5;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid5 = function () {
            $scope.viewrecordspopupdisplay5 = "";
        };


        //to get class by category
        $scope.asmaY_Id1 = "";
        $scope.ttmC_Id1 = "";
        $scope.get_class1 = function () {
            if ($scope.asmaY_Id1 == "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id1,
                    "ASMAY_Id": $scope.asmaY_Id1,
                }
                apiService.create("Fixing/getclass_catg", data).
         then(function (promise) {
             $scope.class_list1 = promise.classbycategory;
             $scope.asmcL_Id1 = "";
             if ($scope.TTFDP_Id != "" && $scope.TTFDP_Id != 0) {
                 angular.forEach($scope.class_list1, function (role) {
                     if (role.asmcL_Id == $scope.temp_class1) {
                         $scope.asmcL_Id1 = role.asmcL_Id;
                         role.Selected = true;
                     }
                 })
             }
             if (promise.classbycategory == "" || promise.classbycategory == null) {
                 swal("No classes Are Mapped To Selected Category");
             }
         })
            }
        };

        //get period and section  by class
        $scope.get_period_section1 = function () {
            if ($scope.asmaY_Id1 == "" && $scope.ttmC_Id1 == "") {
                swal("Please Select The Academic Year And Category !");
            }
            else if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "" && $scope.asmcL_Id1 != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id1,
                    "TTMC_Id": $scope.ttmC_Id1,
                    "ASMCL_Id": $scope.asmcL_Id1,
                }
                apiService.create("Fixing/getperiod_class", data).
         then(function (promise) {

             $scope.period_list1 = promise.periodbyclass;
             $scope.section_list1 = promise.sectionbyclass;
             $scope.ttmP_Id1 = "";
             $scope.asmS_Id1 = "";
             if ($scope.TTFDP_Id != "" && $scope.TTFDP_Id != 0) {
                 angular.forEach($scope.period_list1, function (role) {
                     if (role.ttmP_Id == $scope.temp_period1) {
                         $scope.ttmP_Id1 = role.ttmP_Id;
                         role.Selected = true;
                     }
                 })
             }
             if ($scope.TTFDP_Id != "" && $scope.TTFDP_Id != 0) {
                 angular.forEach($scope.section_list1, function (role) {
                     if (role.asmS_Id == $scope.temp_section1) {
                         $scope.asmS_Id1 = role.asmS_Id;
                         role.Selected = true;
                     }
                 })
             }
             if (promise.periodbyclass == "" || promise.periodbyclass == null) {
                 swal("No periods Are Allocated To Selected Class");
             }
             if (promise.sectionbyclass == "" || promise.sectionbyclass == null) {
                 swal("No sections Are Mapped To Selected Class");
             }
         })
            }
        }

        $scope.get_staff1 = function () {
            if ($scope.asmaY_Id1 == "" && $scope.ttmC_Id1 == "" && $scope.asmcL_Id1 == "") {
                swal("Please Select The Academic Year And Category And Class !");
            }
            else if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "" && $scope.asmcL_Id1 != "" && $scope.asmS_Id1 != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id1,
                    "TTMC_Id": $scope.ttmC_Id1,
                    "ASMCL_Id": $scope.asmcL_Id1,
                    "ASMS_Id": $scope.asmS_Id1,
                }
                apiService.create("Fixing/getstaff_section", data).
         then(function (promise) {
             $scope.staff_list1 = promise.staffbyall;
             $scope.hrmE_Id1 = "";
             if ($scope.TTFDP_Id != "" && $scope.TTFDP_Id != 0) {
                 angular.forEach($scope.staff_list1, function (role) {
                     if (role.hrmE_Id == $scope.temp_staff1) {
                         $scope.hrmE_Id1 = role.hrmE_Id;
                         role.Selected = true;
                     }
                 })
             }
             if (promise.staffbyall == "" || promise.staffbyall == null) {
                 swal("No Staff Are Allocated To Selected Class and Section");
             }
         })
            }
        }

        $scope.get_subject1 = function () {
            
            if ($scope.asmaY_Id1 == "" && $scope.ttmC_Id1 == "" && $scope.asmcL_Id1 == "" && $scope.asmS_Id1 == "") {
                swal("Please Select The Academic Year,Category,Class And Section !");
            }
            else if ($scope.asmaY_Id1 != "" && $scope.ttmC_Id1 != "" && $scope.asmcL_Id1 != "" && $scope.asmS_Id1 != "" && $scope.hrmE_Id1 != "") {
                var data = {
                    // "TTMC_Id": $scope.ttmC_Id1,
                    "ASMAY_Id": $scope.asmaY_Id1,
                    "TTMC_Id": $scope.ttmC_Id1,
                    "ASMCL_Id": $scope.asmcL_Id1,
                    "ASMS_Id": $scope.asmS_Id1,
                    "HRME_Id": $scope.hrmE_Id1,
                }
                apiService.create("Fixing/getsubject_staff", data).
         then(function (promise) {

             $scope.subject_list1 = promise.subjectbystaff;

             $scope.ismS_Id1 = "";

             if ($scope.TTFDP_Id != "" && $scope.TTFDP_Id != 0) {
                 angular.forEach($scope.subject_list1, function (role) {
                     
                     if (role.ismS_Id == $scope.temp_subject1) {
                         $scope.ismS_Id1 = role.ismS_Id;
                         role.Selected = true;
                     }
                 })
             }

             if (promise.subjectbystaff == "" || promise.subjectbystaff == null) {
                 swal("No subject Are Mapped To Selected Staff");
             }
         })
            }
        }

        //active switch1 for day-period
        $scope.switch1 = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfdP_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("Fixing/deactivate1", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }
        //for day-staff fixing
        $scope.switch2 = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfdS_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("Fixing/deactivate2", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        //for day-SUBJECT fixing
        $scope.switch3 = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfdsU_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("Fixing/deactivate3", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }


        //for day-staff fixing
        $scope.switch4 = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfpS_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("Fixing/deactivate4", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        //for day-staff fixing
        $scope.switch5 = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfpsU_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";



            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("Fixing/deactivate5", employee).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal(confirmmgs + " " + "successfully.");
                    }
                    else {
                        swal(confirmmgs + " " + " successfully");
                    }
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }



        $scope.asmaY_Id2 = "";
        // $scope.ttmC_Id1 = "";
        $scope.get_cls_sec_sub2 = function () {
            
            if ($scope.asmaY_Id2 == "") {
                swal("Please Select The Academic Year !");
                $scope.hrmE_Id2 = "";
            }
            else if ($scope.asmaY_Id2 != "" && $scope.hrmE_Id2 != "") {
                var data = {
                    "HRME_Id": $scope.hrmE_Id2,
                    "ASMAY_Id": $scope.asmaY_Id2,
                }
                apiService.create("Fixing/get_cls_sec_subbystaff", data).
         then(function (promise) {

             $scope.class_list2 = promise.clssbystaff;
             $scope.section_list2 = promise.secsbystaff;
             $scope.subject_list2 = promise.subsbystaff;

             $scope.asmcL_Id2 = "";
             $scope.asmS_Id2 = "";
             $scope.ismS_Id2 = "";
             $scope.NOP_2 = 0;
             //if ($scope.TTFDS_Id != "" && $scope.TTFDS_Id != 0) {
             //    angular.forEach($scope.class_list2, function (role) {
             //        
             //        if (role.asmcL_Id == $scope.temp_class2) {                                               
             //        }
             //    })
             //}

             if (promise.clssbystaff == "" || promise.clssbystaff == null) {
                 swal("No Classes Are Mapped To Selected Staff");
             }
             if (promise.secsbystaff == "" || promise.secsbystaff == null) {
                 swal("No Sections Are Mapped To Selected Staff");
             }
             if (promise.subsbystaff == "" || promise.subsbystaff == null) {
                 swal("No Subjects Are Mapped To Selected Staff");
             }
         })
            }
        }

        $scope.asmaY_Id3 = "";
        // $scope.ttmC_Id1 = "";
        $scope.get_cls_sec_staff3 = function () {
            
            if ($scope.asmaY_Id3 == "") {
                swal("Please Select The Academic Year !");
                //$scope.hrmE_Id2 = "";
                $scope.ismS_Id3 = "";
            }
            else if ($scope.asmaY_Id3 != "" && $scope.ismS_Id3 != "") {
                var data = {
                    "ISMS_Id": $scope.ismS_Id3,
                    "ASMAY_Id": $scope.asmaY_Id3,
                }
                apiService.create("Fixing/get_cls_sec_staffbysub", data).
         then(function (promise) {

             $scope.class_list3 = promise.clssbysub;
             $scope.section_list3 = promise.secsbysub;
             // $scope.subject_list3 = promise.subsbystaff;
             $scope.staff_list3 = promise.staffbysub;

             $scope.asmcL_Id3 = "";
             $scope.asmS_Id3 = "";
             $scope.hrmE_Id3 = "";
             $scope.NOP_3 = 0;
             //if ($scope.TTFDS_Id != "" && $scope.TTFDS_Id != 0) {
             //    angular.forEach($scope.class_list2, function (role) {
             //        
             //        if (role.asmcL_Id == $scope.temp_class2) {                                               
             //        }
             //    })
             //}

             if (promise.clssbysub == "" || promise.clssbysub == null) {
                 swal("No Classes Are Mapped To Selected Subject");
             }
             if (promise.secsbysub == "" || promise.secsbysub == null) {
                 swal("No Sections Are Mapped To Selected Subject");
             }
             if (promise.secsbysub == "" || promise.secsbysub == null) {
                 swal("No Staffs Are Mapped To Selected Subject");
             }
         })
            }
        }



        $scope.asmaY_Id4 = "";
        // $scope.ttmC_Id1 = "";
        $scope.get_cls_sec_sub4 = function () {
            
            if ($scope.asmaY_Id4 == "") {
                swal("Please Select The Academic Year !");
                $scope.hrmE_Id4 = "";
            }
            else if ($scope.asmaY_Id4 != "" && $scope.hrmE_Id4 != "") {
                var data = {
                    "HRME_Id": $scope.hrmE_Id4,
                    "ASMAY_Id": $scope.asmaY_Id4,
                }
                apiService.create("Fixing/get_cls_sec_subbystaff", data).
         then(function (promise) {

             $scope.class_list4 = promise.clssbystaff;
             $scope.section_list4 = promise.secsbystaff;
             $scope.subject_list4 = promise.subsbystaff;

             $scope.asmcL_Id4 = "";
             $scope.asmS_Id4 = "";
             $scope.ismS_Id4 = "";
             $scope.NOD_4 = 0;
             //if ($scope.TTFDS_Id != "" && $scope.TTFDS_Id != 0) {
             //    angular.forEach($scope.class_list2, function (role) {
             //        
             //        if (role.asmcL_Id == $scope.temp_class2) {                                               
             //        }
             //    })
             //}

             if (promise.clssbystaff == "" || promise.clssbystaff == null) {
                 swal("No Classes Are Mapped To Selected Staff");
             }
             if (promise.secsbystaff == "" || promise.secsbystaff == null) {
                 swal("No Sections Are Mapped To Selected Staff");
             }
             if (promise.subsbystaff == "" || promise.subsbystaff == null) {
                 swal("No Subjects Are Mapped To Selected Staff");
             }
         })
            }
        }


        $scope.asmaY_Id5 = "";
        // $scope.ttmC_Id1 = "";
        $scope.get_cls_sec_staff5 = function () {
            
            if ($scope.asmaY_Id5 == "") {
                swal("Please Select The Academic Year !");
                //$scope.hrmE_Id2 = "";
                $scope.ismS_Id5 = "";
            }
            else if ($scope.asmaY_Id5 != "" && $scope.ismS_Id5 != "") {
                var data = {
                    "ISMS_Id": $scope.ismS_Id5,
                    "ASMAY_Id": $scope.asmaY_Id5,
                }
                apiService.create("Fixing/get_cls_sec_staffbysub", data).
         then(function (promise) {

             $scope.class_list5 = promise.clssbysub;
             $scope.section_list5 = promise.secsbysub;
             // $scope.subject_list3 = promise.subsbystaff;
             $scope.staff_list5 = promise.staffbysub;

             $scope.asmcL_Id5 = "";
             $scope.asmS_Id5 = "";
             $scope.hrmE_Id5 = "";
             $scope.NOD_5 = 0;
             //if ($scope.TTFDS_Id != "" && $scope.TTFDS_Id != 0) {
             //    angular.forEach($scope.class_list2, function (role) {
             //        
             //        if (role.asmcL_Id == $scope.temp_class2) {                                               
             //        }
             //    })
             //}

             if (promise.clssbysub == "" || promise.clssbysub == null) {
                 swal("No Classes Are Mapped To Selected Subject");
             }
             if (promise.secsbysub == "" || promise.secsbysub == null) {
                 swal("No Sections Are Mapped To Selected Subject");
             }
             if (promise.secsbysub == "" || promise.secsbysub == null) {
                 swal("No Staffs Are Mapped To Selected Subject");
             }
         })
            }
        }

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("Fixing/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.peds_count = promise.period_count;
           $scope.days_count = promise.day_count;
           $scope.year_list = promise.acayear;
           $scope.day_list = promise.daylist;
           $scope.temp_classlist = promise.classlist;
           $scope.temp_sectionlist = promise.sectionlist;
           $scope.temp_stafflist = promise.stafflist;
           $scope.temp_subjectlist = promise.subjectlist;
           $scope.temp_periodlist = promise.periodlist;
           $scope.class_list1 = promise.classlist;
           $scope.class_list2 = promise.classlist;
           $scope.class_list3 = promise.classlist;
           $scope.class_list4 = promise.classlist;
           $scope.class_list5 = promise.classlist;
           $scope.staff_list5 = promise.stafflist;
           $scope.staff_list1 = promise.stafflist;
           $scope.staff_list2 = promise.stafflist;
           $scope.staff_list3 = promise.stafflist;
           $scope.staff_list4 = promise.stafflist;
           $scope.category_list1 = promise.categorylist;
           // $scope.category_list2 = promise.categorylist;
           // $scope.category_list3 = promise.categorylist;
           $scope.period_list1 = promise.periodlist;
           $scope.period_list4 = promise.periodlist;
           $scope.period_list5 = promise.periodlist;
           $scope.section_list5 = promise.sectionlist;
           $scope.section_list1 = promise.sectionlist;
           $scope.section_list2 = promise.sectionlist;
           $scope.section_list3 = promise.sectionlist;
           $scope.section_list4 = promise.sectionlist;
           $scope.subject_list5 = promise.subjectlist;
           $scope.subject_list1 = promise.subjectlist;
           $scope.subject_list2 = promise.subjectlist;
           $scope.subject_list3 = promise.subjectlist;
           $scope.subject_list4 = promise.subjectlist;
           $scope.gridOptions1.data = promise.fix_day_period_list;
           $scope.gridOptions2.data = promise.all_fix_day_staff_list;
           $scope.gridOptions3.data = promise.all_fix_day_subject_list;
           $scope.gridOptions4.data = promise.all_fix_period_staff_list;
           $scope.gridOptions5.data = promise.all_fix_period_subject_list;
           $scope.gridOptions.data = promise.categorylist;
       })
        };
        //TO clear  data
        $scope.clearid = function () {
            $scope.TTMC_CategoryName = "";
            $scope.TTMC_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";


        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Fixing/getdetails", pageid).
            then(function (promise) {

                $scope.TTMC_CategoryName = promise.categorylistedit[0].ttmC_CategoryName;
                $scope.TTMC_Id = promise.categorylistedit[0].ttmC_Id;
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmC_Id;
            var pageid = $scope.editEmployee;
            swal({
                title: "Are you sure",
                text: "Do you want to delete record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("Fixing/deletepages", pageid).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                        }
                        $scope.BindData();
                    })
                    $scope.BindData();
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        };

        $scope.interacted1 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };
        $scope.interacted3 = function (field) {

            return $scope.submitted3 || field.$dirty;
        };
        $scope.interacted4 = function (field) {

            return $scope.submitted4 || field.$dirty;
        };
        $scope.interacted5 = function (field) {

            return $scope.submitted5 || field.$dirty;
        };

        $scope.NOP_2 = 0;
        $scope.NOP_3 = 0;
        $scope.NOD_4 = 0;
        $scope.NOD_5 = 0;
        $scope.cs_flag2 = 0;
        $scope.cs_flag3 = 0;
        $scope.cs_flag4 = 0;
        $scope.cs_flag5 = 0;
        $scope.CSwise_flag2 = true;
        $scope.CSwise_flag3 = true;
        $scope.CSwise_flag4 = true;
        $scope.CSwise_flag5 = true;
        $scope.CSwise2 = function () {
            if ($scope.cs_flag2 == 1) {
                $scope.CSwise_flag2 = false;
                //if ($scope.temp_array2 != "" && $scope.temp_arraysaveDB2 != "") {
                //     $scope.albumNameArray2 = $scope.temp_array2;
                //     $scope.albumNameArraysaveDB2 = $scope.temp_arraysaveDB2;
                //     $scope.gridOptions2_sub.data = $scope.albumNameArray2;
                //}

            }
            else if ($scope.cs_flag2 == 0) {
                $scope.CSwise_flag2 = true;
                //$scope.temp_array2 = $scope.albumNameArray2;
                //$scope.temp_arraysaveDB2 = $scope.albumNameArraysaveDB2;
                $scope.albumNameArray2 = [];
                $scope.albumNameArraysaveDB2 = [];
                $scope.gridOptions2_sub.data = $scope.albumNameArray2;

            }
        }
        $scope.CSwise3 = function () {
            if ($scope.cs_flag3 == 1) {
                $scope.CSwise_flag3 = false;
            }
            else if ($scope.cs_flag3 == 0) {
                $scope.CSwise_flag3 = true;
                $scope.albumNameArray3 = [];
                $scope.albumNameArraysaveDB3 = [];
                $scope.gridOptions3_sub.data = $scope.albumNameArray3;
            }
        }
        $scope.CSwise4 = function () {
            if ($scope.cs_flag4 == 1) {
                $scope.CSwise_flag4 = false;
                //$scope.albumNameArray4 = $scope.temp_array4;
                //$scope.albumNameArraysaveDB4 = $scope.temp_arraysaveDB4;
                //$scope.gridOptions4_sub.data = $scope.albumNameArray4;
            }
            else if ($scope.cs_flag4 == 0) {
                $scope.CSwise_flag4 = true;
                //$scope.temp_array4 = $scope.albumNameArray4;
                //$scope.temp_arraysaveDB4 = $scope.albumNameArraysaveDB4;
                $scope.albumNameArray4 = [];
                $scope.albumNameArraysaveDB4 = [];
                $scope.gridOptions4_sub.data = $scope.albumNameArray4;

            }
        }
        $scope.CSwise5 = function () {
            if ($scope.cs_flag5 == 1) {
                $scope.CSwise_flag5 = false;
            }
            else if ($scope.cs_flag5 == 0) {
                $scope.CSwise_flag5 = true;
                $scope.albumNameArray5 = [];
                $scope.albumNameArraysaveDB5 = [];
                $scope.gridOptions5_sub.data = $scope.albumNameArray5;
            }
        }




        //TO clear  data
        $scope.clear1 = function () {
            $scope.TTFDP_Id = 0;
            $scope.class_list1 = $scope.temp_classlist;
            $scope.section_list1 = $scope.temp_sectionlist;
            $scope.staff_list1 = $scope.temp_stafflist;
            $scope.subject_list1 = $scope.temp_subjectlist;
            $scope.period_list1 = $scope.temp_periodlist;
            $scope.asmaY_Id1 = "";
            $scope.ttmD_Id1 = "";
            $scope.asmcL_Id1 = "";
            $scope.hrmE_Id1 = "";
            $scope.ttmC_Id1 = "";
            $scope.ttmP_Id1 = "";
            $scope.asmS_Id1 = "";
            $scope.ismS_Id1 = "";
            //  $scope.TTMC_CategoryName = "";
            // $scope.TTMC_Id = 0; 
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";
        };


        $scope.clear2 = function () {
            
            // $scope.TTLAB_LABLIBName = "";
            // $scope.popup = false;
            $scope.TTFDS_Id = 0;
            $scope.TTFDSCC_Id = 0;
            $scope.asmaY_Id2 = "";
            $scope.ttmD_Id2 = "";
            $scope.ismS_Id2 = "";
            $scope.cs_flag2 = 0;
            $scope.CSwise2();
            $scope.class_list2 = $scope.temp_classlist;
            $scope.section_list2 = $scope.temp_sectionlist;
            $scope.staff_list2 = $scope.temp_stafflist;
            $scope.subject_list2 = $scope.temp_subjectlist;
            $scope.albumNameArray2 = [];
            $scope.albumNameArraysaveDB2 = [];
            $scope.gridOptions2_sub.data = $scope.albumNameArray2;
            //$scope.TTLAB_Id = 0;
            $scope.submitted2 = false;
            $scope.asmcL_Id2 = "";
            $scope.asmS_Id2 = "";
            $scope.hrmE_Id2 = "";
            $scope.NOP_2 = 0;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
        };
        $scope.clear3 = function () {
            
            // $scope.TTLAB_LABLIBName = "";
            // $scope.popup = false;
            $scope.TTFDSU_Id = 0;
            $scope.TTFDSUCC_Id = 0;
            $scope.asmaY_Id3 = "";
            $scope.ttmD_Id3 = "";
            $scope.ismS_Id3 = "";
            $scope.cs_flag3 = 0;
            $scope.CSwise3();
            $scope.class_list3 = $scope.temp_classlist;
            $scope.section_list3 = $scope.temp_sectionlist;
            $scope.staff_list3 = $scope.temp_stafflist;
            $scope.subject_list3 = $scope.temp_subjectlist;
            $scope.albumNameArray3 = [];
            $scope.albumNameArraysaveDB3 = [];
            $scope.gridOptions3_sub.data = $scope.albumNameArray3;
            //$scope.TTLAB_Id = 0;
            $scope.submitted3 = false;
            $scope.asmcL_Id3 = "";
            $scope.asmS_Id3 = "";
            $scope.hrmE_Id3 = "";
            $scope.NOP_3 = 0;
            $scope.myForm3.$setPristine();
            $scope.myForm3.$setUntouched();
            $scope.search = "";
        };

        $scope.clear4 = function () {
            
            // $scope.TTLAB_LABLIBName = "";
            // $scope.popup = false;
            $scope.TTFPS_Id = 0;
            $scope.TTFPSCC_Id = 0;
            $scope.asmaY_Id4 = "";
            $scope.ttmP_Id4 = "";
            $scope.ismS_Id4 = "";
            $scope.cs_flag4 = 0;
            $scope.CSwise4();
            $scope.class_list4 = $scope.temp_classlist;
            $scope.section_list4 = $scope.temp_sectionlist;
            $scope.staff_list4 = $scope.temp_stafflist;
            $scope.subject_list4 = $scope.temp_subjectlist;
            $scope.albumNameArray4 = [];
            $scope.albumNameArraysaveDB4 = [];
            $scope.gridOptions4_sub.data = $scope.albumNameArray4;
            //$scope.TTLAB_Id = 0;
            $scope.submitted4 = false;
            $scope.asmcL_Id4 = "";
            $scope.asmS_Id4 = "";
            $scope.hrmE_Id4 = "";
            $scope.NOD_4 = 0;
            $scope.myForm4.$setPristine();
            $scope.myForm4.$setUntouched();
            $scope.search = "";
        };
        $scope.clear5 = function () {
            
            // $scope.TTLAB_LABLIBName = "";
            // $scope.popup = false;
            $scope.TTFPSU_Id = 0;
            $scope.TTFPSUCC_Id = 0;
            $scope.asmaY_Id5 = "";
            $scope.ttmP_Id5 = "";
            $scope.ismS_Id5 = "";
            $scope.cs_flag5 = 0;
            $scope.CSwise5();
            $scope.class_list5 = $scope.temp_classlist;
            $scope.section_list5 = $scope.temp_sectionlist;
            $scope.staff_list5 = $scope.temp_stafflist;
            $scope.subject_list5 = $scope.temp_subjectlist;
            $scope.albumNameArray5 = [];
            $scope.albumNameArraysaveDB5 = [];
            $scope.gridOptions5_sub.data = $scope.albumNameArray5;
            //$scope.TTLAB_Id = 0;
            $scope.submitted5 = false;
            $scope.asmcL_Id5 = "";
            $scope.asmS_Id5 = "";
            $scope.hrmE_Id5 = "";
            $scope.NOD_5 = 0;
            $scope.myForm5.$setPristine();
            $scope.myForm5.$setUntouched();
            $scope.search = "";
        };



        $scope.albumNameArray2 = [];
        $scope.albumNameArraysaveDB2 = [];
        $scope.editEmployee = {};
        var cls2, sect2, subj2, period2;
        var cls21, sect21, subj21, period21;
        $scope.TransferDatagrid2 = function (objcls, objsect, objsub, objped) {
            


            if ($scope.asmcL_Id2 === undefined || $scope.asmS_Id2 === undefined || $scope.ismS_Id2 === undefined || $scope.NOP_2 === undefined || $scope.asmcL_Id2 === "" || $scope.asmS_Id2 === "" || $scope.ismS_Id2 === "" || $scope.NOP_2 === "" || $scope.NOP_2 === 0) {
                swal('Please Select Feilds Class,Section,Subject And Enter Period (Not Zero) !');
            }
            else if ($scope.peds_count < parseInt($scope.NOP_2)) {
                swal("No Of Periods Can't Exceed The Total of Daily Periods");
            }
            else {

                for (var k = 0; k < $scope.class_list2.length; k++) {
                    if ($scope.class_list2[k].asmcL_Id == objcls) {
                        cls2 = $scope.class_list2[k].asmcL_ClassName;
                    }
                }
                for (var k = 0; k < $scope.section_list2.length; k++) {
                    if ($scope.section_list2[k].asmS_Id == objsect) {
                        sect2 = $scope.section_list2[k].asmC_SectionName;
                    }
                }

                for (var k = 0; k < $scope.subject_list2.length; k++) {
                    if ($scope.subject_list2[k].ismS_Id == objsub) {
                        subj2 = $scope.subject_list2[k].ismS_SubjectName;
                    }
                }
                //  period =  $scope.NOP_2;
                period2 = parseInt($scope.NOP_2);

                if ($scope.albumNameArray2.length === 0) {
                    
                    $scope.albumNameArray2.push({ clsDisplay: cls2, sectDisplay: sect2, subDisplay: subj2, pedDisplay: period2 });
                    $scope.albumNameArraysaveDB2.push({ ASMCL_Id: $scope.asmcL_Id2, ASMS_Id: $scope.asmS_Id2, ISMS_Id: $scope.ismS_Id2, NOP: parseInt($scope.NOP_2) });

                    // Clear input fields after push

                    $scope.asmcL_Id2 = "";
                    $scope.asmS_Id2 = "";
                    $scope.ismS_Id2 = "";
                    $scope.NOP_2 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray2.length; k++) {
                        
                        if ($scope.albumNameArray2[k].clsDisplay === cls2 && $scope.albumNameArray2[k].sectDisplay === sect2 && $scope.albumNameArray2[k].subDisplay === subj2) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray2.push({ clsDisplay: cls2, sectDisplay: sect2, subDisplay: subj2, pedDisplay: period2 });
                        $scope.albumNameArraysaveDB2.push({ ASMCL_Id: $scope.asmcL_Id2, ASMS_Id: $scope.asmS_Id2, ISMS_Id: $scope.ismS_Id2, NOP: parseInt($scope.NOP_2) });

                        // Clear input fields after push

                        $scope.asmcL_Id2 = "";
                        $scope.asmS_Id2 = "";
                        $scope.ismS_Id2 = "";
                        $scope.NOP_2 = 0;
                        condition = 1;
                    }
                }
                //}
            }

            $scope.gridOptions2_sub.data = $scope.albumNameArray2;

        };

        $scope.albumNameArray3 = [];
        $scope.albumNameArraysaveDB3 = [];
        $scope.editEmployee = {};
        var cls3, sect3, staff3, period3;
        var cls31, sect31, staff31, period31;
        $scope.TransferDatagrid3 = function (objcls, objsect, objstaff, objped) {
            


            if ($scope.asmcL_Id3 === undefined || $scope.asmS_Id3 === undefined || $scope.hrmE_Id3 === undefined || $scope.NOP_3 === undefined || $scope.asmcL_Id3 === "" || $scope.asmS_Id3 === "" || $scope.hrmE_Id3 === "" || $scope.NOP_3 === "" || $scope.NOP_3 === 0) {
                swal('Please Select Feilds Class,Section,Staff And Enter Period (Not Zero) !');
            }
            else if ($scope.peds_count < parseInt($scope.NOP_3)) {
                swal("No Of Periods Can't Exceed The Total of Daily Periods");
            }
            else {

                for (var k = 0; k < $scope.class_list3.length; k++) {
                    if ($scope.class_list3[k].asmcL_Id == objcls) {
                        cls3 = $scope.class_list3[k].asmcL_ClassName;
                    }
                }
                for (var k = 0; k < $scope.section_list3.length; k++) {
                    if ($scope.section_list3[k].asmS_Id == objsect) {
                        sect3 = $scope.section_list3[k].asmC_SectionName;
                    }
                }

                for (var k = 0; k < $scope.staff_list3.length; k++) {
                    if ($scope.staff_list3[k].hrmE_Id == objstaff) {
                        staff3 = $scope.staff_list3[k].staffName;
                    }
                }
                //  period =  $scope.NOP_2;
                period3 = parseInt($scope.NOP_3);

                if ($scope.albumNameArray3.length === 0) {
                    
                    $scope.albumNameArray3.push({ clsDisplay: cls3, sectDisplay: sect3, staffDisplay: staff3, pedDisplay: period3 });
                    $scope.albumNameArraysaveDB3.push({ ASMCL_Id: $scope.asmcL_Id3, ASMS_Id: $scope.asmS_Id3, HRME_Id: $scope.hrmE_Id3, NOP: parseInt($scope.NOP_3) });

                    // Clear input fields after push

                    $scope.asmcL_Id3 = "";
                    $scope.asmS_Id3 = "";
                    //$scope.ismS_Id2 = "";
                    $scope.hrmE_Id3 = "";
                    $scope.NOP_3 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray3.length; k++) {
                        
                        if ($scope.albumNameArray3[k].clsDisplay === cls3 && $scope.albumNameArray3[k].sectDisplay === sect3 && $scope.albumNameArray3[k].staffDisplay === staff3) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray3.push({ clsDisplay: cls3, sectDisplay: sect3, staffDisplay: staff3, pedDisplay: period3 });
                        $scope.albumNameArraysaveDB3.push({ ASMCL_Id: $scope.asmcL_Id3, ASMS_Id: $scope.asmS_Id3, HRME_Id: $scope.hrmE_Id3, NOP: parseInt($scope.NOP_3) });

                        // Clear input fields after push

                        $scope.asmcL_Id3 = "";
                        $scope.asmS_Id3 = "";
                        //$scope.ismS_Id3 = "";
                        $scope.hrmE_Id3 = "";
                        $scope.NOP_3 = 0;
                        condition = 1;
                    }
                }
                //}
            }

            $scope.gridOptions3_sub.data = $scope.albumNameArray3;

        };



        $scope.albumNameArray4 = [];
        $scope.albumNameArraysaveDB4 = [];
        $scope.editEmployee = {};
        var cls4, sect4, subj4, day4;
        var cls41, sect41, subj41, day41;
        $scope.TransferDatagrid4 = function (objcls, objsect, objsub, objday) {
            


            if ($scope.asmcL_Id4 === undefined || $scope.asmS_Id4 === undefined || $scope.ismS_Id4 === undefined || $scope.NOD_4 === undefined || $scope.asmcL_Id4 === "" || $scope.asmS_Id4 === "" || $scope.ismS_Id4 === "" || $scope.NOD_4 === "" || $scope.NOD_4 === 0) {
                swal('Please Select Feilds Class,Section,Subject And Enter No Of Days (Not Zero) !');
            }
            else if ($scope.days_count < parseInt($scope.NOD_4)) {
                swal("No Of Days Can't Exceed The School Days Per Week !!!!");
            }
            else {

                for (var k = 0; k < $scope.class_list4.length; k++) {
                    if ($scope.class_list4[k].asmcL_Id == objcls) {
                        cls4 = $scope.class_list4[k].asmcL_ClassName;
                    }
                }
                for (var k = 0; k < $scope.section_list4.length; k++) {
                    if ($scope.section_list4[k].asmS_Id == objsect) {
                        sect4 = $scope.section_list4[k].asmC_SectionName;
                    }
                }

                for (var k = 0; k < $scope.subject_list4.length; k++) {
                    if ($scope.subject_list4[k].ismS_Id == objsub) {
                        subj4 = $scope.subject_list4[k].ismS_SubjectName;
                    }
                }
                //  period =  $scope.NOP_2;
                day4 = parseInt($scope.NOD_4);

                if ($scope.albumNameArray4.length === 0) {
                    
                    $scope.albumNameArray4.push({ clsDisplay: cls4, sectDisplay: sect4, subDisplay: subj4, dayDisplay: day4 });
                    $scope.albumNameArraysaveDB4.push({ ASMCL_Id: $scope.asmcL_Id4, ASMS_Id: $scope.asmS_Id4, ISMS_Id: $scope.ismS_Id4, NOD: parseInt($scope.NOD_4) });

                    // Clear input fields after push

                    $scope.asmcL_Id4 = "";
                    $scope.asmS_Id4 = "";
                    $scope.ismS_Id4 = "";
                    $scope.NOD_4 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray4.length; k++) {
                        
                        if ($scope.albumNameArray4[k].clsDisplay === cls4 && $scope.albumNameArray4[k].sectDisplay === sect4 && $scope.albumNameArray4[k].subDisplay === subj4) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray4.push({ clsDisplay: cls4, sectDisplay: sect4, subDisplay: subj4, dayDisplay: day4 });
                        $scope.albumNameArraysaveDB4.push({ ASMCL_Id: $scope.asmcL_Id4, ASMS_Id: $scope.asmS_Id4, ISMS_Id: $scope.ismS_Id4, NOD: parseInt($scope.NOD_4) });

                        // Clear input fields after push

                        $scope.asmcL_Id4 = "";
                        $scope.asmS_Id4 = "";
                        $scope.ismS_Id4 = "";
                        $scope.NOD_4 = 0;
                        condition = 1;
                    }
                }
                //}
            }

            $scope.gridOptions4_sub.data = $scope.albumNameArray4;

        };


        $scope.albumNameArray5 = [];
        $scope.albumNameArraysaveDB5 = [];
        $scope.editEmployee = {};
        var cls5, sect5, staff5, day5;
        var cls51, sect51, staff51, day51;
        $scope.TransferDatagrid5 = function (objcls, objsect, objstaff, objday) {
            


            if ($scope.asmcL_Id5 === undefined || $scope.asmS_Id5 === undefined || $scope.hrmE_Id5 === undefined || $scope.NOD_5 === undefined || $scope.asmcL_Id5 === "" || $scope.asmS_Id5 === "" || $scope.hrmE_Id5 === "" || $scope.NOD_5 === "" || $scope.NOD_5 === 0) {
                swal('Please Select Feilds Class,Section,Staff And Enter Day (Not Zero) !');
            }
            else if ($scope.days_count < parseInt($scope.NOD_5)) {
                swal("No Of Days Can't Exceed The School Days Per Week !!!!");
            }
            else {

                for (var k = 0; k < $scope.class_list5.length; k++) {
                    if ($scope.class_list5[k].asmcL_Id == objcls) {
                        cls5 = $scope.class_list5[k].asmcL_ClassName;
                    }
                }
                for (var k = 0; k < $scope.section_list5.length; k++) {
                    if ($scope.section_list5[k].asmS_Id == objsect) {
                        sect5 = $scope.section_list5[k].asmC_SectionName;
                    }
                }

                for (var k = 0; k < $scope.staff_list5.length; k++) {
                    if ($scope.staff_list5[k].hrmE_Id == objstaff) {
                        staff5 = $scope.staff_list5[k].staffName;
                    }
                }
                //  period =  $scope.NOP_2;
                day5 = parseInt($scope.NOD_5);

                if ($scope.albumNameArray5.length === 0) {
                    
                    $scope.albumNameArray5.push({ clsDisplay: cls5, sectDisplay: sect5, staffDisplay: staff5, dayDisplay: day5 });
                    $scope.albumNameArraysaveDB5.push({ ASMCL_Id: $scope.asmcL_Id5, ASMS_Id: $scope.asmS_Id5, HRME_Id: $scope.hrmE_Id5, NOD: parseInt($scope.NOD_5) });

                    // Clear input fields after push

                    $scope.asmcL_Id5 = "";
                    $scope.asmS_Id5 = "";
                    //$scope.ismS_Id2 = "";
                    $scope.hrmE_Id5 = "";
                    $scope.NOD_5 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray5.length; k++) {
                        
                        if ($scope.albumNameArray5[k].clsDisplay === cls5 && $scope.albumNameArray5[k].sectDisplay === sect5 && $scope.albumNameArray5[k].staffDisplay === staff5) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray5.push({ clsDisplay: cls5, sectDisplay: sect5, staffDisplay: staff5, dayDisplay: day5 });
                        $scope.albumNameArraysaveDB5.push({ ASMCL_Id: $scope.asmcL_Id5, ASMS_Id: $scope.asmS_Id5, HRME_Id: $scope.hrmE_Id5, NOD: parseInt($scope.NOD_5) });

                        // Clear input fields after push

                        $scope.asmcL_Id5 = "";
                        $scope.asmS_Id5 = "";
                        //$scope.ismS_Id3 = "";
                        $scope.hrmE_Id5 = "";
                        $scope.NOD_5 = 0;
                        condition = 1;
                    }
                }
                //}
            }

            $scope.gridOptions5_sub.data = $scope.albumNameArray5;

        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid2 = function (employee) {
            
            //$scope.editEmployee = employee.TTLABD_Id;
            //$scope.remove = function (project) {
            //    $scope.gridOptions1.data.splice($scope.gridOptions1.data.indexOf(project), 1);
            //}

            var pageid1 = employee.clsDisplay;
            var pageid2 = employee.sectDisplay;
            var pageid3 = employee.subDisplay;
            var pageid4 = employee.pedDisplay;


            for (var a = 0; a < $scope.class_list2.length; a++) {
                if ($scope.class_list2[a].asmcL_ClassName == pageid1) {
                    
                    cls21 = $scope.class_list2[a].asmcL_Id;
                }
            }
            for (var b = 0; b < $scope.section_list2.length; b++) {
                if ($scope.section_list2[b].asmC_SectionName == pageid2) {
                    
                    sect21 = $scope.section_list2[b].asmS_Id;
                }
            }
            //for (var k = 0; k < $scope.stflst.length; k++) {
            //    if ($scope.stflst[k].ivrmstauL_Name == pageid3) {
            //        staff1 = $scope.stflst[k].ivrmstauL_Id;
            //    }
            //}
            for (var c = 0; c < $scope.subject_list2.length; c++) {
                if ($scope.subject_list2[c].ismS_SubjectName == pageid3) {
                    
                    subj21 = $scope.subject_list2[c].ismS_Id;
                }
            }
            period21 = pageid4;

            for (var i = $scope.albumNameArraysaveDB2.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArraysaveDB2[i].ASMCL_Id == cls21 && $scope.albumNameArraysaveDB2[i].ASMS_Id == sect21 && $scope.albumNameArraysaveDB2[i].ISMS_Id == subj21 && $scope.albumNameArraysaveDB2[i].NOP == period21) {
                    
                    // pedcount -= $scope.albumNameArraysaveDB[i].NOP;
                    $scope.albumNameArraysaveDB2.splice(i, 1);

                }
            }

            for (var i = $scope.albumNameArray2.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArray2[i].clsDisplay == pageid1 && $scope.albumNameArray2[i].sectDisplay == pageid2 && $scope.albumNameArray2[i].pedDisplay == pageid4 && $scope.albumNameArray2[i].subDisplay == pageid3) {
                    $scope.albumNameArray2.splice(i, 1);

                }
            }
            $scope.gridOptions2_sub.data = $scope.albumNameArray2;

        };


        //TO  delete Record Right grid
        $scope.deletedatarightgrid3 = function (employee) {
            
            //$scope.editEmployee = employee.TTLABD_Id;
            //$scope.remove = function (project) {
            //    $scope.gridOptions1.data.splice($scope.gridOptions1.data.indexOf(project), 1);
            //}

            var pageid1 = employee.clsDisplay;
            var pageid2 = employee.sectDisplay;
            var pageid3 = employee.staffDisplay;
            var pageid4 = employee.pedDisplay;


            for (var a = 0; a < $scope.class_list3.length; a++) {
                if ($scope.class_list3[a].asmcL_ClassName == pageid1) {
                    
                    cls31 = $scope.class_list3[a].asmcL_Id;
                }
            }
            for (var b = 0; b < $scope.section_list3.length; b++) {
                if ($scope.section_list3[b].asmC_SectionName == pageid2) {
                    
                    sect31 = $scope.section_list3[b].asmS_Id;
                }
            }
            //for (var k = 0; k < $scope.stflst.length; k++) {
            //    if ($scope.stflst[k].ivrmstauL_Name == pageid3) {
            //        staff1 = $scope.stflst[k].ivrmstauL_Id;
            //    }
            //}
            for (var c = 0; c < $scope.staff_list3.length; c++) {
                if ($scope.staff_list3[c].staffName == pageid3) {
                    
                    staff31 = $scope.staff_list3[c].hrmE_Id;
                }
            }
            period31 = pageid4;

            for (var i = $scope.albumNameArraysaveDB3.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArraysaveDB3[i].ASMCL_Id == cls31 && $scope.albumNameArraysaveDB3[i].ASMS_Id == sect31 && $scope.albumNameArraysaveDB3[i].HRME_Id == staff31 && $scope.albumNameArraysaveDB3[i].NOP == period31) {
                    
                    // pedcount -= $scope.albumNameArraysaveDB[i].NOP;
                    $scope.albumNameArraysaveDB3.splice(i, 1);

                }
            }

            for (var i = $scope.albumNameArray3.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArray3[i].clsDisplay == pageid1 && $scope.albumNameArray3[i].sectDisplay == pageid2 && $scope.albumNameArray3[i].pedDisplay == pageid4 && $scope.albumNameArray3[i].staffDisplay == pageid3) {
                    $scope.albumNameArray3.splice(i, 1);

                }
            }
            $scope.gridOptions3_sub.data = $scope.albumNameArray3;

        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid4 = function (employee) {
            


            var pageid1 = employee.clsDisplay;
            var pageid2 = employee.sectDisplay;
            var pageid3 = employee.subDisplay;
            var pageid4 = employee.dayDisplay;


            for (var a = 0; a < $scope.class_list4.length; a++) {
                if ($scope.class_list4[a].asmcL_ClassName == pageid1) {
                    
                    cls41 = $scope.class_list4[a].asmcL_Id;
                }
            }
            for (var b = 0; b < $scope.section_list4.length; b++) {
                if ($scope.section_list4[b].asmC_SectionName == pageid2) {
                    
                    sect41 = $scope.section_list4[b].asmS_Id;
                }
            }

            for (var c = 0; c < $scope.subject_list4.length; c++) {
                if ($scope.subject_list4[c].ismS_SubjectName == pageid3) {
                    
                    subj41 = $scope.subject_list4[c].ismS_Id;
                }
            }
            day41 = pageid4;

            for (var i = $scope.albumNameArraysaveDB4.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArraysaveDB4[i].ASMCL_Id == cls41 && $scope.albumNameArraysaveDB4[i].ASMS_Id == sect41 && $scope.albumNameArraysaveDB4[i].ISMS_Id == subj41 && $scope.albumNameArraysaveDB4[i].NOD == day41) {
                    
                    // pedcount -= $scope.albumNameArraysaveDB[i].NOP;
                    $scope.albumNameArraysaveDB4.splice(i, 1);

                }
            }

            for (var i = $scope.albumNameArray4.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArray4[i].clsDisplay == pageid1 && $scope.albumNameArray4[i].sectDisplay == pageid2 && $scope.albumNameArray4[i].dayDisplay == pageid4 && $scope.albumNameArray4[i].subDisplay == pageid3) {
                    $scope.albumNameArray4.splice(i, 1);

                }
            }
            $scope.gridOptions4_sub.data = $scope.albumNameArray4;

        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid5 = function (employee) {
            
            //$scope.editEmployee = employee.TTLABD_Id;
            //$scope.remove = function (project) {
            //    $scope.gridOptions1.data.splice($scope.gridOptions1.data.indexOf(project), 1);
            //}

            var pageid1 = employee.clsDisplay;
            var pageid2 = employee.sectDisplay;
            var pageid3 = employee.staffDisplay;
            var pageid4 = employee.dayDisplay;


            for (var a = 0; a < $scope.class_list5.length; a++) {
                if ($scope.class_list5[a].asmcL_ClassName == pageid1) {
                    
                    cls51 = $scope.class_list5[a].asmcL_Id;
                }
            }
            for (var b = 0; b < $scope.section_list5.length; b++) {
                if ($scope.section_list5[b].asmC_SectionName == pageid2) {
                    
                    sect51 = $scope.section_list5[b].asmS_Id;
                }
            }
            //for (var k = 0; k < $scope.stflst.length; k++) {
            //    if ($scope.stflst[k].ivrmstauL_Name == pageid3) {
            //        staff1 = $scope.stflst[k].ivrmstauL_Id;
            //    }
            //}
            for (var c = 0; c < $scope.staff_list5.length; c++) {
                if ($scope.staff_list5[c].staffName == pageid3) {
                    
                    staff51 = $scope.staff_list5[c].hrmE_Id;
                }
            }
            day51 = pageid4;

            for (var i = $scope.albumNameArraysaveDB5.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArraysaveDB5[i].ASMCL_Id == cls51 && $scope.albumNameArraysaveDB5[i].ASMS_Id == sect51 && $scope.albumNameArraysaveDB5[i].HRME_Id == staff51 && $scope.albumNameArraysaveDB5[i].NOD == day51) {
                    
                    // pedcount -= $scope.albumNameArraysaveDB[i].NOP;
                    $scope.albumNameArraysaveDB5.splice(i, 1);

                }
            }

            for (var i = $scope.albumNameArray5.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArray5[i].clsDisplay == pageid1 && $scope.albumNameArray5[i].sectDisplay == pageid2 && $scope.albumNameArray5[i].dayDisplay == pageid4 && $scope.albumNameArray5[i].staffDisplay == pageid3) {
                    $scope.albumNameArray5.splice(i, 1);

                }
            }
            $scope.gridOptions5_sub.data = $scope.albumNameArray5;

        };


    }

})();