
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGFixingController', CLGFixingController)

    CLGFixingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function CLGFixingController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};

        //Day Period Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ASMAY_Year', displayName: 'Academic Year' },
                { name: 'TTMC_CategoryName', displayName: 'Category' },
                { name: 'AMCO_CourseName', displayName: 'Course' },
                { name: 'AMB_BranchName', displayName: 'Branch' },
                { name: 'AMSE_SEMName', displayName: 'Sem' },
                { name: 'ACMS_SectionName', displayName: 'Section' },
                { name: 'ENAME', displayName: 'Staff' },
                { name: 'ISMS_SubjectName', displayName: 'Subject' },
               { name: 'TTMD_DayName', displayName: 'Day' },
               { name: 'TTMP_PeriodName', displayName: 'Period' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                
                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue1(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.TTFDPC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.TTFDPC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
                 
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
             { name: 'ttfdS_SUbSelcFlag', displayName: 'Semester-Section Wise' },

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
                { name: 'ttfdsU_SUbSelcFlag', displayName: 'Semester-Section Wise' },

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
                { name: 'ttfpS_SUbSelcFlag', displayName: 'Semester-Section Wise' },

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
                { name: 'ttfpsU_SUbSelcFlag', displayName: 'Semester-Section Wise' },

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


              { name: 'corDisplay', displayName: 'Course' },
            { name: 'brDisplay', displayName: 'Branch' },
            { name: 'semDisplay', displayName: 'Sem' },
            { name: 'secDisplay', displayName: 'Section' },
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


                { name: 'corDisplay', displayName: 'Course' },
                { name: 'brDisplay', displayName: 'Branch' },
                { name: 'semDisplay', displayName: 'Sem' },
                { name: 'secDisplay', displayName: 'Section' },
                { name: 'stfDisplay', displayName: 'Staff' },
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

                { name: 'corDisplay', displayName: 'Course' },
                { name: 'brDisplay', displayName: 'Branch' },
                { name: 'semDisplay', displayName: 'Sem' },
                { name: 'secDisplay', displayName: 'Section' },
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

                { name: 'corDisplay', displayName: 'Course' },
                { name: 'brDisplay', displayName: 'Branch' },
                { name: 'semDisplay', displayName: 'Sem' },
                { name: 'secDisplay', displayName: 'Section' },
                { name: 'stfDisplay', displayName: 'Staff' },
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
        $scope.savetab1 = function () {
            
            $scope.submitted1 = true;

            if ($scope.myForm1.$valid) {

                var data = {
                    "TTFDPC_Id": $scope.TTFDPC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "TTMC_Id": $scope.TTMC_Id1,
                    "AMCO_Id": $scope.AMCO_Id1,
                    "AMB_Id": $scope.AMB_Id1,
                    "AMSE_Id": $scope.AMSE_Id1,
                    "ACMS_Id": $scope.ACMS_Id1,
                    "HRME_Id": $scope.HRME_Id1,
                    "ISMS_Id": $scope.ISMS_Id1,
                    "TTMD_Id": $scope.TTMD_Id1,
                    "TTMP_Id": $scope.TTMP_Id1
                }
                apiService.create("CLGFixing/savetab1", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected Staff And Subject Is Restricted For Selected Details !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data Saved/Updated successfully');
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
        $scope.savetab2 = function () {
            $scope.submitted2 = true;
            debugger;
            if ($scope.myForm2.$valid) {

                if ($scope.cs_flag2 == 0) {
                    var data = {
                        "TTFDS_Id": $scope.TTFDS_Id,
                        "ASMAY_Id": $scope.ASMAY_Id2,
                        "TTMD_Id": $scope.TTMD_Id2,
                        "HRME_Id": $scope.HRME_Id2,
                        "TTFDS_SUbSelcFlag": $scope.cs_flag2,
                    }
                }
                else if ($scope.cs_flag2 == 1) {
                    var data = {
                        "TTFDS_Id": $scope.TTFDS_Id,
                        "TTFDSCC_Id": $scope.TTFDSCC_Id,
                        "ASMAY_Id": $scope.ASMAY_Id2,
                        "HRME_Id": $scope.HRME_Id2,
                        "TTFDS_SUbSelcFlag": $scope.cs_flag2,
                        "TTMD_Id": $scope.TTMD_Id2,
                        "TempararyArrayList": $scope.albumNameArraysaveDB2
                    }
                }
                apiService.create("CLGFixing/savetab2", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Staff Is Restricted For Selected Day !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB2 = [];
                            $scope.gridOptions2_sub.data = "";
                            $scope.albumNameArray2 = [];
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
        $scope.savetab3 = function () {
            $scope.submitted3 = true;
            
            if ($scope.myForm3.$valid) {

                if ($scope.cs_flag3 == 0) {
                    var data = {
                        "TTFDSU_Id": $scope.TTFDSU_Id,
                        "ASMAY_Id": $scope.ASMAY_Id3,
                        "TTMD_Id": $scope.TTMD_Id3,
                        "ISMS_Id": $scope.ISMS_Id3,
                        "TTFDSU_SUbSelcFlag": $scope.cs_flag3,
                    }
                }
                else if ($scope.cs_flag3 == 1) {
                    var data = {
                        "TTFDSU_Id": $scope.TTFDSU_Id,
                        "TTFDSUCC_Id": $scope.TTFDSUCC_Id,
                        "ASMAY_Id": $scope.ASMAY_Id3,
                        "ISMS_Id": $scope.ISMS_Id3,
                        "TTFDSU_SUbSelcFlag": $scope.cs_flag3,
                        "TTMD_Id": $scope.TTMD_Id3,
                        "TempararyArrayList": $scope.albumNameArraysaveDB3
                    }
                }
                apiService.create("CLGFixing/savetab3", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Subject Is Restricted For Selected Day !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB3 = [];
                            $scope.gridOptions3_sub.data = "";
                            $scope.albumNameArray3 = [];
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
        $scope.savetab4 = function () {
            $scope.submitted4 = true;
            
            if ($scope.myForm4.$valid) {

                if ($scope.cs_flag4 == 0) {
                    var data = {
                        "TTFPS_Id": $scope.TTFPS_Id,
                        "ASMAY_Id": $scope.ASMAY_Id4,
                        "TTMP_Id": $scope.TTMP_Id4,
                        "HRME_Id": $scope.HRME_Id4,
                        "TTFPS_SUbSelcFlag": $scope.cs_flag4,
                    }
                }
                else if ($scope.cs_flag4 == 1) {
                    var data = {
                        "TTFPS_Id": $scope.TTFPS_Id,
                        "TTFPSCC_Id": $scope.TTFPSCC_Id,
                        "ASMAY_Id": $scope.ASMAY_Id4,
                        "TTMP_Id": $scope.TTMP_Id4,
                        "HRME_Id": $scope.HRME_Id4,
                        "TTFPS_SUbSelcFlag": $scope.cs_flag4,
                        "TempararyArrayList": $scope.albumNameArraysaveDB4
                    }
                }
                apiService.create("CLGFixing/savetab4", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Staff Is Restricted For Selected Period !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB4 =[];
                            $scope.gridOptions4_sub.data = "";
                            $scope.albumNameArray4 = [];
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
        $scope.savetab5 = function () {
            $scope.submitted5 = true;
            
            if ($scope.myForm5.$valid) {

                if ($scope.cs_flag5 == 0) {
                    var data = {
                        "TTFPSU_Id": $scope.TTFPSU_Id,
                        "ASMAY_Id": $scope.ASMAY_Id5,
                        "TTMP_Id": $scope.TTMP_Id5,
                        "ISMS_Id": $scope.ISMS_Id5,
                        "TTFPSU_SUbSelcFlag": $scope.cs_flag5,
                    }
                }
                else if ($scope.cs_flag5 == 1) {
                    var data = {
                        "TTFPSU_Id": $scope.TTFPSU_Id,
                        "TTFPSUCC_Id": $scope.TTFPSUCC_Id,
                        "ASMAY_Id": $scope.ASMAY_Id5,
                        "TTMP_Id": $scope.TTMP_Id5,
                        "ISMS_Id": $scope.ISMS_Id5,
                        "TTFPSU_SUbSelcFlag": $scope.cs_flag5,
                        "TempararyArrayList": $scope.albumNameArraysaveDB5
                    }
                }
                apiService.create("CLGFixing/savetab5", data).
                    then(function (promise) {
                        if (promise.returnrestrictstatus === 'Restricted') {
                            swal('Selected  Subject Is Restricted For Selected Day !');
                        }
                        else if (promise.returnval === true) {
                            swal('Data successfully Saved');
                            $scope.albumNameArraysaveDB5 = [];
                            $scope.gridOptions5_sub.data = "";
                            $scope.albumNameArray5 = [];
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
            apiService.create("CLGFixing/edittab1", employee).
                then(function (promise) {

                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semisterlist = promise.semisterlist;
                    $scope.stafflist = promise.stafflist;
                    $scope.subjectlist = promise.subjectlist;
                    $scope.day_list = promise.daydropdown;

                    $scope.TTFDPC_Id = promise.fix_day_period_edit[0].ttfdpC_Id;
                $scope.ASMAY_Id1 = promise.fix_day_period_edit[0].asmaY_Id;
                $scope.TTMD_Id1 = promise.fix_day_period_edit[0].ttmD_Id;
                $scope.TTMC_Id1 = promise.fix_day_period_edit[0].ttmC_Id;
                $scope.AMCO_Id1 = promise.fix_day_period_edit[0].amcO_Id;
                $scope.AMB_Id1 = promise.fix_day_period_edit[0].amB_Id;
                $scope.AMSE_Id1 = promise.fix_day_period_edit[0].amsE_Id;
                $scope.ACMS_Id1 = promise.fix_day_period_edit[0].acmS_Id;
                $scope.TTMP_Id1 = promise.fix_day_period_edit[0].ttmP_Id;
                $scope.HRME_Id1 = promise.fix_day_period_edit[0].hrmE_Id;
                $scope.ISMS_Id1 = promise.fix_day_period_edit[0].ismS_Id;

               

            })

        }


        $scope.getorgvalue2 = function (employee) {
            debugger;
            $scope.albumNameArray2 = [];
            $scope.albumNameArraysaveDB2 = [];
            apiService.create("CLGFixing/gettab2editdata", employee).
            then(function (promise) {
                
                $scope.clear2();
                // pedcount = promise.edit_count;


                $scope.temp_check2 = promise.fix_day_staff_edit[0].ttfdS_SUbSelcFlag;

                if ($scope.temp_check2 == true) {
                    $scope.cs_flag2 = 1;

                    $scope.TTFDS_Id = promise.fix_day_staff_edit[0].TTFDS_Id;
                    $scope.ASMAY_Id2 = promise.fix_day_staff_edit[0].ASMAY_Id;
                    $scope.HRME_Id2 = promise.fix_day_staff_edit[0].HRME_Id;
                    $scope.TTMD_Id2 = promise.fix_day_staff_edit[0].TTMD_Id;
                    $scope.temp_check2 = promise.fix_day_staff_edit[0].TTFDS_SUbSelcFlag;
                    angular.forEach(promise.fix_day_staff_edit, function (ww) {

                        $scope.albumNameArray2.push({ corDisplay: ww.AMCO_CourseName, brDisplay: ww.AMB_BranchName, semDisplay: ww.AMSE_SEMName, secDisplay: ww.ACMS_SectionName, subDisplay: ww.ISMS_SubjectName, pedDisplay: ww.TTFPSCB_Periods, AMCO_Id: ww.AMCO_Id, AMB_Id: ww.AMB_Id, AMSE_Id: ww.AMSE_Id, ACMS_Id: ww.ACMS_Id, ISMS_Id: ww.ISMS_Id });

                        $scope.albumNameArraysaveDB2.push({ AMCO_Id: ww.AMCO_Id, AMB_Id: ww.AMB_Id, AMSE_Id: ww.AMSE_Id, ACMS_Id: ww.ACMS_Id, ISMS_Id: ww.ISMS_Id, NOP: parseInt(ww.TTFPSCB_Periods) });
                    })
                    if ($scope.ASMAY_Id2 != "") {

                        $scope.get_stafftab2();
                        $scope.HRME_Id2 = promise.fix_day_staff_edit[0].HRME_Id;
                    }
                    if ($scope.ASMAY_Id2 != "" && $scope.HRME_Id2 != "") {
                        $scope.get_course_onstaff();
                        $scope.AMCO_Id2 = promise.fix_day_staff_edit[0].AMCO_Id;
                    }

                }
                else if ($scope.temp_check2 == false) {
                    $scope.cs_flag2 = 0;

                    $scope.TTFDS_Id = promise.fix_day_staff_edit[0].ttfdS_Id;

                    $scope.ASMAY_Id2 = promise.fix_day_staff_edit[0].asmaY_Id;
                    $scope.HRME_Id2 = promise.fix_day_staff_edit[0].hrmE_Id;
                    $scope.TTMD_Id2 = promise.fix_day_staff_edit[0].ttmD_Id;

                    if ($scope.ASMAY_Id2 != "") {

                        $scope.get_stafftab2();
                        $scope.HRME_Id2 = promise.fix_day_staff_edit[0].hrmE_Id;
                    }

                    if ($scope.ASMAY_Id2 != "" && $scope.HRME_Id2 != "") {
                        $scope.get_course_onstaff();
                       
                    }
                    
                }

              
               
           $scope.CSwise2();
               

               
                




             
               

                $scope.gridOptions2_sub.data = $scope.albumNameArray2;

            })
        }

        $scope.getorgvalue3 = function (employee) {
            apiService.create("CLGFixing/edittab3", employee).
            then(function (promise) {
                $scope.clear3();
               
                $scope.temp_check3 = promise.fix_day_subject_edit[0].ttfdsU_SUbSelcFlag;
                if ($scope.temp_check3 == true) {
                    $scope.cs_flag3 = 1;
                    $scope.TTFDSU_Id = promise.fix_day_subject_edit[0].TTFDSU_Id;
                    $scope.ASMAY_Id3 = promise.fix_day_subject_edit[0].ASMAY_Id;
                    $scope.ISMS_Id3 = promise.fix_day_subject_edit[0].ISMS_Id;
                    $scope.TTMD_Id3 = promise.fix_day_subject_edit[0].TTMD_Id;

                    angular.forEach(promise.fix_day_subject_edit, function (ff) {
                        $scope.albumNameArray3.push({ corDisplay: ff.AMCO_CourseName, brDisplay: ff.AMB_BranchName, semDisplay: ff.AMSE_SEMName, secDisplay: ff.ACMS_SectionName, stfDisplay: ff.ENAME, pedDisplay: parseInt(ff.TTFDSUCB_Periods), AMCO_Id: ff.AMCO_Id, AMB_Id: ff.AMB_Id, AMSE_Id: ff.AMSE_Id, ACMS_Id: ff.ACMS_Id, HRME_Id: ff.HRME_Id });
                        $scope.albumNameArraysaveDB3.push({ AMCO_Id: ff.AMCO_Id, AMB_Id: ff.AMB_Id, AMSE_Id: ff.AMSE_Id, ACMS_Id: ff.ACMS_Id, HRME_Id: ff.HRME_Id, NOP: parseInt(ff.TTFDSUCB_Periods) });

                    })

                   


                    if ($scope.ASMAY_Id3 != "") {
                        $scope.get_subjecttab3();
                        $scope.ISMS_Id3 = promise.fix_day_subject_edit[0].ISMS_Id;
                    }
                    if ($scope.ASMAY_Id3 != "" && $scope.ISMS_Id3 != "") {
                        $scope.get_course_onsubject();
                    }
                }
                else if ($scope.temp_check3 == false) {
                    $scope.cs_flag3 = 0;

                    $scope.TTFDSU_Id = promise.fix_day_subject_edit[0].ttfdsU_Id;
                    $scope.ASMAY_Id3 = promise.fix_day_subject_edit[0].asmaY_Id;
                    $scope.ISMS_Id3 = promise.fix_day_subject_edit[0].ismS_Id;
                    $scope.TTMD_Id3 = promise.fix_day_subject_edit[0].ttmD_Id;

                    if ($scope.ASMAY_Id3 != "") {
                        $scope.get_subjecttab3();
                        $scope.ISMS_Id3 = promise.fix_day_subject_edit[0].ismS_Id;
                    }
                    if ($scope.ASMAY_Id3 != "" && $scope.ISMS_Id3 != "") {
                        $scope.get_course_onsubject();
                    }
                }

                $scope.CSwise3();
               


                $scope.gridOptions3_sub.data = $scope.albumNameArray3;

            })
        }

        $scope.getorgvalue4 = function (employee) {
            apiService.create("CLGFixing/edittab4", employee).
            then(function (promise) {
                $scope.clear4();
             
                $scope.temp_check4 = promise.fix_period_staff_edit[0].ttfpS_SUbSelcFlag;
                if ($scope.temp_check4 == true) {
                    $scope.cs_flag4 = 1;
                    $scope.TTFPS_Id = promise.fix_period_staff_edit[0].TTFPS_Id;
                    $scope.ASMAY_Id4 = promise.fix_period_staff_edit[0].ASMAY_Id;
                    $scope.HRME_Id4 = promise.fix_period_staff_edit[0].HRME_Id;
                    $scope.TTMP_Id4 = promise.fix_period_staff_edit[0].TTMP_Id;

                    angular.forEach(promise.fix_period_staff_edit, function (rr) {

                        $scope.albumNameArray4.push({ corDisplay: rr.AMCO_CourseName, brDisplay: rr.AMB_BranchName, semDisplay: rr.AMSE_SEMName, secDisplay: rr.ACMS_SectionName, subDisplay: rr.ISMS_SubjectName, dayDisplay: parseInt(rr.TTFPSCB_Days), AMCO_Id: rr.AMCO_Id, AMB_Id: rr.AMB_Id, AMSE_Id: rr.AMSE_Id, ACMS_Id: rr.ACMS_Id, ISMS_Id: rr.ISMS_Id });
                        $scope.albumNameArraysaveDB4.push({ AMCO_Id: rr.AMCO_Id, AMB_Id: rr.AMB_Id, AMSE_Id: rr.AMSE_Id, ACMS_Id: rr.ACMS_Id, ISMS_Id: rr.ISMS_Id , NOD: parseInt(rr.TTFPSCB_Days) });

                    })
                    if ($scope.ASMAY_Id4 != "") {
                        $scope.get_stafftab4();
                        $scope.HRME_Id4 = promise.fix_period_staff_edit[0].HRME_Id;
                    }
                    if ($scope.ASMAY_Id4 != "" && $scope.HRME_Id4 != "") {
                        $scope.get_course_onstafftab4();
                    }
                   
                }
                else if ($scope.temp_check4 == false) {
                    $scope.cs_flag4 = 0;
                    $scope.TTFPS_Id = promise.fix_period_staff_edit[0].ttfpS_Id;
                    $scope.ASMAY_Id4 = promise.fix_period_staff_edit[0].asmaY_Id;
                    $scope.HRME_Id4 = promise.fix_period_staff_edit[0].hrmE_Id;
                    $scope.TTMP_Id4 = promise.fix_period_staff_edit[0].ttmP_Id;

                    if ($scope.ASMAY_Id4 != "") {
                        $scope.get_stafftab4();
                        $scope.HRME_Id4 = promise.fix_period_staff_edit[0].hrmE_Id;
                    }
                    if ($scope.ASMAY_Id4 != "" && $scope.HRME_Id4 != "") {
                        $scope.get_course_onstafftab4();
                    }
                }
                $scope.CSwise4();
                
                
                $scope.gridOptions4_sub.data = $scope.albumNameArray4;
            })
        }

        $scope.getorgvalue5 = function (employee) {
            apiService.create("CLGFixing/edittab5", employee).
            then(function (promise) {
                $scope.clear5();
                
                $scope.temp_check5 = promise.fix_period_subject_edit[0].ttfpsU_SUbSelcFlag;
                if ($scope.temp_check5 == true) {
                    $scope.cs_flag5 = 1;
                    $scope.TTFPSU_Id = promise.fix_period_subject_edit[0].TTFPSU_Id;
                    $scope.ASMAY_Id5 = promise.fix_period_subject_edit[0].ASMAY_Id;
                    $scope.ISMS_Id5 = promise.fix_period_subject_edit[0].ISMS_Id;
                    $scope.TTMP_Id5 = promise.fix_period_subject_edit[0].TTMP_Id;


                    angular.forEach(promise.fix_period_subject_edit, function (gg) {
                        $scope.albumNameArray5.push({ corDisplay: gg.AMCO_CourseName, brDisplay: gg.AMB_BranchName, semDisplay: gg.AMSE_SEMName, secDisplay: gg.ACMS_SectionName, stfDisplay: gg.ENAME, dayDisplay: parseInt(gg.TTFPSUCB_Days), AMCO_Id: gg.AMCO_Id, AMB_Id: gg.AMB_Id, AMSE_Id: gg.AMSE_Id, ACMS_Id: gg.ACMS_Id, HRME_Id: gg.HRME_Id });
                        $scope.albumNameArraysaveDB5.push({ AMCO_Id: gg.AMCO_Id, AMB_Id: gg.AMB_Id, AMSE_Id: gg.AMSE_Id, ACMS_Id: gg.ACMS_Id, HRME_Id: gg.HRME_Id, NOD: parseInt(gg.TTFPSUCB_Days) });

                        if ($scope.ASMAY_Id5 != "") {
                            $scope.get_subjecttab5();
                            $scope.ISMS_Id5 = promise.fix_period_subject_edit[0].ISMS_Id;
                        }

                        if ($scope.ASMAY_Id5 != "" && $scope.ISMS_Id5 !="") {
                            $scope.get_course_onsubject5();
                            
                        }
                    })

                   

                }
                else if ($scope.temp_check5 == false) {
                    $scope.cs_flag5 = 0;

                    $scope.TTFPSU_Id = promise.fix_period_subject_edit[0].ttfpsU_Id;
                    $scope.ASMAY_Id5 = promise.fix_period_subject_edit[0].asmaY_Id;
                    $scope.ISMS_Id5 = promise.fix_period_subject_edit[0].ismS_Id;
                    $scope.TTMP_Id5 = promise.fix_period_subject_edit[0].ttmP_Id;

                    if ($scope.ASMAY_Id5 != "") {
                        $scope.get_subjecttab5();
                        $scope.ISMS_Id5 = promise.fix_period_subject_edit[0].ismS_Id;
                    }

                    if ($scope.ASMAY_Id5 != "" && $scope.ISMS_Id5 != "") {
                        $scope.get_course_onsubject5();

                    }
                }
                $scope.CSwise5();
                
               
                $scope.gridOptions5_sub.data = $scope.albumNameArray5;

            })
        }

        //TO  View Record
        $scope.viewrecordspopup2 = function (employee, SweetAlert) {

            $scope.staff_Name = employee.staffName;
            $scope.day_Name = employee.ttmD_DayName;

            apiService.create("CLGFixing/viewtab2grid", employee).
                    then(function (promise) {
                        $scope.viewrecordspopupdisplay2 = promise.detailspopuparray2;
                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid2 = function () {
            $scope.viewrecordspopupdisplay2 = [];
        };

        //TO  View Record
        $scope.viewrecordspopup3 = function (employee, SweetAlert) {
            $scope.subject_Name = employee.ismS_SubjectName;
            $scope.day_Name = employee.ttmD_DayName;
            apiService.create("CLGFixing/viewtab3grid", employee).
                    then(function (promise) {
                        
                        $scope.viewrecordspopupdisplay3 = promise.detailspopuparray3;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid3 = function () {
            $scope.viewrecordspopupdisplay3 = [];
        };

        //TO  View Record
        $scope.viewrecordspopup4 = function (employee, SweetAlert) {
           
            $scope.staff_Name = employee.staffName;
            $scope.period_Name = employee.ttmP_PeriodName;
            apiService.create("CLGFixing/viewtab4", employee).
                    then(function (promise) {
                        $scope.viewrecordspopupdisplay4 = promise.detailspopuparray4;
                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid4 = function () {
            $scope.viewrecordspopupdisplay4 = [];
        };

        //TO  View Record
        $scope.viewrecordspopup5 = function (employee, SweetAlert) {
            $scope.subject_Name = employee.ismS_SubjectName;
            $scope.period_Name = employee.ttmP_PeriodName;
            apiService.create("CLGFixing/viewtab5", employee).
                    then(function (promise) {
                        $scope.viewrecordspopupdisplay5 = promise.detailspopuparray5;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid5 = function () {
            $scope.viewrecordspopupdisplay5 = [];
        };


        //to get class by category
        $scope.asmaY_Id1 = "";
        $scope.ttmC_Id1 = "";
      

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
            if (employee.TTFDPC_ActiveFlag === true) {
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

                apiService.create("CLGFixing/deactivatetab1", employee).
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

                apiService.create("CLGFixing/deactivatetab2", employee).
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

                apiService.create("CLGFixing/deactivatetab3", employee).
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

                apiService.create("CLGFixing/deactivatetab4", employee).
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

                apiService.create("CLGFixing/deactivatetab5", employee).
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
            var pageid = 1;
            apiService.getURI("CLGFixing/getalldetails", pageid).
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.peds_count = promise.period_count;
           $scope.days_count = promise.day_count;
           //TAB 1 DEFAULT DROPDOWN
           $scope.year_list = promise.academiclist;
           $scope.day_list = promise.daydropdown;
           $scope.category_list1 = promise.categorylist;
           $scope.period_list1 = promise.periodlist;
           $scope.sectionlist = promise.sectionlist;
           $scope.gridOptions1.data = promise.fix_day_period_list;


           //TAB2 LOAD DATA

           $scope.staff_list2 = promise.staff_list2;

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
          
           // $scope.category_list2 = promise.categorylist;
           // $scope.category_list3 = promise.categorylist;
           
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
          
           $scope.gridOptions2.data = promise.all_fix_day_staff_list;
           $scope.gridOptions3.data = promise.all_fix_day_subject_list;
           $scope.gridOptions4.data = promise.all_fix_period_staff_list;
           $scope.gridOptions5.data = promise.all_fix_period_subject_list;
           //$scope.gridOptions.data = promise.categorylist;
       })
        };

        //LOAD TAB1 DATA BASED ON DROPDOWN SELECTION
        $scope.get_course = function () {
            $scope.AMB_Id1 = '';
            $scope.AMCO_Id1 = '';
            $scope.AMSE_Id1 = '';
            $scope.ACMS_Id1 = '';
            $scope.HRME_Id1 = '';
            $scope.ISMS_Id1 = '';
            $scope.semisterlist = [];
            if ($scope.ASMAY_Id1 === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id1 != "" && $scope.TTMC_Id1 != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id1,
                    "ASMAY_Id": $scope.ASMAY_Id1,
                }
                apiService.create("CLGTTCommon/getcourse_catg", data).
                    then(function (promise) {

                        $scope.courselist = promise.courselist;

                        if (promise.courselist == "" || promise.courselist == null) {
                            swal("No Course/Branch Are Mapped To Selected Category");
                        }
                    })
            }
        };
        $scope.get_staff = function () {
            $scope.ISMS_Id1 = '';
            $scope.HRME_Id1 = '';
            if ($scope.ASMAY_Id1 === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id1 != "" && $scope.TTMC_Id1 != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id1,
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "AMCO_Id": $scope.AMCO_Id1,
                    "AMB_Id": $scope.AMB_Id1,
                    "AMSE_Id": $scope.AMSE_Id1,
                    "ACMS_Id": $scope.ACMS_Id1,

                }
                apiService.create("CLGTTCommon/get_staff", data).
                    then(function (promise) {

                        $scope.stafflist = promise.stafflist;

                        if (promise.stafflist == "" || promise.stafflist == null) {
                            swal("Staff are not mapped with selected parameter");
                        }
                    })
            }
        };
        $scope.get_subject = function () {
            $scope.ISMS_Id1 = '';
            if ($scope.ASMAY_Id1 === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id1 != "" && $scope.TTMC_Id1 != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id1,
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "AMCO_Id": $scope.AMCO_Id1,
                    "AMB_Id": $scope.AMB_Id1,
                    "AMSE_Id": $scope.AMSE_Id1,
                    "ACMS_Id": $scope.ACMS_Id1,
                    "HRME_Id": $scope.HRME_Id1,

                }
                apiService.create("CLGTTCommon/get_subject", data).
                    then(function (promise) {

                        $scope.subjectlist = promise.subjectlist;

                        if (promise.subjectlist == "" || promise.subjectlist == null) {
                            swal("Subjects are not mapped for selected parameter");
                        }
                    })
            }
        };
        $scope.get_semday = function () {
            $scope.TTMD_Id1 = '';
            if ($scope.ASMAY_Id1 === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id1 != "" && $scope.TTMC_Id1 != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id1,
                    "ASMAY_Id": $scope.ASMAY_Id1,
                    "AMCO_Id": $scope.AMCO_Id1,
                    "AMB_Id": $scope.AMB_Id1,
                    "AMSE_Id": $scope.AMSE_Id1,
                }
                apiService.create("CLGTTCommon/get_semday", data).
                    then(function (promise) {

                        $scope.day_list = promise.daydropdown;

                        if (promise.daydropdown == "" || promise.daydropdown == null) {
                            swal("Days are not mapped for selected parameter");
                        }
                    })
            }
        };
        $scope.getbranch_catg = function () {
            $scope.AMB_Id1 = '';
            $scope.AMSE_Id1 = '';
            $scope.ACMS_Id1 = '';
            $scope.HRME_Id1 = '';
            $scope.ISMS_Id1 = '';
            $scope.semisterlist = [];
            var data = {
                "TTMC_Id": $scope.TTMC_Id1,
                "AMCO_Id": $scope.AMCO_Id1,
                "ASMAY_Id": $scope.ASMAY_Id1,
            }
            apiService.create("CLGTTCommon/getbranch_catg", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("No Branch Are Mapped To Selected Category/Course");
                    }
                })

        };
        $scope.get_stafftab2 = function () {
            $scope.HRME_Id2 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id2,
            }
            apiService.create("CLGTTCommon/get_staffaca", data).
                then(function (promise) {

                    $scope.staff_list2 = promise.stafflist;

                    if (promise.stafflist == "" || promise.stafflist == null) {
                        swal("Periods are Not disstributed any staff ");
                    }
                })
        };
        $scope.get_stafftab4 = function () {
            $scope.HRME_Id4 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id4,
            }
            apiService.create("CLGTTCommon/get_staffaca", data).
                then(function (promise) {

                    $scope.staff_list4 = promise.stafflist;

                    if (promise.stafflist == "" || promise.stafflist == null) {
                        swal("Periods are Not disstributed any staff ");
                    }
                })
        };

        //TAB 3

        $scope.get_subjecttab3 = function () {
            $scope.ISMS_Id3 = '';
            $scope.HRME_Id3 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id3,
            }
            apiService.create("CLGTTCommon/get_subjecttab3", data).
                then(function (promise) {

                    $scope.subject_list3 = promise.subjectlist;

                    if (promise.subjectlist == "" || promise.subjectlist == null) {
                        swal("Periods are Not distributed any staff/Subject ");
                    }
                })
        };
        $scope.get_subjecttab5 = function () {
            $scope.ISMS_Id5 = '';
            $scope.HRME_Id5 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id5,
            }
            apiService.create("CLGTTCommon/get_subjecttab3", data).
                then(function (promise) {

                    $scope.subject_list3 = promise.subjectlist;

                    if (promise.subjectlist == "" || promise.subjectlist == null) {
                        swal("Periods are Not distributed any staff/Subject ");
                    }
                })
        };
        $scope.get_course_onsubject = function () {
            $scope.AMCO_Id3 = '';
            $scope.ACMS_Id3= '';
            $scope.AMB_Id3 = '';
            $scope.AMSE_Id3 = '';
            $scope.HRME_Id3 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id3,
                "ISMS_Id": $scope.ISMS_Id3,
            }
            apiService.create("CLGTTCommon/get_course_onsubject", data).
                then(function (promise) {

                    $scope.courselist = promise.courselist;

                    if (promise.courselist == "" || promise.courselist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_course_onsubject5 = function () {
            $scope.AMCO_Id5 = '';
            $scope.ACMS_Id5= '';
            $scope.AMB_Id5 = '';
            $scope.AMSE_Id5 = '';
            $scope.HRME_Id5 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id5,
                "ISMS_Id": $scope.ISMS_Id5,
            }
            apiService.create("CLGTTCommon/get_course_onsubject", data).
                then(function (promise) {

                    $scope.courselist = promise.courselist;

                    if (promise.courselist == "" || promise.courselist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_branch_onsubject = function () {
            $scope.ACMS_Id3 = '';
            $scope.AMB_Id3 = '';
            $scope.AMSE_Id3 = '';
            $scope.HRME_Id3 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id3,
                "ISMS_Id": $scope.ISMS_Id3,
                "AMCO_Id": $scope.AMCO_Id3,
            }
            apiService.create("CLGTTCommon/get_branch_onsubject", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_branch_onsubject5 = function () {
            $scope.ACMS_Id5 = '';
            $scope.AMB_Id5 = '';
            $scope.AMSE_Id5 = '';
            $scope.HRME_Id5 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id5,
                "ISMS_Id": $scope.ISMS_Id5,
                "AMCO_Id": $scope.AMCO_Id5,
            }
            apiService.create("CLGTTCommon/get_branch_onsubject", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        

        $scope.get_sem_onsubject = function () {
            $scope.ACMS_Id3 = '';
            $scope.AMSE_Id3 = '';
            $scope.HRME_Id3 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id3,
                "ISMS_Id": $scope.ISMS_Id3,
                "AMCO_Id": $scope.AMCO_Id3,
                "AMB_Id": $scope.AMB_Id3,
            }
            apiService.create("CLGTTCommon/get_sem_onsubject", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;
                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_sem_onsubject5 = function () {
            $scope.ACMS_Id5 = '';
            $scope.AMSE_Id5 = '';
            $scope.HRME_Id5 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id5,
                "ISMS_Id": $scope.ISMS_Id5,
                "AMCO_Id": $scope.AMCO_Id5,
                "AMB_Id": $scope.AMB_Id5,
            }
            apiService.create("CLGTTCommon/get_sem_onsubject", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;
                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };

        
        $scope.get_sec_onsubject = function () {
            $scope.ACMS_Id3 = '';
            $scope.HRME_Id3 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id3,
                "ISMS_Id": $scope.ISMS_Id3,
                "AMCO_Id": $scope.AMCO_Id3,
                "AMB_Id": $scope.AMB_Id3,
                "AMSE_Id": $scope.AMSE_Id3,
            }
            apiService.create("CLGTTCommon/get_sec_onsubject", data).
                then(function (promise) {

                    $scope.sectionlist = promise.sectionlist;
                    if (promise.sectionlist == "" || promise.sectionlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_sec_onsubject5 = function () {
            $scope.ACMS_Id5 = '';
            $scope.HRME_Id5 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id5,
                "ISMS_Id": $scope.ISMS_Id5,
                "AMCO_Id": $scope.AMCO_Id5,
                "AMB_Id": $scope.AMB_Id5,
                "AMSE_Id": $scope.AMSE_Id5,
            }
            apiService.create("CLGTTCommon/get_sec_onsubject", data).
                then(function (promise) {

                    $scope.sectionlist = promise.sectionlist;
                    if (promise.sectionlist == "" || promise.sectionlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };

        $scope.get_staff_onsubject = function () {
            $scope.HRME_Id3 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id3,
                "ISMS_Id": $scope.ISMS_Id3,
                "AMCO_Id": $scope.AMCO_Id3,
                "AMB_Id": $scope.AMB_Id3,
                "AMSE_Id": $scope.AMSE_Id3,
                "ACMS_Id": $scope.ACMS_Id3,
            }
            apiService.create("CLGTTCommon/get_staff_onsubject", data).
                then(function (promise) {

                    $scope.stafflist = promise.stafflist;
                    if (promise.stafflist == "" || promise.stafflist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };

        $scope.get_staff_onsubject5 = function () {
            $scope.HRME_Id5 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id5,
                "ISMS_Id": $scope.ISMS_Id5,
                "AMCO_Id": $scope.AMCO_Id5,
                "AMB_Id": $scope.AMB_Id5,
                "AMSE_Id": $scope.AMSE_Id5,
                "ACMS_Id": $scope.ACMS_Id5,
            }
            apiService.create("CLGTTCommon/get_staff_onsubject", data).
                then(function (promise) {

                    $scope.stafflist = promise.stafflist;
                    if (promise.stafflist == "" || promise.stafflist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };


        $scope.get_semister = function () {
            $scope.AMSE_Id1 = '';
            $scope.ACMS_Id1 = '';
            $scope.HRME_Id1 = '';
            $scope.ISMS_Id1 = '';
            var data = {
                "TTMC_Id": $scope.TTMC_Id1,
                "AMCO_Id": $scope.AMCO_Id1,
                "ASMAY_Id": $scope.ASMAY_Id1,
                "AMB_Id": $scope.AMB_Id1,
            }
            apiService.create("CLGTTCommon/get_semister", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;

                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semester Are Mapped To Selected Course/Branch");
                    }
                })
        };
        $scope.get_course_onstaff = function () {
            $scope.AMCO_Id2 = '';
            $scope.ACMS_Id2 = '';
            $scope.ISMS_Id2 = '';
            $scope.AMB_Id2 = '';
            $scope.AMSE_Id2 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id2,
                "HRME_Id": $scope.HRME_Id2,
            }
            apiService.create("CLGTTCommon/get_course_onstaff", data).
                then(function (promise) {

                    $scope.courselist = promise.courselist;

                    if (promise.courselist == "" || promise.courselist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };

        $scope.get_course_onstafftab4 = function () {
            $scope.AMCO_Id4 = '';
            $scope.ACMS_Id4 = '';
            $scope.ISMS_Id4 = '';
            $scope.AMB_Id4 = '';
            $scope.AMSE_Id4 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id4,
                "HRME_Id": $scope.HRME_Id4,
            }
            apiService.create("CLGTTCommon/get_course_onstaff", data).
                then(function (promise) {

                    $scope.courselist = promise.courselist;

                    if (promise.courselist == "" || promise.courselist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };

        $scope.get_branch_onstaff = function () {
            $scope.ACMS_Id2 = '';
            $scope.ISMS_Id2 = '';
            $scope.AMB_Id2 = '';
            $scope.AMSE_Id2 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id2,
                "HRME_Id": $scope.HRME_Id2,
                "AMCO_Id": $scope.AMCO_Id2,
            }
            apiService.create("CLGTTCommon/get_branch_onstaff", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_branch_onstafftab4 = function () {
            $scope.ACMS_Id4 = '';
            $scope.ISMS_Id4 = '';
            $scope.AMB_Id4 = '';
            $scope.AMSE_Id4 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id4,
                "HRME_Id": $scope.HRME_Id4,
                "AMCO_Id": $scope.AMCO_Id4,
            }
            apiService.create("CLGTTCommon/get_branch_onstaff", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_sem_onstaff = function () {
            $scope.ACMS_Id2 = '';
            $scope.ISMS_Id2 = '';
            $scope.AMSE_Id2 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id2,
                "HRME_Id": $scope.HRME_Id2,
                "AMCO_Id": $scope.AMCO_Id2,
                "AMB_Id": $scope.AMB_Id2,
            }
            apiService.create("CLGTTCommon/get_sem_onstaff", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;
                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_sem_onstafftab4 = function () {
            $scope.ACMS_Id4 = '';
            $scope.ISMS_Id4 = '';
            $scope.AMSE_Id4 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id4,
                "HRME_Id": $scope.HRME_Id4,
                "AMCO_Id": $scope.AMCO_Id4,
                "AMB_Id": $scope.AMB_Id4,
            }
            apiService.create("CLGTTCommon/get_sem_onstaff", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;
                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_sec_onstaff = function () {
            $scope.ACMS_Id2 = '';
            $scope.ISMS_Id2 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id2,
                "HRME_Id": $scope.HRME_Id2,
                "AMCO_Id": $scope.AMCO_Id2,
                "AMB_Id": $scope.AMB_Id2,
                "AMSE_Id": $scope.AMSE_Id2,
            }
            apiService.create("CLGTTCommon/get_sec_onstaff", data).
                then(function (promise) {

                    $scope.sectionlist = promise.sectionlist;
                    if (promise.sectionlist == "" || promise.sectionlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_sec_onstafftab4 = function () {
            $scope.ACMS_Id4 = '';
            $scope.ISMS_Id4 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id4,
                "HRME_Id": $scope.HRME_Id4,
                "AMCO_Id": $scope.AMCO_Id4,
                "AMB_Id": $scope.AMB_Id4,
                "AMSE_Id": $scope.AMSE_Id4,
            }
            apiService.create("CLGTTCommon/get_sec_onstaff", data).
                then(function (promise) {

                    $scope.sectionlist = promise.sectionlist;
                    if (promise.sectionlist == "" || promise.sectionlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_subject_onstaff = function () {
            $scope.ISMS_Id2 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id2,
                "HRME_Id": $scope.HRME_Id2,
                "AMCO_Id": $scope.AMCO_Id2,
                "AMB_Id": $scope.AMB_Id2,
                "AMSE_Id": $scope.AMSE_Id2,
                "ACMS_Id": $scope.ACMS_Id2,
            }
            apiService.create("CLGTTCommon/get_subject_onstaff", data).
                then(function (promise) {

                    $scope.subjectlist = promise.subjectlist;
                    if (promise.subjectlist == "" || promise.subjectlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };
        $scope.get_subject_onstafftab4 = function () {
            $scope.ISMS_Id4 = '';
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id4,
                "HRME_Id": $scope.HRME_Id4,
                "AMCO_Id": $scope.AMCO_Id4,
                "AMB_Id": $scope.AMB_Id4,
                "AMSE_Id": $scope.AMSE_Id4,
                "ACMS_Id": $scope.ACMS_Id4,
            }
            apiService.create("CLGTTCommon/get_subject_onstaff", data).
                then(function (promise) {

                    $scope.subjectlist = promise.subjectlist;
                    if (promise.subjectlist == "" || promise.subjectlist == null) {
                        swal("Period Distribution is not done by any  Course/Branch");
                    }
                })
        };

        ///


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
            $scope.TTFDPC_Id = 0;
            $scope.ASMAY_Id1 = '';
            $scope.TTMC_Id1 = '';
            $scope.AMCO_Id1 = '';
            $scope.AMB_Id1 = '';
            $scope.AMSE_Id1 = '';
            $scope.ACMS_Id1 = '';
            $scope.HRME_Id1 = '';
            $scope.ISMS_Id1 = '';
            $scope.TTMD_Id1 = '';
            $scope.TTMP_Id1 = '';
         
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
            $scope.ASMAY_Id2 = "";
            $scope.TTMD_Id2 = "";
            $scope.ISMS_Id2 = "";
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
            $scope.AMCO_Id2 = "";
            $scope.AMB_Id2 = "";
            $scope.AMSE_Id2 = "";
            $scope.ACMS_Id2 = "";
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
            $scope.ASMAY_Id3 = "";
            $scope.TTMD_Id3 = "";
            $scope.ISMS_Id3 = "";
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
            $scope.AMCO_Id3 = "";
            $scope.AMB_Id3 = "";
            $scope.AMSE_Id3 = "";
            $scope.HRME_Id3 = "";
            $scope.ACMS_Id3 = "";
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
            $scope.ASMAY_Id4 = "";
            $scope.TTMP_Id4 = "";
            $scope.ISMS_Id4 = "";
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
         
            $scope.HRME_Id4 = "";
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
            $scope.AMCO_Id5 = "";
            $scope.AMB_Id5 = "";
            $scope.AMSE_Id5 = "";
            $scope.ACMS_Id5 = "";
            $scope.HRME_Id5 = "";
            $scope.ASMAY_Id5 = "";
            $scope.TTMP_Id5 = "";
            $scope.ISMS_Id5 = "";
            $scope.NOD_5 = 0;
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
        $scope.TransferDatagrid2 = function (objcls, www, objsect, objsub, objped, rrr) {
            debugger;
            if ($scope.AMCO_Id2 ==undefined || $scope.AMB_Id2 == undefined || $scope.AMSE_Id2 == undefined || $scope.ACMS_Id2 == undefined || $scope.ISMS_Id2 === undefined || $scope.NOP_2 == undefined || $scope.AMCO_Id2 == "" || $scope.AMB_Id2 == "" || $scope.AMSE_Id2 == "" || $scope.ACMS_Id2 == "" || $scope.ISMS_Id2 == "" || $scope.NOP_2 == "" || $scope.NOP_2 == 0) {
                swal('Please Select Feilds Course,Branch,Sem,Section,Subject  Period (Not Zero) !');
            }
            else if ($scope.peds_count < parseInt($scope.NOP_2)) {
                swal("No Of Periods Can't Exceed The Total of Daily Periods");
            }
            else {

                var cors = "";
                var bran = "";
                var sem = "";
                var sect = "";
                var subj = "";
              

                angular.forEach($scope.courselist, function (cc) {
                    if (cc.amcO_Id == $scope.AMCO_Id2) {
                        cors = cc.amcO_CourseName;
                    }
                })

                angular.forEach($scope.branchlist, function (bb) {
                    if (bb.amB_Id == $scope.AMB_Id2) {
                        bran = bb.amB_BranchName;
                    }
                })

                angular.forEach($scope.semisterlist, function (ss) {
                    if (ss.amsE_Id == $scope.AMSE_Id2) {
                        sem = ss.amsE_SEMName;
                    }
                })
                angular.forEach($scope.sectionlist, function (scc) {
                    if (scc.acmS_Id == $scope.ACMS_Id2) {
                        sect = scc.acmS_SectionName;
                    }
                })
                angular.forEach($scope.subjectlist, function (sbb) {
                    if (sbb.ismS_Id == $scope.ISMS_Id2) {
                        subj = sbb.ismS_SubjectName;
                    }
                })

                

                //  period =  $scope.NOP_2;
                period2 = parseInt($scope.NOP_2);

                if ($scope.albumNameArray2.length === 0) {
                    
                    $scope.albumNameArray2.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, subDisplay: subj, pedDisplay: period2, AMCO_Id: $scope.AMCO_Id2, AMB_Id: $scope.AMB_Id2, AMSE_Id: $scope.AMSE_Id2, ACMS_Id: $scope.ACMS_Id2, ISMS_Id: $scope.ISMS_Id2    });
                    $scope.albumNameArraysaveDB2.push({ AMCO_Id: $scope.AMCO_Id2, AMB_Id: $scope.AMB_Id2, AMSE_Id: $scope.AMSE_Id2, ACMS_Id: $scope.ACMS_Id2, ISMS_Id: $scope.ISMS_Id2, NOP: parseInt($scope.NOP_2) });

                    // Clear input fields after push

                    $scope.AMCO_Id2 = "";
                    $scope.AMB_Id2 = "";
                    $scope.ISMS_Id2 = "";
                    $scope.AMSE_Id2 = "";
                    $scope.ACMS_Id2 = "";
                    $scope.NOP_2 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray2.length; k++) {

                        if ($scope.albumNameArray2[k].AMCO_Id == $scope.AMCO_Id2 && $scope.albumNameArray2[k].AMB_Id == $scope.AMB_Id2 && $scope.albumNameArray2[k].AMSE_Id == $scope.AMSE_Id2 && $scope.albumNameArray2[k].ACMS_Id == $scope.ACMS_Id2 && $scope.albumNameArray2[k].ISMS_Id == $scope.ISMS_Id2) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray2.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, subDisplay: subj, pedDisplay: period2, AMCO_Id: $scope.AMCO_Id2, AMB_Id: $scope.AMB_Id2, AMSE_Id: $scope.AMSE_Id2, ACMS_Id: $scope.ACMS_Id2, ISMS_Id: $scope.ISMS_Id2 });
                        $scope.albumNameArraysaveDB2.push({ AMCO_Id: $scope.AMCO_Id2, AMB_Id: $scope.AMB_Id2, AMSE_Id: $scope.AMSE_Id2, ACMS_Id: $scope.ACMS_Id2, ISMS_Id: $scope.ISMS_Id2, NOP: parseInt($scope.NOP_2) });

                        // Clear input fields after push

                        $scope.AMCO_Id2 = "";
                        $scope.AMB_Id2 = "";
                        $scope.ISMS_Id2 = "";
                        $scope.AMSE_Id2 = "";
                        $scope.ACMS_Id2 = "";
                        $scope.NOP_2 = 0;
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
        $scope.TransferDatagrid3 = function () {
            


            if ($scope.AMCO_Id3 == undefined || $scope.AMB_Id3 == undefined || $scope.AMSE_Id3 == undefined || $scope.ACMS_Id3 == undefined || $scope.HRME_Id3 === undefined || $scope.NOP_3 == undefined || $scope.AMCO_Id3 == "" || $scope.AMB_Id3 == "" || $scope.AMSE_Id3 == "" || $scope.ACMS_Id3 == "" || $scope.HRME_Id3 == "" || $scope.NOP_3 == "" || $scope.NOP_3 == 0) {
                swal('Please Select Feilds Course,Branch,Sem,Section,Staff  Period (Not Zero) !');
            }
            else if ($scope.peds_count < parseInt($scope.NOP_3)) {
                swal("No Of Periods Can't Exceed The Total of Daily Periods");
            }
            else {

                var cors = "";
                var bran = "";
                var sem = "";
                var sect = "";
                var stff = "";


                angular.forEach($scope.courselist, function (cc) {
                    if (cc.amcO_Id == $scope.AMCO_Id3) {
                        cors = cc.amcO_CourseName;
                    }
                })

                angular.forEach($scope.branchlist, function (bb) {
                    if (bb.amB_Id == $scope.AMB_Id3) {
                        bran = bb.amB_BranchName;
                    }
                })

                angular.forEach($scope.semisterlist, function (ss) {
                    if (ss.amsE_Id == $scope.AMSE_Id3) {
                        sem = ss.amsE_SEMName;
                    }
                })
                angular.forEach($scope.sectionlist, function (scc) {
                    if (scc.acmS_Id == $scope.ACMS_Id3) {
                        sect = scc.acmS_SectionName;
                    }
                })
                angular.forEach($scope.stafflist, function (sbb) {
                    if (sbb.hrmE_Id == $scope.HRME_Id3) {
                        stff = sbb.staffName;
                    }
                })
                //  period =  $scope.NOP_2;
                period3 = parseInt($scope.NOP_3);

                if ($scope.albumNameArray3.length === 0) {
                    
                    $scope.albumNameArray3.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, stfDisplay: stff, pedDisplay: parseInt($scope.NOP_3), AMCO_Id: $scope.AMCO_Id3, AMB_Id: $scope.AMB_Id3, AMSE_Id: $scope.AMSE_Id3, ACMS_Id: $scope.ACMS_Id3, HRME_Id: $scope.HRME_Id3 });
                    $scope.albumNameArraysaveDB3.push({ AMCO_Id: $scope.AMCO_Id3, AMB_Id: $scope.AMB_Id3, AMSE_Id: $scope.AMSE_Id3, ACMS_Id: $scope.ACMS_Id3, HRME_Id: $scope.HRME_Id3, NOP: parseInt($scope.NOP_3) });
                    // Clear input fields after push

                    $scope.AMCO_Id3 = "";
                    $scope.AMB_Id3 = "";
                    $scope.AMSE_Id3 = "";
                    $scope.ACMS_Id3 = "";
                    $scope.HRME_Id3 = "";
                    $scope.NOP_3 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray3.length; k++) {
                        
                        if ($scope.albumNameArray3[k].AMCO_Id == $scope.AMCO_Id3 && $scope.albumNameArray3[k].AMB_Id == $scope.AMB_Id3 && $scope.albumNameArray3[k].AMSE_Id == $scope.AMSE_Id3 && $scope.albumNameArray3[k].ACMS_Id == $scope.ACMS_Id3 && $scope.albumNameArray3[k].HRME_Id == $scope.HRME_Id3) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {

                        $scope.albumNameArray3.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, stfDisplay: stff, pedDisplay: parseInt($scope.NOP_3), AMCO_Id: $scope.AMCO_Id3, AMB_Id: $scope.AMB_Id3, AMSE_Id: $scope.AMSE_Id3, ACMS_Id: $scope.ACMS_Id3, HRME_Id: $scope.HRME_Id3 });
                        $scope.albumNameArraysaveDB3.push({ AMCO_Id: $scope.AMCO_Id3, AMB_Id: $scope.AMB_Id3, AMSE_Id: $scope.AMSE_Id3, ACMS_Id: $scope.ACMS_Id3, HRME_Id: $scope.HRME_Id3, NOP: parseInt($scope.NOP_3) });

                        // Clear input fields after push

                        $scope.AMCO_Id3 = "";
                        $scope.AMB_Id3 = "";
                        $scope.AMSE_Id3 = "";
                        $scope.ACMS_Id3 = "";
                        $scope.HRME_Id3 = "";
                        $scope.NOP_3 = 0;
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

    
        $scope.TransferDatagrid4 = function (objcls, www, objsect, objsub, objped, rrr) {

            if ($scope.AMCO_Id4 == undefined || $scope.AMB_Id4 == undefined || $scope.AMSE_Id4 == undefined || $scope.ACMS_Id4 == undefined || $scope.ISMS_Id4 === undefined || $scope.NOD_4 == undefined || $scope.AMCO_Id4 == "" || $scope.AMB_Id4 == "" || $scope.AMSE_Id4 == "" || $scope.ACMS_Id4 == "" || $scope.ISMS_Id4 == "" || $scope.NOD_4 === "" || $scope.NOD_4 === 0) {
                swal('Please Select Feilds Class,Section,Subject And Enter No Of Days (Not Zero) !');
            }
            else if ($scope.days_count < parseInt($scope.NOD_4)) {
                swal("No Of Days Can't Exceed The School Days Per Week !!!!");
            }
            else {

                var cors = "";
                var bran = "";
                var sem = "";
                var sect = "";
                var subj = "";


                angular.forEach($scope.courselist, function (cc) {
                    if (cc.amcO_Id == $scope.AMCO_Id4) {
                        cors = cc.amcO_CourseName;
                    }
                })

                angular.forEach($scope.branchlist, function (bb) {
                    if (bb.amB_Id == $scope.AMB_Id4) {
                        bran = bb.amB_BranchName;
                    }
                })

                angular.forEach($scope.semisterlist, function (ss) {
                    if (ss.amsE_Id == $scope.AMSE_Id4) {
                        sem = ss.amsE_SEMName;
                    }
                })
                angular.forEach($scope.sectionlist, function (scc) {
                    if (scc.acmS_Id == $scope.ACMS_Id4) {
                        sect = scc.acmS_SectionName;
                    }
                })
                angular.forEach($scope.subjectlist, function (sbb) {
                    if (sbb.ismS_Id == $scope.ISMS_Id4) {
                        subj = sbb.ismS_SubjectName;
                    }
                })
                //  period =  $scope.NOP_2;
                day4 = parseInt($scope.NOD_4);

                if ($scope.albumNameArray4.length === 0) {

                    $scope.albumNameArray4.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, subDisplay: subj, dayDisplay: day4, AMCO_Id: $scope.AMCO_Id4, AMB_Id: $scope.AMB_Id4, AMSE_Id: $scope.AMSE_Id4, ACMS_Id: $scope.ACMS_Id4, ISMS_Id: $scope.ISMS_Id4 });
                    $scope.albumNameArraysaveDB4.push({ AMCO_Id: $scope.AMCO_Id4, AMB_Id: $scope.AMB_Id4, AMSE_Id: $scope.AMSE_Id4, ACMS_Id: $scope.ACMS_Id4, ISMS_Id: $scope.ISMS_Id4, NOD: parseInt($scope.NOD_4) });

                    // Clear input fields after push

                    $scope.AMCO_Id4 = "";
                    $scope.AMB_Id4 = "";
                    $scope.ISMS_Id4 = "";
                    $scope.AMSE_Id4 = "";
                    $scope.ACMS_Id4 = "";
                    $scope.NOD_4 = 0;
                    
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray4.length; k++) {
                        
                        if ($scope.albumNameArray4[k].AMCO_Id == $scope.AMCO_Id4 && $scope.albumNameArray4[k].AMB_Id == $scope.AMB_Id4 && $scope.albumNameArray4[k].AMSE_Id == $scope.AMSE_Id4 && $scope.albumNameArray4[k].ACMS_Id == $scope.ACMS_Id4 && $scope.albumNameArray4[k].ISMS_Id == $scope.ISMS_Id4) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray4.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, subDisplay: subj, dayDisplay: day4, AMCO_Id: $scope.AMCO_Id4, AMB_Id: $scope.AMB_Id4, AMSE_Id: $scope.AMSE_Id4, ACMS_Id: $scope.ACMS_Id4, ISMS_Id: $scope.ISMS_Id4 });
                        $scope.albumNameArraysaveDB4.push({ AMCO_Id: $scope.AMCO_Id4, AMB_Id: $scope.AMB_Id4, AMSE_Id: $scope.AMSE_Id4, ACMS_Id: $scope.ACMS_Id4, ISMS_Id: $scope.ISMS_Id4, NOD: parseInt($scope.NOD_4) });

                        // Clear input fields after push

                        $scope.AMCO_Id4 = "";
                        $scope.AMB_Id4 = "";
                        $scope.ISMS_Id4 = "";
                        $scope.AMSE_Id4 = "";
                        $scope.ACMS_Id4 = "";
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
        $scope.TransferDatagrid5 = function () {
            if ($scope.AMCO_Id5 == undefined || $scope.AMB_Id5 == undefined || $scope.AMSE_Id5 == undefined || $scope.ACMS_Id5 == undefined || $scope.HRME_Id5 === undefined || $scope.NOD_5 == undefined || $scope.AMCO_Id5 == "" || $scope.AMB_Id5 == "" || $scope.AMSE_Id5 == "" || $scope.ACMS_Id5 == "" || $scope.HRME_Id5 == ""  || $scope.NOD_5 === "" || $scope.NOD_5 === 0) {
                swal('Please Select Feilds Class,Section,Subject And Enter No Of Days (Not Zero) !');
            }
            else if ($scope.days_count < parseInt($scope.NOD_5)) {
                swal("No Of Days Can't Exceed The School Days Per Week !!!!");
            }
            else {

                var cors = "";
                var bran = "";
                var sem = "";
                var sect = "";
                var stff = "";


                angular.forEach($scope.courselist, function (cc) {
                    if (cc.amcO_Id == $scope.AMCO_Id5) {
                        cors = cc.amcO_CourseName;
                    }
                })

                angular.forEach($scope.branchlist, function (bb) {
                    if (bb.amB_Id == $scope.AMB_Id5) {
                        bran = bb.amB_BranchName;
                    }
                })

                angular.forEach($scope.semisterlist, function (ss) {
                    if (ss.amsE_Id == $scope.AMSE_Id5) {
                        sem = ss.amsE_SEMName;
                    }
                })
                angular.forEach($scope.sectionlist, function (scc) {
                    if (scc.acmS_Id == $scope.ACMS_Id5) {
                        sect = scc.acmS_SectionName;
                    }
                })
                angular.forEach($scope.stafflist, function (sbb) {
                    if (sbb.hrmE_Id == $scope.HRME_Id5) {
                        stff = sbb.staffName;
                    }
                })
               

                if ($scope.albumNameArray5.length === 0) {

                    $scope.albumNameArray5.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, stfDisplay: stff, dayDisplay: parseInt($scope.NOD_5), AMCO_Id: $scope.AMCO_Id5, AMB_Id: $scope.AMB_Id5, AMSE_Id: $scope.AMSE_Id5, ACMS_Id: $scope.ACMS_Id5, HRME_Id: $scope.HRME_Id5 });
                    $scope.albumNameArraysaveDB5.push({ AMCO_Id: $scope.AMCO_Id5, AMB_Id: $scope.AMB_Id5, AMSE_Id: $scope.AMSE_Id5, ACMS_Id: $scope.ACMS_Id5, HRME_Id: $scope.HRME_Id5, NOD: parseInt($scope.NOD_5) });
                    // Clear input fields after push

                    $scope.AMCO_Id5 = "";
                    $scope.AMB_Id5 = "";
                    $scope.AMSE_Id5 = "";
                    $scope.ACMS_Id5 = "";
                    $scope.HRME_Id5 = "";
                    $scope.NOD_5 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray5.length; k++) {

                        if ($scope.albumNameArray5[k].AMCO_Id == $scope.AMCO_Id5 && $scope.albumNameArray5[k].AMB_Id == $scope.AMB_Id5 && $scope.albumNameArray5[k].AMSE_Id == $scope.AMSE_Id5 && $scope.albumNameArray5[k].ACMS_Id == $scope.ACMS_Id5 && $scope.albumNameArray5[k].HRME_Id == $scope.HRME_Id5) {
                            condition = 1;
                            //pedcount -= period;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {

                        $scope.albumNameArray5.push({ corDisplay: cors, brDisplay: bran, semDisplay: sem, secDisplay: sect, stfDisplay: stff, dayDisplay: parseInt($scope.NOD_5), AMCO_Id: $scope.AMCO_Id5, AMB_Id: $scope.AMB_Id5, AMSE_Id: $scope.AMSE_Id5, ACMS_Id: $scope.ACMS_Id5, HRME_Id: $scope.HRME_Id35 });
                        $scope.albumNameArraysaveDB5.push({ AMCO_Id: $scope.AMCO_Id5, AMB_Id: $scope.AMB_Id5, AMSE_Id: $scope.AMSE_Id5, ACMS_Id: $scope.ACMS_Id5, HRME_Id: $scope.HRME_Id5, NOD: parseInt($scope.NOD_5) });

                        // Clear input fields after push
                        $scope.AMCO_Id5 = "";
                        $scope.AMB_Id5 = "";
                        $scope.AMSE_Id5 = "";
                        $scope.ACMS_Id5 = "";
                        $scope.HRME_Id5 = "";
                        $scope.NOD_5 = 0;
                    }
                }

                
            }

            $scope.gridOptions5_sub.data = $scope.albumNameArray5;

        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid2 = function (option) {
            debugger;
            for (var i = $scope.albumNameArray2.length - 1; i >= 0; i--) {
                if ($scope.albumNameArray2[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArray2[i].AMB_Id == option.AMB_Id && $scope.albumNameArray2[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArray2[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArray2[i].ISMS_Id == option.ISMS_Id ) {
                    $scope.albumNameArray2.splice(i, 1);
                }
            }

            for (var i = $scope.albumNameArraysaveDB2.length - 1; i >= 0; i--) {
                if ($scope.albumNameArraysaveDB2[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArraysaveDB2[i].AMB_Id == option.AMB_Id && $scope.albumNameArraysaveDB2[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArraysaveDB2[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArraysaveDB2[i].ISMS_Id == option.ISMS_Id) {

                    $scope.albumNameArraysaveDB2.splice(i, 1);
                }
            }

       
            $scope.gridOptions2_sub.data = $scope.albumNameArray2;

        };


        //TO  delete Record Right grid
        $scope.deletedatarightgrid3 = function (option) {
            

            debugger;
            for (var i = $scope.albumNameArray3.length - 1; i >= 0; i--) {
                if ($scope.albumNameArray3[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArray3[i].AMB_Id == option.AMB_Id && $scope.albumNameArray3[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArray3[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArray3[i].HRME_Id == option.HRME_Id) {
                    $scope.albumNameArray3.splice(i, 1);
                }
            }

            for (var i = $scope.albumNameArraysaveDB3.length - 1; i >= 0; i--) {
                if ($scope.albumNameArraysaveDB3[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArraysaveDB3[i].AMB_Id == option.AMB_Id && $scope.albumNameArraysaveDB3[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArraysaveDB3[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArraysaveDB3[i].HRME_Id == option.HRME_Id) {

                    $scope.albumNameArraysaveDB3.splice(i, 1);
                }
            }
            
            $scope.gridOptions3_sub.data = $scope.albumNameArray3;

        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid4 = function (option) {
            debugger;
            for (var i = $scope.albumNameArray4.length - 1; i >= 0; i--) {
                if ($scope.albumNameArray4[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArray4[i].AMB_Id == option.AMB_Id && $scope.albumNameArray4[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArray4[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArray4[i].ISMS_Id == option.ISMS_Id) {
                    $scope.albumNameArray4.splice(i, 1);
                }
            }

            for (var i = $scope.albumNameArraysaveDB4.length - 1; i >= 0; i--) {
                if ($scope.albumNameArraysaveDB4[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArraysaveDB4[i].AMB_Id == option.AMB_Id && $scope.albumNameArraysaveDB4[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArraysaveDB4[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArraysaveDB4[i].ISMS_Id == option.ISMS_Id) {

                    $scope.albumNameArraysaveDB4.splice(i, 1);
                }
            }
            $scope.gridOptions4_sub.data = $scope.albumNameArray4;

        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid5 = function (option) {
            
            debugger;
            for (var i = $scope.albumNameArray5.length - 1; i >= 0; i--) {
                if ($scope.albumNameArray5[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArray5[i].AMB_Id == option.AMB_Id && $scope.albumNameArray5[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArray5[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArray5[i].HRME_Id == option.HRME_Id) {
                    $scope.albumNameArray5.splice(i, 1);
                }
            }

            for (var i = $scope.albumNameArraysaveDB5.length - 1; i >= 0; i--) {
                if ($scope.albumNameArraysaveDB5[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArraysaveDB5[i].AMB_Id == option.AMB_Id && $scope.albumNameArraysaveDB5[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArraysaveDB5[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArraysaveDB5[i].HRME_Id == option.HRME_Id) {

                    $scope.albumNameArraysaveDB5.splice(i, 1);
                }
            }
            $scope.gridOptions5_sub.data = $scope.albumNameArray5;

        };


    }

})();