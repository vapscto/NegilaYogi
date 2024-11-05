
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGBifurcationController', CLGBifurcationController)
    CLGBifurcationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function CLGBifurcationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};
        

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

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;

            var pageid = 2;
            apiService.getURI("CLGBifurcation/getalldetails", pageid).
        then(function (promise) {
            $scope.categorylist = promise.categorylist;
            $scope.gridOptions.data = promise.detailslist;
            $scope.acdlist = promise.yearlist;
            $scope.sectionlist = promise.sectionlist;
            $scope.periodlist = promise.periodlist;

        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }
        $scope.get_subject1 = function () {
            
            if ($scope.ASMAY_Id == "" && $scope.TTMC_Id == "" && $scope.ASMCL_Id == "" && $scope.ASMS_Id == "") {
                swal("Please Select The Academic Year,Category,Class And Section !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "" && $scope.ASMCL_Id != "" && $scope.ASMS_Id != "" && $scope.HRME_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "HRME_Id": $scope.HRME_Id,
                }
                apiService.create("Fixing/getsubject_staff", data).
         then(function (promise) {

             $scope.subjectlist = promise.subjectbystaff;

             $scope.ismS_Id1 = "";

             if ($scope.TTFDP_Id != "" && $scope.TTFDP_Id != 0) {
                 angular.forEach($scope.subjectlist, function (role) {
                     
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

        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
         

            columnDefs: [

                { name: 'crssName', displayName: 'Course' },
            { name: 'branchname', displayName: 'Branch' },
            { name: 'semname', displayName: 'Sem' },
            { name: 'secname', displayName: 'Section' },
            { name: 'staffname', displayName: 'Staff' },
                { name: 'subjectname', displayName: 'Subject' },
               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.DelTemp(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 '</div>'
               }
            ]

        };
        //Ui Grid view rendering data from data base
        $scope.gridOptions = {
            enableColumnMenus: false,
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                   { name: 'acdYear', displayName: 'Academic Year' },
              { name: 'categoryName', displayName: 'Category' },
              { name: 'bifricationName', displayName: 'Bifurcation Name' },
            { name: 'periodname', displayName: 'No of Period' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                
                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                  '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal3" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactivatebiff(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactivatebiff(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'


               }
            ]

        };

        //TO  View Record
        $scope.viewrecordspopup = function (employee) {
            $scope.viewrecordspopupdisplay = [];
        
            $scope.combname = employee.ttB_BifurcationName;
            apiService.create("CLGBifurcation/viewrecordspopup", employee).
                    then(function (promise) {
                        $scope.viewrecordspopupdisplay = promise.viewdata;

                    })

        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.arrayText = [];
        $scope.albumNameArray = [];

        $scope.AddTemp = function () {

            if ($scope.AMCO_Id === undefined || $scope.AMB_Id === undefined || $scope.HRME_Id === undefined || $scope.ISMS_Id === undefined || $scope.AMSE_Id === undefined || $scope.ACMS_Id === undefined) {
                swal('Please Select Feilds Course,Branch,Sem,Section,Employee,Subject !');
            }
            else {
                if ($scope.AMCO_Id === "" || $scope.AMB_Id === "" || $scope.HRME_Id === "" || $scope.ISMS_Id === "" || $scope.AMSE_Id === "" || $scope.ACMS_Id === "") {
                    swal('Please Select Feilds Course,Branch,Sem,Section,Employee,Subject !');
                }
                else {
                    var fla = "false";

                    for (var k = 0; k < $scope.arrayText.length; k++) {
                        if ($scope.arrayText[k].AMCO_Id == $scope.AMCO_Id && $scope.arrayText[k].AMB_Id == $scope.AMB_Id && $scope.arrayText[k].HRME_Id == $scope.HRME_Id && $scope.arrayText[k].ISMS_Id == $scope.ISMS_Id && $scope.arrayText[k].AMSE_Id == $scope.AMSE_Id && $scope.arrayText[k].ACMS_Id == $scope.ACMS_Id) {
                            fla = "true";
                        }
                    }

                    if (fla == "false") {
                       

                      

                        var cors = "";
                        var bran = "";
                        var sem = "";
                        var sect = "";
                        var subj = "";
                        var stff = "";

                        angular.forEach($scope.courselist, function (cc) {
                            if (cc.amcO_Id == $scope.AMCO_Id) {
                                cors = cc.amcO_CourseName;
                            }
                        })

                        angular.forEach($scope.branchlist, function (bb) {
                            if (bb.amB_Id == $scope.AMB_Id) {
                                bran = bb.amB_BranchName;
                            }
                        })

                        angular.forEach($scope.semisterlist, function (ss) {
                            if (ss.amsE_Id == $scope.AMSE_Id) {
                                sem = ss.amsE_SEMName;
                            }
                        })
                        angular.forEach($scope.sectionlist, function (scc) {
                            if (scc.acmS_Id == $scope.ACMS_Id) {
                                sect = scc.acmS_SectionName;
                            }
                        })
                        angular.forEach($scope.subjectlist, function (sbb) {
                            if (sbb.ismS_Id == $scope.ISMS_Id) {
                                subj = sbb.ismS_SubjectName;
                            }
                        })

                        angular.forEach($scope.stafflist, function (ctt) {
                            if (ctt.hrmE_Id == $scope.HRME_Id) {
                                stff = ctt.empName;
                            }
                        })

                        var text = {
                            AMCO_Id: $scope.AMCO_Id,
                            AMB_Id: $scope.AMB_Id,
                            HRME_Id: $scope.HRME_Id,
                            ISMS_Id: $scope.ISMS_Id,
                            AMSE_Id: $scope.AMSE_Id,
                            ACMS_Id: $scope.ACMS_Id,
                        };

                        $scope.arrayText.push(text);

                        $scope.albumNameArray.push({
                            crssName: cors,
                            branchname: bran,
                            semname: sem,
                            secname: sect,
                            staffname: stff,
                            subjectname: subj,
                            AMCO_Id: $scope.AMCO_Id,
                            AMB_Id: $scope.AMB_Id,
                            HRME_Id: $scope.HRME_Id,
                            ISMS_Id: $scope.ISMS_Id,
                            AMSE_Id: $scope.AMSE_Id,
                            ACMS_Id: $scope.ACMS_Id,
                        });

                      

                        $scope.gridOptions1.data = $scope.albumNameArray;
                        // Clear input fields after push
                        $scope.AMCO_Id = "";
                        $scope.AMB_Id = "";
                        $scope.HRME_Id = "";
                        $scope.ISMS_Id = "";
                        $scope.AMSE_Id = "";
                        $scope.ACMS_Id = "";
                    }
                    else {
                        swal('Selected Combination is already exist..!');
                    }
                }
            }
        }

        $scope.DelTemp = function (option) {

            for (var i = $scope.arrayText.length - 1; i >= 0; i--) {
                if ($scope.arrayText[i].AMCO_Id == option.AMCO_Id && $scope.arrayText[i].AMB_Id == option.AMB_Id && $scope.arrayText[i].AMSE_Id == option.AMSE_Id && $scope.arrayText[i].ACMS_Id == option.ACMS_Id && $scope.arrayText[i].ISMS_Id == option.ISMS_Id && $scope.arrayText[i].HRME_Id == option.HRME_Id) {
                    $scope.arrayText.splice(i, 1);
                }
            }

            for (var i = $scope.albumNameArray.length - 1; i >= 0; i--) {
                if ($scope.albumNameArray[i].AMCO_Id == option.AMCO_Id && $scope.albumNameArray[i].AMB_Id == option.AMB_Id && $scope.albumNameArray[i].AMSE_Id == option.AMSE_Id && $scope.albumNameArray[i].ACMS_Id == option.ACMS_Id && $scope.albumNameArray[i].ISMS_Id == option.ISMS_Id && $scope.albumNameArray[i].HRME_Id == option.HRME_Id) {
                    $scope.albumNameArray.splice(i, 1);
                }
            }

            $scope.gridOptions1.data = $scope.albumNameArray;

        }

        $scope.cancle = function () {
            $scope.ttB_Id = 0;
            $state.reload();
        };
        $scope.objtype = {};
       
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            debugger;
            if ($scope.myForm.$valid) {

                if ($scope.ttba_checkbox === false || $scope.ttba_checkbox === undefined) {
                    $scope.ttba_checkbox = "0";
                    $scope.tt_Select = "0";
                    $scope.TTMP_Id = "0";
                }
                else if ($scope.ttba_checkbox === true) {
                    $scope.ttba_checkbox = "1";
                }

                if ($scope.ttnp_checkbox === false || $scope.ttnp_checkbox === undefined) {
                    $scope.ttnp_checkbox = "0";
                    $scope.txtnoofconc = "0";
                    $scope.txtnoofday = "0";
                }
                else if ($scope.ttnp_checkbox === true) {
                    $scope.ttnp_checkbox = "1";
                }

                var flag = "false";

                if ($scope.ttba_checkbox === "1") {
                    if ($scope.tt_Select === undefined) {
                        flag = "true";
                        swal('Please Select Bifurcation Before / After Condition.. !');
                    }
                    else if ($scope.TTMP_Id === undefined) {
                        flag = "true";
                        swal('Please Select Before / After Period !');
                    }
                }

                if ($scope.ttnp_checkbox === "1") {
                    if ($scope.txtnoofconc === undefined) {
                        flag = "true";
                        swal('Please Enter No of Consecutive Period !');
                    }
                    else if ($scope.txtnoofday === undefined) {
                        flag = "true";
                        swal('Please Enter No of Consecutive Day !');
                    }
                }
                if ($scope.arrayText.length === 0) {
                    flag = "true";
                    swal('Please Enter at least one Class,section,staff and subject Combination !');
                }

                if (flag === "false") {
                    var data = {
                        "TTB_ID": $scope.ttB_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "TTMC_Id": $scope.TTMC_Id,
                        "TTB_BifurcationName": $scope.tt_Bifurcation,
                        "TTB_NoOfPeriods": $scope.NoPer,
                        "TTB_BefAftApplFlag": $scope.ttba_checkbox,
                        "TTB_BefAftFalg": $scope.tt_Select,
                        "TTMP_Id": Number($scope.TTMP_Id),
                        "TTB_ConsecutiveFlag": $scope.ttnp_checkbox,
                        "TTB_NoOfConPeriods": $scope.txtnoofconc,
                        "TTB_NoOfConDays": $scope.txtnoofday,
                        "combinationlist": $scope.arrayText
                    }
                    apiService.create("CLGBifurcation/savedetail", data).
                        then(function (promise) {
                            if (promise.returnMsg === "Add") {
                                //$scope.gridOptions.data = promise.detailslist;
                                swal('Data successfully Saved');
                            }
                            else if (promise.returnMsg === "update") {
                                $scope.gridOptions.data = promise.detailslist;
                                swal('Data successfully Updated');
                            }
                            else if (promise.returnduplicatestatus === 'Duplicate') {
                                swal('Records Already Exist !');
                            }
                            else {
                                swal('Data Not Saved !');
                            }

                            $scope.loaddata();
                            $state.reload();

                        })
                }

            }

        };

        $scope.get_course = function () {

            $scope.AMB_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';
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

        $scope.get_staff = function () {
            $scope.ISMS_Id = '';
            $scope.HRME_Id = '';
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                   
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
            $scope.ISMS_Id = '';
            if ($scope.ASMAY_Id === "") {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "HRME_Id": $scope.HRME_Id,

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
        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';
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
          
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';
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

                    $scope.sectionlist = promise.section_list;

                    if (promise.section_list == "" || promise.section_list == null) {
                        swal("No Section Are Mapped To Selected Course/Branch");
                    }
                })
        };


        $scope.yearchange = function () {
            $scope.AMB_Id = '';
            $scope.TTMC_Id = '';
            $scope.AMCO_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.HRME_Id = '';
            $scope.ISMS_Id = '';

        }

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
       
            apiService.create("CLGBifurcation/editbiff/", employee).
            then(function (promise) {
                $scope.arrayText = [];
                $scope.albumNameArray = [];
               // $scope.gridOptions1.data = promise.editdetailslist;
                $scope.ttB_Id = promise.editdetailslist[0].ttB_Id
             
                angular.forEach(promise.editdetailslist, function (ff) {

                   

                    $scope.arrayText.push({
                        AMCO_Id: ff.AMCO_Id,
                        AMB_Id: ff.AMB_Id,
                        HRME_Id: ff.HRME_Id,
                        ISMS_Id: ff.ISMS_Id,
                        AMSE_Id: ff.AMSE_Id,
                        ACMS_Id: ff.ACMS_Id,
                    });

                    $scope.albumNameArray.push({
                        crssName: ff.AMCO_CourseName,
                        branchname: ff.AMB_BranchName,
                        semname: ff.AMSE_SEMName,
                        secname: ff.ACMS_SectionName,
                        staffname: ff.ENAME,
                        subjectname: ff.ISMS_SubjectName,
                        AMCO_Id: ff.AMCO_Id,
                        AMB_Id: ff.AMB_Id,
                        HRME_Id: ff.HRME_Id,
                        ISMS_Id: ff.ISMS_Id,
                        AMSE_Id: ff.AMSE_Id,
                        ACMS_Id: ff.ACMS_Id,
                    });

                })

                $scope.ASMAY_Id = promise.editdetailslist[0].asmaY_Id;
                $scope.TTMC_Id = promise.editdetailslist[0].ttmC_Id;
                $scope.tt_Bifurcation = promise.editdetailslist[0].ttB_BifurcationName;
                $scope.NoPer = promise.editdetailslist[0].ttB_NoOfPeriods;

                if (promise.editdetailslist[0].ttB_BefAftApplFlag === 1) {
                    $scope.ttba_checkbox = true;
                    $scope.tt_Select = promise.editdetailslist[0].ttB_BefAftFalg;
                    $scope.TTMP_Id = promise.editdetailslist[0].ttmP_Id;
                }
                else if (promise.editdetailslist[0].ttB_BefAftApplFlag === 0) {
                    $scope.ttba_checkbox = false;
                    $scope.tt_Select = promise.editdetailslist[0].ttB_BefAftFalg;
                    $scope.TTMP_Id = promise.editdetailslist[0].ttmP_Id;
                }

                if (promise.editdetailslist[0].ttB_ConsecutiveFlag === 1) {
                    $scope.ttnp_checkbox = true;
                    $scope.txtnoofconc = promise.editdetailslist[0].ttB_NoOfConPeriods;
                    $scope.txtnoofday = promise.editdetailslist[0].ttB_NoOfConDays;
                }
                else if (promise.editdetailslist[0].ttB_ConsecutiveFlag === 0) {
                    $scope.ttnp_checkbox = false;
                    $scope.txtnoofconc = promise.editdetailslist[0].ttB_NoOfConPeriods;
                    $scope.txtnoofday = promise.editdetailslist[0].ttB_NoOfConDays;
                }
                $scope.get_course();
                $scope.gridOptions1.data = $scope.albumNameArray;
            })
        }
        //TO  delete Record
        $scope.deactivatebiff = function (employee) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttB_ActiveFlag === true) {
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
                    apiService.create("CLGBifurcation/deactivatebiff", employee).
                        then(function (promise) {
                            if (promise.returnval === true) {
                            swal('Record ' + confirmmgs + ' Successfully');
                        }
                        else {
                            swal('Record ' + confirmmgs + ' Successfully!');
                        }
                        //$scope.loaddata();
                        $state.reload();
                    })
                }
                else {
                    swal("Record " + mgs + " Cancelled");
                }
            });
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.get_staff1 = function () {
            if ($scope.ASMAY_Id == "" && $scope.TTMC_Id == "" && $scope.ASMCL_Id == "") {
                swal("Please Select The Academic Year And Category And Class !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.TTMC_Id != "" && $scope.ASMCL_Id != "" && $scope.ASMS_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                }
                
                apiService.create("Fixing/getstaff_section", data).
         then(function (promise) {

             if (promise.staffbyall == "" || promise.staffbyall == null) {
                 swal("No Staff Are Allocated To Selected Class and Section");
                 $scope.stafflist = "";
                 $scope.HRME_Id = "";
             }
             else {
                 $scope.stafflist = promise.staffbyall;
                 $scope.HRME_Id = "";
             }
         })
            }
        }
    }

})();