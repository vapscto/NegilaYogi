/// <reference path="../../views/fees/installmentreport.html" />
(function () {
    'use strict';
    angular
.module('app')
.controller('YearlyFeeGroupMappingController', YearlyFeeGroupMappingController)

    YearlyFeeGroupMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function YearlyFeeGroupMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams) {

        //$scope.totalgrid = [
        //   {
        //       'FMH_Id': totalgrid.FMH_Id,
        //       'FMI_Id': totalgrid.FMI_Id,
        //       'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
        //       'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
        //       'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
        //   }];

        $scope.maingrid = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.savedisable = true;
     
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.saveflg = true;
                    $scope.savebtn = true;

                }
                else {
                    $scope.saveflg = false;
                    $scope.savebtn = false;
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
                }


            }
        }

        $scope.sortKey = "fyghM_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchthird = "";
        $scope.remflg = false;

        $scope.addnewbtn = true;

        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            var pageid = 1;
            apiService.getURI("YearlyFeeGroupMapping/getalldetails", pageid).
        then(function (promise) {

            $scope.yearlst = promise.academicdrp
            $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;
            $scope.groupcount = promise.fillmastergroup;
            $scope.companycount = promise.fillcompany;
            $scope.headcount = promise.fillmasterhead;
            $scope.installmentcount = promise.fillinstallment;
            $scope.totalgrid = promise.fillcompany;
            $scope.thirdgrid = promise.alldata;

            $scope.totcountfirst = $scope.thirdgrid.length;

        })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        //$scope.addNew = function (totalgrid) {
        //    $scope.totalgrid.push({
        //        'FMH_Id': totalgrid.FMH_Id,
        //        'FMI_Id': totalgrid.FMI_Id,
        //        'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
        //        'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
        //        'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
        //    });
        //    $scope.PD = {};
        //};

        $scope.onselectacade = function () {

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("YearlyFeeGroupMapping/selectacademic", data).
        then(function (promise) {
            if (promise.fillmastergroup.length > 0)
            {
                $scope.maingrid = true;
                $scope.groupcount = promise.fillmastergroup;

                $scope.thirdgrid = promise.alldata;
                $scope.totcountfirst = $scope.thirdgrid.length;

            }
            else
            {
                $scope.groupcount = [];
                swal("No Groups are mapped for selected Academic Year")
            }
            
        })
        }

        $scope.totalgridtest = [];
        $scope.addNew = function (totalgrid) {
            $scope.totalgrid.push({
                'FMH_Id': totalgrid.FMH_Id,
                'FMI_Id': totalgrid.FMI_Id,
                'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
                'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
                'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
            });
            $scope.totalgridtest.push({
                'FMH_Id': totalgrid.FMH_Id,
                'FMI_Id': totalgrid.FMI_Id,
                'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
                'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
                'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
            });
            if ($scope.totalgridtest.length > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }
            $scope.PD = {};
        };
        $scope.removerow = function (totalgrid) {
            $scope.totalgrid.pop({
                'FMH_Id': totalgrid.FMH_Id,
                'FMI_Id': totalgrid.FMI_Id,
                'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
                'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
                'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
            });
            $scope.totalgridtest.pop({
                'FMH_Id': totalgrid.FMH_Id,
                'FMI_Id': totalgrid.FMI_Id,
                'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
                'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
                'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
            });
            if ($scope.totalgridtest.length > 0) {
                $scope.remflg = true;
            } else {
                $scope.remflg = false;
            }
            // $scope.PD = {};
        };


        $scope.remove = function (totalgrid) {
            var newDataList = [];
            $scope.selectedAll = false;
            angular.forEach($scope.totalgrid, function (selected) {
                if (!selected.selected) {
                    newDataList.push(selected);
                }
            });
            $scope.totalgrid = newDataList;
        };

        $scope.onselectgroup = function (groupcount, emptyrow) {
            
            if ($scope.FMG_Id == "") {
                swal("Please Select Any Group !!!");
                $scope.grigview1 = false;
                $scope.totalgrid = [];
            }
            else {

                var groupid = $scope.FMG_Id;

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "FMG_Id" : groupid
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("YearlyFeeGroupMapping/getadetailsongroup", data).
              then(function (promise) {
                  $scope.grigview1 = true;
                  $scope.totalgrid = promise.alldata;

                  if ($scope.totalgrid.length == 0) {
                      $scope.totalgrid.push({
                          'FMH_Id': emptyrow.FMH_Id,
                          'FMI_Id': emptyrow.FMI_Id,
                          'FYGHM_FineApplicableFlag': emptyrow.FYGHM_FineApplicableFlag,
                          'FYGHM_Common_AmountFlag': emptyrow.FYGHM_Common_AmountFlag,
                          'FYGHM_ActiveFlag': emptyrow.FYGHM_ActiveFlag,
                      });
                      $scope.PD = {};
                  }
                  //$scope.totalgrid = [{ fmH_Id: 1, fmH_FeeName: "John Doe" },
                  //                    { fmH_Id: 2, fmH_FeeName: "Jane Doe" }]


              })
            }
        };


        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;

            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
           function (isConfirm) {
               if (isConfirm) {
                   apiService.DeleteURI("YearlyFeeGroupMapping/Deletedetails", orgid).
                   then(function (promise) {

                       $scope.thirdgrid = promise.alldata;
                       $scope.grigview1 = false;

                       if (promise.returnval == true) {

                           $scope.masterse = promise.masterSectionData;

                           swal('Record Deleted Successfully');
                       }
                       else {
                           swal('Record Not Deleted Successfully');
                       }
                       $scope.formload();
                   })
                   $scope.formload();
               }
               else {
                   swal("Record Deletion Cancelled");
                   $scope.formload();
               }
           });


            //})
        }

        $scope.cleardata = function () {
            //$scope.FMG_Id = "";
            //$scope.Cmp_Code = "";
            //$scope.install = "";
            //$scope.details = "";
            //$scope.grigview1 = false;

            $state.reload();
        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("YearlyFeeGroupMapping/Editdetails", orgid).
            then(function (promise) {
                $scope.addnewbtn = false;
                $scope.FMG_Id = promise.alldata[0].fmG_Id;

                //$scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                //$scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;

                $scope.grigview1 = true;

                $scope.totalgrid = promise.alldata;

                if (promise.alldata[0].fyghM_FineApplicableFlag = 0) {

                    $scope.totalgrid.FYGHM_FineApplicableFlag = true;
                }
                if (promise.alldata[0].fyghM_Common_AmountFlag = 0) {

                    $scope.totalgrid.FYGHM_Common_AmountFlag = true;
                }
                if (promise.alldata[0].fyghM_ActiveFlag = 0) {

                    $scope.totalgrid.FYGHM_ActiveFlag = true;
                }
            })
        }

        $scope.filterby = function () {
            var entereddata = $scope.search;

            var data = {
                "FMG_GroupName": $scope.searchthird,
                "FMH_FeeName": $scope.typethird
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("YearlyFeeGroupMapping/1", data).
        then(function (promise) {
            $scope.thirdgrid = promise.alldata;
            swal("searched Successfully");
        })
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.submitted = false;
        $scope.savedata = function (totalgrid) {

            if ($scope.myform.$valid) {

                console.log(totalgrid.FYGHM_FineApplicableFlag);
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    savetmpdata: totalgrid,
                    "ASMAY_Id": $scope.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("YearlyFeeGroupMapping/", data).
                then(function (promise) {

                    $scope.addnewbtn = true;
                    $scope.grigview1 = false;
                    $scope.thirdgrid = promise.alldata;

                    swal(promise.displaymessage);

                    $state.reload();

                })
            }
            else {
                $scope.submitted = true;
            }

        };
    }

})();