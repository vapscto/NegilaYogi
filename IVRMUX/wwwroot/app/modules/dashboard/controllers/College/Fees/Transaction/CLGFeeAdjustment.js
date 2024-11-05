(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGFeeAdjustmentController', CLGFeeAdjustmentController)

    CLGFeeAdjustmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CLGFeeAdjustmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.filterdata1 = "Refunable";
        $scope.sortKey = 'fsA_Id';
        $scope.sortReverse = true;
        $scope.totcountsearch = 0;
        $scope.FSA_Date = new Date();
        /* loading start*/
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("CLGFeeAdjustment/getalldetails", pageid).
                then(function (promise) {
                    debugger;
                    $scope.yearlst = promise.fillyear;
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.thirdgrid = promise.filldata;
                    console.log($scope.thirdgrid);
                    $scope.totcountfirst = promise.filldata.length;
                    $scope.onselectyear();
                    //  $scope.classlst = promise.fillclass;
                    //   $scope.studentlst = promise.fillstudent;
                    //  $scope.groupfromlst = promise.fillfromgroup;

                })
        }
        /* loading end*/
        /* Academic year start*/
        $scope.onselectyear = function () {
            debugger;
            if ($scope.cfg.ASMAY_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAdjustment/getclass", data).
                    then(function (promise) {
                        $scope.grigview1 = false;
                        $scope.grigview2 = false;
                        $scope.course_list = promise.courselist;
                    });
            }
        }
        //Academic year end 
        /* classs start*/
        $scope.onselectclass = function () {
            debugger;

            $scope.AMB_Id = "";
            $scope.AMCST_Id = "";

            if ($scope.cfg.ASMAY_Id != "" && $scope.AMCO_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAdjustment/getsection", data).
                    then(function (promise) {
                        $scope.grigview1 = false;
                        $scope.grigview2 = false;
                        $scope.branch_list = promise.branchlist;
                    });
            }
        }
        //classs end 
        $scope.get_semisters = function () {
            debugger;
            $scope.AMCST_Id = "";
            var data = {

                "ASMAY_Id": $scope.cfg.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGFeeRefundable/get_semisters", data).
                then(function (promise) {
                    debugger;

                    if (promise.semisterlist.length > 0) {

                        $scope.semister_list = promise.semisterlist;
                    }
                    else {
                        $scope.semister_list = [];
                        swal('No records found to display');
                    }
                })
        }
        /* section start*/
        $scope.onselectsection = function () {
            $scope.AMCST_Id = "";
            debugger;
            if ($scope.cfg.ASMAY_Id != "" && $scope.AMCO_Id != "" && $scope.AMB_Id != "" && $scope.AMSE_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAdjustment/getstudent", data).
                    then(function (promise) {
                        $scope.grigview1 = false;
                        $scope.grigview2 = false;
                        $scope.studentlst = promise.fillstudent;
                    });
            }
        }
        //classs end 
        //start student
        $scope.onselectstudent = function () {
            if ($scope.cfg.ASMAY_Id != "" && $scope.AMCST_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "filterrefund": $scope.filterdata1,
                    "AMCST_Id": $scope.AMCST_Id
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAdjustment/getbothgroup", data).
                    then(function (promise) {
                        debugger;
                        $scope.grigview1 = false;
                        $scope.grigview2 = false;
                        $scope.groupfromlst = promise.fillfromgroup;
                        // $scope.grouptolst = promise.filltogroup;
                    });
            }
        }
        //end student
        //start group from 
        $scope.optionToggledGF = function () {
            debugger;
            $scope.headtolst = [];
            var groupidss;
            for (var i = 0; i < $scope.groupfromlst.length; i++) {
                debugger;
                if ($scope.groupfromlst[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.groupfromlst[i].fsA_From_FMG_Id;
                    else
                        groupidss = groupidss + "," + $scope.groupfromlst[i].fsA_From_FMG_Id;
                }


            }
            if (groupidss != undefined) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "filterrefund": $scope.filterdata1,
                    "multiplegroupF": groupidss
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CLGFeeAdjustment/getfromhead", data).
                    then(function (promise) {
                        debugger;
                        $scope.grigview1 = true;
                        $scope.headfromlst = promise.fillfromhead;
                        $scope.grouptolst = promise.filltogroup;
                    });
            } else { $scope.grigview1 = false; }
        }
        //end group from 

        //start group to 
        $scope.optionToggledGT = function (fmgtoid) {
            if ($scope.grigview1 == false) {
                swal('Please Select From Group before selecting To Group');
                fmgtoid.selected = false;
            }
            else {
                var groupidss;
                for (var i = 0; i < $scope.grouptolst.length; i++) {
                    debugger;
                    if ($scope.grouptolst[i].selected == true) {

                        if (groupidss == undefined)
                            groupidss = $scope.grouptolst[i].fsA_To_FMG_Id;
                        else
                            groupidss = groupidss + "," + $scope.grouptolst[i].fsA_To_FMG_Id;
                    }


                }
                if (groupidss != undefined) {
                    var data = {

                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        "AMCST_Id": $scope.AMCST_Id,
                        "multiplegroupT": groupidss
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }


                    apiService.create("CLGFeeAdjustment/gettohead", data).
                        then(function (promise) {
                            debugger;
                            $scope.grigview2 = true;
                            $scope.headtolst = promise.filltohead;
                        });
                } else { $scope.grigview2 = false; }
            }
        }
        //end group to

        //validation start 
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //validation  
        /* post data start*/

        $scope.submitted = false;
        $scope.savedata = function (headfromlst, headtolst) {
            debugger;

            if ($scope.myForm.$valid) {
                $scope.albumNameArray1 = [];
                angular.forEach($scope.groupfromlst, function (role) {
                    if (!!role.selected) $scope.albumNameArray1.push(role);
                })
                $scope.albumNameArray2 = [];
                angular.forEach($scope.grouptolst, function (role) {
                    if (!!role.selected) $scope.albumNameArray2.push(role);
                })
                $scope.albumNameArray3 = [];
                angular.forEach($scope.headfromlst, function (user) {
                    if (!!user.studchecked) $scope.albumNameArray3.push(user);
                })
                $scope.albumNameArray4 = [];
                angular.forEach($scope.headtolst, function (role) {
                    if (!!role.studchecked) $scope.albumNameArray4.push(role);
                })

                if ($scope.albumNameArray1.length === 0) {
                    swal('Please Select altleast one From Group ....!')
                }
                else if ($scope.albumNameArray2.length === 0) {
                    swal('Please Select altleast one To Group ....!')
                }
                else if ($scope.albumNameArray3.length === 0) {
                    swal('Please Select altleast one From Head ....!')
                }
                else if ($scope.albumNameArray4.length === 0) {
                    swal('Please Select altleast one To Head ....!')
                }
                else {
                    $scope.albumNameArraynewone = [];
                    $scope.albumNameArraynewtwo = [];
                    var fromtotalamount = 0;
                    var tototalamount = 0;
                    for (var i = 0; i < $scope.headfromlst.length; i++) {
                        if ($scope.headfromlst[i].studchecked == true) {
                            fromtotalamount = fromtotalamount + parseInt(headfromlst[i].fsS_RunningExcessAmount);
                            $scope.albumNameArraynewone.push(headfromlst[i]);
                        }
                    }
                    var a = "";
                    for (var j = 0; j < $scope.headtolst.length; j++) {
                        debugger;

                        if ($scope.headtolst[j].studchecked == true) {
                            tototalamount = tototalamount + parseInt(headtolst[j].fsA_AdjustedAmount);
                            if (headtolst[j].fsA_AdjustedAmount == 0 || headtolst[j].fsA_AdjustedAmount == null || headtolst[j].fsA_AdjustedAmount == "") {
                                a = "s";
                                break
                            }
                            else if (headtolst[j].fsA_AdjustedAmount > headtolst[j].tobepaid) {
                                a = "b";

                                break;
                            }
                            else {
                                $scope.albumNameArraynewtwo.push(headtolst[j]);
                            }


                        }
                        else {
                            if (headtolst[j].fsA_AdjustedAmount > 0) {
                                a = "c";
                                break;
                            }

                        }
                    }
                    var ss = fromtotalamount > tototalamount ? true : false;
                    if (a == "s") {
                        swal('Please Enter the Adjusted Amount for the selected checkbox')
                    }
                    else if (a == "b") {
                        swal('Adjusted Amount Should be lesser than or equal to Balance')
                    }
                    else if (a == "c") {
                        swal('Please select the check box for the entered amount')
                    }
                    else if (tototalamount > fromtotalamount) {
                        swal('Total To Head amount should not be greater than From Head amount')
                    }
                    else {
                        var data = {
                            "FSA_Id": $scope.fsA_Id,
                            "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            "AMCST_Id": $scope.AMCST_Id,
                            "FSA_Date": $scope.FSA_Date,
                            "fromlist": $scope.albumNameArraynewone,
                            "tolist": $scope.albumNameArraynewtwo
                        }

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }
                        if ($scope.fsA_Id > 0) {
                            var disfun = "Update";
                        }
                        else {
                            var disfun = "Save";
                        }
                        swal({
                            title: "Are you sure?",
                            text: "Do You Want To " + disfun + " Record? ",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                            cancelButtonText: "Cancel",
                            closeOnConfirm: false,
                            closeOnCancel: false,
                            showLoaderOnConfirm: true,
                        },
                            function (isConfirm) {
                                if (isConfirm) {
                                    apiService.create("CLGFeeAdjustment/savedata", data).
                                        then(function (promise) {
                                            if (promise.returnduplicatestatus === "Saved" || promise.returnduplicatestatus === "Updated") {
                                                // $scope.thirdgrid = promise.filldata;
                                                swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                                                $state.reload();
                                                $scope.loaddata();
                                            }
                                            else if (promise.returnduplicatestatus === "Duplicate") {
                                                swal('Duplicate Record');
                                            }
                                            else {
                                                if (promise.returnduplicatestatus === "Saved" || promise.returnduplicatestatus === "Updated") {
                                                    swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                                                }
                                                else {
                                                    swal(promise.returnduplicatestatus);
                                                }
                                            }
                                            //if (promise.returnduplicatestatus === "success") {

                                            //    $scope.thirdgrid = promise.filldata;

                                            //    swal('Record Saved/Updated Successfully');
                                            //}
                                            //else if (promise.returnduplicatestatus === "Duplicate") {

                                            //    $scope.thirdgrid = promise.filldata;

                                            //    swal('Duplicate Record');
                                            //}  
                                            //else {
                                            //    swal('Record Not Saved/Updated Successfully');
                                            //}
                                        })
                                    $scope.cleardata();
                                }
                                else {
                                    swal("Record saved Failed", "Failed");
                                }
                            });
                    }

                }
            }
            else { $scope.submitted = true; }
        };
        /* post data end*/
        //clearing data start

        $scope.cleardata = function () {
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
            //$scope.grigview1 = false;
            //$scope.grigview2 = false;
            //$scope.disabledata = false;
            //$scope.cfg.ASMAY_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.ASMS_Id = "";
            //$scope.groupfromlst = "";
            //$scope.grouptolst = "";
            //$scope.AMST_Id = "";
            //$scope.thirdgrid = "";
            $state.reload();
        }
        //clearing data end
        //delete start
        $scope.DeletRecord = function (employee, SweetAlert) {
            debugger;
            $scope.editEmployee = employee.fsA_Id;
            var feechequebounceid = $scope.editEmployee
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
                        apiService.DeleteURI("CLGFeeAdjustment/Deletedetails", feechequebounceid).
                            then(function (promise) {
                                if (promise.returnduplicatestatus == "success") {

                                    $scope.thirdgrid = promise.filldata;

                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                    $scope.cleardata();
                });

            //})
        }
        //edit end
        //edit start
        $scope.editdata = function (employee) {
            debugger;
            $scope.editEmployee = employee.fsA_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CLGFeeAdjustment/getdetails", pageid).
                then(function (promise) {
                    debugger;
                    $scope.grigview1 = true;
                    $scope.grigview2 = true;
                    $scope.disabledata = true;
                    $scope.fsA_Id = promise.fsA_Id;
                    $scope.cfg.ASMAY_Id = promise.asmaY_Id;
                    $scope.FSA_Date = new Date(promise.fsA_Date);

                    $scope.course_list = promise.courselist;
                    $scope.AMCO_Id = promise.amcO_Id;
                    $scope.AMCO_Idedit = promise.amcO_Id;

                    $scope.branch_list = promise.branchlist;
                    $scope.AMB_Id = promise.amB_Id;
                    $scope.AMB_Idedit = promise.amB_Id;

                    $scope.semister_list = promise.semisterlist;
                    $scope.AMSE_Id = promise.amsE_Id;
                    $scope.AMSE_Idedit = promise.amsE_Id;

                    $scope.studentlst = promise.fillstudent;
                    $scope.AMCST_Id = promise.amcsT_Id;
                    $scope.AMCST_Idedit = promise.amcsT_Id;

                    $scope.groupfromlst = promise.fillfromgroup;
                    $scope.groupfromlst[0].selected = true;
                    $scope.grouptolst = promise.filltogroup;
                    $scope.grouptolst[0].selected = true;
                    $scope.headfromlst = promise.fillfromhead;
                    $scope.headfromlst[0].studchecked = true;
                    $scope.headtolst = promise.filltohead;
                    $scope.headtolst[0].studchecked = true;
                })
        }
        //edit end
        //search start

        $scope.search_flag = false;
        $scope.search123 = "";
        var search_s = "";
        var list_s = [];
        $scope.onselectsearch = function () {
            search_s = $scope.search123;
            list_s = $scope.receiptgrid;
            if (search_s == "" || search_s == undefined) {
                swal("Select Any Field For Search");
                $scope.search_flag = false;
            }
            else {
                $scope.search_flag = true;

                if ($scope.search123 == "6") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    $scope.dat = false;

                }
                else if ($scope.search123 == "7") {

                    $scope.txt = false;
                    $scope.numbr = false;
                    $scope.dat = true;

                }
                else {
                    $scope.txt = true;
                    $scope.numbr = false;
                    $scope.dat = false;

                }
                $scope.searchtxt = "";
                $scope.searchdat = "";
                $scope.searchnumbr = "";

            }
        }
        $scope.ShowSearch_Report = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            if ($scope.searchtxt != "" || $scope.searchnumbr != "" || $scope.searchdat != "") {
                if ($scope.search123 == "6") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }
                else if ($scope.search123 == "7") {
                    debugger;

                    var date = new Date($scope.searchdat).toDateString("dd/MM/yyyy");
                    var data = {
                        "searchType": $scope.search123,
                        "searchdate": date
                    }
                }
                else {

                    var data = {
                        "searchType": $scope.search123,
                        "searchtext": $scope.searchtxt
                    }
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAdjustment/searching", data).
                    then(function (promise) {
                        $scope.thirdgrid = promise.filldata;
                        $scope.totcountsearch = promise.filldata.length;
                        if (promise.filldata == null || promise.filldata == "") {
                            swal("Record Does Not Exist For Searched Data !!!!!!")
                        }
                    })
            }
            else {
                swal("Data Is Needed For Search ");
            }
        }
        $scope.clearsearch = function () {
            //$scope.search123 = "";
            //$scope.search_flag = false;
            //$scope.searchtxt = "";
            //$scope.searchnumbr = "";
            //$scope.searchdat = "";
            $state.reload();
        }
        //search end

        $scope.chk_amount = function (user, index) {
            if (user.fsA_AdjustedAmount != undefined && user.fsA_AdjustedAmount != '') {
                if (Number(user.tobepaid) < Number(user.fsA_AdjustedAmount)) {
                    swal("Entered Amount Is Not More Than To Be Pay Amount!!!");
                    $scope.headtolst[index].fsA_AdjustedAmount = 0;
                }
                else {
                    var total_amount = 0;
                    angular.forEach($scope.headfromlst, function (itm) {
                        if (itm.studchecked) {
                            total_amount += Number(itm.fsS_RunningExcessAmount);
                        }
                    })
                    if (total_amount < Number(user.fsA_AdjustedAmount)) {
                        swal("Entered Amount Is Not More Than Adjusting Head Amount!!!");
                        $scope.headtolst[index].fsA_AdjustedAmount = 0;
                    }
                }
            }
        }
    }
})();