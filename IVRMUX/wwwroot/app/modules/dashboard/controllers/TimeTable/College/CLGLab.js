
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGLabController', CLGLabController)

    CLGLabController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function CLGLabController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
     $scope.albumNameArray = [];
        $scope.albumNameArraysaveDB = [];
        $scope.editEmployee = {};
        var cors, bran, sem, sect, subj, period, cat;
        var cors1, bran1, sem1, sect1, subj1, period1, cat1;

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

        $scope.get_course = function () {
           // $scope.albumNameArraysaveDB = [];
           // $scope.albumNameArray = [];
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
        $scope.get_subject_onsec = function () {
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
                    //"HRME_Id": $scope.HRME_Id,

                }
                apiService.create("CLGTTCommon/get_subject_onsec", data).
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

        //Ui Grid view rendering data from data base
        $scope.gridOptions1 = {
            enableColumnMenus: false,
            enableFiltering: false,
            enableEditing: false,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'crsDisplay', displayName: 'Course' },
                { name: 'brDisplay', displayName: 'Branch' },
                { name: 'semDisplay', displayName: 'Sem' },
                { name: 'sectDisplay', displayName: 'Section' },
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
                  { name: 'SL NO', enableFiltering: false, field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'asmaY_Year', displayName: 'Academic Year' },
                { name: 'ttmC_CategoryName', displayName: 'Category' },
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
            debugger;
            if ($scope.myForm.$valid) {
                if ($scope.albumNameArraysaveDB.length > 1) {

                    var data = {
                        "TTLAB_Id": $scope.TTLAB_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "TTLAB_LABLIBName": $scope.TTLAB_LABLIBName,
                        "TTMC_Id": $scope.TTMC_Id,
                        "TempararyArrayList": $scope.albumNameArraysaveDB
                    }
                    apiService.create("CLGLab/savedetail", data).
                        then(function (promise) {
                            if (promise.returnval == true) {
                               
                                swal('Data Saved/Updated Successfully');
                                $scope.albumNameArraysaveDB = [];
                                $scope.gridOptions1.data = "";
                                $scope.albumNameArray = [];
                            }
                            else if (promise.returnval == false && promise.returnduplicatestatus == 'Duplicate') {
                                swal('Records Already Exist !');
                            }
                            else {
                                swal('Data Not Saved !');
                            }
                            
                        })
                    $state.reload();
                }
                else {
                    swal('Pease Enter Lab Details !');

                }
            }


        };


        $scope.canceldd = function () {
            $state.reload();

        }

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("CLGLab/getalldetails", pageid).
       then(function (promise) {
           $scope.currentPage = 1;
           $scope.itemsPerPage = 5;
           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylist = promise.catelist;
           $scope.sectionlist = promise.sectionlist;
           
           $scope.gridOptions.data = promise.labdetailsarray;
       })
        };
        //TO clear  data
        $scope.clearid = function () {
            //$scope.TTLAB_LABLIBName = "";
            //$scope.asmaY_Id = "";
            //$scope.ttmC_Id = "";
            //$scope.albumNameArray = [];
            //$scope.albumNameArraysaveDB = [];
            //$scope.gridOptions1.data = $scope.albumNameArraysaveDB;
            //$scope.TTLAB_Id = 0;
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.search = "";
        };

        //TO clear  data
        $scope.clearCategory = function () {
            $scope.submitted = true;
            $scope.ttmC_Id = "";

        };
        //TO  Edit Record
        $scope.getorgvalue = function (employee) {
           // $scope.albumNameArray = [];
           // $scope.albumNameArraysaveDB = [];
            apiService.create("CLGLab/editlab", employee).
            then(function (promise) {
                //$scope.TTLABD_Id = promise.labconsedit[0].ttlabD_Id;
                $scope.ASMAY_Id = promise.labconsedit[0].ASMAY_Id;
                $scope.TTLAB_LABLIBName = promise.labconsedit[0].TTLAB_LABLIBName;
                $scope.TTMC_Id = promise.labconsedit[0].TTMC_Id;
                $scope.TTLAB_Id = promise.labconsedit[0].TTLAB_Id;

                angular.forEach(promise.labconsedit, function (cc) {
                    $scope.albumNameArray.push({ crsDisplay: cc.AMCO_CourseName, brDisplay: cc.AMB_BranchName, semDisplay: cc.AMSE_SEMName, sectDisplay: cc.ACMS_SectionName, subDisplay: cc.ISMS_SubjectName, AMCO_Id: cc.AMCO_Id, AMB_Id: cc.AMB_Id, AMSE_Id: cc.AMSE_Id, ACMS_Id: cc.ACMS_Id, ISMS_Id: cc.ISMS_Id });
                    $scope.albumNameArraysaveDB.push({ AMCO_Id: cc.AMCO_Id, AMB_Id: cc.AMB_Id, AMSE_Id: cc.AMSE_Id, ACMS_Id: cc.ACMS_Id, ISMS_Id: cc.ISMS_Id });

                })

            
                $scope.gridOptions1.data = $scope.albumNameArray;
                $scope.get_course();
               
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
                    apiService.DeleteURI("CLGLab/deletepages", pageid).
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
        $scope.TransferDatagrid = function ( objcrs, objbr, objsem, objsec, objism) {
            if ($scope.ASMAY_Id != "" && $scope.TTLAB_LABLIBName != "" && $scope.TTMC_Id != "") {
                if ( $scope.AMCO_Id === undefined || $scope.AMB_Id === undefined || $scope.AMSE_Id === undefined || $scope.ACMS_Id === undefined || $scope.ISMS_Id === undefined || $scope.TTMC_Id === "" || $scope.AMCO_Id === "" || $scope.AMB_Id === "" || $scope.AMSE_Id === "" || $scope.ACMS_Id === "" || $scope.ISMS_Id === "" ) {
                    swal('Please Select Feilds  Course, Branch, Semester, Section, Subject!');
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
                    angular.forEach($scope.subjectlist, function (sbb) {
                        if (sbb.ismS_Id == objism) {
                            subj = sbb.ismS_SubjectName;
                        }
                    })

                    //Push selected dropdown to temp array
                    if ($scope.albumNameArray.length === 0) {

                        $scope.albumNameArray.push({ crsDisplay: cors, brDisplay: bran, semDisplay: sem, sectDisplay: sect, subDisplay: subj, AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ISMS_Id });
                        $scope.albumNameArraysaveDB.push({ AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ISMS_Id });
                     
                        // Clear input fields after push
                      
                        $scope.AMCO_Id = "";
                        $scope.AMB_Id = "";
                        $scope.AMSE_Id = "";
                        $scope.ACMS_Id = "";
                        $scope.ISMS_Id = "";
                      
                    }
                    else {
                        var condition = 0;

                        angular.forEach($scope.albumNameArraysaveDB, function (ll) {
                            if ( ll.AMCO_Id == objcrs && ll.AMB_Id == objbr && ll.AMSE_Id == objsem && ll.ACMS_Id == objsec && ll.ISMS_Id == objism) {
                                condition = 1;
                                swal("Record Already Selected !");

                            }

                        })

                        if (condition === 0) {
                            $scope.albumNameArray.push({ crsDisplay: cors, brDisplay: bran, semDisplay: sem, sectDisplay: sect, subDisplay: subj, AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ISMS_Id });
                            $scope.albumNameArraysaveDB.push({ AMCO_Id: $scope.AMCO_Id, AMB_Id: $scope.AMB_Id, AMSE_Id: $scope.AMSE_Id, ACMS_Id: $scope.ACMS_Id, ISMS_Id: $scope.ISMS_Id });
                          
                            // Clear input fields after push
                          
                            $scope.AMCO_Id = "";
                            $scope.AMB_Id = "";
                            $scope.AMSE_Id = "";
                            $scope.ACMS_Id = "";
                            $scope.ISMS_Id = "";
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
            
            $scope.combname =employee.ttlaB_LABLIBName;

            apiService.create("CLGLab/viewrecordspopup", employee).
                    then(function (promise) {
                       
                      
                        $scope.viewrecordspopupdisplay = promise.labdetilspopuparray;

                    })

        };

        //TO clear  popupgrid data
        $scope.clearpopupgrid = function () {
            $scope.viewrecordspopupdisplay = [];
        };
        //TO  delete Record Right grid
        $scope.deletedatarightgrid = function (employee) {
            for (var j = $scope.albumNameArraysaveDB.length - 1; j >= 0; j--) {
                if ( $scope.albumNameArraysaveDB[j].AMCO_Id == employee.AMCO_Id && $scope.albumNameArraysaveDB[j].AMB_Id == employee.AMB_Id && $scope.albumNameArraysaveDB[j].AMSE_Id == employee.AMSE_Id && $scope.albumNameArraysaveDB[j].ACMS_Id == employee.ACMS_Id && $scope.albumNameArraysaveDB[j].ISMS_Id == employee.ISMS_Id ) {

                   
                    $scope.albumNameArraysaveDB.splice(j, 1);

                }
            }

            for (var i = $scope.albumNameArray.length - 1; i >= 0; i--) {

                if ( $scope.albumNameArray[i].AMCO_Id == employee.AMCO_Id && $scope.albumNameArray[i].AMB_Id == employee.AMB_Id && $scope.albumNameArray[i].AMSE_Id == employee.AMSE_Id && $scope.albumNameArray[i].ACMS_Id == employee.ACMS_Id && $scope.albumNameArray[i].ISMS_Id == employee.ISMS_Id ) {
                    $scope.albumNameArray.splice(i, 1);

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
                    apiService.create("CLGLab/getclass_catg", data).
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

                apiService.create("CLGLab/deactivate", employee).
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
                apiService.create("CLGLab/get_catg", data).
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