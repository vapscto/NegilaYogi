(function () {
    'use strict';
    angular
.module('app')
.controller('CLGFeeRefundableController', CLGFeeRefundableController)

    CLGFeeRefundableController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function CLGFeeRefundableController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.filterdata1 = "Refunable";
        $scope.sortKey = 'fcR_Id';
        $scope.reverse = true;
        $scope.totcountsearch = 0;
        $scope.REF_CheqDate = new Date();
        $scope.REF_DATE = new Date();
        $scope.loaddata = function () {
            debugger;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
              var pageid = 2;
          
            
            apiService.getURI("CLGFeeRefundable/getalldetails", pageid).
            then(function (promise) {
                debugger;
                $scope.yearlst = promise.fillyear;
                $scope.currentYear = promise.currentYear;
                for (var i = 0; i < $scope.yearlst.length; i++) {
                    name = $scope.yearlst[i].asmaY_Id;
                    for (var j = 0; j < $scope.currentYear.length; j++) {
                        if (name == $scope.currentYear[j].asmaY_Id) {
                            $scope.yearlst[i].Selected = true;
                            $scope.ASMAY_Id = $scope.currentYear[j].asmaY_Id;
                        }
                    }
                }
                if (promise.imN_AutoManualFlag == "Auto")
                {
                    $scope.REF_REC_No = promise.fR_RefundNo;
                }
                $scope.OnChangeAcademicYearns();
                $scope.course_list = promise.courselist;
                $scope.branch_list = promise.branchlist;
                $scope.student_list = promise.studentlist;
                $scope.grouplst = promise.fillgroup;
                //if (promise.count > 0)
                //{
                    $scope.thirdgrid = promise.fillthirdgriddata;                  
                    $scope.totcountfirst = promise.fillthirdgriddata.length;
                //}
                //else {
                //    swal("No FEE REFUNDABLE DETAIL Records Found.....!!");
                //}
            
                $scope.gridview1 = false;


                $scope.REF_DATE = new Date();
                $scope.REF_CheqDate = new Date();

            })

            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        };
        $scope.sort1 = function (keyname1) {
            $scope.sortKey1 = keyname1;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }
       // $scope.REF_DATE = new Date();
        //$scope.fromDate = $scope.REF_DATE.getFullYear();
        //$scope.frommon = "";
        //$scope.fromDay = "";
        //$scope.minDatef = new Date(
        //             $scope.fromDate,
        //              $scope.frommon,
        //               $scope.fromDay + 1);

        //$scope.maxDatef = new Date(
        //      $scope.fromDate,
        //       $scope.frommon,
        //        $scope.fromDay + 365);


        $scope.onselectacademic = function (yearlst) {
            $scope.AMCST_Id = "";
            var academicyearid = $scope.ASMAY_Id;
            apiService.getURI("CLGFeeRefundable/getacademicyear", academicyearid).
       then(function (promise) {
           $scope.student_list = promise.studentlist;
       })
        };

        $scope.onselectclass = function (classlst) {
            $scope.AMCST_Id = "";
            var studid = $scope.AMCST_Id;
            apiService.getURI("CLGFeeRefundable/getclasswisestudentlst", studid).
       then(function (promise) {
           $scope.student_list = promise.studentlist;
       })
        };

        $scope.OnChangeAcademicYearns = function () {
            debugger;
           
            var id = $scope.ASMAY_Id
            apiService.getURI("CLGFeeRefundable/GetStudentListByYear", id).
          then(function (promise) {
      
              debugger;
              $scope.course_list = promise.courselist;
            //if (promise.courselist.length > 0) {
            //    $scope.course_list = promise.courselist;
            //}
            //else
                if (promise.courselist.length == 0) {
                swal("No Courses Are Mapped");
            }           
        })
        }


        $scope.GetSection = function () {
            debugger;
            $scope.AMCST_Id = "";
            var data = {
               
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
            }
            apiService.create("CLGFeeRefundable/GetSection", data).
        then(function (promise) {
            debugger;

            if (promise.branchlist.length > 0) {
              
                $scope.branch_list = promise.branchlist;
            }
            else {
                $scope.branch_list = [];
                swal('No records found to display');              
            }
        })
        }
        $scope.get_semisters = function () {
            debugger;
            $scope.AMCST_Id = "";
            var data = {

                "ASMAY_Id": $scope.ASMAY_Id,
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
        $scope.GetStudent = function () {
            debugger;
            $scope.AMCST_Id = "";
            var data = {

                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id

                //"ISMS_Id": $scope.ISMS_Id,
                //"ASASB_BatchName": $scope.ASASB_BatchName,
            }
            apiService.create("CLGFeeRefundable/GetStudent", data).
        then(function (promise) {
            debugger;

            if (promise.studentlist.length > 0) {

                $scope.student_list = promise.studentlist;
            }
            else {
                $scope.student_list = {};
                swal('No records found to display');

            }
        })
        }

        $scope.GetStudentListByamst = function () {
            var data = {

                "AMCST_Id": $scope.AMCST_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "filterrefund": $scope.filterdata1,

                //"ISMS_Id": $scope.ISMS_Id,
                //"ASASB_BatchName": $scope.ASASB_BatchName,
            }
            apiService.create("CLGFeeRefundable/GetStudentListByamst", data).
        then(function (promise) {
            debugger;
            $scope.studentviewdetails = [];
            if (promise.showstaticticsdetails.length > 0) {
                $scope.studentviewdetails = promise.showstaticticsdetails;
            }
            else {
                $scope.studentviewdetails.length = 0;
            }
            if (promise.fillgroup.length > 0) {

                $scope.grouplst = promise.fillgroup;
            }
            else {
                $scope.fillgroup = {};
                swal('No records found to display');
               // $scope.AMCST_Id = "";
            }
        })
        }


        $scope.optionToggledGF = function () {
            debugger;
            var groupidss;
            for (var i = 0; i < $scope.grouplst.length; i++) {
                debugger;
                if ($scope.grouplst[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.grouplst[i].fmG_Id;
                    else
                        groupidss = groupidss + "," + $scope.grouplst[i].fmG_Id;
                }


            }
            if (groupidss != undefined) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "filterrefund": $scope.filterdata1,
                    "multiplegroupF": groupidss
                }
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("CLGFeeRefundable/onselectgroup", data).
       then(function (promise) {
           if (promise.fillhead.length > 0)
               debugger;
           if (promise.fillhead.length > 0) {
               $scope.gridview1 = true;
               $scope.students = promise.fillhead;
               for (var i = 0; i < $scope.students.length; i++) {
                   //$scope.students[i].Amountall = Number($scope.students[i].fcsS_RunningExcessAmount) + (Number($scope.students[i].fcsS_RefundableAmount) - Number($scope.students[i].fsS_RefundAmount));
                   $scope.students[i].Amountall = Number($scope.students[i].fcsS_RunningExcessAmount) + (Number($scope.students[i].fcsS_RefundableAmount));
                   //  $scope.students[i].balamt = (Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount)) - Number($scope.students[i].fsS_RefundAmount);
                   // $scope.students[i].balamt = (Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount)) ;
                   $scope.students[i].balamt = Number($scope.students[i].fcsS_ToBePaid);
                   $scope.students[i].FCR_RefundAmount = Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount);
               }
           }
           else {
               $scope.students = {};
               swal("No Records Found.....!!");
           }
           //if (promise.count > 0)
           //{
           //    $scope.gridview1 = true;
           //    $scope.students = promise.fillhead;
           //    for(var i=0;i< $scope.students.length;i++)
           //    {
           //        $scope.students[i].balamt = Number($scope.students[i].fcsS_RunningExcessAmount);
           //        $scope.students[i].FCR_RefundAmount = $scope.students[i].fcsS_RunningExcessAmount;
           //    }
           //}
           //else {
           //    $scope.students = {};
           //    swal("No Records Found.....!!");
           //}
       })
        };
        $scope.isOptionsRequired = function () {
            return !$scope.students.some(function (options) {
                return options.Selected;
            });
        }
        $scope.isOptionsRequired_grp = function () {
            return !$scope.grouplst.some(function (options) {
                return options.selected;
            });
        }

        $scope.cleardata = function () {
            //$scope.AMCST_Id = "";
            //$scope.FMG_Id = "";
            //$scope.REF_DATE = "";
            //$scope.REF_REMARKS = "";
            //$scope.REF_BANK_CASH = "";
            //$scope.REF_CheqDate = "";
            //$scope.REF_CheqNo = "";
            //$scope.REF_REC_No = "";
            //$scope.REF_BANK_NAME = "";
            //$scope.L_Code = "";
            //$scope.AMCO_Id = "";
            //$scope.ASMAY_Id = "";
            //$scope.gridview1 = false;

            $state.reload();
        }


        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fcR_Id;
            var feechequebounceid = $scope.editEmployee;
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
                    apiService.DeleteURI("CLGFeeRefundable/Deletedetails", feechequebounceid).
                   then(function (promise) {
                       if (promise.returnVal == true)
                       {
                           swal("Record Deleted Successfully");
                           $state.reload();
                       }
                       else {
                           swal("Failed to Delete Record");
                       }
                         //  $scope.thirdgrid = promise.fillthirdgriddata;
                   })
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });


            //})
        }

        $scope.formbtns = false;
        $scope.onselectmodeofpayment = function () {

            var data = {
                "REF_BANK_CASH": $scope.REF_BANK_CASH,
                "filterinitialdata": $scope.filterdata
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("CLGFeeRefundable/modeofpayment", data).
       then(function (promise) {
         
           if ($scope.REF_BANK_CASH == 'C' || $scope.REF_BANK_CASH == 'B') {
               $scope.formbtns = true;
               $scope.accountlst = promise.fillacclst;
           }
         
       })
        };


        $scope.reF_AMOUNT = true;
        $scope.balanceamt = function (students, index) {
            debugger;
            //if (totalgrid[index].checkedgrid == true) {
            //    $scope.Bal_AMOUNT = Number(students[index].REF_AMOUNT) + Number(students[index].enteredamt)
            //}
            //else
            if (Number(students.FCR_RefundAmount) > (Number(students.fcsS_RunningExcessAmount) + Number(students.fcsS_RefundableAmount))) {
                swal("Refund Amount Should not be greater than Excess Amount");
                $scope.students[index].FCR_RefundAmount = Number($scope.students[index].fcsS_RunningExcessAmount) + Number($scope.students[index].fcsS_RefundableAmount);
            }
            else {
              //  $scope.students[index].balamt = (Number(students.fcsS_RunningExcessAmount) + Number(students.fcsS_RefundableAmount));
                $scope.students[index].balamt = Number($scope.students[index].fcsS_ToBePaid);
                // $scope.students[index].Amountall = Number(students.fcsS_RunningExcessAmount) + (Number(students.fcsS_RefundableAmount));
                // $scope.students[index].balamt = (Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount)) - Number($scope.students[i].fsS_RefundAmount);
                // $scope.students[index].FCR_RefundAmount = Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount);
            }
        }
       
        $scope.edit = function (employee) {
            debugger;
            $scope.FCR_Id = employee.fcR_Id;
            $scope.disablerec = false;
            $scope.disableflag = false;
           

            apiService.getURI("CLGFeeRefundable/editdetails", $scope.FCR_Id).
            then(function (promise) {

                $scope.gridview1 = true;
                $scope.formbtns = true;
                $scope.ASMAY_Id = promise.fillthirdgriddata[0].asmaY_ID;
                $scope.AMCO_Id = promise.amcO_Id;
                $scope.AMCST_Id = promise.fillthirdgriddata[0].amcsT_Id;
                $scope.FMG_Id = promise.fillthirdgriddata[0].fmG_Id;
                $scope.REF_REC_No = promise.fillthirdgriddata[0].fR_RefundNo;
                $scope.REF_DATE = new Date(promise.fillthirdgriddata[0].fR_Date);
                $scope.REF_BANK_CASH = promise.fillthirdgriddata[0].fR_BANK_CASH;
                $scope.REF_REMARKS = promise.fillthirdgriddata[0].fR_RefundRemarks;
                $scope.REF_CheqDate = new Date(promise.fillthirdgriddata[0].fR_CheqDate);
                $scope.bank.REF_CheqNo = promise.fillthirdgriddata[0].fR_CheqNo;
                $scope.students = promise.editFeeRefund;
                for (var i = 0; i < $scope.students.length; i++) {
                    $scope.students[i].Amountall = Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount);
                    // $scope.students[i].balamt = (Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount));
                    $scope.students[i].balamt = Number($scope.students[i].fcsS_ToBePaid);
                    $scope.students[i].FCR_RefundAmount = Number($scope.students[i].fcsS_RunningExcessAmount) + Number($scope.students[i].fcsS_RefundableAmount);

                    //$scope.students[i].FCR_RefundAmount = $scope.students[i].fcsS_RunningExcessAmount;
                    //$scope.students[i].balamt = Number($scope.students[i].fcsS_RunningExcessAmount);
                    $scope.students[i].fmH_FeeName = promise.fmH_FeeName;
                    if ($scope.students[i].fsS_RefundOverFlag == true) {
                        $scope.students[i].Selected1 = true;
                        $scope.disablerec = true;
                        $scope.disableflag = true;
                    }
                }
                //  $scope.FCR_RefundAmount = promise.fillthirdgriddata[0].reF_AMOUNT;
               // $scope.Bal_AMOUNT = true;
                //$scope.Bal_AMOUNT = Number(promise.fillthirdgriddata[0].reF_AMOUNT) - Number($scope.enteredamt)
               //  $scope.reF_AMOUNT = promise.fillthirdgriddata[0].reF_AMOUNT;
                $scope.bank.REF_BANK_NAME = promise.fillthirdgriddata[0].fR_BankName;
            })
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        $scope.submitted = false;
        $scope.selectedStudentList = [];
        $scope.selectedrefundorder = [];
        $scope.bank = {};
        $scope.savedata = function (students) {
            debugger;
            if ($scope.myForm.$valid) {
                $scope.selectedStudentList = [];
                if ($scope.students != "" && $scope.students != null) {
                    if ($scope.students.length > 0) {
                        for (var i = 0; i < $scope.students.length; i++) {
                            if ($scope.students[i].Selected == true) {
                                if ($scope.students[i].Selected1 == true)
                                {
                                    $scope.selectedrefundorder.push($scope.students[i]);
                                }
                                $scope.selectedStudentList.push($scope.students[i]);
                            }
                        }
                    }
                }
                debugger;
               
                var data = {
                    "FCR_Id":$scope.FCR_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "FCR_Date":new Date($scope.REF_DATE).toDateString(),
                    "FCR_RefundRemarks": $scope.REF_REMARKS,
                    "FCR_ModeOfPayment": $scope.REF_BANK_CASH,
                    "FCR_ChequeDDDate": new Date($scope.REF_CheqDate).toDateString(),
                    "FCR_ChequeDDNo": $scope.bank.REF_CheqNo,
                    "FCR_RefundNo": $scope.REF_REC_No,
                    "FCR_Bank": $scope.bank.REF_BANK_NAME,
                    "filterrefund": $scope.filterdata1,
                    savetmpdata: $scope.selectedStudentList,
                    selectedRefundOverList: $scope.selectedrefundorder,
                   
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                if ($scope.fswO_Id > 0) {
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

        apiService.create("CLGFeeRefundable/", data).
                then(function (promise) {
                    //if (promise.returnval == true) {
                    if (promise.returntxt == "Saved" || promise.returntxt == "Updated") {
                        swal('Record ' + promise.returntxt + ' Successfully', 'success');
                        $state.reload();
                        $scope.loaddata();
                    }
                    else {
                        if (promise.returntxt == "not Saved" || promise.returntxt == "not Updated") {
                            swal('Record ' + promise.returntxt + ' Successfully', 'Failed');
                        }
                        else {
                            swal(promise.returntxt);
                        }
                    }

                })
    }
    else {
        swal("Record saved Failed", "Failed");
    }


});






                

            }
            else
            {
                $scope.submitted = true;
            }
         
        };
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

                if ($scope.search123 == "2") {
                    $scope.txt = false;
                    $scope.numbr = true;
                    $scope.dat = false;

                }
                else
                if ($scope.search123 == "3") {

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
                if ($scope.search123 == "2") {
                    var data = {
                        "searchType": $scope.search123,
                        "searchnumber": $scope.searchnumbr
                    }
                }
                else if ($scope.search123 == "3") {
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

                apiService.create("CLGFeeRefundable/searching", data).
            then(function (promise) {
                $scope.thirdgrid = promise.fillthirdgriddata;
                $scope.totcountsearch = promise.fillthirdgriddata.length;
                if (promise.fillthirdgriddata == null || promise.fillthirdgriddata == "") {
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
            $scope.loaddata();
        }
        //search end
    }

})();