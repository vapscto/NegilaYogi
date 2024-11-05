
(function () {
    'use strict';
    angular
.module('app')
.controller('BifurcationConstraintsController', BifurcationConstraintsController)


    BifurcationConstraintsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams']
    function BifurcationConstraintsController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams) {
        $scope.editEmployee = {};


        // Time picker starts
        //$scope.timedis = true;
        //$scope.ScheduleTime = new Date();

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
            apiService.getURI("Bifurcation/getalldetails", pageid).
        then(function (promise) {
            $scope.categorylist = promise.categorylist;
            $scope.classlist = promise.classlist;
            $scope.gridOptions.data = promise.detailslist;
            $scope.acdlist = promise.acdlist;
            $scope.sectionlist = promise.sectionlist;
            $scope.subjectlist = promise.subjectlist;
            $scope.stafflist = promise.stafflist;
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

        { name: 'className', displayName: 'Class' },
    { name: 'sectioname', displayName: 'Section' },
{ name: 'staffname', displayName: 'Staff' },
             { name: 'subjectname', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
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
                 //'<div class="grid-action-cell">' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>' +
                 //'<a href="javascript:void(0)" ng-click="grid.appScope.deletedata(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
                 //'</div>'

                    '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                  '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal3" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deletedata(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deletedata(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'


               }
            ]

        };

        //TO  View Record
        $scope.viewrecordspopup = function (employee) {
            
            $scope.editEmployee = employee.ttB_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("Bifurcation/getalldetailsviewrecords", pageid).
                    then(function (promise) {
                        $scope.combname = promise.viewdata[0].ttB_BifurcationName;
                        $scope.viewrecordspopupdisplay = promise.viewdata;

                    })

        };

        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };

        $scope.arrayText = [];
        $scope.albumNameArray = [];

        $scope.AddTemp = function () {

            if ($scope.ASMCL_Id === undefined || $scope.ASMS_Id === undefined || $scope.HRME_Id === undefined || $scope.ISMS_Id === undefined) {
                swal('Please Select Feilds Class,Section,Employee,Subject !');
            }
            else {
                if ($scope.ASMCL_Id === "" || $scope.ASMS_Id === "" || $scope.HRME_Id === "" || $scope.ISMS_Id === "") {
                    swal('Please Select Feilds Class,Section,Employee,Subject !');
                }
                else {
                    var fla = "false";

                    for (var k = 0; k < $scope.arrayText.length; k++) {
                        if ($scope.arrayText[k].ASMCL_Id == $scope.ASMCL_Id && $scope.arrayText[k].ASMS_Id == $scope.ASMS_Id && $scope.arrayText[k].HRME_Id == $scope.HRME_Id && $scope.arrayText[k].ISMS_Id == $scope.ISMS_Id) {
                            fla = "true";
                        }
                    }

                    if (fla == "false") {
                        var text = {
                            ASMCL_Id: $scope.ASMCL_Id,
                            ASMS_Id: $scope.ASMS_Id,
                            HRME_Id: $scope.HRME_Id,
                            ISMS_Id: $scope.ISMS_Id
                        };

                        $scope.arrayText.push(text);

                        var cls = "";
                        var sect = "";
                        var staff = "";
                        var subj = "";

                        for (var k = 0; k < $scope.classlist.length; k++) {
                            if ($scope.classlist[k].asmcL_Id == $scope.ASMCL_Id) {
                                cls = $scope.classlist[k].asmcL_ClassName;
                            }
                        }
                        for (var k = 0; k < $scope.sectionlist.length; k++) {
                            if ($scope.sectionlist[k].asmS_Id == $scope.ASMS_Id) {
                                sect = $scope.sectionlist[k].asmC_SectionName;
                            }
                        }
                        for (var k = 0; k < $scope.stafflist.length; k++) {
                            if ($scope.stafflist[k].hrmE_Id == $scope.HRME_Id) {
                                staff = $scope.stafflist[k].staffName;
                            }
                        }
                        for (var k = 0; k < $scope.subjectlist.length; k++) {
                            if ($scope.subjectlist[k].ismS_Id == $scope.ISMS_Id) {
                                subj = $scope.subjectlist[k].ismS_SubjectName;
                            }
                        }
                        $scope.albumNameArray.push({
                            className: cls,
                            sectioname: sect,
                            staffname: staff,
                            subjectname: subj,
                        });

                        $scope.gridOptions1.data = $scope.albumNameArray;
                        // Clear input fields after push
                        $scope.ASMCL_Id = "";
                        $scope.ASMS_Id = "";
                        $scope.HRME_Id = "";
                        $scope.ISMS_Id = "";
                    }
                    else {
                        swal('Selected Combination is already exist..!');
                    }
                }
            }
        }

        $scope.DelTemp = function (option) {
            var cls = "";
            var sect = "";
            var staff = "";
            var subj = "";

            $scope.albumNamedelArray = [];

            for (var k = 0; k < $scope.classlist.length; k++) {
                if ($scope.classlist[k].asmcL_ClassName == option.className) {
                    cls = $scope.classlist[k].asmcL_Id;
                }
            }
            for (var k = 0; k < $scope.sectionlist.length; k++) {
                if ($scope.sectionlist[k].asmC_SectionName == option.sectioname) {
                    sect = $scope.sectionlist[k].asmS_Id;
                }
            }
            for (var k = 0; k < $scope.stafflist.length; k++) {
                if ($scope.stafflist[k].staffName == option.staffname) {
                    staff = $scope.stafflist[k].hrmE_Id;
                }
            }
            for (var k = 0; k < $scope.subjectlist.length; k++) {
                if ($scope.subjectlist[k].ismS_SubjectName == option.subjectname) {
                    subj = $scope.subjectlist[k].ismS_Id;
                }
            }
            $scope.albumNamedelArray.push({

                ISMS_Id: subj,
                ASMCL_Id: cls,
                ASMC_Id: sect,
                HRME_Id: staff
            });


            for (var i = $scope.arrayText.length - 1; i >= 0; i--) {
                if ($scope.arrayText[i].ASMCL_Id == cls && $scope.arrayText[i].ASMS_Id == sect && $scope.arrayText[i].ISMS_Id == subj && $scope.arrayText[i].HRME_Id == staff) {
                    $scope.arrayText.splice(i, 1);
                }
            }

            for (var i = $scope.albumNameArray.length - 1; i >= 0; i--) {
                if ($scope.albumNameArray[i].className == option.className && $scope.albumNameArray[i].sectioname == option.sectioname && $scope.albumNameArray[i].staffname == option.staffname && $scope.albumNameArray[i].subjectname == option.subjectname) {
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
                    apiService.create("Bifurcation/savedetail", data).
                        then(function (promise) {
                            if (promise.returnMsg === "Add") {
                                $scope.gridOptions.data = promise.detailslist;
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

        $scope.onSelectGetclass = function () {


            if ($scope.ASMAY_Id === undefined) {

                swal('Please Select Academic Year.. !');

            }
            else {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id
                }
                apiService.create("Bifurcation/getClassdetails", data).
                        then(function (promise) {

                            $scope.classlist = promise.classlist;
                        })
            }

        };

        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Bifurcation/getdetails/", pageid).
            then(function (promise) {
                $scope.stafflist = promise.stafflist
                $scope.gridOptions1.data = promise.editdetailslist;
                $scope.ttB_Id = promise.detailslist[0].ttB_Id
                for (var q = 0; q < promise.editdetailslist.length; q++) {

                    $scope.albumNameArray.push({
                        className: promise.editdetailslist[q].className,
                        sectioname: promise.editdetailslist[q].sectioname,
                        staffname: promise.editdetailslist[q].staffname,
                        subjectname: promise.editdetailslist[q].subjectname,
                    });


                    var cls = "";
                    var sect = "";
                    var staff = "";
                    var subj = "";

                    for (var k = 0; k < $scope.classlist.length; k++) {
                        if ($scope.classlist[k].asmcL_ClassName == promise.editdetailslist[q].className) {
                            cls = $scope.classlist[k].asmcL_Id;
                        }
                    }
                    for (var k = 0; k < $scope.sectionlist.length; k++) {
                        if ($scope.sectionlist[k].asmC_SectionName == promise.editdetailslist[q].sectioname) {
                            sect = $scope.sectionlist[k].asmS_Id;
                        }
                    }
                    for (var k = 0; k < $scope.stafflist.length; k++) {
                        if ($scope.stafflist[k].staffName == promise.editdetailslist[q].staffname) {
                            staff = $scope.stafflist[k].hrmE_Id;
                        }
                    }
                    for (var k = 0; k < $scope.subjectlist.length; k++) {
                        if ($scope.subjectlist[k].ismS_SubjectName == promise.editdetailslist[q].subjectname) {
                            subj = $scope.subjectlist[k].ismS_Id;
                        }
                    }
                    $scope.arrayText.push({
                        ASMCL_Id: cls,
                        ASMS_Id: sect,
                        HRME_Id: staff,
                        ISMS_Id: subj,
                    });

                }

                $scope.ASMAY_Id = promise.detailslist[0].asmaY_Id;
                $scope.TTMC_Id = promise.detailslist[0].ttmC_Id;
                $scope.tt_Bifurcation = promise.detailslist[0].ttB_BifurcationName;
                $scope.NoPer = promise.detailslist[0].ttB_NoOfPeriods;

                if (promise.detailslist[0].ttB_BefAftApplFlag === 1) {
                    $scope.ttba_checkbox = true;
                    $scope.tt_Select = promise.detailslist[0].ttB_BefAftFalg;
                    $scope.TTMP_Id = promise.detailslist[0].ttmP_Id;
                }
                else if (promise.detailslist[0].ttB_BefAftApplFlag === 0) {
                    $scope.ttba_checkbox = false;
                    $scope.tt_Select = promise.detailslist[0].ttB_BefAftFalg;
                    $scope.TTMP_Id = promise.detailslist[0].ttmP_Id;
                }

                if (promise.detailslist[0].ttB_ConsecutiveFlag === 1) {
                    $scope.ttnp_checkbox = true;
                    $scope.txtnoofconc = promise.detailslist[0].ttB_NoOfConPeriods;
                    $scope.txtnoofday = promise.detailslist[0].ttB_NoOfConDays;
                }
                else if (promise.detailslist[0].ttB_ConsecutiveFlag === 0) {
                    $scope.ttnp_checkbox = false;
                    $scope.txtnoofconc = promise.detailslist[0].ttB_NoOfConPeriods;
                    $scope.txtnoofday = promise.detailslist[0].ttB_NoOfConDays;
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id
                }
                apiService.create("Bifurcation/getClassdetails", data).
                        then(function (promise) {

                            $scope.classlist = promise.classlist;
                        })

            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee) {
            $scope.editEmployee = employee.ttB_Id;
            var pageid = $scope.editEmployee;
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
                    apiService.DeleteURI("Bifurcation/deletedetails", pageid).
                    then(function (promise) {
                        if (promise.returnMsg === "true") {
                            swal('Record ' + confirmmgs + ' Successfully');
                        }
                        else {
                            swal('Record ' + confirmmgs + ' Successfully!');
                        }
                        $scope.loaddata();
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