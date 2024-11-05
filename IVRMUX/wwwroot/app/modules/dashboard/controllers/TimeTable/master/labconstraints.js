
(function () {
    'use strict';
    angular
.module('app')
.controller('LabconstraintsController', LabconstraintsController)

    LabconstraintsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function LabconstraintsController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.albumNameArray = [];
        $scope.albumNameArraysaveDB = [];
        $scope.editEmployee = {};
        var cls, sect, staff, subj;
        var cls1, sect1, staff1, subj1;

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
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [


              { name: 'clsDisplay', displayName: 'Class' },
            { name: 'sectDisplay', displayName: 'Section' },
           // { name: 'stafDisplay', displayName: 'Staff' },
             { name: 'subDisplay', displayName: 'Subject' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableSorting: false, cellTemplate:
                 '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.deletedatarightgrid(row.entity);"> <i class="fa fa-trash text-danger"></i></a>' +
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
                  { name: 'SNO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
              { name: 'asmayYear', displayName: 'Academic Year' },
            { name: 'categoryName', displayName: 'Category Name' },
            { name: 'ttlaB_LABLIBName', displayName: 'LAB Name' },

               {
                   field: 'id', name: '',
                   displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                     '<div class="grid-action-cell">' +
                 '<a href="javascript:void(0)" ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +
                  '<a href="javascript:void(0)" data-toggle="modal" data-target="#myModal3" data-backdrop="static" ng-click="grid.appScope.viewrecordspopup(row.entity);"> <i class="fa fa-eye text-purple" ></i></a>  &nbsp; &nbsp;' +
                '<a ng-if="row.entity.ttlaB_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
              '<span ng-if="row.entity.ttlaB_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                '</div>'
               }
            ]

        };


        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                if ($scope.albumNameArraysaveDB.length > 1) {

                    var data = {
                        "TTLAB_Id": $scope.TTLAB_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "TTLAB_LABLIBName": $scope.TTLAB_LABLIBName,
                        "TTMC_Id": $scope.ttmC_Id,
                        "TempararyArrayList": $scope.albumNameArraysaveDB
                    }
                    apiService.create("Labconstraints/savedetail", data).
                        then(function (promise) {
                            if (promise.returnval == true) {
                                $scope.albumNameArraysaveDB = "";
                                $scope.gridOptions1.data = "";
                                $scope.albumNameArray = "";
                                swal('Data successfully Saved');
                            }
                            else if (promise.returnval == false && promise.returnduplicatestatus == 'Duplicate') {
                                swal('Records Already Exist !');
                            }
                            else {
                                swal('Data Not Saved !');
                            }
                            $scope.BindData();
                        })
                    $scope.BindData();
                    $scope.clearid();
                }
                else {
                    swal('Pease Enter Lab Details !');

                }
            }


        };


        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("Labconstraints/getalldetails").
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.arrlist2 = promise.classDrpDwn;
           $scope.section = promise.sectDrpDwn;
           $scope.stflst = promise.staffDrpDwn;
           $scope.sublst = promise.subjDrpDwn;
           $scope.gridOptions.data = promise.labdetailsarray;
       })
        };
        //TO clear  data
        $scope.clearid = function () {
            $scope.TTLAB_LABLIBName = "";
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.albumNameArray = [];
            $scope.albumNameArraysaveDB = [];
            $scope.gridOptions1.data = $scope.albumNameArraysaveDB;
            $scope.TTLAB_Id = 0;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        //TO clear  data
        $scope.clearCategory = function () {
            $scope.submitted = true;
            $scope.ttmC_Id = "";

        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ttlaB_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("Labconstraints/getdetails", pageid).
            then(function (promise) {
                $scope.TTLABD_Id = promise.labconsedit[0].ttlabD_Id;
                $scope.asmaY_Id = promise.labconsedit[0].asmaY_Id;
                $scope.TTLAB_LABLIBName = promise.labconsedit[0].ttlaB_LABLIBName;
                $scope.ttmC_Id = promise.labconsedit[0].ttmC_Id;
                $scope.TTLAB_Id = promise.labconsedit[0].ttlaB_Id;
                for (var i = 0; i < promise.labconsdetailsedit.length; i++) {
                    $scope.albumNameArraysaveDB.push(promise.labconsdetailsedit[i]);

                    for (var k = 0; k < $scope.arrlist2.length; k++) {
                        if ($scope.arrlist2[k].asmcL_Id === promise.labconsdetailsedit[i].asmcL_Id) {
                            cls = $scope.arrlist2[k].asmcL_ClassName;
                        }
                    }
                    for (var j = 0; j < $scope.section.length; j++) {
                        if ($scope.section[j].asmS_Id === promise.labconsdetailsedit[i].asmS_Id) {
                            sect = $scope.section[j].asmC_SectionName;
                        }
                    }
                    for (var m = 0; m < $scope.sublst.length; m++) {
                        if ($scope.sublst[m].ismS_Id === promise.labconsdetailsedit[i].ismS_Id) {
                            subj = $scope.sublst[m].ismS_SubjectName;
                        }
                    }
                    $scope.albumNameArray.push({
                        clsDisplay: cls,
                        sectDisplay: sect,
                        subDisplay: subj,
                    });
                }
                //$scope.albumNameArraysaveDB.push({
                //    ASMCL_Id: $scope.asmcL_Id,
                //    asmS_Id: $scope.asmS_Id,
                //    HRME_Id: $scope.ivrmstauL_Id,
                //    IMS_Id: $scope.amsU_Id,
                //});   
                
                $scope.gridOptions1.data = $scope.albumNameArray;
                $scope.deletedata();
            })
        }
        //TO  delete Record
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.ttlaB_Id;
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
                    apiService.DeleteURI("Labconstraints/deletepages", pageid).
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


        //Transfer LeftDrps to Right grid
        $scope.TransferDatagrid = function (objcls, objsect, objsub) {
            if ($scope.asmaY_Id != "" && $scope.TTLAB_LABLIBName != "" && $scope.ttmC_Id != "") {
                if ($scope.asmcL_Id === "" || $scope.asmS_Id === "" || $scope.ismS_Id === "") {
                    swal('Please Select Feilds Class,Section,Subject !');
                }
                else {
                    for (var k = 0; k < $scope.arrlist2.length; k++) {
                        if ($scope.arrlist2[k].asmcL_Id == objcls) {
                            cls = $scope.arrlist2[k].asmcL_ClassName;
                        }
                    }
                    for (var j = 0; j < $scope.section.length; j++) {
                        if ($scope.section[j].asmS_Id == objsect) {
                            sect = $scope.section[j].asmC_SectionName;
                        }
                    }

                    for (var m = 0; m < $scope.sublst.length; m++) {
                        if ($scope.sublst[m].ismS_Id == objsub) {
                            subj = $scope.sublst[m].ismS_SubjectName;
                        }
                    }

                    if ($scope.albumNameArray.length == 0) {
                        $scope.albumNameArray.push({ clsDisplay: cls, sectDisplay: sect, subDisplay: subj });
                        $scope.albumNameArraysaveDB.push({ ASMCL_Id: $scope.asmcL_Id, ASMS_Id: $scope.asmS_Id, ISMS_Id: $scope.ismS_Id });
                        // Clear input fields after push
                        $scope.asmcL_Id = "";
                        $scope.asmS_Id = "";
                        $scope.ismS_Id = "";
                    }
                    else {
                        var condition = 0;
                        for (var b = 0; b < $scope.albumNameArray.length; b++) {
                            if ($scope.albumNameArray[b].clsDisplay == cls && $scope.albumNameArray[b].sectDisplay == sect && $scope.albumNameArray[b].subDisplay == subj) {
                                condition = 1;
                                swal("Record Already Selected !");
                            }
                        }
                        if (condition == 0) {
                            $scope.albumNameArray.push({ clsDisplay: cls, sectDisplay: sect, subDisplay: subj });
                            $scope.albumNameArraysaveDB.push({ ASMCL_Id: $scope.asmcL_Id, ASMS_Id: $scope.asmS_Id, ISMS_Id: $scope.ismS_Id });
                            // Clear input fields after push
                            $scope.asmcL_Id = "";
                            $scope.asmS_Id = "";
                            $scope.ismS_Id = "";
                            condition = 1;
                        }
                    }
                    $scope.gridOptions1.data = $scope.albumNameArray;
                }
            }
            else {
                swal("Please Enter Combination Name & Academic year & Category !!!");
            }
        };

        //TO  View Record
        $scope.viewrecordspopup = function (employee, SweetAlert) {
            
            $scope.editEmployee = employee.ttlaB_Id;
            var pageid = $scope.editEmployee;

            apiService.getURI("Labconstraints/getalldetailsviewrecords", pageid).
                    then(function (promise) {
                        $scope.combname = promise.labdetilspopuparray[0].ttlaB_LABLIBName;
                        //  $scope.gridOptionspopup.data == promise.labdetilspopuparray;
                        $scope.viewrecordspopupdisplay = promise.labdetilspopuparray;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = "";
        };
        //TO  delete Record Right grid
        $scope.deletedatarightgrid = function (employee) {
            var pageid1 = employee.clsDisplay;
            var pageid2 = employee.sectDisplay;
            var pageid3 = employee.stafDisplay;
            var pageid4 = employee.subDisplay;

            for (var k = 0; k < $scope.arrlist2.length; k++) {
                if ($scope.arrlist2[k].asmcL_ClassName == pageid1) {
                    cls1 = $scope.arrlist2[k].asmcL_Id;
                }
            }
            for (var j = 0; j < $scope.section.length; j++) {
                if ($scope.section[j].asmC_SectionName == pageid2) {
                    sect1 = $scope.section[j].asmS_Id;
                }
            }
            for (var m = 0; m < $scope.sublst.length; m++) {
                if ($scope.sublst[m].ismS_SubjectName == pageid4) {
                    subj1 = $scope.sublst[m].ismS_Id;
                }
            }
            for (var i = $scope.albumNameArraysaveDB.length - 1; i >= 0; i--) {
                if ($scope.albumNameArraysaveDB[i].asmcL_Id == cls1 && $scope.albumNameArraysaveDB[i].asmS_Id == sect1 && $scope.albumNameArraysaveDB[i].ismS_Id == subj1) {
                    $scope.albumNameArraysaveDB.splice(i, 1);
                }
            }

            for (var x = $scope.albumNameArray.length - 1; x >= 0; x--) {
                if ($scope.albumNameArray[x].clsDisplay == pageid1 && $scope.albumNameArray[x].sectDisplay == pageid2 && $scope.albumNameArray[x].stafDisplay == pageid3 && $scope.albumNameArray[x].subDisplay == pageid4) {
                    $scope.albumNameArray.splice(x, 1);
                }
            }
            $scope.gridOptions1.data = $scope.albumNameArray;
        };
        $scope.get_class = function () {

            if ($scope.asmaY_Id == "") {
                swal("Please Select the Academic Year !");
            }
            else {

                if ($scope.ttmC_Id == "" || $scope.ttmC_Id == undefined) {
                    // swal("Please Select the Academic Year !");
                }
                else {
                    var data = {
                        "TTMC_Id": $scope.ttmC_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                    }
                    apiService.create("Labconstraints/getclass_catg", data).
             then(function (promise) {

                 $scope.arrlist2 = promise.classbycategory;
                 if (promise.classbycategory == "" || promise.classbycategory == null) {
                     swal("No classes Are Mapped To Selected Category");
                 }
                 $scope.asmcL_Id = "";
                 $scope.asmS_Id = "";
                 $scope.ismS_Id = "";
             })
                }
            }
        };

        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttlaB_ActiveFlag === true) {
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

                apiService.create("Labconstraints/deactivate", employee).
                then(function (promise) {
                    if (promise.returnval === true) {
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



        $scope.get_category = function () {

            if ($scope.asmaY_Id == "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("Labconstraints/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             if (promise.catelist == "" || promise.catelist == null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })

            }


        };


    }
})();