
(function () {
    'use strict';
    angular
.module('app')
.controller('FeeGroupController', feeController)

    feeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function feeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "fmG_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.userPrivileges = "";
        $scope.sortKey1 = "fyG_Id";
        $scope.reverse1 = true;

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            if (obj.fmG_ActiceFlag == true) {
                $scope.test = "Active";
            } else if (obj.fmG_ActiceFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.fmG_Remarks).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.searchValue1 = "";
        $scope.searchValue2 = "";
        $scope.filterValue1 = function (obj) {
            if (obj.fyG_ActiveFlag == true) {
                $scope.test = "Active";
            } else if (obj.fyG_ActiveFlag == false) {
                $scope.test = "Deactive";
            }
            return angular.lowercase(obj.grpname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase(obj.yearname).indexOf(angular.lowercase($scope.searchValue1)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue1)) >= 0;
        }
        $scope.isOptionsRequired = function () {
            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
        $scope.savedisable = true;
        $scope.editdisable = true;
        $scope.deletedisable = true;
        $scope.userPrivileges = [];
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.disabled = true;
                if (privlgs[i].ivrmirP_AddFlag == true) {
                    $scope.save = true;
                    $scope.savebtn = true;
                    $scope.savedisable = true;
                }
                else {
                    $scope.save = false;
                    $scope.savebtn = false;
            
                }
                if (privlgs[i].ivrmirP_UpdateFlag == true) {
                    $scope.editflag = true;
                    $scope.editbtn = true;
                    $scope.editdisable = true;

                }
                else {
                    $scope.editflag = false;
                    $scope.editbtn = false;
                  
                }
                if (privlgs[i].ivrmirP_DeleteFlag == true) {
                    $scope.deactiveflag = true;
                    $scope.deletebtn = true;
                    $scope.deletedisable = true;
                }
                else {
                    $scope.deactiveflag = false;
                    $scope.deletebtn = false;
              
                }


            }
        }



        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.itemsPerPage2 = paginationformasters;
            $scope.page1 = "page1";
            $scope.page2 = "page2";
           // var pageid = 2;
            apiService.getURI("FeeGroup/getalldetails", pageid).
        then(function (promise) {
            $scope.pages = promise.groupData;

           // $scope.arrlist6 = academicyrlst;
            // $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

            $scope.arrlist6 = promise.academicdrp;
        
            $scope.arrlistchk = promise.activegrpnames;
            $scope.arrlistchkkk = promise.activegrpnames;
            $scope.students = promise.newary;
            $scope.getarray = promise.getarray;

            $scope.totcountfirst = promise.groupData.length;
            $scope.totcountsecond = promise.newary.length;
            $scope.feeGroupname = promise.feeGroupname;
            $scope.edit = false;
            $scope.deactive = false;
            $scope.user = {
                arrlistchk: [$scope.arrlistchk[1]]
            };
            $scope.checkAll = function () {
                $scope.user.arrlistchk = angular.copy($scope.arrlistchk);
            };
            $scope.uncheckAll = function () {
                $scope.user.arrlistchk = [];
            };
            $scope.checkFirst = function () {
                $scope.user.arrlistchk.splice(0, $scope.user.arrlistchk.length);
                $scope.user.arrlistchk.push($scope.arrlistchk[0]);
            };


            if ($scope.userPrivileges[0].ivrmirP_AddFlag == true) {
                $scope.save = true;
            }
            else {
                $scope.save = false;
            }

            if ($scope.userPrivileges[0].ivrmirP_UpdateFlag == true) {
                $scope.edit = true;
            }
            else {
                $scope.edit = false;
            }
            if ($scope.userPrivileges[0].ivrmirP_DeleteFlag == true) {
                $scope.deactive = true;
            }
            else {
                $scope.deactive = false;
            }
         
          
        })
        }
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.sort1 = function (keyname) {
            $scope.sortKey1 = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
        $scope.sort2 = function (keyname) {
            $scope.sortKey2 = keyname;   //set the sortKey to the param passed
            $scope.reverse2 = !$scope.reverse2; //if true make it false and vice versa
        }
        $scope.cance = function () {
            $scope.fmG_Id = 0;
            $scope.FMG_Remarks = "";
            $scope.FMG_CompulsoryFlag = "";
            $scope.FMG_GroupName = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.searchValue = "";
            var pageid = 1;
            apiService.getURI("FeeGroup/getalldetails", pageid).
       then(function (promise) {

           $scope.arrlistchk = promise.activegrpnames;
       })
        }

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {

            return angular.lowercase(obj.fmG_GroupName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.selectacademicyear=function()
        {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeGroup/selectacademicyear", data).
       then(function (promise) {

          // $scope.arrlistchk = promise.fillmastergroup;
       })

     }

        $scope.submitted = false;
        $scope.saveGroupdata = function () {

            if ($scope.myForm.$valid) {

                //var activeflag = $scope.FMG_CompulsoryFlag
                //if (activeflag == true) {
                //    activeflag = 1;
                //}
                //else {
                //    activeflag = 0;
                //}

                var transport =0;
           
                var hostle = 0;
               
                if ($scope.FMG_Transport == 'T') {
                    transport = 1;
                }
                else if ($scope.FMG_Transport == 'H') {
                    hostle = 1;
                }
                else {
                    transport = 0;
                    hostle = 0;
                }
                var FMG_CompulsoryFlag = $scope.FMG_CompulsoryFlag
                if (FMG_CompulsoryFlag == true) {
                    FMG_CompulsoryFlag = 1;
                }
                else {
                    FMG_CompulsoryFlag = 0;
                }

                var data = {
                    //"ASMAY_Id":$scope.ASMAY_Id,
                    "FMG_Id": $scope.fmG_Id,
                    "FMG_GroupName": $scope.FMG_GroupName,
                    "FMG_Remarks": $scope.FMG_Remarks,
                    "FMG_CompulsoryFlag": FMG_CompulsoryFlag,
                    "FMG_ActiceFlag": true,
                    "FMG_HostelFlg": hostle,
                    "FMG_TransportFlg": transport,
                    "FMG_RegNewFlg": $scope.FMG_Reg,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeGroup/", data).then(function (promise) {
                    if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record Already Exist');
                       // $scope.cance();
                    }

                    else if (promise.returnduplicatestatus == "Save") {

                        swal('Record Saved Successfully');
                        $state.reload();
                    }

                    else if (promise.returnduplicatestatus == "NotSave") {

                        swal('Record Not Saved');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == "Update") {

                        swal('Record Updated Successfully');
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == "NotUpdate") {

                        swal('Record Not Updated');
                        $state.reload();
                    }
                   
                      
                    else {
                        if (promise.message != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }
                        $scope.cance();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMG_GroupName": $scope.search,
                "FMG_Remarks": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeGroup/1", data).
        then(function (promise) {
            $scope.pages = promise.groupData;
            swal("searched Successfully");
        })
        }
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmG_Id;
            var pageid = $scope.editEmployee;
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
                    apiService.DeleteURI("FeeGroup/deletepages", pageid).
                    then(function (promise) {
                        $scope.pages = promise.groupData;
                        if (promise.returnval != true && promise.retvalue === true) {
                            swal('Record Not Deleted.', 'Group Already Mapped Yearly Group Head');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully!');

                        }
                        $scope.loaddata();
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmG_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeGroup/getdetails", pageid).
            then(function (promise) {

                $scope.FMG_GroupName = promise.groupData[0].fmG_GroupName;
                $scope.FMG_Remarks = promise.groupData[0].fmG_Remarks;

                $scope.FMG_CompulsoryFlag = promise.groupData[0].fmG_CompulsoryFlag;
                var activeflag = promise.groupData[0].fmG_CompulsoryFlag;

                if (promise.groupData[0].fmG_TransportFlg == true) {
                    $scope.FMG_Transport = true;

                }
                else {
                    $scope.FMG_Transport = false;
                }

                if (promise.groupData[0].fmG_HostelFlg == true) {
                    $scope.FMG_Transport = 'H';

                }
               
                else if (promise.groupData[0].fmG_TransportFlg == true) {
                    $scope.FMG_Transport = 'T';

                }
                else {
                    $scope.FMG_Transport = false;
                }

                if (promise.groupData[0].fmG_CompulsoryFlag == true) {
                    $scope.FMG_CompulsoryFlag = true;

                }
                else {
                    $scope.FMG_CompulsoryFlag = false;
                }

                if (promise.groupData[0].fmG_RegNewFlg == 'R') {
                    $scope.FMG_Reg = 'R';

                }
                else {
                    $scope.FMG_Reg = 'N';
                }
                $scope.fmG_Id = promise.groupData[0].fmG_Id;
            })
        }
        $scope.deactivefeegroup = function (groupData, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (groupData.fmG_ActiceFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {

                apiService.create("FeeGroup/deactivate", groupData).
                then(function (promise) {
                    if (promise.returnval == true) {
                        swal("Record "+confirmmgs  + " Successfully");
                    }
                    else {
                        swal("Record is already been used !!!");
                    }
                    $scope.loaddata();
                })

            }
            else {
                swal("Record " + mgs + " Cancelled");
                $scope.loaddata();
            }
        });
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.arrlistchk, function (role) { role.selected = toggleStatus; });

        }

        $scope.optionToggled = function (fmG_Id) {
            
            if ($scope.disablegroups == false)
            {
                $scope.selectAll = $scope.arrlistchk.every(function (role) { return role.selected; })
            }
            else {
                for (var i = 0; i < $scope.arrlistchk.length; i++) {
                    
                    if (fmG_Id == $scope.arrlistchk[i].fmG_Id) {
                        $scope.arrlistchk[i].selected = true;
                    }
                    else
                    {
                        $scope.arrlistchk[i].selected = false;
                    }
                }
            }           
        }

        var name = "";
        var roleflag = "";
        $scope.submitted1 = false;
        $scope.saveYearlyGroupdata = function (arrlistchk) {
            if ($scope.myForm1.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })

                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Group From List Atleast One....!')
                }
                else {

                    var data = {
                        "MI_Id": 2,
                        "FYG_Id": $scope.fyG_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "FYG_ActiveFlag": true,
                        "TempararyArrayList": $scope.albumNameArray,
                        "FYG_PartialRebateAmtOrPercentageValue": $scope.FYG_PartialRebateAmtOrPercentageValue,
                        "FYG_RebateTypeFlg": $scope.FYG_RebateTypeFlg,
                        "FYG_RebateApplicableFlg": $scope.FYG_RebateApplicableFlg

                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeGroup/savedetailY", data).
                    then(function (promise) {
                        if (promise.returnduplicatestatus == 'Duplicate') {
                            swal("Record Already Exist");
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                        }

                        else if (promise.returnduplicatestatus == "Save") {
                            $scope.students = promise.groupData;
                            swal('Record Saved Successfully');
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                        }

                        else if (promise.returnduplicatestatus == "NotSave") {

                            swal('Record Not Saved');
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                        }
                        else if (promise.returnduplicatestatus == "Update") {
                            $scope.students = promise.groupData;
                            swal('Record Updated Successfully');
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                        }
                        else if (promise.returnduplicatestatus == "NotUpdate") {

                            swal('Record Not Updated');
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                        }
                       
                        else {


                            if (promise.message != null) {
                                swal('Record Not Updated', 'success');
                            }
                            else {
                                swal('Record Not Saved', 'success');
                            }
                            $scope.cance1($scope.arrlistchk);
                            $scope.loaddata();
                        }

                    })
                }
            }
            else {
                $scope.submitted1 = true;
            }

        };
        //$scope.cance1 = function (arrlistchk) {
        //    $scope.ASMAY_Id = "";
        //    for (var i = 0; i < $scope.arrlistchk.length; i++) {
        //        name = $scope.arrlistchk[i].selected
        //        if (name == true) {
        //            $scope.arrlistchk[i].selected = false;
        //        }
        //    }
        //    $scope.submitted1 = false;
        //    $scope.myForm1.$setPristine();
        //    $scope.myForm1.$setUntouched();
        //    $scope.searchValue1 = "";
        //    $scope.searchchkbx = "";

        //    $scope.arrlist6 = academicyrlst;
        //    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

        //}

        $scope.cance1 = function (arrlistchk) {
            $scope.ASMAY_Id = "";
            for (var i = 0; i < $scope.arrlistchk.length; i++) {
                name = $scope.arrlistchk[i].selected
                if (name == true) {
                    $scope.arrlistchk[i].selected = false;
                }
            }
            $scope.submitted1 = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.searchValue1 = "";
            $scope.searchchkbx = "";
            $scope.disablegroups = false;
            
            //  $scope.arrlist6 = academicyrlst;
            //  $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;

        }

        $scope.searchsourceY = function () {
            var entereddata = $scope.search;
            var data = {
                "Fee": $scope.search,
                "AcademicYear": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeGroup/1", data).
        then(function (promise) {
            $scope.loaddata();
            $scope.students = promise.retriveYearlyGrpdata;

            swal("searched Successfully");
        })
        }
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fyG_Id;
            var pageid = $scope.editEmployeeY;
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
                    apiService.DeleteURI("FeeGroup/deletepagesY", pageid).
                    then(function (promise) {
                        $scope.loaddata();
                        $scope.students = promise.retriveYearlyGrpdata;
                        if (promise.returnval != true && promise.retvalue === true) {
                            swal('Record Not Deleted. Group Already Mapped Yearly Group Head');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted Successfully!');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        };
        //extra added
        $scope.edit = function (user) {
            $scope.FTMCOM_Id = user.ftmcoM_Id;
            $scope.fmgg_Id = user.fmgG_Id;
            $scope.FFGCMA_Id = user.ffgcmA_Id;
            angular.forEach($scope.arrlistchkkk, function (group) {
                if (group.fmG_Id === $scope.fmgg_Id) {
                    group.selected = true;
                }
            })
           
            
        };
        $scope.Deletedata = function (item, SweetAlert) {
            $scope.FFGCMA_Id = item.ffgcmA_Id;
            var data = {
                "FFGCMA_Id": item.ffgcmA_Id,

            }
            var dystring = "";
            if (item.ffgcmA_ActiveId == true) {
                dystring = "Deactivate";
            }
            else if (item.ffgcmA_ActiveId == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FeeGroup/deletedataYYY", data).
                            then(function (promise) {
                                if (promise.return_val == "Delete") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                if (promise.return_val == "NotDelete") {
                                    swal("Record  " + dystring + "d Successfully!!!");
                                }
                                //NotDelete

                                $scope.FTMCOM_Id = "";
                                $scope.FFGCMA_Id = "";
                                $scope.submitted2 = false;
                                $scope.loaddata();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }     
        $scope.cancel2 = function () {
            $scope.FFGCMA_Id = "";
            $scope.submitted2 = false;
            $scope.loaddata();
        };
        $scope.submitted2 = false;
        $scope.saveTallyMaster = function () {
            $scope.submitted2 = true;
            if ($scope.myForm2.$valid) {
                var FFGCMA_Id = 0;
                if ($scope.FFGCMA_Id != null && $scope.FFGCMA_Id > 0) {
                    FFGCMA_Id = $scope.FFGCMA_Id;
                }
                $scope.tempararyArrayList = [];   
                angular.forEach($scope.arrlistchkkk, function (comp) {
                    if (comp.selected == true) {
                        $scope.tempararyArrayList.push({ FMG_Id: comp.fmG_Id, FMGG_GroupName: comp.fmG_GroupName });
                    }
                });
                var data = {
                    "TempararyArrayList": $scope.tempararyArrayList,
                    "FTMCOM_Id": $scope.FTMCOM_Id,                    
                    "FFGCMA_Id": FFGCMA_Id,

                }
                apiService.create("FeeGroup/savedataFTally", data).
                    then(function (promise) {

                        if (promise.return_val == "Update") {
                            swal("Record Update Successfully !");
                        }
                        if (promise.return_val == "Notupdate") {
                            swal("Record Not Update !");
                        }
                        if (promise.return_val == "save") {
                            swal("Record Saved Successfully !");
                        }
                        if (promise.return_val == "Notsave") {
                            swal("Record Not Saved !");
                        }
                        if (promise.return_val == "RecordExist") {
                            swal("Record Already Exist !");
                        }
                        if (promise.return_val === "") {
                            swal("Please Contact Administrator !");
                        }
                        if (promise.return_valtwo != null && promise.return_valtwo != "") {
                            swal('Some Record Already Exist', "These Group name " + promise.return_valtwo + " Duplicate");
                        }
                        $scope.tempararyArrayList = [];
                        $scope.FTMCOM_Id = "";
                        $scope.FFGCMA_Id = "";
                        $scope.submitted2 = false;
                        $scope.loaddata();
                    })

            }
        };
        $scope.togchkbxCCCC = function () {
            $scope.fmG_Id = $scope.arrlistchkkk.every(function (options) {
                return options.selected;
            });
        };
        $scope.all_checkCCCC = function () {
            var fmG_Id = $scope.fmgg_Id;
            var count = 0;
            angular.forEach($scope.arrlistchkkk, function (itm) {
                itm.selected = fmG_Id;
                if (itm.selected == true) {
                    count += 1;
                }
                else {
                    count = 0;
                }
            });
        };
        $scope.isOptionsRequiredtwo = function () {
            return !$scope.arrlistchkkk.some(function (options) {
                return options.selected;
            });
        };
        //close
        $scope.disablegroups = false;

        $scope.getorgvalueY = function (employee, arrlistchk, arrlist6) {
            $scope.editEmployeeY = employee.fyG_Id;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeGroup/getdetailsY", pageid).
            then(function (promise) {
                
                for (var i = 0; i < arrlistchk.length; i++) {

                    $scope.disablegroups = true;

                    name = arrlistchk[i].fmG_Id;
                    if (name == promise.groupYearData[0].fmG_Id) {
                        arrlistchk[i].selected = true
                        //$scope. arrlistchk[i].disablegroups = true;
                    }
                    else {
                        arrlistchk[i].selected = false;
                       // $scope.arrlistchk[i].disablegroups = true;
                    }

                }
                $scope.ASMAY_Id = promise.groupYearData[0].asmaY_Id;
                $scope.fyG_Id = promise.groupYearData[0].fyG_Id;
            })
        }
        $scope.deactiveY = function (newary, SweetAlert) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            var confirmmgs = "";
            if (newary.fyG_ActiveFlag == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("FeeGroup/deactivateY", newary).
                then(function (promise) {
                    $scope.students = promise.groupYearData;
                    if (promise.returnval == true) {
                        swal(confirmmgs + " Successfully");
                    }
                    else {
                        swal("Record is already been used !!!");
                    } $scope.loaddata();
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
        });
        }

    }
    function getIndependentDrpDwnss() {
        apiService.getURI("FeeGroup/getdpforyear", 2).then(function (promise) {

            $scope.arrlist6 = promise.academicdrp;

            // for pagination 
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            $scope.pages = promise.academicdrp;
            // for pagination
        })
        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }

   
})();

