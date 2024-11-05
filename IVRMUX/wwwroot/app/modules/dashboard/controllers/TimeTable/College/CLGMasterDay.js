
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGMasterDayController', CLGMasterDayController)

    CLGMasterDayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function CLGMasterDayController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                  { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'ttmD_DayName', displayName: 'Day Name' },
                { name: 'ttmD_DayCode', displayName: 'Day Code' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                       '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.editDay(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.daydeactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmD_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.daydeactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ]
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.dayorderlist) {
                    $scope.dayorderlist[index].order_Id = Number(index) + 1;

                }


            }
        };
        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SlNo', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'amcO_CourseName', displayName: 'Course' },
                { name: 'amB_BranchName', displayName: 'Branch' },
                { name: 'amsE_SEMName', displayName: 'Semester' },
                { name: 'ttmD_DayName', displayName: 'Day Name' },

                  {
                      field: 'id', name: '',
                      displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                    '<div class="grid-action-cell">' +                  
                   '<a ng-if="row.entity.ttmdC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactivecrsday(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmdC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactivecrsday(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                    '</div>'
                  }
            ]

        };


        $scope.getyearorder = function () {

            apiService.getDATA("CLGMasterDay/getorder").then(function (promise) {
                if (promise != null) {
                    $scope.dayorderlist = promise.dayorderlist
                    if (promise.dayorderlist != null && promise.dayorderlist.length > 0) {
                        $scope.dayorderlist = promise.dayorderlist
                        $scope.details = true;
                    } else {
                        swal("No Records Found");
                        $scope.details = false;
                    }
                } else {
                    swal("No Records Found");
                    $scope.details = false;
                }
            })
        }


        $scope.closere = function () {
            $('#myModalreadmit').modal('hide');
            $state.reload();
        }



        $scope.dayorderlist = [];
        $scope.saveorder = function (newuser2) {
            var data = {
                "ordeidss": $scope.dayorderlist
            }
            apiService.create("CLGMasterDay/saveorder", data).then(function (promise) {
                if (promise != null) {
                    if (promise.returnval == true) {
                        swal("Updated Successfully")
                    }
                    else {
                        swal("Failed To Update")
                    }
                    $state.reload();
                }
            })
        }



        $scope.submitted = false;
        $scope.saveday = function () {
            $scope.submitted = true;

            if ($scope.myForm1.$valid) {

                var data = {
                    "TTMD_DayName": $scope.ttmD_DayName,
                    "TTMD_DayCode": $scope.ttmD_DayCode,
                    "TTMD_Id": $scope.TTMD_Id
                }
                apiService.create("CLGMasterDay/saveday", data).
                    then(function (promise) {
                        if (promise.returnval === true) {

                            // swal('Data successfully Saved');
                            if ($scope.TTMD_Id > 0 && $scope.TTMD_Id != undefined) {
                                swal('Data Updated successfully');
                            }
                            else {
                                swal('Data Saved successfully');
                            }

                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                        $scope.BindData();
                        $scope.clearid1();
                    })
            }

        };


        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageidd = 5;
            apiService.getURI("CLGMasterDay/getalldetails", pageidd).
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
       
           $scope.daydropdown = promise.daydropdown;
           $scope.year_list = promise.yearlist;
           $scope.courselist = promise.courselist;
           $scope.Category_list = promise.categorylist;
           $scope.temp_daylist1 = promise.daylisttwo;

           $scope.gridOptions.data = promise.daylist;
           $scope.gridOptions2.data = promise.daymappedlist;
           //$scope.datatwo();


       })
        };
        $scope.datatwo = function () {

            apiService.getDATA("PeriodAllocation/getalldetails").
       then(function (promise) {
           
           $scope.year_list = promise.acayear;
           $scope.Category_list = promise.catelist;
           //$scope.daylist1 = promise.daylisttwo;
           $scope.Temp_class_list = promise.tempararyArray;
           $scope.classlist = promise.tempararyArray;
           $scope.temp_category_list = promise.catelist;

       })
        };

        $scope.clearid2 = function () {
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.TTMC_Id = "";
            $scope.AMB_Id = "";
            $scope.TTMDC_Id = 0;
            $scope.categoryl = "";
        
            $scope.submitted1 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
            angular.forEach($scope.semisterlist, function (role) {
                role.selected = false;
            })
            angular.forEach($scope.daydropdown, function (role) {
                role.selected = false;
            })
        };

        $scope.submitted1 = false;

        $scope.save_day_sem = function () {
            $scope.submitted1 = true;
            $scope.checksemlst = [];
            angular.forEach($scope.semisterlist, function (option) {
                if (option.selected == true) {

                    $scope.checksemlst.push(option);
                }
                    
            })
            $scope.checkdaylst = [];
            angular.forEach($scope.daydropdown, function (optionday) {
                if (optionday.selected == true) {
                    $scope.checkdaylst.push(optionday);
                }
            })

            var allow;
            if ($scope.myForm2.$valid) {
                if ($scope.checkdaylst.length === 0 || $scope.checksemlst.length === 0) {
                    swal('Atleast select one checkbox');
                }
                else {
                    if ($scope.TTMDC_Id > 0)
                    {
                        if($scope.checkdaylst.length === 1 && $scope.checksemlst.length === 1)
                        {
                            allow = 'Allow';
                        }
                        else
                        {
                            allow = 'Not Allow';
                        }
                    }
                    else {
                        allow = 'Allow';
                    }


                    if (allow == 'Allow') {
                        var data = {
                            "ASMAY_Id": $scope.ASMAY_Id,
                            "TTMDC_Id": $scope.TTMDC_Id,
                            "AMCO_Id": $scope.AMCO_Id,
                            "AMB_Id": $scope.AMB_Id,
                            "TTMC_Id": $scope.TTMC_Id,
                            dayids: $scope.checkdaylst,
                            semids: $scope.checksemlst
                        }
                        apiService.create("CLGMasterDay/savesemday/", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    //if ($scope.TTMDC_Id > 0 && $scope.TTMDC_Id != undefined) {
                                        swal('Data Updated successfully');
                                   // }
                                    //else {
                                        swal('Data Saved successfully');
                                    //}
                                }
                                else if (promise.returnval === false) {
                                    swal('Records Already Exist !');
                                }
                                else {
                                    swal('Data Not Saved !');
                                }
                                //        $scope.gridOptions2.data = promise.all_list;
                                
                            })
                        $state.reload();

                    }
                    else {
                        swal('Only One Record Can update At a Time!');
                    }

                }
            }
            else
            {
                $scope.submitted1 = true;
            }
        };

        $scope.isOptionsRequired = function () {

            return !$scope.semisterlist.some(function (options) {
                return options.selected;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.daydropdown.some(function (optionday) {
                return optionday.selected;
            });
        }



        $scope.yearchange = function () {
            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.semisterlist = [];

        }


        $scope.get_course = function () {

            $scope.AMB_Id = '';
            $scope.semisterlist = [];
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
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

        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.semisterlist = [];
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }
                apiService.create("CLGTTCommon/getbranch_catg", data).
         then(function (promise) {

             $scope.branchlist = promise.branchlist;
            
             if (promise.branchlist == "" || promise.branchlist == null) {
                 swal("No Branch Are Mapped To Selected Category/Course");
             }
         })
           
        };
        $scope.get_semister = function () {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMB_Id": $scope.AMB_Id,
                }
            apiService.create("CLGTTCommon/get_semister", data).
         then(function (promise) {

             $scope.semisterlist = promise.semisterlist;
            
             if (promise.semisterlist == "" || promise.semisterlist == null) {
                 swal("No Semester Are Mapped To Selected Course/Branch");
             }
         })
           
        };
        //TO clear  data
        $scope.clearid1 = function () {
            $scope.ttmD_DayName = "";
            $scope.ttmD_DayCode = "";
            $scope.TTMD_Id = 0;
            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.search = "";



        };


        //TO  Edit Record
        $scope.editDay = function (employee) {
            debugger;
            $scope.editEmployee = employee.ttmD_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CLGMasterDay/editDay", pageid).
            then(function (promise) {
                $scope.ttmD_DayName = promise.daylistedit[0].ttmD_DayName;
                $scope.ttmD_DayCode = promise.daylistedit[0].ttmD_DayCode;
                $scope.TTMD_Id = promise.daylistedit[0].ttmD_Id;
            })
        }

        $scope.getorgvalue2 = function (employee) {
            $scope.editEmployee = employee.ttmdC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CLGMasterDay/getdaydetails", pageid).
            then(function (promise) {
                $scope.TTMDC_Id = promise.daydetailedit[0].ttmdC_Id;
                $scope.ASMAY_Id2 = promise.daydetailedit[0].asmaY_Id;
                $scope.categoryl = promise.daydetailedit[0].ttmC_Id;
                for (var i = 0; i < $scope.classlist.length; i++) {
                    if ($scope.classlist[i].asmcL_Id == promise.daydetailedit[0].asmcL_Id) {
                        $scope.classlist[i].class = true;
                    }
                }
                for (var i = 0; i < $scope.daylist1.length; i++) {
                    if ($scope.daylist1[i].ttmD_Id == promise.daydetailedit[0].ttmD_Id) {
                        $scope.daylist1[i].gg = true;
                    }
                }               
            })
            $scope.clearid2();
        }


        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttmD_Id;
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
                    apiService.DeleteURI("CLGMasterDay/deletepages", pageid).
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


        $scope.interacted = function (field) {

            return $scope.submitted ;
        };

        $scope.interacted2 = function (field) {

            return $scope.submitted1;
        };
   

        $scope.daydeactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmD_ActiveFlag === true) {
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

                apiService.create("CLGMasterDay/daydeactive", employee).
                    then(function (promise) {
                        if (promise.returnMsg=='E') {
                            swal("Day is Already Mapped with Course");
                        }
                        else {
                            if (promise.returnval == true) {
                                swal(confirmmgs + " " + "successfully.");
                            }
                            else {
                                swal(confirmmgs + " " + " successfully");
                            }
                        }
                   
                    $scope.BindData();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

        $scope.deactivecrsday = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttmdC_ActiveFlag === true) {
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

                apiService.create("CLGMasterDay/deactivecrsday", employee).
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