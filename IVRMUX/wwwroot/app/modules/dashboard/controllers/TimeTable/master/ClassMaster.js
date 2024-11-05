
(function () {
    'use strict';
    angular
.module('app')
.controller('ClassMasterController', ClassMasterController)

    ClassMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function ClassMasterController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
        $scope.editEmployee = {};
        var curDate = new Date();
        
        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        // Time picker end here

        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmaY_Year', displayName: 'Academic Year' },

               { name: 'staffName', displayName: 'Staff Name' },
             { name: 'ttfpD_TotWeekPeriods', displayName: ' No Of Periods' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:

                    '<div class="grid-action-cell">' +
                     '<a href="javascript:void(0)" data-toggle="modal" data-target="#popup" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttfpD_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.switch(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttfpD_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.switch(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +

                '</div>'
               }
            ]

        };
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [

                 { name: 'catDisplay', displayName: 'Category' },
              { name: 'clsDisplay', displayName: 'Class' },
            { name: 'sectDisplay', displayName: 'Section' },
             { name: 'subDisplay', displayName: 'Subject' },
              { name: 'pedDisplay', displayName: 'No Of Periods' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedatarightgrid(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };

        $scope.gridOptions2 = {
            enableColumnMenus: false,
            enableFiltering: true,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'asmaY_Year', displayName: 'Academic Year' },
                   { name: 'ttmC_CategoryName', displayName: 'Category Name' },
                   { name: 'asmcL_ClassName', displayName: 'Class Name' },
                 { name: 'asmC_SectionName', displayName: 'Section' },
                   { name: 'ttmsaB_Abbreviation', displayName: 'Staff Abbreviation' },
            { name: 'ttmsuaB_Abbreviation', displayName: 'Subject Abbreviation' },
                   { name: 'nop', displayName: 'No Of Periods' },
                  {
                      field: 'id', name: '',
                      displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.DelTemp(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
                  }
            ]

        };
        $scope.albumNameArraysaveDB = [];
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid && $scope.NOP_1 != 0) {
                if ($scope.HRME_Id != "") {


                    if ($scope.albumNameArraysaveDB.length> 0) {

                        var data = {
                            "TTFPD_Id": $scope.TTFPD_Id,
                            "TTFPDD_Id": $scope.TTFPDD_Id,
                            "ASMAY_Id": $scope.asmaY_Id,
                            "HRME_Id": $scope.HRME_Id.hrmE_Id,
                            "TTFPD_TotWeekPeriods": $scope.NOP_1,
                            "TempararyArrayList": $scope.albumNameArraysaveDB,
                            "TTFPD_StaffConsecutive": $scope.TTFPD_StaffConsecutive
                        }
                        apiService.create("ClassMaster/savedetail", data).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal('Data successfully Saved');
                                    $scope.albumNameArraysaveDB = [];
                                    $scope.gridOptions1.data = [];
                                    $scope.albumNameArray = [];
                                }
                                else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                                    swal('Records AlReady Exist !');
                                }
                                else {
                                    swal('Data Not Saved !');
                                }
                                $scope.BindData();
                            })
                        $scope.BindData();
                        $scope.clear();

                    }
                    else {
                        swal('Need To Distribute Atleast One Period');
                    }
                }
                    else {
                    swal('Select Staff');
                    }
            }
            else {
                $scope.submitted = true;

            }
        };


        //TO  GEt The Values iN Grid
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("ClassMaster/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.year_list = promise.acayear;
           $scope.day_list = promise.daylist;
           $scope.Temp_class_list = promise.classlist;
           $scope.class_list = promise.classlist;
           $scope.TTFPD_StaffConsecutive = 1;
           $scope.albumNameArray1 = [];
           //for (var i = 0; i < promise.stafflist.length; i++) {
           //    if (promise.stafflist[i].firstName != '') {
           //        if (promise.stafflist[i].middleName !== null) {
           //            if (promise.stafflist[i].lastName !== null) {

           //                $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName +' '+ promise.stafflist[i].middleName + ' '+promise.stafflist[i].lastName, hrmE_Id: promise.stafflist[i].hrmE_Id });
           //            }
           //            else {
           //                $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName+ ' '+ promise.stafflist[i].middleName, hrmE_Id: promise.stafflist[i].hrmE_Id });
           //            }
           //        }
           //        else {
           //            if (promise.stafflist[i].lastName !== null) {

           //                $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName + ' ' + promise.stafflist[i].lastName, hrmE_Id: promise.stafflist[i].hrmE_Id });
           //            }
           //            else {
           //                $scope.albumNameArray1.push({ staffName: promise.stafflist[i].firstName, hrmE_Id: promise.stafflist[i].hrmE_Id });

           //            }
           //        }
           //    }
           //}
           //$scope.staff_list = $scope.albumNameArray1;
           $scope.staff_list = promise.stafflist;
           // $scope.staff_list = promise.stafflist;
           $scope.temp_category_list = promise.categorylist;
           $scope.category_list = promise.categorylist;
           $scope.section_list = promise.sectionlist;
           $scope.subject_list = promise.subjectlist;
           $scope.gridOptions.data = promise.all_period_distri_list;


       })
        };
        $scope.asmaY_Id = "";
        $scope.ttmC_Id = "";
        $scope.get_class = function () {
            
            if ($scope.asmaY_Id == "") {
                swal("Please Select The Academic Year !");
                $scope.ttmC_Id = "";
            }
            else if ($scope.asmaY_Id != "" && $scope.ttmC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("ClassMaster/getclass_catg", data).
         then(function (promise) {

             $scope.class_list = promise.classbycategory;
             $scope.asmcL_Id = "";
             if (promise.classbycategory == "" || promise.classbycategory == null) {
                 swal("No classes Are Mapped To Selected Category");
             }
         })
            }
        };

        $scope.clear = function () {  
            $scope.TTFPD_StaffConsecutive = 1;
            $scope.TTFPD_Id = 0;
            $scope.TTFPDD_Id = 0;
            $scope.asmaY_Id = "";
            $scope.category_list = $scope.temp_category_list;
            $scope.class_list = $scope.Temp_class_list;
            $scope.albumNameArray = [];
            $scope.albumNameArraysaveDB = [];
            $scope.gridOptions1.data = $scope.albumNameArraysaveDB;
            $scope.submitted = false;
            $scope.ttmC_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.ismS_Id = "";
            $scope.HRME_Id = "";
            $scope.NOP_2 = 0;
            $scope.NOP_1 = 1;
            $scope.AVPS = $scope.NOP_1;
            pedcount = 0;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $state.reload();
        };

        //TO  delete Record Right grid
        $scope.deletedatarightgrid = function (employee) {
            
            var pageid5 = employee.catDisplay;
            var pageid1 = employee.clsDisplay;
            var pageid2 = employee.sectDisplay;
            var pageid3 = employee.subDisplay;
            var pageid4 = employee.pedDisplay;


            for (var a = 0; a < $scope.class_list.length; a++) {
                if ($scope.class_list[a].asmcL_ClassName == pageid1) {
                    
                    cls1 = $scope.class_list[a].asmcL_Id;
                }
            }
            for (var b = 0; b < $scope.section_list.length; b++) {
                if ($scope.section_list[b].asmC_SectionName == pageid2) {
                    
                    sect1 = $scope.section_list[b].asmS_Id;
                }
            }
            for (var c = 0; c < $scope.subject_list.length; c++) {
                if ($scope.subject_list[c].ismS_SubjectName == pageid3) {
                    
                    subj1 = $scope.subject_list[c].ismS_Id;
                }
            }
            period1 = pageid4;
            for (var d = 0; d < $scope.category_list.length; d++) {
                if ($scope.category_list[d].ttmC_CategoryName == pageid5) {
                    
                    cat1 = $scope.category_list[d].ttmC_Id;
                }
            }
            for (var i = $scope.albumNameArraysaveDB.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArraysaveDB[i].ASMCL_Id == cls1 && $scope.albumNameArraysaveDB[i].ASMS_Id == sect1 && $scope.albumNameArraysaveDB[i].ISMS_Id == subj1 && $scope.albumNameArraysaveDB[i].NOP == period1 && $scope.albumNameArraysaveDB[i].TTMC_Id == cat1) {
                    debugger;
                    pedcount -= $scope.albumNameArraysaveDB[i].NOP;
                    $scope.albumNameArraysaveDB.splice(i, 1);

                }
            }

            for (var i = $scope.albumNameArray.length - 1; i >= 0; i--) {
                
                if ($scope.albumNameArray[i].clsDisplay == pageid1 && $scope.albumNameArray[i].sectDisplay == pageid2 && $scope.albumNameArray[i].pedDisplay == pageid4 && $scope.albumNameArray[i].subDisplay == pageid3 && $scope.albumNameArray[i].catDisplay == pageid5) {
                    debugger;
                    $scope.albumNameArray.splice(i, 1);

                }
            }
            debugger;
            $scope.gridOptions1.data = $scope.albumNameArray;
            $scope.get_avps();
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
        //TO  View Record
        $scope.viewrecordspopup = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.ttfpD_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("ClassMaster/getalldetailsviewrecords", pageid).
                    then(function (promise) {
                        
                        $scope.staff_Name = promise.detailspopuparray[0].staffName;;
                        $scope.viewrecordspopupdisplay = promise.detailspopuparray;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.isOptionsRequired = function () {

            return !$scope.classlist.some(function (options) {
                return options.class;
            });
        }
        //TO  Edit Record
        $scope.getorgvalue2 = function (period) {
            
            $scope.editperiod = period.ttmdpT_Id;
            var editid = $scope.editperiod;
            angular.forEach($scope.classlist, function (role) {
                role.class = false;
            })
            apiService.getURI("ClassMaster/getdetails", editid).
            then(function (promise) {
                
                $scope.timing = moment(promise.periodlistedit[0].ttmdpT_StartTime, 'h:mm a').format();
                $scope.timing1 = moment(promise.periodlistedit[0].ttmdpT_EndTime, 'h:mm a').format();

                $scope.TTMDPT_Id = promise.periodlistedit[0].ttmdpT_Id;
                $scope.categoryl = promise.periodlistedit[0].ttmC_Id;
                $scope.Period = promise.periodlistedit[0].tT_Period;
                $scope.Day_Name = promise.periodlistedit[0].tT_Day;

                angular.forEach($scope.classlist, function (role) {
                    if (role.asmcL_Id == promise.periodlistedit[0].asmcL_Id)
                        role.class = true;
                })
                $scope.StartTime = $scope.timing;
                $scope.EndTime = $scope.timing1;
            })
        }

        //active switch1 for period_distri_detailed
        $scope.switch = function (employee, SweetAlert) {
            
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfpD_ActiveFlag === true) {
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

                apiService.create("ClassMaster/deactivate", employee).
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

        $scope.HRME_Id = "";

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttfpD_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("ClassMaster/getdetails", pageid).
            then(function (promise) {
                $scope.albumNameArray = [];
                $scope.albumNameArraysaveDB = [];
                $scope.gridOptions1.data = $scope.albumNameArraysaveDB;
                pedcount = promise.edit_count;
                $scope.TTFPD_StaffConsecutive = promise.period_distri_edit[0].ttfpD_StaffConsecutive;
                $scope.TTFPD_Id = promise.period_distri_edit[0].ttfpD_Id;
                $scope.asmaY_Id = promise.period_distri_edit[0].asmaY_Id;


                $scope.HRME_Id = promise.period_distri_edit[0];
                $scope.HRME_Id.hrmE_Id = promise.period_distri_edit[0].hrmE_Id;

                $scope.NOP_1 = promise.period_distri_edit[0].ttfpD_TotWeekPeriods;
                if (promise.period_distri_detail_edit != "" && promise.period_distri_detail_edit != null) {
                    $scope.TTFPDD_Id = promise.period_distri_detail_edit[0].ttfpdD_Id;
                    $scope.temp_category = promise.period_distri_detail_edit[0].ttmC_Id;
                    $scope.temp_class = promise.period_distri_detail_edit[0].asmcL_Id;

                    for (var i = 0; i < promise.period_distri_detail_edit.length; i++) {

                        $scope.albumNameArraysaveDB.push({ ASMCL_Id: promise.period_distri_detail_edit[i].asmcL_Id, ASMS_Id: promise.period_distri_detail_edit[i].asmS_Id, ISMS_Id: promise.period_distri_detail_edit[i].ismS_Id, NOP: promise.period_distri_detail_edit[i].ttfpD_TotalPeriods, TTMC_Id: promise.period_distri_detail_edit[i].ttmC_Id });

                        for (var k = 0; k < $scope.Temp_class_list.length; k++) {
                            if ($scope.Temp_class_list[k].asmcL_Id == promise.period_distri_detail_edit[i].asmcL_Id) {
                                cls = $scope.Temp_class_list[k].asmcL_ClassName;
                            }
                        }
                        for (var k = 0; k < $scope.section_list.length; k++) {
                            if ($scope.section_list[k].asmS_Id == promise.period_distri_detail_edit[i].asmS_Id) {
                                sect = $scope.section_list[k].asmC_SectionName;
                            }
                        }
                        for (var k = 0; k < $scope.subject_list.length; k++) {
                            if ($scope.subject_list[k].ismS_Id == promise.period_distri_detail_edit[i].ismS_Id) {
                                subj = $scope.subject_list[k].ismS_SubjectName;
                            }
                        }
                        period = promise.period_distri_detail_edit[i].ttfpD_TotalPeriods;
                        for (var k = 0; k < $scope.category_list.length; k++) {
                            if ($scope.category_list[k].ttmC_Id == promise.period_distri_detail_edit[i].ttmC_Id) {
                                cat = $scope.category_list[k].ttmC_CategoryName;
                            }
                        }

                        $scope.albumNameArray.push({
                            catDisplay: cat,
                            clsDisplay: cls,
                            sectDisplay: sect,
                            subDisplay: subj,
                            pedDisplay: period,
                        });
                    }
                }

                $scope.gridOptions1.data = $scope.albumNameArray;
                $scope.get_avps();

            })
        }
        $scope.min_1 = 1;

        $scope.get_avps = function () {
            debugger;
            if ($scope.TTFPD_Id === undefined || ($scope.TTFPD_Id != 0 && $scope.TTFPD_Id != "")) {

                if (pedcount == 0) {
                    $scope.AVPS = $scope.NOP_1;
                }

                $scope.AVPS = $scope.NOP_1 - pedcount;


                if ($scope.AVPS == 0 || pedcount > 0) {
                    $scope.min_1 = pedcount;
                }

            }
            else if ($scope.TTFPD_Id == 0 && $scope.TTFPD_Id == "") {
                if (pedcount == 0) {
                    $scope.AVPS = $scope.NOP_1;
                }
                $scope.AVPS = $scope.NOP_1 - pedcount;
            }

            if ($scope.AVPS <= 0) {
                $scope.AVPS = 0;
            }

        }


        $scope.NOP_1 = 1;
        $scope.NOP_2 = 0;
        $scope.albumNameArray = [];
        $scope.albumNameArraysaveDB = [];
        $scope.editEmployee = {};
        var cls, sect, subj, period, cat;
        var cls1, sect1, subj1, period1, cat1;
        var pedcount = 0;
        $scope.AVPS = $scope.NOP_1;
        $scope.TransferDatagrid = function (objcat, objcls, objsect, objsub, objped) {
            
            if (parseInt($scope.NOP_1) < parseInt($scope.NOP_2)) {
                swal("Period Distribution Can't exceed the Total for only single class !!!");
            }
            else if ($scope.ttmC_Id === undefined || $scope.asmcL_Id === undefined || $scope.asmS_Id === undefined || $scope.ismS_Id === undefined || $scope.NOP_2 === undefined || $scope.ttmC_Id === "" || $scope.asmcL_Id === "" || $scope.asmS_Id === "" || $scope.ismS_Id === "" || $scope.NOP_2 === "" || parseInt($scope.NOP_2) === 0) {
                swal('Please Select Feilds Class,Section,Subject And Enter Period (Not Zero) !');
            }
            else {
                for (var k = 0; k < $scope.class_list.length; k++) {
                    if ($scope.class_list[k].asmcL_Id == objcls) {
                        cls = $scope.class_list[k].asmcL_ClassName;
                    }
                }
                for (var k = 0; k < $scope.section_list.length; k++) {
                    if ($scope.section_list[k].asmS_Id == objsect) {
                        sect = $scope.section_list[k].asmC_SectionName;
                    }
                }

                for (var k = 0; k < $scope.subject_list.length; k++) {
                    if ($scope.subject_list[k].ismS_Id == objsub) {
                        subj = $scope.subject_list[k].ismS_SubjectName;
                    }
                }
                period = parseInt($scope.NOP_2);
                for (var k = 0; k < $scope.category_list.length; k++) {
                    if ($scope.category_list[k].ttmC_Id == objcat) {
                        cat = $scope.category_list[k].ttmC_CategoryName;
                    }
                }
                if ($scope.albumNameArray.length === 0) {
                    
                    $scope.albumNameArray.push({ catDisplay: cat, clsDisplay: cls, sectDisplay: sect, subDisplay: subj, pedDisplay: period });
                    $scope.albumNameArraysaveDB.push({ ASMCL_Id: $scope.asmcL_Id, ASMS_Id: $scope.asmS_Id, ISMS_Id: $scope.ismS_Id, NOP: parseInt($scope.NOP_2), TTMC_Id: $scope.ttmC_Id });
                    pedcount += period;
                    // Clear input fields after push
                    $scope.ttmC_Id = "";
                    $scope.asmcL_Id = "";
                    $scope.asmS_Id = "";
                    $scope.ismS_Id = "";
                    $scope.NOP_2 = 0;
                }
                else {
                    var condition = 0;
                    for (var k = 0; k < $scope.albumNameArray.length; k++) {
                        
                        if ($scope.albumNameArray[k].catDisplay === cat && $scope.albumNameArray[k].clsDisplay === cls && $scope.albumNameArray[k].sectDisplay === sect && $scope.albumNameArray[k].subDisplay === subj) {
                            condition = 1;
                            swal("Record Already Selected !");
                        }

                    }
                    if (condition === 0) {
                        $scope.albumNameArray.push({ catDisplay: cat, clsDisplay: cls, sectDisplay: sect, subDisplay: subj, pedDisplay: period });
                        $scope.albumNameArraysaveDB.push({ ASMCL_Id: $scope.asmcL_Id, ASMS_Id: $scope.asmS_Id, ISMS_Id: $scope.ismS_Id, NOP: parseInt($scope.NOP_2), TTMC_Id: $scope.ttmC_Id });
                        pedcount += period;
                        // Clear input fields after push
                        $scope.ttmC_Id = "";
                        $scope.asmcL_Id = "";
                        $scope.asmS_Id = "";
                        $scope.ismS_Id = "";
                        $scope.NOP_2 = 0;
                        condition = 1;
                    }
                }
                //}
            }

            $scope.gridOptions1.data = $scope.albumNameArray;
            $scope.get_avps();
        };



        //TO  delete Record
        $scope.deletedata = function (period, SweetAlert) {
            $scope.deleteId = period.ttmdpT_Id;
            var MdeleteId = $scope.deleteId;
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
                    apiService.DeleteURI("ClassMaster/deletepages", MdeleteId).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                        }
                        $scope.BindData();
                    })
                    //  $scope.BindData();
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

    }

})();