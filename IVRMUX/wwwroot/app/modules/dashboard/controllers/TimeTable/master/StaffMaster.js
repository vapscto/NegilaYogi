
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffMasterController', StaffMasterController)

    StaffMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function StaffMasterController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};
       

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },                    
              { name: 'staffName', displayName: 'Staff Name' },
            { name: 'ttmsaB_Abbreviation', displayName: 'Staff Abbreviation' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:            
                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmsaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmsaB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
          
                '</div>'
               }
            ]          

        };

        $scope.daychange = function () {
            if ($scope.TTMSAB_PerWeekMaxDeputation != undefined && $scope.TTMSAB_PerWeekMaxDeputation != null && $scope.TTMSAB_PerWeekMaxDeputation != '') {
                $scope.TTMSAB_PerWeekMaxDeputation = 0;
            }
            if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {
                $scope.TTMSAB_PerMonthMaxDeputation = 0;
            }

            if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                $scope.TTMSAB_PerYearMaxDeputation = 0;
            }

        }


        $scope.weekchange = function () {
            debugger;
            if ($scope.TTMSAB_PerDayMaxDeputation != undefined && $scope.TTMSAB_PerDayMaxDeputation != null && $scope.TTMSAB_PerDayMaxDeputation != '') {

                if ($scope.TTMSAB_PerWeekMaxDeputation != undefined && $scope.TTMSAB_PerWeekMaxDeputation != null && $scope.TTMSAB_PerWeekMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerDayMaxDeputation) > parseInt($scope.TTMSAB_PerWeekMaxDeputation)) {

                        swal('Weekly Deputation Must be greater than Or Equal to Day Deputation')
                        $scope.TTMSAB_PerWeekMaxDeputation = 0;
                    }
                }

            }


            if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {
                $scope.TTMSAB_PerMonthMaxDeputation = 0;
            }

            if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                $scope.TTMSAB_PerYearMaxDeputation = 0;
            }

        }




        $scope.monthchange = function () {
            debugger;
            if ($scope.TTMSAB_PerDayMaxDeputation != undefined && $scope.TTMSAB_PerDayMaxDeputation != null && $scope.TTMSAB_PerDayMaxDeputation != '') {

                if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerDayMaxDeputation) > parseInt($scope.TTMSAB_PerMonthMaxDeputation)) {

                        swal('Monthly Deputation Must be greater than Or Equal to Day Deputation')
                        $scope.TTMSAB_PerMonthMaxDeputation = 0;
                    }
                }

            }


            if ($scope.TTMSAB_PerWeekMaxDeputation != undefined && $scope.TTMSAB_PerWeekMaxDeputation != null && $scope.TTMSAB_PerWeekMaxDeputation != '') {

                if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerWeekMaxDeputation) > parseInt($scope.TTMSAB_PerMonthMaxDeputation)) {

                        swal('Monthly Deputation Must be greater than Or Equal to Weekly Deputation')
                        $scope.TTMSAB_PerMonthMaxDeputation = 0;
                    }
                }

            }
            

            if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                $scope.TTMSAB_PerYearMaxDeputation = 0;
            }

        }

        $scope.monthchange = function () {
            debugger;
            if ($scope.TTMSAB_PerDayMaxDeputation != undefined && $scope.TTMSAB_PerDayMaxDeputation != null && $scope.TTMSAB_PerDayMaxDeputation != '') {

                if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerDayMaxDeputation) > parseInt($scope.TTMSAB_PerMonthMaxDeputation)) {

                        swal('Monthly Deputation Must be greater than Or Equal to Day Deputation')
                        $scope.TTMSAB_PerMonthMaxDeputation = 0;
                    }
                }

            }


            if ($scope.TTMSAB_PerWeekMaxDeputation != undefined && $scope.TTMSAB_PerWeekMaxDeputation != null && $scope.TTMSAB_PerWeekMaxDeputation != '') {

                if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerWeekMaxDeputation) > parseInt($scope.TTMSAB_PerMonthMaxDeputation)) {

                        swal('Monthly Deputation Must be greater than Or Equal to Weekly Deputation')
                        $scope.TTMSAB_PerMonthMaxDeputation = 0;
                    }
                }

            }


            if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                $scope.TTMSAB_PerYearMaxDeputation = 0;
            }

        }


        $scope.yearchange = function () {
            debugger;
            if ($scope.TTMSAB_PerDayMaxDeputation != undefined && $scope.TTMSAB_PerDayMaxDeputation != null && $scope.TTMSAB_PerDayMaxDeputation != '') {

                if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerDayMaxDeputation) > parseInt($scope.TTMSAB_PerYearMaxDeputation)) {

                        swal('Yearly Deputation Must be greater than Or Equal to Day Deputation')
                        $scope.TTMSAB_PerYearMaxDeputation = 0;
                    }
                }

            }


            if ($scope.TTMSAB_PerWeekMaxDeputation != undefined && $scope.TTMSAB_PerWeekMaxDeputation != null && $scope.TTMSAB_PerWeekMaxDeputation != '') {

                if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerWeekMaxDeputation) > parseInt($scope.TTMSAB_PerYearMaxDeputation)) {

                        swal('Yearly Deputation Must be greater than Or Equal to Weekly Deputation')
                        $scope.TTMSAB_PerYearMaxDeputation = 0;
                    }
                }

            }

            if ($scope.TTMSAB_PerMonthMaxDeputation != undefined && $scope.TTMSAB_PerMonthMaxDeputation != null && $scope.TTMSAB_PerMonthMaxDeputation != '') {

                if ($scope.TTMSAB_PerYearMaxDeputation != undefined && $scope.TTMSAB_PerYearMaxDeputation != null && $scope.TTMSAB_PerYearMaxDeputation != '') {
                    if (parseInt($scope.TTMSAB_PerMonthMaxDeputation) > parseInt($scope.TTMSAB_PerYearMaxDeputation)) {

                        swal('Yearly Deputation Must be greater than Or Equal to Monthly Deputation')
                        $scope.TTMSAB_PerYearMaxDeputation = 0;
                    }
                }

            }


            

        }

        // TO Save The Data
        $scope.submitted = false;
        $scope.savestaffabbreviationdata = function () {
          

            if ($scope.myForm.$valid) {

                var data = {
                    "TTMSAB_Id":$scope.TTMSAB_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "TTMSAB_Abbreviation": $scope.StaffAbbreviation, 
                    "TTMSAB_PerDayMaxDeputation": $scope.TTMSAB_PerDayMaxDeputation, 
                    "TTMSAB_PerWeekMaxDeputation": $scope.TTMSAB_PerWeekMaxDeputation, 
                    "TTMSAB_PerMonthMaxDeputation": $scope.TTMSAB_PerMonthMaxDeputation, 
                    "TTMSAB_PerYearMaxDeputation": $scope.TTMSAB_PerYearMaxDeputation, 
                }
                apiService.create("StaffMaster/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true)
                        {
                            swal('Data successfully Saved');
                            $state.reload();
                        }
                        else if (promise.returnduplicatestatus === 'Duplicate')
                        {
                            swal('Records Already Exist !');
                            $state.reload();
                        }
                        else if (promise.returnval === false)
                        {
                            swal('Data Not Saved !');
                            $state.reload();
                        }
                        $scope.BindData();
                    })
              
                $scope.BindData();
               $scope.clearid();
            }
            else {
                $scope.submitted = true;

            }

        };

        $scope.HRME_Id = "";
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("StaffMaster/getalldetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;    
         
           $scope.stafflist = promise.stafflist;

           //$scope.albumNameArray = [];
           //for (var i = 0; i < promise.ttstafflist.length; i++) {
           //    if (promise.ttstafflist[i].firstName != '') {
           //        if (promise.ttstafflist[i].middleName !== null) {
           //            if (promise.ttstafflist[i].lastName !== null) {

           //                $scope.albumNameArray.push({ staffName: promise.ttstafflist[i].firstName + promise.ttstafflist[i].middleName + promise.ttstafflist[i].lastName, hrmE_Id: promise.ttstafflist[i].hrmE_Id, ttmsaB_Id: promise.ttstafflist[i].ttmsaB_Id, ttmsaB_Abbreviation: promise.ttstafflist[i].ttmsaB_Abbreviation, ttmsaB_ActiveFlag: promise.ttstafflist[i].ttmsaB_ActiveFlag });
           //            }
           //            else {
           //                $scope.albumNameArray.push({ staffName: promise.ttstafflist[i].firstName + promise.ttstafflist[i].middleName, hrmE_Id: promise.ttstafflist[i].hrmE_Id, ttmsaB_Id: promise.ttstafflist[i].ttmsaB_Id, ttmsaB_Abbreviation: promise.ttstafflist[i].ttmsaB_Abbreviation, ttmsaB_ActiveFlag: promise.ttstafflist[i].ttmsaB_ActiveFlag });
           //            }
           //        }
           //        else {
           //            $scope.albumNameArray.push({ staffName: promise.ttstafflist[i].firstName, hrmE_Id: promise.ttstafflist[i].hrmE_Id, ttmsaB_Id: promise.ttstafflist[i].ttmsaB_Id, ttmsaB_Abbreviation: promise.ttstafflist[i].ttmsaB_Abbreviation, ttmsaB_ActiveFlag: promise.ttstafflist[i].ttmsaB_ActiveFlag });
           //        }
           //    }
           //}
           $scope.gridOptions.data = promise.ttstafflist;
       })
        };
        //TO clear  data
        $scope.clearid= function () {      
            //$scope.submitted = false;
            //$scope.myForm.$setPristine(); 
            //$scope.myForm.$setUntouched();
            //$scope.search = "";
            //$scope.asmaY_Id = "";
            //$scope.TTMSAB_Id = 0;
            //$scope.Staff_Id = "";
            //$scope.StaffAbbreviation = "";
            $state.reload();

        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmsaB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("StaffMaster/getdetails", pageid).
            then(function (promise) {
                $scope.TTMSAB_Id = promise.sujectslistedit[0].ttmsaB_Id;

                $scope.HRME_Id = promise.sujectslistedit[0];
                $scope.HRME_Id.hrmE_Id = promise.sujectslistedit[0].hrmE_Id;
                $scope.StaffAbbreviation = promise.sujectslistedit[0].ttmsaB_Abbreviation;

                $scope.TTMSAB_PerDayMaxDeputation = promise.sujectslistedit[0].ttmsaB_PerDayMaxDeputation;
                $scope.TTMSAB_PerWeekMaxDeputation = promise.sujectslistedit[0].ttmsaB_PerWeekMaxDeputation;
                $scope.TTMSAB_PerMonthMaxDeputation = promise.sujectslistedit[0].ttmsaB_PerMonthMaxDeputation;
                $scope.TTMSAB_PerYearMaxDeputation = promise.sujectslistedit[0].ttmsaB_PerYearMaxDeputation;




            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmsaB_Id;
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
                    apiService.DeleteURI("StaffMaster/deletepages", pageid).
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
                    $scope.BindData();
                }
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        //active switch
        $scope.switch = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmsaB_ActiveFlag === true) {
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

                apiService.create("StaffMaster/deactivate", employee).
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



    }

})();