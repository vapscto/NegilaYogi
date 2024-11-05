(function () {
    'use strict';
    angular
.module('app')
.controller('FeeCardDetailsEntryController', FeeCardDetailsEntry)

    FeeCardDetailsEntry.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', 'blockUI']
    function FeeCardDetailsEntry($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, blockUI) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.totcountsearch = 0;
        $scope.cfg = {};
        //load start
        $scope.loaddata = function () {
            
            $scope.search = "";
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            $scope.showstudentname = true;
            $scope.IsHiddenup = true;
            $scope.tablegrid = true;
            apiService.getURI("FeeCardDetailsEntry/getdata", pageid).
            then(function (promise) {
                
                $scope.yearlst = promise.fillyear;
                $scope.studentlst = promise.fillstudent;
                $scope.cfg.ASMAY_Id = promise.fillyear[0].asmaY_Id;
                $scope.grid = promise.fillgrid;
                $scope.totcountfirst = $scope.grid.length;
            })

        };
        //load end
        //validation start
        $scope.order = function (keyname) {
            
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        //validation end
        //student list start
        $scope.studentlst = [];
        $scope.searchfilter = function (objj) {
            
            var data = {
                "searchfilter": objj.search
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeCardDetailsEntry/searchfilter", data).
            then(function (promise) {
                
                $scope.studentlst = promise.fillstudent;
                angular.forEach($scope.studentlst, function (objectt) {
                    if (objectt.amsT_FirstName.length > 0) {
                        var string = objectt.amsT_FirstName;
                        objectt.amsT_FirstName = string.replace(/  +/g, ' ');
                    }
                })
            })
        };
        //student list end
        //group start
        $scope.onselectstudent = function (studentlst) {
            
            var studid = studentlst.amst_Id;
            var data = {
                "Amst_Id": studid,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeCardDetailsEntry/getstudlistgroup", data).
            then(function (promise) {            
                if (promise.fillmastergroup.length > 0) {
                    $scope.grouplst = promise.fillmastergroup;
                }               
            })
        };
        //group end
        //head start
        $scope.onselectgroup = function (grouplst) {
            
            var groupid = $scope.FMG_Id;
            var data = {
                "Amst_Id": $scope.Amst_Id1.amst_Id,
                "FMG_Id": groupid,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeCardDetailsEntry/getgroupmappedheads", data).
            then(function (promise) {
            if (promise.alldata.length > 0) {
                $scope.headcount = promise.alldata;
            }
            else {
               swal("No Fee Head for that Group")
           }
       })
        };
        //head end
        //save start
        $scope.submitted = false;
        $scope.Savedata = function () {
            
            if ($scope.myForm.$valid) {
                if ($scope.FSFM_Id>0)
                {
                     var data = {
                    "FSFM_Id": $scope.FSFM_Id,
                    "FSFM_Amount": $scope.amount,
                    "Amst_Id": $scope.amst_Id,
                    "FMH_Id": $scope.FMH_Id,
                    "FMG_Id": $scope.FMG_Id,
                    }
                }
            else{
                var data = {
                    "FSFM_Amount": $scope.amount,
                    "Amst_Id": $scope.Amst_Id1.amst_Id,
                    "FMH_Id": $scope.FMH_Id,
                    "FMG_Id": $scope.FMG_Id,
                }
            }
               
               
                apiService.create("FeeCardDetailsEntry/savedata", data).then(function (promise) {
                    if (promise.returnval == "Save") {
                        swal('Record Saved Successfully');
                    }
                    else if (promise.returnval == "not Save") {
                        swal('Record not Saved Successfully');
                    }
                    else if (promise.returnval == "Update") {
                        swal('Record Updated Successfully');
                    }
                    else if (promise.returnval == "not Update") {
                        swal('Record not Updated Successfully');
                    }
                    else if (promise.returnval == "Duplicate") {
                        swal('Duplicate Record');
                    }
                    else if (promise.returnval == "a") {
                        swal('Fee Amount Entry is not mapped');
                    }
                    else {
                        swal('Record Not Saved');
                    }
                    $state.reload();

                });
            }
            else {
                $scope.submitted = true;

            }
        };
        //save end
        //edit start
        $scope.EditData = function (data) {
            
            $scope.editdata = data.fsfM_Id;
            var pageid = $scope.editdata;
            apiService.getURI("FeeCardDetailsEntry/editdetails", pageid).
            then(function (promise) {
                
                if (promise.returnval == "Already Paid. Record cannot Edit") {
                    swal("Already Paid. Record cannot Edit");
                }
                else {
                    $scope.amst_Id = promise.fillgrid[0].amst_Id;
                    $scope.amsT_FirstName=promise.fillgrid[0].amsT_FirstName+' '+promise.fillgrid[0].amsT_MiddleName+' '+promise.fillgrid[0].amsT_LastName;
                    $scope.amount = promise.fillgrid[0].fsfM_Amount;
                    $scope.FSFM_Id = promise.fillgrid[0].fsfM_Id;
                    $scope.updateshowlabel = true;
                    $scope.updatename = $scope.amsT_FirstName;
                    $scope.showstudentname = false;
                    $scope.headcount = promise.alldata;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.fmG_Idedit = promise.fillgrid[0].fmG_Id;
                    $scope.fmH_Idedit = promise.fillgrid[0].fmH_Id;
                    $scope.FMG_Id = promise.fillgrid[0].fmG_Id;
                    $scope.FMH_Id = promise.fillgrid[0].fmH_Id;
                    //for (var q = 0; q < $scope.grouplst.length; q++) {
                    //    if ($scope.grouplst[q].fmG_Id == promise.fillgrid[0].fmG_Id) {
                    //        $scope.grouplst[q].Selected = true;
                    //    }
                    //}
                    //for (var q = 0; q < $scope.headcount.length; q++) {
                    //    if ($scope.headcount[q].fmH_Id == promise.fillgrid[0].fmH_Id) {
                    //        $scope.headcount[q].Selected = true;
                    //    }
                    //}
                    
                   
                }
            })
        };
        //edit end
        //delete start
        $scope.deletedata = function (data) {
            
            var del = data.fsfM_Id;
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
                   apiService.DeleteURI("FeeCardDetailsEntry/Deletedetails", del).
                   then(function (promise) {

                       if (promise.returnval == "true") {
                           swal('Record Deleted Successfully');
                       }
                       else if (promise.returnval == "false") {
                           swal('Record Not Deleted')
                       }
                       else {
                           swal(promise.returnval)
                       }
                       $state.reload();
                   })
               }
               else {
                   swal("Record Deletion Cancelled");
                   $state.reload();
               }
           });
        };
        //delete end
        //cancel start
        $scope.ClearAll = function () {
            
            $scope.submitted = false;
            $scope.myFormhead.$setPristine();
            $scope.myFormhead.$setUntouched();
            $state.reload();

        };
        //cancel end
    }

})();