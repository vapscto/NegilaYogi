(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGFeeGroupwiseRecieptController', CLGFeeGroupwiseRecieptController)
    CLGFeeGroupwiseRecieptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function CLGFeeGroupwiseRecieptController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var headid;
        var ftiid;

        $scope.finslab1 = [];
        $scope.finslab2 = [];
        $scope.reciptno = false;
        $scope.dateclick = false;
        $scope.feereceipt = false;
        $scope.frdt = false;
        $scope.tdt = false;

        $scope.FMCB_fromdt = new Date();
        $scope.FMCB_toDt = new Date();

        $scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
        { value: "Day Slab", FTFS_FineType: "Day Slab" }
        ];

        $scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
        { value: "Day Slab", FTFSE_FineType: "Day Slab" }
        ];

        $scope.typeofstudentregular = false;

        $scope.sortKey = "fmA_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchthird = "";
        $scope.ecsmonthdisable = false;
        $scope.ecsdatedisable = false;
        $scope.commonamountflag = [];
        $scope.route = false;
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        if (configsettings.length > 0) {
            var ecsflag = configsettings[0].ispaC_ECSFlag;
        }
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.categoryshow = true;
        $scope.changeradio = function (abcc) {
            $scope.grigview1 = false;
            $scope.FMG_Id = "";
            $scope.FMCC_Id = "";
            $scope.ASMAY_Id = "";
            if (abcc == 'stfoth' || abcc == 'stfothamt') {
                $scope.categoryshow = false;
            }
            else if (abcc == 'stud') {
                $scope.categoryshow = true;
            }
            var data = {
                "selectiontype": abcc
            }
            apiService.create("CLGFeeGroupwiseReciept/getalldetailsOnselectiontype", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;

                })
        }

        var amount;
        $scope.commonamount = function (totalgrid, amount, headidfrongrid) {


            for (var j = 0; j <= $scope.commonamountflag.length; j++) {
                var commflagheadid = $scope.commonamountflag[j].fmH_Id;
                for (var i = 0; i <= totalgrid.length; i++) {
                    var headid = totalgrid[i].fmH_Id
                    if (headidfrongrid == commflagheadid) {
                        amount = amount;
                    }
                    else {
                        break;
                    }

                    if (commflagheadid == headid) {
                        totalgrid[i].fcmaS_Amount = amount;
                    }
                }
            }

        }

        // $scope.selected = {};


        //$scope.namesecs = [{ value: 1, FTDDE_Month: "January" },
        //                  { value: 2, ftddE_Month: "February" },
        //                   { value: 3, FTDDE_Month: "March" },
        //                    { value: 4, FTDDE_Month: "April" },
        //                     { value: 5, FTDDE_Month: "May" },
        //                      { value: 6, FTDDE_Month: "June" },
        //                       { value: 7, FTDDE_Month: "July" },
        //                        { value: 8, FTDDE_Month: "August" },
        //                         { value: 9, FTDDE_Month: "September" },
        //                          { value: 10, FTDDE_Month: "October" },
        //                           { value: 11, FTDDE_Month: "November" },
        //                            { value: 12, FTDDE_Month: "December" },
        //];

        var ASMAY_Year;
        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            $scope.route_id();
            apiService.getURI("CLGFeeGroupwiseReciept/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;
                    $scope.ASMAY_Id = promise.currfillyear[0].asmaY_Id;
                    $scope.Fillcourse = promise.fillcourse;
                    $scope.groupcount = promise.fillgroup;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    $scope.installlst = promise.fillinstallment;

                    $scope.grouplstedit = promise.fillmastergroup;
                    $scope.headlstedit = promise.fillmasterhead;
                    $scope.installlstedit = promise.fillinstallment;
                    //$scope.totcountfirst = $scope.thirdgrid.length;

                })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }

        $scope.addNew = function (totalgrid) {
            debugger;
            $scope.totalgrid.push({
                'FMH_Id': totalgrid.FMH_Id,
                'FMI_Id': totalgrid.FMI_Id,
                'FYGHM_FineApplicableFlag': totalgrid.FYGHM_FineApplicableFlag,
                'FYGHM_Common_AmountFlag': totalgrid.FYGHM_Common_AmountFlag,
                'FYGHM_ActiveFlag': totalgrid.FYGHM_ActiveFlag,
            });
            $scope.PD = {};
        };


        $scope.selectacademicyear = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeGroupwiseReciept/selectacademicyear", data).
                then(function (promise) {
                    $scope.Fillcourse = promise.fillcourse;
                    $scope.grouplst = promise.fillmastergroup;
                    $scope.headlst = promise.fillmasterhead;
                    $scope.installlst = promise.fillinstallment;

                    $scope.grouplstedit = promise.fillmastergroup;
                    $scope.headlstedit = promise.fillmasterhead;
                    $scope.installlstedit = promise.fillinstallment;


                })
        }
      
        $scope.allstudentcheck = function () {

            angular.forEach($scope.fillstudent, function (ff) {
                if ($scope.allstdcheck == true) {
                    ff.checkedgrplst1 = true;
                }
                else {
                    ff.checkedgrplst1 = false;
                }
            });
            $scope.firstfnc1();
        };


        $scope.allreceiptcheck = function () {
            if ($scope.reciptno === true) {
                angular.forEach($scope.installmentcount, function (ff) {
                    if ($scope.allrecptchk == true) {
                        ff.receipt = true;
                    }
                    else {
                        ff.receipt = false;
                    }
                })
            }
            else {
                angular.forEach($scope.installmentcount, function (rr) {
                    rr.receipt = false;

                })
            }

        }

        $scope.receiptClick = function () {
            angular.forEach($scope.installmentcount, function (rr) {
                rr.receipt = false;

            })
            $scope.allrecptchk = false;
        };

        $scope.firstfnc1 = function (aa) {

            $scope.allstdcheck = $scope.fillstudent.every(function (itm) { return itm.checkedgrplst1; });


            $scope.valsgroup = [];
            $scope.valshead = [];
            $scope.valsstd = [];

            for (var t = 0; t < $scope.grouplst.length; t++) {
                if ($scope.grouplst[t].checkedgrplst == true) {
                    $scope.valsgroup.push($scope.grouplst[t]);
                }
            }

            for (var u = 0; u < $scope.headlst.length; u++) {
                if ($scope.headlst[u].checkedheadlst == true) {
                    $scope.valshead.push($scope.headlst[u]);
                }
            }

            for (var u = 0; u < $scope.fillstudent.length; u++) {
                if ($scope.fillstudent[u].checkedgrplst1 == true) {
                    $scope.valsstd.push($scope.fillstudent[u]);
                }
            }
            var data = {
                "ACMS_Id": $scope.ACMS_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                savegrplst: $scope.valsgroup,
                saveheadlst: $scope.valshead,
                studentdata: $scope.valsstd,
            }

            apiService.create("CLGFeeGroupwiseReciept/getreceipt", data).
                then(function (promise) {

                    $scope.installmentcount = promise.receiptcount;

                })

        }


        $scope.allgroupcheck = function () {
            if ($scope.allcheck == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    obj.checkedgrplst = true;
                    angular.forEach($scope.headlst, function (obj1) {
                        if (obj1.fmG_Id == obj.fmG_Id) {
                            obj1.checkedheadlst = true;
                            //angular.forEach($scope.installlst, function (obj2) {
                            //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                            //        obj2.checkedinstallmentlst = true;
                            //    }
                            //});
                        }
                    });

                });
            }
            else {
                angular.forEach($scope.grouplst, function (obj) {
                    obj.checkedgrplst = false;
                    angular.forEach($scope.headlst, function (obj1) {
                        if (obj1.fmG_Id == obj.fmG_Id) {
                            obj1.checkedheadlst = false;
                            //angular.forEach($scope.installlst, function (obj2) {
                            //    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                            //        obj2.checkedinstallmentlst = false;
                            //    }
                            //});
                        }
                    });

                });
            }

        }


        //==========Select for selected Grouplst,headlst,installment data for store record(for save button)================//
        $scope.firstfnc = function (vlobj) {

            if (vlobj.checkedgrplst == true) {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = true;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = true;
                                    }
                                });
                            }
                        });
                    }
                });
            } else {
                angular.forEach($scope.grouplst, function (obj) {
                    if (vlobj.fmG_Id == obj.fmG_Id) {
                        angular.forEach($scope.headlst, function (obj1) {
                            if (obj1.fmG_Id == obj.fmG_Id) {
                                obj1.checkedheadlst = false;
                                angular.forEach($scope.installlst, function (obj2) {
                                    if (obj.fmG_Id == obj2.fmG_Id && obj1.fmH_Id == obj2.fmH_Id) {
                                        obj2.checkedinstallmentlst = false;
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }

        $scope.secfnc = function (vlobj1) {

            //if (vlobj1.checkedheadlst == true) {
            //    angular.forEach($scope.headlst, function (val) {
            //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
            //            angular.forEach($scope.installlst, function (val1) {
            //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
            //                    val1.checkedinstallmentlst = true;
            //                }
            //            });
            //        }
            //    });
            //} else {

            //    angular.forEach($scope.headlst, function (val) {
            //        if (vlobj1.fmG_Id == val.fmG_Id && vlobj1.fmH_Id == val.fmH_Id) {
            //            angular.forEach($scope.installlst, function (val1) {
            //                if (val1.fmH_Id == val.fmH_Id && val1.fmG_Id == val.fmG_Id) {
            //                    val1.checkedinstallmentlst = false;
            //                }
            //            });
            //        }
            //    });
            //}

            for (var s = 0; s < $scope.grouplst.length; s++) {
                if (vlobj1.fmG_Id == $scope.grouplst[s].fmG_Id) {
                    for (var t = 0; t < $scope.headlst.length; t++) {
                        if (vlobj1.fmG_Id == $scope.headlst[t].fmG_Id) {
                            if ($scope.headlst[t].checkedheadlst == false) {
                                $scope.grouplst[s].checkedgrplst = false;
                            }
                            else {
                                $scope.grouplst[s].checkedgrplst = true;
                                break;
                            }
                        }
                    }
                }
            }
        }






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
        $scope.FMCC_Id = "";
        $scope.FMG_Id = "";
        // $scope.selected = {};
        $scope.regslbary = [];
        $scope.ecsslbary = [];


        $scope.onsemselection = function () {
            $scope.ACMS_Id = '';
            $scope.fillsection = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_ID": $scope.AMB_Id,
                "AMB_ID": $scope.AMSE_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeGroupwiseReciept/onsemselection", data).
                then(function (promise) {


                    if (promise.fillsection != null) {
                        $scope.fillsection = promise.fillsection;
                    }
                    //else {
                    //    swal("No Records found.!!")
                    //}
                })

        };
        $scope.onselectsec = function () {
            $scope.fillstudent = [];
            if ($scope.ACMS_Id != '' && $scope.ACMS_Id != null && $scope.ACMS_Id != undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_ID": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeGroupwiseReciept/onselectsec", data).
                    then(function (promise) {


                        if (promise.fillstudent != null) {
                            $scope.fillstudent = promise.fillstudent;



                        }
                        //else {
                        //    swal("No Records found.!!")
                        //}
                    })
            }


        };
        $scope.onselectcourse = function (amcoid) {


            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": amcoid
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeGroupwiseReciept/selectcourse", data).
                then(function (promise) {


                    if (promise.fillbranch.length > 0) {
                        $scope.Branch = promise.fillbranch;
                    }
                    //else {
                    //    swal("No Records found.!!")
                    //}
                })

        };

        $scope.onselectbranch = function (branchid) {


            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_ID": branchid
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeGroupwiseReciept/selectbran", data).
                then(function (promise) {

                    if (promise.fillsemester.length > 0) {

                        $scope.fillsemester = promise.fillsemester;
                    }
                    //else {
                    //    swal("No Records found.!!")
                    //}
                })

        };

        $scope.editEmployee = {}
        $scope.DeletRecord = function (amountid, selectiontype) {
            //  $scope.editEmployee = FMA_Id;
            //  var orgid = $scope.editEmployee;
            var data = {
                "FMA_Id": amountid,
                "selectiontype": selectiontype
            }
            var amtid = amountid;
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
                        apiService.create("CLGFeeGroupwiseReciept/Deletedetails/", data).
                            then(function (promise) {

                                // $scope.thirdgrid = promise.alldata;

                                if (promise.returnval == "true") {
                                    swal('Record Deleted Successfully');
                                }
                                else if (promise.returnval == "false") {
                                    swal('Contact Administrator');
                                }
                                else if (promise.returnval == "RecordExists") {
                                    swal('Data has already been used in Transactions!! So record cannot be deleted');
                                }
                                $scope.formload();

                                // $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
            //})
        }

        $scope.cleardata = function () {
            $state.reload();
            //$scope.searchthird = "";
            //$scope.ASMAY_Id = "";
            //$scope.FMG_Id = "";
            //$scope.FMCC_Id = "";
            //$scope.grigview1 = false;
        }

        $scope.slab1 = true;
        $scope.slab2 = true;

        var regval = "";
        var ecsval = "";
        $scope.cancel = function () {
            $state.reload();

        }
        $scope.cancell = function () {
            $state.reload();
            //$scope.typeofstudentregular = false;
            //$scope.typeofstudentecs = false;
            //$scope.finslab1 = [];
            //$scope.finslab2 = [];
            //$scope.slab1 = false;
            //$scope.slab2 = false;
        }
        $scope.clicktypeofstudent = function () {

            var typeofcatreg = $scope.typeofstudentregular
            var typeofcatecs = $scope.typeofstudentecs

            if (typeofcatreg == true) {
                $scope.slab1 = true;
                regval = "R";
            }
            else {
                $scope.slab1 = false;
            }
            if (typeofcatecs == true) {
                $scope.slab2 = true;
                ecsval = "E"
            }
            else {
                $scope.slab2 = false;
            }

            var data = {
                "regularfalg": regval,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeGroupwiseReciept/fillslabdetails", data).
                then(function (promise) {

                    $scope.finslab1 = promise.fillslab
                })

        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("CLGFeeGroupwiseReciept/Editdetails", orgid).
                then(function (promise) {
                    $scope.addnewbtn = false;
                    $scope.FMG_Id = promise.alldata[0].fmG_Id;
                    $scope.totalgrid.headcount.selected.FMH_Id = promise.alldata[0].fmH_Id;
                    $scope.totalgrid.installmentcount.FMI_Id = promise.alldata[0].fmI_Id;
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

            apiService.create("CLGFeeGroupwiseReciept/1", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    swal("searched Successfully");
                })
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };


        $scope.submitted = false;
        $scope.savedata = function (totalgrid, finslab1, finslab2) {

            $scope.submitted = true;

            if ($scope.myform.$valid) {

                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    savetmpdata: totalgrid,
                    savefineslabreg: $scope.savedatatrans1,
                    savefineslabecs: $scope.savedatatrans2,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeGroupwiseReciept/savedata", data).
                    then(function (promise) {

                        if (promise.returnval != "false") {
                            if (promise.returnval == "used") {
                                $scope.addnewbtn = true;
                                swal('Record already Used');
                            }
                            else {
                                $scope.addnewbtn = true;
                                swal('Record ' + promise.amtentrystatus + ' Successfully');
                            }

                        }

                        else {
                            $scope.addnewbtn = true;
                            swal('Contact Administrator');
                        }
                        $state.reload();
                    })
            }
            else {
                // swal("Please enter Valid date")
                $scope.submitted = true;
            }
        };


        $scope.totgdchkbx = function (totary, tchkbx, index) {


            if (tchkbx == true) {
                $scope.finslab1 = [];
                $scope.finslab2 = [];
                $scope.slab1 = false;
                $scope.slab2 = false;
                $scope.typeofstudentregular = false;
                $scope.typeofstudentecs = false;
                headid = totary[index].fmH_Id;
                ftiid = totary[index].ftI_Id;
                $('#myModal').modal('show');


            }
        }
        $scope.headinstallmentid = function (totalgrid, index) {

            //$scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
            //           { value: "Day Slab", FTFS_FineType: "Day Slab" }
            //];


            //$scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
            //          { value: "Day Slab", FTFSE_FineType: "Day Slab" }
            //];

            $scope.typeofstudentregular = false;

            headid = totalgrid[index].fmH_Id;
            ftiid = totalgrid[index].ftI_Id;
            if ($scope.stuchk == "stfoth" && $scope.ASMAY_Id != "") {
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "selectiontype": $scope.stuchk
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
            }
            else {
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "FMCC_Id": $scope.FMCC_Id,
                    "selectiontype": $scope.stuchk,
                    "ASMAY_Id": $scope.ASMAY_Id,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
            }
            apiService.create("CLGFeeGroupwiseReciept/getgroupmappedheads/", data).
                then(function (promise) {

                    $scope.savedatatransregular = [];
                    $scope.savedatatransecs = [];
                    var r = 0;
                    var e = 0;
                    if (promise.fineslabdetails.length > 0) {
                        $scope.slab1 = true;
                        if ($scope.stuchk == "stfoth") {
                            $scope.slab2 = false;
                        }
                        else {
                            $scope.slab2 = true;
                        }


                        var lenofarray = promise.fineslabdetails.length;

                        for (var i = 0; i < lenofarray; i++) {
                            if (promise.fineslabdetails[i].fmfS_ECSFlag == "R" && promise.fineslabdetails[i].fmH_Id == headid && promise.fineslabdetails[i].ftI_Id == ftiid) {
                                $scope.savedatatransregular[r] = promise.fineslabdetails[i]
                                $scope.savedatatransregular[r].isSelected = true;
                                r = r + 1;
                            }
                        }
                        //if (promise.fineslabdetailsecs != null) {
                        //    var lenofarrayecs = promise.fineslabdetailsecs.length;
                        //    for (var i = 0; i < lenofarrayecs; i++) {
                        //        if (promise.fineslabdetailsecs[i].fmfS_ECSFlag == "E" && promise.fineslabdetailsecs[i].fmH_Id == headid && promise.fineslabdetailsecs[i].ftI_Id == ftiid) {
                        //            $scope.savedatatransecs[e] = promise.fineslabdetailsecs[i]
                        //            $scope.savedatatransecs[e].isSelected = true;
                        //            e = e + 1;
                        //        }
                        //    }
                        //}



                        if ($scope.savedatatransregular.length > 0) {
                            $scope.typeofstudentregular = true;
                            $scope.slab1 = true;
                            $scope.slab2 = false;
                            $scope.finslab1 = $scope.savedatatransregular;

                        }
                        if ($scope.savedatatransecs != null) {
                            if ($scope.savedatatransecs.length > 0) {
                                $scope.typeofstudentecs = true;
                                $scope.slab2 = true;
                                $scope.slab1 = true;
                                $scope.finslab2 = $scope.savedatatransecs;
                            }
                        }

                    }
                    else {
                        $scope.slab1 = false;
                        $scope.slab2 = false;
                    }
                })

        }
        $scope.savedatatrans1 = [];
        $scope.savedatatrans2 = [];
        var count = 0;
        var count1 = 0;
        $scope.addtocart = function (finslab1, finslab2) {

            var countofanarray = $scope.savedatatrans1.length;

            angular.forEach($scope.finslab1, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans1.push(user);
                    count = Number(count) + 1
                }
            })

            var countofanarrayecs = $scope.savedatatrans2.length;

            angular.forEach($scope.finslab2, function (user) {
                if (!!user.isSelected) {
                    $scope.savedatatrans2.push(user);
                    count1 = Number(count1) + 1
                }
            })

            var lenofarray = $scope.savedatatrans1.length;
            //lenofarray =Number(lenofarray) -Number(countofanarray);

            for (var i = countofanarray; i < lenofarray; i++) {
                $scope.savedatatrans1[i].FMH_Id = headid;
                $scope.savedatatrans1[i].FTI_ID = ftiid;
            }


            var lenofarrayecs = $scope.savedatatrans2.length;
            //lenofarray =Number(lenofarray) -Number(countofanarray);

            for (var i = countofanarrayecs; i < lenofarrayecs; i++) {
                $scope.savedatatrans2[i].FMH_Id = headid;
                $scope.savedatatrans2[i].FTI_ID = ftiid;
            }



            //$scope.slab1 = false;
            //$scope.slab2 = false;

            //  $scope.dismiss('addtocart');

            if (count == 0) {
                swal("Kindly select any one from Regular slab");
            }
            else {
                swal("Selected Fine Slab is added successfully");
                $scope.typeofstudentregular = "";
                $scope.typeofstudentecs = "";
            }
        }

        $scope.amountinwords = function convertNumberToWords(atotalc) {
            var words = new Array();
            words[0] = '';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            atotalc = atotalc.toString();
            var atemp = atotalc.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                atotalc = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        atotalc = n_array[i] * 10;
                    } else {
                        atotalc = n_array[i];
                    }
                    if (atotalc != 0) {
                        words_string += words[atotalc] + " ";
                    }
                    if ((i == 1 && atotalc != 0) || (i == 0 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && atotalc != 0) || (i == 2 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && atotalc != 0) || (i == 4 && atotalc != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && atotalc != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && atotalc != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }


        $scope.validdate = function (month, day, user123) {
            //$scope.monthcnt
            debugger;
            var dayno;
            if (month != "" || month != undefined) {
                angular.forEach($scope.monthcnt, function (itm1) {
                    if (itm1.ftdD_Month == month) {
                        if (month == 2) {
                            // var date = new Date();
                            //var year = date.getFullYear() -1;
                            // if (year % 4 != 0 || year % 100 == 0 && year % 400 != 0) {
                            var year_str;
                            for (var i = 0; i < ASMAY_Year.length; i++) {
                                //if (i < 4) {
                                //    if (i == 0) {
                                //        name = data[i];
                                //    } else {
                                //        name += data[i];
                                //    }
                                //}
                                if (i > 4) {
                                    if (i == 5) {
                                        year_str = ASMAY_Year[5];
                                    } else {
                                        year_str += ASMAY_Year[i];
                                    }
                                }
                            }
                            var year = Number(year_str);
                            if (year % 4 == 0 || year % 100 != 0 && year % 400 == 0) {
                                dayno = itm1.amM_Month_Max_Days + 1;
                            }
                            else {
                                dayno = itm1.amM_Month_Max_Days;
                            }
                        }
                        else {
                            dayno = itm1.amM_Month_Max_Days;
                        }
                        if (Number(day) > dayno) {
                            swal("Date Not More Than No.of Days Per Selected Month");
                            angular.forEach($scope.totalgrid, function (itm) {
                                if (itm.fmH_Id == user123.fmH_Id) {
                                    debugger;
                                    itm.fctdD_Day = "";
                                }
                            })



                        }

                    }
                })
            }
            else {
                swal("First Select Month !!!");
                $scope.fctdD_Day = "";
            }



            //$scope.fctdD_Month = month;
            //$scope.fctdD_Day = day;
            //if($scope.fctdD_Month!="" || $scope.fctdD_Month!=undefined)
            //{
            //    if ($scope.fctdD_Month == "2") {
            //        var date = new Date();
            //         var year = date.getFullYear()+1;

            //        // if (year % 4 != 0 || year % 100 == 0 && year % 400 != 0) {
            //             if (year % 4 == 0 || year % 100 != 0 && year % 400 == 0) {
            //                 if (Number($scope.fctdD_Day) > 29) {
            //                     swal("Date Not More Than No.of Days Per Selected Month");
            //                     $scope.fctdD_Day = "";
            //            }
            //        }
            //             else {
            //                 if (Number($scope.fctdD_Day) > 28) {
            //                     swal("Date Not More Than No.of Days Per Selected Month");
            //                     $scope.fctdD_Day = "";
            //                 }
            //        }
            //    }
            //    else if ($scope.fctdD_Month == "1" || $scope.fctdD_Month == "3" || $scope.fctdD_Month == "5" || $scope.fctdD_Month == "7" || $scope.fctdD_Month == "8" || $scope.fctdD_Month == "10" || $scope.fctdD_Month == "12")
            //    {
            //        if(Number($scope.fctdD_Day)>31)
            //        {
            //            swal("Date Not More Than No.of Days Per Selected Month");
            //            $scope.fctdD_Day = "";
            //        }
            //   }
            //    else if ($scope.fctdD_Month == "4" || $scope.fctdD_Month == "6" || $scope.fctdD_Month == "9" || $scope.fctdD_Month == "11") {
            //       if (Number($scope.fctdD_Day) > 30) {
            //           swal("Date Not More Than No.of Days Per Selected Month");
            //           angular.forEach($scope.totalgrid, function (itm) {
            //               if(itm.fmA_Id==user123.fmA_Id)
            //               {
            //                   itm.fctdD_Day = "";
            //               }
            //           })

            //       }
            //   }
            //}
            //else {
            //    swal("First Select Month !!!");
            //    $scope.fctdD_Day = "";
            //}
        }


        $scope.route_id = function () {

            if ($scope.route == "1") {
                $scope.trmR_Id = true;

            }
            else if ($scope.route == "0") {
                $scope.trmR_Id = false;

            }
        }


        $scope.ngdt = function () {
            if ($scope.dateclick == false) {
                $scope.frdt = false;
                $scope.tdt = false;
            }
            if ($scope.dateclick == true) {
                $scope.frdt = true;
                $scope.tdt = true;
            }
        }


        $scope.FMCC_Id = "";
        $scope.FMG_Id = "";
        $scope.regslbary = [];
        $scope.ecsslbary = [];
        $scope.fillstudent = [];
        $scope.valsgroup = [];
        $scope.valsgroup1 = [];
        $scope.valsreceipt = [];
        $scope.valsstd1 = [];
        $scope.getreceiptreport = function () {


            $scope.yearname = '';
            angular.forEach($scope.yearlst, function (kk) {
                if (kk.asmaY_Id == $scope.ASMAY_Id) {
                    $scope.yearname = kk.asmaY_Year;

                }
            })
            //$scope.monthecscnt = [];
            //$scope.grigview1 = false;
            //$scope.regslbary = [];;
            //$scope.ecsslbary = [];
            //$scope.totalgrid = [];
            //  $scope.submitted = true;

            if ($scope.myform.$valid) {
                $scope.valsgroup = [];
                $scope.valsreceipt = [];
                $scope.valshead = [];
                $scope.valsstd = [];
                $scope.valsgroup1 = [];
                $scope.valsstd1 = [];
                $scope.valsinstallment = [];
                $scope.valstudentlst = [];
                for (var t = 0; t < $scope.grouplst.length; t++) {
                    if ($scope.grouplst[t].checkedgrplst == true) {
                        $scope.valsgroup1.push($scope.grouplst[t]);
                    }
                }

                for (var u = 0; u < $scope.fillstudent.length; u++) {
                    if ($scope.fillstudent[u].checkedgrplst1 == true) {
                        $scope.valsstd1.push($scope.fillstudent[u]);
                    }
                }

                if ($scope.installmentcount != undefined) {
                    for (var u = 0; u < $scope.installmentcount.length; u++) {
                        if ($scope.installmentcount[u].receipt == true) {
                            $scope.valsreceipt.push($scope.installmentcount[u]);
                        }
                    }
                }

                if ($scope.route == "1") {
                    $scope.FYP_Id = $scope.fyP_Id;
                }
                else {
                    $scope.FYP_Id = 0;
                }




                if ($scope.valsgroup1.length > 0) {


                    for (var t = 0; t < $scope.grouplst.length; t++) {
                        if ($scope.grouplst[t].checkedgrplst == true) {
                            $scope.valsgroup.push($scope.grouplst[t]);
                        }
                    }

                    for (var u = 0; u < $scope.headlst.length; u++) {
                        if ($scope.headlst[u].checkedheadlst == true) {
                            $scope.valshead.push($scope.headlst[u]);
                        }
                    }

                    for (var u = 0; u < $scope.fillstudent.length; u++) {
                        if ($scope.fillstudent[u].checkedgrplst1 == true) {
                            $scope.valsstd.push($scope.fillstudent[u]);
                        }
                    }

                    $scope.from_date = new Date($scope.FMCB_fromdt).toDateString();

                    $scope.to_date = new Date($scope.FMCB_toDt).toDateString();

                    //var from_date = "";
                    //var to_date = "";
                    //if ($scope.FMCB_fromDATE != null && $scope.FMCB_toDATE != null) {
                    //    from_date = new Date($scope.FMCB_fromDATE).toDateString();

                    //    to_date = new Date($scope.FMCB_toDATE).toDateString();
                    //}



                    var data = {
                        "ACMS_Id": $scope.ACMS_Id,
                        "AMCO_Id": $scope.AMCO_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "AMB_Id": $scope.AMB_Id,
                        "AMSE_Id": $scope.AMSE_Id,
                        savegrplst: $scope.valsgroup,
                        saveheadlst: $scope.valshead,
                        studentdata: $scope.valsstd,
                        receiptlist: $scope.valsreceipt,
                        "Fromdate": $scope.from_date,
                        "Todate": $scope.to_date,
                        //"Fromdate": from_date,
                        //"Todate": to_date,
                        // "FYP_Id": $scope.FYP_Id
                    }

                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("CLGFeeGroupwiseReciept/getreceiptreport", data).
                        then(function (promise) {
                            if (promise.receiptdetails != null) {
                                console.log(promise.receiptdetails);
                                $scope.feereceipt = true;
                                var AMCST_AdmNo = '';
                                var AMCST_RegistrationNo = '';
                                var FYP_DOE = '';
                                var StudentName = '';
                                var AMCST_FatherName = '';
                                $scope.tempmainarray = [];
                                $scope.fiestreciept = [];
                                $scope.secondreciept = [];
                                $scope.headarray = [];
                                $scope.receiptdetails = promise.receiptdetails;

                                //angular.forEach($scope.receiptdetails, function (rr) {
                                //    $scope.valsstd.push(rr.AMCST_Id);
                                //})

                                //$scope.newarray = [];

                                //$scope.newarray = $scope.valsstd.filter(function (value, index) {
                                //    return $scope.valsstd.indexOf(value) == index
                                //});

                                $scope.valsstd = promise.studentlist;
                                angular.forEach($scope.valsstd, function (dd) {
                                    $scope.headarray = [];
                                    var tottal = 0;
                                    var sem = '';
                                    var branch = '';
                                    var course = '';
                                    var mode = '';

                                    angular.forEach($scope.receiptdetails, function (rr) {

                                        if (dd.AMCST_Id == rr.AMCST_Id) {
                                            $scope.headarray.push({ FMH_FeeName: rr.FMH_FeeName, FYP_ReceiptNo: rr.FYP_ReceiptNo, Amount: rr.Amount });
                                            AMCST_RegistrationNo = rr.AMCST_RegistrationNo;
                                            FYP_DOE = rr.FYP_DOE;
                                            AMCST_AdmNo = rr.AMCST_AdmNo;
                                            StudentName = rr.StudentName;
                                            AMCST_FatherName = rr.AMCST_FatherName;
                                            tottal += rr.Amount;
                                            mode = rr.FYPPM_TransactionTypeFlag;
                                            angular.forEach($scope.Fillcourse, function (ff) {
                                                if (rr.AMCO_Id == ff.amcO_Id) {
                                                    course = ff.amcO_CourseName;
                                                }

                                            })
                                            angular.forEach($scope.Branch, function (ff1) {
                                                if (rr.AMB_Id == ff1.amB_Id) {
                                                    branch = ff1.amB_BranchName;
                                                }

                                            })
                                            angular.forEach($scope.fillsemester, function (ff2) {
                                                if (rr.AMSE_Id == ff2.amsE_Id) {
                                                    sem = ff2.amsE_SEMName;
                                                }
                                            })
                                        }

                                    })


                                    if (tottal > 0) {
                                        $scope.words = $scope.amountinwords(tottal);
                                        $scope.tempmainarray.push({ AMCST_Id: dd.amcsT_Id, StudentName: StudentName, AMCST_RegistrationNo: AMCST_RegistrationNo, AMCST_AdmNo: AMCST_AdmNo, FYP_DOE: FYP_DOE, stdheadlst: $scope.headarray, total: tottal, words: $scope.words, AMCST_FatherName: AMCST_FatherName, AMCO_CourseName: course, AMB_BranchName: branch, AMSE_SEMName: sem, mode: mode })
                                    }


                                }
                                )

                                console.log($scope.tempmainarray)
                                for (var i = 0; i < $scope.tempmainarray.length; i++) {
                                    if ((i + 2) % 2 == 0) {
                                        $scope.fiestreciept.push($scope.tempmainarray[i]);
                                    }
                                    else {
                                        $scope.secondreciept.push($scope.tempmainarray[i]);
                                    }
                                }



                            }
                            else {
                                swal("No Records found.Kindly map Group with heads!!")
                                $scope.grigview1 = false;
                            }
                        })

                }
                else {
                    swal("You are missing mandatory parameter to select")
                }
            } else {
                $scope.submitted = true;
            }
        };

        $scope.printData = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }


    }

})();
