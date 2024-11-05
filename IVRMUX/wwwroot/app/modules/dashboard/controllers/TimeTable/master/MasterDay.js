
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterDayController', MasterDayController)

    MasterDayController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function MasterDayController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
        $scope.editEmployee = {};



        $scope.closere = function () {
            $('#myModalreadmit').modal('hide');
            $state.reload();
        }



        $scope.dayorderlist = [];
        $scope.saveorder = function (newuser2) {
            var data = {
                "ordeidss": $scope.dayorderlist
            }
            apiService.create("MasterDay/saveorder", data).then(function (promise) {
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
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttmD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmD_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
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
                  { name: 'SlNo', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'academicyr', displayName: 'Academic Year' },
                   { name: 'classname', displayName: 'Class Name' },
                  { name: 'ttmD_DayName', displayName: 'Day Name' },

                  {
                      field: 'id', name: '',
                      displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                    '<div class="grid-action-cell">' +                  
                   '<a ng-if="row.entity.ttmdC_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive1(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttmdC_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive1(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
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
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveday = function () {
            $scope.submitted = true;

            if ($scope.myForm1.$valid) {

                var data = {
                    "TTMD_DayName": $scope.ttmD_DayName,
                    "TTMD_DayCode": $scope.ttmD_DayCode,
                    "TTMD_Id": $scope.TTMD_Id
                }
                apiService.create("MasterDay/savedetail", data).
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
            apiService.getDATA("MasterDay/getalldetails").
       then(function (promise) {
           
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.gridOptions.data = promise.daylist;
           $scope.gridOptions2.data = promise.daylistdetail;
           $scope.daylist1 = promise.daylisttwo;
           $scope.temp_daylist1 = promise.daylisttwo;
           $scope.datatwo();


       })
        };


        $scope.getyearorder = function () {

            apiService.getDATA("MasterDay/getorder").then(function (promise) {
                if (promise != null) {
                    $scope.dayorderlist = promise.dayorderlist
                    if (promise.dayorderlist != null &&  promise.dayorderlist.length > 0) {
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
            $scope.ASMAY_Id2 = "";
            $scope.TTMDC_Id = 0;
            $scope.categoryl = "";
            $scope.Category_list = $scope.temp_category_list;
            $scope.classlist = $scope.Temp_class_list;
            $scope.daylist1 = $scope.temp_daylist1;
            $scope.submitted1 = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
            $scope.search = "";
            angular.forEach($scope.classlist, function (role) {
                role.class = false;
            })
            angular.forEach($scope.daylist1, function (role) {
                role.objday = false;
            })
        };

        $scope.submitted1 = false;
        $scope.save_day_class = function () {
            $scope.submitted1 = true;
            $scope.checkclaaslst = [];
            angular.forEach($scope.classlist, function (option) {
                if (!!option.class) $scope.checkclaaslst.push(option);
            })
            $scope.checkdaylst = [];
            angular.forEach($scope.daylist1, function (optionday) {
                if (!!optionday.gg) $scope.checkdaylst.push(optionday);
            })

            var allow;
            if ($scope.myForm2.$valid) {
                if ($scope.checkdaylst.length === 0 || $scope.checkclaaslst.length === 0) {
                    swal('Atleast select one checkbox');
                }
                else {
                    if ($scope.TTMDC_Id > 0)
                    {
                        if($scope.checkdaylst.length === 1 && $scope.checkclaaslst.length === 1)
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
                            "TTMDC_Id": $scope.TTMDC_Id,
                            temp_day_Array: $scope.checkdaylst,
                            "ASMAY_Id": $scope.ASMAY_Id2,
                            Temp_class_Array: $scope.checkclaaslst
                        }
                        apiService.create("MasterDay/savedaydetail/", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    if ($scope.TTMDC_Id > 0 && $scope.TTMDC_Id != undefined) {
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
                                //        $scope.gridOptions2.data = promise.all_list;
                                $scope.BindData();
                            })
                        $scope.clearid2();

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

            return !$scope.classlist.some(function (options) {
                return options.class;
            });
        }

        $scope.isOptionsRequired1 = function () {

            return !$scope.daylist1.some(function (optionday) {
                return optionday.gg;
            });
        }

        $scope.get_class = function () {
            
            if ($scope.ASMAY_Id2 === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id2 != "" && $scope.categoryl != "") {
                var data = {
                    "TTMC_Id": $scope.categoryl,
                    "ASMAY_Id": $scope.ASMAY_Id2,
                }
                apiService.create("PeriodAllocation/getclass_catg", data).
         then(function (promise) {

             $scope.classlist = promise.tempararyArray;
             if ($scope.TTMPC_Id != "" && $scope.TTMPC_Id != 0) {
                 angular.forEach($scope.classlist, function (role) {
                     
                     if (role.asmcL_Id == $scope.temp_class) {
                         role.class = true;
                         $scope.class = role.asmcL_Id;
                     }
                 })
             }
             if (promise.tempararyArray == "" || promise.tempararyArray == null) {
                 swal("No classes Are Mapped To Selected Category");
             }
         })
            }
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
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttmD_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterDay/getdetails", pageid).
            then(function (promise) {
                $scope.ttmD_DayName = promise.daylistedit[0].ttmD_DayName;
                $scope.ttmD_DayCode = promise.daylistedit[0].ttmD_DayCode;
                $scope.TTMD_Id = promise.daylistedit[0].ttmD_Id;
            })
        }

        $scope.getorgvalue2 = function (employee) {
            $scope.editEmployee = employee.ttmdC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("MasterDay/getdaydetails", pageid).
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
                    apiService.DeleteURI("MasterDay/deletepages", pageid).
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

            return $scope.submitted || field.$dirty;
        };

        $scope.interacted2 = function (field) {

            return $scope.submitted1 || field.$dirty;
        };
   

        $scope.deactive = function (employee, SweetAlert) {
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

                apiService.create("MasterDay/deactivate", employee).
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

        $scope.deactive1 = function (employee, SweetAlert) {
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

                apiService.create("MasterDay/deactivate1", employee).
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