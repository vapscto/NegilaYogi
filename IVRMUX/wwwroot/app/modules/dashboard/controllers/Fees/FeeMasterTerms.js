

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeMasterTermsController', feetrController)

    feetrController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams']
    function feetrController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache,$stateParams) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        //$scope.ternduedatess = false;

        $scope.sortKey = "fmT_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.sortKey1 = "idforedt";   //set the sortKey to the param passed
        $scope.reverse1 = true; //if true make it false and vice versa

        $scope.sortKeythird = "idforedt";   //set the sortKey to the param passed
        $scope.reversethird = true; //if true make it false and vice versa
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

        $scope.search1 = "";
        $scope.search2 = "";
        $scope.searchthird = "";
        $scope.termsdisable = false;
        $scope.search14 = "";
        $scope.termduadatestab = true;
        var name = "";
        //extra
        $scope.To_Month = '';
        $scope.from_Month = '';
        $scope.accyr = false;
        $scope.trmdisa = false;
        $scope.yeardisable = false;
        $scope.loaddata = function () {
            $scope.termsdisable = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = paginationformasters;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = paginationformasters;
            $scope.currentPagethird1 = 1;
            $scope.itemsPerPagethird1 = paginationformasters;
            $scope.currentPagefourth = 1;
            $scope.itemsPerPagefourth = paginationformasters;

            $scope.currentPagenew = 1;
            $scope.itemsPerPagenew = paginationformasters;
            $scope.page = "page";
            $scope.page1 = "page1";
            $scope.page2 = "page2";
        //    $scope.page3 = "page3";
            $scope.Page4 = "page4";
            $scope.page31 = "page31";

            var pageid = 2;
            apiService.getURI("FeeMasterTerms/getalldetails", pageid).
        then(function (promise) {            
            $scope.pages = promise.feetermsarray;
            $scope.arrlist6 = promise.feetermsarrayDrop;
            $scope.arrlist7 = promise.hdnames;
            $scope.arrlistchk = promise.insnames;
            $scope.termsmaping = promise.dataretrive;
            $scope.arrlist9 = promise.feetermsarray;
            $scope.arrlist11 = promise.hdnames;
            $scope.arrlist8 = promise.academicdrp;
            $scope.saveddatadisplay = promise.duadateget;
            $scope.totcountfirst = $scope.pages.length;
            $scope.totcountsecond = $scope.termsmaping.length;

            //$scope.masteperiodarray = promise.masteperiodarray;
            $scope.masteperiodarraydetails = promise.masteperiodarray;
            $scope.asmaY_Id = promise.asmaY_ID;
            $scope.fillyear = [];
            angular.forEach($scope.arrlist8, function (dd) {
                if (dd.asmaY_Id === $scope.asmaY_Id) {
                    var yearname = dd.asmaY_Year;
                    var yearname1 = yearname.split('-');
                    $scope.fillyear.push({ fmtP_Year: yearname1[0] });
                    $scope.fillyear.push({ fmtP_Year: yearname1[1] });
                }
            });
                })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
            $scope.sort1 = function (keyname) {
                $scope.sortKey1 = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
            }
            $scope.sortthird = function (keyname) {
                $scope.sortKeythird = keyname;   //set the sortKey to the param passed
                $scope.reversethird = !$scope.reversethird; //if true make it false and vice versa
            }                        //aextra
            $scope.change_year();
        }
        $scope.change_year = function () {
            $scope.fillyear = [];
            angular.forEach($scope.arrlist8, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                    var yearname = dd.asmaY_Year;
                    var yearname1 = yearname.split('-');
                    $scope.fillyear.push({ fmtP_Year: yearname1[0] });
                    $scope.fillyear.push({ fmtP_Year: yearname1[1] });
                }
            });
        }
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.ftI_NAME).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }       
        $scope.cance = function () {
            $scope.fmT_Id = 0;
            $scope.FMT_Name = "";
            $scope.FMT_IncludeArrearFeeFlg = false;
            $scope.submitted = false;
            $scope.myform1.$setPristine();
            $scope.myform1.$setUntouched();
            $scope.search1 = "";
        }
        //extra
        $scope.cancell = function () {           
            $scope.change_year();
            $scope.accyr = false;
            $scope.trmdisa = false;
            $scope.yeardisable = false;
            $scope.fmtP_Id = 0;
            $scope.submitted4 = false;
            $scope.myform4.$setPristine();
            $scope.myform4.$setUntouched();
            $scope.To_Month = '';
            $scope.from_Month = '';
            $scope.loaddata();
        }
        $scope.submitted = false;
        $scope.savedata = function () {            
            if ($scope.myform1.$valid) {

                var arrflg = $scope.FMT_IncludeArrearFeeFlg
                if (arrflg == true) {
                    $scope.FMT_IncludeArrearFeeFlg = 1;
                }
                else {
                    $scope.FMT_IncludeArrearFeeFlg = 0;
                }

                var data = {
                    "FMT_Id": $scope.fmT_Id,
                    "FMT_Name": $scope.FMT_Name,
                    "FMT_ActiveFlag": true,
                    "FMT_IncludeArrearFeeFlg": $scope.FMT_IncludeArrearFeeFlg
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("FeeMasterTerms/", data).
                then(function (promise) {
                    if (promise.returnduplicatestatus === 'Duplicate' && promise.returnval === false) {
                        swal('Record Already Exist');
                    }
                    if (promise.returnduplicatestatus == "Duplicate") {
                        swal('Record Already Exist');
                     
                        $state.reload();
                    }
                    else if (promise.returnduplicatestatus == "Save") {
                       
                        swal('Record Saved Successfully');
                        // $scope.loaddata();
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
                    else if (promise.returnvalue == "NotUpdate") {                       
                        swal('Record Not Updated');                     
                        $state.reload();
                    }
                  
                    else {
                        $scope.pages = promise.feetermsarray;

                        if (promise.status != null) {
                            swal('Record Not Updated', 'success');
                        }
                        else {
                            swal('Record Not Saved', 'success');
                        }                      
                        $state.reload();
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };
        //extra addedd
        $scope.submitted4 = false;
        $scope.savedatafourth = function () {
            $scope.submitted4 = true;
            if ($scope.myform4.$valid) {
                var data = {
                    "FMT_Id": $scope.fmT_Ids,
                    "FMTP_Year": $scope.fmtP_Years,
                    "FMTP_FROM_MONTH": $scope.from_Month,
                    "FMTP_TO_MONTH": $scope.To_Month,
                    "ASMAY_ID": $scope.asmaY_Id,
                    "FeeFlag": $scope.rdopunch,
                    "FMTP_Id": $scope.fmtP_Id
                }
                apiService.create("FeeMasterTerms/savedetailfourth", data).
                    then(function (promise) {
                        $scope.loaddata();
                        $scope.accyr = false;
                        $scope.trmdisa = false;
                        $scope.yeardisable = false;
                        $scope.change_year();
                        if (promise.returnduplicatestatus == 'Save') {
                            swal('Data successfully Saved');
                            $scope.myform4.$setPristine();
                            $scope.myform4.$setUntouched();
                            $scope.To_Month = '';
                            $scope.from_Month = '';
                            $scope.loaddata();
                        }
                        if (promise.returnduplicatestatus == 'RecordExist') {
                            swal('Record Already Exists !');
                           
                        }                      
                        else if (promise.returnduplicatestatus == 'NotSave') {
                            swal('Data Not Saved');
                        }
                        else if (promise.returnduplicatestatus == 'NotUpdate') {
                            swal('Data Not Update !');
                        }
                        else if (promise.returnduplicatestatus == 'Update') {
                            swal('Data Update SuccessFully !');
                            $scope.myform4.$setPristine();
                            $scope.myform4.$setUntouched();
                            $scope.To_Month = '';
                            $scope.from_Month = '';
                            $scope.loaddata();
                        }
                    })               
            }
            else {
                $scope.submitted = true;
            }
        };
        //interacted4
        $scope.interacted4 = function (field) {
            return $scope.submitted4 || field.$dirty;
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.searchsource = function () {
            var entereddata = $scope.search;
            var data = {
                "FMT_Name": $scope.search,
                "FMT_Name": $scope.type
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeMasterTerms/1", data).
        then(function (promise) {
            $scope.pages = promise.feetermsarray;
            swal("searched Successfully");
        })
        }
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fmT_Id;
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
                    apiService.DeleteURI("FeeMasterTerms/deletepages", pageid).
                    then(function (promise) {
                        $scope.pages = promise.feetermsarray;
                        if (promise.returnduplicatestatus === 'Already' && promise.returnval === false) {
                            swal('Record Already Mapped', 'Term And Head ');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal('Record Not Deleted');
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.fmT_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("FeeMasterTerms/getdetails", pageid).
            then(function (promise) {
                $scope.FMT_Name = promise.feetermsarray[0].fmT_Name;
                $scope.fmT_Id = promise.feetermsarray[0].fmT_Id;

                if (promise.feetermsarray[0].fmT_IncludeArrearFeeFlg == true) {
                    $scope.FMT_IncludeArrearFeeFlg = true;
                }
                else {
                    $scope.FMT_IncludeArrearFeeFlg = false;
                }

            })
        }
        $scope.deactive = function (feetermsarray) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            var confirmmgs = "";
            if (feetermsarray.fmT_ActiveFlag == true) {
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
            apiService.create("FeeMasterTerms/deactivate", feetermsarray).
            then(function (promise) {
                if (promise.returnval == true) {
                    swal("Record " + confirmmgs + " Successfully");
                }
                else {
                    //swal("Record " + confirmmgs + " Successfully");
                    swal("Record is already been used !!!");
                }
                $scope.loaddata();
            })
            }
                else {
                    swal("Record " + mgs + " Cancelled");
                }
        });
        }    
        $scope.submitted1 = false;
        $scope.savetermsmap = function (arrlistchk) {           
            if ($scope.myform2.$valid) {
                $scope.albumNameArray = [];
                angular.forEach($scope.arrlistchk, function (role) {
                    if (!!role.selected) $scope.albumNameArray.push(role);
                })
                $scope.albumNameArrayhd = [];
                angular.forEach($scope.arrlist7, function (role) {
                    if (!!role.selected) $scope.albumNameArrayhd.push(role);
                })
                if ($scope.albumNameArray.length === 0) {
                    swal('Please Select Installment From List Atleast One....!')
                }
                else if ($scope.albumNameArrayhd.length === 0) {
                    swal('Please Select Head From List Atleast One....!')
                }
                else {
                    var data = {
                        "FMTFH_Id": $scope.fmtfH_Id,
                        "FMT_Id": $scope.fterm,
                       // "FMH_Id": $scope.hname,
                        "TempararyArrayList": $scope.albumNameArray,
                        "TempararyArrayListhd": $scope.albumNameArrayhd,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeMasterTerms/savedetailY", data).
                    then(function (promise) {
                        if (promise.returnduplicatestatus == 'Duplicate' && promise.returnval == false) {
                            swal("Record Already Exist");
                        }
                        else if (promise.returnduplicatestatus === "Save") {
                            swal('Record Saved/Updated Successfully');
                            $scope.termsmaping = promise.dataretrive;
                            $scope.termsdisable = false;
                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        else if (promise.returnduplicatestatus === "NotSave") {
                            swal('Record Not Saved');
                            $scope.termsmaping = promise.dataretrive;

                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        else if (promise.returnduplicatestatus === "Update") {
                            swal('Record Saved/Updated Successfully');
                            $scope.termsmaping = promise.dataretrive;
                            $scope.termsdisable = false;
                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }
                        else if (promise.returnduplicatestatus === "NotUpdate") {
                            swal('Record Not Updated');
                            $scope.termsmaping = promise.dataretrive;
                            $scope.cance1($scope.arrlistchk, $scope.arrlist7);
                        }                        
                        else {
                            swal('Record Not Saved/Updated');
                        }
                    })
                }
            }
            else {
                $scope.submitted1 = true;
            }
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1 || field.$dirty;
        };
        $scope.cance1 = function (arrlistchk, arrlist7) {
            $scope.fmtfH_Id = 0;
            $scope.fterm = "";
            $scope.hname = "";
            for (var i = 0; i < $scope.arrlistchk.length; i++) {
                name = arrlistchk[i].selected
                if (name == true) {
                    arrlistchk[i].selected = false;
                }
            }
            for (var i = 0; i < $scope.arrlist7.length; i++) {
                name = arrlist7[i].selected
                if (name == true) {
                    arrlist7[i].selected = false;
                }
            }
            $scope.submitted1 = false;
            $scope.myform2.$setPristine();
            $scope.myform2.$setUntouched();
            $scope.search2 = "";
        };
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
            apiService.create("FeeMasterTerms/1", data).
        then(function (promise) {
            $scope.loaddata();
            $scope.termsmaping = promise.retrivefeeHeadData;

            swal("searched Successfully");
        })
        }
        $scope.editEmployeeY = {}
        $scope.deletedataY = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.idforedt;
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
                    var data = {
                        "FMTFH_Id": pageid
                    }
                    apiService.create("FeeMasterTerms/deletepagesY", data).
                    //apiService.DeleteURI("FeeMasterTerms/deletepagesY", pageid).

                        then(function (promise) {
                        $scope.loaddata();
                        $scope.termsmaping = promise.retrivefeeHeadData;
                        if (promise.returnduplicatestatus === 'Already' && promise.returnval === false) {
                            swal('Record Already Mapped');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                        }
                        else {
                            swal("Record is already been used !!!");
                        }
                    })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        //extta
        $scope.deletedataYTTTTT = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fmtP_Id;
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
                        apiService.DeleteURI("FeeMasterTerms/DeleteYss", pageid).
                            then(function (promise) {
                                $scope.accyr = false;
                                $scope.trmdisa = false;
                                $scope.yeardisable = false;
                                $scope.myform4.$setPristine();
                                $scope.myform4.$setUntouched();
                                $scope.To_Month = '';
                                $scope.from_Month = '';
                                $scope.loaddata();

                                if (promise.returnduplicatestatus === 'NotRemove') {
                                    swal('Record Not Deleted');
                                }
                                else if (promise.returnduplicatestatus == 'Remove') {
                                    swal('Record Deleted Successfully');
                                }

                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }
        //close
        $scope.getorgvalueY = function (employee, arrlistchk) {
            $scope.editEmployeeY = employee.idforedt;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeMasterTerms/getdetailsY", pageid).
            then(function (promise) {
                $scope.termsdisable = true;
                $scope.grigview2 = true;
                for (var i = 0; i < $scope.arrlistchk.length; i++) {
                    if ($scope.arrlistchk[i].ftI_ID == promise.dataretrive[0].ftI_Id) {
                        $scope.arrlistchk[i].selected = true
                    }
                    else {
                        $scope.arrlistchk[i].selected = false;
                    }
                }
                $scope.fmtfH_Id = promise.dataretrive[0].fmtfH_Id;
                $scope.fterm = promise.dataretrive[0].fmT_Id;
                $scope.hname = promise.dataretrive[0].fmH_Id;
                angular.forEach($scope.arrlist7, function (pp) {
                    if (pp.fmH_Id == promise.dataretrive[0].fmH_Id) {
                        pp.selected = true;
                    }
                    else {
                        pp.selected = false;
                    }
                })

            })
        }
        //for deactive avtive 
        //$scope.deactiveY = function (retrivefeeHeadData) {
        //    var config = {
        //        headers: {
        //            'Content-Type': 'application/json;'
        //        }
        //    } 
        //    var mgs = "";
        //    var confirmmgs = "";
        //    if (retrivefeeHeadData.fmT_ActiveFlag == 0) {
        //        mgs = "Deactive";
        //        confirmmgs = "Deactivated";
        //    }
        //    else {
        //        mgs = "Active";
        //        confirmmgs = "Activated";
        //    }
        //    swal({
        //        title: "Are you sure",
        //        text: "Do you want to " + mgs + " record??????",
        //        type: "warning",
        //        showCancelButton: true,
        //        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
        //        cancelButtonText: "Cancel",
        //        closeOnConfirm: false,
        //        closeOnCancel: false
        //    },
        //function (isConfirm) {
        //    if (isConfirm) {
        //    apiService.create("FeeMasterTerms/deactivateY", retrivefeeHeadData).
        //    then(function (promise) {
        //        $scope.loaddata();
        //        $scope.students = promise.retrivefeeHeadData;
        //        if (promise.retrivefeeHeadData.returnval == 0) {
        //            swal("Activated successfully.Kindly contact Administrator");
        //        }
        //        else {
        //            swal("Data Saved/Updated successfully");
        //        } $scope.loaddata();
        //    })
        //} else {
        //    swal("Record " + mgs + " Cancelled");
        //}
        //});
        //}

        //$scope.cancethird = function () {
        //    $state.reload();
        //}

        //third tab
        $scope.cancethird = function () {
            $scope.termsmapin = "";
            $scope.fmTIdT = "";
            $scope.ASMAYIdT = "";
            $scope.fmHIdT = "";
            $scope.acyrdisabl = false;
            $scope.trmdisabl = false;
            $scope.fhdisabl = false;
            $scope.fmtfhdD_Id = 0;
            $scope.grigview2 = false;
            $scope.submitted2 = false;
            $scope.myform123.$setPristine();
            $scope.myform123.$setUntouched();
            $scope.search3 = "";
        };
        $scope.interacted2 = function (field) {

            return $scope.submitted2 || field.$dirty;
        };
        $scope.BindGrid = function (arrlist9, arrlist11) {
            $scope.grigview2 = true;
            var data = {

                "ASMAY_Id": $scope.ASMAYIdT,  // academic year
                "FMT_Id": $scope.fmTIdT,   // term id
                "FMH_Id": $scope.fmHIdT,   // head id
            };
            apiService.create("FeeMasterTerms/Getduedates", data).
                then(function (promise) {
                    if (promise.length > 0) {
                        $scope.termsmapin = promise;
                    }
                    else {
                        swal('Installments are not mapped for your selection ');
                    }

                })

        };
        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();
            if (new Date(FromDate) > new Date(ToDate)) {
                swal('To Date should be greater than from date');

                return false;
            }
        };
        $scope.submitted2 = false;
        $scope.savedatadues = function (termsmapin) {
            $scope.albumNameArraynewone = [];
            var chkDatecondition = '';
            var fdate = '';
            var tdate = '';

            if ($scope.myform123.$valid) {

                for (var i = 0; i < $scope.termsmapin.length; i++) {
                    var curDate = new Date();
                    fdate = termsmapin[i].fmtfhdD_FromDate;
                    tdate = termsmapin[i].fmtfhdD_ToDate;
                    if (new Date(fdate) > new Date(tdate)) {
                        chkDatecondition = 'NotAllowed';
                    }
                    else {
                        chkDatecondition = 'Allowed';
                    }
                }
                if (chkDatecondition === 'NotAllowed') {
                    swal('To Date should be greater than from date');
                }
                else {
                    for (var i = 0; i < $scope.termsmapin.length; i++) {
                        var f = new Date(termsmapin[i].fmtfhdD_FromDate).toDateString();
                        var t = new Date(termsmapin[i].fmtfhdD_ToDate).toDateString();
                        var a = new Date(termsmapin[i].fmtfhdD_ApplicableDate).toDateString();
                        var d = new Date(termsmapin[i].fmtfhdD_DueDate).toDateString();
                        termsmapin[i].fmtfhdD_FromDate = f;
                        termsmapin[i].fmtfhdD_ToDate = t;
                        termsmapin[i].fmtfhdD_ApplicableDate = a;
                        termsmapin[i].fmtfhdD_DueDate = d;
                        $scope.albumNameArraynewone.push(termsmapin[i]);
                    }
                    var data = {
                        "mainid": $scope.fmtfhdD_Id,
                        "temyrid": $scope.ASMAYIdT,
                        "FMT_Id": $scope.fmTIdT,
                        "FMH_Id": $scope.fmHIdT,
                        "feetfhddd": $scope.albumNameArraynewone,
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    apiService.create("FeeMasterTerms/savedetailDDD", data).
                    then(function (promise) {
                        if (promise.returnvalue === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Record Already Exist');
                        }
                        else if (promise.returnvalue === false) {
                            swal('Record Not Saved/Updated Successfully');
                        }
                        else {
                            swal('Record Saved/Updated Successfully');
                            $scope.loaddata();
                            $scope.cance2();
                        }
                    })
                }
            }
            else {
                $scope.submitted2 = true;
            }
        };
        $scope.editEmployeeY = {}
        $scope.deletedataYTTTT = function (employee, SweetAlert) {
            $scope.editEmployeeY = employee.fmthddid;
            var iiiiii = $scope.editEmployeeY;
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
                    apiService.DeleteURI("FeeMasterTerms/deletepagesthird", iiiiii).
                    then(function (promise) {
                        $scope.saveddatadisplay = promise.duadateget;
                        if (promise.returnduplicatestatus === 'Already' && promise.returnval === false) {
                            swal('Record Already Mapped', 'Term Duadates');
                        }
                        else if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                            $scope.cance2();
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
        }
        $scope.getorgvalueDYTTTT = function (employee) {
            $scope.editEmployeeY = employee.fmthddid;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeMasterTerms/getdetailsDYthird", pageid).
            then(function (promise) {
                $scope.grigview2 = true;
                $scope.fmtfhdD_Id = promise.arrduadates[0].fmthddid;
                $scope.fmHIdT = promise.arrduadates[0].fmh;
                $scope.fmTIdT = promise.arrduadates[0].fmt;
                $scope.acyrdisabl = true;
                $scope.trmdisabl = true;
                $scope.fhdisabl = true;
                $scope.fmT_Id = promise.arrduadates[0].fmt;
                $scope.fmH_Id = promise.arrduadates[0].fmh;
                $scope.ASMAYIdT = promise.arrduadates[0].asyid;
                $scope.myArrayR = [];
                angular.forEach(promise.arrduadates, function (user) {
                    var frdate = new Date(promise.arrduadates[0].fdate1);
                    user.fmtfhdD_FromDate = frdate;
                    var frtdate = new Date(promise.arrduadates[0].tdate1);
                    user.fmtfhdD_ToDate = frtdate;
                    var fradate = new Date(promise.arrduadates[0].aplc1);
                    user.fmtfhdD_ApplicableDate = fradate;
                    var frddate = new Date(promise.arrduadates[0].ddate1);
                    user.fmtfhdD_DueDate = frddate;
                    user.fmtfH_Id = promise.arrduadates[0].FMTFHNew;
                    user.insname = promise.arrduadates[0].termname;
                    $scope.myArrayR.push(user);
                })
                $scope.termsmapin = $scope.myArrayR;
            })
        }
        //extra
        $scope.getmasterDYTTTT = function (employee) {
            $scope.editEmployeeY = employee.fmtP_Id;
            var pageid = $scope.editEmployeeY;
            apiService.getURI("FeeMasterTerms/getdetailsDYfourth", pageid).
                then(function (promise) {
                    $scope.fmtP_Id = promise.masterdit[0].fmtP_Id;
                    $scope.asmaY_Id = promise.masterdit[0].asmaY_ID;
                    $scope.fmT_Ids = promise.masterdit[0].fmT_Id;
                    $scope.fmtP_Years = promise.masterdit[0].fmtP_Year;
                    $scope.rdopunch = promise.masterdit[0].feeFlag;
                    $scope.To_Month = promise.masterdit[0].fmtToMonth;
                    $scope.from_Month = promise.masterdit[0].fromMonth;
                    $scope.accyr = true;
                    $scope.trmdisa = true;
                    $scope.yeardisable = true;
                    $scope.change_year();

                })
        }
        //close
        $scope.isOptionsRequired = function () {

            return !$scope.arrlistchk.some(function (options) {
                return options.selected;
            });
        }
        $scope.isOptionsRequiredhd = function () {
            return !$scope.arrlist7.some(function (options) {
                return options.selected;
            });
        }
    }
})();
