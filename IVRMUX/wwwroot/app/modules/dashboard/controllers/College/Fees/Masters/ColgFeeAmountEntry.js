(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGFeeAmountEntryController', CLGFeeAmountEntryController)
    CLGFeeAmountEntryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache']
    function CLGFeeAmountEntryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        var headid;
        var ftiid;
        var semid;
        $scope.finslab1 = [];
        $scope.finslab2 = [];


        $scope.fineslb = [{ value: "Per Day", FTFS_FineType: "Per Day" },
        { value: "Day Slab", FTFS_FineType: "Day Slab" }
        ];


        $scope.fineslbecs = [{ value: "Per Day", FTFSE_FineType: "Per Day" },
        { value: "Day Slab", FTFSE_FineType: "Day Slab" }
        ];

        $scope.fineslbperamount = [{ value: "Amount", FCTFS_PercentageFlg: "Amount" },
        { value: "Percent", FCTFS_PercentageFlg: "Percent" }
        ];

        $scope.typeofstudentregular = false;

        $scope.sortKey = "fmA_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa
        $scope.searchthird = "";
        $scope.ecsmonthdisable = false;
        $scope.ecsdatedisable = false;
        $scope.commonamountflag = [];

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        if (configsettings.length > 0) {
            var ecsflag = configsettings[0].ispaC_ECSFlag;
        }


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
            apiService.create("CLGFeeAmountEntry/getalldetailsOnselectiontype", data).
                then(function (promise) {
                    $scope.thirdgrid = promise.alldata;
                    $scope.totcountfirst = $scope.thirdgrid.length;
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

        $scope.hrdallcheck = function () {
            var toggleStatus1 = $scope.checkallhrd;
            angular.forEach($scope.fillsemester, function (itm) {
                itm.selected = toggleStatus1;
            });

        }

        $scope.hrgsinglecheck = function () {

            $scope.checkallhrd = $scope.fillsemester.every(function (itm) {

                return itm.selected;
            });


        };


        var ASMAY_Year;
        $scope.formload = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            apiService.getURI("CLGFeeAmountEntry/getalldetails", pageid).
                then(function (promise) {

                    $scope.yearlst = promise.academicdrp;
                    $scope.ASMAY_Id = promise.currfillyear[0].asmaY_Id;
                    $scope.Fillcourse = promise.fillcourse;
                    $scope.groupcount = promise.fillgroup;
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

            apiService.create("CLGFeeAmountEntry/selectacademicyear", data).
                then(function (promise) {
                    //$scope.Fillcourse = promise.Fillcourse;
                    $scope.Fillcourse = promise.fillcourseyearwise;
                    $scope.groupcount = promise.fillgroup;

                })
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

            apiService.create("CLGFeeAmountEntry/selectcourse", data).
                then(function (promise) {


                    if (promise.fillbranch.length > 0) {
                        $scope.Branch = promise.fillbranch;
                    }
                    else {
                        swal("No Records found.!!")
                    }
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

            apiService.create("CLGFeeAmountEntry/selectbran", data).
                then(function (promise) {

                    if (promise.fillsemester.length > 0) {

                        $scope.fillsemester = promise.fillsemester;
                    }
                    else {
                        swal("No Records found.!!")
                    }
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
                        apiService.create("CLGFeeAmountEntry/Deletedetails/", data).
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
                "ASMAY_Id": $scope.ASMAY_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeAmountEntry/fillslabdetails", data).
                then(function (promise) {

                    $scope.finslab1 = promise.fillslab
                })

        }

        $scope.EditMasterSectvalue = function (employee) {
            $scope.editEmployee = employee.fyghM_Id;
            var orgid = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("CLGFeeAmountEntry/Editdetails", orgid).
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

            apiService.create("CLGFeeAmountEntry/1", data).
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
            //var doob = $scope.VPAYVOU_ChequePrintedDate == null ? "" : $filter('date')($scope.VPAYVOU_ChequePrintedDate, "yyyy-MM-dd");

            $scope.submitted = true;
            $scope.totalgrid = [];
            angular.forEach($scope.plannerid, function (y) {
                angular.forEach(y.feedetails, function (yy) {
                    if (y.amsE_Id == yy.amsE_Id) {
                        var arr = [];
                        var duedate = $filter('date')(yy.fcmaS_DueDate, "yyyy-MM-dd");

                        var arr = duedate.split('-');


                        $scope.totalgrid.push(
                            {

                                AMSE_SEMName: yy.amsE_SEMName,
                                FMH_FeeName: yy.fmH_FeeName,
                                FCMAS_Amount: yy.fcmaS_Amount,
                                FCMAS_Id: yy.fcmaS_Id,
                                FMH_Id: yy.fmH_Id,
                                FMI_Id: yy.FMI_Id,
                                FCMA_Id: yy.fcmA_Id,
                                //FMH_FeeName: yy.FMH_FeeName,
                                FMI_Name: yy.fmI_Name,
                                FTI_Id: yy.ftI_Id,
                                FCTDD_Month: arr[1],
                                FCTDD_Day: arr[2],
                                FCTDD_Year: arr[0],
                                AMSE_Id: yy.amsE_Id,
                                FCMAS_DueDate: new Date(yy.fcmaS_DueDate).toDateString(),
                                //  AMCO_Id: yy.AMCO_Id,


                                FYGMM_FineApplicableFlag: yy.fyghM_FineApplicableFlag
                            });
                    }

                });
            });
            if ($scope.myform.$valid) {

                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMB_Id": $scope.AMB_Id,
                    // "AMSE_Id": $scope.AMSE_Id,
                    savetmpdata: $scope.totalgrid,
                    savefineslabreg: $scope.savedatatrans1,
                    savefineslabecs: $scope.savedatatrans2,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAmountEntry/savedata", data).
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
                semid = totary[index].amsE_Id;

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
            semid = totalgrid[index].amsE_Id;
            if ($scope.stuchk == "stfoth" && $scope.ASMAY_Id != "") {
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "selectiontype": $scope.stuchk,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": semid,
                    // "AMSE_Id": $scope.AMSE_Id,
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
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": semid,
                    //  "AMSE_Id": $scope.AMSE_Id,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
            }
            apiService.create("CLGFeeAmountEntry/getgroupmappedheads/", data).
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
                            if (promise.fineslabdetails[i].fmfS_ECSFlag == "R" && promise.fineslabdetails[i].fmH_Id == headid && promise.fineslabdetails[i].ftI_Id == ftiid && promise.fineslabdetails[i].amsE_Id == semid) {
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
                $scope.savedatatrans1[i].AMSE_Id = semid;
            }


            var lenofarrayecs = $scope.savedatatrans2.length;
            //lenofarray =Number(lenofarray) -Number(countofanarray);

            for (var i = countofanarrayecs; i < lenofarrayecs; i++) {
                $scope.savedatatrans2[i].FMH_Id = headid;
                $scope.savedatatrans2[i].FTI_ID = ftiid;
                $scope.savedatatrans2[i].AMSE_Id = semid;
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



        $scope.FMCC_Id = "";
        $scope.FMG_Id = "";
        $scope.regslbary = [];
        $scope.ecsslbary = [];
        $scope.optionToggled = function () {

            //$scope.monthecscnt = [];
            //$scope.grigview1 = false;
            //$scope.regslbary = [];;
            //$scope.ecsslbary = [];
            //$scope.totalgrid = [];
            //  $scope.submitted = true;

            //if ($scope.myform.$valid) {
            if ($scope.FMG_Id != "" && $scope.AMCO_Id != "" && $scope.ASMAY_Id != "" && $scope.AMB_Id != "" && $scope.AMSE_Id != "") {

                $scope.slab1 = true;
                $scope.slab2 = true;
                //var semid = [];
                //angular.forEach($scope.fillsemester, function (crs) {
                //    if (crs.selected) {

                //        semid.push(crs.amsE_Id);
                //    }
                //})
                var data = {
                    "FMG_Id": $scope.FMG_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMB_Id": $scope.AMB_Id,
                     AMSE_Ids: semid
                    //"AMSE_Id": $scope.AMSE_Id,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("CLGFeeAmountEntry/getgroupmappedheads", data).
                    then(function (promise) {

                        var temp = [];
                        var s1 = "";
                        var s2 = "";
                        angular.forEach(promise.fillallyear, function (mm) {
                            //$scope.array.push(value.asmaY_Year.split('-')[0]);
                            var year1 = mm.asmaY_Year;
                            s1 = year1.substring(0, 4);
                            s2 = year1.substring(year1.length, 5);
                            temp.push({ fctdD_Year: s1 });
                            $scope.years = temp;
                        });

                        var amountentryarray = [];
                        //added Praveen
                        $scope.monthecscnt = promise.fillmonth;
                        console.log($scope.monthecscnt)
                        //end praveen
                        ASMAY_Year = promise.asmaY_Year;
                        if (promise.allgroupheaddata.length > 0) {

                            $scope.monthcnt = promise.fillmonth;
                            console.log($scope.monthcnt);

                            angular.forEach(promise.allgroupheaddata, function (obj) {
                                amountentryarray.push(obj);
                            })

                            angular.forEach(promise.newlyupdatedrec, function (obj1) {
                                var already_cnt = 0;
                                angular.forEach(promise.allgroupheaddata, function (obj) {
                                    if (obj1.fmH_Id == obj.fmH_Id) {
                                        already_cnt += 1;
                                    }
                                })
                                if (already_cnt == 0) {
                                    amountentryarray.push(obj1);
                                }
                            })

                            promise.allgroupheaddata = amountentryarray;
                            //}


                            $scope.grigview1 = true;
                            $scope.plannerid = [];
                            $scope.regslbary = promise.fineslabdetails;

                            $scope.get_approvalreport = promise.allgroupheaddata;
                            angular.forEach($scope.get_approvalreport, function (dev) {
                                if ($scope.plannerid.length === 0) {

                                    $scope.plannerid.push(dev);
                                } else if ($scope.plannerid.length > 0) {
                                    var intcount = 0;
                                    angular.forEach($scope.plannerid, function (emp) {
                                        if (emp.amsE_Id === dev.amsE_Id) {
                                            intcount += 1;
                                            emp.showdetails = true;
                                        }
                                    });
                                    if (intcount === 0) {
                                        $scope.plannerid.push(dev);
                                    }
                                }
                            });

                            angular.forEach($scope.plannerid, function (ddd) {
                                $scope.templist = [];
                                var result = 0;
                                angular.forEach($scope.get_approvalreport, function (dd) {
                                    if (dd.amsE_Id === ddd.amsE_Id) {

                                        if ($scope.regslbary.length > 0) {
                                            angular.forEach($scope.regslbary, function (rgobj) {

                                                if (rgobj.fmH_Id == dd.fmH_Id && rgobj.ftI_Id == dd.ftI_Id) {
                                                    result = 1;
                                                    dd.details = true;
                                                    dd.showdetails = true;
                                                }

                                            })
                                        }
                                        if ($scope.ecsslbary.length > 0) {
                                            angular.forEach($scope.ecsslbary, function (ecsobj) {

                                                if (ecsobj.fmH_Id == dd.fmH_Id && ecsobj.ftI_Id == dd.ftI_Id) {
                                                    result = 1;
                                                    dd.details = true;
                                                    dd.showdetails = true;
                                                }

                                            })
                                        }

                                        if (result == "1") {
                                            $scope.templist.push(
                                                {
                                                    details: dd.details,
                                                    showdetails: dd.details,
                                                    amsE_SEMName: dd.amsE_SEMName,
                                                    fmH_FeeName: dd.fmH_FeeName,
                                                    fmH_Id: dd.fmH_Id,
                                                    fmI_Name: dd.fmI_Name,
                                                    ftI_Id: dd.ftI_Id,
                                                    fcmaS_Id: dd.fcmaS_Id,
                                                    fcmaS_Amount: dd.fcmaS_Amount,
                                                    fcmaS_DueDate: new Date(dd.fcmaS_DueDate),

                                                    fmI_Id: dd.fmI_Id,


                                                    fmA_Id: dd.fmA_Id,
                                                    amsE_Id: dd.amsE_Id,


                                                    fyghM_FineApplicableFlag: dd.fyghM_FineApplicableFlag,
                                                    fcmA_Id: dd.fcmA_Id,
                                                });
                                        }

                                        else {
                                            $scope.templist.push(
                                                {
                                                    amsE_SEMName: dd.amsE_SEMName,
                                                    fmH_FeeName: dd.fmH_FeeName,
                                                    fmH_Id: dd.fmH_Id,
                                                    fmI_Name: dd.fmI_Name,
                                                    ftI_Id: dd.ftI_Id,
                                                    fcmaS_Id: dd.fcmaS_Id,
                                                    fcmaS_Amount: dd.fcmaS_Amount,
                                                    fcmaS_DueDate: new Date(dd.fcmaS_DueDate),

                                                    fmI_Id: dd.fmI_Id,


                                                    fmA_Id: dd.fmA_Id,
                                                    amsE_Id: dd.amsE_Id,


                                                    fyghM_FineApplicableFlag: dd.fyghM_FineApplicableFlag,
                                                    fcmA_Id: dd.fcmA_Id,
                                                });
                                        }

                                    }
                                });
                                ddd.feedetails = $scope.templist;

                            });







                            angular.forEach($scope.totalgrid, function (obj) {
                                obj.ftddE_Day = parseInt(obj.ftddE_Day);
                            })
                            //console.log($scope.totalgrid);
                            $scope.commonamountflag = promise.commountamountflag;

                            $scope.ispaC_ECSFlag = ecsflag;
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
            //} else {
            //    $scope.submitted = true;
            //}
        };
    }

})();