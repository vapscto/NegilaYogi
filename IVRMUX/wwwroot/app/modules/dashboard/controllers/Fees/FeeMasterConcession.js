
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeMasterConcessionController', FeeMasterConcessionController)

    FeeMasterConcessionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$stateParams']
    function FeeMasterConcessionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter,$stateParams) {

        //var paginationformasters;
        //var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings.length > 0) {
        //    paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        //}
        //$scope.itemsPerPage = paginationformasters;
        //var paginationformasters2;
        //var ivrmcofigsettings2 = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        //if (ivrmcofigsettings2.length > 0) {
        //    paginationformasters2 = ivrmcofigsettings2[0].ivrmgC_PagePagination;
        //}
        //$scope.itemsPerPage2 = paginationformasters2;
       // $scope.masterlist = false;
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
                    $scope.editflg = true;
                    $scope.editbtn = true;

                }
                else {
                    $scope.editflg = false;
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


        $scope.itemsPerPage = 5;
        $scope.itemsPerPage2 = 5;
        $scope.itemsPerPage3 = 5;



        $scope.currentPage = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        
        $scope.searchValue = "";
        $scope.searchValue2 = "";
        $scope.searchValue3 = "";

        $scope.BindData = function () {
        
            var pageid = 2;
            apiService.getURI("FeeMasterConcession/getdata", pageid).then(function (promise) {
               
                $scope.categorylist = promise.concession;
                $scope.categorylist3 = promise.concession3;
                if (promise != null) {

                    $scope.mastervehicle = promise.savedata;
                    $scope.totcountfirst = $scope.mastervehicle.length;

                        $scope.mastervehicle2 = promise.savedata22;
                        $scope.totcountfirst2 = $scope.mastervehicle2.length;


                        $scope.mastervehicle3 = promise.savedata33;
                        $scope.totcountfirst3 = $scope.mastervehicle3.length;


                        $scope.grouplist = promise.group;
                       
                       // $scope.masterlist = true;
                  
                }
                else {
                    swal("No Records Found");
                }
            })

        }

        //=======selection of checkbox....
        $scope.togchkbxC = function () {

            $scope.usercheckC = $scope.headlist.every(function (role) {
                return role.selected;
            });
        }

        //---------all checkbox Select...
        $scope.all_checkC = function (all) {

            $scope.usercheckC = all;
            var toggleStatus = $scope.usercheckC;
            //if ($scope.usercheckc == 1) {
            //    angular.foreach($scope.headlist, function (role) {
            //        role.selected = true;
            //    });
            //}

            angular.forEach($scope.headlist, function (itm) {
                itm.selected = toggleStatus;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.headlist.some(function (item) {
                return item.selected;
            });
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };
        $scope.limit = function () {
            if ($scope.fmcC_ConcessionApplLimit != null && $scope.fmcC_ConcessionApplLimit != '' && $scope.fmcC_ConcessionApplLimit != undefined) {
                if (parseInt($scope.fmcC_ConcessionApplLimit) > 100) {
                    swal("Number Should be less than or equal to 100");
                    $scope.fmcC_ConcessionApplLimit = '';
                }             
            }

          
        }

        //---Save--//
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "FMCC_Id": $scope.fmcC_Id,
                    "FMCC_ConcessionName": $scope.fmcC_ConcessionName,
                    "FMCC_ConcessionApplLimit": $scope.fmcC_ConcessionApplLimit
                }
                apiService.create("FeeMasterConcession/savedata", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.retrunval == true) {
                            swal("Record Saved Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.retrunval == true) {
                            swal("Record Updated Successfully");
                            $state.reload();
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.submitted2 = false;
        $scope.submitted3 = false;
        $scope.savedata2 = function () {
         
            if ($scope.myform2.$valid) {
                var data = {
                    "FMCCD_Id": $scope.FMCCD_Id,
                    "FMCC_Id": $scope.FMCC_Id,
                    "FMCCD_FromNoSibblings": $scope.FMCCD_FromNoSibblings,
                    "FMCCD_ToNoSibblings": $scope.FMCCD_ToNoSibblings,
                    "FMCCD_PerOrAmt": $scope.FMCCD_PerOrAmt,
                    "FMCCD_PerOrAmtFlag": $scope.FMCCD_PerOrAmtFlag,
                   

                }
                apiService.create("FeeMasterConcession/savedata2", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.retrunval == true) {
                            swal("Record Saved Successfully");
                          
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.retrunval == true) {
                            swal("Record Updated Successfully");
                           
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $scope.FMCC_Id = '';
                    $scope.FMCCD_FromNoSibblings = '';
                    $scope.FMCCD_ToNoSibblings = '';
                    $scope.FMCCD_PerOrAmt = '';
                    $scope.FMCCD_PerOrAmtFlag = '';
                    $scope.FMCCD_Id = 0;
                    $scope.BindData();
                })
            }
            else {

                    $scope.submitted2 = true;
                }
        }
        $scope.savedata3 = function () {
            if ($scope.myform3.$valid) {
                $scope.headlistdata = [];
                angular.forEach($scope.headlist, function (ty) {
                    if (ty.selected) {
                        $scope.headlistdata.push({
                            fmH_Id: ty.fmH_Id,
                        });
                    }
                })
                var data = {                 
                    "FMCC_Id": $scope.FMCC_Id,
                    "FMACCG_Id": $scope.FMACCG_Id,
                    "FMG_Id": $scope.FMG_Id,
                    headlistdata: $scope.headlistdata,                  
                }
                apiService.create("FeeMasterConcession/savedata3", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.retrunval == true) {
                            swal("Record Saved Successfully");
                        
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.retrunval == true) {
                            swal("Record Updated Successfully");
                        
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }

                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $scope.FMCC_Id = '';
                    $scope.FMG_Id = '';
                    $scope.headlist = [];
                    $scope.FMACCG_Id = 0;
                    $scope.BindData();
                    $scope.usercheckC = false;
                })
            }
            else {

                $scope.submitted2 = true;
            }
        }
        $scope.gethead = function () {
            var data = {
                "FMG_Id": $scope.FMG_Id
            }
            apiService.create("FeeMasterConcession/gethead", data).then(function (promise) {
                $scope.headlist = promise.head;

            })
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        //--Sorting--//     
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.sort2 = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.sort2 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.sort3 = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.sort3 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.fmcC_ConcessionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.fmcC_ConcessionApplLimit)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.fmcC_ConcessionName)).indexOf(angular.lowercase($scope.searchValue2)) >= 0
                ||
                (JSON.stringify(obj.fmccD_ToNoSibblings)).indexOf($scope.searchValue2) >= 0 ||
                    (JSON.stringify(obj.fmccD_PerOrAmt)).indexOf($scope.searchValue2) >= 0 ||
                        (JSON.stringify(obj.fmccD_FromNoSibblings)).indexOf($scope.searchValue2) >= 0;
        }


        $scope.filterValue4 = function (obj) {
            return (angular.lowercase(obj.fmcC_ConcessionName)).indexOf(angular.lowercase($scope.searchValue3)) >= 0 ||
                (angular.lowercase(obj.fmG_GroupName)).indexOf(angular.lowercase($scope.searchValue3)) >= 0
                ||
                (angular.lowercase(obj.fmH_FeeName)).indexOf(angular.lowercase($scope.searchValue3)) >= 0
        }
        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "FMCC_Id": user.fmcC_Id
            }

            apiService.create("FeeMasterConcession/editdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.fmcC_Id = Promise.editdata[0].fmcC_Id;
                    $scope.fmcC_ConcessionName = Promise.editdata[0].fmcC_ConcessionName;
                    $scope.fmcC_ConcessionApplLimit = Promise.editdata[0].fmcC_ConcessionApplLimit;
                }
            })
        }

        $scope.edit2 = function (user1) {
           
            var data = {
                "FMCCD_Id": user1.fmccD_Id,
                "FMCC_Id": user1.fmcC_Id,
            }

            apiService.create("FeeMasterConcession/edit2", data).then(function (Promise) {
              
                if (Promise != null) {
                    $scope.FMCC_Id = Promise.editdata2[0].fmcC_Id;
                    $scope.FMCCD_Id =Promise.editdata2[0].fmccD_Id;
                    $scope.fmcC_ConcessionName = Promise.editdata2[0].fmcC_ConcessionName;
                    $scope.FMCCD_FromNoSibblings = Promise.editdata2[0].fmccD_FromNoSibblings;
                    $scope.FMCCD_ToNoSibblings = Promise.editdata2[0].fmccD_ToNoSibblings;
                    $scope.FMCCD_PerOrAmt = Promise.editdata2[0].fmccD_PerOrAmt;
                    $scope.FMCCD_PerOrAmtFlag = Promise.editdata2[0].fmccD_PerOrAmtFlag;
                   
                    
                }
            })
        }
        $scope.edit3 = function (user3) {
        
            var data = {
               
                "FMCC_Id": user3.fmcC_Id,
                "FMACCG_Id": user3.fmaccG_Id,
            }
            apiService.create("FeeMasterConcession/edit3", data).then(function (promise) {
             
                if (promise != null) {
                    $scope.FMACCG_Id = promise.editlist3[0].fmaccG_Id;
                    $scope.headlist = promise.head;
                    $scope.grouplist = promise.group;
                    $scope.categorylist = promise.concession3;
                    $scope.grouplist = promise.group;
                  
                    $scope.FMCC_Id = promise.fmcC_Id;
                    $scope.FMG_Id = promise.fmG_Id;
                    
                    angular.forEach($scope.headlist, function (yy) {
                        angular.forEach(promise.editlist3, function (uu) {
                            if (yy.fmH_Id == uu.fmH_Id) {
                                yy.selected = true;
                            }
                        })
                    })
                }
            })
        }

        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fmcC_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("FeeMasterConcession/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }
        $scope.deactive2 = function (user, SweetAlert) {
         
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fmccD_ActiveFlg === true) {
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
                        apiService.create("FeeMasterConcession/deactive2/", user).
                            then(function (promise) {
                             
                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                        $scope.FMCC_Id = '';
                                        $scope.FMCCD_FromNoSibblings = '';
                                        $scope.FMCCD_ToNoSibblings = '';
                                        $scope.FMCCD_PerOrAmt = '';
                                        $scope.FMCCD_PerOrAmtFlag = '';
                                        $scope.BindData();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $scope.FMCC_Id = '';
                                        $scope.FMCCD_FromNoSibblings = '';
                                        $scope.FMCCD_ToNoSibblings = '';
                                        $scope.FMCCD_PerOrAmt = '';
                                        $scope.FMCCD_PerOrAmtFlag = '';
                                        $scope.BindData();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.FMCC_Id = '';
                    $scope.FMCCD_FromNoSibblings = '';
                    $scope.FMCCD_ToNoSibblings = '';
                    $scope.FMCCD_PerOrAmt = '';
                    $scope.FMCCD_PerOrAmtFlag = '';
                    $scope.BindData();
                });
        }
        $scope.deactive3 = function (user, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.fmaccG_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeMasterConcession/deactive3/", user).
                            then(function (promise) {

                                if (promise.message != null) {
                                    swal(promise.message);
                                }
                                else {
                                    if (promise.returnval == true) {
                                        swal(confirmmgs + " " + "Successfully");
                                        $scope.FMCC_Id = '';
                                        $scope.FMG_Id = '';
                                        $scope.headlist = [];
                                        $scope.BindData();
                                    }
                                    else {
                                        swal(confirmmgs + " " + " Successfully");
                                        $scope.FMCC_Id = '';
                                        $scope.FMG_Id = '';
                                        $scope.headlist = [];
                                        $scope.BindData();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $scope.FMCC_Id = '';
                    $scope.FMCCD_FromNoSibblings = '';
                    $scope.FMCCD_ToNoSibblings = '';
                    $scope.FMCCD_PerOrAmt = '';
                    $scope.FMCCD_PerOrAmtFlag = '';
                    $scope.BindData();
                });
        }
        //-Clear-//
        $scope.clearid = function () {
            $state.reload();
        };

        $scope.clearid2 = function () {
            $scope.FMCC_Id = '';
            $scope.FMCCD_FromNoSibblings = '';
            $scope.FMCCD_ToNoSibblings = '';
            $scope.FMCCD_PerOrAmt = '';
            $scope.FMCCD_PerOrAmtFlag = '';
            $scope.BindData();
        };
        $scope.clearid3 = function () {
            $scope.FMCC_Id = '';
            $scope.FMG_Id = '';
            $scope.headlist = [];
            $scope.searchchkbx1 = '';
            $scope.BindData();

        };
    };
})();