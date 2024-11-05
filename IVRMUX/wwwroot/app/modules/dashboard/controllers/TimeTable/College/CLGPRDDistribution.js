
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGPRDDistributionController', CLGPRDDistributionController)
    CLGPRDDistributionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter']
    function CLGPRDDistributionController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter) {
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
                 '<a href="javascript:void(0)" ng-click="grid.appScope.editprddestr(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
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
                 { name: 'crsDisplay', displayName: 'Course' },
                 { name: 'brDisplay', displayName: 'Branch' },
                 { name: 'semDisplay', displayName: 'Sem' },
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

        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            debugger;
            var cnout=0
            angular.forEach($scope.albumNameArraysaveDB, function (gg) {
                cnout += parseInt(gg.NOP);
            })

            if (cnout > parseInt($scope.NOP_1)) {
                swal("Distributed Periods Don't exceeds the Total Weekly Periods");
            }
            else {
                if ($scope.myForm.$valid && $scope.NOP_1 != 0) {
                    var data = {
                        "TTFPD_Id": $scope.TTFPD_Id,
                        "TTFPDD_Id": $scope.TTFPDD_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "HRME_Id": $scope.hrmE_Id,
                        "TTFPD_TotWeekPeriods": $scope.NOP_1,
                        "TempararyArrayList": $scope.albumNameArraysaveDB,
                        "TTFPD_StaffConsecutive": $scope.TTFPD_StaffConsecutive
                    }
                    apiService.create("CLGPRDDistribution/savedetail", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data Saved successfully !!!!');
                                $scope.albumNameArraysaveDB = "";
                                $scope.gridOptions1.data = "";
                                $scope.albumNameArray = "";
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
                    $scope.submitted = true;

                }
            }
           
        };

        $scope.yearchange= function () {

            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
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
        $scope.get_section = function () {
            var data = {
                "TTMC_Id": $scope.TTMC_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
            }
            apiService.create("CLGTTCommon/get_section", data).
                then(function (promise) {

                    $scope.section_list = promise.section_list;

                    if (promise.section_list == "" || promise.section_list == null) {
                        swal("No Sections Are Mapped To Selected Course/Branch");
                    }
                })
        };

        //TO  GEt The Values iN Grid
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("CLGPRDDistribution/getalldetails",pageid).
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.TTFPD_StaffConsecutive = 1;
           $scope.year_list = promise.yearlist;
           $scope.staff_list = promise.stafflist;
           $scope.category_list = promise.categorylist;
           $scope.subject_list = promise.subjectlist;
           $scope.sectionlist = promise.sectionlist;
          $scope.gridOptions.data = promise.all_period_distri_list;

       })
        };
        $scope.ASMAY_Id = "";
        $scope.TTMC_Id = "";
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
                apiService.create("CLGPRDDistribution/getclass_catg", data).
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
            $scope.hrmE_Id = "";
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
           
            $scope.gg = employee;

            for (var j = $scope.albumNameArraysaveDB.length - 1; j >= 0; j--) {
                if ($scope.albumNameArraysaveDB[j].TTMC_Id == employee.TTMC_Id && $scope.albumNameArraysaveDB[j].AMCO_Id == employee.AMCO_Id && $scope.albumNameArraysaveDB[j].AMB_Id == employee.AMB_Id && $scope.albumNameArraysaveDB[j].AMSE_Id == employee.AMSE_Id && $scope.albumNameArraysaveDB[j].ACMS_Id == employee.ACMS_Id && $scope.albumNameArraysaveDB[j].ISMS_Id == employee.ISMS_Id && $scope.albumNameArraysaveDB[j].NOP == employee.NOP) {

                    pedcount -= $scope.albumNameArraysaveDB[j].NOP;
                    $scope.albumNameArraysaveDB.splice(j, 1);

                    }
                }

                for (var i = $scope.albumNameArray.length - 1; i >= 0; i--) {

                    if ($scope.albumNameArray[i].TTMC_Id == employee.TTMC_Id && $scope.albumNameArray[i].AMCO_Id == employee.AMCO_Id && $scope.albumNameArray[i].AMB_Id == employee.AMB_Id && $scope.albumNameArray[i].AMSE_Id == employee.AMSE_Id && $scope.albumNameArray[i].ACMS_Id == employee.ACMS_Id && $scope.albumNameArray[i].ISMS_Id == employee.ISMS_Id && $scope.albumNameArray[i].NOP == employee.NOP) {
                        $scope.albumNameArray.splice(i, 1);

                    }
                }
         
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
            $scope.staff_Name = employee.staffName;;
            apiService.getURI("CLGPRDDistribution/viewperiods", pageid).
                    then(function (promise) {
                        
                       
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
            apiService.getURI("CLGPRDDistribution/getdetails", editid).
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

                apiService.create("CLGPRDDistribution/deactivate", employee).
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



        //TO  Edit Record
        $scope.editprddestr = function (employee) {

            apiService.create("CLGPRDDistribution/editprddestr", employee).
            then(function (promise) {
                $scope.albumNameArray = [];
                $scope.albumNameArraysaveDB = [];
                $scope.gridOptions1.data = $scope.albumNameArraysaveDB;
                pedcount = promise.edit_count;
                $scope.TTFPD_StaffConsecutive = promise.period_distri_edit[0].TTFPD_StaffConsecutive;
                $scope.TTFPD_Id = promise.period_distri_edit[0].TTFPD_Id;
                $scope.ASMAY_Id = promise.period_distri_edit[0].ASMAY_Id;
                $scope.hrmE_Id = promise.period_distri_edit[0].HRME_Id;
                $scope.NOP_1 = promise.period_distri_edit[0].TTFPD_TotWeekPeriods;

               // $scope.albumNameArray = promise.period_distri_edit;
              //  $scope.albumNameArraysaveDB = promise.period_distri_edit;


                angular.forEach(promise.period_distri_edit, function (ww) {

                    $scope.albumNameArray.push({ catDisplay: ww.catDisplay, crsDisplay: ww.crsDisplay, brDisplay: ww.brDisplay, semDisplay: ww.semDisplay, sectDisplay: ww.sectDisplay, subDisplay: ww.subDisplay, pedDisplay: ww.pedDisplay, TTMC_Id: ww.TTMC_Id, AMCO_Id: ww.AMCO_Id, AMB_Id: ww.AMB_Id, AMSE_Id: ww.AMSE_Id, ACMS_Id: ww.ACMS_Id, ISMS_Id: ww.ISMS_Id, NOP: ww.NOP });
                    $scope.albumNameArraysaveDB.push({ TTMC_Id: ww.TTMC_Id, AMCO_Id: ww.AMCO_Id, AMB_Id: ww.AMB_Id, AMSE_Id: ww.AMSE_Id, ACMS_Id: ww.ACMS_Id, ISMS_Id: ww.ISMS_Id, NOP: ww.NOP });
                })
                
                $scope.gridOptions1.data = $scope.albumNameArray;
                $scope.get_avps();

            })
        }
        $scope.min_1 = 1;

        $scope.get_avps = function () {
            
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
        var cors,bran,sem,sect, subj, period, cat;
        var cors1, bran1, sem1, sect1, subj1, period1, cat1;
        var pedcount = 0;
        $scope.AVPS = $scope.NOP_1;
       

        $scope.TransferDatagrid = function (objcat, objcrs, objbr, objsem, objsec, objism, objped) {
            var pdcnt = 0;
            if ($scope.albumNameArraysaveDB.length > 0) {
               
                angular.forEach($scope.albumNameArraysaveDB, function (ff) {
                    pdcnt += parseInt(ff.NOP);

                })
            }
            if ($scope.albumNameArraysaveDB.length > 0 && parseInt($scope.NOP_1)<(pdcnt + parseInt($scope.NOP_2))) {
                swal("Period Distribution Can't exceed the Total Periods !!!");
            }
            else
                if (parseInt($scope.NOP_1) < parseInt($scope.NOP_2)) {
                swal("Period Distribution Can't exceed the Total Periods !!!");
            }
            else if ($scope.TTMC_Id === undefined || $scope.AMCO_Id === undefined || $scope.AMB_Id === undefined || $scope.AMSE_Id === undefined || $scope.ACMS_Id === undefined || $scope.ismS_Id === undefined || $scope.NOP_2 === undefined || $scope.TTMC_Id === "" || $scope.AMCO_Id === "" || $scope.AMB_Id === "" || $scope.AMSE_Id === "" || $scope.ACMS_Id === "" || $scope.ismS_Id === "" || $scope.NOP_2 === "" || parseInt($scope.NOP_2) === 0) {
                swal('Please Select Feilds Category,Course,Branch,Semester,Section,Subject And Enter Period (Not Zero) !');
            }
            else {
                var cnnnn = 0;
                angular.forEach($scope.courselist, function (cc) {
                    if (cc.amcO_Id == objcrs) {
                        cors = cc.amcO_CourseName;
                    }
                })

                angular.forEach($scope.branchlist, function (bb) {
                    if (bb.amB_Id == objbr) {
                        bran = bb.amB_BranchName;
                    }
                })

                angular.forEach($scope.semisterlist, function (ss) {
                    if (ss.amsE_Id == objsem) {
                        sem = ss.amsE_SEMName;
                    }
                })
                angular.forEach($scope.sectionlist, function (scc) {
                    if (scc.acmS_Id == objsec) {
                        sect = scc.acmS_SectionName;
                    }
                })
                angular.forEach($scope.subject_list, function (sbb) {
                    if (sbb.ismS_Id == objism) {
                        subj = sbb.ismS_SubjectName;
                    }
                })

                angular.forEach($scope.category_list, function (ctt) {
                    if (ctt.ttmC_Id == objcat) {
                        cat = ctt.ttmC_CategoryName;
                    }
                })
                period = parseInt($scope.NOP_2);

                //Push selected dropdown to temp array
                if ($scope.albumNameArray.length === 0) {

                    $scope.albumNameArray.push({ catDisplay: cat, crsDisplay: cors, brDisplay: bran, semDisplay: sem, sectDisplay: sect, subDisplay: subj, pedDisplay: period, TTMC_Id: $scope.TTMC_Id, AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ismS_Id, NOP: parseInt($scope.NOP_2) });
                    $scope.albumNameArraysaveDB.push({ TTMC_Id: $scope.TTMC_Id, AMCO_Id: $scope.AMCO_Id, AMB_Id:$scope.AMB_Id, AMSE_Id:$scope.AMSE_Id, ACMS_Id:$scope.ACMS_Id, ISMS_Id: $scope.ismS_Id, NOP: parseInt($scope.NOP_2) });
                    pedcount += period;
                    // Clear input fields after push
                    $scope.TTMC_Id = "";
                    $scope.AMCO_Id = "";
                    $scope.AMB_Id = "";
                    $scope.AMSE_Id = "";
                    $scope.ACMS_Id = "";
                    $scope.ismS_Id = "";
                    $scope.NOP_2 = 0;
                }
                else {
                    var condition = 0;

                    angular.forEach($scope.albumNameArraysaveDB, function (ll) {
                        if (ll.TTMC_Id == objcat && ll.AMCO_Id == objcrs && ll.AMB_Id == objbr && ll.AMSE_Id == objsem && ll.ACMS_Id == objsec && ll.ISMS_Id == objism) {
                            condition = 1;
                            swal("Record Already Selected !");

                        }

                    })
                 
                    if (condition === 0) {
                        $scope.albumNameArray.push({ catDisplay: cat, crsDisplay: cors, brDisplay: bran, semDisplay: sem, sectDisplay: sect, subDisplay: subj, pedDisplay: period, TTMC_Id: $scope.TTMC_Id, AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ismS_Id, NOP: parseInt($scope.NOP_2)});
                        $scope.albumNameArraysaveDB.push({ TTMC_Id: $scope.TTMC_Id, AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ismS_Id, NOP: parseInt($scope.NOP_2) });
                        pedcount += period;
                        // Clear input fields after push
                        $scope.TTMC_Id = "";
                        $scope.AMCO_Id = "";
                        $scope.AMB_Id = "";
                        $scope.AMSE_Id = "";
                        $scope.ACMS_Id = "";
                        $scope.ismS_Id = "";
                        $scope.NOP_2 = 0;
                        condition = 1;
                    }
                }
              
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
                    apiService.DeleteURI("CLGPRDDistribution/deletepages", MdeleteId).
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